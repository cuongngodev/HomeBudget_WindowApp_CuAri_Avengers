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

        public Presenter(ViewInterfaces.MainViewInterface v)
        {
            _MainView = v;
        }

        #region Setup
        public void SetViews(ViewInterfaces.CategoryInterface categoryView, ViewInterfaces.FileSelectInterface fileSelectView)
        {
            _CategoryView = categoryView;
            _FileSelectView = fileSelectView;
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
        #endregion










        //public void AddExpense(DateTime date, int cat, double amount, string desc)
        //{
        //    _model.expenses.Add(date,cat,amount,desc);
        //}

    }
}
