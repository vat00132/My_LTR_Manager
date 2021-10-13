using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTRManager.Model
{
    public class LogInformation
    {
        /// <summary>
        /// Дата сообщения
        /// </summary>
        public DateTime Date { get; }
        /// <summary>
        /// Тип сообщения
        /// </summary>
        public string TypeMessage { get; }
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; }
        public LogInformation(string typeMes, string mes)
        {
            Date = DateTime.Now;
            TypeMessage = typeMes;
            Message = mes;
        }
    }
}
