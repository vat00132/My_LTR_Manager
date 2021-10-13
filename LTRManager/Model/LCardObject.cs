using ltrModulesNet;

namespace LTRManager.Model
{
    public class LCardObject : BindableBase
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Номер слота
        /// </summary>
        public int Slot { get; }
        /// <summary>
        /// Тип модуля
        /// </summary>
        private _LTRNative.MODULETYPE MODULETYPE;
        /// <summary>
        /// Крейт которому принадлежит модуль
        /// </summary>
        public CrateObject CrateObject;
        /// <summary>
        /// Состояние
        /// </summary>
        private string _Condition;
        public string Condition { get { return _Condition; } set { SetProperty(ref _Condition, value); } }
        /// <summary>
        /// Всего клиентов
        /// </summary>
        private int _CountClients;
        public int CountClients { get { return _CountClients; } set { SetProperty(ref _CountClients, value); } }
        /// <summary>
        /// Передано слов
        /// </summary>
        private long _WordsPassed;
        public long WordsPassed { get { return _WordsPassed; } set { SetProperty(ref _WordsPassed, value); } }
        /// <summary>
        /// Принято слов
        /// </summary>
        private long _MadeWords;
        public long MadeWords { get { return _MadeWords; } set { SetProperty(ref _MadeWords, value); } }
        /// <summary>
        /// Скорость передачи
        /// </summary>
        private double _TransferRate;
        public double TransferRate { get { return _TransferRate; } set { SetProperty(ref _TransferRate, value); } }
        /// <summary>
        /// Скорость приема
        /// </summary>
        private double _ReceptionSpeed;
        public double ReceptionSpeed { get { return _ReceptionSpeed; } set { SetProperty(ref _ReceptionSpeed, value); } }
        /// <summary>
        /// Буфер на передачу
        /// </summary>
        private int _TransferBuffer;
        public int TransferBuffer { get { return _TransferBuffer; } set { SetProperty(ref _TransferBuffer, value); } }
        /// <summary>
        /// Буфер на прием
        /// </summary>
        private int _BufferReceive;
        public int BufferReceive { get { return _BufferReceive; } set { SetProperty(ref _BufferReceive, value); } }
        /// <summary>
        /// Макс буфер на передачу
        /// </summary>
        private int _MaxTransferBuffer;
        public int MaxTransferBuffer { get { return _MaxTransferBuffer; } set { SetProperty(ref _MaxTransferBuffer, value); } }
        /// <summary>
        /// Макс буфер на прием
        /// </summary>
        private int _MaxBufferReceive;
        public int MaxBufferReceive { get { return _MaxBufferReceive; } set { SetProperty(ref _MaxBufferReceive, value); } }
        /// <summary>
        /// Переполнение буфера
        /// </summary>
        private int _BufferOverflow;
        public int BufferOverflow { get { return _BufferOverflow; } set { SetProperty(ref _BufferOverflow, value); } }
        /// <summary>
        /// Потеряно слов
        /// </summary>
        private long _LostWords;
        public long LostWords { get { return _LostWords; } set { SetProperty(ref _LostWords, value); } }

