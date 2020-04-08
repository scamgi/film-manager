using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace film_manager.View
{
    /// <summary>
    /// Logica di interazione per MessageBoxDialog.xaml
    /// </summary>
    public partial class MessageBoxDialog : MetroWindow
    {
        public MessageBoxDialog(string title, string message)
        {
            InitializeComponent();
            this.DataContext = new { Title = title, Message = message };
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public static bool? ShowMessage(string title, string message)
        {
            return new MessageBoxDialog(title, message).ShowDialog();
        }

        public static Task<bool?> ShowMessageAsync(string title, string message)
        {
            return Task.Run(() => ShowMessage(title, message));
        }
    }
}
