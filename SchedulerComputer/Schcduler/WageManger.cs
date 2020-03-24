using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;

namespace Schcduler
{
    class WageManger
    {
        DBManager dBConn = MainWindow.GetDBConn();


        public ScheduleData Select(string tableName, string date)
        {
            SQLiteCommand command;
            SQLiteDataReader reader;
            ScheduleData scheduleData = new ScheduleData();
            string sql = "where Date=\""+date+"\"";

            string year = SplitString(date, '-')[0];

            dBConn.DBOpen();

            command = dBConn.Select(tableName+ year, sql);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                scheduleData.Date = reader["Date"].ToString();
                scheduleData.OnTime = reader["OnTime"].ToString();
                scheduleData.OffTime = reader["OffTime"].ToString();
                scheduleData.Time = reader["Time"].ToString();
                scheduleData.RestTime = reader["RestTime"].ToString();
                scheduleData.ExtensionTime = reader["ExtensionTime"].ToString();
                scheduleData.NightTime = reader["NightTime"].ToString();
                scheduleData.TotalTime = reader["TotalTime"].ToString();
                scheduleData.Wage = reader["Wage"].ToString();
                scheduleData.RestWage = reader["RestWage"].ToString();
                scheduleData.ExtensionWage = reader["ExtensionWage"].ToString();
                scheduleData.NightWage = reader["NightWage"].ToString();
                scheduleData.TotalWage = reader["TotalWage"].ToString();
            }

            dBConn.DBClose();

            return scheduleData;
        }
        /// <summary>
        /// 스케줄 테이터 Update
        /// </summary>
        /// <param name="tableName">입력할 테이블이름</param>
        /// <param name="scheduleData">입력할 데이터 구조체</param>
        /// <returns>영양받은 행수</returns>
        public int Update(string tableName, ScheduleData scheduleData)
        {
            int result = -1;
            string sql = "OnTime = \"" + scheduleData.OnTime + "\", OffTime = \"" + scheduleData.OffTime + "\", Time = \"" + scheduleData.Time + "\", RestTime = \"" + scheduleData.RestTime +
                "\", ExtensionTime = \"" + scheduleData.ExtensionTime + "\", NightTime = \"" + scheduleData.NightTime + "\", TotalTime = \"" + scheduleData.TotalTime + "\", Wage = \"" 
                + scheduleData.Wage + "\", RestWage = \"" + scheduleData.RestWage + "\", " + "ExtensionWage = \"" + scheduleData.ExtensionWage + "\", NightWage = \"" + scheduleData.NightWage + 
                "\", TotalWage = \"" + scheduleData.TotalWage + "\" where Date = \"" + scheduleData.Date + "\"";

            string year = SplitString(scheduleData.Date, '-')[0];

            dBConn.DBOpen();
            dBConn.Update(tableName+ year, sql);
            dBConn.DBClose();

            return result;
        }
        /// <summary>
        /// 스케줄 데이터 Insert
        /// </summary>
        /// <param name="tableName">입력할 테이블이름</param>
        /// <param name="scheduleData">입력할 데이터 구조체</param>
        /// <returns>영양받은 행수</returns>
        public int Insert(string tableName, ScheduleData scheduleData)
        {
            int result = -1;
            string sql = "values(\"" +scheduleData.Date+ "\", \"" + scheduleData.OnTime + "\", \"" + scheduleData.OffTime + "\", \"" + scheduleData.Time + "\", \"" + scheduleData.RestTime + "\", \""
                + scheduleData.ExtensionTime + "\", \"" + scheduleData.NightTime + "\", \"" + scheduleData.TotalTime + "\", \"" + scheduleData.Wage + "\", \"" + scheduleData.RestWage +
                "\", \"" + scheduleData.ExtensionWage + "\", \"" + scheduleData.NightWage + "\", \"" + scheduleData.TotalWage + "\")";

            string year = SplitString(scheduleData.Date, '-')[0];

            dBConn.DBOpen();
            dBConn.Insert(tableName+ year, sql);
            dBConn.DBClose();

            return result;
        }