        public object LtrXXXapi { get; }
        //private TLTR_MODULE_STATISTIC Statistic;
        public LCardObject(_LTRNative.MODULETYPE module, int slot, CrateObject crate)
        {
            Name = module == _LTRNative.MODULETYPE.EMPTY ? "---" : module.ToString();
            Slot = slot + 1;
            MODULETYPE = module;
            CrateObject = crate;
            LtrXXXapi = CreateLtrXXXapi();
            //Вернуть тип ltrapi и тогда вычислять
            _LTRNative.LTRERROR err = _LTRNative.LTRERROR.OK;
            if (LtrXXXapi != null)
            {
                err = CrateObject.Ltrcrate.GetModuleStatistic(Slot, CrateObject.CSN);//_LTRNative.LTR_GetModuleStatistic(ref crate.Ltrcrate.hnd, (int)crate.Ltrcrate.Interface, crate.CSN, Slot, out Statistic, 512);
                if (err == _LTRNative.LTRERROR.OK)
                {
                    _Condition = new string(CrateObject.Ltrcrate.STATISTIC_MODULE.name).Trim('\0');
                    _CountClients = CrateObject.Ltrcrate.STATISTIC_MODULE.client_cnt;
                    _WordsPassed = (long)CrateObject.Ltrcrate.STATISTIC_MODULE.wrd_sent;
                    _MadeWords = (long)CrateObject.Ltrcrate.STATISTIC_MODULE.wrd_recv;
                    _TransferRate = CrateObject.Ltrcrate.STATISTIC_MODULE.bw_send;
                    _ReceptionSpeed = CrateObject.Ltrcrate.STATISTIC_MODULE.bw_recv;
                    _TransferBuffer = (int)CrateObject.Ltrcrate.STATISTIC_MODULE.send_srvbuf_full;
                    _BufferReceive = (int)CrateObject.Ltrcrate.STATISTIC_MODULE.recv_srvbuf_full;
                    _MaxTransferBuffer = (int)CrateObject.Ltrcrate.STATISTIC_MODULE.send_srvbuf_full_max;
                    _MaxBufferReceive = (int)CrateObject.Ltrcrate.STATISTIC_MODULE.recv_srvbuf_full_max;
                    _BufferOverflow = (int)CrateObject.Ltrcrate.STATISTIC_MODULE.rbuf_ovfls;
                    _LostWords = (long)CrateObject.Ltrcrate.STATISTIC_MODULE.wrd_recv_drop;
                }
            }
            if (LtrXXXapi == null || err != _LTRNative.LTRERROR.OK) 
            {
                _Condition = "Пустой слот";
                _CountClients = 0;
                _WordsPassed = 0;
                _MadeWords = 0;
                _TransferRate = 0;
                _ReceptionSpeed = 0;
                _TransferBuffer = 0;
                _BufferReceive = 0;
                _MaxTransferBuffer = 0;
                _MaxBufferReceive = 0;
                _BufferOverflow = 0;
                _LostWords = 0;
            }
        }

        public _LTRNative.LTRERROR UpdateStatisticsInfo()
        {
            _LTRNative.LTRERROR err = _LTRNative.LTRERROR.OK;

            err = CrateObject.Ltrcrate.GetModuleStatistic(Slot, CrateObject.CSN);//_LTRNative.LTR_GetModuleStatistic(ref CrateObject.Ltrcrate.hnd, (int)CrateObject.Ltrcrate.Interface, CrateObject.CSN, Slot, out Statistic, 512);
            if (err == _LTRNative.LTRERROR.OK)
            {
                Condition = new string(CrateObject.Ltrcrate.STATISTIC_MODULE.name).Trim('\0');
                CountClients = CrateObject.Ltrcrate.STATISTIC_MODULE.client_cnt;
                WordsPassed = (long)CrateObject.Ltrcrate.STATISTIC_MODULE.wrd_sent;
                MadeWords = (long)CrateObject.Ltrcrate.STATISTIC_MODULE.wrd_recv;
                TransferRate = CrateObject.Ltrcrate.STATISTIC_MODULE.bw_send;
                ReceptionSpeed = CrateObject.Ltrcrate.STATISTIC_MODULE.bw_recv;
                TransferBuffer = (int)CrateObject.Ltrcrate.STATISTIC_MODULE.send_srvbuf_full;
                BufferReceive = (int)CrateObject.Ltrcrate.STATISTIC_MODULE.recv_srvbuf_full;
                MaxTransferBuffer = (int)CrateObject.Ltrcrate.STATISTIC_MODULE.send_srvbuf_full_max;
                MaxBufferReceive = (int)CrateObject.Ltrcrate.STATISTIC_MODULE.recv_srvbuf_full_max;
                BufferOverflow = (int)CrateObject.Ltrcrate.STATISTIC_MODULE.rbuf_ovfls;
                LostWords = (long)CrateObject.Ltrcrate.STATISTIC_MODULE.wrd_recv_drop;
            }

            return err;
        }

