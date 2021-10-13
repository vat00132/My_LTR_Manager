using ltrModulesNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCore.LTRManager;

namespace LTRManager.Model
{
    public class CrateObject : BindableBase
    {
        /// <summary>
        /// Название
        /// </summary>
        private string _Name;
        public string Name { get { return _Name; } set { SetProperty(ref _Name, value); } }
        /// <summary>
        /// Серийный номер
        /// </summary>
        private string _Serial;
        public string Serial { get { return _Serial; } set { SetProperty(ref _Serial, value); } }
        /// <summary>
        /// Крейт
        /// </summary>
        public ltrcrate Ltrcrate { get; }
        /// <summary>
        /// Количество слотов в крейте
        /// </summary>
        public int CountSlots { get; private set; }
        /// <summary>
        /// Интерфейс
        /// </summary>
        private string _Interface;
        public string Interface { get { return _Interface; } set { SetProperty(ref _Interface, value); } }
        /// <summary>
        /// Серийный номер для TLTR
        /// </summary>
        public string CSN;
        //private CRATE_DESCR Descr;
        /// <summary>
        /// Версия прошивки
        /// </summary>
        private string _FirmwareVersion;
        public string FirmwareVersion { get { return _FirmwareVersion; } set { SetProperty(ref _FirmwareVersion, value); } }
        /// <summary>
        /// Версия загрузчика
        /// </summary>
        private string _LoaderVersion;
        public string LoaderVersion { get { return _LoaderVersion; } set { SetProperty(ref _LoaderVersion, value); } }
        /// <summary>
        /// Версия прошивки ПЛИС
        /// </summary>
        private string _FirmwareVersionPLIS;
        public string FirmwareVersionPLIS { get { return _FirmwareVersionPLIS; } set { SetProperty(ref _FirmwareVersionPLIS, value); } }
        /// <summary>
        /// Ревизия
        /// </summary>
        private string _Revision;
        public string Revision { get { return _Revision; } set { SetProperty(ref _Revision, value); } }
        /// <summary>
        /// Версия протокола
        /// </summary>
        private string _ProtocolVersion;
        public string ProtocolVersion { get { return _ProtocolVersion; } set { SetProperty(ref _ProtocolVersion, value); } }
        /// <summary>
        /// Статистика крейта
        /// </summary>
        //private CRATE_STATISTIC Statistic;
        /// <summary>
        /// Время подключения
        /// </summary>
        private string _TimeConnection;
        public string TimeConnection { get { return _TimeConnection; } set { SetProperty(ref _TimeConnection, value); } }
        private DateTime TimeConn;
        /// <summary>
        /// Количество всех клиентов
        /// </summary>
        private int _CountClient;
        public int CountClient { get { return _CountClient; } set { SetProperty(ref _CountClient, value); } }
        /// <summary>
        /// Количество управляющих соединений
        /// </summary>
        private int _CountControlConnections;
        public int CountControlConnections { get { return _CountControlConnections; } set { SetProperty(ref _CountControlConnections, value); } }
        /// <summary>
        /// Количество переданных слов
        /// </summary>
        private long _CountWordsPassed;
        public long CountWordsPassed { get { return _CountWordsPassed; } set { SetProperty(ref _CountWordsPassed, value); } }
        /// <summary>
        /// Количество переданных слов
        /// </summary>
        private long _CountAcceptedWords;
        public long CountAcceptedWords { get { return _CountAcceptedWords; } set { SetProperty(ref _CountAcceptedWords, value); } }
        /// <summary>
        /// Скорость передачи
        /// </summary>
        private double _TransferRate;
        public double TransferRate { get { return _TransferRate; } set { SetProperty(ref _TransferRate, value); } }
        /// <summary>
        /// Скорость приема
        /// </summary>
        private double _ReceptionRate;
        public double ReceptionRate { get { return _ReceptionRate; } set { SetProperty(ref _ReceptionRate, value); } }
        /// <summary>
        /// Метки старт
        /// </summary>
        private int _StartMark;
        public int StartMark { get { return _StartMark; } set { SetProperty(ref _StartMark, value); } }
        /// <summary>
        /// Метки секунда
        /// </summary>
        private int _SecondMark;
        public int SecondMark { get { return _SecondMark; } set { SetProperty(ref _SecondMark, value); } }
        public IPAddressCrateObject IPAddressCrate;
        public LTRManagerSyncCrateSettings LTRManagerSyncCrateSettings { get; set; }

