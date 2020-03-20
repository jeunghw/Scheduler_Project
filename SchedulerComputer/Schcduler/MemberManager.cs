using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    class MemberManager
    {
        DBConn dBConn = MainWindow.GetDBConn();
        EncryptionManager encryptionManager = new EncryptionManager();

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

            sql = "where Phone=\"" + loginData.Phone + "\"";

            dBConn.DBOpen();

            command = dBConn.Select(DataBaseData.TableMember, sql);
            rdr = command.ExecuteReader();

            while (rdr.Read())
            {
                login.Phone = rdr["Phone"].ToString();
                login.Password = rdr["Password"].ToString();
            }

            dBConn.DBClose();

            return login;
        }

        public List<LoginData> SelectName()
        {
            SQLiteCommand command;
            SQLiteDataReader reader;
            List<LoginData> loginDataList = new List<LoginData>();

            dBConn.DBOpen();

            command = dBConn.Select(DataBaseData.TableMember, "");
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                LoginData loginData = new LoginData();

                loginData.Phone = reader["Phone"].ToString();
                loginData.Name = reader["Name"].ToString();

                if (!loginData.Phone.Equals("00000000000"))
                {
                    loginDataList.Add(loginData);
                }
            }

            dBConn.DBClose();

            return loginDataList;
        }

        /// <summary>
        /// 해당 사용자의 스케줄테이블 생성
        /// </summary>
        /// <param name="loingData"></param>
        public void Create(LoginData loingData)
        {
            string sql = "(Date char(10), OnTime char(5), OffTime char(5), Time char(5), RestTime char(5), ExtensionTime char(5), NightTime char(5), " +
                "TotalTime char(5), Wage varchar(6), RestWage varchar(6), ExtensionWage varchar(6), NightWage varchar(6), TotalWage varchar(6), primary key(\"date\"))";

            //년도가 뀌어서 테이블 생성시 ID가 있는지 확인해서 있으면 테이블 생성
            if (!Select(loingData).Phone.Equals(""))
            {
                dBConn.DBOpen();
                dBConn.Create(loingData.Phone + DateTime.Now.ToString("yyyy"), sql); ;
                dBConn.DBClose();
            }
        }

        /// <summary>
        /// 사용자 추가
        /// </summary>
        /// <param name="loinData">추가할 사용자 정보</param>
        /// <returns>영양받은 행수</returns>
        public int Insert(LoginData loinData)
        {
            int result = -1;
            string sql = "values(\"" + loinData.Phone + "\",\"" + encryptionManager.EncryptionPassword(loinData) + "\",\"" + loinData.Name + "\",\"" + loinData.Wage + "\", " + loinData.Authority + ")";

            dBConn.DBOpen();
            result = dBConn.Insert(DataBaseData.TableMember, sql);
            dBConn.DBClose();

            return result;
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
            string sql = "where Phone=\"" + loginData.Phone + "\"";

            dBConn.DBOpen();

            command = dBConn.Select(DataBaseData.TableMember, sql);
            rdr = command.ExecuteReader();

            while (rdr.Read())
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
