using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;

namespace HomeBudgetWPF
{
  
    public class Presenter
    {
        private HomeBudget? _model;
        private readonly ViewInterfaces _view;

        public Presenter(ViewInterfaces v, HomeBudget ?model)
        {
            _view = v;
            _model = model;
        }
       
        public List<Budget.Category> GetAllCategories()
        {
            var boo = _model.categories.List();
            return boo; 
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

        public void CreateNewCategory(string desc, Budget.Category.CategoryType type)
        {
            _model.categories.Add(desc, type);
        }
        
        public void AddExpense(DateTime date, int cat, double amount, string desc)
        {
            _model.expenses.Add(date,cat,amount,desc);
        }

    }
}
