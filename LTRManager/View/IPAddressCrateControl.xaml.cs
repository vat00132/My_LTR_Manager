using LTRManager.Model;
using LTRManager.ViewModel;
using ltrModulesNet;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TikApiModels;

namespace LTRManager.View
{
    /// <summary>
    /// Логика взаимодействия для IPAddressCrateControl.xaml
    /// </summary>
    public partial class IPAddressCrateControl : UserControl
    {
        private MainWindow MainWindow;
        public IPAddressCrateControl()
        {
            InitializeComponent();
        }

        public void SetMainWindow(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }

        /// <summary>
        /// Удаление IP адреса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteIpAddress_Click(object sender, RoutedEventArgs e)
        {
            IPAddressCrateObject selectedIPAddress = IPAddresses.SelectedItem as IPAddressCrateObject;
            LtrdControlViewModel vm = DataContext as LtrdControlViewModel;
            if (selectedIPAddress == null || vm == null) return;

            if (selectedIPAddress.On) //значит отключить
            {
                vm.DisconnectCrate(selectedIPAddress);
            }

            if (vm.DeleteIPAddress(selectedIPAddress)) 
                MainWindow.UpdateTree();
        }

        /// <summary>
        /// Добавление ip адреса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddIpAddress_Click(object sender, RoutedEventArgs e)
        {
            LtrdControlViewModel vm = DataContext as LtrdControlViewModel;

            _LTRNative.LTRERROR err = vm.Srvcon.IsOpened();
            if (err == _LTRNative.LTRERROR.ERROR_CHANNEL_CLOSED)
            {
                MessageBox.Show("Необходимо подключение к ltrd!", "Информация", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            AddIPAddressWindow window = new AddIPAddressWindow();
            IPAddress ip = window.ShowDialog();
           
            if (vm != null && ip != null) 
            {
                err = vm.Srvcon.AddIPCrate(ip, 0, false);
                if (vm.CheckErrorSrv(err)) return;
                err = vm.Srvcon.ConnectIPCrate(ip);
                if (vm.CheckErrorSrv(err)) return;
                await Task.Delay(1000);
                //Когда добавили новый ip, нужно снова получить список всех серийных номеров и добавить его
                vm.AddNewIPAddress(ip);
            }
            MainWindow.UpdateTree();
        }

        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //var track = ((ListViewItem)sender).Content as Track; //Casting back to the binded Track
            IPAddressCrateObject selectedIPAddress = IPAddresses.SelectedItem as IPAddressCrateObject;
            LtrdControlViewModel vm = DataContext as LtrdControlViewModel;
            if (selectedIPAddress == null || vm == null) return;

            if (selectedIPAddress.On) //значит отключить
            {
                vm.DisconnectCrate(selectedIPAddress);
            }
            else//Подключить
            {
                vm.ConnectCrate(selectedIPAddress);

            }
            MainWindow.UpdateTree();
        }

        /// <summary>
        /// Включить/выключить крейт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OffOn_Click(object sender, RoutedEventArgs e)
        {
            IPAddressCrateObject selectedIPAddress = IPAddresses.SelectedItem as IPAddressCrateObject;
            LtrdControlViewModel vm = DataContext as LtrdControlViewModel;
            if (selectedIPAddress == null || vm == null) return;

            if (selectedIPAddress.On) //значит отключить
            {
                vm.DisconnectCrate(selectedIPAddress);
            }
            else//Подключить
            {
                vm.ConnectCrate(selectedIPAddress);

            }
            MainWindow.UpdateTree();
        }

        private void Autoconnect_Checked(object sender, RoutedEventArgs e)
        {
            UpdateIPAddress(sender);
        }

        private void Autoconnect_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateIPAddress(sender);
        }

        private void Reconnect_Checked(object sender, RoutedEventArgs e)
        {
            UpdateIPAddress(sender);
        }

        private void Reconnect_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateIPAddress(sender);
        }
        private void UpdateIPAddress(object sender)
        {
            IPAddressCrateObject selectedIPAddress = (sender as CheckBox).DataContext as IPAddressCrateObject;
            LtrdControlViewModel vm = DataContext as LtrdControlViewModel;
            if (selectedIPAddress == null || vm == null) return;
            //Сохранить изменение
            vm.UpdateIPAddress(selectedIPAddress);
        }
    }
}
