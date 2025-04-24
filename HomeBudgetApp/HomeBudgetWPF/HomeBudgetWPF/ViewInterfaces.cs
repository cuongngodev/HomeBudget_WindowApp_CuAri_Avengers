using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudgetWPF
{
    public class ViewInterfaces
    {
        public interface Basic
        {
            void DisplayError(string message);

            void DisplayConfirmation(string message);

        }

        public interface CategoryInterface
        {
            void DisplayCategoryType();

            void SendCategoryInfo(string desc, Budget.Category.CategoryType type);
            
            void DisplayError(string message);

            void DisplayConfirmation(string message);
        }


    }
}
