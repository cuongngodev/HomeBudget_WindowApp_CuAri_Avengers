using Budget;
using System.Data.SQLite;

namespace BudgetCodeTests
{
    [Collection("Sequential")]
    public class TestExpenses
    {
        int numberOfExpensesInFile = TestConstants.numberOfExpensesInFile;
        String testInputFile = TestConstants.testExpensesInputFile;
        int maxIDInExpenseFile = TestConstants.maxIDInExpenseFile;
        Expense firstExpenseInFile = new Expense(1, new DateTime(2021, 1, 10), 10, 12, "hat (on credit)");


        [Fact]
        public void ExpensesObject_New()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String messy = $"{folder}\\messy.db";
            Database.existingDatabase(messy);
            SQLiteConnection conn = Database.dbConnection;

            // Act
            Expenses expenses = new Expenses(conn);

            // Assert 
            Assert.IsType<Expenses>(expenses);
        }

        [Fact]
        public void ExpensesMethod_List_ReturnsListOfExpenses()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String messy = $"{folder}\\messy.db";
            Database.newDatabase(messy);
            SQLiteConnection conn = Database.dbConnection;


            Expenses expenses = new Expenses(conn);
            int oldSizeOfList = expenses.List().Count;
            // Act
            List<Expense> list = expenses.List();

            // Assert
            Assert.Equal(oldSizeOfList, list.Count);

        }

        [Fact]
        public void ExpensesMethod_Add()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;

            Expenses expenses = new Expenses(conn);
            string desc = "New Expense";
            DateTime date = DateTime.Now;
            int category = 1;
            double amount = 20.00;

            // Act
            List<Expense> oldExpensesList = expenses.List();
            int oldSizeOfList = expenses.List().Count;

            expenses.Add(date, category, amount, desc);
            List<Expense> expensesList = expenses.List();
            int sizeOfList = expenses.List().Count;

            // Assert
            Assert.Equal(oldSizeOfList + 1, sizeOfList);
            Assert.Equal(desc, expensesList[sizeOfList - 1].Description);

        }



        [Fact]
        public void ExpensesMethod_GetExpenseByID()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;


            Expenses expenses = new Expenses(conn);
            string desc = "New Expense";
            DateTime date = DateTime.Now;
            int category = 1;
            double amount = 20.00;

            int boo = expenses.List().Count;

            expenses.Add(date, category, amount, desc);

            int newExpenseId = expenses.List().Count - 1;

            //Act 
            Expense newExpense = expenses.GetExpenseFromId(boo + 2); //SERIOUS ISSUE WITH THE ID SKIPPING 6

            //Assert 
            Assert.Equal(amount, newExpense.Amount);

        }

        [Fact]
        public void ExpensesMethod_GetExpenseByID_InvalidIDCrashes()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;


            Expenses expenses = new Expenses(conn);

            int catID = (expenses.List()[expenses.List().Count - 1].Id) + 10;
            // Act

            Assert.Throws<ArgumentOutOfRangeException>(() => expenses.GetExpenseFromId(catID));

            // Assert


        }


        [Fact]
        public void ExpensesMethod_Delete()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String messy = $"{folder}\\messy.db";
            Database.existingDatabase(messy);
            SQLiteConnection conn = Database.dbConnection;


            Expenses expenses = new Expenses(conn);

            int IdToDelete = 1;
            string desc = "New Expense";
            DateTime date = DateTime.Now;
            int category = 1;
            double amount = 20.00;
            expenses.Add(date, category, amount, desc);

            int oldSizeOfList = expenses.List().Count;
            // Act
            expenses.Delete(IdToDelete);
            List<Expense> expensesList = expenses.List();
            int sizeOfList = expensesList.Count;

            // Assert
            Assert.Equal(oldSizeOfList - 1, sizeOfList);
            // Assert.Null(expenses.GetExpenseFromId(IdToDelete));

        }

        [Fact]
        public void ExpensesMethod_Delete_InvalidIDDoesntCrash()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String messy = $"{folder}\\messy.db";
            Database.existingDatabase(messy);
            SQLiteConnection conn = Database.dbConnection;

            Expenses expenses = new Expenses(conn);

            int IdToDelete = 1006;
            int sizeOfList = expenses.List().Count;

            // Act
            try
            {
                expenses.Delete(IdToDelete);
                Assert.Equal(sizeOfList, expenses.List().Count);
            }

            // Assert
            catch
            {
                Assert.Fail("Invalid ID causes Delete to break");
            }
        }

        [Fact]
        public void ExpensesMethod_UpdateProperties_WithMultipleNewProperties()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String messy = $"{folder}\\messy.db";
            Database.existingDatabase(messy);
            SQLiteConnection conn = Database.dbConnection;

            Expenses expenses = new Expenses(conn);

            int id = 1;
            string desc = "New Expense";
            DateTime date = DateTime.Now;
            int category = 1;
            double amount = 20.00;

            double newAmount = 10.00;
            int newCat = 2;
            string newDescr = "New Desc";

            expenses.Add(date, category, amount, desc);

            int expenseID = expenses.List()[expenses.List().Count - 1].Id;

            // Act
            expenses.UpdateProperties(expenseID, date, newCat, newAmount, newDescr);
            Expense newExpense = expenses.GetExpenseFromId(expenseID);

            // Assert 
            Assert.Equal(newAmount, newExpense.Amount);
            Assert.Equal(newCat, newExpense.Category);


        }

        [Fact]
        public void ExpensesMethod_UpdateCategory_IDDoesntExistNoCrash()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String messy = $"{folder}\\messy.db";
            Database.existingDatabase(messy);
            SQLiteConnection conn = Database.dbConnection;

            Expenses expenses = new Expenses(conn);

            String newDescr = "Presents";
            int id = 11;

            expenses.Delete(id);
            List<Expense> oldexpenses = expenses.List();

            int length = expenses.List().Count();

            // Act
            try
            {
                expenses.UpdateProperties(id, DateTime.Now, 1, 20.00, "b");
                List<Expense> newExpenses = expenses.List();
                Assert.Equal(newExpenses.Count, oldexpenses.Count);
            }
            // Assert 
            catch (Exception ex)
            {
                Assert.True(false, "Invalid Id causes Update to crash");
            }
        }
    }
}
