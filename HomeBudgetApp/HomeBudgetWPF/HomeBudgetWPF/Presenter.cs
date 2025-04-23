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
        private readonly ViewInterface _view;

        public Presenter(ViewInterface v)
        {
            _view = v;
            _model = new HomeBudget();
        }
    }
}
