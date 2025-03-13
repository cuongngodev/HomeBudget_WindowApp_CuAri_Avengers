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
    /// Represents an expense made and accessed on the Budget application.
    /// </summary>
    public class Expense
    {
        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// Gets the unique identifier of an expense object.
        /// </summary>
        public int Id { get; }
        
        /// <summary>
        /// Gets the date when a new expense object was created. 
        /// </summary>
        public DateTime Date { get; }
        
        /// <summary>
        /// Gets the amount tied to an expense object.
        /// </summary>
        public Double Amount { get; }
        
        /// <summary>
        /// Gets the description of an expense object.
        /// </summary>
        public String Description { get;  }
        
        /// <summary>
        /// Gets the category tied to an expense object.
        /// </summary>
        public int Category { get; }

        // ====================================================================
        // Constructor
        //    NB: there is no verification the expense category exists in the
        //        categories object
        // ====================================================================
        /// <summary>
        /// Parameterized constructor that creates an <see cref="Expense"/> object by taking in input corresponding to the class' individual fields.
        /// </summary>
        /// <param name="id">The unique identifier of the new expense object.</param>
        /// <param name="date">The date when the new expense object was created.</param>
        /// <param name="category">The category tied to the new expense object.</param>
        /// <param name="amount">The amount tied to the new expense object.</param>
        /// <param name="description">The description of the new expense object.</param>
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
        /// <summary>
        /// Parameterized constructor that creates an <see cref="Expense"/> object by taking in input corresponding to another Expense object.
        /// </summary>
        /// <param name="obj">The <see cref="Expense"/> object used to base the new object on.</param>
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