        public string GetErrorString(_LTRNative.LTRERROR err)
        {
            switch(MODULETYPE)
            {
                case _LTRNative.MODULETYPE.LTR11:
                    return ltr11api.GetErrorString(err);
                case _LTRNative.MODULETYPE.LTR114:
                    return ltr114api.GetErrorString(err);
                case _LTRNative.MODULETYPE.LTR210:
                    return ltr210api.GetErrorString(err);
                case _LTRNative.MODULETYPE.LTR212:
                    return ltr212api.GetErrorString(err);
                case _LTRNative.MODULETYPE.LTR22:
                    return ltr22api.GetErrorString(err);
                case _LTRNative.MODULETYPE.LTR24:
                    return ltr24api.GetErrorString(err);
                case _LTRNative.MODULETYPE.LTR25:
                    return ltr25api.GetErrorString(err);
                case _LTRNative.MODULETYPE.LTR27:
                    return ltr27api.GetErrorString(err);
                case _LTRNative.MODULETYPE.LTR34:
                    return ltr34api.GetErrorString(err);
                case _LTRNative.MODULETYPE.LTR41:
                    return ltr41api.GetErrorString(err);
                case _LTRNative.MODULETYPE.LTR42:
                    return ltr42api.GetErrorString(err);
                case _LTRNative.MODULETYPE.LTR43:
                    return ltr43api.GetErrorString(err);
                case _LTRNative.MODULETYPE.LTR51:
                    return ltr51api.GetErrorString(err);

                default:
                    break;
            }

            return _LTRNative.GetErrorString(err);
        }

        private object CreateLtrXXXapi()
        {
            switch(MODULETYPE)
            {
                case _LTRNative.MODULETYPE.LTR11:
                    ltr11api ltr11 = new ltr11api();
                    //ltr11.Open(CrateObject.CSN, (ushort)Slot);
                    return ltr11;
                case _LTRNative.MODULETYPE.LTR114:
                    ltr114api ltr114 = new ltr114api();
                    //ltr114.Open(CrateObject.CSN, (ushort)Slot);
                    return ltr114;
                case _LTRNative.MODULETYPE.LTR210:
                    ltr210api ltr210 = new ltr210api();
                    //ltr210.Open(CrateObject.CSN, Slot);
                    return ltr210;
                case _LTRNative.MODULETYPE.LTR212:
                    ltr212api ltr212 = new ltr212api();
                    //ltr212.Open(CrateObject.CSN, (ushort)Slot);
                    return ltr212;
                case _LTRNative.MODULETYPE.LTR22:
                    ltr22api ltr22 = new ltr22api();
                    //ltr22.Open(CrateObject.CSN, (ushort)Slot);
                    return ltr22;
                case _LTRNative.MODULETYPE.LTR24:
                    ltr24api ltr24 = new ltr24api();
                    //ltr24.Open(CrateObject.CSN, Slot);
                    return ltr24;
                case _LTRNative.MODULETYPE.LTR25:
                    ltr25api ltr25 = new ltr25api();
                    //ltr25.Open(CrateObject.CSN, Slot);
                    return ltr25;
                case _LTRNative.MODULETYPE.LTR27:
                    ltr27api ltr27 = new ltr27api();
                    //ltr27.Open(CrateObject.CSN, (ushort)Slot);
                    return ltr27;
                case _LTRNative.MODULETYPE.LTR34:
                    ltr34api ltr34 = new ltr34api();
                    //ltr34.Open(CrateObject.CSN, (ushort)Slot);
                    return ltr34;
                case _LTRNative.MODULETYPE.LTR41:
                    ltr41api ltr41 = new ltr41api();
                    //ltr41.Open(CrateObject.CSN, (ushort)Slot);
                    return ltr41;
                case _LTRNative.MODULETYPE.LTR42:
                    ltr42api ltr42 = new ltr42api();
                    //ltr42.Open(CrateObject.CSN, (ushort)Slot);
                    return ltr42;
                case _LTRNative.MODULETYPE.LTR43:
                    ltr43api ltr43 = new ltr43api();
                    //ltr43.Open(CrateObject.CSN, (ushort)Slot);
                    return ltr43;
                case _LTRNative.MODULETYPE.LTR51:
                    ltr51api ltr51 = new ltr51api();
                    //ltr51.Open(CrateObject.CSN, (ushort)Slot);
                    return ltr51;

                default:
                    break;
            }

            return null;
        }
    }
}
