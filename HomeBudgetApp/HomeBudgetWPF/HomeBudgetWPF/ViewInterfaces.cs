using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudgetWPF
{
    public class ViewInterfaces
    {
        public interface Basic
        {
            public void DisplayError(string message);

            public void DisplayConfirmation(string message);

        }
    }
}
