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
    public partial class Category : Window, ViewInterfaces.CategoryInterface
    {
        Presenter _p;
        public Category(Presenter p)
        {
            InitializeComponent();
            _p = p;
            
        }

        public void DisplayCategoryType()
        {
            throw new NotImplementedException();
        }

        public void DisplayConfirmation(string message)
        {
            throw new NotImplementedException();
        }

        public void DisplayError(string message)
        {
            throw new NotImplementedException();
        }

        public void SendCategoryInfo()
        {
            string desc = TxtDescription.Text;
            //Budget.Category.CategoryType type = CmbCatType.Text;
            _p.CreateNewCategory(desc, type);
        }

        private void NewCatSubmitClicked(object sender, RoutedEventArgs e)
        {
            SendCategoryInfo();
        }
    }
}
