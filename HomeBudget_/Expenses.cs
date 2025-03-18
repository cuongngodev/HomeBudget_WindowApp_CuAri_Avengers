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
    /// Responsible for manageing a collection of <see cref="Expense"/> objects, providing different functionality for reading from and writing to files, and managing expense entries. Uses SQLite for file operations.
    /// </summary>
    public class Expenses
    {
        private SQLiteConnection _DbConnection;

        private SQLiteConnection DBConnection { get { return _DbConnection; } set { _DbConnection = value; } }

        /// <summary>
        /// Parameterized constructor that takes in a database connection path as input in order to link the Expenses object to the correct database.
        /// </summary>
        /// <param name="dbConnection">Database connection path to link up Expense object and database.</param>
        public Expenses(SQLiteConnection dbConnection)
        {
            DBConnection = dbConnection;
        }

        // ====================================================================
        // get a specific expense from the list where the id is the one specified
        // ====================================================================
        /// <summary>
        /// Gets an <see cref="Expense"/> object by a given ID number as input and retrieving its data from the database.
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

            string stm = "SELECT Id, Date, Description, Amount, CategoryId FROM expenses WHERE Id=@id";
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
        //private void Add(Expense exp)
        //{
        //    string stm = "INSERT INTO expenses(Id, Date, CategoryId, Amount, Description) VALUES(@id, @date, @categoryId, @amount, @description)";
        //    SQLiteCommand cmd = new SQLiteCommand(stm, DBConnection);

        //    cmd.CommandText = stm;

        //    cmd.Parameters.AddWithValue("@id", exp.Id);
        //    cmd.Parameters.AddWithValue("@date", exp.Date);
        //    cmd.Parameters.AddWithValue("@categoryId", exp.Category);
        //    cmd.Parameters.AddWithValue("@amount", exp.Amount);
        //    cmd.Parameters.AddWithValue("@description", exp.Description);

        //    cmd.Prepare();

        //    cmd.ExecuteNonQuery();
        //}

        /// <summary>
        /// Adds a new expense entry to the database and generates a unique id for it automatically.
        /// </summary>
        /// <param name="date">Creation date of the expense object.</param>
        /// <param name="category">Category Id of the expense object.</param>
        /// <param name="amount">Monetary amount assigned to the expense object.</param>
        /// <param name="description">Description of the expense object.</param>
        /// <example>
        /// Enxpenses expenses = new Expenses();
        /// expenses.Add(date,category,amount,description)
        /// </example>
        public void Add(DateTime date, int category, Double amount, String description)
        {
            string stm = "INSERT INTO expenses(Date, CategoryId, Amount, Description) VALUES(@date, @categoryId, @amount, @description)";
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
        /// <summary>Deletes an expense entry from the database.
        /// </summary>
        /// <param name="id">Id of the expense to delete.</param>
        /// <exception cref="ArgumentNullException"> Thrown if id is null. </exception>
        /// <example>
        /// <code>
        /// expenses.Delete(23)
        /// </code>
        /// </example> 
        public void Delete(int id)
        {
            string stm = "DELETE FROM expenses WHERE Id=@id";

            SQLiteCommand cmd = new SQLiteCommand(stm, DBConnection);

            cmd.CommandText = stm;

            cmd.Parameters.AddWithValue("@id", id);

            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        // ====================================================================
        // Return list of expenses
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================
        /// <summary>
        /// Returns a copy of all the expenses in the database in the form of an <see cref="Expense"/> list.
        /// </summary>
        /// <returns>A new list containning copies of all expenses</returns>
        public List<Expense> List()
        {
            const int ID_INDEX = 0;
            const int DATE_INDEX = 1;
            const int DESCRIPTION_INDEX = 2;
            const int AMOUNT_INDEX = 3;
            const int CATEGORY_ID_INDEX = 4;
            
            List<Expense> newList = new List<Expense>();

            string stm = "SELECT Id, Date, Description, Amount, CategoryId FROM expenses";
            SQLiteCommand cmd = new SQLiteCommand(stm, DBConnection);

            cmd.ExecuteNonQuery();

            SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int id = rdr.GetInt32(ID_INDEX);
                DateTime date = rdr.GetDateTime(DATE_INDEX);
                string description = rdr.GetString(DESCRIPTION_INDEX);
                double amount = rdr.GetDouble(AMOUNT_INDEX);
                int categoryId = rdr.GetInt32(CATEGORY_ID_INDEX);

                newList.Add(new Expense(id, date, categoryId, amount, description));
            }
            return newList;
        }
    }
}
