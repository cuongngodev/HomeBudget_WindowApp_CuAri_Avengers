using System;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Budget;
using System.Data.SQLite;

namespace BudgetCodeTests
{
    [Collection("Sequential")]
    public class TestHomeBudget
    {
        string testInputNewFile = "MessyDB.db";
        string testIntPutExistingDb = "messy.db";



        // ========================================================================

        [Fact]
        public void HomeBudgetObject_New_BadFile()
        {
            // Arrange

            // Act
            HomeBudget homeBudget = new HomeBudget("abc.txt");

            // Assert 
            Assert.IsType<HomeBudget>(homeBudget);

            Assert.True(typeof(HomeBudget).GetProperty("FileName").CanWrite == false, "Filename read only");
            Assert.True(typeof(HomeBudget).GetProperty("DirName").CanWrite == false, "Dirname read only");
            Assert.True(typeof(HomeBudget).GetProperty("PathName").CanWrite == false, "Pathname read only");
            Assert.True(typeof(HomeBudget).GetProperty("categories").CanWrite == false, "categories read only");
            Assert.True(typeof(HomeBudget).GetProperty("expenses").CanWrite == false, "expenses read only");

            Assert.Empty(homeBudget.expenses.List());
            Assert.NotEmpty(homeBudget.categories.List());
        }
    
        // ========================================================================

        [Fact]
        public void HomeBudgetObject_With_New_Database()
        {
            // Arrange
            string file = TestConstants.GetSolutionDir() + "\\" + testInputNewFile;
            int numDefaultCategories = 16;
          
            // Act
            HomeBudget homeBudget = new HomeBudget(file, true);

            // Assert 
            Assert.Empty(homeBudget.expenses.List());
            Assert.IsType<HomeBudget>(homeBudget);
            Assert.Equal(numDefaultCategories, homeBudget.categories.List().Count);
        }

        [Fact]
        public void HomeBudgetObject_With_ExistingDatabase()
        {
            // Arrange
            string file = TestConstants.GetSolutionDir() + "\\" + testIntPutExistingDb;

            // Act
            HomeBudget homeBudget = new HomeBudget(file);
            int numExpenses = homeBudget.GetBudgetItems(null,null,false,1).Count;
            // Assert 
            //Assert.NotEmpty(homeBudget.expenses.List());
            Assert.Equal(homeBudget.expenses.List().Count(), numExpenses);
            Assert.IsType<HomeBudget>(homeBudget);

        }

    }
}

