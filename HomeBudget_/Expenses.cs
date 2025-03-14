using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Data.SQLite;
using System.Data.SqlClient;
using Microsoft.VisualBasic;

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
        private List<Expense> _Expenses = new List<Expense>();

        private SQLiteConnection _DbConnection;

        public Expenses(SQLiteConnection dbConnection)
        {
            DBConnection = dbConnection;
     
        }
        /// <summary>
        /// Gets and sets the connection between the budget application and the necessary database to access information on all expenses.
        /// </summary>
        private SQLiteConnection DBConnection { get { return _DbConnection; } set { _DbConnection = value; } }

        // ====================================================================
        // get a specific expense from the list where the id is the one specified
        // ====================================================================
        /// <summary>
        /// Gets an <see cref="Expense"/> object by a given ID number as input.
        /// </summary>
        /// <param name="i">The unique ID number of an expense to retrieve.</param>
        /// <returns>An <see cref="Expense"/> object corresponding to its specified ID number.</returns>
        /// <example>
        /// <code>
        /// Expenses expenses = new Expenses();
        /// categories.GetExpenseFromId(1);
        /// </code>
        /// </example>
        public Expense GetExpenseFromId(int i)
        {
            const int ID_INDEX = 0;
            const int DATE_INDEX = 1;
            const int DESCRIPTION_INDEX = 2;
            const int AMOUNT_INDEX = 3;
            const int CATEGORY_ID_INDEX = 4;

            string stm = "SELECT Id, Date, Description, Amount, CategoryId FROM expenses WHERE id=@id";
            var cmd = new SQLiteCommand(stm, DBConnection);

            cmd.CommandText = stm;

            cmd.Parameters.AddWithValue("@id", i);
            cmd.Prepare();

            SQLiteDataReader rdr = cmd.ExecuteReader();

            int id = 0;
            DateTime date = DateTime.Now;
            string description = "";
            double amount = 0;
            int categoryId = 0;

            while (rdr.Read())
            {
                id = rdr.GetInt32(ID_INDEX);
                date = rdr.GetDateTime(DATE_INDEX);
                description = rdr.GetString(DESCRIPTION_INDEX);
                amount = rdr.GetDouble(AMOUNT_INDEX);
                categoryId = rdr.GetInt32(CATEGORY_ID_INDEX);
            }

            return new Expense(id, date, categoryId, amount, description);
        }

        // ====================================================================
        // Add expense
        // ====================================================================
        private void Add(Expense exp)
        {
            string stm = "INSERT INTO expenses(Id, Date, CategoryId, Amount, Description) VALUES(@id, @date, @categoryId, @amount, @description)";
            SQLiteCommand cmd = new SQLiteCommand(stm, DBConnection);

            cmd.CommandText = stm;

            cmd.Parameters.AddWithValue("@id", exp.Id);
            cmd.Parameters.AddWithValue("@date", exp.Date);
            cmd.Parameters.AddWithValue("@categoryId", exp.Category);
            cmd.Parameters.AddWithValue("@amount", exp.Amount);
            cmd.Parameters.AddWithValue("@description", exp.Description);

            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }
        
        /// <summary>
        /// Adds a new expense to the collection. Generates a unique id automatically.
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
            string stm = "INSERT INTO categories(Id, Date, CategoryId, Amount, Description) VALUES(@id, @date, @categoryId, @amount, @description)";
            SQLiteCommand cmd = new SQLiteCommand(stm, DBConnection);

            cmd.CommandText = stm;

            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@categoryId", category);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@description", description);

            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        // ====================================================================
        // Update Expense
        // ====================================================================
        public void UpdateProperties(int expenseId, DateTime newDate, int categoryId, double newAmount, string newDescription)
        {
            string stm = "UPDATE expenses SET Date = @date, CategoryId = @categoryId, Amount = @amount, Description = @description WHERE Id = @id";
            var cmd = new SQLiteCommand(stm, DBConnection);

            cmd.CommandText = stm;

            cmd.Parameters.AddWithValue("@id", expenseId);
            cmd.Parameters.AddWithValue("@date", newDate);
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);
            cmd.Parameters.AddWithValue("@amount", newAmount);
            cmd.Parameters.AddWithValue("@description", newDescription);
            cmd.Prepare();

            SQLiteDataReader rdr = cmd.ExecuteReader();
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
            string stm = "DELETE FROM expenses WHERE Id=@id";

            SQLiteCommand cmd = new SQLiteCommand(stm, DBConnection);

            cmd.CommandText = stm;

            cmd.Parameters.AddWithValue("@id", Id);

            cmd.Prepare();

            cmd.ExecuteNonQuery();
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
