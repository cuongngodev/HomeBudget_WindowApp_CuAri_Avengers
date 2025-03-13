using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Dynamic;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================


namespace Budget
{
    // ====================================================================
    // CLASS: HomeBudget
    //        - Combines categories Class and expenses Class
    //        - One File defines Category and Budget File
    //        - etc
    // ====================================================================
    /// <summary>
    /// Combines the Categories and Expenses classes to manage budget data. 
    /// It provides functionality to read, save, and analyze budget information, including grouping expenses by month, category, or both.
    /// </summary>
    public class HomeBudget
    {

        private string _FileName;
        private string _DirName;
        private Categories _categories;
        private Expenses _expenses;

        // ====================================================================
        // Properties
        // ===================================================================

        // Properties (location of files etc)
        /// <summary>
        /// Gets the name of the budget file.
        /// </summary>
        public String FileName { get { return _FileName; } }
        /// <summary>
        /// Gets the directory name of the home budget file.
        /// </summary>
        public String DirName { get { return _DirName; } }
        /// <summary>
        /// Gets the path name of home budget app, path name is a combination of file name and directory name.
        /// Return null if not found.
        /// </summary>
        public String PathName
        {
            get
            {
                if (_FileName != null && _DirName != null)
                {
                    return Path.GetFullPath(_DirName + "\\" + _FileName);
                }
                else
                {
                    return null;
                }
            }
        }

        // Properties (categories and expenses object)
        /// <summary>
        /// Gets the collection of category items.
        /// </summary>
        public Categories categories { get { return _categories; } }
        /// <summary>
        /// Gets the collection of expense items.
        /// </summary>
        public Expenses expenses { get { return _expenses; } }

        // -------------------------------------------------------------------
        // Constructor (new... default categories, no expenses)
        // -------------------------------------------------------------------
        public HomeBudget()
        {
            _categories = new Categories();
            _expenses = new Expenses();

        }

        // -------------------------------------------------------------------
        // Constructor (existing budget ... must specify file)
        // -------------------------------------------------------------------
    /*    public HomeBudget(String budgetFileName)
        {
            _categories = new Categories();
            _expenses = new Expenses();
            //ReadFromFile(budgetFileName);
        }*/
        public HomeBudget(String databaseFile, bool newDB = false)
        {
            // if database exists, and user doesn't want a new database, open existing DB
            if (!newDB && File.Exists(databaseFile))
            {
                Database.existingDatabase(databaseFile);
            }
            // file did not exist, or user wants a new database, so open NEW DB
            else
            {
                Database.newDatabase(databaseFile);
                newDB = true;
            }
            // create the category object
            _categories = new Categories(Database.dbConnection, newDB);
            // create the expenses object
            _expenses = new Expenses(); // need a constructor ? or default one is good?
        }
       /* public HomeBudget(String databaseFile, String expensesXMLFile, bool newDB = false)
        {
            // if database exists, and user doesn't want a new database, open existing DB
            if (!newDB && File.Exists(databaseFile))
            {
                Database.existingDatabase(databaseFile);
            }

            // file did not exist, or user wants a new database, so open NEW DB
            else
            {
                Database.newDatabase(databaseFile);
                newDB = true;
            }

            // create the category object
            _categories = new Categories(Database.dbConnection, newDB);

            // create the _expenses course
            _expenses = new Expenses();
            _expenses.ReadFromFile(expensesXMLFile);
        }
*/

    
        #region GetList



