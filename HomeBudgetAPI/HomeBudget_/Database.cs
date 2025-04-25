using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Threading;

// ===================================================================
// Very important notes:
// ... To keep everything working smoothly, you should always
//     dispose of EVERY SQLiteCommand even if you recycle a 
//     SQLiteCommand variable later on.
//     EXAMPLE:
//            Database.newDatabase(GetSolutionDir() + "\\" + filename);
//            var cmd = new SQLiteCommand(Database.dbConnection);
//            cmd.CommandText = "INSERT INTO categoryTypes(Description) VALUES('Whatever')";
//            cmd.ExecuteNonQuery();
//            cmd.Dispose();
//
// ... also dispose of reader objects
//
// ... by default, SQLite does not impose Foreign Key Restraints
//     so to add these constraints, connect to SQLite something like this:
//            string cs = $"Data Source=abc.sqlite; Foreign Keys=1";
//            var con = new SQLiteConnection(cs);
//
// ===================================================================

namespace Budget
{
    /// <summary>
    /// Corresponds to a new database for the Budget application to access and manage expenses from.
    /// </summary>
    public class Database
    {
        public static SQLiteConnection dbConnection { get { return _connection; } }
        private static SQLiteConnection _connection;

        // ===================================================================
        // create and open a new database
        // ===================================================================
        /// <summary>
        /// Creates a new budget database at on a newly created file location, generating the necessary tables for the database itself.
        /// </summary>
        /// <param name="filename">The path, absolute or relative, leading to the location of the new database file.</param>
        public static void newDatabase(string filename)
        {
            try
            {
                // If there was a database open before, close it and release the lock
                CloseDatabaseAndReleaseFile();

                // your code
                // SQLiteConnection.CreateFile(filename);
                string cs = $"URI=file:{filename}";
                _connection = new SQLiteConnection(cs);

                dbConnection.Open();
                using var cmd = new SQLiteCommand(dbConnection);

                cmd.CommandText = @"CREATE TABLE categoryTypes(Id INTEGER  PRIMARY KEY, Description TEXT)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE categories(Id INTEGER  PRIMARY KEY, Description TEXT, TypeId int, FOREIGN KEY (TypeId) REFERENCES categoryTypes (Id));";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE expenses(Id INTEGER  PRIMARY KEY, Date TEXT, Description TEXT, Amount DOUBLE, CategoryId int, FOREIGN KEY (CategoryId) REFERENCES categories (Id));";
                cmd.ExecuteNonQuery();
                dbConnection.Close();

                existingDatabase(filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // ===================================================================
        // open an existing database
        // ===================================================================
        /// <summary>
        /// Connects to a pre-existing database on a given file location.
        /// </summary>
        /// <param name="filename">The path, absolute or relative, leading to the location of the existing database file.</param>
        public static void existingDatabase(string filename)
        {
            try
            {
                CloseDatabaseAndReleaseFile();

                // your code
                string cs = $"URI=file:{filename}; Foreign Keys=1";
                _connection = new SQLiteConnection(cs);
                
                dbConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        // ===================================================================
        // close existing database, wait for garbage collector to
        // release the lock before continuing
        // ===================================================================
        /// <summary>
        /// Closes the database and waits for the garbage collector to release the lock.
        /// </summary>
        static public void CloseDatabaseAndReleaseFile()
        {
            if (Database.dbConnection != null)
            {
                // close the database connection
                Database.dbConnection.Close();

                // wait for the garbage collector to remove the
                // lock from the database file
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }

}
