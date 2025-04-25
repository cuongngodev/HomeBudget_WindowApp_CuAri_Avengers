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
           
            _model.expenses.Add(date,cat + 1,StringToDouble(amount),desc);
            _View.DisplayConfirmation ("Added Succesfully!");
            _View.CloseExpenseMenu();
        }
        #endregion

        public double StringToDouble(string amount)
        {
            double result; 
            if (!double.TryParse(amount, out result))
            {
                _View.DisplayError("Invalid Amount,\nPlease enter a number");
            }
            return result;
        }


    }
}
