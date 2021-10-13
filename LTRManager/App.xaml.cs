using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TikApiCore.Authorization;
using TikApiModels;
using TikControls.Exceptions;
using TikControls.WaitWindows;
using VCore;
using VCore.AdvancedSettings;
using VCore.Logger;
using VCore.LTRManager;
using VCore.ObjectDataValues.DataEngine.Client;

namespace LTRManager
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        ConnectResult connectResult;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AdvancedSettingsEntry advSetting = new AdvancedSettingsService().LoadSettings();
            string logPath = advSetting.ApplicationLogPath;
            AppLog.InitializeLogFilePath(logPath);

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            bool doAutorun = e.Args.Any(s => s.ToLower().Equals("autorun"));

            ConnectionHelper connHelper = new ConnectionHelper();
            connectResult = connHelper.Connect(MainWindow, ClientType.LTRManager, doAutorun);
            if (!connectResult.OK)
            {
                Shutdown(1);
                return;
            }
            connectResult.AuthorizeClient.Dispose();

            //RuntimeRuleService ruleServiсe = null;
            //AppPuls.AppStateDelegate stateDelegate = () => ruleServiсe?.AppState() ?? ApplicationState.Wait;
            //MakeAppPuls(stateDelegate);

            mainWindow.Title = $"LTR Manager [{connectResult.ServerName}]";

            WaitWindow ww = new WaitWindow { Owner = mainWindow };
            IEnumerable<LTRManagerIPAddress> ipAddresses = null;
            IEnumerable<LTRManagerSyncCrateSettings> cratesSettings = null;
            LTRManagerSettingLtrd setting = null;
            DataValueClient dataValueClient = null;
            ww.RunWaitingWithoutCatchingExceptions(() =>
            {
                dataValueClient = new DataValueClient(connectResult.DataEngineClient);

                ww.SetText("Загрузка IP-адресов...");
                ipAddresses = dataValueClient.Client.DownloadCollection<LTRManagerIPAddress>(ApiMethods.LTRManager.GET_IP_ADDRESS_RECORDS);

                ww.SetText("Загрузка настроек ltrd...");
                IEnumerable<LTRManagerSettingLtrd> settings = dataValueClient.Client.DownloadCollection<LTRManagerSettingLtrd>(ApiMethods.LTRManager.GET_SETTING_LTRD_RECORDS);
                //Если нет записи с данным ip, то создать с начальными настроиками
                setting = settings.FirstOrDefault(u => u.IP == "127.0.0.1");
                if (setting == null)
                {
                    setting = new LTRManagerSettingLtrd("127.0.0.1", 15000, 5000, 5000, 3000, false, 512, 1024);
                    dataValueClient.Client.Send<bool>(ApiMethods.LTRManager.INSERT_SETTING_LTRD_RECORD, setting);
                }
                settings = dataValueClient.Client.DownloadCollection<LTRManagerSettingLtrd>(ApiMethods.LTRManager.GET_SETTING_LTRD_RECORDS);
                setting = settings.FirstOrDefault(u => u.IP == "127.0.0.1");

                ww.SetText("Загрузка настроек крейтов...");
                cratesSettings = dataValueClient.Client.DownloadCollection<LTRManagerSyncCrateSettings>(ApiMethods.LTRManager.GET_SYNC_CRATE_SETTINGS_RECORD);
            });
            if (ipAddresses == null || dataValueClient == null || setting == null)  
            {
                connectResult.DataEngineClient?.Dispose();
                connectResult.AuthorizeClient?.Dispose();
                Shutdown(1);
            }
            else
            {
                mainWindow.SetSetting(ipAddresses, dataValueClient, this, setting, cratesSettings);
                mainWindow.ConnectLtrd_Click(null, null);
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //_PulsThread?.Abort();
            Exception ex = (Exception)e.ExceptionObject;

            try
            {
                AppLog.Log(AppLogLevel.Fatal, ex.Message, ex.StackTrace);
            }
            catch { }

            ex.Show();

        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //_PulsThread?.Abort();

            try
            {
                AppLog.Log(AppLogLevel.Fatal, e.Exception.Message, e.Exception.StackTrace);
            }
            catch { }

            e.Exception?.Show();
            e.Handled = true;
        }

        public void Application_Close()
        {
            connectResult.DataEngineClient?.Dispose();
            connectResult.AuthorizeClient?.Dispose();
            Shutdown(1);
        }
    }
}
