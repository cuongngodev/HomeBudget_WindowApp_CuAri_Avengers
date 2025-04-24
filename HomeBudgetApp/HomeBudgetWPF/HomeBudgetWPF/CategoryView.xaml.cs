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
    public partial class CategoryView : Window, ViewInterfaces.CategoryInterface
    {
        Presenter _p;
        public CategoryView(Presenter p)
        {
            InitializeComponent();
            _p = p;
        }

        public void OpenWindow()
        {
            this.Show();
            CmbCatType.ItemsSource = _p.GetAllCategories();
            CmbCatType.DisplayMemberPath = "Description";
        }

        public void CloseWindow()
        {
            this.Close();
        }



        public void DisplayCategoryType()
        {
           
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
            Object type = CmbCatType.SelectedItem;
            _p.CreateNewCategory(desc, type);
        }

        private void NewCatSubmitClicked(object sender, RoutedEventArgs e)
        {
            SendCategoryInfo();
        }

    }
}
