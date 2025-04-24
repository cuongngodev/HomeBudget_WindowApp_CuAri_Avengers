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

        public void CreateNewCategory(string desc, object type)
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
        }

        public List<Budget.Category> GetAllCategories()
        {
            return _model.categories.List();
        }


        //needs to be fixed
        public void CreateNewCategoryFromDropDown(string name)
        {
            foreach (Budget.Category cat in _model.categories.List())
            {
                if (cat.ToString() == name)
                {
                    return;
                }
            }

            
            CreateNewCategory(name,Budget.Category.CategoryType.Expense); //Default for now have to ask teacher

        }
        #endregion

        #region Expenses
        public void OpenExpense()
        {
            _View.DisplayExpenseMenu();
        }

        public void CreateNewExpense(DateTime? date, int cat, string amount, string desc)
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

            double expenseAmount = StringToDouble(amount);
            if (expenseAmount<0)
            {
                // error message already sent
                return;
            }
            if(!date.HasValue)
            {
                _View.DisplayError("You did not select date!");
                return;
            }
            

            DateTime selectedDate = date.Value;
            _model.expenses.Add(selectedDate,cat + 1,StringToDouble(amount),desc);
            _View.DisplayConfirmation ("Added Succesfully!");
            _View.CloseExpenseMenu();
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
                _View.DisplayError("Invalid Amount,\nPlease enter a number");
                result = -1;
            }
            return result;
        }


    }
}
