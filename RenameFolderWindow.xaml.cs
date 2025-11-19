using System.Windows;
using System.Windows.Input;

namespace PromptHandbook
{
    public partial class RenameFolderWindow : Window
    {
        public string NewFolderName { get; set; }

        public RenameFolderWindow(string currentName)
        {
            InitializeComponent();
            NewFolderName = currentName;
            DataContext = this;
            FolderNameTextBox.Focus();
            FolderNameTextBox.SelectAll();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewFolderName))
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Folder name cannot be empty.",
                              "Invalid Name",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void FolderNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OKButton_Click(sender, e);
            }
        }
    }
}