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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for Category.xaml
    /// </summary>
    public partial class CategoryView : Window
    {
        private Presenter _p;
        private bool _fromExpense;
        public CategoryView(Presenter p)
        {
            InitializeComponent();
            _p = p;
            this.Closing += MainWindow_Closing;
            _fromExpense = false;
        }

        public void ShowView()
        {
            this.Show();
        }

        public void ShowView(string name)
        {
            this.Show();
            _fromExpense = true;
            TxtDescription.Text = name;
        }



        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void SetupInputBoxes(List<Category.CategoryType> categoryTypes)
        {
            CmbCatType.ItemsSource = categoryTypes;
            CmbCatType.SelectedIndex = 0;
        }

      
        private void NewCatSubmitClicked(object sender, RoutedEventArgs e)
        {

            string desc = TxtDescription.Text;
            Object type = CmbCatType.SelectedItem;
            


            if (_fromExpense)
            {   
                _p.CreateNewCategory(desc, type, _fromExpense);
            }
            else
            {
                _p.CreateNewCategory(desc, type);
            }
        }

        private void Cancel_Category_Click(object sender, RoutedEventArgs e)
        {
            _p.CloseCategory();
        }
    }
}
