using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using LTRManager.Model;
using ltrModulesNet;
using TikApiModels;
using VCore.LTRManager;
using VCore.ObjectDataValues.DataEngine.Client;

namespace LTRManager.ViewModel
{
    public class LtrdControlViewModel : BindableBase
    {
        /// <summary>
        /// Для подключения к сервису
        /// </summary>
        private bool _IsConnectLtrd;
        public bool IsConnectLtrd { get { return _IsConnectLtrd; } set { SetProperty(ref _IsConnectLtrd, value); } }
        /// <summary>
        /// Для отключения от сервиса
        /// </summary>
        private bool _IsDisconnectLtrd;
        public bool IsDisconnectLtrd { get { return _IsDisconnectLtrd; } set { SetProperty(ref _IsDisconnectLtrd, value); } }
        /// <summary>
        /// Сервис
        /// </summary>
        private ltrsrvcon _Srvcon;
        public ltrsrvcon Srvcon { get { return _Srvcon; } set { SetProperty(ref _Srvcon, value); } }
        /// <summary>
        /// Список логов
        /// </summary>
        private ObservableCollection<LogInformation> _LogInformation;
        public ObservableCollection<LogInformation> LogInformation { get { return _LogInformation; } set { SetProperty(ref _LogInformation, value); } }
        /// <summary>
        /// Дерево объектов ltrd
        /// </summary>
        private ObservableCollection<NodeObject> _Nodes;
        public ObservableCollection<NodeObject> Nodes { get { return _Nodes; } set { SetProperty(ref _Nodes, value); } }
        /// <summary>
        /// Выбранный лтрд сервер
        /// </summary>
        private LtrdServerObject _SelectedLtrdServer;
        public LtrdServerObject SelectedLtrdServer { get { return _SelectedLtrdServer; } set { SetProperty(ref _SelectedLtrdServer, value); } }
        /// <summary>
        /// Выбранный крейт
        /// </summary>
        private CrateObject _SelectedCrate;
        public CrateObject SelectedCrate { get { return _SelectedCrate; } set { SetProperty(ref _SelectedCrate, value); } }
        /// <summary>
        /// выбранная карта
        /// </summary>
        private LCardObject _SelectedLCard;
        public LCardObject SelectedLCard { get { return _SelectedLCard; } set { SetProperty(ref _SelectedLCard, value); } }
        /// <summary>
        /// Для сброса модуля
        /// </summary>
        private bool _SelectModule;
        public bool SelectModule { get { return _SelectModule; } set { SetProperty(ref _SelectModule, value); } }
        /// <summary>
        /// Для настройки синхронизации
        /// </summary>
        private bool _SelectCrate;
        public bool SelectCrate { get { return _SelectCrate; } set { SetProperty(ref _SelectCrate, value); } }
        /// <summary>
        /// Массив названий для enum DIGOUT
        /// </summary>
        public string[] DigoutArray = new string[6] 
        { 
            "Постоянный низкий уровень (логический 0)",
            "Поятоянный высокий уровень (логическая 1)",
            "Трансляция сигнала со входа DIGIN1",
            "Трансляция сигнала со входа DIGIN2",
            "Трансляция метки СТАРТ",
            "Трансляция метки СЕКУНДА"
        };
        /// <summary>
        /// Массив названий для enum StartMark
        /// </summary>
        public string[] StartMarkArray = new string[5] 
        { 
            "Отключено",
            "По фронту сигнала на входе DIGIN1",
            "По спаду сигнала на входе DIGIN1",
            "По фронту сигнала на входе DIGIN2",
            "По спаду сигнала на входе DIGIN2"
        };
        /// <summary>
        /// Массив названий для enum SecondMark
        /// </summary>
        public string[] SecondMarkArray = new string[6]
        {
            "Отключено",
            "По внутреннему таймеру крейта",
            "По фронту сигнала на входе DIGIN1",
            "По спаду сигнала на входе DIGIN1",
            "По фронту сигнала на входе DIGIN2",
            "По спаду сигнала на входе DIGIN2"
        };
        /// <summary>
        /// Список IP адресов
        /// </summary>
        private ObservableCollection<IPAddressCrateObject> _IPAddressCrates;
        public ObservableCollection<IPAddressCrateObject> IPAddressCrates { get { return _IPAddressCrates; } set { SetProperty(ref _IPAddressCrates, value); } }
        /// <summary>
        /// Визуализация статистики для лтрд
        /// </summary>
        private System.Windows.Visibility _StatisticsLtrdVisibility;
        public System.Windows.Visibility StatisticsLtrdVisibility { get { return _StatisticsLtrdVisibility; } set { SetProperty(ref _StatisticsLtrdVisibility, value); } }
        /// <summary>
        /// Визуализация для крейта
        /// </summary>
        private System.Windows.Visibility _StatisticsCrateVisibility;
        public System.Windows.Visibility StatisticsCrateVisibility { get { return _StatisticsCrateVisibility; } set { SetProperty(ref _StatisticsCrateVisibility, value); } }
        /// <summary>
        /// Визуализация для карты(слота)
        /// </summary>
        private System.Windows.Visibility _StatisticsLCardVisibility;
        public System.Windows.Visibility StatisticsLCardVisibility { get { return _StatisticsLCardVisibility; } set { SetProperty(ref _StatisticsLCardVisibility, value); } }

