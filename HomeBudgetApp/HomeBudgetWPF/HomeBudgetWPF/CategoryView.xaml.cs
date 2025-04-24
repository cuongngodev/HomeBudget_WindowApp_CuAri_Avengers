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
            SetupInputBoxes();
            this.Closing += MainWindow_Closing;
            _fromExpense = false;
        }

        public CategoryView(Presenter p, string name)
        {
            InitializeComponent();
            _p = p;
            SetupInputBoxes(name);
            this.Closing += MainWindow_Closing;
            _fromExpense = true;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (Application.Current.Windows.Count <= 2)
            {
                Application.Current.Shutdown();
            }
        }

        private void SetupInputBoxes(string name = "")
        {
            CmbCatType.ItemsSource = _p.GetAllCategoryTypes();
            CmbCatType.SelectedIndex = 0;

            TxtDescription.Text = name;
        }

      
        private void NewCatSubmitClicked(object sender, RoutedEventArgs e)
        {
            if (_fromExpense)
            {
                string desc = TxtDescription.Text;
                Object type = CmbCatType.SelectedItem;
                _p.CreateNewCategory(desc, type, _fromExpense);
            }
            else
            {
                string desc = TxtDescription.Text;
                Object type = CmbCatType.SelectedItem;
                _p.CreateNewCategory(desc, type);
            }
    
        }
      
    }
}
