using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Schcduler
{
    class AuthorityManager
    {
        SQLiteManager dBConn = MainWindow.GetSqliteManager();

        public AuthorityData Select()
        {
            SQLiteDataReader rdr;
            SQLiteCommand command;
            string sql = " where Authority=" + MemberData.GetMemberData.AuthorityData.Authority;

            dBConn.DBOpen();

            command = dBConn.Select(SQLiteData.TableAuthority, sql);
            rdr = command.ExecuteReader();

            AuthorityData authorityData = new AuthorityData();

            while (rdr.Read())
            {
                authorityData.Authority = Convert.ToInt32(rdr["Authority"]);
                authorityData.SignUp = Convert.ToInt32(rdr["SignUp"]);
                authorityData.Remove = Convert.ToInt32(rdr["Remove"]);
                authorityData.Modify = Convert.ToInt32(rdr["Modify"]);
                authorityData.Search = Convert.ToInt32(rdr["Search"]);
                authorityData.Schedule = Convert.ToInt32(rdr["Schedule"]);
            }

            dBConn.DBClose();

            return authorityData;
        }

        /// <summary>
        /// 근태관리 패이지의 권한이 있는지 확인
        /// </summary>
        /// <param name="obj">근태관리 페이지 버튼</param>
        /// <returns>
        /// 1 : 권한있음
        /// 0 : 없음
        /// </returns>
        public int AuthorityCheck(object obj)
        {
            int result = -1;
            Button button = (Button)obj;
            //근태관리 페이지 권한
            if (MemberData.GetMemberData.AuthorityData.Authority == 0)
            {
                button.Visibility = Visibility.Hidden;
                result = 0;
            }
            else
            {
                button.Visibility = Visibility.Visible;
                result = 1;
            }
            return result;
        }
        /// <summary>
        /// 회원가입 페이지의 권한이 있는지 확인
        /// </summary>
        /// <param name="obj">근태관리 페이지 버튼</param>
        /// <returns>
        /// 1 : 권한있음
        /// 0 : 없음
        /// </returns>
        public int SignUpCheck(object obj)
        {
            int result = -1;
            Button button = (Button)obj;
            //회원가입 페이지 권한
            if (MemberData.GetMemberData.AuthorityData.SignUp == 1)
            {
                button.Visibility = Visibility.Hidden;
                result = 0;
            }
            else
            {
                button.Visibility = Visibility.Visible;
                result = 1;
            }
            return result;
        }

        /// <summary>
        /// 근무관리페이지의 권한이 있는지 확인
        /// </summary>
        /// <param name="obj">근무관리페이지버튼</param>
        /// <returns>        
        /// 1 : 권한있음
        /// 0 : 없음
        /// </returns>
        public int ScheduleCheck(object obj)
        {
            int result = -1;
            Button button = (Button)obj;
            //스케줄 페이지 권한
            if(MemberData.GetMemberData.AuthorityData.Schedule == 1)
            {
                button.Visibility = Visibility.Hidden;
                result = 0;
            }
            else
            {
                button.Visibility = Visibility.Visible;
                result = 1;
            }
            return result;
        }

    }
}
