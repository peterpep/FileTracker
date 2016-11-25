using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
using TaskScheduler;
using Forms = System.Windows.Forms;


namespace FileTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region Declarations
        private ObservableCollection<FileSystemWatcher> _listOfFileSystemWatchers = new ObservableCollection<FileSystemWatcher>();
        private ListOfFolders _trackingFolderList = new ListOfFolders();
        private readonly Forms.NotifyIcon _notifyIcon = new Forms.NotifyIcon();

        private EmailLogin _newEmailPerson = new EmailLogin();
        private PersonEmail _newEmailer;
        private EmailProcess _emailer;
        private string _emailInfo = "EmailInfo.bin";
        private bool _isEmailSaved = false;

        private string _folderInfo = "FolderInfo.bin";
        private bool _isFolderSaved = false;
        #endregion  

        public MainWindow()
        {
            InitializeComponent();
            _notifyIcon.Icon = new Icon(Resource.Hopstarter_Mac_Folders_Windows, 20, 20);
            this.StateChanged += Window_Minimized;
            _notifyIcon.MouseDoubleClick += Window_Unminimized;



            DeserializeEmail(_emailInfo); //check if saved email login exists

            //with no saved login, prompt user
            if (_isEmailSaved == false)
            {
                _newEmailPerson.ShowDialog();

                _newEmailer = new PersonEmail(_newEmailPerson.EmailUser, _newEmailPerson.EmailPass)
                {
                    SendingTo = _newEmailPerson.SendToEmail
                };

                if (_newEmailPerson.WillSerialize)
                {
                    SerializeEmail(_newEmailer, _emailInfo);
                }
            }


            _emailer = new EmailProcess(_newEmailer.EmailAddress, _newEmailer.Password, _newEmailer.SendingTo);

            DeserializeFolders(_folderInfo); //check if saved folders exist

            FolderListView.ItemsSource = _trackingFolderList;

            FolderListView.Items.Refresh();
        }

        #region FileSystemWatcher
        private FileSystemWatcher InitializeFileSystemWatcher(FolderObj newFolder)
        {
            FileSystemWatcher newFileSystemWatcher = new FileSystemWatcher
            {
                Path = newFolder.Path,
                Filter = "*.*",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime
            };
            
            newFileSystemWatcher.Created += new FileSystemEventHandler(NewFileWatcher_Handler);
            newFileSystemWatcher.Changed += new FileSystemEventHandler(NewFileWatcher_Handler);

            newFileSystemWatcher.EnableRaisingEvents = true;

            return newFileSystemWatcher;
            
        }

        public void NewFileWatcher_Handler(object sender, FileSystemEventArgs e)
        {
            //this will send email, implement later. for now will test with messagebox
            FileSystemWatcher sentBy = (FileSystemWatcher) sender;
            var DirName = System.IO.Path.GetFileName(sentBy.Path);
            _emailer.SendMail($"New Media for {DirName} is Available on Plex", $"{DirName} is available for viewing on Plex!");
        }
        #endregion

        #region UI Controls
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
            if (_isEmailSaved == true)
            {
                SerializeEmail(_newEmailer, _emailInfo);
            }
            SerializeFolders(_trackingFolderList, _folderInfo);
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

        private void Setting_OnClick(object sender, RoutedEventArgs e)
        {
            _newEmailPerson = new EmailLogin(_newEmailer);

            _newEmailPerson.ShowDialog();

            if (!string.IsNullOrEmpty(_newEmailPerson.EmailUser))
            {

            }

            try
            {
                _newEmailer = new PersonEmail(_newEmailPerson.EmailUser, _newEmailPerson.EmailPass)
                {
                    SendingTo = _newEmailPerson.SendToEmail
                };

                if (_newEmailPerson.WillSerialize)
                {
                    SerializeEmail(_newEmailer, _emailInfo);
                }

                _emailer = new EmailProcess(_newEmailer.EmailAddress, _newEmailer.Password, _newEmailer.SendingTo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Email information was not entered", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region (De)serialize
        private static void SerializeEmail(PersonEmail savedEmail, string fileName)
        {
            try
            {
                using (Stream stream = File.Open(fileName, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, savedEmail);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeserializeEmail(string fileName)
        {
            if (File.Exists(fileName) == false)
            {
                _isEmailSaved = false;
                return;
            }
            try
            {

                using (Stream stream = File.Open(fileName, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    _newEmailer = (PersonEmail)bin.Deserialize(stream);
                }

                _isEmailSaved = true;

            }
            catch (Exception)
            {
                _isEmailSaved = false;
            }
        }

        private static void SerializeFolders(ListOfFolders saveFolders, string fileName)
        {
            try
            {
                using (Stream stream = File.Open(fileName, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, saveFolders);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeserializeFolders(string fileName)
        {
            if (File.Exists(fileName) == false)
            {
                _isFolderSaved = false;
                return;
            }
            try
            {

                using (Stream stream = File.Open(fileName, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    _trackingFolderList = (ListOfFolders)bin.Deserialize(stream);
                }

                _isFolderSaved = true;

            }
            catch (Exception)
            {
                _isFolderSaved = false;
            }
        }
        #endregion
    }
}
