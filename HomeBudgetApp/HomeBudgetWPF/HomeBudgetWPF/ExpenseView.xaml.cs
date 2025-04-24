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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static HomeBudgetWPF.ViewInterfaces;

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for Expense.xaml
    /// </summary>
    public partial class ExpenseView : Window, ViewInterfaces.ViewInterface
    {
        public Presenter _p;

        public ExpenseView(Presenter p)
        {
            InitializeComponent();
            _p = p;
        }

        public void CloseWindow()
        {
            this.Close();
        }

        public void DisplayConfirmation(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButton.OK);
        }

        public void DisplayError(string message)
        {
            MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void OpenWindow()
        {
            this.Show();
            SetupCatCmb();
        }

        public void SetupCatCmb()
        {
            CmbCategory.ItemsSource = _p.GetAllCategories();
            CmbCategory.DisplayMemberPath = "Description";
            CmbCategory.SelectedIndex = 0;
        }


        private void ExpenseSubmitClick(object sender, RoutedEventArgs e)
        {
            DateTime date = DtDate.SelectedDate.Value;
            int catType = CmbCategory.SelectedIndex;
            string catName = CmbCategory.Text;
            string desc = TxtDesc.Text;
            string amount = TxtAmount.Text;

            _p.CreateNewCategory(catName);
            _p.CreateNewExpense(date, catType, amount, desc);
        }
    }
}
