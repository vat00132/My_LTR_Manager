using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace LTRManager.View
{
    /// <summary>
    /// Логика взаимодействия для AddIPAddressControl.xaml
    /// </summary>
    public partial class AddIPAddressWindow : Window
    {
        public AddIPAddressWindow()
        {
            InitializeComponent();
        }
        public IPAddress ShowDialog()
        {
            IPAddress ip = null;
            bool showResult = base.ShowDialog() ?? false;
            if (showResult)
            {
                try
                {
                    ip = IPAddress.Parse(IPAddressText.Text);
                }
                catch
                {

                }
            }
            return ip;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
