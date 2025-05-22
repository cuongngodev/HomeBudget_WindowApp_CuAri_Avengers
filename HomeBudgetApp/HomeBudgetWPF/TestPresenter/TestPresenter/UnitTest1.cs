namespace TestPresenter;
using HomeBudgetWPF;
using Budget;
using System.Security.Policy;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

public class MockView : ViewInterface
{
    public bool calledDisplayError;
    public bool calledDisplayConfirmation;
    public bool calledAskConfirmation;
    public bool responseToAskConfirmation = false;
    public bool calledDisplayCategoryMenuWithName;
    public bool calledDisplayCategoryMenu;
    public bool calledDisplayCategoryTypes;
    public bool calledDisplayCategories;
    public bool calledDisplayExpenseMenu;
    public bool calledDisplaySelectFileMenu;
    public bool calledCloseCategoryMenu;
    public bool calledCloseExpenseMenu;
    public bool calledCloseFileSelectMenu;
    public bool calledChangeColorTheme;
    public bool calledSetDefaultTheme;
    public bool calledSetProtanopiaDeuteranopiaTheme;
    public bool calledSetTritanopiaTheme;
    public bool calledCloseMain;
    public bool calledDisplayExpenseItemsGrid;
    public bool calledDisplayExpenseItemsByCategory;
    public bool calledDisplayExpenseItemsByMonth;
    public bool calledDisplayExpenseItemmsByCategoryAndMonthGrid;
    public bool calledSetDefaultDate;
    public bool calledDisplayUpdateExpenseMenu;
    public bool calledDisplaySearchBar;
    public bool calledCloseSearchBar;
    public bool calledShowAudioError;
    public bool calledShowCategoriesOptions;
    public bool calledSetDataSourceForViewControl;
    public bool calledSetMonthSelectionForControlView;
    public bool calledSetCategoryForControlView;
    public bool calledUpdateSummaryButtonVisibility;
    public void DisplayError(string message)
    {
        calledDisplayError = true;
    }

