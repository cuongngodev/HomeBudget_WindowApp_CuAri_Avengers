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

        void DisplaySelectFileMenu();

        void DisplayCategoryMenuWithName(string name);

        void DisplayUpdateExpenseMenu(Expense expense);

        void CloseCategoryMenu();

        void CloseExpenseMenu();
        
        void DisplayExpenseItemsGrid(List<BudgetItem> items);

        void DisplayExpenseItemsByCategoryGrid(List<BudgetItemsByCategory> items);

        void DisplayExpenseItemsByMonthGrid(List<BudgetItemsByMonth> items);
        
        void DisplayExpenseItemmsByCategoryAndMonthGrid(List<Dictionary<string, object>> items, List<string> catNames);

        void CloseFileSelectMenu();

        void ChangeColorTheme(string option);

        void SetDefaultTheme();

        void SetProtanopiaDeuteranopiaTheme();

        void SetTritanopiaTheme();

        void CloseMain();

        void DisplayCategoryTypes(List<Category.CategoryType> categoryTypes);

        void DisplayCategories(List<Category> categories);

        void ShowCategoriesOptions(List<Category> categories);

        void SetDefaultDate(DateTime start, DateTime end);
    }


}
