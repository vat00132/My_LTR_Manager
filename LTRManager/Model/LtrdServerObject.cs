using ltrModulesNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTRManager.Model
{
    public class LtrdServerObject : BindableBase
    {
        /// <summary>
        /// Название
        /// </summary>
        private string _Name;
        public string Name { get { return _Name; } set { SetProperty(ref _Name, value); } }
        /// <summary>
        /// дтрд сервер
        /// </summary>
        public ltrsrvcon Ltrsrvcon { get; }
        /// <summary>
        /// Состояние о подключении
        /// </summary>
        public bool IsConnect { get; set; }
        /// <summary>
        /// Версия
        /// </summary>
        private string _Version;
        public string Version { get { return _Version; } set { SetProperty(ref _Version, value); } }

        private string _ConnectLtrdInfo;
        public string ConnectLtrdInfo { get { return _ConnectLtrdInfo; } set { SetProperty(ref _ConnectLtrdInfo, value); } }

        public LtrdServerObject(ltrsrvcon ltrsrvcon, string ver)
        {
            Ltrsrvcon = ltrsrvcon;
            _Name = "ltrd (127.0.0.1)";
            IsConnect = true;
            _Version = ver;
            _ConnectLtrdInfo = "Подключено";
        }
    }
}