        // ============================================================================
        // Get all expenses list
        // NOTE: VERY IMPORTANT... budget amount is the negative of the expense amount
        // Reasoning: an expense of $15 is -$15 from your bank account.
        // ============================================================================
        /// <summary>
        /// Retrieves the list of budget items within a specific time frame, optionally filtered by category.
        /// If Start or End has null value, the generated dates will be used.
        /// </summary>
        /// <param name="Start"> The Date start (optional). If null, default to the earliest date.</param>
        /// <param name="End"> The Date end (optional). If null, default to the latest date. </param>
        /// <param name="FilterFlag"> If true, filters by the provided category, if false, list all budget items.</param>
        /// <param name="CategoryID"> The Category Id is used to filter by (if FilterFlag is true). </param>
        /// <returns>The list of all budget items sorted by date; with calculated balances</returns>
        /// <remarks>
        /// If FilterFlag is true and the CategoryID is not found, the method returns all budget items.
        /// </remarks>
        /// <example>
        /// <code>
        /// HomeBudget homeBudget = new HomeBudget();
        /// DateTime start = new DateTime(2019,1,11);
        /// DateTime end = new DateTime(2019,2,5);
        /// List<BudgetItem> budgetItems = homeBudget.GetBudgetItems(start, end, true, 2);
        /// foreach(BudgetItem item in budgetItems)
        /// {
        ///     Console.WriteLine($"{item.Date} + {item.Date.ToString("yyyy/mm/dd")} {item.Category}");
        /// }
        /// </code>
        /// <code> 
        /// // Example of output 
        /// ====================================================================================================
        ///                                       Budget Items Report
        /// ====================================================================================================
        /// Cat_ID     Category        Date            Description               Amount          Balance
        /// ----------------------------------------------------------------------------------------------------
        /// 10         Clothes         01-10-2018      hat (on credit)           -$10.00         -$10.00
        /// 9          Credit Card     01-11-2018      hat                       $10.00          $0.00
        /// 10         Clothes         01-10-2019      scarf (on credit)         -$15.00         -$15.00
        /// 9          Credit Card     01-10-2020      scarf                     $15.00          $0.00
        /// 14         Eating Out      01-11-2020      McDonalds                 -$45.00         -$45.00
        /// 14         Eating Out      01-12-2020      Wendys                    -$25.00         -$70.00
        /// 14         Eating Out      02-01-2020      Pizza                     -$33.33         -$103.33
        /// 9          Credit Card     02-10-2020      mittens                   $15.00          -$88.33
        /// 9          Credit Card     02-25-2020      Hat                       $25.00          -$63.33
        /// 14         Eating Out      02-27-2020      Pizza                     -$33.33         -$96.66
        /// 14         Eating Out      07-11-2020      Cafeteria                 -$11.11         -$107.77
        /// ----------------------------------------------------------------------------------------------------
        /// </code>
        /// <code> 
        /// //Example showing the date range is inclusive, which includes items from the start date.
        /// Enter start Date YYYY/MM/DD: 2020/1/1
        /// Enter end Date YYYY/MM/DD: 2020/1/12
        /// Filter Flag y/n?: n
        /// Enter category ID: 1
        /// ====================================================================================================
        ///                                         Budget Items Report
        /// ====================================================================================================
        /// Cat_ID     Category        Date            Description               Amount          Balance
        /// ----------------------------------------------------------------------------------------------------
        /// 9          Credit Card     01-10-2020      scarf                     $15.00          $15.00
        /// 14         Eating Out      01-11-2020      McDonalds                 -$45.00         -$30.00
        /// 14         Eating Out      01-12-2020      Wendys                    -$25.00         -$55.00
        /// ----------------------------------------------------------------------------------------------------
        /// </code>
        /// </example>
        /// - What happens if the filter flag is set to true? 
        /// If FilterFlag is true, filters Budget items by the provided category ID, but if the CategoryID is not found, the method returns all budget items.
        /// 
        /// Category ID is provided as 10 when the FilterFlag is true, here is the output:
        /// <code>
        /// ====================================================================================================
        ///                                         Budget Items Report
        /// ====================================================================================================
        /// Cat_ID     Category        Date            Description               Amount          Balance
        /// ----------------------------------------------------------------------------------------------------
        /// 10         Clothes         01-10-2018      hat (on credit)           -$10.00         -$10.00
        /// 10         Clothes         01-10-2019      scarf (on credit)         -$15.00         -$25.00
        /// ----------------------------------------------------------------------------------------------------
        /// </code>
        public List<BudgetItem> GetBudgetItems(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // ------------------------------------------------------------------------
            // return joined list within time frame
            // ------------------------------------------------------------------------
            Start = Start ?? new DateTime(1900, 1, 1);
            End = End ?? new DateTime(2500, 1, 1);

            var query = from c in _categories.List()
                        join e in _expenses.List() on c.Id equals e.Category
                        where e.Date >= Start && e.Date <= End
                        select new { CatId = c.Id, ExpId = e.Id, e.Date, Category = c.Description, e.Description, e.Amount };

            // ------------------------------------------------------------------------
            // create a BudgetItem list with totals,
            // ------------------------------------------------------------------------
            List<BudgetItem> items = new List<BudgetItem>();
            Double total = 0;

            foreach (var e in query.OrderBy(q => q.Date))
            {
                // filter out unwanted categories if filter flag is on
                if (FilterFlag && CategoryID != e.CatId)
                {
                    continue;
                }
                // testing with branch

                // keep track of running totals
                total = total + e.Amount;
                items.Add(new BudgetItem
                {
                    CategoryID = e.CatId,
                    ExpenseID = e.ExpId,
                    ShortDescription = e.Description,
                    Date = e.Date,
                    Amount = -e.Amount,
                    Category = e.Category,
                    Balance = total
                });
            }

            return items;
        }

