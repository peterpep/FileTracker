using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileTracker
{
    [Serializable()]
    public class ListOfFolders : ObservableCollection<FolderObj>
    {
        public void AddFolder(FolderObj folderToAdd)
        {
            foreach (var folder in this)
            {
                if (folder.Path == folderToAdd.Path)
                {
                    MessageBox.Show("This folder is already being tracked", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
            }
            this.Add(folderToAdd);
        }

        public void RemoveTask(int IndexOfFolder)
        {
            this.RemoveAt(IndexOfFolder);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
