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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFileSelection(object sender, RoutedEventArgs e)
        {
            FileSelect fileSelect = new FileSelect();
            fileSelect.Show();
            
        }

        private void OpenExpenseManagement(object sender, RoutedEventArgs e)
        {

        }

        private void OpenCategoryManagement(object sender, RoutedEventArgs e)
        {

        }
    }
}