        // ============================================================================
        // Group all expenses month by month (sorted by year/month)
        // returns a list of BudgetItemsByMonth which is 
        // "year/month", list of budget items, and total for that month
        // ============================================================================

        /// <summary>
        /// Retrieves a list of Budget Item groupped by month (sorted by year/month)
        /// </summary>
        /// <param name="Start"> Date start (optional). If null, default to the earliest date.</param>
        /// <param name="End"> Date end (optional). If null, default to the latest date. </param>
        /// <param name="FilterFlag"></param> If true budget items are filtered by category, if false, list all budget items.
        /// <param name="CategoryID"></param> Category Id is used to filter the budget item.
        /// <returns>The list of all budget items group by month clustered by specified category ID if filter flag is true; if the category ID is not found,it would fall into the false case of FilterFlag, where it returns a list of budget items of all categories.</returns>
        /// <example>
        /// <code>
        /// HomeBudget homeBudget = new HomeBudget();
        /// DateTime start = new DateTime(2019,1,11);
        /// DateTime end = new DateTime(2019,2,5);
        /// List -ltBudgetItemsByMonth-gt budgetItemsByMonth = homeBudget.GetBudgetItemsByMonth(start, end, true, 2);
        /// foreach(BudgetItem itemByMonth in budgetItemsByMonth)
        /// {
        ///     Concole.Write($"{itemByMonth.Month}");
        ///     Concole.Write($"{itemByMonth.Total}");
        ///     foreach(BudgetItem item in itemByMonth.Details)
        ///     {
        ///         Console.WriteLine($"{item.ShortDescription}");
        ///     }    
        /// }
        /// </code>
        /// <code>
        /// // Example of output
        /// ==============================================================
        ///                     Budget Items by Month
        /// ==============================================================
        /// Month           Total                Description
        /// --------------------------------------------------------------
        /// 2018/01         $0.00               hat (on credit)
        ///                                     hat
        /// --------------------------------------------------------------
        /// 2019/01         -$15.00             scarf (on credit)
        /// --------------------------------------------------------------
        /// 2020/01         -$55.00             scarf
        ///                                     McDonalds
        ///                                     Wendys
        /// --------------------------------------------------------------
        /// 2020/02         -$26.66             Pizza
        ///                                     mittens
        ///                                     Hat
        ///                                     Pizza
        /// --------------------------------------------------------------
        /// 2020/07         -$11.11             Cafeteria
        /// --------------------------------------------------------------
        /// </code>
        /// </example>       
        public List<BudgetItemsByMonth> GetBudgetItemsByMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items first
            // -----------------------------------------------------------------------
            List<BudgetItem> items = GetBudgetItems(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // Group by year/month
            // -----------------------------------------------------------------------
            var GroupedByMonth = items.GroupBy(c => c.Date.Year.ToString("D4") + "/" + c.Date.Month.ToString("D2"));

            // -----------------------------------------------------------------------
            // create new list
            // -----------------------------------------------------------------------
            var summary = new List<BudgetItemsByMonth>();
            foreach (var MonthGroup in GroupedByMonth)
            {
                // calculate total for this month, and create list of details
                double total = 0;
                var details = new List<BudgetItem>();
                foreach (var item in MonthGroup)
                {
                    total = total + item.Amount;
                    details.Add(item);
                }

                // Add new BudgetItemsByMonth to our list
                summary.Add(new BudgetItemsByMonth
                {
                    Month = MonthGroup.Key,
                    Details = details,
                    Total = total
                });
            }

            return summary;
        }

        // ============================================================================
        // Group all expenses by category (ordered by category name)
        // ============================================================================









