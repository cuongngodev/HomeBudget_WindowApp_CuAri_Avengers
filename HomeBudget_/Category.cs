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
    // CLASS: Category
    //        - An individual category for budget program
    //        - Valid category types: Income, Expense, Credit, Saving
    // ====================================================================
    /// <summary>
    /// Represents a category used in Budget app. Each category corresponds to a budget item such as income, expense, credit, or savings.
    /// </summary>
    public class Category
    {
        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// Gets or sets the unique identifier for the category.
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// Gets or sets the description of the category.
        /// </summary>
        public String Description { get;}
        /// <summary>
        /// Gets or sets the type of the category.
        /// </summary>
        public CategoryType Type { get;  }
        /// <summary>
        /// Enumeration specifying if the category is related to income, expense, credit, savings.
        /// </summary>
        public enum CategoryType
        {
            Income,
            Expense,
            Credit,
            Savings
        };

        // ====================================================================
        // Constructor
        // ====================================================================
        public Category(int id, String description, CategoryType type = CategoryType.Expense)
        {
            this.Id = id;
            this.Description = description;
            this.Type = type;
        }

        // ====================================================================
        // Copy Constructor
        // ====================================================================
        public Category(Category category)
        {
            this.Id = category.Id;;
            this.Description = category.Description;
            this.Type = category.Type;
        }
        // ====================================================================
        // String version of object
        // ====================================================================
        public override string ToString()
        {
            return Description;
        }

    }
}

