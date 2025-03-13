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

namespace Budget
{
    // ====================================================================
    // CLASS: categories
    //        - A collection of category items,
    //        - Read / write to file
    //        - etc
    // ====================================================================
    /// <summary>
    /// Responsible for managing a collection of categories stored on a database, representing different expense categories. 
    /// <br></br>It allows for operations including adding, deleting, and listing categories.
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
        /// Gets the path of the file used for reading and writing category data.
        /// </summary>
        public String FileName { get { return _FileName; } }
        
        /// <summary>
        /// Gets the directory path where the file corresponding to the <see cref="Categories.FileName"/> property is stored.
        /// </summary>
        public String DirName { get { return _DirName; } }

        /// <summary>
        /// Gets and sets the connection between the budget application and the necessary database to access information on expense categories.
        /// </summary>
        private SQLiteConnection DBConnection { get { return _DbConnection; } set { _DbConnection = value; } }

        // ====================================================================
        // Constructor
        // ====================================================================
        /// <summary>
        /// Default constructor which takes in no parameters and instead empties the category database and resets it too its default state.
        /// </summary>
        public Categories()
        {
            SetCategoriesToDefaults();
        }

        /// <summary>
        /// Parameterized constructor which creates a <see cref="Categories"/> object by taking in input corresponding to the
        /// <br></br>
        /// correct database connection and a boolean stating whether the database is new or not in the potential need to create a new one.
        /// </summary>
        /// <param name="dbConnection">The connection to a given database location.</param>
        /// <param name="isNewDb">An indicator to whether the desired database location is meant to be new or is pre-existing.</param>
        public Categories(SQLiteConnection dbConnection, bool isNewDb)
        {
            DBConnection = dbConnection;

            if (isNewDb)
            {
                AddCategoryTypes();
                SetCategoriesToDefaults(); 
            }
        }

        /// <summary>
        /// A method designed to add the necessary category types to the categoryTypes table on a new database.
        /// </summary>
        private void AddCategoryTypes()
        { 
            foreach (Category.CategoryType cat in Enum.GetValues<Category.CategoryType>())
            {
                string stm = "INSERT INTO categoryTypes(Description) VALUES(@description);";
                SQLiteCommand cmd = new(stm, DBConnection);
                cmd.Parameters.AddWithValue("@description", cat.ToString());
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }

        // ====================================================================
        // get a specific category from the list where the id is the one specified
        // ====================================================================
        /// <summary>
        /// Gets a <see cref="Category"/> object by a given ID number as input.
        /// </summary>
        /// <param name="i">The unique ID number of a category to retrieve.</param>
        /// <returns>A <see cref="Category"/> object corresponding to its specified ID number.</returns>
        /// <example>
        /// <code>
        /// Categories categories = new Categories();
        /// categories.GetCategoryFromId(1);
        /// </code>
        /// </example>
        public Category GetCategoryFromId(int i)
        {
            
            
            string stm = "SELECT Id, Description, TypeId FROM categories WHERE id=@id";
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
        }

        // ====================================================================
        // set categories to default
        // ====================================================================
        /// <summary>
        /// Resets the category table within the database to a predefined set of default categories such as "Utilities", "Rent", "Education", etc.
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
        /// <summary>
        /// Adds a new category to the database. The ID of the new category is automatically set based on the exisiting categories.
        /// </summary>
        /// <param name="cat">A <see cref="Category"/> object for the to-be added category to base itself on.</param>
        /// <example>
        /// <code>
        /// Categories categories = new Categories();
        /// categories.Add(vacationCategory);
        /// </code>
        /// </example>
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
        /// Adds a new category to the database by specifying its description and type. The ID of the new category is automatically set based on the exisiting categories.
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

            int categoryType = (int)type + 1;

            cmd.Parameters.AddWithValue("@description", desc);
            cmd.Parameters.AddWithValue("@type", categoryType);

            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        // ====================================================================
        // Delete category
        // ====================================================================
        /// <summary>
        /// Deletes a category from the database based on a given ID number.
        /// </summary>
        /// <param name="Id">The ID number of the category to delete.</param>
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
        }

        // ====================================================================
        // Return list of categories
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================
        /// <summary>
        /// Returns a copy of the list of categories from the database. 
        /// </summary>
        /// <returns>A list of <see cref="Category"/> objects containing all the categories found in the database.</returns>
        /// <example>
        /// <code>
        /// Categories categories = new Categories();
        /// List Category allCategories = categories.List();
        /// </code>
        /// </example>
        public List<Category> List()
        {
            List<Category> newList = new List<Category>();

            string stm = "SELECT Id, Description, TypeId FROM categories";
            SQLiteCommand cmd = new SQLiteCommand(stm, DBConnection);

            cmd.ExecuteNonQuery();

            SQLiteDataReader rdr = cmd.ExecuteReader();

           
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string description = rdr.GetString(1);
                Category.CategoryType categoryType = (Category.CategoryType)rdr.GetInt32(2);

                newList.Add(new Category(id, description, categoryType));
            }
            return newList;
        }

    }
}