        /// <summary>
        /// 스케줄 데이터 Delete
        /// </summary>
        /// <param name="tableName">테이블이름</param>
        /// <param name="scheduleData">지울 데이터 구조체</param>
        /// <returns>영양받은 행수</returns>
        public int Delete(string tableName, ScheduleData scheduleData)
        {
            int result = -1;
            string sql = "where Date=\"" + scheduleData.Date + "\"";

            string year = SplitString(scheduleData.Date, '-')[0];

            dBConn.DBOpen();
            dBConn.Delete(tableName+year, sql);
            dBConn.DBClose();

            return result;
        }
        /// <summary>
        /// 출근
        /// </summary>
        /// <returns></returns>
        public int OnWork(LoginData loginData)
        {
            int result = -1;
            string sql = "(Date, OnTime) values(\"" + DateTime.Now.ToString("yyyy-MM-dd") + "\",\"" + DateTime.Now.ToString("HH:mm") + "\")";

            if (!CheckDate(loginData, DateTime.Now.ToString("yyyy-MM-dd")))
            {
                dBConn.DBOpen();
                dBConn.Insert(loginData.Phone+DateTime.Now.ToString("yyyy"), sql);
                dBConn.DBClose();
            }

            return result;
        }

        /// <summary>
        /// 퇴근
        /// </summary>
        /// <returns></returns>
        public int OffWork(LoginData loginData)
        {
            int result = -1;
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("HH:mm");
            string sql;

            //24시가 넘어가서 퇴근한것인지 여부를 확인
            string[] swapTime = time.Split(':');
            string[] swapDate = date.Split('-');

            if (Convert.ToInt32(swapTime[0]) < 5)
            {
                swapTime[0] = (Convert.ToInt32(swapTime[0]) + 24).ToString();
                swapDate[2] = (Convert.ToInt32(swapDate[2]) - 1).ToString();

                time = swapTime[0] + ":" + swapTime[1];
                date = swapDate[0] + "-" + swapDate[1] + "-" + swapDate[2];

            }

            sql = "OffTime =\"" + time + "\" where Date=\"" + date + "\"";

            //출근을 했는지 확인
            if (CheckDate(loginData, date))
            {
                dBConn.DBOpen();
                result = dBConn.Update(loginData.Phone+DateTime.Now.ToString("yyyy"), sql);
                dBConn.DBClose();
            }


            return result;
        }

        /// <summary>
        /// 사용자가 출근을 했는지 확인
        /// </summary>
        /// <param name="loginData">사용자 정보</param>
        /// <returns>
        /// 출근여부
        /// true : 출근완료
        /// false : 출근안함
        /// </returns>
        private bool CheckDate(LoginData loginData, string date)
        {
            SQLiteCommand command;
            SQLiteDataReader reader;
            bool result = false;
            string sql = " where Date=\""+ date + "\"";

            string year = SplitString(date, '-')[0];

            dBConn.DBOpen();
            command = dBConn.Select(loginData.Phone+year, sql);

            if (command != null)
            {
                try
                {
                    reader = command.ExecuteReader();
                    reader.Read();
                    if (reader["Date"].ToString().Equals(""))
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine("날짜체크실패 : " + ex.Message);
                }
            }
            dBConn.DBClose();
            
            return result;
        }

        private int SelectWage(string phone)
        {
            SQLiteCommand command;
            SQLiteDataReader reader;
            string sql = "where Phone=\"" + phone + "\"";
            int result = 0;

            dBConn.DBOpen();

            command = dBConn.Select(DataBaseData.TableMember, sql);
            reader = command.ExecuteReader();

            result = Convert.ToInt32(reader["Wage"]);

            dBConn.DBClose();

            return result;
        }

