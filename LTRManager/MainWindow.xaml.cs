using LTRManager.Model;
using LTRManager.View;
using LTRManager.ViewModel;
using ltrModulesNet;
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
using TikApiCore.Authorization;
using TikApiModels;
using TikControls.Exceptions;
using VCore;
using VCore.AdvancedSettings;
using VCore.Logger;
using VCore.LTRManager;
using VCore.ObjectDataValues.DataEngine.Client;

namespace LTRManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LtrdControlViewModel LtrdControlViewModel;
        private App App;

        public MainWindow()
        {
            InitializeComponent();
            LtrdControlViewModel = new LtrdControlViewModel();
            DataContext = LtrdControlViewModel;
            IPAddressCrateControlInstance.SetMainWindow(this);
        }

        public void SetSetting(IEnumerable<LTRManagerIPAddress> iPAddresses, DataValueClient dataValueClient, App app, LTRManagerSettingLtrd setting, IEnumerable<LTRManagerSyncCrateSettings> cratesSettings)
        {
            //LtrdControlViewModel.IPAddressesModel = iPAddresses.ToList();
            LtrdControlViewModel.SetSettings(iPAddresses, cratesSettings.ToList(), this);
            LtrdControlViewModel.DataValueClient = dataValueClient;
            LtrdControlViewModel.SettingLtrdModel = setting;
            //LtrdControlViewModel.LTRManagerSyncCrateSettings = cratesSettings.ToList();
            App = app;
            LtrdControlViewModel.ThreadStatistics.Start();
            LtrdControlViewModel.ThreadReconnect.Start();
        }

        public void ConnectLtrd_Click(object sender, RoutedEventArgs e)
        {
            LtrdControlViewModel.ConnectLtrd();
            TreeCratesControlInstance.treeView1.ItemsSource = LtrdControlViewModel.Nodes;
        }

        private void DisconnectLtrd_Click(object sender, RoutedEventArgs e)
        {
            LtrdControlViewModel.DisconnectLtrd();
            TreeCratesControlInstance.treeView1.ItemsSource = LtrdControlViewModel.Nodes;
        }

        public void UpdateTree()
        {
            LtrdControlViewModel.UpdateTree();
            TreeCratesControlInstance.treeView1.ItemsSource = null;//LtrdControlViewModel.Nodes;
            TreeCratesControlInstance.treeView1.ItemsSource = LtrdControlViewModel.Nodes;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LtrdControlViewModel.ThreadStatistics.Abort();
            LtrdControlViewModel.ThreadReconnect.Abort();
            if (App != null) 
                App.Application_Close();
        }

        /// <summary>
        /// Настройки лтрд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingLtrd_Click(object sender, RoutedEventArgs e)
        {
            LtrdControlViewModel vm = DataContext as LtrdControlViewModel;

            _LTRNative.LTRERROR err = vm.Srvcon.IsOpened();
            if (err == _LTRNative.LTRERROR.ERROR_CHANNEL_CLOSED)
            {
                MessageBox.Show("Необходимо подключение к ltrd!", "Информация", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SettingLtrdWindow settingLtrdWindow = new SettingLtrdWindow();
            SettingLtrd setting = settingLtrdWindow.ShowDialog(LtrdControlViewModel.SettingLtrd);
            if (setting == null) return;

            //Сохранить в бд, а затем вбить настройки
            LtrdControlViewModel.SettingLtrdModel = new LTRManagerSettingLtrd(
                setting.IP,
                setting.TimeSurvey,
                setting.TimeoutForCommandExecution,
                setting.ConnectionEstablishmentTimeout,
                setting.TimeRecconect,
                setting.TransferWithoutDelay,
                setting.SizeBufferTransfer,
                setting.SizeBufferReceive);
            LtrdControlViewModel.DataValueClient.Client.Send<bool>(ApiMethods.LTRManager.INSERT_SETTING_LTRD_RECORD, LtrdControlViewModel.SettingLtrdModel);
            LtrdControlViewModel.SetSettingLtrd();
        }

        /// <summary>
        /// Сброс модуля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetModule_Click(object sender, RoutedEventArgs e)
        {
            LtrdControlViewModel vm = DataContext as LtrdControlViewModel;

            _LTRNative.LTRERROR error = vm.Srvcon.ResetModule(vm.SelectedLCard.CrateObject.Serial, vm.SelectedLCard.Slot);
            vm.CheckErrorSrv(error);
        }

        private void SyncCrateSetting_Click(object sender, RoutedEventArgs e)
        {
            LtrdControlViewModel vm = DataContext as LtrdControlViewModel;

            _LTRNative.LTRERROR err = vm.Srvcon.IsOpened();
            if (err == _LTRNative.LTRERROR.ERROR_CHANNEL_CLOSED)
            {
                MessageBox.Show("Необходимо подключение к ltrd!", "Информация", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            CrateSyncSettingsWindow window = new CrateSyncSettingsWindow();
            LTRManagerSyncCrateSettings setting = window.ShowDialog(vm.SelectedCrate.LTRManagerSyncCrateSettings, vm.DigoutArray, vm.StartMarkArray, vm.SecondMarkArray);
            if (setting == null) return;
            vm.SelectedCrate.LTRManagerSyncCrateSettings = setting;
            vm.SetSettingCrate(vm.SelectedCrate);
            vm.DataValueClient.Client.Send<bool>(ApiMethods.LTRManager.INSERT_SYNC_CRATE_SETTINGS_RECORD, setting);
        }
    }
}
