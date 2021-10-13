using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VCore.LTRManager;

namespace LTRManager.Model
{
    public class IPAddressCrateObject : BindableBase
    {
        /// <summary>
        /// IP адресс
        /// </summary>
        private IPAddress _IPAddres;
        public IPAddress IPAddres { get { return _IPAddres; } set { SetProperty(ref _IPAddres, value); } }
        /// <summary>
        /// Статус подключения
        /// </summary>
        private string _Status;
        public string Status { get { return _Status; } set { SetProperty(ref _Status, value); } }
        private bool _On;
        public bool On { get { return _On; } set { SetProperty(ref _On, value); ChangedOn(); } }
        /// <summary>
        /// Серийный номер
        /// </summary>
        private string _Serial;
        public string Serial { get { return _Serial; } set { SetProperty(ref _Serial, value); } }
        /// <summary>
        /// Автоподключение
        /// </summary>
        private bool _Autoconnect;
        public bool Autoconnect { get { return _Autoconnect; } set { SetProperty(ref _Autoconnect, value); } }
        /// <summary>
        /// Переподключение
        /// </summary>
        private bool _Reconnect;
        public bool Reconnect { get { return _Reconnect; } set { SetProperty(ref _Reconnect, value); } }
        private void ChangedOn()
        {
            if (On)
                Status = "Подключено";
            else
                Status = "Отключено";
        }
        public IPAddressCrateObject(IPAddress ipadd, string stat, string serial, bool auto, bool rec)
        {
            _IPAddres = ipadd;
            _Status = stat;
            _Serial = serial;
            _Autoconnect = auto;
            _Reconnect = rec;
        }
        public IPAddressCrateObject(IPAddress ipadd, bool on, string serial, bool auto, bool rec)
        {
            _IPAddres = ipadd;
            _On = on;
            ChangedOn();
            //_Status = stat;
            _Serial = serial;
            _Autoconnect = auto;
            _Reconnect = rec;
        }
        public IPAddressCrateObject()
        { }
    }
}
