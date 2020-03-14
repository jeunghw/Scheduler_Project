using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    class LoginManager
    {
        String sql = "";
        DBConn dBConn = MainWindow.GetDBConn();
        SQLiteDataReader rdr;

        /// <summary>
        /// 멤버테이블에서 select해오는 메소드
        /// </summary>
        /// <returns></returns>
        public MemberData selectMember()
        {
            sql = "select * from " + DBInfo.TableMember + " where phone = \"" + LoginData.GetLoginData.LoginPhone + "\"";

            dBConn.DBOpen();
            rdr = dBConn.DBSelect(sql);

            MemberData memberData = new MemberData();

            while (rdr.Read())
            {
                memberData.Phone = rdr["phone"].ToString();
                memberData.Password = rdr["password"].ToString();
                memberData.Name = rdr["name"].ToString();
                memberData.Wage = rdr["wage"].ToString();
            }

            dBConn.DBClose();

            return memberData;

        }
    }
}
