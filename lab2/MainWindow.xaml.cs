using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;

namespace lab2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MenuItem openMenuItem;
    private MenuItem createMenuItem;
    public MainWindow()
    {
        InitializeComponent();
        openMenuItem = new MenuItem();
        createMenuItem = new MenuItem();
    }

    private void Open_OnClick(object sender, RoutedEventArgs e)
    {
        var dlg = new FolderBrowserDialog() { Description = "Select directory to open" };
        var result = dlg.ShowDialog();

        if (result == System.Windows.Forms.DialogResult.OK)
        {
            string selectedPath = dlg.SelectedPath;
            treeView.Items.Clear();
            
            var root = new TreeViewItem
            {
                Header = Path.GetFileName(selectedPath),
                Tag = selectedPath
            };
            
            PopulateTreeView(selectedPath, root);
            treeView.Items.Add(root);
        }
    }

    private void PopulateTreeView(string directory, TreeViewItem parentItem)
    {
        try
        {
            string[] directories = Directory.GetDirectories(directory);

            foreach (string dir in directories)
            {
                var directoryItem = new TreeViewItem
                {
                    Header = Path.GetFileName(dir),
                    Tag = dir
                };
                
                PopulateTreeView(dir, directoryItem);
                parentItem.Items.Add(directoryItem);
            }
            
            string[] files = Directory.GetFiles(directory);

            foreach (string file in files)
            {
                var fileItem = new TreeViewItem
                {
                    Header = Path.GetFileName(file),
                    Tag = file
                };
                parentItem.Items.Add(fileItem);
            }
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show("Error populating tree view: " + ex.Message);
        }
    }

    private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (treeView.SelectedItem != null)
        {
            string path = (string)((TreeViewItem)treeView.SelectedItem).Tag;
            
            FileAttributes attributes = File.GetAttributes(path);
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                File.SetAttributes(path, attributes & ~FileAttributes.ReadOnly);
            }
            
            if (((TreeViewItem)treeView.SelectedItem).Parent is TreeViewItem parentItem)
            {
                parentItem.Items.Remove(treeView.SelectedItem);
            }
            else
            {
                treeView.Items.Remove(treeView.SelectedItem);
            }
            
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                else if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting item: {ex.Message}");
            }
        }
    }

    private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        TreeViewItem selectedItem = (TreeViewItem)e.NewValue;
        if (selectedItem != null)
        {
            string path = (string)selectedItem.Tag;
            if (File.Exists(path))
            {
                openMenuItem.IsEnabled = true;
                createMenuItem.IsEnabled = false;
            }
            else if (Directory.Exists(path))
            {
                openMenuItem.IsEnabled = false;
                createMenuItem.IsEnabled = true;
            }
            
            FileAttributes attributes = File.GetAttributes(path);
            
            string status = "";
            status += (attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly ? "r" : "-";
            status += (attributes & FileAttributes.Archive) == FileAttributes.Archive ? "a" : "-";
            status += (attributes & FileAttributes.System) == FileAttributes.System ? "s" : "-";
            status += (attributes & FileAttributes.Hidden) == FileAttributes.Hidden ? "h" : "-";
                
            statusText.Text = status;
        }
        else
        {
            openMenuItem.IsEnabled = false;
            createMenuItem.IsEnabled = false;
            statusText.Text = "";
        }
    }
    
  
    private void Exit_OnClick(object sender, RoutedEventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }
    
    private void CreateMenuItem_Click(object sender, RoutedEventArgs e)
    {
        TreeViewItem selectedItem = treeView.SelectedItem as TreeViewItem;

        if (selectedItem != null && selectedItem.Tag is string selectedPath)
        {
            if (Directory.Exists(selectedPath))
            {
                CreateItemDialog dialog = new CreateItemDialog(selectedPath);
                if (dialog.ShowDialog() == true)
                {
                    selectedItem.Items.Clear();
                    PopulateTreeView((string)selectedItem.Tag, selectedItem);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Cannot create item. Selected item is not a folder.");
            }
        }
        else
        {
            System.Windows.MessageBox.Show("Please select a folder to create item.");
        }
    }
    
    private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (treeView.SelectedItem != null)
        {
            string path = (string)((TreeViewItem)treeView.SelectedItem).Tag;

            if (File.Exists(path))
            {
                try
                {
                    string content = File.ReadAllText(path);
                    OpenFileContentWindow(content);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Cannot open item. Selected item is not a file.");
            }
        }
    }

    private void OpenFileContentWindow(string content)
    {
        Window fileContentWindow = new Window
        {
            Title = "File Content",
            Width = 400,
            Height = 300
        };

        ScrollViewer scrollViewer = new ScrollViewer();
        TextBlock textBlock = new TextBlock
        {
            Text = content,
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(10)
        };
        scrollViewer.Content = textBlock;

        fileContentWindow.Content = scrollViewer;
        fileContentWindow.ShowDialog();
    }

}