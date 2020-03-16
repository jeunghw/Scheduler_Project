using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    public static class DataBaseData
    {
        /// <summary>
        /// DB이름
        /// </summary>
        public static string DbScheduler = "Scheduler.sqlite";
        /// <summary>
        /// 회원테이블이름
        /// </summary>
        public static string TableMember = "Member";
        /// <summary>
        /// 출퇴근시간테이블이름
        /// </summary>
        public static string TableSchedule = "Schedule";
        /// <summary>
        /// 셋팅관련테이블이름
        /// </summary>
        public static string TableAuthority = "Authority";
    }
}
