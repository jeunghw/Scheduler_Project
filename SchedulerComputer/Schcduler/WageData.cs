using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    public class WageData
    {
        public WageData()
        {
            Date = "";
            OnTime = "";
            OffTime = "";
            Time = "";
            RestTime = "";
            ExtensionTime = "";
            NightTime = "";
            TotalTime = "";
            Wage = "";
            RestWage = "";
            ExtensionWage = "";
            NightWage = "";
            TotalWage = "";
        }
        /// 날짜
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 출근시간
        /// </summary>
        public string OnTime { get; set; }
        /// <summary>
        /// 퇴근시간
        /// </summary>
        public string OffTime { get; set; }
        /// <summary>
        /// 일반시간
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 휴계시간
        /// </summary>
        public string RestTime { get; set; }
        /// <summary>
        /// 연장시간
        /// </summary>
        public string ExtensionTime { get; set; }
        /// <summary>
        /// 야간시간
        /// </summary>
        public string NightTime { get; set; }
        /// <summary>
        /// 총시간
        /// </summary>
        public string TotalTime { get; set; }
        /// <summary>
        /// 일반시간시급
        /// </summary>
        public string Wage { get; set; }
        /// <summary>
        /// 휴계시간시급
        /// </summary>
        public string RestWage { get; set; }
        /// <summary>
        /// 연장시간시급
        /// </summary>
        public string ExtensionWage { get; set; }
        /// <summary>
        /// 야간시간시급
        /// </summary>
        public string NightWage { get; set; }
        /// <summary>
        /// 총시급
        /// </summary>
        public string TotalWage { get; set; }
    }
}
