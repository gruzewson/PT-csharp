using System;
using System.IO;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace lab2
{
    public partial class CreateItemDialog : Window
    {
        private readonly string parentDirectory;

        public CreateItemDialog(string parentDirectory)
        {
            InitializeComponent();
            this.parentDirectory = parentDirectory;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string itemName = itemNameTextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(itemName))
                {
                    MessageBox.Show("Please enter a valid name for the item.");
                    return;
                }

                if (fileRadioButton.IsChecked == true)
                {
                    string pattern = @"^[a-zA-Z0-9_~\-]{1,8}\.(txt|php|html)$";
                    if (!System.Text.RegularExpressions.Regex.IsMatch(itemName, pattern))
                    {
                        MessageBox.Show(
                            "Invalid file name format. File name must be between 1 and 8 characters long and have a valid extension (txt, php, or html).");
                        return;
                    }
                }

                string itemPath = Path.Combine(parentDirectory, itemName);
                if (Directory.Exists(itemPath) || File.Exists(itemPath))
                {
                    MessageBox.Show("Item with the same name already exists in this directory.");
                    return;
                }

                FileAttributes attributes = 0;

                if (readOnlyCheckBox.IsChecked == true)
                {
                    attributes |= FileAttributes.ReadOnly;
                }
                if (archiveCheckBox.IsChecked == true)
                {
                    attributes |= FileAttributes.Archive;
                }
                if (systemCheckBox.IsChecked == true)
                {
                    attributes |= FileAttributes.System;
                }
                if (hiddenCheckBox.IsChecked == true)
                {
                    attributes |= FileAttributes.Hidden;
                }

                if (fileRadioButton.IsChecked == true)
                {
                    using (File.Create(itemPath)) { }
                }
                else if (directoryRadioButton.IsChecked == true)
                {
                    Directory.CreateDirectory(itemPath);
                }

                File.SetAttributes(itemPath, attributes);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating item: {ex.Message}");
            }
        }

        private void FileRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            directoryRadioButton.IsChecked = false;
        }

        private void DirectoryRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            fileRadioButton.IsChecked = false;
        }
    }
}
