namespace TestPresenter;
using HomeBudgetWPF;
using Budget;

public class MockView : ViewInterface
{
    public bool calledDisplayError;
    public bool calledDisplayConfirmation;
    public bool calledDisplayCategoryMenu;
    public bool calledDisplayExpenseMenu;
    public bool calledDisplaySelectFileMenu;
    public bool calledCloseCategoryMenu;
    public bool calledCloseExpenseMenu;
    public bool calledCloseFileSelectMenu;

    public void DisplayError(string message)
    {
        calledDisplayError = true;
    }

    public void DisplayConfirmation(string message)
    {
        calledDisplayConfirmation = true;
    }

    public void DisplayCategoryMenu()
    {
        calledDisplayCategoryMenu = true;
    }

    public void DisplayExpenseMenu()
    {
        calledDisplayExpenseMenu = true;
    }

    public void DisplaySelectFileMenu()
    {
        calledCloseFileSelectMenu = true;
    }
    public void CloseCategoryMenu()
    {
        calledCloseCategoryMenu = true;
    }
    public void CloseExpenseMenu()
    {
        calledCloseExpenseMenu = true;
    }
    public void CloseFileSelectMenu()
    {
        calledCloseFileSelectMenu = true;
    }
}
public class UnitTest1
{
    [Fact]
    public void Test1()
    {
      
    }
}