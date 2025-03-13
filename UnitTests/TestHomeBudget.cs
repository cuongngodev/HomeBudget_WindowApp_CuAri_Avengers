using System;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Budget;

namespace BudgetCodeTests
{
    [Collection("Sequential")]
    public class TestHomeBudget
    {
        string testInputFile = TestConstants.testBudgetFile;


        //// ========================================================================

        //[Fact]
        //public void HomeBudgetObject_New_NoFileSpecified()
        //{
        //    // Arrange

        //    // Act
        //    HomeBudget homeBudget  = new HomeBudget("abc.txt");

        //    // Assert 
        //    Assert.IsType<HomeBudget>(homeBudget);

        //    Assert.True(typeof(HomeBudget).GetProperty("FileName").CanWrite == false, "Filename read only");
        //    Assert.True(typeof(HomeBudget).GetProperty("DirName").CanWrite == false, "Dirname read only");
        //    Assert.True(typeof(HomeBudget).GetProperty("PathName").CanWrite == false, "Pathname read only");
        //    Assert.True(typeof(HomeBudget).GetProperty("categories").CanWrite == false, "categories read only");
        //    Assert.True(typeof(HomeBudget).GetProperty("expenses").CanWrite == false, "expenses read only");

        //    Assert.Empty(homeBudget.expenses.List());
        //    Assert.NotEmpty(homeBudget.categories.List());
        //}

        //// ========================================================================

        [Fact]
        public void HomeBudgetObject_New_WithFilename()
        {
            // Arrange
            string file = TestConstants.GetSolutionDir() + "\\" + testInputFile;
            int numExpenses = TestConstants.numberOfExpensesInFile;
            int numCategories = TestConstants.numberOfCategoriesInFile;

            // Act
            HomeBudget homeBudget = new HomeBudget(file);

            // Assert 
            Assert.IsType<HomeBudget>(homeBudget);
            Assert.Equal(numExpenses, homeBudget.expenses.List().Count);
            Assert.Equal(numCategories, homeBudget.categories.List().Count);

        }

    }
}