        /// <summary>
        /// Список IP адресов из бд
        /// </summary>
        public List<LTRManagerIPAddress> IPAddressesModel;

        /// <summary>
        /// Настройки лтрд из БД
        /// </summary>
        public LTRManagerSettingLtrd SettingLtrdModel;

        /// <summary>
        /// Настройки всех крейтов из БД
        /// </summary>
        public List<LTRManagerSyncCrateSettings> LTRManagerSyncCrateSettings;

        /// <summary>
        /// Настройки лтрд для отображения
        /// </summary>
        private SettingLtrd _SettingLtrd;
        public SettingLtrd SettingLtrd { get { return _SettingLtrd; } set { SetProperty(ref _SettingLtrd, value); } }

        /// <summary>
        /// Для общения с цодом
        /// </summary>
        public DataValueClient DataValueClient;
        private MainWindow MainWindow;

        public Thread ThreadStatistics;
        public Thread ThreadReconnect;

        public LtrdControlViewModel()
        {
            _IsConnectLtrd = true;
            _IsDisconnectLtrd = false;
            _SelectCrate = false;
            _SelectModule = false;
            _LogInformation = new ObservableCollection<LogInformation>();
            _SelectedLtrdServer = null;
            _SelectedCrate = null;
            _SelectedLCard = null;
            _SettingLtrd = null;
            LTRManagerSyncCrateSettings = null;
            _IPAddressCrates = new ObservableCollection<IPAddressCrateObject>();
            IPAddressesModel = new List<LTRManagerIPAddress>();
            _StatisticsLtrdVisibility = System.Windows.Visibility.Collapsed;
            _StatisticsCrateVisibility = System.Windows.Visibility.Collapsed;
            _StatisticsLCardVisibility = System.Windows.Visibility.Collapsed;
            /* для передачи управляющих команд устанавливаем управляющее соединение
             * с сервисом (ltrd/LtrServer). Для управляющего соединения существует
             * специальный класс ltrsrvcon с набором функций ltrapi, которые относятся
             * к командом управления сервисом */
            _Srvcon = new ltrsrvcon();
            _Nodes = new ObservableCollection<NodeObject>();
            ThreadStatistics = new Thread(new ThreadStart(UpdateStatisticsInfo));
            ThreadReconnect = new Thread(new ThreadStart(CheckReconnectCrates));
            //ThreadStatistics.Start();
        }

