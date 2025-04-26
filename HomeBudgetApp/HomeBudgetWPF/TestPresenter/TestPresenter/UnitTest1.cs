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
        calledDisplaySelectFileMenu = true;
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
    public void TestConstructor()
    {
        // Arrange
        MockView view = new MockView();
        // Act
        Presenter presenter = new Presenter(view);
        // Assert

        Assert.IsType<Presenter>(presenter);
    }
    [Fact]
    public void Test_SetUpApp()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        view.calledDisplaySelectFileMenu = false;
        // Act 
        presenter.SetupPresenter();
        // Assert
        Assert.True(view.calledDisplaySelectFileMenu);
    } 
    [Fact]
    public void Test_SetUpDatabase_Valid()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        string filePath = "./../Test.db";

        view.calledDisplayConfirmation = false;
        view.calledCloseFileSelectMenu = false;
        // Act 
        presenter.SetDatabase(filePath, true);
        // Assert
        Assert.True(view.calledDisplayConfirmation);
        Assert.True(view.calledCloseFileSelectMenu);
    } 
     [Fact]
    public void Test_SetUpDatabase_FileNameEmpty()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        string filePath = "";
        string fileName = "";
        string combined = System.IO.Path.Combine(filePath, fileName);
        view.calledDisplayError = false;
        // Act 
        presenter.SetDatabase(combined, true);
        // Assert
        Assert.True(view.calledDisplayError);
    } 
    
    [Fact]
    public void Test_OpenSelectFileMenu()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        view.calledDisplaySelectFileMenu = false;
        // Act
        presenter.OpenSelectFile();
        // Assert

        Assert.True(view.calledDisplaySelectFileMenu);
    }
    [Fact]
    public void Test_GetAllCategoryTypes()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        // Act
        var result = presenter.GetAllCategoryTypes();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
        Assert.Contains(result, type => type == Category.CategoryType.Income);
        Assert.Contains(result, type => type == Category.CategoryType.Savings);
        Assert.Contains(result, type => type == Category.CategoryType.Expense);
        Assert.Contains(result, type => type == Category.CategoryType.Credit);
    }
    [Fact]
    public void Test_OpenCategory()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        view.calledDisplayCategoryMenu = false;
        // Act
        presenter.OpenCategory();
        // Assert
        Assert.True(view.calledDisplayCategoryMenu);
    } 
    [Fact]
    public void Test_OpenExpense()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        view.calledDisplayExpenseMenu = false;
        // Act
        presenter.OpenExpense();
        // Assert
        Assert.True(view.calledDisplayExpenseMenu);
    }
    [Fact]
    public void Test_CloseExpense()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        view.calledCloseExpenseMenu = false;
        // Act
        presenter.CloseExpense();
        // Assert
        Assert.True(view.calledCloseExpenseMenu);
    }
    [Fact]
    public void Test_CloseCategory()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        view.calledDisplayCategoryMenu = false;
        // Act
        presenter.CloseCategory();
        // Assert
        Assert.True(view.calledCloseCategoryMenu);
    }
    [Fact]
    public void Test_GetAllCategories()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        view.calledDisplayCategoryMenu  = false;
        // Act
        presenter.CloseCategory();
        // Assert
        Assert.True(view.calledCloseCategoryMenu);
    }
    [Fact]
    public void Test_CreateNewExpense_InvalidAmount() { 
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        string description = "Test New Expense";
        DateTime date = new DateTime(2023, 10, 1);
        string amount = "100notgood";
        int categoryId = 1;

        view.calledDisplayError  = false;
        // Act
        presenter.SetDatabase("Test.db", true);
        presenter.CreateNewExpense( date, categoryId, amount, description);
        // Assert
        Assert.True(view.calledDisplayError);
    }
   [Fact]
    public void Test_CreateNewExpense_AmountIsEmpty()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        string description = "Test New Expense";
        DateTime date = new DateTime(2023, 10, 1);
        string amount = "";
        int categoryId = 1;

        view.calledDisplayError = false;
        // Act
        presenter.SetDatabase("Test.db", true);
        presenter.CreateNewExpense(date, categoryId, amount, description);
        // Assert
        Assert.True(view.calledDisplayError);
    }
    [Fact]
    public void Test_CreateNewExpense_DescriptionIsEmpty()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        string description = "";
        DateTime date = new DateTime(2023, 10, 1);
        string amount = "100";
        int categoryId = 1;

        view.calledDisplayError = false;
        // Act
        presenter.SetDatabase("Test.db", true);
        presenter.CreateNewExpense(date, categoryId, amount, description);
        // Assert
        Assert.True(view.calledDisplayError);
    }
   [Fact]
    public void Test_CreateNewExpense_Valid()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        string description = "Test New Expense";
        DateTime date = new DateTime(2023, 10, 1);
        string amount = "100";
        int categoryId = 1;

        view.calledDisplayConfirmation  = false;
        view.calledCloseExpenseMenu = false;
        // Act
        presenter.SetDatabase("Test.db", true);
        presenter.CreateNewExpense( date, categoryId, amount, description);
        // Assert
        Assert.True(view.calledDisplayConfirmation);
        Assert.True(view.calledCloseExpenseMenu);
    } 
    [Fact]
    public void Test_CreateNewCategory_Valid()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        string description = "Test New Category";
        Category.CategoryType type = Category.CategoryType.Income;
        view.calledDisplayConfirmation  = false;
        view.calledCloseExpenseMenu = false;
        // Act
        presenter.SetDatabase("Test.db", true);
        int numberOfCategory = presenter.GetAllCategories().Count;

        presenter.CreateNewCategory( description, type);
        // Assert
        Assert.True(view.calledDisplayConfirmation);
        Assert.True(view.calledCloseCategoryMenu);
        Assert.Equal(numberOfCategory + 1, presenter.GetAllCategories().Count);
    }
   
}