        public CrateObject(ltrcrate crate, string csn, IPAddressCrateObject ipObj, LTRManagerSyncCrateSettings setting)
        {
            LTRManagerSyncCrateSettings = setting;
            IPAddressCrate = ipObj;
            Ltrcrate = crate;
            DefineNameCrate(Ltrcrate.Type);
            _Serial = Ltrcrate.Serial;
            _Interface = Ltrcrate.Interface.ToString();
            CSN = csn;
            _LTRNative.LTRERROR err = crate.IsOpened();
            err = Ltrcrate.GetCrateDescr(CSN);//ltrcrate.LTR_GetCrateDescr(ref crate.hnd, (int)crate.Interface, CSN, out Descr, 512);
            //Записываем данные о крейте
            if (err == _LTRNative.LTRERROR.OK)
            {
                _FirmwareVersion = new string(Ltrcrate.DESCR.soft_ver.Where(u=>u!='\0').ToArray());
                _LoaderVersion = new string(Ltrcrate.DESCR.bootloader_ver.Where(u => u != '\0').ToArray());
                _FirmwareVersionPLIS = new string(Ltrcrate.DESCR.fpga_version.Where(u => u != '\0').ToArray());
                _Revision = new string(Ltrcrate.DESCR.brd_revision.Where(u => u != '\0').ToArray());
                _ProtocolVersion = Convert.ToString(Ltrcrate.DESCR.protocol_ver_major) + "." + Convert.ToString(Ltrcrate.DESCR.protocol_ver_minor);
            }
            err = Ltrcrate.GetCrateStatistic(CSN);//ltrcrate.LTR_GetCrateStatistic(ref crate.hnd, (int)crate.Interface, CSN, out Statistic, 512);
            //Записываем статистику
            if (err == _LTRNative.LTRERROR.OK)
            {
                TimeConn = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(Ltrcrate.STATISTIC.con_time);
                _TimeConnection = Convert.ToString(TimeConn);
                _CountClient = Ltrcrate.STATISTIC.total_mod_clients_cnt;
                _CountControlConnections = Ltrcrate.STATISTIC.ctl_clients_cnt;
                _CountWordsPassed = (long)Ltrcrate.STATISTIC.wrd_sent;
                _CountAcceptedWords = (long)Ltrcrate.STATISTIC.wrd_recv;
                _TransferRate = Ltrcrate.STATISTIC.bw_send;
                _ReceptionRate = Ltrcrate.STATISTIC.bw_recv;
                _StartMark = (int)Ltrcrate.STATISTIC.crate_start_marks;
                _SecondMark = (int)Ltrcrate.STATISTIC.crate_sec_marks;
            }
        }

        public _LTRNative.LTRERROR UpdateStatisticsInfo()
        {
            _LTRNative.LTRERROR err = _LTRNative.LTRERROR.OK;
            err = Ltrcrate.GetCrateStatistic(CSN);//ltrcrate.LTR_GetCrateStatistic(ref Ltrcrate.hnd, (int)Ltrcrate.Interface, CSN, out Statistic, 512);
            if (err == _LTRNative.LTRERROR.OK)
            {
                TimeConn = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(Ltrcrate.STATISTIC.con_time);
                TimeConnection = Convert.ToString(TimeConn);
                CountClient = Ltrcrate.STATISTIC.total_mod_clients_cnt;
                CountControlConnections = Ltrcrate.STATISTIC.ctl_clients_cnt;
                CountWordsPassed = (long)Ltrcrate.STATISTIC.wrd_sent;
                CountAcceptedWords = (long)Ltrcrate.STATISTIC.wrd_recv;
                TransferRate = Ltrcrate.STATISTIC.bw_send;
                ReceptionRate = Ltrcrate.STATISTIC.bw_recv;
                StartMark = (int)Ltrcrate.STATISTIC.crate_start_marks;
                SecondMark = (int)Ltrcrate.STATISTIC.crate_sec_marks;
            }
            return err;
        }

        private void DefineNameCrate(ltrcrate.Types type)
        {
            switch (type)
            {
                case ltrcrate.Types.LTR010:
                    _Name = "LTR-U-8/LTR-U-16";
                    CountSlots = 16;
                    break;
                case ltrcrate.Types.LTR021:
                    _Name = "LTR-U-1";
                    CountSlots = 1;
                    break;
                case ltrcrate.Types.LTR030:
                    _Name = "LTR-EU-8/LTR-EU-16";
                    CountSlots = 16;
                    break;
                case ltrcrate.Types.LTR031:
                    _Name = "LTR-EU-2";
                    CountSlots = 2;
                    break;
                case ltrcrate.Types.LTR_CU_1:
                    _Name = "LTR-CU-1";
                    CountSlots = 1;
                    break;
                case ltrcrate.Types.LTR_CEU_1:
                    _Name = "LTR-CEU-1";
                    CountSlots = 1;
                    break;
                case ltrcrate.Types.BOOTLOADER:
                    _Name = "Загрузка...";
                    CountSlots = 0;
                    break;
                default:
                    //Name = "Неизвестный тип крейта";
                    _Name = type.ToString();
                    CountSlots = 16;
                    break;
            }
        }
    }
}
