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
            _fileSelectView.Hide();
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

        const string LIGHT_GREEN = "#82b74b";
        const string DARK_GREEN = "#405d27";
        const string DARK_GREY = "#3e4444";
        const string GOLDEN_YELLOW = "#FFC107";
        const string LIGHT_BLUE = "#1E88E5";
        const string TURQUOISE = "#004D40";
        const string MARROON = "#D81B60";

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




    }
}