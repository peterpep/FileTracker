using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler
{
    [Serializable()]
    public class PersonEmail
    {

        private string emailAddress;
        private string password;
        private string sendingTo;

        public PersonEmail(string email)
        {
            EmailAddress = email;
        }

        public PersonEmail(string email, string pass)
        {
            EmailAddress = email;
            Password = pass;
        }

        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string SendingTo
        {
            get { return sendingTo; }
            set { sendingTo = value; }
        }
    }
}
