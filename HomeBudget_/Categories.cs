using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Text.Json.Serialization;
using System.Runtime.Intrinsics.Arm;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

//TODO: 
//REMOVE ALL SELECT *'s
namespace Budget
{
    // ====================================================================
    // CLASS: categories
    //        - A collection of category items,
    //        - Read / write to file
    //        - etc
    // ====================================================================
    /// <summary>
    /// Responsible for manageing a collection of Category object, representing different budget categories. It allows for operations including adding, deleting, and listing categories.
    /// </summary>
    public class Categories
    {
        private static String DefaultFileName = "budgetCategories.txt";
        private List<Category> _Cats = new List<Category>();
        private string _FileName;
        private string _DirName;
        private SQLiteConnection _DbConnection;


        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// Gets the name of the file used for storing category data.
        /// </summary>
        public String FileName { get { return _FileName; } }
        /// <summary>
        /// Gets directory path where category file is stored.
        /// </summary>
        public String DirName { get { return _DirName; } }

        private SQLiteConnection DBConnection { get { return _DbConnection; } set { _DbConnection = value; } }


        // ====================================================================
        // Constructor
        // ====================================================================
        public Categories()
        {
            SetCategoriesToDefaults();
        }

        public Categories(SQLiteConnection dbConnection, bool isNewDb)
        {
            DBConnection = dbConnection;

            if (isNewDb)
            {
                AddCategoryTypes();
                SetCategoriesToDefaults(); 
            }
            

        }

