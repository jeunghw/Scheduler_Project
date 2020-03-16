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
        DBConn dBConn = MainWindow.GetDBConn();

        /// <summary>
        /// 사용자 데이터 검색
        /// </summary>
        /// <param name="loginData">입력된 사용자 데이터</param>
        /// <returns>검색된 사용자 데이터</returns>
        public LoginData Select(LoginData loginData)
        {
            SQLiteDataReader rdr;
            SQLiteCommand command;
            LoginData login = new LoginData();
            string sql;

            sql= "where Phone=\""+loginData.Phone+"\"";

            dBConn.DBOpen();

            command = dBConn.Select(DataBaseData.TableMember, sql);
            rdr = command.ExecuteReader();

            while(rdr.Read())
            {
                login.Phone = rdr["Phone"].ToString();
                login.Password = rdr["Password"].ToString();
            }

            dBConn.DBClose();

            return login;
        }
        /// <summary>
        /// 현재사용자 정보를 저장
        /// </summary>
        /// <param name="loginData">입력된 사용자 데이터</param>
        public void SetMemberData(LoginData loginData)
        {
            SQLiteDataReader rdr;
            SQLiteCommand command;
            AuthorityManager authorityManager = new AuthorityManager();
            string sql = "where Phone=\""+loginData.Phone+"\"";

            dBConn.DBOpen();

            command = dBConn.Select(DataBaseData.TableMember, sql);
            rdr = command.ExecuteReader();

            while(rdr.Read())
            {
                MemberData.GetMemberData.Phone = rdr["Phone"].ToString();
                MemberData.GetMemberData.Password = rdr["Password"].ToString();
                MemberData.GetMemberData.Name = rdr["Name"].ToString();
                MemberData.GetMemberData.Wage = rdr["Wage"].ToString();
                MemberData.GetMemberData.AuthorityData.Authority = Convert.ToInt32(rdr["Authority"]);
            }

            dBConn.DBClose();

            MemberData.GetMemberData.AuthorityData = authorityManager.Select();


        }
    }
}
