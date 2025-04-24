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
    public partial class ExpenseView : Window, ViewInterfaces.ExpenseInterface
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

        public void OpenWindow()
        {
            this.Show();
            SetupCatCmb();
        }

        public void SendCategoryInfo()
        {
            throw new NotImplementedException();
        }

        public void SetupCatCmb()
        {
            CmbCategory.ItemsSource = _p.GetAllCategories();
            CmbCategory.DisplayMemberPath = "Description";
            CmbCategory.SelectedIndex = 0;
        }

        public void ShowConfirmation(string msg)
        {
            MessageBox.Show(msg, "Success", MessageBoxButton.OK);
        }

        public void ShowError(string msg)
        {
            MessageBox.Show(msg, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ExpenseSubmitClick(object sender, RoutedEventArgs e)
        {
            DateTime date = DtDate.SelectedDate.Value;
            int catType = CmbCategory.TabIndex;
            string desc = TxtDesc.Text;
            string amount = TxtAmount.Text;
            _p.CreateNewExpense(date, catType, amount, desc);
        }

   

    }
}
