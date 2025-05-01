using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;
using static HomeBudgetWPF.ViewInterface;



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

        //DB STUFF 
        #region Database
        public void SetDatabase(string db, bool isNew)
        {
            if(string.IsNullOrEmpty(db))
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
            _view. DisplayCategories(_model.categories.List());
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

        public void OpenUpdateExpense()
        {
            _view.DisplayUpdateExpenseMenu();
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
                cat = _model.categories.List().Count() -1;
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

            if (MakeIdenticalExpense(date,cat,newAmount,desc))
                return;

            
            _model.expenses.Add(date,cat, newAmount, desc);
 
            _view.DisplayConfirmation ("Added Succesfully!");
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

        public void DeleteExpense(int id)
        {
            if (id == -1)
            {
                _view.DisplayError("You did not select an expense to delete!");
                return;
            }
            if (_view.AskConfirmation("Are you sure you want to delete this expense?"))
            {
                _model.expenses.Delete(id);
                _view.DisplayConfirmation("Deleted Succesfully!");
            }
        }

        public void UpdateExpense(int id, DateTime date, int cat, string amount, string desc)
        {
            if (id == -1)
            {
                _view.DisplayError("You did not select an expense to edit!");
                return;
            }
            double newAmount = StringToDouble(amount);
            if (newAmount == -1)
            {
                _view.DisplayError("Invalid Amount,\nPlease enter a number");
                return;
            }
            _model.expenses.UpdateProperties(id, date, cat, newAmount, desc);
            _view.DisplayConfirmation("Edited Succesfully!");
        }
        #endregion

        public double StringToDouble(string amount)
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
    }
}
