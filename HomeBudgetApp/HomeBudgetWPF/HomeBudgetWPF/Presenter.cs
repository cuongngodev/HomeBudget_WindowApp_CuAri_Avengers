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
        private readonly HomeBudget _model;
        private readonly ViewInterfaces _view;

        public Presenter(ViewInterfaces v)
        {
            _view = v;
            _model = new HomeBudget("../../../../../../HomeBudgetAPI/UnitTests/messyDB.db",false);
        }
       
        public List<Category> GetAllCategories()
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

        

    }
}
