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

using Budget;
using System.Windows.Interop;

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for FileSelect.xaml
    /// </summary>
    public partial class FileSelect : Window
    {
        static string selectedLocation = "";
        public Presenter _p;
        public FileSelect(Presenter p)
        {
            InitializeComponent();
            _p = p;
        }


        /// <summary>
        /// Selects the path where user want to save the new file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_To_Click(object sender, RoutedEventArgs e)
        {
            string newFileName = fileNameTextBox.Text.Trim();
            // if text box empty
            if (string.IsNullOrWhiteSpace(newFileName))
            {
                MessageBox.Show("Please enter a file name.", "File name empty", MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Select Location to Save Database",
                Filter = "Database files (*.db)|*.db",
                DefaultExt = ".db",
                FileName = newFileName, 
                CheckPathExists = true,
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                // Get the path where user wants to save
                selectedLocation = saveFileDialog.FileName;
                string selectedFileName = saveFileDialog.FileName;

                // Update the UI with the selected location
                fileLocation.Text = selectedLocation;

            }
        }
        /// <summary>
        /// Creates new file for new users
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateFileButton_Click(object sender, RoutedEventArgs e)
        {
            string newFileName =  fileLocation.Text;

            // Start create the db
            _p.SetDatabase(newFileName, true);
        }

        /// <summary>
        /// Opens current file for current user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Current_Database_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select File to work with";
            openFileDialog.Filter = "Database files (*.db;*.sqlite;*.mdb;*.accdb)|*.db;*.sqlite";

            if (openFileDialog.ShowDialog() == true)
            {
                // path to datbase
                string selectedFilePath = openFileDialog.FileName;
                _p.SetDatabase(selectedFilePath,false);
   
            }
        }

  
    }
}
