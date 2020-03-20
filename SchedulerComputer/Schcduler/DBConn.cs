using System;
using System.Data.SQLite;
using System.IO;

namespace Schcduler
{
    public class DBConn
    {
        private string Path = Directory.GetCurrentDirectory() + "\\..\\..\\..\\DB";                   //데이터베이스 위치
        protected SQLiteConnection conn = null;                                                     //SQL커낵션을 위한 객체


        public DBConn()
        {
            DBDirectoryCreate();
            DBInit();
        }
        /// <summary>
        /// 데이터베이스 열기
        /// </summary>
        public void DBOpen()
        {
            try
            {
                conn = new SQLiteConnection("Data Source=" + Path + "\\" + DataBaseData.DbScheduler);
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("데이터베이스 열기 실패 : " + ex.Message);
            }
        }
        /// <summary>
        /// 데이터베이스 닫기
        /// </summary>
        public void DBClose()
        {
            try
            {
                conn.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine("데이터베이스 닫기 실패 : " + ex.Message);
            }
        }
        /// <summary>
        /// 데이터베이스 저장경로 생성
        /// </summary>
        private void DBDirectoryCreate()
        {
            try
            {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("데이터베이스 저장경로 생성 실패 : " + ex.Message);
            }
        }
        /// <summary>
        /// 데이터베이스 초기화
        /// </summary>
        private void DBInit()
        {
            CreateTable();
        }

        /// <summary>
        /// 기본테이븰 생성
        /// </summary>
        private void CreateTable()
        {
            string sql = "";

            DBOpen();

            if (!CheckTable(DataBaseData.TableMember))
            {
                sql = "(Phone char(11), Password varchar(50), Name varchar(8), Wage varchar(8), Authority int(2), primary Key(\"Phone\"))";
                Create(DataBaseData.TableMember, sql);
                MemberDataInsert();
            }

            if (!CheckTable(DataBaseData.TableAuthority))
            { 
                sql = "(Authority char(2), SignUp char(1), Remove char(1), Modify char(1), Search char(1), primary Key(\"Authority\"))";
                Create(DataBaseData.TableAuthority, sql);
                AuthorityInsert();
            }

            DBClose();
        }

        /// <summary>
        /// 멤버 테이블 기본값 입력
        /// </summary>
        private void MemberDataInsert()
        {
            EncryptionManager encryptionManager = new EncryptionManager();
            LoginData loginData = new LoginData();
            loginData.Phone = "00000000000";
            loginData.Password = "0000";
            string password = encryptionManager.EncryptionPassword(loginData);
            string sql = "values(\"00000000000\",\"" + password + "\",\"관리자\",\"8590\",0)";

            Insert(DataBaseData.TableMember, sql);
        }