    public void DisplayConfirmation(string message)
    {
        calledDisplayConfirmation = true;
    }
    public bool AskConfirmation(string message)
    {
        calledAskConfirmation = true;
        return responseToAskConfirmation;
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
    public void ChangeColorTheme(string option)
    {
        calledChangeColorTheme = true;
    }
    public void SetDefaultTheme()
    {
        calledSetDefaultTheme = true;
    }
    public void SetProtanopiaDeuteranopiaTheme()
    {
        calledSetProtanopiaDeuteranopiaTheme = true;
    }
    public void SetTritanopiaTheme()
    {
        calledSetTritanopiaTheme = true;
    }
    public void CloseMain()
    {
        calledCloseMain = true;
    }
    public void DisplayCategoryTypes(List<Category.CategoryType> categoryTypes)
    {
        calledDisplayCategoryTypes = true;
    }
    public void DisplayCategories(List<Category> categories)
    {
        calledDisplayCategories = true;
    }
    public void ShowCategoriesOptions(List<Category> categories)
    {
        calledShowCategoriesOptions = true;
    }
    public void DisplayCategoryMenuWithName(string name)
    {
        calledDisplayCategoryMenuWithName = true;
    }   
    public void DisplayExpenseItemsGrid(List<BudgetItem> items)
    {
        calledDisplayExpenseItemsGrid = true;
    }
    public void DisplayExpenseItemsByCategoryGrid(List<BudgetItemsByCategory> items)
    {
        calledDisplayExpenseItemsByCategory = true;
    }
    public void DisplayExpenseItemsByMonthGrid(List<BudgetItemsByMonth> items)
    {
        calledDisplayExpenseItemsByMonth= true;
    }

    public void DisplayExpenseItemmsByCategoryAndMonthGrid(List<Dictionary<string, object>> items, List<string> catNames)
    {
        calledDisplayExpenseItemmsByCategoryAndMonthGrid = true;
    }
    public void DisplayUpdateExpenseMenu(Expense expense)
    {
        // Implementation not needed for this test
        calledDisplayUpdateExpenseMenu = true;
    }
    public void SetDefaultDate(DateTime start, DateTime end)
    {
        calledSetDefaultDate = true;
    }

    public void DisplaySearchBar()
    {
        calledDisplaySearchBar = true;
    }

    public void CloseSearchBar()
    {
        calledCloseSearchBar = true;
    }

    public void ShowAudioError()
    {
        calledShowAudioError = true;
    }

    public void SetDataSourceForViewControl(List<Dictionary<string, object>> data)
    {
        calledSetDataSourceForViewControl = true;
    }
    public void SetMonthSelectionForControlView(List<string> months)
    {
        calledSetMonthSelectionForControlView = true;
    }
    public void SetCategoryForControlView(List<string> categories)
    {
        calledSetCategoryForControlView = true;
    }
    public void UpdateSummaryButtonVisibility(bool showBtnPieChart, bool showBtnDataGrid)
    {
        calledUpdateSummaryButtonVisibility = true;
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
        view.calledDisplayCategories = false;
        presenter.SetDatabase("Test.db", true);
        // Act
        presenter.OpenSelectFile();
        // Assert

        Assert.True(view.calledDisplaySelectFileMenu);
        Assert.True(view.calledDisplayCategories);
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
        presenter.SetDatabase("Test.db", true);
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

        presenter.SetDatabase("Test.db", true);
        view.calledDisplayError = false;
        // Act

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
        view.calledDisplayConfirmation = false;
        view.calledCloseExpenseMenu = false;
        // Act
        presenter.SetDatabase("Test.db", true);

        presenter.CreateNewCategory(description, type);
        // Assert
        Assert.True(view.calledDisplayConfirmation);
        Assert.True(view.calledCloseCategoryMenu);
    }
    [Fact]
    public void Test_CreateDuplicateExpense_DisplayError()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        string description = "Test Duplicate Category";
        Category.CategoryType type = Category.CategoryType.Income;
        view.calledDisplayError = false;
        // Act
        presenter.SetDatabase("Test.db", true);

        presenter.CreateNewCategory(description, type);
        presenter.CreateNewCategory(description, type);
        // Assert
        Assert.True(view.calledDisplayError);
    }
    [Fact]
    public void Test_CreateEmptyCategory_DisplayError()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        string description = "";
        Category.CategoryType type = Category.CategoryType.Income;
        view.calledDisplayError = false;
        // Act
        presenter.SetDatabase("Test.db", true);

