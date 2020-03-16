using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    class JoinManager
    {
        DBConn dBConn = MainWindow.GetDBConn();

        /// <summary>
        /// 해당 사용자의 스케줄테이블 생성
        /// </summary>
        /// <param name="loingData"></param>
        public void Create(LoginData loingData)
        {
            string sql = "(Date char(10), OnTime char(5), OffTime char(5), Time char(5), RestTime char(5), ExtensionTime char(5), NightTime char(5), " +
                "TotalTime char(5), Wage varchar(6), RestWage varchar(6), ExtensionWage varchar(6), NightWage varchar(6), TotalWage varchar(6), primary key(\"date\"))";

            dBConn.DBOpen();
            dBConn.Create(loingData.Phone, sql);
            dBConn.DBClose();
        }

        /// <summary>
        /// 사용자 추가
        /// </summary>
        /// <param name="loinData">추가할 사용자 정보</param>
        /// <returns>영양받은 행수</returns>
        public int Insert(LoginData loinData)
        {
            int result = -1;
            string sql = "values(\""+loinData.Phone+"\",\""+loinData.Password+"\",\""+loinData.Name+"\",\""+loinData.Wage+"\", "+loinData.Authority+")";

            dBConn.DBOpen();
            result = dBConn.Insert(DataBaseData.TableMember, sql);
            dBConn.DBClose();

            return result;
        }
    }
}
