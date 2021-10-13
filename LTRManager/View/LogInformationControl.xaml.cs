using LTRManager.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LTRManager.View
{
    /// <summary>
    /// Логика взаимодействия для LogInformationControl.xaml
    /// </summary>
    public partial class LogInformationControl : UserControl
    {
        public LogInformationControl()
        {
            InitializeComponent();
        }

        private void ClearList(object sender, RoutedEventArgs e)
        {
            LtrdControlViewModel vm = DataContext as LtrdControlViewModel;
            if (vm == null) return;
            vm.LogInformation.Clear();
        }
    }
}
