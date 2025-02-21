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
    // CLASS: Expense
    //        - An individual expens for budget program
    // ====================================================================
    /// <summary>
    /// Represents an expense in the budget app.
    /// </summary>
    public class Expense
    {
        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// Gets a number representing the expense's unique ID.
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// Gets the date when a new expense is created. 
        /// </summary>
        public DateTime Date { get;  }
        /// <summary>
        /// Gets or sets the amount of the expense.
        /// </summary>
        public Double Amount { get; set; }
        /// <summary>
        /// Gets or sets a description of the expense.
        /// </summary>
        public String Description { get; set; }
        /// <summary>
        /// Gets or sets the category of the expense.
        /// </summary>
        public int Category { get; set; }

        // ====================================================================
        // Constructor
        //    NB: there is no verification the expense category exists in the
        //        categories object
        // ====================================================================
        public Expense(int id, DateTime date, int category, Double amount, String description)
        {
            this.Id = id;
            this.Date = date;
            this.Category = category;
            this.Amount = amount;
            this.Description = description;
        }

        // ====================================================================
        // Copy constructor - does a deep copy
        // ====================================================================
        public Expense (Expense obj)
        {
            this.Id = obj.Id;
            this.Date = obj.Date;
            this.Category = obj.Category;
            this.Amount = obj.Amount;
            this.Description = obj.Description;
           
        }
    }
}