        public void AddCategoryTypes()
        { 
            foreach (Category.CategoryType cat in Enum.GetValues<Category.CategoryType>())
            {
                string stm = "INSERT INTO categoryTypes(Description) VALUES(@description);";
                SQLiteCommand cmd = new(stm, DBConnection);
                //cmd.Parameters.AddWithValue("@id", (int)cat + 1);
                cmd.Parameters.AddWithValue("@description", cat.ToString());
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }

        // ====================================================================
        // get a specific category from the list where the id is the one specified
        // ====================================================================
        /// <summary>
        /// Gets a Category object by Id.
        /// </summary>
        /// <param name="i"> The ID of the category to retrieve.</param>
        /// <returns>A Category object corresponding to the specified ID.</returns>
        /// <exception cref="Exception">If the category with the specified ID does not exist.</exception>
        /// <example>
        /// <code>
        /// Categories categories = new Categories();
        /// categories.GetCategoryFromId(1);       
        /// </code>
        /// </example>
        public Category GetCategoryFromId(int i)
        {
            
            string stm = "SELECT * FROM categories WHERE id=@id";
            var cmd = new SQLiteCommand(stm,DBConnection);

            cmd.CommandText = stm;

            cmd.Parameters.AddWithValue("@id", i);
            cmd.Prepare();

            SQLiteDataReader rdr = cmd.ExecuteReader();

            int id = 0;
            string description = "";
            Category.CategoryType categoryType = Category.CategoryType.Income;
            while (rdr.Read())
            {
                id = rdr.GetInt32(0);
                description = rdr.GetString(1);
                categoryType = (Category.CategoryType)rdr.GetInt32(2);

            }

            return new Category(id,description,categoryType);

            //Category c = _Cats.Find(x => x.Id == i);
            //if (c == null)
            //{
            //    throw new Exception("Cannot find category with id " + i.ToString());
            //}
            //return c;
        }

        // ====================================================================
        // populate categories from a file
        // if filepath is not specified, read/save in AppData file
        // Throws System.IO.FileNotFoundException if file does not exist
        // Throws System.Exception if cannot read the file correctly (parsing XML)
        // ====================================================================
        /// <summary>
        /// Reads category data from an XML file. If filepath is null, it sets to the defaults to readding from the application data file.
        /// </summary>
        /// <param name="filepath">The path to the file to read from.</param>
        /// <example>
        /// <code>
        /// Categories categories = new Categories();
        /// categories.ReadFromFile("categories.xml");       
        /// </code>
        /// </example>


        public void ReadFromFile(String filepath = null)
        {

            // ---------------------------------------------------------------
            // reading from file resets all the current categories,
            // ---------------------------------------------------------------
            _Cats.Clear();

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
            // If file exists, read it
            // ---------------------------------------------------------------
            _ReadXMLFile(filepath);
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
        }



        // ====================================================================
        // save to a file
        // if filepath is not specified, read/save in AppData file
        // ====================================================================
        /// <summary>
        /// Saves the current categories list to a specified file with XML format. If file path fails to provided, it saves to the last read file.
        /// </summary>
        /// <param name="filepath">The path to the file where data is saved.</param>
        /// <exception cref="Exception">If filepath does not exist.</exception>
        /// <example>
        /// <code>
        /// Categories categories = new Categories();
        /// categories.SaveToFile("categories.xml");       
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
        // set categories to default
        // ====================================================================
        /// <summary>
        /// Resets the categories to a predefined set of default categories such as "Utilities", "Rent", "Education", etc.
        /// </summary>
        /// <example>
        /// <code>
        /// Categories categories = new Categories();
        /// categories.SetCategoriesToDefaults();        
        /// </code>
        /// </example>

        public void SetCategoriesToDefaults()
        {
            // ---------------------------------------------------------------
            // reset any current categories,
            // ---------------------------------------------------------------

            //_Cats.Clear();

           string stm = "DELETE FROM categories; VACUUM;";

            SQLiteCommand cmd = new SQLiteCommand(stm, DBConnection);
            cmd.ExecuteNonQuery();

            // ---------------------------------------------------------------
            // Add Defaults
            // ---------------------------------------------------------------
            Add("Utilities", Category.CategoryType.Expense);
            Add("Rent", Category.CategoryType.Expense);
            Add("Food", Category.CategoryType.Expense);
            Add("Entertainment", Category.CategoryType.Expense);
            Add("Education", Category.CategoryType.Expense);
            Add("Miscellaneous", Category.CategoryType.Expense);
            Add("Medical Expenses", Category.CategoryType.Expense);
            Add("Vacation", Category.CategoryType.Expense);
            Add("Credit Card", Category.CategoryType.Credit);
            Add("Clothes", Category.CategoryType.Expense);
            Add("Gifts", Category.CategoryType.Expense);
            Add("Insurance", Category.CategoryType.Expense);
            Add("Transportation", Category.CategoryType.Expense);
            Add("Eating Out", Category.CategoryType.Expense);
            Add("Savings", Category.CategoryType.Savings);
            Add("Income", Category.CategoryType.Income);

        }

        // ====================================================================
        // Add category
        // ====================================================================
        private void Add(Category cat)
        {
            string stm = "INSERT INTO categories(Id,Description,TypeId) VALUES(@id,@description,@type)";

            SQLiteCommand cmd = new SQLiteCommand(stm, DBConnection);

            cmd.CommandText = stm;

            cmd.Parameters.AddWithValue("@id", cat.Id);
            cmd.Parameters.AddWithValue("@description", cat.Description);
            cmd.Parameters.AddWithValue("@type", ((int) cat.Type) + 1);

            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }


        /// <summary>
        /// Adds a new category by specifying its description and type. The ID of the new category is automatically set based on the exisiting categories.
        /// </summary>
        /// <param name="desc">The description of the new category.</param>
        /// <param name="type">The type of the category (e.g: Income, Expense, Credit, or Savings). </param>
        /// <example>
        /// <code>
        /// Categories categories = new Categories();
        /// categories.Add("Transportation", Category.CategoryType.Expense);        
        /// </code>
        /// </example>
        public void Add(String desc, Category.CategoryType type)
        {
            string stm = "INSERT INTO categories(Description,TypeId) VALUES(@description,@type)";

            SQLiteCommand cmd = new SQLiteCommand(stm, DBConnection);

            cmd.CommandText = stm;

            //cmd.Parameters.AddWithValue("@id", cat.Id);
            int categoryType = (int)type + 1;

            cmd.Parameters.AddWithValue("@description", desc);
            cmd.Parameters.AddWithValue("@type", categoryType); //FIX THIS 

            cmd.Prepare();

            cmd.ExecuteNonQuery();

            //int new_num = 1;
            //if (_Cats.Count > 0)
            //{
            //    new_num = (from c in _Cats select c.Id).Max();
            //    new_num++;
            //}
            //_Cats.Add(new Category(new_num, desc, type));
        }

        // ====================================================================
        // Delete category
        // ====================================================================
        /// <summary>
        /// Deletes a category based on its ID.
        /// </summary>
        /// <param name="Id">The ID of the category to delete.</param>
        /// <example>
        /// <code>
        /// Categories categories = new Categories();
        /// categories.Delete(1);
        /// </code>
        /// </example>

        public void Delete(int Id)
        {

            string stm = "DELETE FROM categories WHERE id=@id";

            SQLiteCommand cmd = new SQLiteCommand(stm, DBConnection);

            cmd.CommandText = stm;

            cmd.Parameters.AddWithValue("@id", Id);

            cmd.Prepare();

            cmd.ExecuteNonQuery();

            //int i = _Cats.FindIndex(x => x.Id == Id);
            //_Cats.RemoveAt(i);
        }

        // ====================================================================
        // Return list of categories
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================
        /// <summary>
        /// Returns a copy of the list of categories. 
        /// </summary>
        /// <returns>A List of Category object containing all the categories.</returns>
        /// <example>
        /// <code>
        /// Categories categories = new Categories();
        /// List Category allCategories = categories.List();
        /// </code>
        /// </example>
        public List<Category> List()
        {
            List<Category> newList = new List<Category>();

            string stm = "SELECT * FROM categories";
            SQLiteCommand cmd = new SQLiteCommand(stm, DBConnection);

            cmd.ExecuteNonQuery();

            SQLiteDataReader rdr = cmd.ExecuteReader();

           
            while (rdr.Read())
            {
                //Console.WriteLine(rdr.GetInt32(1));
                int id = rdr.GetInt32(0);
                string description = rdr.GetString(1);
                Category.CategoryType categoryType = (Category.CategoryType)rdr.GetInt32(2);

                newList.Add(new Category(id, description, categoryType));
            }
            return newList;
           

            //foreach (Category category in _Cats)
            //{
            //    newList.Add(new Category(category));
            //}
            //return newList;
        }

        // ====================================================================
        // read from an XML file and add categories to our categories list
        // ====================================================================
        private void _ReadXMLFile(String filepath)
        {

            // ---------------------------------------------------------------
            // read the categories from the xml file, and add to this instance
            // ---------------------------------------------------------------
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);

                foreach (XmlNode category in doc.DocumentElement.ChildNodes)
                {
                    String id = (((XmlElement)category).GetAttributeNode("ID")).InnerText;
                    String typestring = (((XmlElement)category).GetAttributeNode("type")).InnerText;
                    String desc = ((XmlElement)category).InnerText;

                    Category.CategoryType type;
                    switch (typestring.ToLower())
                    {
                        case "income":
                            type = Category.CategoryType.Income;
                            break;
                        case "expense":
                            type = Category.CategoryType.Expense;
                            break;
                        case "credit":
                            type = Category.CategoryType.Credit;
                            break;
                        default:
                            type = Category.CategoryType.Expense;
                            break;
                    }
                    this.Add(new Category(int.Parse(id), desc, type));
                }

            }
            catch (Exception e)
            {
                throw new Exception("ReadXMLFile: Reading XML " + e.Message);
            }

        }


        // ====================================================================
        // write all categories in our list to XML file
        // ====================================================================
        private void _WriteXMLFile(String filepath)
        {
            try
            {
                // create top level element of categories
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Categories></Categories>");

                // foreach Category, create an new xml element
                foreach (Category cat in _Cats)
                {
                    XmlElement ele = doc.CreateElement("Category");
                    XmlAttribute attr = doc.CreateAttribute("ID");
                    attr.Value = cat.Id.ToString();
                    ele.SetAttributeNode(attr);
                    XmlAttribute type = doc.CreateAttribute("type");
                    type.Value = cat.Type.ToString();
                    ele.SetAttributeNode(type);

                    XmlText text = doc.CreateTextNode(cat.Description);
                    doc.DocumentElement.AppendChild(ele);
                    doc.DocumentElement.LastChild.AppendChild(text);

                }

                // write the xml to FilePath
                doc.Save(filepath);

            }
            catch (Exception e)
            {
                throw new Exception("_WriteXMLFile: Reading XML " + e.Message);
            }

        }

    }
}

