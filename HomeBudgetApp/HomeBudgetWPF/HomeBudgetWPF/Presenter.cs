using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;
using static HomeBudgetWPF.ViewInterfaces;

namespace HomeBudgetWPF
{
  
    public class Presenter
    {
        private HomeBudget? _model;
        private ViewInterfaces.MainViewInterface _MainView;
        private ViewInterfaces.CategoryInterface _CategoryView;
        private ViewInterfaces.FileSelectInterface _FileSelectView;
        private ViewInterfaces.ExpenseInterface _ExpenseView;

        public Presenter(ViewInterfaces.MainViewInterface v)
        {
            _MainView = v;
        }

        #region Setup
        public void SetViews(ViewInterfaces.CategoryInterface categoryView, ViewInterfaces.FileSelectInterface fileSelectView, ViewInterfaces.ExpenseInterface expenseView)
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

            _FileSelectView.ShowConfirmation("Db file succesfully selected");
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
            foreach (Budget.Category test in _model.categories.List())
            {
                if (test.ToString() == desc)
                {
                    _CategoryView.DisplayError("Error");
                }
            }


            _model.categories.Add(desc, (Budget.Category.CategoryType)type);
        }

        public List<Budget.Category> GetAllCategories()
        {
            return _model.categories.List();
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
        }
        #endregion

        public double StringToDouble(string amount)
        {
            double result; 
            if (!double.TryParse(amount, out result))
            {
                _ExpenseView.ShowError("Invalid Amount,\nPlease enter a number");
            }
            return result;
        }







        //public void AddExpense(DateTime date, int cat, double amount, string desc)
        //{
        //    _model.expenses.Add(date,cat,amount,desc);
        //}

    }
}
