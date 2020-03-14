using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    public static class DBInfo
    {
        /// <summary>
        /// DB이름
        /// </summary>
        public static String DbScheduler = "Scheduler.sqlite";
        /// <summary>
        /// 회원테이블이름
        /// </summary>
        public static String TableMember = "Member";
        /// <summary>
        /// 출퇴근시간테이블이름
        /// </summary>
        public static String TableSchedule = "Schedule";
        /// <summary>
        /// 셋팅관련테이블이름
        /// </summary>
        public static String TableSetting = "Setting";
    }
}
