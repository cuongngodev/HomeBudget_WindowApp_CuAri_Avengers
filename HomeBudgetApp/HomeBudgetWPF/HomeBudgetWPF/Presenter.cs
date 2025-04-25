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
            _model = new HomeBudget(VerifyDb(db), isNew);

            _View.DisplayConfirmation("Db file succesfully selected");
            _View.CloseFileSelectMenu();
        }

        public string VerifyDb(string db)
        {
            return db;
        }

        public void OpenSelectFile()
        {
            _View.DisplaySelectFileMenu();
        }
        #endregion

        //Cat Stuff
        #region Category
        public List<Budget.Category.CategoryType> GetAllCategoryTypes()
        {
            return Enum.GetValues<Category.CategoryType>().ToList();
        }

        public void OpenCategory()
        {
            _View.DisplayCategoryMenu();
        }

        public void CreateNewCategory(string desc, object type, bool fromExpense = false)
        {
            foreach (Budget.Category cat in _model.categories.List())
            {
                if (cat.ToString() == desc)
                {
                    _View.DisplayError("Error");
                }
            }
            _model.categories.Add(desc, (Budget.Category.CategoryType)type);
            _View.DisplayConfirmation("Added Succesfully!");
            _View.CloseCategoryMenu();
            
            if (fromExpense)
            {
                _View.CloseMain();
            }
        }

        public List<Budget.Category> GetAllCategories()
        {
            return _model.categories.List();
        }


        //needs to be fixed
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

        public void CreateNewExpense(DateTime date, int cat, string amount, string desc)
        {
            if (cat == -1)
            {
                cat = _model.categories.List().Count() -1;
            }

            cat += 1;

            double newAmount = StringToDouble(amount);

            if (newAmount == -1)
            {
                _View.DisplayError("Invalid Amount,\nPlease enter a number");
                return;
            }

            if (!MakeIdenticalExpense(date,cat,newAmount,desc))
                return; //should there be a message

            
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
            if (!double.TryParse(amount, out result))
            {
                return -1;
            }
            return result;
        }


    }
}
