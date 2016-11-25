using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TaskScheduler
{
    public static class CustomCommands
    {

        public static readonly RoutedUICommand Send = new RoutedUICommand
            (
                "Send",
                "Send",
                typeof(CustomCommands)
            );

        public static readonly RoutedUICommand AddNewFolder = new RoutedUICommand
            (
                "AddNewFolder",
                "AddNewFolder",
                typeof(CustomCommands)
            );

    }
}
