using System.Windows;

namespace HomeBudgetWPF
{

    public interface ViewInterface
    {
        void DisplayError(string message);

        void DisplayConfirmation(string message);

        void DisplayCategoryMenu();

        void DisplayExpenseMenu();

        void DisplaySelectFileMenu();

        void CloseCategoryMenu();

        void CloseExpenseMenu();

        void CloseFileSelectMenu();

        void ChangeColorTheme(object sender, RoutedEventArgs e);
    }


}
