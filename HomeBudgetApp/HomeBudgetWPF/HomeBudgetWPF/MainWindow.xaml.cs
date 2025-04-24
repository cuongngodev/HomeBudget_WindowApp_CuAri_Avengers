using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterfaces.Basic
    {
        public Presenter _p;
        public FileSelectPage fileSelectPage = new FileSelectPage();
        public ExpensePage expensePage = new ExpensePage();
        public CategoryPage categoryPage = new CategoryPage();
        
        public MainWindow()
        {
            InitializeComponent();

            ViewInterfaces.CategoryInterface categoryView = new CategoryView();
            HomePageFrame.Content = fileSelectPage;

            categoryView.RegisterPresenter(_p);
        }

        private void OpenFileSelection(object sender, RoutedEventArgs e)
        {
            HomePageFrame.Content = fileSelectPage;
        }

        private void OpenExpenseManagement(object sender, RoutedEventArgs e)
        {
            HomePageFrame.Content = expensePage;
        }

        private void OpenCategoryManagement(object sender, RoutedEventArgs e)
        {
            HomePageFrame.Content = categoryPage;
        }

        public void DisplayError(string message)
        {
            throw new NotImplementedException();
        }

        public void DisplayConfirmation(string message)
        {
            throw new NotImplementedException();
        }
    }
}