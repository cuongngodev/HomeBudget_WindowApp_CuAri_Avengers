using System;
using Xunit;
using System.IO;
using System.Collections.Generic;
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
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
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
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
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

            expenses.Add(date,category,amount,desc);
            List<Expense> expensesList = expenses.List();
            int sizeOfList = expenses.List().Count;

            // Assert
            Assert.Equal(oldSizeOfList+1, sizeOfList);
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


            //Act
            Expense newExpense = expenses.GetExpenseFromId(1);

            //Assert 
            Assert.Equal(amount, newExpense.Amount);

        }

        [Fact]
        public void ExpensesMethod_Delete()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;

            
            Expenses expenses = new Expenses(conn);

            int IdToDelete = 1;
            string desc = "New Expense";
            DateTime date = DateTime.Now;
            int category = 1;
            double amount = 20.00;
            expenses.Add(date,category, amount, desc);

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
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
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
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
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


            // Act
            expenses.UpdateProperties(id, date, newCat, newAmount,desc);
            Expense newExpense = expenses.GetExpenseFromId(id);

            // Assert 
            Assert.Equal(newAmount,newExpense.Amount);
            Assert.Equal(newCat, newExpense.Category);


        }




    }
}
