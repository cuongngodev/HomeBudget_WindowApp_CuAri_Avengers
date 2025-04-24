using Budget;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        public Presenter _p;
        public Window _categoryView,_fileSelectView, _expenseView;
        public MainWindow()
        {
            InitializeComponent();
            _p = new(this);

           
            _p.SetupPresenter();


         }

        private void OpenFileSelection(object sender, RoutedEventArgs e)
        {
            _p.OpenSelectFile();
        }

        private void OpenExpenseManagement(object sender, RoutedEventArgs e)
        {
            _p.OpenExpense();
        }

        private void OpenCategoryManagement(object sender, RoutedEventArgs e)
        {
            _p.OpenCategory();
        }

        public void DisplayError(string message)
        {
            MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void DisplayConfirmation(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButton.OK);
        }

        public void OpenWindow()
        {
            this.Show();
        }

        public void CloseWindow()
        {
            this.Close();
        }

        public void DisplayCategoryMenu()
        {
            this.Hide();
            _categoryView = new CategoryView(_p);
            _categoryView.Show();

        }

        public void DisplayExpenseMenu()
        {
            this.Hide();
            _expenseView = new ExpenseView(_p);
            _expenseView.Show();
        }

        public void DisplaySelectFileMenu()
        {
             this.Hide();
            _fileSelectView = new FileSelect(_p);
            _fileSelectView.Show();
        }

        public void CloseCategoryMenu()
        {
            _categoryView.Close();
            this.Show();
        }

        public void CloseExpenseMenu()
        {
            _expenseView.Close();
            this.Show();

        }

        public void CloseFileSelectMenu()
        {
            _fileSelectView.Close();
            this.Show();
        }
    }
}