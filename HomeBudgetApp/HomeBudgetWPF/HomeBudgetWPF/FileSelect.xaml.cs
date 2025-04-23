using System.IO;
using Microsoft.Win32;
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
    /// Interaction logic for FileSelect.xaml
    /// </summary>
    public partial class FileSelect : Window
    {
        static string selectedLocation = "";
        public FileSelect()
        {
            InitializeComponent();
        }

        private void SelectFile(object sender, RoutedEventArgs e)
        {

        }

       
        /// <summary>
        /// Selects the path where user want to save the new file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_To_Click(object sender, RoutedEventArgs e)
        {
           

            // Open dialog
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                CheckFileExists = false, // Disable file selection
                CheckPathExists = true,  // Ensure the path exists
                FileName = ""            // Set filename to empty
            };
            if (openFileDialog.ShowDialog() == true)
            {
                // get the path where user want to save
                selectedLocation = openFileDialog.FileName;    
            }
        }
        /// <summary>
        /// Creates new file for new users
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateFileButton_Click(object sender, RoutedEventArgs e)
        {
            string newFileName = FileNameTextBox.Text.Trim();

            // if text box empty
            if (string.IsNullOrWhiteSpace(newFileName))
            {
                MessageBox.Show("Please enter a file name.");
                return;
            }

            string newFile = selectedLocation + newFileName + ".db";

            // Start create the db
            MessageBox.Show("Create new DB successfully!" + newFile);
        }

        /// <summary>
        /// Opens current file for current user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Current_Database_Click(object sender, RoutedEventArgs e)
        {

        }
        private void OpenDatabase(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select File to work with";
            openFileDialog.Filter = "Database files (*.db)|*.db";

            if (openFileDialog.ShowDialog() == true)
            {
                // path to datbase
                string selectedFilePath = openFileDialog.FileName;
                MessageBox.Show("Selected DB file: " + selectedFilePath);
            }

        }
    }
}
