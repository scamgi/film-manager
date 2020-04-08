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
    /// Logica di interazione per InputWindow.xaml
    /// </summary>
    public partial class InputTextBoxDialog : MetroWindow
    {
        public InputTextBoxDialog(string title, string message)
        {
            InitializeComponent();
            this.DataContext = new { Title = title, Message = message };
        }

        public string Input
        {
            get { return inputTextBox.Text; }
        }

        private void MetroWindow_ContentRendered(object sender, EventArgs e)
        {
            inputTextBox.Focus();
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
