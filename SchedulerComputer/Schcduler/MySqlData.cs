using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    public static class MySQLData
    {
        /// <summary>
        /// MySql 서버 IP
        /// </summary>
        public static string ServerIP = "15.164.228.119";
        /// <summary>
        /// MySql 데이터베이스 이름
        /// </summary>
        public static string DataBase = "Scheduler";
        /// <summary>
        /// MySql ID
        /// </summary>
        public static string ID = "user";
        /// <summary>
        /// MySql PW
        /// </summary>
        public static string PW = "user1234";
        /// <summary>
        /// MySql Port
        /// </summary>
        public static string Port = "3306";
        /// <summary>
        /// 회원테이블이름
        /// </summary>
        public static string TableMember = "Member";
        /// <summary>
        /// 출퇴근시간테이블이름
        /// </summary>
        public static string TableWage = "Wage";
        /// <summary>
        /// 셋팅관련테이블이름
        /// </summary>
        public static string TableAuthority = "Authority";
        /// <summary>
        /// 근무관리테이블 이름
        /// </summary>
        public static string TableSchedule = "Schedule";
    }
}
