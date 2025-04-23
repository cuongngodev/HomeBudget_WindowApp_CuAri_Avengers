using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;
namespace HomeBudgetWPF
{
    internal interface ViewInterface
    {
        public void DisplayError(string message);

        public void DisplayConfirmation(string message);

    }
}