        public void WageCalculation(string phone, string date)
        {
            ScheduleData scheduleData = Select(phone, date);
            int Wage = SelectWage(phone);

            //출근,퇴근 시간, 분 분리
            int OnTime, OnMinute, OffTime, OffMinute;

            string[] swap = SplitString(scheduleData.OnTime, ':');

            OnTime = Convert.ToInt32(swap[0]);
            OnMinute = Convert.ToInt32(swap[1]);

            swap = SplitString(scheduleData.OffTime, ':');

            OffTime = Convert.ToInt32(swap[0]);
            OffMinute = Convert.ToInt32(swap[1]);

            //시간들 계산
            int Time, Minute;

            //24시 이후 퇴근은 24를 더해서 계산
            if (OffTime < 5)
                OffTime += 24;

            //오전 6시 이후 출근
            if (OnTime >= 6)
            {
                //일반시간
                if (OffTime < 22)
                {
                    Time = OffTime - OnTime;
                    if (OnMinute <= OffMinute)
                    {
                        Minute = OffMinute - OnMinute;
                    }
                    else
                    {
                        Time -= 1;
                        Minute = (OffMinute + 60) - OnMinute;
                    }
                    scheduleData.Time = Time + ":" + Minute.ToString().PadLeft(2,'0');
                }
                //야간시간
                else
                {
                    if (OnTime >= 22)
                    {
                        Time = 0;
                        Minute = 0;

                        OffMinute -= OnMinute;
                    }
                    else
                    {
                        Time = 22 - OnTime;
                        Time -= 1;
                        Minute = 60 - OnMinute;

                        if (Minute >= 60)
                        {
                            Time += 1;
                            Minute -= 60;
                        }
                    }
                    scheduleData.Time = Time + ":" + Minute.ToString().PadLeft(2, '0');

                    Time = OffTime - 22;
                    Minute = OffMinute;
                    scheduleData.NightTime = Time + ":" + Minute.ToString().PadLeft(2, '0');
                }
            }
            //오전 6시 이전 출근
            else
            {
                //일반시간
                if (OffTime < 22)
                {
                    Time = OffTime - 6;
                    Minute = OffMinute;
                    scheduleData.Time = Time + ":" + Minute.ToString().PadLeft(2, '0');
                }
                //야간시간
                else
                {
                    Time = (6 - OnTime) + (OffTime - 22);
                    if (OnMinute <= OffMinute)
                    {
                        Minute = OffMinute - OnMinute;
                    }
                    else
                    {
                        Time -= 1;
                        Minute = (OffMinute + 60) - OnMinute;
                    }
                    scheduleData.NightTime = Time + ":" + Minute.ToString().PadLeft(2, '0');
                }
            }

            //총 시간
            if (!scheduleData.Time.Equals(""))
            {
                swap = SplitString(scheduleData.Time, ':');
                Time = Convert.ToInt32(swap[0]);
                Minute = Convert.ToInt32(swap[1]);
            }

            if (!scheduleData.NightTime.Equals(""))
            {
                swap = SplitString(scheduleData.NightTime, ':');
                Time += Convert.ToInt32(swap[0]);
                Minute += Convert.ToInt32(swap[1]);
            }

            if (Minute >= 60)
            {
                Time += 1;
                Minute -= 60;
            }
            scheduleData.TotalTime = Time + ":" + Minute.ToString().PadLeft(2, '0');

            //휴계시간
            if ((Time / 4) > 0)
            {
                Minute = (Time / 4) * 30;
                Time = 0;
                if (Minute >= 60)
                {
                    Time += 1;
                    Minute -= 60;
                }
                scheduleData.RestTime = Time + ":" + Minute.ToString().PadLeft(2, '0');
            }

            //연장시간
            swap = SplitString(scheduleData.TotalTime, ':');
            Time = Convert.ToInt32(swap[0]);
            Minute = Convert.ToInt32(swap[1]);
            if (Time > 8)
            {
                Time -= 8;
                scheduleData.ExtensionTime = Time + ":" + Minute.ToString().PadLeft(2, '0');

                //일반시간에서 연장시간 빼기
                int Time1, Minute1;
                swap = SplitString(scheduleData.Time, ':');
                Time = Convert.ToInt32(swap[0]);
                Minute = Convert.ToInt32(swap[1]);

                swap = SplitString(scheduleData.ExtensionTime, ':');
                Time1 = Convert.ToInt32(swap[0]);
                Minute1 = Convert.ToInt32(swap[1]);

                Time -= Time1;

                if (Minute < Minute1)
                {
                    Time -= 1;
                    Minute += 60;
                }

                Minute -= Minute1;

                scheduleData.Time = Time + ":" + Minute;
            }

            //일반시급
            if (!scheduleData.Time.Equals(""))
            {
                swap = SplitString(scheduleData.Time, ':');
                Time = Convert.ToInt32(swap[0]);
                Minute = Convert.ToInt32(swap[1]);

                scheduleData.Wage = ((Wage * Time) + (Wage * Minute) / 60).ToString();
            }
            else
            {
                scheduleData.Wage = "0";
            }

            //휴계시급
            if (!scheduleData.RestTime.Equals(""))
            {
                swap = SplitString(scheduleData.RestTime, ':');
                Time = Convert.ToInt32(swap[0]);
                Minute = Convert.ToInt32(swap[1]);

                scheduleData.RestWage = ((Wage * Time) + (Wage * Minute) / 60).ToString();
            }
            else
            {
                scheduleData.RestWage = "0";
            }

            //연장시급
            if (!scheduleData.ExtensionTime.Equals(""))
            {
                swap = SplitString(scheduleData.ExtensionTime, ':');
                Time = Convert.ToInt32(swap[0]);
                Minute = Convert.ToInt32(swap[1]);

                scheduleData.ExtensionWage = string.Format("{0:F0}", ((Wage * Time) + (Wage * Minute) / 60) * 1.5);
            }
            else
            {
                scheduleData.ExtensionWage = "0";
            }
            //야간시급
            if (!scheduleData.NightTime.Equals(""))
            {
                swap = SplitString(scheduleData.NightTime, ':');
                Time = Convert.ToInt32(swap[0]);
                Minute = Convert.ToInt32(swap[1]);

                scheduleData.NightWage = string.Format("{0:F0}", ((Wage * Time) + (Wage * Minute) / 60) * 1.5);
            }
            else
            {
                scheduleData.NightWage = "0";
            }

            //총 시급
            scheduleData.TotalWage = (Convert.ToInt32(scheduleData.Wage) + Convert.ToInt32(scheduleData.ExtensionWage)
                + Convert.ToInt32(scheduleData.NightWage) - Convert.ToInt32(scheduleData.RestWage)).ToString();


            Update(phone, scheduleData);
        }

