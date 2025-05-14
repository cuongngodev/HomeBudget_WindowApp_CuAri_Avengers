using Budget;
using System.Windows.Automation;



namespace HomeBudgetWPF
{

    public class Presenter
    {
        private HomeBudget? _model;
        private ViewInterface _view;

        public Presenter(ViewInterface v)
        {
            _view = v;
        }

        public void SetupPresenter()
        {
            _view.DisplaySelectFileMenu();

        }
        public void SetupDefaultDate()
        {
            _view.SetDefaultDate(DateTime.Today.AddDays(-364), DateTime.Today);
        }
        //DB STUFF 
        #region Database
        public void SetDatabase(string db, bool isNew)
        {
            if (string.IsNullOrEmpty(db))
            {
                _view.DisplayError("You did not select location to save to.");
                return;
            }
            _model = new HomeBudget(db, isNew);

            _view.DisplayConfirmation("Db file succesfully selected");
            _view.CloseFileSelectMenu();
        }

        public void OpenSelectFile()
        {
            _view.DisplaySelectFileMenu();
            _view.DisplayCategories(_model.categories.List());
        }
        #endregion

        //Cat Stuff
        #region Category

        public void OpenCategory()
        {
            _view.DisplayCategoryMenu();
            _view.DisplayCategoryTypes(Enum.GetValues<Category.CategoryType>().ToList());
        }


        public void CloseCategory()
        {
            _view.CloseCategoryMenu();
        }
        public void GetCategoriesForFilter()
        {
            _view.ShowCategoriesOptions(_model.categories.List());
        }

        /// <summary>
        /// Adds new category that has not exisited in the database yet.
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="type"></param>
        public void CreateNewCategory(string desc, object type, bool fromExpense = false)
        {
            foreach (Budget.Category cat in _model.categories.List())
            {
                if (cat.ToString().ToLower() == desc.ToLower())
                {
                    _view.DisplayError("Category already exisited");
                    return;
                }
            }
            if (string.IsNullOrEmpty(desc))
            {
                _view.DisplayError("You did not enter description!");
                return;
            }
            try
            {
                _model.categories.Add(desc, (Budget.Category.CategoryType)type);
                _view.DisplayConfirmation("Added Succesfully!");

            }
            catch (Exception ex)
            {
                _view.DisplayError(ex.Message);
            }

            _view.CloseCategoryMenu();
            if (fromExpense)
            {
                _view.CloseMain();
            }

        }

        public bool CreateNewCategoryFromDropDown(string name)
        {
            foreach (Budget.Category cat in _model.categories.List())
            {
                if (cat.ToString() == name)
                {
                    return false;
                }
            }
            _view.DisplayCategoryMenuWithName(name);
            _view.DisplayCategoryTypes(Enum.GetValues<Category.CategoryType>().ToList());
            return true;
        }
        #endregion

        #region Expenses
        public void OpenExpense()
        {
            _view.DisplayExpenseMenu();
            _view.DisplayCategories(_model.categories.List());
        }

        public void OpenUpdateExpense(int expenseId)
        {
            _view.DisplayUpdateExpenseMenu(_model.expenses.GetExpenseFromId(expenseId));
            _view.DisplayCategories(_model.categories.List());
        }

        public void CloseExpense()
        {
            _view.CloseExpenseMenu();
        }

        public void CreateNewExpense(DateTime date, int cat, string amount, string desc)
        {
            if (cat == -1)
            {
                cat = _model.categories.List().Count() - 1;
            }


            if (string.IsNullOrEmpty(desc))
            {
                _view.DisplayError("You did not enter description!");
                return;
            }


            cat += 1;

            double newAmount = StringToDouble(amount);

            if (newAmount == -1)
            {
                _view.DisplayError("Invalid Amount,\nPlease enter a number");
                return;
            }

            if (MakeIdenticalExpense(date, cat, newAmount, desc))
                return;


            _model.expenses.Add(date, cat, newAmount, desc);

            _view.DisplayConfirmation("Added Succesfully!");
            _view.CloseExpenseMenu();
        }

        public bool MakeIdenticalExpense(DateTime date, int cat, double amount, string desc)
        {

            bool makeIdenticalExpense = false;

            List<Expense> expenses = _model.expenses.List();

            if (expenses.Count() <= 0)
                return makeIdenticalExpense;

            Budget.Expense expense = expenses[expenses.Count() - 1];

            if (expense.Date == date && expense.Category == cat && expense.Amount == amount && expense.Description == desc)
            {
                makeIdenticalExpense = _view.AskConfirmation("This expense is identical to the one you just entered\nAre you sure you want to make it again?");
            }

            return makeIdenticalExpense;
        }



        public void UpdateExpense(int id, DateTime date, int cat, string amount, string desc)
        {
            if (id == -1)
            {
                _view.DisplayError("You did not select an expense to edit!");
                return;
            }
            double newAmount = StringToDouble(amount);
            cat += 1;

            if (newAmount == -1)
            {
                _view.DisplayError("Invalid Amount,\nPlease enter a number");
                return;
            }
            _model.expenses.UpdateProperties(id, date, cat, newAmount, desc);
            _view.DisplayConfirmation("Edited Succesfully!");
            _view.CloseExpenseMenu();
        }

