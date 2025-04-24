namespace HomeBudgetWPF
{

    public interface ViewInterface
    {

        void DisplayError(string message);

        void DisplayConfirmation(string message);

        void DisplayCategoryMenu();

        void DisplayExpenseMenu();

        void DisplaySelectFileMenu();

        void DisplayCategoryMenuWithName(string name);

        void CloseCategoryMenu();

        void CloseExpenseMenu();

        void CloseFileSelectMenu();
        void CloseMain();

    }


}
