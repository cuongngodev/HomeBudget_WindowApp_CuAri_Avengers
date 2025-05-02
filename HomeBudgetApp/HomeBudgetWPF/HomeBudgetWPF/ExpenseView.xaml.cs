using Budget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using static HomeBudgetWPF.ViewInterface;

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for Expense.xaml
    /// </summary>
    public partial class ExpenseView : Window
    {
        public Presenter _p;

        public ExpenseView(Presenter p)
        {
            InitializeComponent();
            _p = p;
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ModifyExpense(object sender, RoutedEventArgs e)
        {

        }

        private void AddExpense(object sender, RoutedEventArgs e)
        {
            _p.OpenAddExpense();
        }
    }
}
