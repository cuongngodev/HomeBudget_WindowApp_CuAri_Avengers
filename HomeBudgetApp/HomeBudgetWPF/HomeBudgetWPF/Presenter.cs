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
       
        public void SetViews(ViewInterfaces.CategoryInterface categoryView, ViewInterfaces.FileSelectInterface fileSelectView)
        {
            _CategoryView = categoryView;
            _FileSelectView = fileSelectView;
        }

        public List<Budget.Category> GetAllCategories()
        {
            var boo = _model.categories.List();
            return boo; 
        }

        public void OpenCategorySelectFile()
        {
            _FileSelectView.OpenWindow();
        }

        public void OpenCategory()
        {
            _CategoryView.OpenWindow();
        }

        public string GetCategory(int id)
        {
           
            if (id < 0)
                throw new ArgumentException();
            if (id > _model.categories.List().Count)
                throw new ArgumentOutOfRangeException();

            return _model.categories.List()[id].ToString();
            
        }

        public void SetDatabase(string db, bool isNew)
        {
            _model = new HomeBudget(VerifyDb(db), isNew);
            
        }

        public string VerifyDb(string db)
        {
            return db;
        }

        public void CreateNewCategory(string desc, object type)
        {
            foreach (Budget.Category test in _model.categories.List())
            {
                if (test.ToString() == desc){
                    _CategoryView.DisplayError("Error");
                }
            }
                

            _model.categories.Add(desc, (Budget.Category.CategoryType) type);
        }
        
        public void AddExpense(DateTime date, int cat, double amount, string desc)
        {
            _model.expenses.Add(date,cat,amount,desc);
        }

    }
}