        public void DeleteExpense(BudgetItem budgetItem)
        {
            if (budgetItem.ExpenseID == -1)
            {
                _view.DisplayError("You did not select an expense to delete!");
                return;
            }


            _model.expenses.Delete(budgetItem.ExpenseID);
            _view.DisplayConfirmation("Deleted Succesfully!");
            _view.CloseExpenseMenu();

        }


        #endregion

        private double StringToDouble(string amount)
        {
            double result;
            if (string.IsNullOrEmpty(amount))
            {
                _view.DisplayError("You did not enter amount!");
                result = -1;
            }
            else if (!double.TryParse(amount, out result))
            {
                return -1;
            }
            return result;
        }

        public bool CheckDatePeriod(DateTime start, DateTime end)
        {
            return start <= end;

        }

        public void ChangeColorTheme(string theme)
        {
            switch (theme)
            {
                case "Default":
                    _view.SetDefaultTheme();
                    break;
                case "Protanopia / Deuteranopia":
                    _view.SetProtanopiaDeuteranopiaTheme();
                    break;
                case "Tritanopia":
                    _view.SetTritanopiaTheme();
                    break;
                default:
                    break;
            }
        }
        // Data Grid

        #region DataGrid
        
        private (List<BudgetItem>, bool matchFound) FilterItems(string searchParams, List<BudgetItem> items)
        {
            searchParams = searchParams.Trim().ToLower();
            List<BudgetItem> test = [];

            foreach (BudgetItem item in items)
            {
                if (item.ShortDescription.ToLower().Contains(searchParams))
                {
                    test.Add(item);
                }

                else if (item.Amount.ToString().Contains(searchParams))
                {
                    test.Add(item);
                }
            }

   
            return (test,test.Count == 0);

        }


        public void DisplayExpenseItems(DateTime start, DateTime end, bool filterFlag, int catID, string searchParams = "")
        {
            if (CheckDatePeriod(start, end))
            {
                List<BudgetItem> items = _model.GetBudgetItems(start, end, filterFlag, catID);

                bool notFound = false;
                if (searchParams != "")
                {
                   
                    (items, notFound) = FilterItems(searchParams,items);
                    
              
                }

                _view.DisplayExpenseItemsGrid(items);

                if (notFound)
                {
                    _view.DisplayError("No items found");
                }

            }
            else
            {
                _view.DisplayError("Start date can not excess end date");
            }
        }
        public void DisplayExpenseItemsByMonth(DateTime start, DateTime end, bool filterFlag, int catID)
        {
            if (!CheckDatePeriod(start, end))
            {
                _view.DisplayError("Start date can not excess end date");
                return;
            }
            List<BudgetItemsByMonth> items = _model.GetBudgetItemsByMonth(start, end, filterFlag, catID);
            _view.DisplayExpenseItemsByMonthGrid(items);



        }
        public void DisplayExpenseItemsByCategory(DateTime start, DateTime end, bool filterFlag, int catID)
        {
            if (!CheckDatePeriod(start, end))
            {
                _view.DisplayError("Start date can not excess end date");
                return; 
            }
            List<BudgetItemsByCategory> items = _model.GetBudgetItemsByCategory(start, end, filterFlag, catID);
            _view.DisplayExpenseItemsByCategoryGrid(items);


        }
        public void DisplayExpenseItemsByCategoryAndMonth(DateTime start, DateTime end, bool filterFlag, int catID)
        {
            if (!CheckDatePeriod(start, end))
            {
                _view.DisplayError("Start date can not excess end date");
                return; 
            }
            List<Dictionary<string, object>> itemsDict = _model.GetBudgetDictionaryByCategoryAndMonth(start, end, filterFlag, catID);
            List<string> categoryNames = _model.categories.List().Select(c => c.Description).ToList();
            _view.DisplayExpenseItemmsByCategoryAndMonthGrid(itemsDict, categoryNames);

        }


        public void DisplayExpenseDataGrid(DateTime start, DateTime end, bool isFilterByCategory, int catID, bool isSummaryByMonth, bool isSummaryByCategory)
        {
            _view.CloseSearchBar();
            if (isSummaryByCategory && isSummaryByMonth)
            {
                DisplayExpenseItemsByCategoryAndMonth(start, end, isFilterByCategory, catID);
            }
            else if (isSummaryByCategory && !isSummaryByMonth)
            {
                DisplayExpenseItemsByCategory(start, end, isFilterByCategory, catID);
            }
            else if (!isSummaryByCategory && isSummaryByMonth)
            {
                DisplayExpenseItemsByMonth(start, end, isFilterByCategory, catID);
            }
            else
            {
                _view.DisplaySearchBar();
                DisplayExpenseItems(start, end, isFilterByCategory, catID);
            }
        }        
        #endregion
    }
}