        /// <summary>
        /// Retrieves a list of Budget Item groupped by category.
        /// </summary>
        /// <example>
        /// <code>
        /// HomeBudget homeBudget = new HomeBudget();
        /// DateTime start = new DateTime(2019, 1, 11);
        /// DateTime end = new DateTime(2019, 2, 5);
        /// List-ltBudgetItemsByCategory-gt budgetItemsByCategory = homeBudget.GetBudgetItemsByCategory(start,end,true,2);
        /// foreach(BudgetItemsByCategory itemByCat in budgetItemsByCategory)
        /// {
        ///     Console.Write($"{itemByCat.Category}");
        ///     Console.Write($"{itemByCat.Total}");
        ///     foreach(BudgetItem item in itemByCat.Details)
        ///     {
        ///         Console.WriteLine($"{item.ShortDescription}");
        ///     }
        /// }
        /// </code>
        /// <code>
        /// //Sample output 
        /// ==============================================================
        ///                    Budget Items by Category
        /// ==============================================================
        /// Category        Total                Description
        /// --------------------------------------------------------------
        /// Clothes         -$25.00             hat (on credit)
        ///                                     scarf (on credit)
        /// --------------------------------------------------------------
        /// Credit Card     $65.00              hat
        ///                                     scarf
        ///                                     mittens
        ///                                     Hat
        /// --------------------------------------------------------------
        /// Eating Out      -$147.77            McDonalds
        ///                                     Wendys
        ///                                     Pizza
        ///                                     Pizza
        ///                                     Cafeteria
        /// --------------------------------------------------------------
        /// </code>
        /// 
        /// <code>
        /// //Example of Filter by date from 2020/1/1 to 2020/1/12
        /// ==============================================================
        ///                    Budget Items by Category
        /// ==============================================================
        /// Category        Total               Description
        /// --------------------------------------------------------------
        /// Credit Card     $15.00              scarf
        /// --------------------------------------------------------------
        /// Eating Out      -$70.00             McDonalds
        ///                                     Wendys
        /// --------------------------------------------------------------
        /// </code>
        /// </example>
        /// <param name="Start">Date start (optional). If null, default to the earliest date.</param>
        /// <param name="End">Date end (optional). If null, default to the latest date. </param>
        /// <param name="FilterFlag">If true budget items are filtered by category, if false, list all budget items.</param>
        /// <param name="CategoryID">Category Id is used to filter the budget item.</param>
        /// <returns>The list of all budget items group by category clustered by specified category ID if filter flag is true; if the category ID is not found, returns a list of budget items</returns>
        public List<BudgetItemsByCategory> GetBudgetItemsByCategory(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items first
            // -----------------------------------------------------------------------
            List<BudgetItem> items = GetBudgetItems(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // Group by Category
            // -----------------------------------------------------------------------
            var GroupedByCategory = items.GroupBy(c => c.Category);

            // -----------------------------------------------------------------------
            // create new list
            // -----------------------------------------------------------------------
            var summary = new List<BudgetItemsByCategory>();
            foreach (var CategoryGroup in GroupedByCategory.OrderBy(g => g.Key))
            {
                // calculate total for this category, and create list of details
                double total = 0;
                var details = new List<BudgetItem>();
                foreach (var item in CategoryGroup)
                {
                    total = total + item.Amount;
                    details.Add(item);
                }
                // Add new BudgetItemsByCategory to our list
                summary.Add(new BudgetItemsByCategory
                {
                    Category = CategoryGroup.Key,
                    Details = details,
                    Total = total
                });
            }
            return summary;
        }


        // ============================================================================
        // Group all events by category and Month
        // creates a list of Dictionary objects (which are objects that contain key value pairs).
        // The list of Dictionary objects includes:
        //          one dictionary object per month with expenses,
        //          and one dictionary object for the category totals
        // 
        // Each per month dictionary object has the following key value pairs:
        //           "Month", <the year/month for that month as a string>
        //           "Total", <the total amount for that month as a double>
        //            and for each category for which there is an expense in the month:
        //             "items:category", a List<BudgetItem> of all items in that category for the month
        //             "category", the total amount for that category for this month
        //
        // The one dictionary for the category totals has the following key value pairs:
        //             "Month", the string "TOTALS"
        //             for each category for which there is an expense in ANY month:
        //             "category", the total for that category for all the months
        // ============================================================================

        /// <summary>
        /// Creates a list of dictionary budget items has a colection of string and object pair by both month and category.
        /// </summary>
        /// <param name="Start"> Date start (optional). If null, default to the earliest date.</param>
        /// <param name="End"> Date end (optional). If null, default to the latest date. </param>
        /// <param name="FilterFlag">If true budget items are filtered by category, if false, list all budget items.</param>
        /// <param name="CategoryID">Category Id is used to filter the budget item.</param>
        /// <returns>List of dictionaries containing budget information as follows:
        ///   - For each month with expenses:
        ///   - "details: CategoryName": A list of budget items for that category.
        ///   - "CategoryName": The total amount for that category in the month.
        ///   - A final dictionary with "Month" set to "TOTALS" provides category totals across all months.
        /// </returns>
        /// <example>
        /// <code>
        /// HomeBudget homeBudget = new HomeBudget(filePath);
        /// DateTime startDate = new DateTime(2019,1,11)
        /// DateTime endDate = new DateTime(2019,2,5)
        /// List -lt Dictionary-lt string, object -gt -gt budgetItemByCategoryAndMonth = homeBudget.GetBudgetDictionaryByCategoryAndMonth(startDate, endDate, filterFlag, categoryId);
        /// foreach(var budgetItemDict in budgetItemByCategoryAndMonth)
        /// {
        ///     // Gets the month and its total 
        ///     Console.WriteLine(budgetItemDict["Month"].ToString());
        ///     // Gets total budget of this month
        ///     Console.WriteLine(budgetItemDict["Total"]);
        ///     // Gets Category
        ///     Console.WriteLine(budgetItemDict["categoryName"]);
        /// }
        /// </code>
        /// 
        /// <code>
        /// // Sameple output 
        /// ==========================================================================================
        ///                             Budget Items by Month and Category
        /// ==========================================================================================
        /// Month          Clothes             Credit Card         Eating Out          Total
        /// ------------------------------------------------------------------------------------------
        /// 2018/01        -10.00              10.00               0.00                0.00
        /// 2019/01        -15.00              0.00                0.00                -15.00
        /// 2020/01        0.00                15.00               -70.00              -55.00
        /// 2020/02        0.00                40.00               -66.66              -26.66
        /// 2020/07        0.00                0.00                -11.11              -11.11
        /// TOTALS         -25.00              65.00               -147.77             0.00
        /// ------------------------------------------------------------------------------------------
        /// ==========================================================================================
        /// </code>
        /// </example> 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <param name="FilterFlag"></param>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetBudgetDictionaryByCategoryAndMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items by month 
            // -----------------------------------------------------------------------
            List<BudgetItemsByMonth> GroupedByMonth = GetBudgetItemsByMonth(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // loop over each month
            // -----------------------------------------------------------------------
            var summary = new List<Dictionary<string, object>>();
            var totalsPerCategory = new Dictionary<String, Double>();

            foreach (var MonthGroup in GroupedByMonth)
            {
                // create record object for this month
                Dictionary<string, object> record = new Dictionary<string, object>();
                record["Month"] = MonthGroup.Month;
                record["Total"] = MonthGroup.Total;

                // break up the month details into categories
                var GroupedByCategory = MonthGroup.Details.GroupBy(c => c.Category);

                // -----------------------------------------------------------------------
                // loop over each category
                // -----------------------------------------------------------------------
                foreach (var CategoryGroup in GroupedByCategory.OrderBy(g => g.Key))
                {

                    // calculate totals for the cat/month, and create list of details
                    double total = 0;
                    var details = new List<BudgetItem>();

                    foreach (var item in CategoryGroup)
                    {
                        total = total + item.Amount;
                        details.Add(item);
                    }

                    // add new properties and values to our record object
                    record["details:" + CategoryGroup.Key] = details;
                    record[CategoryGroup.Key] = total;

                    // keep track of totals for each category
                    if (totalsPerCategory.TryGetValue(CategoryGroup.Key, out Double CurrentCatTotal))
                    {
                        totalsPerCategory[CategoryGroup.Key] = CurrentCatTotal + total;
                    }
                    else
                    {
                        totalsPerCategory[CategoryGroup.Key] = total;
                    }
                }

                // add record to collection
                summary.Add(record);
            }
            // ---------------------------------------------------------------------------
            // add final record which is the totals for each category
            // ---------------------------------------------------------------------------
            Dictionary<string, object> totalsRecord = new Dictionary<string, object>();
            totalsRecord["Month"] = "TOTALS";

            foreach (var cat in categories.List())
            {
                try
                {
                    totalsRecord.Add(cat.Description, totalsPerCategory[cat.Description]);
                }
                catch { }
            }
            summary.Add(totalsRecord);


            return summary;
        }
      



        #endregion GetList

    }
}
