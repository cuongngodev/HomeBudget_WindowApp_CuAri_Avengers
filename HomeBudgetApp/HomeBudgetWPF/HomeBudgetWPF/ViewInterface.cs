using Budget;

using System.Windows;

namespace HomeBudgetWPF
{

    public interface ViewInterface
    {

        void DisplayError(string message);

        void DisplayConfirmation(string message);

        bool AskConfirmation(string message);

        void DisplayCategoryMenu();

        void DisplayExpenseMenu();

        void DisplayAddExpenseMenu();

        void DisplaySelectFileMenu();

        void DisplayCategoryMenuWithName(string name);

        void CloseCategoryMenu();

        void CloseExpenseMenu();

        void CloseAddExpenseMenu();

        void CloseFileSelectMenu();

        void ChangeColorTheme(string option);

        void SetDefaultTheme();

        void SetProtanopiaDeuteranopiaTheme();

        void SetTritanopiaTheme();

        void CloseMain();

        void DisplayCategoryTypes(List<Category.CategoryType> categoryTypes);

        void DisplayCategories(List<Category> categories);

    }


}