        presenter.CreateNewCategory(description, type);
        presenter.CreateNewCategory(description, type);
        // Assert
        Assert.True(view.calledDisplayError);
    }
    
    [Fact]
    public void Test_CreateCategoryFromDropDown()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        string description = "Test Create Category From Dropdown";
        Category.CategoryType type = Category.CategoryType.Income;
        view.calledDisplayCategoryMenuWithName = false;
        view.calledDisplayCategoryTypes = false;
        presenter.SetDatabase("Test.db", true);
        // Act

        presenter.CreateNewCategoryFromDropDown(description);
        // Assert
        Assert.True(view.calledDisplayCategoryMenuWithName);
        Assert.True(view.calledDisplayCategoryTypes);
    }

    [Fact]
    public void Test_CreateCategoryFromExpense()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        string description = "Test Create Category From Expense";
        bool isCreateFromExpense = true;
        Category.CategoryType type = Category.CategoryType.Income;
        view.calledCloseMain = false;
        // Act
        presenter.SetDatabase("Test.db", true);

        presenter.CreateNewCategory(description, type, isCreateFromExpense);
        // Assert
        Assert.True(view.calledCloseMain);
    }
    [Fact]
    public void Test_MakeIdenticalExpense_AskForConfirmation()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        string description = "Test Duplicate Expense";
        DateTime date = new DateTime(2023, 10, 1);
        string amount = "100";
        int categoryId = 1;

        view.calledAskConfirmation = false;
        // Act
        presenter.SetDatabase("Test.db", true);
        presenter.CreateNewExpense(date, categoryId, amount, description);
        presenter.CreateNewExpense(date, categoryId, amount, description);
        // Assert
        Assert.True(view.calledAskConfirmation);
    }
  
    [Fact]
    public void Test_ChangeColorTheme_Default()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        string colorOfChoice = "Default";
        view.calledSetDefaultTheme = false;
        //Act
        presenter.ChangeColorTheme(colorOfChoice);
        // Assert
        Assert.True(view.calledSetDefaultTheme);

    }
    [Fact]
    public void Test_ChangeColorTheme_Protanopia()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        string colorOfChoice = "Protanopia / Deuteranopia";
        view.calledSetProtanopiaDeuteranopiaTheme = false;
        //Act
        presenter.ChangeColorTheme(colorOfChoice);
        // Assert
        Assert.True(view.calledSetProtanopiaDeuteranopiaTheme);
    }
      [Fact]
    public void Test_ChangeColorTheme_Tritanopia()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        string colorOfChoice = "Tritanopia";
        view.calledSetTritanopiaTheme = false;
        //Act
        presenter.ChangeColorTheme(colorOfChoice);
        // Assert
        Assert.True(view.calledSetTritanopiaTheme);
    }
    [Fact]
    public void Test_DisplayExpenseItems_NoFilter()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        DateTime start = new DateTime(2025, 5, 1);
        DateTime end = new DateTime(2025, 5, 29);
        view.calledDisplayExpenseItemsGrid = false;
        presenter.SetDatabase("Test.db", true);

        // Act
        presenter.DisplayExpenseItems(start, end, false, 1);
        // Assert
        Assert.True(view.calledDisplayExpenseItemsGrid);

    }

    [Fact]
    public void Test_DisplayExpenseItemsByMonth()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        DateTime start = new DateTime(2025, 3, 1);
        DateTime end = new DateTime(2025, 5, 29);
        view.calledDisplayExpenseItemsByMonth = false;
        presenter.SetDatabase("Test.db", true);

        // Act
        presenter.DisplayExpenseItemsByMonth(start, end, false, 1);
        // Assert
        Assert.True(view.calledDisplayExpenseItemsByMonth);
    }
    [Fact]
    public void Test_DisplayExpenseItemsByCategory()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        DateTime start = new DateTime(2025, 3, 1);
        DateTime end = new DateTime(2025, 5, 29);
        view.calledDisplayExpenseItemsByCategory = false;
        presenter.SetDatabase("Test.db", true);

        // Act
        presenter.DisplayExpenseItemsByCategory(start, end, false, 1);
        // Assert
        Assert.True(view.calledDisplayExpenseItemsByCategory);
    }
    [Fact]
    public void Test_DisplayExpenseItemsByMonthAndCategory()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        DateTime start = new DateTime(2025, 3, 1);
        DateTime end = new DateTime(2025, 5, 29);
        view.calledDisplayExpenseItemmsByCategoryAndMonthGrid = false;
        presenter.SetDatabase("Test.db", true);

        // Act
        presenter.DisplayExpenseItemsByCategoryAndMonth(start, end, false, 1);
        // Assert
        Assert.True(view.calledDisplayExpenseItemmsByCategoryAndMonthGrid);
    }
    [Fact]
    public void Test_DefaultDate()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        DateTime start = new DateTime(2025, 3, 1);
        DateTime end = new DateTime(2025, 5, 29);
        view.calledSetDefaultDate = false;
        presenter.SetDatabase("Test.db", true);

        // Act
        presenter.SetupDefaultDate();
        // Assert
        Assert.True(view.calledSetDefaultDate);
    }
    [Fact]
    public void Test_CheckDatePeriod_Valid()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        DateTime start = new DateTime(2025, 3, 1);
        DateTime end = new DateTime(2025, 5, 29);
        bool validDatePeriod = false;
        presenter.SetDatabase("Test.db", true);
        view.calledDisplayExpenseItemsGrid = false;

        // Act
        presenter.DisplayExpenseItems(start, end, false, 1);
        validDatePeriod = presenter.CheckDatePeriod(start, end);
        // Assert
        Assert.True(validDatePeriod);
        Assert.True(view.calledDisplayExpenseItemsGrid);
    }
    [Fact]
    public void Test_CheckDatePeriod_InValid()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        DateTime start = new DateTime(2025, 5, 1);
        DateTime end = new DateTime(2025, 3, 29);
        bool validDatePeriod = false;
        presenter.SetDatabase("Test.db", true);
        view.calledDisplayExpenseItemsGrid = false;
        view.calledDisplayError = false;

        // Act
        presenter.DisplayExpenseItems(start, end, false, 1);
        validDatePeriod = presenter.CheckDatePeriod(start, end);
        // Assert
        Assert.True(view.calledDisplayError);

        Assert.False(validDatePeriod);
    }

    [Fact]
    public void Test_OpenUpdateExpense()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        DateTime start = new DateTime(2025, 3, 1);
        DateTime end = new DateTime(2025, 5, 29);

        int expenseId = 1;
        presenter.SetDatabase("Test.db", true);
        presenter.CreateNewExpense(new DateTime(2025, 5, 5), 1, "100", "Testing Expense");

        view.calledDisplayUpdateExpenseMenu = false;
        view.calledDisplayCategories = false;

        // Act
        presenter.OpenUpdateExpense(expenseId);

        // Assert
        Assert.True(view.calledDisplayCategories);

        Assert.True(view.calledDisplayUpdateExpenseMenu);
    }

    [Fact]
    public void Test_UpdateExpense()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        DateTime start = new DateTime(2025, 3, 1);
        DateTime end = new DateTime(2025, 5, 29);

        int expenseId = 1;
        presenter.SetDatabase("Test.db", true);
        // presenter.CreateNewExpense(new DateTime(2025, 5, 5), 1, "100", "Testing Expense");


        view.calledDisplayConfirmation = false;
        view.calledCloseExpenseMenu = false;

        // Act
        presenter.UpdateExpense(0, new DateTime(2025, 2, 2), 2, "200", "Updated Expense");


        // Assert
        Assert.True(view.calledCloseExpenseMenu);

        Assert.True(view.calledDisplayConfirmation);
    }
    [Fact]
    public void Test_UpdateExpense_ExpenseNotSelected()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        DateTime start = new DateTime(2025, 3, 1);
        DateTime end = new DateTime(2025, 5, 29);

        int invalidExpenseId = -1;
        presenter.SetDatabase("Test.db", true);
        // presenter.CreateNewExpense(new DateTime(2025, 5, 5), 1, "100", "Testing Expense");


        view.calledDisplayError = false;

        // Act
        presenter.UpdateExpense(invalidExpenseId, new DateTime(2025, 2, 2), 2, "200", "Updated Expense");


        // Assert
        Assert.True(view.calledDisplayError);
    }
    [Fact]
    public void Test_UpdateExpense_AmountIsNotANumber()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        DateTime start = new DateTime(2025, 3, 1);
        DateTime end = new DateTime(2025, 5, 29);

        int expenseId = 1;
        string invalidAmount = "sdf";
        presenter.SetDatabase("Test.db", true);
        // presenter.CreateNewExpense(new DateTime(2025, 5, 5), 1, "100", "Testing Expense");


        view.calledDisplayError = false;

        // Act
        presenter.UpdateExpense(expenseId, new DateTime(2025, 2, 2), 2, invalidAmount, "Updated Expense");


        // Assert
        Assert.True(view.calledDisplayError);
    }

    [Fact]
    public void Test_searchBar()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        view.calledDisplaySearchBar = false;

        List<BudgetItem> items = new List<BudgetItem>
        {
            new BudgetItem { ExpenseID = 1, ShortDescription = "Test1" },
            new BudgetItem { ExpenseID = 2, ShortDescription = "Test2" },
            new BudgetItem { ExpenseID = 3, ShortDescription = "Test3" }
        };

        // Act
        BudgetItem test = presenter.Search(items[0],items,"2");

        // Assert
        Assert.Equal(test, items[1]);
    }
    [Fact]
    public void Test_searchBarNoFound()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        view.calledDisplaySearchBar = false;

        view.calledDisplayError = false;
        view.calledShowAudioError = false;
        List<BudgetItem> items = new List<BudgetItem>
        {
            new BudgetItem { ExpenseID = 1, ShortDescription = "Test1" },
            new BudgetItem { ExpenseID = 2, ShortDescription = "Test2" },
            new BudgetItem { ExpenseID = 3, ShortDescription = "Test3" }
        };

        // Act
        BudgetItem test = presenter.Search(items[0], items, "a");

        // Assert
        Assert.True(view.calledDisplayError);
        Assert.True(view.calledShowAudioError);
        Assert.Equal(test, items[0]);
    }
    [Fact]
    public void Test_searchBarReverse()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        view.calledDisplaySearchBar = false;

        view.calledDisplayError = false;
        view.calledShowAudioError = false;
        List<BudgetItem> items = new List<BudgetItem>
        {
            new BudgetItem { ExpenseID = 1, ShortDescription = "Test1" },
            new BudgetItem { ExpenseID = 2, ShortDescription = "Test2" },
            new BudgetItem { ExpenseID = 3, ShortDescription = "Test3" }
        };

        // Act
        BudgetItem test = presenter.Search(items[1], items, "1");

        // Assert
        Assert.Equal(test, items[0]);
    }

    [Fact]
    public void Test_searchNullSearchParams()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        view.calledDisplaySearchBar = false;

        view.calledDisplayError = false;
        view.calledShowAudioError = false;
        List<BudgetItem> items = new List<BudgetItem>
        {
            new BudgetItem { ExpenseID = 1, ShortDescription = "Test1" },
            new BudgetItem { ExpenseID = 2, ShortDescription = "Test2" },
            new BudgetItem { ExpenseID = 3, ShortDescription = "Test3" }
        };

        // Act
        BudgetItem test = presenter.Search(items[1], items, "");

        // Assert
        Assert.Equal(test, items[1]);
    }
    [Fact]
    public void Test_searchNullItems()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        view.calledDisplaySearchBar = false;

        view.calledDisplayError = false;
        view.calledShowAudioError = false;
        List<BudgetItem> items = new();
        BudgetItem item = new BudgetItem { ExpenseID = 1, ShortDescription = "Test1" };
        // Act
        BudgetItem test = presenter.Search(item, items, "a");

        // Assert
        Assert.Equal(test,item);
    }

    [Fact]
    public void TestGetCategoriesForFilter()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        presenter.SetDatabase("Test.db", true);
        view.calledShowCategoriesOptions = false;

        presenter.CreateNewCategory("test1", Category.CategoryType.Income);
        presenter.CreateNewCategory("test2", Category.CategoryType.Income);
        presenter.CreateNewCategory("test3", Category.CategoryType.Income);
        // Act
        presenter.GetCategoriesForFilter();
        // Assert
        Assert.True(view.calledShowCategoriesOptions);
    }

    [Fact]
    public void Test_NewCatFromDropDownFail()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        string description = "Test Create Category From Dropdown";
        Category.CategoryType type = Category.CategoryType.Income;
        view.calledDisplayError = false;
        presenter.SetDatabase("Test.db", true);
        presenter.CreateNewCategory(description, type);
        // Act
       
        bool test = presenter.CreateNewCategoryFromDropDown(description);
        // Assert
        Assert.False(test);
    }

    [Fact]
    public void Test_CreateNewExpenseInvalidCat()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        DateTime date = DateTime.MaxValue;
        string description = "Test Create Category From Dropdown";
        int categoryId = -1;
        
        presenter.SetDatabase("Test.db", true);
        // Act
        presenter.CreateNewExpense(date, categoryId, "100", description);

    }

    [Fact]
    public void Test_DeleteExpense()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        int expenseId = 1;
        view.calledDisplayConfirmation = false;
        view.calledCloseExpenseMenu = false;
        presenter.SetDatabase("Test.db", true);

        BudgetItem item = new BudgetItem
        {
            ExpenseID = expenseId,
            ShortDescription = "Test Delete Expense"
        };
        presenter.CreateNewExpense(DateTime.MaxValue, 1, "100", "Test Delete Expense");
        // Act
        presenter.DeleteExpense(item);
        // Assert
        Assert.True(view.calledDisplayConfirmation);
        Assert.True(view.calledCloseExpenseMenu);
    } 
    [Fact]
    public void Test_DeleteExpenseInvalidId()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        int expenseId = -1;
        view.calledDisplayError = false;
        presenter.SetDatabase("Test.db", true);

        BudgetItem item = new BudgetItem
        {
            ExpenseID = expenseId,
            ShortDescription = "Test Delete Expense"
        };
        presenter.CreateNewExpense(DateTime.MaxValue, 1, "100", "Test Delete Expense");
        // Act
        presenter.DeleteExpense(item);
        // Assert
        Assert.True(view.calledDisplayError);
    }

    [Fact]
    public void Test_SetDataSourceForViewControl()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        view.calledSetCategoryForControlView = false;
        view.calledSetMonthSelectionForControlView = true;
        presenter.SetDatabase("Test.db", true);
        presenter.GetMonthList(data);
        // Act
        presenter.GetCategoryList(data);

        Assert.True(view.calledSetCategoryForControlView);
        Assert.True(view.calledSetMonthSelectionForControlView);
    }
    [Fact]
    public void Test_SetMonthSelectionForControlView()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        view.calledSetCategoryForControlView = false;
        view.calledSetMonthSelectionForControlView = true;
        presenter.SetDatabase("Test.db", true);
        presenter.GetMonthList(data);
        // Act
        presenter.GetCategoryList(data);

        Assert.True(view.calledSetCategoryForControlView);
        Assert.True(view.calledSetMonthSelectionForControlView);
    }

    [Fact]
    public void Test_GetBudgetItemsByMonthAndCategory_DateNull()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        DateTime? startDate = null;
        DateTime? endDate = null;
        bool isFilterByCategoryChecked = true;
        int selectedCategoryIndex = 1;
        bool isSummaryByMonthChecked = true;
        bool isSummaryByCategoryChecked = false;

        view.calledDisplayError = false;
        presenter.SetDatabase("Test.db", true);
        // Act
        presenter.GetBudgetItemsByMonthAndCategory(startDate, endDate, isFilterByCategoryChecked, selectedCategoryIndex, isSummaryByMonthChecked, isSummaryByCategoryChecked);
        Assert.True(view.calledDisplayError);
    }

    [Fact]
    public void Test_GetBudgetItemsByMonthAndCategory_Valid()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        DateTime startDate = new DateTime(2025, 4, 29);
        DateTime endDate = new DateTime(2025, 5, 29);
        bool isFilterByCategoryChecked = true;
        int selectedCategoryIndex = 1;
        bool isSummaryByMonthChecked = true;
        bool isSummaryByCategoryChecked = false;

        view.calledSetDataSourceForViewControl = false;
        presenter.SetDatabase("Test.db", true);
        // Act
        presenter.GetBudgetItemsByMonthAndCategory(startDate, endDate, isFilterByCategoryChecked, selectedCategoryIndex, isSummaryByMonthChecked, isSummaryByCategoryChecked);
        Assert.True(view.calledSetDataSourceForViewControl);
    }
      [Fact]
    public void Test_UpdateSummaryButtonVisibility()
    {
        // Arrange
        MockView view = new MockView();
        Presenter presenter = new Presenter(view);
        DateTime startDate = new DateTime(2025, 4, 29);
        DateTime endDate = new DateTime(2025, 5, 29);
        bool isFilterByCategoryChecked = true;
        int selectedCategoryIndex = 1;
        bool isSummaryByMonthChecked = true;
        bool isSummaryByCategoryChecked = false;

        view.calledUpdateSummaryButtonVisibility = false;
        presenter.SetDatabase("Test.db", true);
        // Act
        presenter.HandleSummaryButtonVisibility(isSummaryByMonthChecked, isFilterByCategoryChecked);
        presenter.DisplayExpenseDataGrid(startDate, endDate, isFilterByCategoryChecked, selectedCategoryIndex, isSummaryByMonthChecked, isSummaryByCategoryChecked);

        Assert.True(view.calledUpdateSummaryButtonVisibility);
    }

}