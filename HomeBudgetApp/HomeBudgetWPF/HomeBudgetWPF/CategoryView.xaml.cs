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
    /// Interaction logic for Category.xaml
    /// </summary>
    public partial class CategoryView : Window, ViewInterfaces.ViewInterface
    {
        Presenter _p;
        public CategoryView(Presenter p)
        {
            InitializeComponent();
            _p = p;
            SetupCmb();
        }

        public void OpenWindow()
        {
            this.ShowDialog();
        }

        private void SetupCmb()
        {
            CmbCatType.ItemsSource = _p.GetAllCategoryTypes();
            CmbCatType.SelectedIndex = 0;
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
           MessageBox.Show(message,"ERROR",MessageBoxButton.OK, MessageBoxImage.Error); 
        }

        private void NewCatSubmitClicked(object sender, RoutedEventArgs e)
        {
            string desc = TxtDescription.Text;
            Object type = CmbCatType.SelectedItem;
            _p.CreateNewCategory(desc, type);
        }

    }
}
