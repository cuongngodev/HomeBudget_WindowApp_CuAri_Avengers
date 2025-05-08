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

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
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
            DgBudgetItems.Columns.Clear();

            if (!DtStartDate.SelectedDate.HasValue || !DtEndDate.SelectedDate.HasValue)
            {
                return;
            }
            DateTime start = DtStartDate.SelectedDate.Value;
            DateTime end = DtEndDate.SelectedDate.Value;
            bool isFilterByCategory = false;
            int catId = -1;
    

            bool isSummaryByMonth = ChkByMonth.IsChecked == true;
            bool isSummaryByCategory = ChkByCategory.IsChecked == true;

            if (chkFilterCategory.IsChecked == true)
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
            DgBudgetItems.ItemsSource = expenseList;
            DgBudgetItems.AutoGenerateColumns = false;
            
            // CategoryID column
            DataGridTextColumn categoryIdColumn = new DataGridTextColumn();

            categoryIdColumn.Header = "CategoryID";
            categoryIdColumn.Binding = new Binding("CategoryID");
            DgBudgetItems.Columns.Add(categoryIdColumn);

            //  Category column
            DataGridTextColumn categoryColumn = new DataGridTextColumn();
            categoryColumn.Header = "Category";
            categoryColumn.Binding = new Binding("Category");
            DgBudgetItems.Columns.Add(categoryColumn);

            // Date column
            DataGridTextColumn dateColumn = new DataGridTextColumn();
            dateColumn.Header = "Date";
            dateColumn.Binding = new Binding("Date");
            DgBudgetItems.Columns.Add(dateColumn);

            // Description column
            DataGridTextColumn descriptionColumn = new DataGridTextColumn();
            descriptionColumn.Header = "Description";
            descriptionColumn.Binding = new Binding("ShortDescription");
            DgBudgetItems.Columns.Add(descriptionColumn);
            // Amount column
            DataGridTextColumn amountColumn = new DataGridTextColumn();
            amountColumn.Header = "Amount";
            amountColumn.Binding = new Binding("Amount");
            DgBudgetItems.Columns.Add(amountColumn);

            // Balance column
            DataGridTextColumn balanceColumn = new DataGridTextColumn();
            balanceColumn.Header = "Balance";
            balanceColumn.Binding = new Binding("Balance");
            DgBudgetItems.Columns.Add(balanceColumn);
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
            foreach (BudgetItemsByCategory item in expenseList)
            {
                DataRow row = dataTable.NewRow();
                row["Category"] = item.Category;
                row["Total"] = item.Total;
                row["Details"] = string.Join(",\n", item.Details.Select(d => d.ShortDescription));

                dataTable.Rows.Add(row);
            }
            // Bind the DataTable to the DataGrid
            DgBudgetItems.ItemsSource = dataTable.DefaultView;

            // Auto-generate columns
            DgBudgetItems.AutoGenerateColumns = true;


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
            foreach (BudgetItemsByMonth item in expenseList)
            {
                DataRow row = dataTable.NewRow();
                row["Month"] = item.Month;
                row["Total"] = item.Total;
                row["Descriptions"] = string.Join(",\n", item.Details.Select(d => d.ShortDescription));

                dataTable.Rows.Add(row);
            }

            // Bind the DataTable to the DataGrid
            DgBudgetItems.ItemsSource = dataTable.DefaultView;

            // Auto-generate columns
            DgBudgetItems.AutoGenerateColumns = true;
        }


        public void DisplayExpenseItemmsByCategoryAndMonthGrid(List<Dictionary<string, object>> data, List<string> catNames)
        {
            DgBudgetItems.ItemsSource = data;
            DgBudgetItems.AutoGenerateColumns = false;

            // Add month column
            DataGridTextColumn monthColumn = new DataGridTextColumn()
            {
                Header = "Month",
                Binding = new Binding("[Month]")
            };
            DgBudgetItems.Columns.Add(monthColumn);
            // Add Category Columns
            foreach (string catName in catNames)
            {
                DataGridTextColumn catColumn = new DataGridTextColumn()
                {
                    Header = catName,
                    Binding = new Binding($"[{catName}]")
                };
                DgBudgetItems.Columns.Add(catColumn);
            }
            // Add Total Column
            DataGridTextColumn totalColumn = new DataGridTextColumn()
            {
                Header = "Total",
                Binding = new Binding("[Total]")
            };
            DgBudgetItems.Columns.Add(totalColumn);

        }

        public void DisplaySelectFileMenu()
        {
            this.DefaultThemeBtn.IsChecked = true;

            _fileSelectView.Show();
            this.Hide();
        }


        public void DisplayCategoryMenuWithName(string name)
        {
            _categoryView.ShowView(name);
        }

        public void DisplayUpdateExpenseMenu(Expense expense)
        {
            _expenseView.OpenExpenseUpdate(expense);
            _expenseView.Show();
            this.Hide();
        }

        private void AddExpense(object sender, RoutedEventArgs e)
        {
            _p.OpenExpense();
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
            ApplyFilters();
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
            BrushConverter brushConverter = new BrushConverter();

            #region Home
            this.Background = (Brush)brushConverter.ConvertFrom("#3e4444");
            this.Home_Title.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");

            this.ThemeTitleLbl.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");

            this.DefaultThemeBtn.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");
            this.ProDeuterThemeBtn.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");
            this.TritanopiaThemeBtn.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");
            #endregion

            #region FileSelect
            this._fileSelectView.Background = (Brush)brushConverter.ConvertFrom("#3e4444");
            this._fileSelectView.FileSelectPageTitle.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");

            this._fileSelectView.BtnCreateFile.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");
            this._fileSelectView.BtnCreateFile.Background = (Brush)brushConverter.ConvertFrom("#405d27");

            this._fileSelectView.BtnOpenFile.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");
            this._fileSelectView.BtnOpenFile.Background = (Brush)brushConverter.ConvertFrom("#405d27");
            #endregion

            #region Categories
            this._categoryView.Background = (Brush)brushConverter.ConvertFrom("#3e4444");
            this._categoryView.CategoryPageTitle.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");

            this._categoryView.LblCatDescription.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");
            this._categoryView.LblCatType.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");

            this._categoryView.TxtDescription.Background = (Brush)brushConverter.ConvertFrom("#405d27");
            this._categoryView.TxtDescription.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");

            this._categoryView.CmbCatType.BorderBrush = (Brush)brushConverter.ConvertFrom("#405d27");
            this._categoryView.CmbCatType.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");

            this._categoryView.BtnCreateCategory.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");
            this._categoryView.BtnCreateCategory.Background = (Brush)brushConverter.ConvertFrom("#405d27");

            this._categoryView.BtnCancelCategory.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");
            this._categoryView.BtnCancelCategory.Background = (Brush)brushConverter.ConvertFrom("#405d27");
            #endregion

            #region Expenses
            this._expenseView.Background = (Brush)brushConverter.ConvertFrom("#3e4444");
            this._expenseView.ExpensePageTitle.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");

            this._expenseView.LblExpenseDesc.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");
            this._expenseView.LblExpenseAmount.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");
            this._expenseView.LblExpenseCat.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");
            this._expenseView.LblExpenseDate.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");
            //this._expenseView.LblExpenseCredit.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");

            this._expenseView.TxtDesc.Background = (Brush)brushConverter.ConvertFrom("#405d27");
            this._expenseView.TxtDesc.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");

            this._expenseView.TxtAmount.Background = (Brush)brushConverter.ConvertFrom("#405d27");
            this._expenseView.TxtAmount.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");

            this._expenseView.CmbCategory.BorderBrush = (Brush)brushConverter.ConvertFrom("#405d27");
            this._expenseView.CmbCategory.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");

            this._expenseView.DtDate.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");
            this._expenseView.DtDate.Background = (Brush)brushConverter.ConvertFrom("#405d27");

            // this._expenseView.NewExpenseOnCreditChkBox.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");
            //this._expenseView.NewExpenseOnCreditChkBox.Background = (Brush)brushConverter.ConvertFrom("#405d27");

            //  this._expenseView.BtnLogExpense.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");
            // this._expenseView.BtnLogExpense.Background = (Brush)brushConverter.ConvertFrom("#405d27");

            this._expenseView.BtnCancelExpense.Foreground = (Brush)brushConverter.ConvertFrom("#82b74b");
            this._expenseView.BtnCancelExpense.Background = (Brush)brushConverter.ConvertFrom("#405d27");
            #endregion
        }

        //Button Text: #82b74b, #FFC107

        //Button Background: #405d27, #1E88E5, #D81B60

        //Button Outline: White, 

        //Text Color: #82b74b, #FFC107, 

        //Background: #3e4444, #004D40, 

        public void SetProtanopiaDeuteranopiaTheme()
        {
            BrushConverter brushConverter = new BrushConverter();

            #region ProDeuterHome
            this.Background = (Brush)brushConverter.ConvertFrom("#004D40");
            this.Home_Title.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this.ThemeTitleLbl.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this.DefaultThemeBtn.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this.ProDeuterThemeBtn.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this.TritanopiaThemeBtn.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            #endregion

            #region FileSelect
            this._fileSelectView.Background = (Brush)brushConverter.ConvertFrom("#004D40");
            this._fileSelectView.FileSelectPageTitle.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._fileSelectView.BtnCreateFile.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._fileSelectView.BtnCreateFile.Background = (Brush)brushConverter.ConvertFrom("#1E88E5");

            this._fileSelectView.BtnOpenFile.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._fileSelectView.BtnOpenFile.Background = (Brush)brushConverter.ConvertFrom("#1E88E5");
            #endregion

            #region Categories
            this._categoryView.Background = (Brush)brushConverter.ConvertFrom("#004D40");
            this._categoryView.CategoryPageTitle.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._categoryView.LblCatDescription.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._categoryView.LblCatType.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._categoryView.TxtDescription.Background = (Brush)brushConverter.ConvertFrom("#1E88E5");
            this._categoryView.TxtDescription.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._categoryView.CmbCatType.BorderBrush = (Brush)brushConverter.ConvertFrom("#1E88E5");
            this._categoryView.CmbCatType.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._categoryView.BtnCreateCategory.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._categoryView.BtnCreateCategory.Background = (Brush)brushConverter.ConvertFrom("#1E88E5");

            this._categoryView.BtnCancelCategory.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._categoryView.BtnCancelCategory.Background = (Brush)brushConverter.ConvertFrom("#1E88E5");
            #endregion

            #region Expenses
            this._expenseView.Background = (Brush)brushConverter.ConvertFrom("#004D40");
            this._expenseView.ExpensePageTitle.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._expenseView.LblExpenseDesc.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._expenseView.LblExpenseAmount.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._expenseView.LblExpenseCat.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._expenseView.LblExpenseDate.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            // this._expenseView.LblExpenseCredit.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._expenseView.TxtDesc.Background = (Brush)brushConverter.ConvertFrom("#1E88E5");
            this._expenseView.TxtDesc.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._expenseView.TxtAmount.Background = (Brush)brushConverter.ConvertFrom("#1E88E5");
            this._expenseView.TxtAmount.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._expenseView.CmbCategory.BorderBrush = (Brush)brushConverter.ConvertFrom("#1E88E5");
            this._expenseView.CmbCategory.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._expenseView.DtDate.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._expenseView.DtDate.Background = (Brush)brushConverter.ConvertFrom("#1E88E5");

            //this._expenseView.NewExpenseOnCreditChkBox.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            //this._expenseView.NewExpenseOnCreditChkBox.Background = (Brush)brushConverter.ConvertFrom("#1E88E5");

            //this._expenseView.BtnLogExpense.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            //this._expenseView.BtnLogExpense.Background = (Brush)brushConverter.ConvertFrom("#1E88E5");

            #endregion
        }

        public void SetTritanopiaTheme()
        {
            BrushConverter brushConverter = new BrushConverter();
            this.Background = (Brush)brushConverter.ConvertFrom("#004D40");

            #region TritanHome
            this.Home_Title.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this.ThemeTitleLbl.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this.DefaultThemeBtn.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this.ProDeuterThemeBtn.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this.TritanopiaThemeBtn.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            #endregion

            #region FileSelect
            this._fileSelectView.Background = (Brush)brushConverter.ConvertFrom("#004D40");
            this._fileSelectView.FileSelectPageTitle.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._fileSelectView.BtnCreateFile.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._fileSelectView.BtnCreateFile.Background = (Brush)brushConverter.ConvertFrom("#D81B60");

            this._fileSelectView.BtnOpenFile.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._fileSelectView.BtnOpenFile.Background = (Brush)brushConverter.ConvertFrom("#D81B60");
            #endregion

            #region Categories
            this._categoryView.Background = (Brush)brushConverter.ConvertFrom("#004D40");
            this._categoryView.CategoryPageTitle.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._categoryView.LblCatDescription.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._categoryView.LblCatType.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._categoryView.TxtDescription.Background = (Brush)brushConverter.ConvertFrom("#D81B60");
            this._categoryView.TxtDescription.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._categoryView.CmbCatType.BorderBrush = (Brush)brushConverter.ConvertFrom("#D81B60");
            this._categoryView.CmbCatType.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._categoryView.BtnCreateCategory.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._categoryView.BtnCreateCategory.Background = (Brush)brushConverter.ConvertFrom("#D81B60");

            this._categoryView.BtnCancelCategory.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._categoryView.BtnCancelCategory.Background = (Brush)brushConverter.ConvertFrom("#D81B60");
            #endregion

            #region Expenses
            this._expenseView.Background = (Brush)brushConverter.ConvertFrom("#004D40");
            this._expenseView.ExpensePageTitle.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._expenseView.LblExpenseDesc.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._expenseView.LblExpenseAmount.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._expenseView.LblExpenseCat.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._expenseView.LblExpenseDate.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            //this._expenseView.LblExpenseCredit.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._expenseView.TxtDesc.Background = (Brush)brushConverter.ConvertFrom("#D81B60");
            this._expenseView.TxtDesc.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._expenseView.TxtAmount.Background = (Brush)brushConverter.ConvertFrom("#D81B60");
            this._expenseView.TxtAmount.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._expenseView.CmbCategory.BorderBrush = (Brush)brushConverter.ConvertFrom("#D81B60");
            this._expenseView.CmbCategory.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");

            this._expenseView.DtDate.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            this._expenseView.DtDate.Background = (Brush)brushConverter.ConvertFrom("#D81B60");

            //this._expenseView.NewExpenseOnCreditChkBox.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            //this._expenseView.NewExpenseOnCreditChkBox.Background = (Brush)brushConverter.ConvertFrom("#D81B60");

            //this._expenseView.BtnLogExpense.Foreground = (Brush)brushConverter.ConvertFrom("#FFC107");
            //this._expenseView.BtnLogExpense.Background = (Brush)brushConverter.ConvertFrom("#D81B60");


            #endregion
        }
        #endregion

    

        private void SelectedDateChanged_Click(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
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


        private void MenuItemModify_Click(object sender, RoutedEventArgs e)
        {
            BudgetItem selectedItem = (BudgetItem)DgBudgetItems.SelectedItem;
            _p.OpenUpdateExpense(selectedItem.ExpenseID);

        }

        private void MenuItemCancel_Click(object sender, RoutedEventArgs e)
        {
            DgBudgetItems.SelectedItem = null;
        }
    }
}