using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    public class DBConn
    {
        private string Paht = Directory.GetCurrentDirectory() + "\\..\\..\\..\\DB";                   //데이터베이스 위치
        protected SQLiteConnection conn = null;                                                     //SQL커낵션을 위한 객체
        string sql;
        SQLiteCommand command;
        int result;
        SQLiteDataReader rdr;

        public DBConn()
        {
            try
            {
                if (!Directory.Exists(Paht))
                {
                    DBCreate();
                }

                DBOpen();
                TableCreate();
            }
            catch (Exception e)
            {
                Console.WriteLine("데이터베이스 연결실패", e.Message);
            }
            DBClose();
        }

        private void TableCreate()
        {
            if (CheckTable(DBInfo.TableMember).Equals(""))
            {
                //회원테이블 생성
                sql = "create table " + DBInfo.TableMember +
                    " ( Phone char(11) primary key, Password varchar(8), Name varchr(8), Wage varchar(8))";
                TableCreate(sql);
            }
            if (CheckTable(DBInfo.TableSchedule).Equals(""))
            {
                //스케줄테이블 생성
                sql = "create table " + DBInfo.TableSchedule +
                    " ( Phone char(11), Date char(10), OnTime char(5), OffTime char(5), Time char(5), RestTime char(5), ExtensionTime char(5), NightTime char(5), TotalTime char(5)," +
                    "Wage varchar(6), RestWage varchar(6), ExtensionWage varchar(6), NightWage varchar(6), TotalWage varchar(6), primary key(\"phone\",\"date\"))";
                TableCreate(sql);
            }
            /*if(CheckTable(DBInfo.TableSetting).Equals(""))
            {
                //셋팅테이블 생성
                sql = "create table " + DBInfo.TableSetting +
                    " ( number char(1) primary key, wage varchar(8))";
                TableCreate(sql);
            }*/

            //관리자계정 생성
            sql = "insert into member (Phone, Password, name, Wage) values(\"00000000000\",\"0000\",\"관리자\",\"8590\")";

            result = DBManipulation(sql);

            if (result == 0)
            {
                Console.WriteLine("관리자계정 생성실패");
            }
        }

        private String CheckTable(String tableName)
        {
            sql = "select name from sqlite_master where name=\"" + tableName + "\"";
            rdr = DBSelect(sql);

            rdr.Read();

            return rdr["name"].ToString();
        }

        public void DBOpen()
        {
            conn = new SQLiteConnection("Data Source=" + Paht + "\\" + DBInfo.DbScheduler);
            conn.Open();
        }

        public void DBClose()
        {
            conn.Close();
        }

        public SQLiteDataReader DBSelect(string sql)
        {
            command = new SQLiteCommand(sql, conn);
            rdr = command.ExecuteReader();

            return rdr;
        }

        public SQLiteDataAdapter DBSelect2(string sql)
        {
            return new SQLiteDataAdapter(new SQLiteCommand(sql, conn));
        }

        public int DBSelect_Count(string sql)
        {
            command = new SQLiteCommand(sql, conn);
            return Convert.ToInt32(command.ExecuteScalar());
        }

        /// <summary>
        /// DML전송시 사용
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int DBManipulation(string sql)
        {
            command = new SQLiteCommand(sql, conn);
            result = command.ExecuteNonQuery();

            return result;
        }

        public void DBCreate()
        {
            Directory.CreateDirectory(Paht);
        }

        private void TableCreate(string sql)
        {
            command = new SQLiteCommand(sql, conn);
            result = command.ExecuteNonQuery();
            Console.WriteLine(result);
        }
    }
}