        /// <summary>
        /// 권한테이블 기본값 입력
        /// </summary>
        private void AuthorityInsert()
        {
            string sql = "";

            sql = "values(0,0,1,1,1)";
            Insert(DataBaseData.TableAuthority, sql);

            sql = "values(1,0,0,0,0)";
            Insert(DataBaseData.TableAuthority, sql);

            sql = "values(2,0,1,0,0)";
            Insert(DataBaseData.TableAuthority, sql);

            sql = "values(3,1,1,0,0)";
            Insert(DataBaseData.TableAuthority, sql);
        }
        /// <summary>
        /// 테이블이 있는지 확인
        /// </summary>
        /// <param name="tableName">확인할 테이블 이름</param>
        /// <returns>
        /// true : 있음
        /// false : 없음
        /// </returns>
        public bool CheckTable(string tableName)
        {
            SQLiteCommand command = null;
            SQLiteDataReader reader;
            bool result = false;
            string sql = "Select name from sqlite_master where name=\"" + tableName + "\"";

            try
            {
                command = new SQLiteCommand(sql, conn);
                reader = command.ExecuteReader();
                reader.Read();
                if (reader["name"].ToString().Equals(""))
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine("데이터베이스 테이블 체크 실패 : " + ex.Message);
            }

            return result;
        }
        /// <summary>
        /// 테이블 생성
        /// </summary>
        /// <param name="tableName">테이블이름</param>
        /// <param name="inputSql">추가Sql</param>
        /// <returns>영양받은 행수</returns>
        public int Create(string tableName, string inputSql)
        {
            SQLiteCommand command = null;
            int result = -1;
            string sql = "Create table \"" + tableName +"\" "+ inputSql;

            if (!CheckTable(tableName))
            {
                try
                {
                    command = new SQLiteCommand(sql, conn);
                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(tableName + "테이블생성실패 : " + ex.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// 테이블 제거
        /// </summary>
        /// <param name="tableName">제거할 테이블 명</param>
        /// <returns></returns>
        public int Drop(string tableName)
        {
            SQLiteCommand command = null;
            SQLiteDataReader reader = null;
            int result = -1;
            string sql = "Select name from sqlite_master where name like \""+tableName+"%\"";
            string tableNamePull = "";

            try
            {
                command = new SQLiteCommand(sql, conn);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tableNamePull = reader["name"].ToString();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("DBConn:Drop 테이블 검색 실패 :" + e.Message);
            }

            sql = "Drop table \"" + tableNamePull + "\"";

            if (CheckTable(tableNamePull))
            {
                try
                {
                    command = new SQLiteCommand(sql, conn);
                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(tableNamePull + "테이블삭제실패 : " + ex.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// 테이블 데이터 검색
        /// </summary>
        /// <param name="tableName">테이블이름</param>
        /// <param name="inputSql">추가Sql</param>
        /// <returns>영양받은 행수</returns>
        public SQLiteCommand Select(string tableName, string inputSql)
        {
            SQLiteCommand command = null;

            if (CheckTable(tableName))
            {
                try
                {
                    string sql = "Select * from \"" + tableName + "\" " + inputSql;
                    command = new SQLiteCommand(sql, conn);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(tableName + "테이블검색실패 : " + ex.Message);
                }
            }

            return command;
        }
        /// <summary>
        /// 테이블 데이터 수정
        /// </summary>
        /// <param name="tableName">테이블이름</param>
        /// <param name="inputSql">추가Sql</param>
        /// <returns>영양받은 행수</returns>
        public int Update(string tableName, string inputSql)
        {
            SQLiteCommand command = null;
            int result = -1;
            string sql = "Update \"" + tableName + "\" set " + inputSql;

            if (CheckTable(tableName))
            {
                try
                {
                    command = new SQLiteCommand(sql, conn);
                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(tableName + "테이블갱신실패 : " + ex.Message);
                }
            }

            return result;
        }
        /// <summary>
        /// 테이블 데이터 삭제
        /// </summary>
        /// <param name="tableName">테이블이름</param>
        /// <param name="inputSql">추가Sql</param>
        /// <returns>영양받은 행수</returns>
        public int Delete(string tableName, string inputSql)
        {
            SQLiteCommand command = null;
            int result = -1;
            string sql = "Delete from \"" + tableName + "\" "+ inputSql;

            if (CheckTable(tableName))
            {
                try
                {
                    command = new SQLiteCommand(sql, conn);
                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(tableName + "테이블삭제실패 : " + ex.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// 테이블 데이터 삽입
        /// </summary>
        /// <param name="tableName">테이블이름</param>
        /// <param name="inputSql">추가Sql</param>
        /// <returns>영양받은 행수</returns>
        public int Insert(string tableName, string inputSql)
        {
            SQLiteCommand command = null;
            int result = -1;
            string sql = "Insert into \""+ tableName + "\" "+ inputSql;

            if (CheckTable(tableName))
            {
                try
                {
                    command = new SQLiteCommand(sql, conn);
                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(tableName + "테이블삽입실패 : " + ex.Message);
                }
            }

            return result;
        }
    }
}
