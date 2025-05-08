using Budget;
using System.ComponentModel;
using System.Security.Policy;
using System.Text;
using System.Windows;
using static System.Windows.Application;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Data;
using System.Collections.ObjectModel;

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        public Theme theme = new Theme(0);
        
        public Presenter _p;
        public FileSelect _fileSelectView;
        public CategoryView _categoryView;
        public ExpenseView _expenseView;

        public MainWindow()
        {
            InitializeComponent();
            _p = new(this);

            _fileSelectView = new FileSelect(_p);
            _categoryView = new CategoryView(_p);
            _expenseView = new ExpenseView(_p);

            SetupWindow();
            _p.SetupPresenter();

            this.Closing += MainWindow_Closing;
        }

        private void SetupWindow()
        {
            //Fixes a WPF bug where some windows are opened on different monitors
            _expenseView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _categoryView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _fileSelectView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.BtnDefaultTheme.IsChecked = true;
        }
        /// <summary>
        /// Shows Categories for filtering usage in the mainwindow after file selected.
        /// </summary>
        /// <param name="categories"></param>
        public void ShowCategoriesOptions(List<Category> categories)
        {
            CmbFilterCategory.ItemsSource = categories;
            CmbFilterCategory.SelectedIndex = 0;
        }
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        { 
            Current.Shutdown();    
        }

        public void OpenWindow()
        {
            this.Show();
        }

        public void CloseWindow()
        {
            this.Close();
        }

        public void DisplayCategoryTypes(List<Category.CategoryType> categoryTypes)
        {
            _categoryView.SetupInputBoxes(categoryTypes);
        }

        public void DisplayCategories(List<Category> categories)
        {
            _expenseView.AddingCategory(categories);
        }

        #region OpeningWindows

        private void OpenFileSelection(object sender, RoutedEventArgs e)
        {
            _p.OpenSelectFile();
        }

        private void BtnLogExpense_Click(object sender, RoutedEventArgs e)
        {
            _p.OpenExpense();
        }

        private void BtnCreateCategory_Click(object sender, RoutedEventArgs e)
        {
            _p.OpenCategory();
        }

        public void DisplayCategoryMenu()
        {
            _categoryView.ShowView();
            this.Hide();

        }

        public void DisplayExpenseMenu()
        {
            _expenseView.OpenExpenseAdd();
            _expenseView.Show();
            this.Hide();
        }
        public void ApplyFilters()
        {
            // Clear all current collumn to avoid create more column to exsiting datagrid
            ExpensesDataGrid.Columns.Clear();

            if (!DtStartDate.SelectedDate.HasValue || !DtEndDate.SelectedDate.HasValue)
            {
                return;
            }
            DateTime start = DtPckrStartDate.SelectedDate.Value;
            DateTime end = DtPckrEndDate.SelectedDate.Value;
            bool isFilterByCategory = false;
            int catId = -1;

            bool isSummaryByMonth = ChkByMonth.IsChecked == true;
            bool isSummaryByCategory = ChkByCategory.IsChecked == true;

            if (ChkFilterByCategory.IsChecked == true)
            {
                isFilterByCategory = true;
                catId = CmbFilterCategory.SelectedIndex + 1;
            }
            
            DisplayExpenseDataGrid(start, end, isFilterByCategory, catId, isSummaryByMonth, isSummaryByCategory);   
        }
        public void DisplayExpenseDataGrid(DateTime start, DateTime end, bool isFilterByCategory, int catID, bool isSummaryByMonth, bool isSummaryByCategory)
        {
            

            if (isSummaryByCategory && isSummaryByMonth)
            {
                _p.DisplayExpenseItemsByCategoryAndMonth(start, end, isFilterByCategory, catID);
            }
            else if (isSummaryByCategory && !isSummaryByMonth)
            {
                _p.DisplayExpenseItemsByCategory(start, end, isFilterByCategory, catID);
            }
            else if (!isSummaryByCategory && isSummaryByMonth)
            {
                _p.DisplayExpenseItemsByMonth(start, end, isFilterByCategory, catID);
            }
            else
            {
                _p.DisplayExpenseItems(start, end, isFilterByCategory, catID);
            }
        }

        public void DisplayExpenseItemsGrid(List<BudgetItem> expenseList)
        {
            if (expenseList == null || !expenseList.Any())
            {
                return;
            }
            ExpensesDataGrid.ItemsSource = expenseList;
            ExpensesDataGrid.AutoGenerateColumns = false;
            
            // CategoryID column
            DataGridTextColumn categoryIdColumn = new DataGridTextColumn();

            categoryIdColumn.Header = "CategoryID";
            categoryIdColumn.Binding = new Binding("CategoryID");
            ExpensesDataGrid.Columns.Add(categoryIdColumn);

            //  Category column
            DataGridTextColumn categoryColumn = new DataGridTextColumn();
            categoryColumn.Header = "Category";
            categoryColumn.Binding = new Binding("Category");
            ExpensesDataGrid.Columns.Add(categoryColumn);

            // Date column
            DataGridTextColumn dateColumn = new DataGridTextColumn();
            dateColumn.Header = "Date";
            dateColumn.Binding = new Binding("Date");
            ExpensesDataGrid.Columns.Add(dateColumn);

            // Description column
            DataGridTextColumn descriptionColumn = new DataGridTextColumn();
            descriptionColumn.Header = "Description";
            descriptionColumn.Binding = new Binding("ShortDescription");
            ExpensesDataGrid.Columns.Add(descriptionColumn);

            // Amount column
            DataGridTextColumn amountColumn = new DataGridTextColumn();
            amountColumn.Header = "Amount";
            amountColumn.Binding = new Binding("Amount");
            ExpensesDataGrid.Columns.Add(amountColumn);

            // Balance column
            DataGridTextColumn balanceColumn = new DataGridTextColumn();
            balanceColumn.Header = "Balance";
            balanceColumn.Binding = new Binding("Balance");
            ExpensesDataGrid.Columns.Add(balanceColumn);
            
        }
        
        public void DisplayExpenseItemsByCategoryGrid(List<BudgetItemsByCategory> expenseList)
        {
            if (expenseList == null || !expenseList.Any())
            {
                return;
            }
            // table hold the data
            DataTable dataTable = new DataTable();

            // Add columns 
            dataTable.Columns.Add("Category", typeof(string));
            dataTable.Columns.Add("Total", typeof(double));
            dataTable.Columns.Add("Details", typeof(string));

            // Add rows 
            foreach (var item in expenseList)
            {
                DataRow row = dataTable.NewRow();
                row["Category"] = item.Category;
                row["Total"] = item.Total;
                row["Details"] = string.Join(",\n", item.Details.Select(d => d.ShortDescription));

                dataTable.Rows.Add(row);
            }
            // Bind the DataTable to the DataGrid
            ExpensesDataGrid.ItemsSource = dataTable.DefaultView;

            // Auto-generate columns
            ExpensesDataGrid.AutoGenerateColumns = true;


        }
        public void DisplayExpenseItemsByMonthGrid(List<BudgetItemsByMonth> expenseList)
        {
            if (expenseList == null || !expenseList.Any())
            {
                return;
            }
            // table hold the data
            DataTable dataTable = new DataTable();

            // Add columns 
            dataTable.Columns.Add("Month", typeof(string));
            dataTable.Columns.Add("Total", typeof(double));
            dataTable.Columns.Add("Descriptions", typeof(string));

            // Add rows 
            foreach (var item in expenseList)
            {
                DataRow row = dataTable.NewRow();
                row["Month"] = item.Month;
                row["Total"] = item.Total;
                row["Descriptions"] = string.Join(",\n", item.Details.Select(d => d.ShortDescription));

                dataTable.Rows.Add(row);
            }

            // Bind the DataTable to the DataGrid
            ExpensesDataGrid.ItemsSource = dataTable.DefaultView;

            // Auto-generate columns
            ExpensesDataGrid.AutoGenerateColumns = true;
        }


        public void DisplayExpenseItemmsByCategoryAndMonthGrid(List<Dictionary<string, object>> data, List<string> catNames)
        {
            ExpensesDataGrid.ItemsSource = data;
            ExpensesDataGrid.AutoGenerateColumns = false;

            // Add month column
            DataGridTextColumn monthColumn = new DataGridTextColumn()
            {
                Header = "Month",
                Binding = new Binding("[Month]")
            };
            ExpensesDataGrid.Columns.Add(monthColumn);
            // Add Category Columns
            foreach (string catName in catNames)
            {
                DataGridTextColumn catColumn = new DataGridTextColumn()
                {
                    Header = catName,
                    Binding = new Binding($"[{catName}]")
                };
                ExpensesDataGrid.Columns.Add(catColumn);
            }
            // Add Total Column
            DataGridTextColumn totalColumn = new DataGridTextColumn()
            {
                Header = "Total",
                Binding = new Binding("[Total]")
            };
            ExpensesDataGrid.Columns.Add(totalColumn);

        }

        public void DisplaySelectFileMenu()
        {
            _fileSelectView.Show();
            this.Hide();
        }


        public void DisplayCategoryMenuWithName(string name)
        {
            _categoryView.ShowView(name);
        }

        public void DisplayUpdateExpenseMenu()
        {
            _expenseView.OpenExpenseUpdate();
            _expenseView.Show();
            this.Hide();
        }

        #endregion

        #region MessageBoxes
        public void DisplayError(string message)
        {
            MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void DisplayConfirmation(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButton.OK);
        }

        public bool AskConfirmation(string message)
        {
            return MessageBox.Show(message, "", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }
        #endregion

        #region ClosingWindows

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
            // Application is ready
            _p.GetCategoriesForFilter();
            _p.SetupDefaultDate();
            _fileSelectView.Hide();
        }
        public void SetDefaultDate(DateTime start, DateTime end)
        {
            DtStartDate.SelectedDate = start;
            DtEndDate.SelectedDate = end;
        }
        public void CloseMain()
        {
            this.Hide();
        }
        #endregion

        #region ColorStuff
        public void SetupInputBoxes(List<Category> categoryList)
        {
            CmbFilterCategory.ItemsSource = categoryList;
            CmbFilterCategory.DisplayMemberPath = "Description";
            CmbFilterCategory.SelectedIndex = 0;
        }

        private void AlertColorChange(object sender, RoutedEventArgs e)
        {
            RadioButton? radioButton = sender as RadioButton;
            ChangeColorTheme(radioButton.Content.ToString());
        }

        public void ChangeColorTheme(string selectedTheme)
        {
            _p.ChangeColorTheme(selectedTheme);
        }

        public void SetDefaultTheme()
        {
            theme.CurrentTheme = Theme.Mode.DEFAULT;

            SetThemeProperties();
        }

        public void SetProtanopiaDeuteranopiaTheme()
        {
            theme.CurrentTheme = Theme.Mode.PROTAN_DEUTERAN;

            SetThemeProperties();
        }

        public void SetTritanopiaTheme()
        {
            theme.CurrentTheme = Theme.Mode.TRITAN;

            SetThemeProperties();
        }
        #endregion

        public void SetThemeProperties()
        {
            BrushConverter brushConverter = new BrushConverter();
            Background = (Brush)brushConverter.ConvertFrom(theme.BackgroundColor);
            _categoryView.Background = (Brush)brushConverter.ConvertFrom(theme.BackgroundColor);
            _expenseView.Background = (Brush)brushConverter.ConvertFrom(theme.BackgroundColor);
            _fileSelectView.Background = (Brush)brushConverter.ConvertFrom(theme.BackgroundColor);

            var collection = LogicalChildren;
            UIElementCollection mainViewCollection = GrdMainView.Children;
            UIElementCollection categoryViewCollection = _categoryView.StkCategoryView.Children;
            UIElementCollection expenseViewCollection = _expenseView.StkExpenseView.Children;
            UIElementCollection fileSelectViewCollection = _fileSelectView.StkFileSelectView.Children;

            SetThemeOnControls(mainViewCollection);
            SetThemeOnControls(categoryViewCollection);
            SetThemeOnControls(expenseViewCollection);
            SetThemeOnControls(fileSelectViewCollection);
        }

        private void SetThemeOnControls(UIElementCollection children)
        {
            foreach (Object child in children)
            {
                if (child is Panel)
                {
                    Panel panel = (Panel)child;
                    SetThemeOnControls(panel.Children);
                }
                else
                {
                    Control control = (Control)child;
                    SetThemeOnControl(control);
                }
            }
        }

        private void SetThemeOnControl(Control control)
        {
            BrushConverter brushConverter = new BrushConverter();

            switch (control)
            {
                case Label lbl:
                    lbl.Foreground = (Brush)brushConverter.ConvertFrom(theme.ElementForegroundColor);
                    break;
                case TextBox txt:
                    txt.Foreground = (Brush)brushConverter.ConvertFrom(theme.ElementForegroundColor);
                    txt.Background = (Brush)brushConverter.ConvertFrom(theme.ElementBackgroundColor);
                    break;
                case Button btn:
                    btn.Foreground = (Brush)brushConverter.ConvertFrom(theme.ElementForegroundColor);
                    btn.Background = (Brush)brushConverter.ConvertFrom(theme.ElementBackgroundColor);
                    btn.BorderBrush = (Brush)brushConverter.ConvertFrom(theme.ElementBorderColor);
                    break;
                case DatePicker dtPckr:
                    dtPckr.Foreground = (Brush)brushConverter.ConvertFrom(theme.ElementForegroundColor);
                    dtPckr.Background = (Brush)brushConverter.ConvertFrom(theme.ElementBackgroundColor);
                    dtPckr.BorderBrush = (Brush)brushConverter.ConvertFrom(theme.ElementBorderColor);
                    break;
                case GroupBox grpBx:
                    grpBx.Foreground = (Brush)brushConverter.ConvertFrom(theme.ElementForegroundColor);
                    break;
                case CheckBox chk:
                    chk.Foreground = (Brush)brushConverter.ConvertFrom(theme.ElementForegroundColor);
                    chk.Background = (Brush)brushConverter.ConvertFrom(theme.ElementBackgroundColor);
                    chk.BorderBrush = (Brush)brushConverter.ConvertFrom(theme.ElementBorderColor);
                    break;
                case RadioButton rdBtn:
                    rdBtn.Foreground = (Brush)brushConverter.ConvertFrom(theme.ElementForegroundColor);
                    rdBtn.Background = (Brush)brushConverter.ConvertFrom(theme.ElementBackgroundColor);
                    rdBtn.BorderBrush = (Brush)brushConverter.ConvertFrom(theme.ElementBorderColor);
                    break;
                case DataGrid dtGrd:
                    dtGrd.Foreground = (Brush)brushConverter.ConvertFrom(theme.ElementForegroundColor);
                    dtGrd.Background = (Brush)brushConverter.ConvertFrom(theme.ElementBackgroundColor);
                    dtGrd.BorderBrush = (Brush)brushConverter.ConvertFrom(theme.ElementForegroundColor); 
                    break;
                default:
                    control.Foreground = (Brush)brushConverter.ConvertFrom(theme.ElementForegroundColor);
                    control.Background = (Brush)brushConverter.ConvertFrom(theme.ElementBackgroundColor);
                    control.BorderBrush = (Brush)brushConverter.ConvertFrom(theme.ElementBorderColor);
                    break;
            }
        }

       

        private void CmbFilterCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();

        }

        private void chkFilterCategory_Checked(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void chkFilterCategory_Unchecked(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ChkByMonth_Checked(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ChkByMonth_Unchecked(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ChkByCategory_Checked(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ChkByCategory_Unchecked(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }
    }
}