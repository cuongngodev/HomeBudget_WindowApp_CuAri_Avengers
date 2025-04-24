using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudgetWPF
{
    public class ViewInterfaces
    {
        public interface MainViewInterface
        {
            void DisplayError(string message);

            void DisplayConfirmation(string message);

        }

        public interface CategoryInterface
        {
            void DisplayCategoryType();

            void SendCategoryInfo();
            
            void DisplayError(string message);

            void DisplayConfirmation(string message);

          

            void OpenWindow();

            void CloseWindow();
        }

        public interface FileSelectInterface
        {
            void OpenWindow();

            void CloseWindow();

            void ShowError(string msg);

            void ShowConfirmation(string msg);
        }

    }
}