        private string[] SplitString(string str, char separator)
        {
            string[] result = new string[2];

            if (str.Contains(separator))
            {
                result = str.Split(separator);
            }
            else
            {
                if (str.Length == 4)
                {
                    result[0] = str.Substring(0, 2);
                    result[1] = str.Substring(2);
                }
            }

            return result;
        }

        /// <summary>
        /// 데이터 그리드, 데이터 테이블 맵핑
        /// </summary>
        /// <param name="yaer">선택년도</param>
        /// <param name="month">선택월</param>
        /// <returns></returns>
        public DataTable MappingDataTable(string yaer, string month)
        {
            SQLiteCommand command;
            SQLiteDataAdapter adapter;
            DataTable dataTable = new DataTable();
            DataSet dataSet = new DataSet();
            string sql = "where Date LIKE \"" + yaer + "-" + month.PadLeft(2,'0') + "-" + "%\" Order by Date";
            string phone ="";

            //사용자의 권한이 일반직원인지 확인
            if(MemberData.GetMemberData.AuthorityData.Authority==3)
            {
                //자기자신의 핸드폰번호
                phone = MemberData.GetMemberData.Phone;
            }
            else
            {
                //선택된 콤보박스의 인덱스를 가져와서 콤보박스에 연결된 List에서 핸드폰번호를 가져옴
                int index = TransitionPage.pgMain.cbName.SelectedIndex;
                phone = TransitionPage.pgMain.loginDataList[index].Phone;
                
            }

            dBConn.DBOpen();

            //DB에 핸드폰 번호와 sql 전달, 핸드폰 번호는 DB의 테이블명
            command = dBConn.Select(phone+yaer, sql);
            adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dataSet);
            dataTable = dataSet.Tables[0];

            int TotalWage = 0;

