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
            expenses.Add(date,category,amount,desc);
            List<Expense> expensesList = expenses.List();
            int sizeOfList = expenses.List().Count;

            // Assert
            Assert.Equal(1, sizeOfList);
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

            expenses.Add(date, category, amount, desc);


            //Act
            Expense newExpense = expenses.GetExpenseFromId(0);

            //Assert 
            Assert.Equal(amount, newExpense.Amount);

        }
    }
}
