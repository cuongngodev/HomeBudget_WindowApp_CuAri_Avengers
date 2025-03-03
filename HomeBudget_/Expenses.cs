using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{
    // ====================================================================
    // CLASS: expenses
    //        - A collection of expense items,
    //        - Read / write to file
    //        - etc
    // ====================================================================
    /// <summary>
    /// Responsible for manageing a collection of <see cref="Expense"/> objects, providing different functionality for reading from and writing to files, and managing expense entries. Uses XML format for file operations.
    /// </summary>
    public class Expenses
    {
        private static String DefaultFileName = "budget.txt";
        private List<Expense> _Expenses = new List<Expense>();
        private string _FileName;
        private string _DirName;

        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// Gets the name of the current expense file.
        /// Read-only
        /// </summary>
        public String FileName { get { return _FileName; } }
        
        /// <summary>
        ///  Gets the directory name where contains expense file. Read - only
        /// </summary>
        public String DirName { get { return _DirName; } }

        // ====================================================================
        // populate categories from a file
        // if filepath is not specified, read/save in AppData file
        // Throws System.IO.FileNotFoundException if file does not exist
        // Throws System.Exception if cannot read the file correctly (parsing XML)
        // ====================================================================
        /// <summary>
        /// Reads expense data from a XML fille.
        /// </summary>
        /// <param name="filepath"> Path to the file to read (optional). If null, uses default file in the AppData </param>
        /// <exception cref="FileNotFoundException">Thrown when the file path does not exist</exception>
        /// <exception cref="ArgumentException">If there are problems parsing the file</exception>
        /// <exception cref="PathTooLongException">If there are problems parsing the file</exception>
        /// <notes>If filepath is not provided, uses default file name "</notes>
        /// <example>
        /// <code>
        /// Expenses expenses = new Expenses();
        /// expenses.ReadFromFile("./users/document/filepathtoread")
        /// </code>
        /// </example>
        public void ReadFromFile(String filepath = null)
        {
            // ---------------------------------------------------------------
            // reading from file resets all the current expenses,
            // so clear out any old definitions
            // ---------------------------------------------------------------
            _Expenses.Clear();

            // ---------------------------------------------------------------
            // reset default dir/filename to null 
            // ... filepath may not be valid, 
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if it doesn't exist)
            // ---------------------------------------------------------------
            filepath = BudgetFiles.VerifyReadFromFileName(filepath, DefaultFileName);

            // ---------------------------------------------------------------
            // read the expenses from the xml file
            // ---------------------------------------------------------------
            _ReadXMLFile(filepath);

            // ----------------------------------------------------------------
            // save filename info for later use?
            // ----------------------------------------------------------------
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
        }

        // ====================================================================
        // save to a file
        // if filepath is not specified, read/save in AppData file
        // ====================================================================
        /// <summary>
        /// Saves the current expense data to an XML file. 
        /// Creates a new XML file if it doesn't exist
        /// Use default filename if filepath is null.
        /// </summary>
        /// <param name="filepath">Path where the file is saved (optional). If null, uses last read file location </param>
        /// <exception cref="Exception">Thrown when the file path does not exist.</exception>
        /// <exception cref="ArgumentException"> Thrown if something is wrong with the filepath that fails to get directory Name, to get absolute path from specified filepath or to get filename, or path does not include file extension</exception>
        /// <exception cref="PathTooLongException"> Thrown if path is too long. </exception>
        /// <example>
        /// <code>
        /// Expenses expenses = new Expenses();
        /// expenses.SaveToFile("./FilePath/To/Save")
        /// </code>
        /// </example>
        public void SaveToFile(String filepath = null)
        {
            // ---------------------------------------------------------------
            // if file path not specified, set to last read file
            // ---------------------------------------------------------------
            if (filepath == null && DirName != null && FileName != null)
            {
                filepath = DirName + "\\" + FileName;
            }

            // ---------------------------------------------------------------
            // just in case filepath doesn't exist, reset path info
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if it doesn't exist)
            // ---------------------------------------------------------------
            filepath = BudgetFiles.VerifyWriteToFileName(filepath, DefaultFileName);

            // ---------------------------------------------------------------
            // save as XML
            // ---------------------------------------------------------------
            _WriteXMLFile(filepath);

            // ----------------------------------------------------------------
            // save filename info for later use
            // ----------------------------------------------------------------
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
        }



        // ====================================================================
        // Add expense
        // ====================================================================
        private void Add(Expense exp)
        {
            _Expenses.Add(exp);
        }
        
        /// <summary>
        /// Adds a new expense to the collection. Generated a unique id automatically.
        /// </summary>
        /// <param name="date"> Date of the expense</param>
        /// <param name="category"> Category Id tge expense </param>
        /// <param name="amount">Amount of earning of the expense </param>
        /// <param name="description">Description of the expense</param>
        /// <example>
        /// Enxpenses expenses = new Expenses();
        /// expenses.Add(date,category,amount,description)
        /// </example>
        public void Add(DateTime date, int category, Double amount, String description)
        {
            int new_id = 1;

            // if we already have expenses, set ID to max
            if (_Expenses.Count > 0)
            {
                new_id = (from e in _Expenses select e.Id).Max();
                new_id++;
            }

            _Expenses.Add(new Expense(new_id, date, category, amount, description));

        }

        // ====================================================================
        // Delete expense
        // ====================================================================
        /// <summary>Removes an expense from the collection
        /// </summary>
        /// <param name="Id">Id of the expense to delete</param>
        /// <exception cref="ArgumentNullException"> Thrown if id is null. </exception>
        /// <example>
        /// <code>
        /// Enxpenses expenses = new Expenses();
        /// expenses.Delete()
        /// </code>
        /// </example> 
        public void Delete(int Id)
        {
            try
            {
                int i = _Expenses.FindIndex(x => x.Id == Id);
                _Expenses.RemoveAt(i);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: Invalid expense id given.");
            }
     

        }

        // ====================================================================
        // Return list of expenses
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================
        /// <summary>
        /// Returns a copy of all the expenses in the collection.
        /// </summary>
        /// <returns>A new list containning copies of all expenses</returns>
        public List<Expense> List()
        {
            List<Expense> newList = new List<Expense>();
            foreach (Expense expense in _Expenses)
            {
                newList.Add(new Expense(expense));
            }
            return newList;
        }


        // ====================================================================
        // read from an XML file and add categories to our categories list
        // ====================================================================
        private void _ReadXMLFile(String filepath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);

                // Loop over each Expense
                foreach (XmlNode expense in doc.DocumentElement.ChildNodes)
                {
                    // set default expense parameters
                    int id = int.Parse((((XmlElement)expense).GetAttributeNode("ID")).InnerText);
                    String description = "";
                    DateTime date = DateTime.Parse("2000-01-01");
                    int category = 0;
                    Double amount = 0.0;

                    // get expense parameters
                    foreach (XmlNode info in expense.ChildNodes)
                    {
                        switch (info.Name)
                        {
                            case "Date":
                                date = DateTime.Parse(info.InnerText);
                                break;
                            case "Amount":
                                amount = Double.Parse(info.InnerText);
                                break;
                            case "Description":
                                description = info.InnerText;
                                break;
                            case "Category":
                                category = int.Parse(info.InnerText);
                                break;
                        }
                    }

                    // have all info for expense, so create new one
                    this.Add(new Expense(id, date, category, amount, description));

                }

            }
            catch (Exception e)
            {
                throw new Exception("ReadFromFileException: Reading XML " + e.Message);
            }
        }


        // ====================================================================
        // write to an XML file
        // if filepath is not specified, read/save in AppData file
        // ====================================================================
        private void _WriteXMLFile(String filepath)
        {
            // ---------------------------------------------------------------
            // loop over all categories and write them out as XML
            // ---------------------------------------------------------------
            try
            {
                // create top level element of expenses
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Expenses></Expenses>");

                // foreach Category, create an new xml element
                foreach (Expense exp in _Expenses)
                {
                    // main element 'Expense' with attribute ID
                    XmlElement ele = doc.CreateElement("Expense");
                    XmlAttribute attr = doc.CreateAttribute("ID");
                    attr.Value = exp.Id.ToString();
                    ele.SetAttributeNode(attr);
                    doc.DocumentElement.AppendChild(ele);

                    // child attributes (date, description, amount, category)
                    XmlElement d = doc.CreateElement("Date");
                    XmlText dText = doc.CreateTextNode(exp.Date.ToString("yyyy-MM-dd"));
                    ele.AppendChild(d);
                    d.AppendChild(dText);

                    XmlElement de = doc.CreateElement("Description");
                    XmlText deText = doc.CreateTextNode(exp.Description);
                    ele.AppendChild(de);
                    de.AppendChild(deText);

                    XmlElement a = doc.CreateElement("Amount");
                    XmlText aText = doc.CreateTextNode(exp.Amount.ToString());
                    ele.AppendChild(a);
                    a.AppendChild(aText);

                    XmlElement c = doc.CreateElement("Category");
                    XmlText cText = doc.CreateTextNode(exp.Category.ToString());
                    ele.AppendChild(c);
                    c.AppendChild(cText);

                }

                // write the xml to FilePath
                doc.Save(filepath);

            }
            catch (Exception e)
            {
                throw new Exception("SaveToFileException: Reading XML " + e.Message);
            }
        }

    }
}

