using Budget;
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
    public partial class MainWindow : Window, ViewInterfaces.MainViewInterface
    {
        public Presenter _p;
        
        public MainWindow()
        {
            InitializeComponent();
            _p = new(this);
            ViewInterfaces.CategoryInterface categoryView = new CategoryView(_p);
            ViewInterfaces.FileSelectInterface fileSelectView = new FileSelect(_p);

            _p.SetViews(categoryView, fileSelectView);
        }

        private void OpenFileSelection(object sender, RoutedEventArgs e)
        {
            _p.OpenSelectFile();
        }

        private void OpenExpenseManagement(object sender, RoutedEventArgs e)
        {

        }

        private void OpenCategoryManagement(object sender, RoutedEventArgs e)
        {
            _p.OpenCategory();
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