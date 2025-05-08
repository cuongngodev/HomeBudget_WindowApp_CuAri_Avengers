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
        private bool _update;
        private Expense _updateExpense; 
        public ExpenseView(Presenter p)
        {
            InitializeComponent();
            _p = p;
            this.Closing += MainWindow_Closing;
            this._update = false;
            SetupWindow();
        }

        public void OpenExpenseAdd()
        {
            _update = false;
            SetupWindow();
        }

        public void OpenExpenseUpdate(Expense expense)
        {
            _updateExpense = expense;
            _update = true;
            SetupWindow();
        }

        private void SetupWindow()
        {
            if (_update)
            {
                this.Title = "Update Expense";
                LblExpensePageTitle.Content = "Update Expense";
                BtnSubmit.Content = "Update";
                
                TxtDesc.Text = _updateExpense.Description;
                TxtAmount.Text = _updateExpense.Amount.ToString();
                DtPckrDate.SelectedDate = _updateExpense.Date;
                CmbCategory.SelectedIndex = _updateExpense.Category - 1;

            }
            else
            {
                LblExpensePageTitle.Content = "Add Expense";
                this.Title = "Add Expense";
                BtnSubmit.Content = "Add";
                DtDate.SelectedDate = DateTime.Now;

                TxtAmount.Text = "0";
                TxtDesc.Text = "";
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void AddingCategory(List<Category> categoryList)
        {
            CmbCategory.ItemsSource = categoryList;
            CmbCategory.DisplayMemberPath = "Description";
            CmbCategory.SelectedIndex = 0;
        }

        private void ExpenseSubmitClick(object sender, RoutedEventArgs e)
        {
            DateTime date = DtPckrDate.SelectedDate.Value;
            int catType = CmbCategory.SelectedIndex;
            string catName = CmbCategory.Text;
            string desc = TxtDesc.Text;
            string amount = TxtAmount.Text;
       
            if (!_p.CreateNewCategoryFromDropDown(catName))
            {
                if (_update)
                {
                    _p.UpdateExpense(_updateExpense.Id, date, catType, amount, desc);
                }
                else
                {
                    _p.CreateNewExpense(date, catType, amount, desc);
                }

            }
        }

        private void Cancel_Expense_Click(object sender, RoutedEventArgs e)
        {
            _p.CloseExpense();
        }

        private void Delete_Expense_Click(object sender, RoutedEventArgs e)
        {
            _p.CloseExpense();
        }
    }
}
