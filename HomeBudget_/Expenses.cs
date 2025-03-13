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


    }
}

