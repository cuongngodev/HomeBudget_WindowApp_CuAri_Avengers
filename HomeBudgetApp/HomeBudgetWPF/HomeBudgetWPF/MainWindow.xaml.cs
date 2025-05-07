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
    }
}