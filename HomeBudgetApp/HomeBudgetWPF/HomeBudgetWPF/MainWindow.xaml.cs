using Budget;
using System.ComponentModel;
using System.Security.Policy;
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
        public Window _fileSelectView;
        public CategoryView _categoryView;
        public ExpenseView _expenseView;
        public MainWindow()
        {
            InitializeComponent();
            _p = new(this);

            _fileSelectView = new FileSelect(_p);
            _categoryView = new CategoryView(_p);
            _expenseView = new ExpenseView(_p);

            _expenseView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _categoryView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _fileSelectView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
           
            _p.SetupPresenter();

            this.Closing += MainWindow_Closing;

        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        { 
            Application.Current.Shutdown();    
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
            _categoryView.ShowView();
            this.Hide();
            
        }

        public void DisplayExpenseMenu()
        {
            _expenseView.Show();
            this.Hide();
        }

        public void DisplaySelectFileMenu()
        {
            _fileSelectView.Show();
            this.Hide();
        }

        public void CloseCategoryMenu()
        {
            this.Show();
            _categoryView.Hide();
        }

        public void CloseExpenseMenu()
        {
            this.Show();
            _expenseView.Hide();
        }

        public void CloseFileSelectMenu()
        {
            this.Show();
            _fileSelectView.Hide();
        }

        public void DisplayCategoryMenuWithName(string name)
        {
            _categoryView.ShowView(name);

        }

        public void CloseMain()
        {
            this.Hide();
        }

        public void DisplayCategoryTypes(List<Category.CategoryType> categoryTypes)
        {
            _categoryView.SetupInputBoxes(categoryTypes);
        }

        public void DisplayCategories(List<Category> categories)
        {
            _expenseView.SetupInputBoxes(categories);
        }

        public bool AskConfirmation(string message)
        {
            return MessageBox.Show(message, "", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

    }
}