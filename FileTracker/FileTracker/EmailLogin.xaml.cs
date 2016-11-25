using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using TaskScheduler;
using MessageBox = System.Windows.MessageBox;

namespace FileTracker
{
    /// <summary>
    /// Interaction logic for EmailLogin.xaml
    /// </summary>
    [Serializable()]
    public partial class EmailLogin : MetroWindow
    {

        private string _emailUser;
        private string _emailPass;
        private string _sendToEmail;
        private bool _willSerialize = false;

        public EmailLogin()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }

        public EmailLogin(PersonEmail savedEmail)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            //load user info
            UsernameTxt.Text = savedEmail.EmailAddress;
            PasswordTxt.Password = savedEmail.Password;
            SendToTxt.Text = savedEmail.SendingTo;

            SaveLoginInfo.IsChecked = true;
        }

        public string EmailUser
        {
            get { return _emailUser; }
            private set { _emailUser = value; }
        }

        public string EmailPass
        {
            get { return _emailPass; }
            private set { _emailPass = value; }
        }

        public string SendToEmail
        {
            get { return _sendToEmail; }
            private set { _sendToEmail = value; }
        }

        public bool WillSerialize
        {
            get { return _willSerialize; }
            private set { _willSerialize = value; }
        }


        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                EmailUser = UsernameTxt.Text;
                EmailPass = PasswordTxt.Password;
                SendToEmail = SendToTxt.Text;
                if (_emailUser.Contains("@") && _sendToEmail.Contains("@"))
                {
                    Close();
                }
                else
                {
                    MessageBox.Show("Please enter valid email addresses", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveLoginInfo_Checked(object sender, RoutedEventArgs e)
        {
            WillSerialize = true;
        }

        private void EmailLogin_OnClosing(object sender, CancelEventArgs e)
        {
            try
            {
                
                EmailUser = UsernameTxt.Text;
                EmailPass = PasswordTxt.Password;
                SendToEmail = SendToTxt.Text;
                if (_emailUser.Contains("@") && _sendToEmail.Contains("@"))
                {
                    //Close();
                }
                else
                {
                    MessageBox.Show("Please enter valid email addresses", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    
                    //return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExitBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
