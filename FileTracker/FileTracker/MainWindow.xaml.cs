using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Forms = System.Windows.Forms;


namespace FileTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ObservableCollection<FileSystemWatcher> _listOfFileSystemWatchers = new ObservableCollection<FileSystemWatcher>();
        private ListOfFolders _trackingFolderList = new ListOfFolders();
        private readonly Forms.NotifyIcon _notifyIcon = new Forms.NotifyIcon();


        public MainWindow()
        {
            InitializeComponent();
            _notifyIcon.Icon = new Icon(Resource.Hopstarter_Mac_Folders_Windows, 20, 20);
            this.StateChanged += Window_Minimized;
            _notifyIcon.MouseDoubleClick += Window_Unminimized;


            FolderListView.ItemsSource = _trackingFolderList;
        }

        private FileSystemWatcher InitializeFileSystemWatcher(FolderObj newFolder)
        {
            FileSystemWatcher newFileSystemWatcher = new FileSystemWatcher();


            newFileSystemWatcher.Path = newFolder.Path;

            newFileSystemWatcher.Filter = "*.*";

            newFileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            

            newFileSystemWatcher.Created += new FileSystemEventHandler(NewFileWatcher_Handler);
            newFileSystemWatcher.Changed += new FileSystemEventHandler(NewFileWatcher_Handler);

            newFileSystemWatcher.EnableRaisingEvents = true;

            return newFileSystemWatcher;

        }

        public void NewFileWatcher_Handler(object sender, FileSystemEventArgs e)
        {
            //this will send email, implement later. for now will test with messagebox
            MessageBox.Show("File Created!");
        }

        public void Window_Minimized(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                _notifyIcon.Visible = true;
                this.ShowInTaskbar = true;
                this.Hide();
            }   
        }

        public void Window_Unminimized(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            _notifyIcon.Visible = false;
            this.ShowInTaskbar = true;
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            GC.Collect();
            Environment.Exit(0);
        }

        private void AddFolderBtn_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Forms.FolderBrowserDialog fbd = new Forms.FolderBrowserDialog();

                fbd.ShowDialog();

                FolderObj newfolder = new FolderObj(fbd);

                _trackingFolderList.AddFolder(newfolder);

                _listOfFileSystemWatchers.Add(InitializeFileSystemWatcher(newfolder));

                FolderListView.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        

        private void RemoveFolderBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var indexToRemove = FolderListView.SelectedIndex;
            _trackingFolderList.RemoveAt(indexToRemove);
            _listOfFileSystemWatchers.RemoveAt(indexToRemove);

            FolderListView.Items.Refresh();
        }


        
    }
}
