using Budget;
using System.Data.SQLite;

namespace BudgetCodeTests
{
    [Collection("Sequential")]
    public class TestCategories
    {
        public int numberOfCategoriesInFile = TestConstants.numberOfCategoriesInFile;
        public String testInputFile = TestConstants.testDBInputFile;
        public int maxIDInCategoryInFile = TestConstants.maxIDInCategoryInFile;
        Category firstCategoryInFile = TestConstants.firstCategoryInFile;
        int IDWithSaveType = TestConstants.CategoryIDWithSaveType;

        // ========================================================================

        [Fact]
        public void CategoriesObject_New()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;

            // Act
            Categories categories = new Categories(conn, true);

            // Assert 
            Assert.IsType<Categories>(categories);
        }

        // ========================================================================

        [Fact]
        public void CategoriesObject_New_CreatesDefaultCategories()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;

            // Act
            Categories categories = new Categories(conn, true);

            // Assert 
            Assert.False(categories.List().Count == 0, "Non zero categories");

        }

        // ========================================================================

        [Fact]
        public void CategoriesMethod_ReadFromDatabase_ValidateCorrectDataWasRead()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String existingDB = $"{folder}\\{TestConstants.testDBInputFile}";
            Database.existingDatabase(existingDB);
            SQLiteConnection conn = Database.dbConnection;

            // Act
            Categories categories = new Categories(conn, false);
            List<Category> list = categories.List();
            Category firstCategory = list[0];

            // Assert
            Assert.Equal(numberOfCategoriesInFile, list.Count);
            Assert.Equal(firstCategoryInFile.Id, firstCategory.Id);
            Assert.Equal(firstCategoryInFile.Description, firstCategory.Description);

        }

        // ========================================================================

        [Fact]
        public void CategoriesMethod_List_ReturnsListOfCategories()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\{TestConstants.testDBInputFile}";
            Database.existingDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, false);

            // Act
            List<Category> list = categories.List();

            // Assert
            Assert.Equal(numberOfCategoriesInFile, list.Count);

        }


        // ========================================================================

        [Fact]
        public void CategoriesMethod_Add()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, false);
            string descr = "New Category";
            Category.CategoryType type = Category.CategoryType.Income;

            // Act
            categories.Add(descr, type);
            List<Category> categoriesList = categories.List();
            int sizeOfList = categories.List().Count;

            // Assert
            Assert.Equal(numberOfCategoriesInFile + 1, sizeOfList);
            Assert.Equal(descr, categoriesList[sizeOfList - 1].Description);

        }

        // ========================================================================

        [Fact]
        public void CategoriesMethod_Delete()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, false);
            int IdToDelete = 3;

            // Act
            categories.Delete(IdToDelete);
            List<Category> categoriesList = categories.List();
            int sizeOfList = categoriesList.Count;

            // Assert
            Assert.Equal(numberOfCategoriesInFile - 1, sizeOfList);
            Assert.False(categoriesList.Exists(e => e.Id == IdToDelete), "correct Category item deleted");

        }

        // ========================================================================

        [Fact]
        public void CategoriesMethod_Delete_InvalidIDDoesntCrash()
        {
            // Arrange
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messyDB";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, false);
            int IdToDelete = 9999;
            int sizeOfList = categories.List().Count;

            // Act
            try
            {
                categories.Delete(IdToDelete);
                Assert.Equal(sizeOfList, categories.List().Count);
            }

            // Assert
            catch
            {
                Assert.True(false, "Invalid ID causes Delete to break");
            }
        }



        // ========================================================================

        [Fact]
        public void CategoriesMethod_GetCategoryFromId()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\{TestConstants.testDBInputFile}";
            Database.existingDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, false);
            int catID = 15;

            // Act
            Category category = categories.GetCategoryFromId(catID);

            // Assert
            Assert.Equal(catID, category.Id);

        }


        [Fact]
        public void CategoriesMethod_GetCategoryFromId_InvalidIDCrashes()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\{TestConstants.testDBInputFile}";
            Database.existingDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, false);

            int catID = (categories.List()[categories.List().Count - 1].Id) + 10;

            // Act   // Assert

            Assert.Throws<ArgumentOutOfRangeException>(() => categories.GetCategoryFromId(catID));


        }

        // ========================================================================

        [Fact]
        public void CategoriesMethod_SetCategoriesToDefaults()
        {

            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;

            // Act
            Categories categories = new Categories(conn, true);
            List<Category> originalList = categories.List();

            // modify list of categories
            categories.Delete(1);
            categories.Delete(2);
            categories.Delete(3);
            categories.Add("Another one ", Category.CategoryType.Credit);

            //"just double check that initial conditions are correct");
            Assert.NotEqual(originalList.Count, categories.List().Count);

            // Act
            categories.SetCategoriesToDefaults();

            // Assert
            Assert.Equal(originalList.Count, categories.List().Count);
            foreach (Category defaultCat in originalList)
            {
                Assert.True(categories.List().Exists(c => c.Description == defaultCat.Description && c.Type == defaultCat.Type));
            }
        }

        // ========================================================================

        [Fact]
        public void CategoriesMethod_UpdateProperties_WithOneNewProperty()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, true);

            String newDescr = "Presents";
            int id = 1;
            Category.CategoryType newCatType = Category.CategoryType.Income;

            // Act
            Category category = categories.GetCategoryFromId(id);
            Category.CategoryType catType = category.Type;
            categories.UpdateProperties(id, newDescr, catType); //I'm thinking that maybe we might want an overload for Update
            Category newCat = categories.GetCategoryFromId(id);
            // Assert 
            //checking if the description updated
            Assert.Equal(newDescr, newCat.Description);

            //checking if the cat didn't change
            //Assert.Equal(catType, category.Type);

        }

        [Fact]
        public void CategoriesMethod_UpdateProperties_WithMultipleNewProperties()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, true);

            String newDescr = "Presents";
            int id = 11;
            Category.CategoryType newCatType = Category.CategoryType.Savings;

            // Act
            categories.UpdateProperties(id, newDescr, newCatType);
            Category category = categories.GetCategoryFromId(id);

            // Assert 
            Assert.Equal(newDescr, category.Description);
            Assert.Equal(newCatType, category.Type);


        }

        [Fact]
        public void CategoriesMethod_UpdateCategory_IDDoesntExistNoCrash()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, true);

            String newDescr = "Presents";
            int id = 11;
            Category.CategoryType catType = Category.CategoryType.Income;

            int defaultId = 0;
            string defaultDesc = "";
            Category.CategoryType defaultCat = Category.CategoryType.Income;

            categories.Delete(id);
            Category category = new Category(defaultId, defaultDesc, defaultCat);
            List<Category> oldCategories = categories.List();

            int length = categories.List().Count();

            // Act
            try
            {
                categories.UpdateProperties(id, newDescr, catType);
                List<Category> newCategories = categories.List();
                Assert.Equal(newCategories, oldCategories);
            }
            // Assert 
            catch (Exception ex)
            {
                Assert.True(false, "Invalid Id causes Update to crash");
            }
        }

        [Fact]
        public void CategoriesMethod_UpdateCategory_InvalidIdNoCrash()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, true);

            String newDescr = "Presents";
            int id = -11;
            Category.CategoryType catType = Category.CategoryType.Income;

            int defaultId = 0;
            string defaultDesc = "";
            Category.CategoryType defaultCat = Category.CategoryType.Income;

            categories.Delete(id);
            Category category = new Category(defaultId, defaultDesc, defaultCat);

            int length = categories.List().Count();

            // Act
            try
            {
                categories.UpdateProperties(id, newDescr, catType);
            }
            // Assert 
            catch (Exception ex)
            {
                Assert.True(false, "Invalid Id causes Update to crash");
            }

            Assert.Equal(length, categories.List().Count());
        }

        [Fact]
        public void CategoriesMethod_UpdateCategory_NullValuesNoCrash()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories categories = new Categories(conn, true);

            String newDescr = "Presents";
            int id = 10;
            Category.CategoryType catType = Category.CategoryType.Income;

            int defaultId = 0;
            string defaultDesc = "";
            Category.CategoryType defaultCat = Category.CategoryType.Income;

            categories.Delete(id);
            Category category = new Category(defaultId, defaultDesc, defaultCat);

            int length = categories.List().Count();

            // Act
            try
            {
                categories.UpdateProperties(id, newDescr, catType);
                category = categories.GetCategoryFromId(id);
            }
            // Assert 
            catch (Exception ex)
            {
                Assert.True(false, "Null Id causes Update to crash");
            }

            Assert.Equal(length, categories.List().Count());
        }

        [Fact]
        public void Categories_DeleteCategoryWithExpenses_ShouldFail()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            string newDB = $"{folder}\\testDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;

            Categories categories = new Categories(conn, true);
            Expenses expenses = new Expenses(conn);

            categories.Add("New Cat", Category.CategoryType.Savings);

            expenses.Add(DateTime.Now, 1, 20.00, "New Expense");

            List<Expense> expenseList = expenses.List();

            int catId = categories.List()[categories.List().Count() - 1].Id;
            // Act and Assert
            Assert.Throws<Exception>(() => categories.Delete(catId));

        }

    }
}

