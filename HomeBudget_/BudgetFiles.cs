using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{

    /// <summary>
    /// Provides methods for managing file operations related to the Budget App. Its purpose is to encusre that files are valid, readable, and writable before performing any reading for saving operations
    /// </summary>
    public class BudgetFiles
    {
        private static String DefaultSavePath = @"Budget\";
        private static String DefaultAppData = @"%USERPROFILE%\AppData\Local\";

        // ====================================================================
        // verify that the name of the file exists, or set the default file, and 
        // is it readable?
        // throws System.IO.FileNotFoundException if file does not exist
        // ====================================================================
        /// <summary>
        /// Verifies that the file name exists or readable, if the file path is not provided, use default on to the App directory.
        /// </summary>
        /// <param name="FilePath">The path of file to read.</param> 
        /// <param name="DefaultFileName">The default file name to read if FilePath is null.</param> 
        /// <returns>The valid file path if file exists.</returns>
        /// <exception cref="FileNotFoundException">Throw when the file path does not work or cannot be foiund.</exception>
        /// <example>
        /// <code>
        ///    BudgetFiles budgetFiles = new BudgetFiles();
        ///    budgetFiles.VerifyReadFromFileName("../../../testbudget.txt", "testbudget.txt")
        /// </code>
        /// </example>
        public static String VerifyReadFromFileName(String FilePath, String DefaultFileName)
        {

            // ---------------------------------------------------------------
            // if file path is not defined, use the default one in AppData
            // ---------------------------------------------------------------
            if (FilePath == null)
            {
                FilePath = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath + DefaultFileName);
            }

            // ---------------------------------------------------------------
            // does FilePath exist?
            // ---------------------------------------------------------------
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("ReadFromFileException: FilePath (" + FilePath + ") does not exist");
            }

            // ----------------------------------------------------------------
            // valid path
            // ----------------------------------------------------------------
            return FilePath;

        }

        // ====================================================================
        // verify that the name of the file exists, or set the default file, and 
        // is it writable
        // ====================================================================
        /// <summary>
        /// Verifies that the file name exists or readable, if the file is null then use the default one in the App Data to set the default file to make sure path is writeable.
        /// </summary>
        /// <param name="FilePath">The path of file to write to.</param>
        /// <param name="DefaultFileName">The default file name to write if the FilePath is null.</param>
        /// <returns>The valid file path</returns>
        /// <exception cref="Exception">Thrown when the file path does not work.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided file path is invalid or the file path format is incorrect.</exception>
        /// <exception cref="PathTooLongException">Thrown when the file path exceeds the system-defined maximum length.</exception>
        /// <exception cref="System.Security.SecurityException">Thrown when the caller does not acquire permissions to access the file or directory.</exception>
        /// <exception cref="NotSupportedException">Thrown when the file path contains an unsupported format that cannot be processed.</exception>
        /// <exception cref="ArgumentNullException">Thrown if Environment variable DefaultAppData has issue such ass not set or invalid.</exception>
        ///  /// <example>
        /// <code>
        ///    BudgetFiles budgetFiles = new BudgetFiles();
        ///    budgetFiles.VerifyWriteToFileName("../../../testbudget.txt", "testbudget.txt")
        /// </code>
        /// </example>
        public static String VerifyWriteToFileName(String FilePath, String DefaultFileName)
        {
            // ---------------------------------------------------------------
            // if the directory for the path was not specified, then use standard application data
            // directory
            // ---------------------------------------------------------------
            if (FilePath == null)
            {
                // create the default appdata directory if it does not already exist
                String tmp = Environment.ExpandEnvironmentVariables(DefaultAppData);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                // create the default Budget directory in the appdirectory if it does not already exist
                tmp = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                FilePath = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath + DefaultFileName);
            }

            // ---------------------------------------------------------------
            // does directory where you want to save the file exist?
            // ... this is possible if the user is specifying the file path
            // ---------------------------------------------------------------
            String folder = Path.GetDirectoryName(FilePath);
            String delme = Path.GetFullPath(FilePath);
            if (!Directory.Exists(folder))
            {
                throw new Exception("SaveToFileException: FilePath (" + FilePath + ") does not exist");
            }

            // ---------------------------------------------------------------
            // can we write to it?
            // ---------------------------------------------------------------
            if (File.Exists(FilePath))
            {
                FileAttributes fileAttr = File.GetAttributes(FilePath);
                if ((fileAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    throw new Exception("SaveToFileException:  FilePath(" + FilePath + ") is read only");
                }
            }

            // ---------------------------------------------------------------
            // valid file path
            // ---------------------------------------------------------------
            return FilePath;

        }



    }
}
