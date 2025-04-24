using Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for BudgetApp.xaml
    /// </summary>
    public partial class BudgetApp : Window
    {
        public BudgetApp(HomeBudget homeBudget)
        {
            InitializeComponent();
        }



        private void OpenCategoryManagement_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenExpenseManagement_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
