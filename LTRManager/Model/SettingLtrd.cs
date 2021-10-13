using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCore.LTRManager;

namespace LTRManager.Model
{
    public class SettingLtrd : BindableBase
    {
        public Guid ID;
        /// <summary>
        /// IP
        /// </summary>
        private string _IP;
        public string IP
        {
            get { return _IP; }
            set { SetProperty(ref _IP, value); }
        }
        /// <summary>
        /// Время опросов крейтов 
        /// </summary>
        private int _TimeSurvey;
        public int TimeSurvey { get { return _TimeSurvey; } set { SetProperty(ref _TimeSurvey, value); } }
        /// <summary>
        /// Таймаут на выполнение команд
        /// </summary>
        private int _TimeoutForCommandExecution;
        public int TimeoutForCommandExecution
        {
            get { return _TimeoutForCommandExecution; }
            set { SetProperty(ref _TimeoutForCommandExecution, value); }
        }
        /// <summary>
        /// Таймаут на установление соединения
        /// </summary>
        private int _ConnectionEstablishmentTimeout;
        public int ConnectionEstablishmentTimeout
        {
            get { return _ConnectionEstablishmentTimeout; }
            set { SetProperty(ref _ConnectionEstablishmentTimeout, value); }
        }
        /// <summary>
        /// Время для повтороного подключения
        /// </summary>
        private int _TimeRecconect;
        public int TimeRecconect
        {
            get { return _TimeRecconect; }
            set { SetProperty(ref _TimeRecconect, value); }
        }
        /// <summary>
        /// Передача по TCP без задержки
        /// </summary>
        private bool _TransferWithoutDelay;
        public bool TransferWithoutDelay
        {
            get { return _TransferWithoutDelay; }
            set { SetProperty(ref _TransferWithoutDelay, value); }
        }
        /// <summary>
        /// Размер буфера модуля на передачу
        /// </summary>
        private int _SizeBufferTransfer;
        public int SizeBufferTransfer
        {
            get { return _SizeBufferTransfer; }
            set { SetProperty(ref _SizeBufferTransfer, value); }
        }
        /// <summary>
        /// Размер буфера модуля на прием
        /// </summary>
        private int _SizeBufferReceive;
        public int SizeBufferReceive
        {
            get { return _SizeBufferReceive; }
            set { SetProperty(ref _SizeBufferReceive, value); }
        }
        public SettingLtrd(LTRManagerSettingLtrd setting)
        {
            ID = setting.ID;
            _IP = setting.IP;
            _TimeSurvey = setting.TimeSurvey;
            _TimeoutForCommandExecution = setting.TimeoutForCommandExecution;
            _ConnectionEstablishmentTimeout = setting.ConnectionEstablishmentTimeout;
            _TimeRecconect = setting.TimeRecconect;
            _TransferWithoutDelay = setting.TransferWithoutDelay;
            _SizeBufferTransfer = setting.SizeBufferTransfer;
            _SizeBufferReceive = setting.SizeBufferReceive;
        }
    }
}
