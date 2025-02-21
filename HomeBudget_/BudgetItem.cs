using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{
    // ====================================================================
    // CLASS: BudgetItem
    //        A single budget item, includes Category and Expense
    // ====================================================================

    /// <summary>
    /// Represents a budget entry, which includes information about an expense. BudgetItem contains details such as the associated category, the amount spent, balance after the expense, and a brief description.
    /// </summary>
    public class BudgetItem
    {
        /// <summary>
        /// Gets or sets the ID for the category to which the budget item belongs.
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// Gets or sets the ID for the budget expense item.
        /// </summary>
        public int ExpenseID { get; set; }
        /// <summary>
        /// Gets or sets the date when the budget item was recorded.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Gets or sets the name of the category to which the budget item belongs.
        /// </summary>
        public String Category { get; set; }
        /// <summary>
        /// Gets or sets a short description of the budget item.
        /// </summary>
        public String ShortDescription { get; set; }
        /// <summary>
        /// Gets or sets the amount of the expense for the budget item.
        /// </summary>
        public Double Amount { get; set; }
        /// <summary>
        ///  Gets or sets the balance remaining after expense has been recorded.
        /// </summary>
        public Double Balance { get; set; }

    }
    /// <summary>
    /// Represents a cist of budget items grouped by month. Includes details of all expenses for each month, and its total expenses for that month.
    /// </summary>
    public class BudgetItemsByMonth
    {
        /// <summary>
        /// Gets or sets the month for which the budget items are grouped.
        /// </summary>
        public String Month { get; set; }
        /// <summary>
        /// Gets or sets the list of budget item object for the certain month.
        /// </summary>
        public List<BudgetItem> Details { get; set; }
        /// <summary>
        /// Gets or sets the total of all budget items for that month.
        /// </summary>
        public Double Total { get; set; }
    }

    /// <summary>
    /// Represents a list of budget items grouped by category. It provides a summary of all expenses within a certain  category, and its total amount for that category.
    /// </summary>
    public class BudgetItemsByCategory
    {
        /// <summary>
        /// Gets or sets the name of the category for which budget items are grouped.
        /// </summary>
        public String Category { get; set; }
        /// <summary>
        /// Gets or sets the list of Budget items for the specific category.
        /// </summary>
        public List<BudgetItem> Details { get; set; }
        /// <summary>
        /// Gets or sets the total amount of all the budget items within a specific category.
        /// </summary>
        public Double Total { get; set; }

    }


}
