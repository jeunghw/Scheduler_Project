using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    public class MySQLManager
    {
        MySqlConnection connection = null;

        public MySQLManager()
        {
            try
            {
                //Mysql 연결
                connection = new MySqlConnection("Server=" + MySQLData.ServerIP + ";Port=" + MySQLData.Port + ";Database=" + MySQLData.DataBase + ";Uid=" + MySQLData.ID + ";Pwd=" + MySQLData.PW);
            }
            catch(MySqlException e)
            {
                Console.WriteLine("MySql연결 실패 : " + e.Message);
            }
        }

        public void DBOpen()
        {
            connection.Open();
        }

        public void DBClose()
        {
            connection.Close();
        }

        /// <summary>
        /// DB에서 Create 관련 쿼리 실행
        /// </summary>
        /// <param name="type">Create할 객체(Database, table등)</param>
        /// <param name="name">생성객체 이름</param>
        /// <param name="inputSql">이름 이후의 SQL문</param>
        /// <returns>실패 : -1 </returns>
        public int Create(string type, string name, string inputSql)
        {
            int result = -1;
            string sql = "create " + type + " " + name + " " + inputSql;
            sql = sql.Replace("\"", "\'");

            try
            {
                MySqlCommand command = new MySqlCommand(sql, connection);
                result = command.ExecuteNonQuery();
            }
            catch(MySqlException e)
            {
                Console.WriteLine(type + "생성 실패 : " + e.Message);
            }

            return result;
        }

        /// <summary>
        /// DB에서 Select 관련 쿼리 실행
        /// </summary>
        /// <param name="tableName">테이블이름</param>
        /// <param name="inputSql">추가 sql문</param>
        /// <returns>영양받은 행</returns>
        public MySqlDataReader Select(string tableName, string inputSql)
        {
            MySqlDataReader reader = null;
            string sql = "Select * from " + tableName + " " + inputSql;
            sql = sql.Replace("\"", "\'");

            try
            {
                MySqlCommand command = new MySqlCommand(sql, connection);
                reader = command.ExecuteReader();
            }
            catch(MySqlException e)
            {
                Console.WriteLine("검색실패 : " + e.Message);
            }

            return reader;
        }

        /// <summary>
        /// DB에서 Update 관련 쿼리 실행
        /// </summary>
        /// <param name="tableName">테이블이름</param>
        /// <param name="inputSql">추가 sql문</param>
        /// <returns>영양받은 행</returns>
        public int Update(string tableName, string inputSql)
        {
            int result = -1;
            string sql = "Update " + tableName + " set " + inputSql;
            sql = sql.Replace("\"", "\'");

            try
            {
                MySqlCommand command = new MySqlCommand(sql, connection);
                result = command.ExecuteNonQuery();
            }
            catch(MySqlException e)
            {
                Console.WriteLine("갱신실패 : " + e.Message);
            }

            return result;
        }

        /// <summary>
        /// DB에서 Delete 관련 쿼리 실행
        /// </summary>
        /// <param name="tableName">테이블이름</param>
        /// <param name="inputSql">추가 sql문</param>
        /// <returns>영양받은 행</returns>
        public int Delete(string tableName, string inputSql)
        {
            int result = -1;
            string sql = "Delete from " + tableName + " " + inputSql;
            sql = sql.Replace("\"", "\'");

            try
            {
                MySqlCommand command = new MySqlCommand(sql, connection);
                result = command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine("삭제실패 : " + e.Message);
            }

            return result;
        }

        /// <summary>
        /// DB에서 Insert 관련 쿼리 실행
        /// </summary>
        /// <param name="tableName">테이블이름</param>
        /// <param name="inputSql">추가 sql문</param>
        /// <returns>영양받은 행</returns>
        public int Insert(string tableName, string inputSql)
        {
            int result = -1;
            string sql = "Insert into " + tableName + " " + inputSql;
            sql = sql.Replace("\"", "\'");

            try
            {
                MySqlCommand command = new MySqlCommand(sql, connection);
                result = command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine("삽입실패 : " + e.Message);
            }

            return result;
        }

        
    }
}
