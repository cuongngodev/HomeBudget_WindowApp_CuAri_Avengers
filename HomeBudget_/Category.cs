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
    /// Represents an <see cref="Expense"/>'s category used in the Budget application. Each category corresponds to a budget item such as income, expense, credit, or savings.
    /// </summary>
    public class Category
    {
        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// Gets or sets the unique identifier of a category object.
        /// </summary>
        public int Id { get;}

        /// <summary>
        /// Gets or sets the description of a category object.
        /// </summary>
        public String Description { get;}

        /// <summary>
        /// Gets or sets the type of a category object.
        /// </summary>
        public CategoryType Type { get; }

        /// <summary>
        /// Enumeration specifying a <see cref="Category"/> object's possible types.
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
        /// <summary>
        /// Parameterized constructor that creates a <see cref="Category"/> object by taking in input corresponding to the class' individual fields.
        /// </summary>
        /// <param name="id">The unique identifier of a category object.</param>
        /// <param name="description">The description of a category object.</param>
        /// <param name="type">The type of a category object.</param>
        public Category(int id, String description, CategoryType type = CategoryType.Expense)
        {
            this.Id = id;
            this.Description = description;
            this.Type = type;
        }

        // ====================================================================
        // Copy Constructor
        // ====================================================================
        /// <summary>
        /// Parameterized constructor that creates a <see cref="Category"/> object by taking in input corresponding to another Category object.
        /// </summary>
        /// <param name="category">The <see cref="Category"/> object used to base the new object on.</param>
        public Category(Category category)
        {
            this.Id = category.Id; ;
            this.Description = category.Description;
            this.Type = category.Type;
        }

        // ====================================================================
        // String version of object
        // ====================================================================
        /// <summary>
        /// Overrides ToString() to allow for printing out the details of a <see cref="Category"/> object.
        /// </summary>
        /// <example>
        /// <code>
        /// Console.WriteLine(category1);
        /// </code>
        /// </example>
        /// <returns>The text contained within the <see cref="Category"/> object's <see cref="Description"/> property.</returns>
        public override string ToString()
        {
            return Description;
        }
    }
}