            if (dataTable.Rows.Count != 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    if (!row["TotalWage"].ToString().Equals(""))
                    {
                        TotalWage += Convert.ToInt32(row["TotalWage"]);
                    }
                }
            }

            try
            {
                DataRow datarow = dataTable.NewRow();
                datarow["NightWage"] = "합계";
                datarow["TotalWage"] = TotalWage;

                dataTable.Rows.Add(datarow);
            }
            catch(Exception e)
            {
                Console.WriteLine("MappingDataTable : " + e.Message);
            }

            dBConn.DBClose();

            return dataTable;
        }

        /// <summary>
        /// 데이터 테이블의 행 추가
        /// </summary>
        public void AddDataRow(DataTable dataTable, string date)
        {
            DataRow swapdataRow = dataTable.NewRow();
            DataRow dataRow = dataTable.NewRow();

            //마지막 행의 데이터를 저장후 마지막행 제거
            swapdataRow.ItemArray = dataTable.Rows[dataTable.Rows.Count - 1].ItemArray;

            DeleteDataTableRow(dataTable, dataTable.Rows.Count - 1);

            dataRow["Date"] = date;

            dataTable.Rows.Add(dataRow);
            dataTable.Rows.Add(swapdataRow);

        }

        /// <summary>
        /// 데이터 테이블의 행 제거
        /// </summary>
        public void DeleteDataTableRow(DataTable dataTable, int index)
        {
            dataTable.Rows[index].Delete();
        }

        /// <summary>
        /// 데이터 테이블에 있는 날을 반환해줌
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>날을 가진 리스트</returns>
        public List<int> GetDataTableDayList(DataTable dataTable)
        {
            List<int> daysList = new List<int>();

            if(dataTable != null)
            {
                foreach(DataRow data in dataTable.Rows)
                {
                    if (!data["Date"].ToString().Equals(""))
                    {
                        string[] swap = SplitString(data["Date"].ToString(), '-');
                        daysList.Add(Convert.ToInt32(swap[2]));
                    }
                }
            }

            return daysList;
        }

        /// <summary>
        /// 데이터 테이블의 변경사항만 DB에 저장
        /// </summary>
        /// <param name="dataTable"></param>
        public void SaveDataTable(DataTable dataTable)
        {
            ModifyDataTable(dataTable);
            AddDataTable(dataTable);
            DeleteDataTable(dataTable);

        }

        /// <summary>
        /// 변경된 데이터 추출
        /// </summary>
        /// <param name="dataTable">변경하는 데이터 테이블</param>
        public void ModifyDataTable(DataTable dataTable)
        {
            //전달받은 데이터테이블중 변경된 행을 새로운 데이터 테이블에 입력
            DataTable dtChanges = dataTable.GetChanges(DataRowState.Modified);

            //테이블의 값이 있는지 확인
            if (dtChanges != null)
            {
                foreach (DataRow data in dtChanges.Rows)
                {
                    //변경된 데이터테이블의 값이 조건에 맞는지 확인
                    if ((!data["OnTime"].ToString().Contains(":") && data["OnTime"].ToString().Length != 4) || (!data["OffTime"].ToString().Contains(":") && data["OffTime"].ToString().Length != 4)
                        || (data["OnTime"].ToString().Contains(":") && data["OnTime"].ToString().Length > 5) || (data["OffTime"].ToString().Contains(":") && data["OffTime"].ToString().Length > 5))
                    {
                        return;
                    }
                    else
                    {
                        string[] swap = SplitString(data["OnTime"].ToString(), ':');
                        int Time, Minute;
                        Time = Convert.ToInt32(swap[0]);
                        Minute = Convert.ToInt32(swap[1]);
                        data["OnTime"] = Time.ToString().PadLeft(2, '0') + ":" + Minute.ToString().PadLeft(2, '0');

                        if (Time < 0 || Time > 29 || Minute > 60 || Minute < 0)
                        {
                            return;
                        }

                        swap = SplitString(data["OffTime"].ToString(), ':');
                        Time = Convert.ToInt32(swap[0]);
                        Minute = Convert.ToInt32(swap[1]);
                        data["OffTime"] = Time.ToString().PadLeft(2, '0') + ":" + Minute.ToString().PadLeft(2, '0');

                        if (Time < 0 || Time > 29 || Minute > 60 || Minute < 0)
                        {
                            return;
                        }
                    }

                    string phone = "";

                    //권한이 일반직원인지 확인
                    if (MemberData.GetMemberData.AuthorityData.Authority == 3)
                    {
                        phone = MemberData.GetMemberData.Phone;
                    }
                    else
                    {
                        int index = TransitionPage.pgMain.cbName.SelectedIndex;
                        phone = TransitionPage.pgMain.loginDataList[index].Phone;
                    }


                    ScheduleData scheduleData = new ScheduleData();

                    scheduleData.OnTime = data["OnTime"].ToString();
                    scheduleData.OffTime = data["OffTime"].ToString();
                    scheduleData.Date = data["Date"].ToString();

                    Update(phone, scheduleData);
                    WageCalculation(phone, data["Date"].ToString());
                }
            }
        }

        /// <summary>
        /// 추가된 데이터 추출
        /// </summary>
        /// <param name="dataTable">변경된 데이터테이블</param>
        public void AddDataTable(DataTable dataTable)
        {
            //전달받은 데이터테이블중 추가된 행을 새로운 데이터 테이블에 입력
            DataTable dtChanges = dataTable.GetChanges(DataRowState.Added);

            //테이블의 값이 있는지 확인
            if (dtChanges != null)
            {
                foreach (DataRow data in dtChanges.Rows)
                {
                    //변경된 데이터테이블의 값이 조건에 맞는지 확인
                    if ((!data["OnTime"].ToString().Contains(":") && data["OnTime"].ToString().Length != 4) || (!data["OffTime"].ToString().Contains(":") && data["OffTime"].ToString().Length != 4)
                        || (data["OnTime"].ToString().Contains(":") && data["OnTime"].ToString().Length > 5) || (data["OffTime"].ToString().Contains(":") && data["OffTime"].ToString().Length > 5))
                    {
                        return;
                    }
                    else
                    {
                        string[] swap = SplitString(data["OnTime"].ToString(), ':');
                        int Time, Minute;
                        Time = Convert.ToInt32(swap[0]);
                        Minute = Convert.ToInt32(swap[1]);
                        data["OnTime"] = Time.ToString().PadLeft(2, '0') + ":" + Minute.ToString().PadLeft(2, '0');

                        if (Time < 0 || Time > 29 || Minute > 60 || Minute < 0)
                        {
                            return;
                        }

                        swap = SplitString(data["OffTime"].ToString(), ':');
                        Time = Convert.ToInt32(swap[0]);
                        Minute = Convert.ToInt32(swap[1]);
                        data["OffTime"] = Time.ToString().PadLeft(2, '0') + ":" + Minute.ToString().PadLeft(2, '0');

                        if (Time < 0 || Time > 29 || Minute > 60 || Minute < 0)
                        {
                            return;
                        }
                    }

                    string phone = "";

                    //권한이 일반직원인지 확인
                    if (MemberData.GetMemberData.AuthorityData.Authority == 3)
                    {
                        phone = MemberData.GetMemberData.Phone;
                    }
                    else
                    {
                        int index = TransitionPage.pgMain.cbName.SelectedIndex;
                        phone = TransitionPage.pgMain.loginDataList[index].Phone;
                    }


                    ScheduleData scheduleData = new ScheduleData();

                    scheduleData.OnTime = data["OnTime"].ToString();
                    scheduleData.OffTime = data["OffTime"].ToString();
                    scheduleData.Date = data["Date"].ToString();

                    Insert(phone, scheduleData);
                    WageCalculation(phone, data["Date"].ToString());
                }
            }
        }

        /// <summary>
        /// 제거된 데이터 추출
        /// </summary>
        /// <param name="dataTable">변경된 데이터테이블</param>
        public void DeleteDataTable(DataTable dataTable)
        {
            //전달받은 데이터테이블중 삭제된 행을 새로운 데이터 테이블에 입력
            DataTable dtChanges = dataTable.GetChanges(DataRowState.Deleted);
            string OnTime, OffTime;

            //테이블의 값이 있는지 확인
            if (dtChanges != null)
            {
                foreach (DataRow data in dtChanges.Rows)
                {
                    //변경된 데이터테이블의 값이 조건에 맞는지 확인
                    if ((!data["OnTime", DataRowVersion.Original].ToString().Contains(":") && data["OnTime", DataRowVersion.Original].ToString().Length != 4) || (!data["OffTime", DataRowVersion.Original].ToString().Contains(":") && data["OffTime", DataRowVersion.Original].ToString().Length != 4)
                        || (data["OnTime", DataRowVersion.Original].ToString().Contains(":") && data["OnTime", DataRowVersion.Original].ToString().Length > 5) || (data["OffTime", DataRowVersion.Original].ToString().Contains(":") && data["OffTime", DataRowVersion.Original].ToString().Length > 5))
                    {
                        return;
                    }
                    else
                    {
                        string[] swap = SplitString(data["OnTime", DataRowVersion.Original].ToString(), ':');
                        int Time, Minute;
                        Time = Convert.ToInt32(swap[0]);
                        Minute = Convert.ToInt32(swap[1]);
                        OnTime = Time.ToString().PadLeft(2, '0') + ":" + Minute.ToString().PadLeft(2, '0');

                        if (Time < 0 || Time > 29 || Minute > 60 || Minute < 0)
                        {
                            return;
                        }

                        swap = SplitString(data["OffTime", DataRowVersion.Original].ToString(), ':');
                        Time = Convert.ToInt32(swap[0]);
                        Minute = Convert.ToInt32(swap[1]);
                        OffTime = Time.ToString().PadLeft(2, '0') + ":" + Minute.ToString().PadLeft(2, '0');

                        if (Time < 0 || Time > 29 || Minute > 60 || Minute < 0)
                        {
                            return;
                        }
                    }

                    string phone = "";

                    //권한이 일반직원인지 확인
                    if (MemberData.GetMemberData.AuthorityData.Authority == 3)
                    {
                        phone = MemberData.GetMemberData.Phone;
                    }
                    else
                    {
                        int index = TransitionPage.pgMain.cbName.SelectedIndex;
                        phone = TransitionPage.pgMain.loginDataList[index].Phone;
                    }


                    ScheduleData scheduleData = new ScheduleData();

                    scheduleData.OnTime = OnTime;
                    scheduleData.OffTime = OffTime;
                    scheduleData.Date = data["Date", DataRowVersion.Original].ToString();

                    Delete(phone, scheduleData);
                    WageCalculation(phone, data["Date", DataRowVersion.Original].ToString());
                }
            }
        }

        /// <summary>
        /// 엑셀로 내보내기
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="dataTable"></param>
        public void ExportToExcel(DataGrid dataGrid, DataTable dataTable)
        {
            Excel.Application excel = new Excel.Application();
            Excel.Workbook workbook = excel.Workbooks.Add();
            Excel.Worksheet worksheet = excel.ActiveSheet;
            string saveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) +"\\ExcelFiles";

            if(!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            if (dataGrid.Items.Count == 0)
            {
                MessageBox.Show("데이터가 없습니다.");
                return;
            }

            int x = dataGrid.Items.Count;

            try
            {
                int cellRowIndex = 1;
                int cellColumnIndex = 1;

                for (int col = 0; col < dataGrid.Columns.Count; col++)
                {
                    worksheet.Cells[cellRowIndex, cellColumnIndex] = dataGrid.Columns[col].Header;
                    cellColumnIndex++;
                }

                cellColumnIndex = 1;
                cellRowIndex++;

                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        worksheet.Cells[cellRowIndex, cellColumnIndex] = dataTable.Rows[row][col];
                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.CheckPathExists = true;
                saveFile.AddExtension = true;
                saveFile.ValidateNames = true;
                saveFile.InitialDirectory = saveDirectory;

                saveFile.DefaultExt = ".xlsx";
                saveFile.Filter = "Microsoft Excel Workbook (*.xls)|*.xlsx";

                string Name = "";

                if(MemberData.GetMemberData.AuthorityData.Authority==3)
                {
                    Name = MemberData.GetMemberData.Name;
                }
                else
                {
                    Name = TransitionPage.pgMain.cbName.Text;
                }
                saveFile.FileName = Name;
                workbook.SaveAs(saveDirectory + "\\" + saveFile.FileName);

                workbook.Close();
                excel.Quit();

                releaseObject(worksheet);
                releaseObject(workbook);
                releaseObject(excel);

                Process.Start(saveDirectory + "\\" + saveFile.FileName + ".xlsx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 오브젝트 해제
        /// </summary>
        /// <param name="obj"></param>
        private void releaseObject(object obj)
        {
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(obj);
        }
    }
}