        private void CheckReconnectCrates()
        {
            while (true)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        //Проверить подключения и настройки в крейтах
                        if (IsDisconnectLtrd)
                            CheckingCrates();
                    }
                    catch(Exception ex)
                    {
                        AddLogInformation("Ошибка", ex.Message);
                    }
                });
                Thread.Sleep(1000);
            }
        }

        private void UpdateStatisticsInfo()
        {
            while (true)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        if (SelectedLtrdServer != null)
                        {

                        }
                        if (SelectedCrate != null && SelectedCrate.Ltrcrate.Open(SelectedCrate.CSN) == _LTRNative.LTRERROR.OK)
                        {
                            CheckErrorCrate(SelectedCrate.UpdateStatisticsInfo());
                        }
                        if (SelectedLCard != null)
                        {
                            if (SelectedLCard.LtrXXXapi != null)
                                CheckErrorLCard(SelectedLCard.UpdateStatisticsInfo(), SelectedLCard);
                        }
                    }
                    catch(Exception ex)
                    {
                        AddLogInformation("Ошибка", ex.Message);
                    }
                    
                });
                Thread.Sleep(500);
            }
        }
        private void CheckingCrates()
        {
            //Из всего списка адресов взять только с меткой On
            //Если есть флаги reconnect, то переподключить
            //Проверить настройки, если слетели, то снова забить
            //Настройки вбивать каждый раз, когда подключаем

            bool update = false;
            var addressesOn = IPAddressCrates.Where(u => u.On).ToList();//Включенные
            
            //Получить подключенные у сервера, и если каких-то нет, то сделать off
            /* получаем список серийных номеров крейтов, которые подключены */
            string[] crateSerialList;
            _LTRNative.LTRERROR err = Srvcon.GetCrates(out crateSerialList);
            //Удалять из списка on если они подключены
            foreach (var ser in crateSerialList)
            {
                var item = addressesOn.FirstOrDefault(u => u.Serial.Trim('\0') == ser.Trim('\0'));
                if (item != null)
                    addressesOn.Remove(item);
            }
            //В списке On остались не подключенные крейты
            foreach (var addr in addressesOn)
            {
                //Убрать выбранные узлы
                if (SelectedCrate != null && SelectedCrate.Serial == addr.Serial)
                {
                    StatisticsCrateVisibility = System.Windows.Visibility.Collapsed;
                    StatisticsLCardVisibility = System.Windows.Visibility.Collapsed;
                    SelectModule = false;
                    SelectCrate = false;
                    SelectedCrate = null;
                    SelectedLCard = null;
                }
                addr.On = false;
                AddLogInformation("Ошибка", "Подключение разорвано! : " + addr.Serial);
                update = true;
            }
            //Взять список переподключения и с отключенными крейтами, а затем их подключить
            var addressesRec = IPAddressCrates.Where(u => u.Reconnect && !u.On).ToList();//С переподключением
            foreach (var addr in addressesRec)
            {
                //Проверить нужно ли подключение
                //var addressesOff= IPAddressCrates.Where(u => !u.On).ToList();
                //if (addressesOff.FirstOrDefault(u => u.Serial == addr.Serial) != null) 
                if (ConnectCrate(addr)) update = true;
                Thread.Sleep(100);
            }
            if (update)
                MainWindow.UpdateTree();

            /*bool update = false;
            //Проверка включенных на настройки
            var workingCrates = IPAddressCrates.Where(u => u.On = true).ToList();
            //Крейты из дерева
            var cratesFromTree = Nodes[0].GetCratesObjects(Nodes[0]);
            foreach (var crateW in workingCrates)
            {
                var crateT = cratesFromTree.FirstOrDefault(u => u.Serial == crateW.Serial);
                if (crateT == null)
                {
                    //crateT.IPAddressCrate.On = false;
                    crateW.On = false;
                    AddLogInformation("Ошибка", "Подключение разорвано! : " + crateW.Serial);
                    MainWindow.UpdateTree();
                }
                //else if (crateT.Ltrcrate.IsOpened() == _LTRNative.LTRERROR.ERROR_CHANNEL_CLOSED)
                else if (crateT.Ltrcrate.Open(crateT.CSN) != _LTRNative.LTRERROR.OK)
                {
                    crateT.IPAddressCrate.On = false;
                    AddLogInformation("Ошибка", "Подключение разорвано! : " + crateT.Serial);
                    MainWindow.UpdateTree();
                }
            }

            workingCrates = IPAddressCrates.Where(u => u.Reconnect == true && u.On == false).ToList();
            
            //Подключить крейт и забить настройки
            foreach (var crateW in workingCrates)
            {
                if (ConnectCrate(crateW)) update = true;
                //crateW.On = true;
                //Задать настройки
            }
            if (update)
                MainWindow.UpdateTree();
                //UpdateTree();*/
        }
        public void SetSettings(IEnumerable<LTRManagerIPAddress> addresses, List<LTRManagerSyncCrateSettings> cratesSettings, MainWindow window)
        {
            MainWindow = window;
            LTRManagerSyncCrateSettings = cratesSettings;
            IPAddressesModel = addresses.ToList();
            foreach (var addr in addresses)
            {
                _IPAddressCrates.Add(new IPAddressCrateObject(IPAddress.Parse(addr.IP), false, addr.Serial.Trim('\0'), addr.Autoconnect, addr.Reconnect));
            }
        }

        public void ConnectLtrd()
        {
            string srv_ver = "0";
            LogInformation log = new LogInformation("", "");
            _LTRNative.LTRERROR err = Srvcon.IsOpened();
            if (err == _LTRNative.LTRERROR.ERROR_CHANNEL_CLOSED)
                err = Srvcon.Open();
            //Когда подключились, нужно задать настройки
            if (!SetSettingLtrd())
            {
                Srvcon.Close();
                return;
            }
            if (err != _LTRNative.LTRERROR.OK)
            {
                log = new LogInformation("Ошибка", "Не удалось установить связь с сервисом. Ошибка " + err + " : " + "ltrsrvcon.GetErrorString(err)");
            }
            else
            {
                err = Srvcon.GetServerVersion(out srv_ver);
                if (err != _LTRNative.LTRERROR.OK)
                {
                    log = new LogInformation("Ошибка", "Не удалось установить связь с сервисом. Ошибка " + err + " : " + ltrsrvcon.GetErrorString(err));
                }
                else
                {
                    log = new LogInformation("Информация", "Установлено соединение с сервисом, версия сервиса " + srv_ver);
                    IsConnectLtrd = false;
                    IsDisconnectLtrd = true;
                }
            }

            LogInformation.Add(log);

            //Создание дерева объектов
            LoadTreeObjects(srv_ver);
        }
        public bool SetSettingLtrd()
        {
            //Присвоить в модель для отображения
            SettingLtrd = new SettingLtrd(SettingLtrdModel);
            //Задать настройки в лтрд запросом
            _LTRNative.LTRERROR error = Srvcon.SetServerParameter((uint)_LTRNative.en_LTRD_Params.LTRD_PARAM_ETH_CRATE_POLL_TIME, (uint)SettingLtrdModel.TimeSurvey, sizeof(int));
            if (CheckErrorSrv(error)) return false;
            error = Srvcon.SetServerParameter((uint)_LTRNative.en_LTRD_Params.LTRD_PARAM_ETH_CRATE_CTLCMD_TOUT, (uint)SettingLtrdModel.TimeoutForCommandExecution, sizeof(int));
            if (CheckErrorSrv(error)) return false;
            error = Srvcon.SetServerParameter((uint)_LTRNative.en_LTRD_Params.LTRD_PARAM_ETH_INTF_CHECK_TIME, (uint)SettingLtrdModel.ConnectionEstablishmentTimeout, sizeof(int));
            if (CheckErrorSrv(error)) return false;
            error = Srvcon.SetServerParameter((uint)_LTRNative.en_LTRD_Params.LTRD_PARAM_ETH_CRATE_RECONNECT_TIME, (uint)SettingLtrdModel.TimeRecconect, sizeof(int));
            if (CheckErrorSrv(error)) return false;
            uint delay = 0;
            if (SettingLtrdModel.TransferWithoutDelay) delay = 1;
            error = Srvcon.SetServerParameter((uint)_LTRNative.en_LTRD_Params.LTRD_PARAM_ETH_SEND_NODELAY, delay, sizeof(int));
            if (CheckErrorSrv(error)) return false;
            error = Srvcon.SetServerParameter((uint)_LTRNative.en_LTRD_Params.LTRD_PARAM_MODULE_SEND_BUF_SIZE, (uint)SettingLtrdModel.SizeBufferTransfer * 1024, sizeof(int));
            if (CheckErrorSrv(error)) return false;
            error = Srvcon.SetServerParameter((uint)_LTRNative.en_LTRD_Params.LTRD_PARAM_MODULE_RECV_BUF_SIZE, (uint)SettingLtrdModel.SizeBufferReceive * 1024, sizeof(int));
            if (CheckErrorSrv(error)) return false;

            //Проверка буферов
            //uint n = 0;
            //error = Srvcon.GetServerParameter((uint)_LTRNative.en_LTRD_Params.LTRD_PARAM_MODULE_RECV_BUF_SIZE, out n, sizeof(int));

            return true;
        }
        private void LoadTreeObjects(string srv_ver)
        {
            NodeObject nodeLtrd = new NodeObject(Srvcon, srv_ver, null);

            /* получаем список серийных номеров крейтов, которые подключены */
            string[] crateSerialList;
            LogInformation log = new LogInformation("", "");
            _LTRNative.LTRERROR err = Srvcon.GetCrates(out crateSerialList);

            if (err != _LTRNative.LTRERROR.OK)
            {
                log = new LogInformation("Ошибка", "Не удалось получить список крейтов. Ошибка " + err + " : " + ltrsrvcon.GetErrorString(err));
                LogInformation.Add(log);
                return;
            }

            log = new LogInformation("Информация", "Найдено крейтов: " + crateSerialList.Length);
            LogInformation.Add(log);

            foreach (string csn in crateSerialList)
            {
                /* устанавливаем соединение с каждым крейтом. для этого
                 * используем класс ltrcrate, который реализует подмножество
                 * функций из ltrapi для работы с одним крейтом */
                ltrcrate crate = new ltrcrate();
                err = crate.Open(csn);
                if (err != _LTRNative.LTRERROR.OK)
                {
                    log = new LogInformation("Ошибка", "Не удалось установить связь с крейтом " + csn + ". Ошибка " + err + " : " + ltrsrvcon.GetErrorString(err));
                    LogInformation.Add(log);
                }
                else
                {
                    log = new LogInformation("Информация", "Крейт " + crate.Type + ", S/N: " + crate.Serial + ", интерфейс " + crate.Interface);
                    LogInformation.Add(log);
                    //Найти настройки из бд, если нет, то сделать их
                    LTRManagerSyncCrateSettings crateSetting = LTRManagerSyncCrateSettings.FirstOrDefault(u => u.Serial == csn.Trim('\0'));
                    if (crateSetting == null)
                    {
                        //Создать новые настройки и задать их
                        crateSetting = new LTRManagerSyncCrateSettings(
                            csn.Trim('\0'), 
                            false, 
                            (int)DIGOUT.Start, 
                            (int)DIGOUT.Second, 
                            (int)StartMark.Off, 
                            (int)SecondMark.InternalTimer, 
                            false);
                        //Сохранить в бд
                        DataValueClient.Client.Send<bool>(ApiMethods.LTRManager.INSERT_SYNC_CRATE_SETTINGS_RECORD, crateSetting);
                        //Занести в коллекцию
                        LTRManagerSyncCrateSettings.Add(crateSetting);
                        AddLogInformation("Информация", "Не было настроек в БД! Настройки заданы по умолчанию : " + csn.Trim('\0'));
                    }

                    //Найти среди подключений и задать подключение
                    IPAddressCrateObject ipObject = IPAddressCrates.FirstOrDefault(u => u.Serial == csn.Trim('\0'));
                    if (ipObject == null)
                    {
                        AddLogInformation("Ошибка", "Данного крейта нет в БД : " + csn);
                        continue;
                    }
                    
                    ipObject.On = true;
                    //Создать узел крейта
                    NodeObject nodeCrate = new NodeObject(crate, nodeLtrd, csn, ipObject, crateSetting);

                    //Внести настройки
                    if (!SetSettingCrate(nodeCrate.Object as CrateObject)) continue;

                    /* получаем список модулей */
                    _LTRNative.MODULETYPE[] mids;
                    err = crate.GetModules(out mids);
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                        log = new LogInformation("Ошибка", "Не удалось получить список модулей. Ошибка " + err + " : " + ltrsrvcon.GetErrorString(err));
                        LogInformation.Add(log);
                    }
                    else
                    {
                        var crateObj = nodeCrate.Object as CrateObject;
                        if (crateObj != null)
                        {
                            //По типу крейта определить количество слотов
                            for (int i = 0; i < crateObj.CountSlots; i++) //mids.Length; i++)
                            {
                                /* для наглядности при выводе для пустых слотов выводим ---, а не
                                 * название типа EMPTY, чтобы не сливалось и явно было видно где нет модулей */
                                NodeObject nodeModule = new NodeObject(mids[i], i, nodeCrate);
                                log = new LogInformation("Информация", "  Слот " + (i + 1) + " : " + (mids[i] == _LTRNative.MODULETYPE.EMPTY ? "---" : mids[i].ToString()));
                                LogInformation.Add(log);
                                //Создать узел карты
                                nodeCrate.Nodes.Add(nodeModule);
                            }
                        }
                    }
                    nodeLtrd.Nodes.Add(nodeCrate);
                }
            }
            Nodes = new ObservableCollection<NodeObject>();
            Nodes.Add(nodeLtrd);
            //Заполнить недостающие ip адреса

        }

        public bool SetSettingCrate(CrateObject crate)
        {
            ltrcrate.Config config = new ltrcrate.Config();
            config.digout_en = crate.LTRManagerSyncCrateSettings.OutputResolution;
            config.digout1 = ReturnDigOutCfg(crate.LTRManagerSyncCrateSettings.DIGOUT1);
            config.digout2 = ReturnDigOutCfg(crate.LTRManagerSyncCrateSettings.DIGOUT2);
            _LTRNative.LTRERROR err = crate.Ltrcrate.SetConfig(ref config);
            if (CheckErrorCrate(err)) return false;
            err = crate.Ltrcrate.MakeStartMark(ReturnStartMarkMode(crate.LTRManagerSyncCrateSettings.StartMarkMode));
            if (CheckErrorCrate(err)) return false;
            if (crate.LTRManagerSyncCrateSettings.SecondMarkMode != 0)
                err = crate.Ltrcrate.StartSecondMark(ReturnSecondMarkMode(crate.LTRManagerSyncCrateSettings.SecondMarkMode));
            else
                err = crate.Ltrcrate.StopSecondMark();
            if (CheckErrorCrate(err)) return false;

            return true;
        }

        private _LTRNative.en_LTR_MarkMode ReturnSecondMarkMode(int mode)
        {
            switch (mode)
            {
                case 0:
                    return _LTRNative.en_LTR_MarkMode.LTR_MARK_OFF;
                case 1:
                    return _LTRNative.en_LTR_MarkMode.LTR_MARK_INTERNAL;
                case 2:
                    return _LTRNative.en_LTR_MarkMode.LTR_MARK_EXT_DIGIN1_RISE;
                case 3:
                    return _LTRNative.en_LTR_MarkMode.LTR_MARK_EXT_DIGIN1_FALL;
                case 4:
                    return _LTRNative.en_LTR_MarkMode.LTR_MARK_EXT_DIGIN2_RISE;
                case 5:
                    return _LTRNative.en_LTR_MarkMode.LTR_MARK_EXT_DIGIN2_FALL;
            }

            return _LTRNative.en_LTR_MarkMode.LTR_MARK_OFF;
        }

        private _LTRNative.en_LTR_MarkMode ReturnStartMarkMode(int mode)
        {
            switch(mode)
            {
                case 0:
                    return _LTRNative.en_LTR_MarkMode.LTR_MARK_OFF;
                case 1:
                    return _LTRNative.en_LTR_MarkMode.LTR_MARK_EXT_DIGIN1_RISE;
                case 2:
                    return _LTRNative.en_LTR_MarkMode.LTR_MARK_EXT_DIGIN1_FALL;
                case 3:
                    return _LTRNative.en_LTR_MarkMode.LTR_MARK_EXT_DIGIN2_RISE;
                case 4:
                    return _LTRNative.en_LTR_MarkMode.LTR_MARK_EXT_DIGIN2_FALL;
            }

            return _LTRNative.en_LTR_MarkMode.LTR_MARK_OFF;
        }

        private ltrcrate.DigOutCfg ReturnDigOutCfg(int dig)
        {
            switch(dig)
            {
                case 0:
                    return ltrcrate.DigOutCfg.CONST0;
                case 1:
                    return ltrcrate.DigOutCfg.CONST1;
                case 2:
                    return ltrcrate.DigOutCfg.DIGIN1;
                case 3:
                    return ltrcrate.DigOutCfg.DIGIN2;
                case 4:
                    return ltrcrate.DigOutCfg.START;
                case 5:
                    return ltrcrate.DigOutCfg.SECOND;

                default:
                    break;
            }

            return ltrcrate.DigOutCfg.DEFAULT;
        }

        public void UpdateTree()
        {
            //Зачистить крейты у лтрд
            Nodes[0].Nodes = new ObservableCollection<NodeObject>();
            IPAddressCrates.ToList().ForEach(u => u.On = false);
            /* получаем список серийных номеров крейтов, которые подключены */
            string[] crateSerialList;
            _LTRNative.LTRERROR err = Srvcon.GetCrates(out crateSerialList);

            foreach (string csn in crateSerialList)
            {
                /* устанавливаем соединение с каждым крейтом. для этого
                 * используем класс ltrcrate, который реализует подмножество
                 * функций из ltrapi для работы с одним крейтом */
                ltrcrate crate = new ltrcrate();
                err = crate.Open(csn);
                if (err != _LTRNative.LTRERROR.OK)
                {
                    
                }
                else
                {
                    //Найти среди подключений и задать подключение
                    IPAddressCrateObject ipObject = IPAddressCrates.FirstOrDefault(u => u.Serial == csn.Trim('\0'));
                    if (ipObject == null)
                    {
                        //AddLogInformation("Ошибка", "Данного крейта нет в БД : " + csn);
                        continue;
                    }
                    //ipObject.On = true;
                    //Настройки из бд
                    LTRManagerSyncCrateSettings crateSetting = LTRManagerSyncCrateSettings.FirstOrDefault(u => u.Serial == csn.Trim('\0'));
                    //Флаг для задания настроек
                    bool setSetting = false;
                    if (crateSetting == null)
                    {
                        //Создать новые настройки и задать их
                        crateSetting = new LTRManagerSyncCrateSettings(
                            csn.Trim('\0'),
                            false,
                            (int)DIGOUT.Start,
                            (int)DIGOUT.Second,
                            (int)StartMark.Off,
                            (int)SecondMark.InternalTimer,
                            false);
                        //Сохранить в бд
                        DataValueClient.Client.Send<bool>(ApiMethods.LTRManager.INSERT_SYNC_CRATE_SETTINGS_RECORD, crateSetting);
                        //Занести в коллекцию
                        LTRManagerSyncCrateSettings.Add(crateSetting);
                        AddLogInformation("Информация", "Не было настроек в БД! Настройки заданы по умолчанию : " + csn.Trim('\0'));
                        setSetting = true;
                    }

                    //Создать узел крейта
                    NodeObject nodeCrate = new NodeObject(crate, Nodes[0], csn, ipObject, crateSetting);

                    //Если был отключен, то надо забить настройки
                    if (setSetting || !ipObject.On)
                        //Внести настройки
                        if (!SetSettingCrate(nodeCrate.Object as CrateObject)) continue;
                    ipObject.On = true;

                    /* получаем список модулей */
                    _LTRNative.MODULETYPE[] mids;
                    err = crate.GetModules(out mids);
                    if (err != _LTRNative.LTRERROR.OK)
                    {
                    }
                    else
                    {
                        var crateObj = nodeCrate.Object as CrateObject;
                        if (crateObj != null)
                        {
                            //По типу крейта определить количество слотов
                            for (int i = 0; i < crateObj.CountSlots; i++) //mids.Length; i++)
                            {
                                /* для наглядности при выводе для пустых слотов выводим ---, а не
                                 * название типа EMPTY, чтобы не сливалось и явно было видно где нет модулей */
                                NodeObject nodeModule = new NodeObject(mids[i], i, nodeCrate);
                                //Создать узел карты
                                nodeCrate.Nodes.Add(nodeModule);
                            }
                        }
                    }
                    Nodes[0].Nodes.Add(nodeCrate);
                }
            }
        }

        public bool ConnectCrate(IPAddressCrateObject selectedIPAddress)
        {
            //Создать новое подключение
            _LTRNative.LTRERROR err = Srvcon.AddIPCrate(selectedIPAddress.IPAddres, 0, false);
            if (CheckErrorSrv(err)) return false;
            err = Srvcon.ConnectIPCrate(selectedIPAddress.IPAddres);
            if (CheckErrorSrv(err)) return false;
            Thread.Sleep(1000);
            //Проверить появился ли он
            string[] crateSerialList;
            err = Srvcon.GetCrates(out crateSerialList);
            if (CheckErrorSrv(err)) return false;
            crateSerialList = crateSerialList.Select(u => u.Trim('\0')).ToArray();
            //Если нет в подключенных, то вывести информацию
            if (!crateSerialList.Contains(selectedIPAddress.Serial))
            {
                AddLogInformation("Ошибка", "Не удалось подключиться! ip : " + selectedIPAddress.IPAddres + ", serial : " + selectedIPAddress.Serial);
                return false;
            }

            AddLogInformation("Информация", "Подключение установлено! ip : " + selectedIPAddress.IPAddres + ", serial : " + selectedIPAddress.Serial);
            selectedIPAddress.On = true;
            //Забить настройки в крейт
            return true;
        }

        public bool DisconnectCrate(IPAddressCrateObject selectedIPAddress)
        {
            //Получить все крейты из дерева
            List<CrateObject> crates = Nodes[0].GetCratesObjects(Nodes[0]);
            if (crates.Count == 0) return false;

            CrateObject crate = crates.FirstOrDefault(u => u.Serial == selectedIPAddress.Serial);
            if (crate == null) return false;

            //Тк крейт есть в дереве, то нужно его убрать из выбранных
            if (SelectedCrate == crate)
            {
                SelectedCrate = null;
                SelectedLCard = null;
                SelectCrate = false;
                SelectModule = false;
            }

            _LTRNative.LTRERROR err = crate.Ltrcrate.IsOpened();
            if (err == _LTRNative.LTRERROR.ERROR_CHANNEL_CLOSED)
            {
                AddLogInformation("Информация", "Крейт уже отключен!");
                return false;
            }
            //err = crate.Ltrcrate.Close();
            if (CheckErrorCrate(err)) return false;
            err = crate.Ltrcrate.DisconnectIPCrate(NetAddrConverter.ipAddrToUint(crate.IPAddressCrate.IPAddres));
            if (CheckErrorCrate(err)) return false;
            selectedIPAddress.On = false;
            return true;
        }

        public bool DeleteIPAddress(IPAddressCrateObject crateObject)
        {
            //Удалить из бд и обновить списки с деревьями
            _LTRNative.LTRERROR err = Srvcon.IsOpened();
            if (err == _LTRNative.LTRERROR.ERROR_CHANNEL_CLOSED)
            {
                MessageBox.Show("Необходимо подключение к ltrd!", "Информация", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            //Удалить из бд
            var model = IPAddressesModel.FirstOrDefault(u => u.Serial.Trim('\0') == crateObject.Serial);
            if (model == null)
            {
                AddLogInformation("Ошибка", "Не найдено записи в БД");
                return false;
            }
            DataValueClient.Client.Send<bool>(ApiMethods.LTRManager.DELETE_IP_ADDRESS_RECORD, model);//(ApiMethods.LTRManager.DELETE_IP_ADDRESS_RECORD, model.ID);
            //Удалить из объектов
            //if (!DisconnectCrate(crateObject)) return false;
            var _crate = IPAddressCrates.FirstOrDefault(u => u.Serial == crateObject.Serial);
            IPAddressCrates.Remove(_crate);
            IPAddressesModel.Remove(model);

            return true;
        }

        public void UpdateIPAddress(IPAddressCrateObject crateObject)
        {
            LTRManagerIPAddress address = IPAddressesModel.FirstOrDefault(u => u.Serial == crateObject.Serial);//new LTRManagerIPAddress(crateObject., serials[0].Trim('\0'), false, false);
            address.Autoconnect = crateObject.Autoconnect;
            address.Reconnect = crateObject.Reconnect;
            DataValueClient.Client.Send<bool>(ApiMethods.LTRManager.UPDATE_IP_ADDRESS_RECORD, address);
        }

        public void AddNewIPAddress(IPAddress ip)
        {
            //Получить список подключенных крейтов и взять от туда номер
            string[] crateSerialList;
            _LTRNative.LTRERROR err = Srvcon.GetCrates(out crateSerialList);
            if (CheckErrorSrv(err)) return;
            List<string> serials = new List<string>();
            List<string> serialsExists = IPAddressesModel.Select(u => u.Serial.Trim('\0')).ToList();
            foreach (var ser in crateSerialList)
            {
                if (!serialsExists.Contains(ser.Trim('\0'))) serials.Add(ser);
            }
            if (serials.Count == 0)
            {
                LogInformation.Add(new LogInformation("Ошибка", "Новое подключение не найдено!"));
                return;
            }
            else if (serials.Count > 1)
            {
                LogInformation.Add(new LogInformation("Ошибка", "Найдено множество новых подключений, вместо одного!"));
                return;
            }

            LTRManagerIPAddress address = new LTRManagerIPAddress(ip.ToString(), serials[0].Trim('\0'), false, true);
            IPAddressesModel.Add(address);
            //IPAddressCrates.Add(new IPAddressCrateObject(ip, "Подключено", address.Serial, false, false));
            IPAddressCrates.Add(new IPAddressCrateObject(ip, true, address.Serial.Trim('\0'), false, true));
            //занести новый адрес в бд
            DataValueClient.Client.Send<bool>(ApiMethods.LTRManager.INSERT_IP_ADDRESS_RECORD, address);
        }
        public void DisconnectLtrd()
        {
            LogInformation log = new LogInformation("Информация", "Соединение с сервисом уже разорвано!");
            _LTRNative.LTRERROR err = Srvcon.IsOpened();
            if (err == _LTRNative.LTRERROR.OK)
            {
                err = Srvcon.Close();
                if (err == _LTRNative.LTRERROR.OK)
                {
                    log = new LogInformation("Информация", "Соединение с сервисом разорвано успешно!");

                    NodeObject nodeLtrd = new NodeObject(Srvcon, "0", null);
                    var obj = nodeLtrd.Object as LtrdServerObject;
                    if (obj != null)
                    {
                        obj.IsConnect = false;
                        obj.ConnectLtrdInfo = "Отключено";
                        nodeLtrd.Object = obj;
                    }
                    Nodes = new ObservableCollection<NodeObject>();
                    Nodes.Add(nodeLtrd);
                    //IPAddressCrates = new ObservableCollection<IPAddressCrateObject>();

                    IsConnectLtrd = true;
                    IsDisconnectLtrd = false;
                }
                else
                {
                    log = new LogInformation("Ошибка", "Не удалось разорвать соединение с сервисом!");
                }
            }
            LogInformation.Add(log);
        }

        public void AddLogInformation(string type, string message)
        {
            LogInformation.Add(new LogInformation(type, message));
        }

        public bool CheckErrorSrv(_LTRNative.LTRERROR error)
        {
            if (error != _LTRNative.LTRERROR.OK)
            {
                LogInformation log = new LogInformation("Ошибка", ltrsrvcon.GetErrorString(error));
                LogInformation.Add(log);
                return true;
            }
            return false;
        }
        public bool CheckErrorCrate(_LTRNative.LTRERROR error)
        {
            if (error != _LTRNative.LTRERROR.OK)
            {
                LogInformation log = new LogInformation("Ошибка", ltrcrate.GetErrorString(error));
                LogInformation.Add(log);
                return true;
            }
            return false;
        }

        public bool CheckErrorLCard(_LTRNative.LTRERROR error, LCardObject lcard)
        {
            if (error != _LTRNative.LTRERROR.OK)
            {
                LogInformation log = new LogInformation("Ошибка", lcard.GetErrorString(error));
                LogInformation.Add(log);
                return true;
            }
            return false;
        }
    }
}
