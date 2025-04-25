using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Budget;
using static HomeBudgetWPF.ViewInterface;



namespace HomeBudgetWPF
{
  
    public class Presenter
    {
        private HomeBudget? _model;
        private ViewInterface _View;

        public Presenter(ViewInterface v)
        {
            _View = v;
        }

        public void SetupPresenter()
        {
            _View.DisplaySelectFileMenu();
        }

        //DB STUFF 
        #region Database
        public void SetDatabase(string db, bool isNew)
        {
            if(string.IsNullOrEmpty(db))
            {
                _View.DisplayError("You did not select location to save to.");
                return;
            }
            _model = new HomeBudget(db, isNew);

            _View.DisplayConfirmation("Db file succesfully selected");
            _View.CloseFileSelectMenu();
        }

        public void OpenSelectFile()
        {
            _View.DisplaySelectFileMenu();
            _View.DisplayCategories(_model.categories.List());
        }
        #endregion

        //Cat Stuff
        #region Category

        public void OpenCategory()
        {
            _View.DisplayCategoryMenu();
            _View.DisplayCategoryTypes(Enum.GetValues<Category.CategoryType>().ToList());
        }


        public void CloseCategory()
        {
            _View.CloseCategoryMenu();
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
                    _View.DisplayError("Category already exisited");
                    return;
                }
            }
            if (string.IsNullOrEmpty(desc))
            {
                _View.DisplayError("You did not enter description!");
                return;
            }
            _model.categories.Add(desc, (Budget.Category.CategoryType)type);
            _View.DisplayConfirmation("Added Succesfully!");
            _View.CloseCategoryMenu();
            
            if (fromExpense)
            {
                _View.CloseMain();
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
            _View.DisplayCategoryMenuWithName(name);
            return true;
        }
        #endregion

        #region Expenses
        public void OpenExpense()
        {
            _View.DisplayExpenseMenu();
        }
        public void CloseExpense()
        {
            _View.CloseExpenseMenu();
        }

        public void CreateNewExpense(DateTime date, int cat, string amount, string desc)
        {
            if (cat == -1)
            {
                cat = _model.categories.List().Count() -1;
            }

            
            if (string.IsNullOrEmpty(desc))
            {
                _View.DisplayError("You did not enter description!");
                return;
            }

            
            cat += 1;

            double newAmount = StringToDouble(amount);

            if (newAmount == -1)
            {
                _View.DisplayError("Invalid Amount,\nPlease enter a number");
                return;
            }

            if (!MakeIdenticalExpense(date,cat,newAmount,desc))
                return;

            
            _model.expenses.Add(date,cat, newAmount, desc);

            _View.DisplayConfirmation ("Added Succesfully!");
            _View.CloseExpenseMenu();
        }

        public bool MakeIdenticalExpense(DateTime date, int cat, double amount, string desc)
        {
            bool makeIdenticalExpense = false;

            Budget.Expense expense = _model.expenses.List()[_model.expenses.List().Count() - 1];

            if (expense.Date == date && expense.Category == cat && expense.Amount == amount && expense.Description == desc)
            {
                makeIdenticalExpense = _View.AskConfirmation("This expense is identical to the one you just entered\nAre you sure you want to make it again?");
            }

            return makeIdenticalExpense;
        }
        #endregion

        public double StringToDouble(string amount)
        {
            double result; 
            if (string.IsNullOrEmpty(amount))
            {
                _View.DisplayError("You did not enter amount!");
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
                    _View.SetDefaultTheme();
                    break;
                case "Protanopia / Deuteranopia":
                    _View.SetProtanopiaDeuteranopiaTheme();
                    break;
                case "Tritanopia":
                    _View.SetTritanopiaTheme();
                    break;
                default:
                    break;
            }
        }
    }
}
