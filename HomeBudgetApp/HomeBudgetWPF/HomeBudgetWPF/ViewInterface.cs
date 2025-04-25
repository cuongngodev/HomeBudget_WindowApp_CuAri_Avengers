using Budget;

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

        void CloseCategoryMenu();

        void CloseExpenseMenu();

        void CloseFileSelectMenu();

        void CloseMain();

        void DisplayCategoryTypes(List<Category.CategoryType> categoryTypes);

        void DisplayCategories(List<Category> categories);

    }


}
