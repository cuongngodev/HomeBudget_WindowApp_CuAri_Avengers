using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Budget;
using static HomeBudgetWPF.ViewInterfaces;

namespace HomeBudgetWPF
{
  
    public class Presenter
    {
        private HomeBudget? _model;
        private ViewInterfaces.ViewInterface _MainView;
        private ViewInterfaces.ViewInterface _CategoryView;
        private ViewInterfaces.ViewInterface _FileSelectView;
        private ViewInterfaces.ViewInterface _ExpenseView;

        public Presenter(ViewInterfaces.ViewInterface v)
        {
            _MainView = v;
        }

        #region Setup

        public void SetViews(ViewInterfaces.ViewInterface categoryView, ViewInterfaces.ViewInterface fileSelectView, ViewInterfaces.ViewInterface expenseView)
        {
            _CategoryView = categoryView;
            _FileSelectView = fileSelectView;
            _ExpenseView = expenseView;
        }
        #endregion


        //DB STUFF 
        #region Database
        public void SetDatabase(string db, bool isNew)
        {
            _model = new HomeBudget(VerifyDb(db), isNew);

            _FileSelectView.DisplayConfirmation("Db file succesfully selected");
            _FileSelectView.CloseWindow();
        }

        public string VerifyDb(string db)
        {
            return db;
        }

        public void OpenSelectFile()
        {
            _FileSelectView.OpenWindow();
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
            _CategoryView.OpenWindow();
        }

        public void CreateNewCategory(string desc, object type)
        {
            foreach (Budget.Category cat in _model.categories.List())
            {
                if (cat.ToString() == desc)
                {
                    _CategoryView.DisplayError("Error");
                }
            }
            _model.categories.Add(desc, (Budget.Category.CategoryType)type);
            _CategoryView.DisplayConfirmation("Added Succesfully!");
            _CategoryView.CloseWindow();
        }

        public List<Budget.Category> GetAllCategories()
        {
            return _model.categories.List();
        }



        public void CreateNewCategory(string name)
        {
            foreach (Budget.Category cat in _model.categories.List())
            {
                if (cat.ToString() == name)
                {
                    break;
                }
            }
            CreateNewCategory(name,Budget.Category.CategoryType.Expense); //Default for now have to ask teacher
        }
        #endregion

        #region Expenses
        public void OpenExpense()
        {
            _ExpenseView.OpenWindow();
        }

        public void CreateNewExpense(DateTime date, int cat, string amount, string desc)
        {
            _model.expenses.Add(date,cat,StringToDouble(amount),desc);
            _ExpenseView.DisplayError("Added Succesfully!");
            _ExpenseView.CloseWindow();
        }
        #endregion

        public double StringToDouble(string amount)
        {
            double result; 
            if (!double.TryParse(amount, out result))
            {
                _ExpenseView.DisplayError("Invalid Amount,\nPlease enter a number");
            }
            return result;
        }


    }
}
