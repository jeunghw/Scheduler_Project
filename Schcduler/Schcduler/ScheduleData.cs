using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    public class ScheduleData
    {
        public ScheduleData()
        {
            Phone = "";
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
        /// <summary>
        /// 핸드폰번호
        /// </summary>
        public String Phone { get; set; }
        /// <summary>
        /// 날짜
        /// </summary>
        public String Date { get; set; }
        /// <summary>
        /// 출근시간
        /// </summary>
        public String OnTime { get; set; }
        /// <summary>
        /// 퇴근시간
        /// </summary>
        public String OffTime { get; set; }
        /// <summary>
        /// 일반시간
        /// </summary>
        public String Time { get; set; }
        /// <summary>
        /// 휴계시간
        /// </summary>
        public String RestTime { get; set; }
        /// <summary>
        /// 연장시간
        /// </summary>
        public String ExtensionTime { get; set; }
        /// <summary>
        /// 야간시간
        /// </summary>
        public String NightTime { get; set; }
        /// <summary>
        /// 총시간
        /// </summary>
        public String TotalTime { get; set; }
        /// <summary>
        /// 일반시간시급
        /// </summary>
        public String Wage { get; set; }
        /// <summary>
        /// 휴계시간시급
        /// </summary>
        public String RestWage { get; set; }
        /// <summary>
        /// 연장시간시급
        /// </summary>
        public String ExtensionWage { get; set; }
        /// <summary>
        /// 야간시간시급
        /// </summary>
        public String NightWage { get; set; }
        /// <summary>
        /// 총시급
        /// </summary>
        public String TotalWage { get; set; }
    }
}
