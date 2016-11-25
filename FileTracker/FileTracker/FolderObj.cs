using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forms = System.Windows.Forms;

namespace FileTracker
{
    class FolderObj
    {
        private string _name;
        private string _path;

        public FolderObj(Forms.FolderBrowserDialog fbd)
        {
            if (string.IsNullOrWhiteSpace(System.IO.Path.GetFileName(fbd.SelectedPath)))
            {
                Name = fbd.SelectedPath;
            }
            else
            {
                Name = System.IO.Path.GetFileName(fbd.SelectedPath);
            }

            Path = fbd.SelectedPath;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        
        public override string ToString()
        {
            return Name;
        }

    }
}
