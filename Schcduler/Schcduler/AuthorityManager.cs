using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    class AuthorityManager
    {
        DBConn dBConn = MainWindow.GetDBConn();

        public AuthorityData Select()
        {
            SQLiteDataReader rdr;
            SQLiteCommand command;
            string sql = " where Authority=" + MemberData.GetMemberData.AuthorityData.Authority;

            dBConn.DBOpen();

            command = dBConn.Select(DataBaseData.TableAuthority, sql);
            rdr = command.ExecuteReader();

            AuthorityData authorityData = new AuthorityData();

            while (rdr.Read())
            {
                authorityData.Authority = Convert.ToInt32(rdr["Authority"]);
                authorityData.SignUp = Convert.ToInt32(rdr["SignUp"]);
                authorityData.Modify = Convert.ToInt32(rdr["Modify"]);
                authorityData.Search = Convert.ToInt32(rdr["Search"]);
            }

            dBConn.DBClose();

            return authorityData;
        }

        public List<LoginData> SelectName()
        {
            SQLiteCommand command;
            SQLiteDataReader reader;
            List<LoginData> loginDataList = new List<LoginData>();
            
            dBConn.DBOpen();

            command = dBConn.Select(DataBaseData.TableMember, "");
            reader = command.ExecuteReader();

            while(reader.Read())
            {
                LoginData loginData = new LoginData();

                loginData.Phone = reader["Phone"].ToString();
                loginData.Name = reader["Name"].ToString();

                if (!loginData.Phone.Equals("00000000000"))
                {
                    loginDataList.Add(loginData);
                }
            }

            return loginDataList;
        }
    }
}
