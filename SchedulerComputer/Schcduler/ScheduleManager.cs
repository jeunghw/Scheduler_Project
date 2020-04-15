using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading;

namespace Schcduler
{
    class ScheduleManager
    {
        SQLiteManager sqliteManager = MainWindow.GetSqliteManager();
        DataTable dtSchedule;
        List<DateWeek> dateWeekList;

        /// <summary>
        /// 스케줄 데이터를 검색
        /// </summary>
        /// <param name="tableName">테이블명</param>
        /// <param name="startDate">시작날짜</param>
        /// <param name="endDate">마지막날짜</param>
        /// <param name="phone">핸드폰번호</param>
        /// <returns>데이터가 들어간 딕셔너리</returns>
        public Dictionary<string, List<ScheduleData>> Select(string tableName, string startDate, string endDate, string phone)
        {
            SQLiteCommand command;
            SQLiteDataReader reader;
            Dictionary<string, List<ScheduleData>> scheduleDatas = new Dictionary<string, List<ScheduleData>>();
            string sql = "where Date Between \"" + startDate + "\" and \"" + endDate + "\"";

            if (!phone.Equals(""))
            {
                sql = sql + " and Phone=\"" + phone + "\"";
            }

            sql = sql +" ORDER by Phone, Date";

            sqliteManager.DBOpen();

            command = sqliteManager.Select(tableName, sql);

            reader = command.ExecuteReader();

            string phoneInedx = "";
            List<ScheduleData> scheduleList = new List<ScheduleData>();
            ScheduleData scheduleData1;
            while (reader.Read())
            {
                scheduleData1 = new ScheduleData();

                scheduleData1.Phone = reader["Phone"].ToString();

                if(phoneInedx.Equals(""))
                {
                    phoneInedx = scheduleData1.Phone;
                } else if(!phoneInedx.Equals(scheduleData1.Phone))
                {
                    scheduleDatas.Add(phoneInedx, scheduleList);
                    phoneInedx = scheduleData1.Phone;
                    scheduleList = new List<ScheduleData>();
                }

                scheduleData1.Date = reader["Date"].ToString();
                scheduleData1.OnTime = reader["OnTime"].ToString();
                scheduleData1.OffTime = reader["OffTime"].ToString();

                scheduleList.Add(scheduleData1);

            }

            scheduleDatas.Add(phoneInedx, scheduleList);
            sqliteManager.DBClose();

            return scheduleDatas;
        }

        /// <summary>
        /// 데이터 베이스에 데이터 입력
        /// </summary>
        /// <param name="tableName">테이블명</param>
        /// <param name="scheduleData">스케줄데이터</param>
        /// <returns>영향받은 행수</returns>
        public int Insert(string tableName, ScheduleData scheduleData)
        {
            int result = -1;
            string sql = "values(\"" + scheduleData.Date + "\",\"" + scheduleData.Phone + "\",\"" + scheduleData.OnTime + "\",\"" + scheduleData.OffTime + "\")";

            sqliteManager.DBOpen();


            result = sqliteManager.Insert(tableName, sql);

            Thread thread = new Thread(() => MainWindow.runThread(4, MySQLData.TableSchedule, sql));
            thread.Start();

            sqliteManager.DBClose();
            

            return result;
        }

        /// <summary>
        /// 데이터 베이스에 데이터 변경
        /// </summary>
        /// <param name="tableName">테이블명</param>
        /// <param name="scheduleData">스케줄데이터</param>
        /// <returns>영향받은 행수</returns>
        public int Update(string tableName, ScheduleData scheduleData)
        {
            int result = -1;
            string sql = "OnTime = \"" + scheduleData.OnTime + "\", OffTime =\"" + scheduleData.OffTime +"\" where Phone = \"" + scheduleData.Phone + "\" and Date = \"" + scheduleData.Date + "\"";

            sqliteManager.DBOpen();


            result = sqliteManager.Update(tableName, sql);


            sqliteManager.DBClose();

            Thread thread = new Thread(() => MainWindow.runThread(2, MySQLData.TableSchedule, sql));
            thread.Start();

            return result;
        }

        /// <summary>
        /// 해당날짜의 스케줄이 작성되어 있는지 확인
        /// </summary>
        /// <param name="tableName">테이블이름</param>
        /// <param name="scheduleData">테이블 내부 데이터</param>
        /// <returns>
        /// true : 값이 있음
        /// false : 값이 없음
        /// </returns>
        public bool CheckData(string tableName, ScheduleData scheduleData)
        {
            SQLiteCommand command;
            SQLiteDataReader reader;
            bool result = false;
            string sql = " where Phone=\"" + scheduleData.Phone + "\" and Date=\"" + scheduleData.Date + "\"";

            sqlliteManager.DBOpen();
            command = sqlliteManager.Select(tableName, sql);

            if (command != null)
            {
                try
                {
                    reader = command.ExecuteReader();
                    reader.Read();
                    if (reader["Phone"].ToString().Equals(""))
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
                    Console.WriteLine("ScheduleManager_CheckData: " + ex.Message);
                }
            }
            sqlliteManager.DBClose();

            return result;
        }

        SQLiteManager sqlliteManager = new SQLiteManager();
        string[] dayOfWeek = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };


        /// <summary>
        /// 데이터테이블 생성
        /// </summary>
        /// <param name="dateWeekList"></param>
        /// <returns></returns>
        public DataTable inItDataTable(List<DateWeek> dateWeekList, string phone, int task)
        { 
            dtSchedule = new DataTable("dtSchedule");
            DataColumn[] key = new DataColumn[1];
            this.dateWeekList = dateWeekList;

            String[] dcNames = { "Name", "Phone", "OnTime", "OffTime", "Date"};
            
            ///데이터 테이블 생성부분
            //데이터테이블 컬럼 생성 및 추가
            for(int i=0; i< dcNames.Length; i++)
            {
                //컬럼명이 Name, Phone인지 확인
                if (!dcNames[i].Equals("Name") && !dcNames[i].Equals("Phone"))
                {
                    for (int j = 0; j < dateWeekList.Count; j++)
                    {
                        DataColumn col = new DataColumn();
                        col.DataType = Type.GetType("System.String");
                        col.ColumnName = dateWeekList[j].date + dcNames[i];
                        dtSchedule.Columns.Add(col);
                    }
                }
                else
                {
                    DataColumn col = new DataColumn();
                    col.DataType = Type.GetType("System.String");
                    col.ColumnName = dcNames[i];
                    dtSchedule.Columns.Add(col);
                }
            }

            setDataTable(dateWeekList[0].date, dateWeekList[dateWeekList.Count - 1].date, phone, task);

            return dtSchedule;
        }

        /// <summary>
        /// 데이터 테이블 데이터 저장
        /// </summary>
        /// <param name="dataTable"></param>
        public void SaveDataTable(DataTable dataTable)
        {
            AddDataTable(dataTable);
            ModifyDataTable(dataTable);
        }

        /// <summary>
        /// 추가된 데이터 추출
        /// </summary>
        /// <param name="dataTable">변경하는 데이터 테이블</param>
        private void AddDataTable(DataTable dataTable)
        {
            //전달받은 데이터테이블중 변경된 행을 새로운 데이터 테이블에 입력
            DataTable dtChanges = dataTable.GetChanges(DataRowState.Added);

            //테이블의 값이 있는지 확인
            if (dtChanges != null)
            {
                foreach (DataRow data in dtChanges.Rows)
                {
                    for (int i = 0; i < dateWeekList.Count; i++)
                    {
                        //변경된 데이터테이블의 값이 조건에 맞는지 확인
                        if ((!data[dateWeekList[i].date+"OnTime"].ToString().Contains(":") && data[dateWeekList[i].date + "OnTime"].ToString().Length != 4) || 
                            (!data[dateWeekList[i].date + "OffTime"].ToString().Contains(":") && data[dateWeekList[i].date + "OffTime"].ToString().Length != 4)
                            || (data[dateWeekList[i].date + "OnTime"].ToString().Contains(":") && data[dateWeekList[i].date + "OnTime"].ToString().Length > 5) || 
                            (data[dateWeekList[i].date + "OffTime"].ToString().Contains(":") && data[dateWeekList[i].date + "OffTime"].ToString().Length > 5))
                        {
                            continue;
                        }
                        else
                        {
                            string[] swap = SplitString(data[dateWeekList[i].date + "OnTime"].ToString(), ':');
                            int Time, Minute;
                            Time = Convert.ToInt32(swap[0]);
                            Minute = Convert.ToInt32(swap[1]);
                            data[dateWeekList[i].date + "OnTime"] = Time.ToString().PadLeft(2, '0') + ":" + Minute.ToString().PadLeft(2, '0');

                            if (Time < 0 || Time > 29 || Minute > 60 || Minute < 0)
                            {
                                continue;
                            }

                            swap = SplitString(data[dateWeekList[i].date + "OffTime"].ToString(), ':');
                            Time = Convert.ToInt32(swap[0]);
                            Minute = Convert.ToInt32(swap[1]);
                            data[dateWeekList[i].date + "OffTime"] = Time.ToString().PadLeft(2, '0') + ":" + Minute.ToString().PadLeft(2, '0');

                            if (Time < 0 || Time > 29 || Minute > 60 || Minute < 0)
                            {
                                continue;
                            }
                        }

                        ScheduleData scheduleData = new ScheduleData();

                        scheduleData.Phone = data["Phone"].ToString();
                        scheduleData.Date = data[dateWeekList[i].date + "Date"].ToString();
                        scheduleData.OnTime = data[dateWeekList[i].date + "OnTime"].ToString();
                        scheduleData.OffTime = data[dateWeekList[i].date + "OffTime"].ToString();

                        if(CheckData(SQLiteData.TableSchedule, scheduleData))
                        {
                            Update(SQLiteData.TableSchedule, scheduleData);
                        }
                        else
                        {
                            Insert(SQLiteData.TableSchedule, scheduleData);
                        }
                        
                    }
                }
            }
        }

        /// <summary>
        /// 변경된 데이터 추출
        /// </summary>
        /// <param name="dataTable">변경하는 데이터 테이블</param>
        private void ModifyDataTable(DataTable dataTable)
        {
            //전달받은 데이터테이블중 변경된 행을 새로운 데이터 테이블에 입력
            DataTable dtChanges = dataTable.GetChanges(DataRowState.Modified);

            //테이블의 값이 있는지 확인
            if (dtChanges != null)
            {
                foreach (DataRow data in dtChanges.Rows)
                {
                    //변경된 데이터테이블의 값이 조건에 맞는지 확인
                    for (int i = 0; i < dateWeekList.Count; i++)
                    {
                        //변경된 데이터테이블의 값이 조건에 맞는지 확인
                        if ((!data[dateWeekList[i].date + "OnTime"].ToString().Contains(":") && data[dateWeekList[i].date + "OnTime"].ToString().Length != 4) ||
                            (!data[dateWeekList[i].date + "OffTime"].ToString().Contains(":") && data[dateWeekList[i].date + "OffTime"].ToString().Length != 4)
                            || (data[dateWeekList[i].date + "OnTime"].ToString().Contains(":") && data[dateWeekList[i].date + "OnTime"].ToString().Length > 5) ||
                            (data[dateWeekList[i].date + "OffTime"].ToString().Contains(":") && data[dateWeekList[i].date + "OffTime"].ToString().Length > 5))
                        {
                            continue;
                        }
                        else
                        {
                            string[] swap = SplitString(data[dateWeekList[i].date + "OnTime"].ToString(), ':');
                            int Time, Minute;
                            Time = Convert.ToInt32(swap[0]);
                            Minute = Convert.ToInt32(swap[1]);
                            data[dateWeekList[i].date + "OnTime"] = Time.ToString().PadLeft(2, '0') + ":" + Minute.ToString().PadLeft(2, '0');

                            if (Time < 0 || Time > 29 || Minute > 60 || Minute < 0)
                            {
                                continue;
                            }

                            swap = SplitString(data[dateWeekList[i].date + "OffTime"].ToString(), ':');
                            Time = Convert.ToInt32(swap[0]);
                            Minute = Convert.ToInt32(swap[1]);
                            data[dateWeekList[i].date + "OffTime"] = Time.ToString().PadLeft(2, '0') + ":" + Minute.ToString().PadLeft(2, '0');

                            if (Time < 0 || Time > 29 || Minute > 60 || Minute < 0)
                            {
                                continue;
                            }
                        }

                        ScheduleData scheduleData = new ScheduleData();

                        scheduleData.Phone = data["Phone"].ToString();
                        scheduleData.Date = data[dateWeekList[i].date + "Date"].ToString();
                        scheduleData.OnTime = data[dateWeekList[i].date + "OnTime"].ToString();
                        scheduleData.OffTime = data[dateWeekList[i].date + "OffTime"].ToString();
                        
                        Update(SQLiteData.TableSchedule, scheduleData);
                    }
                }
            }
        }

        /// <summary>
        /// 데이터 테이블의 테이터 입력
        /// </summary>
        /// <param name="startDate">시작날짜</param>
        /// <param name="endDate">마지막날짜</param>
        /// <param name="phone"></param>
        /// <param name="task"></param>
        public void setDataTable(string startDate, string endDate, string phone, int task)
        {
            Dictionary<string, List<ScheduleData>> scheduleDatas = Select(SQLiteData.TableSchedule, startDate, endDate, phone);
            List<LoginData> loginDataList = CommData.GetCommData().getLoginDataList();

            //데이터 그리드와 데이터 테이블 바인딩
            for (int i = 0; i < loginDataList.Count; i++)
            {
                if((!phone.Equals(""))&&!loginDataList[i].Phone.Equals(phone))
                {
                    continue;
                }
                if(task == 1)
                {
                    if (loginDataList[i].Task != 0 && loginDataList[i].Task != 1)
                        continue;
                }
                else if(task== 2)
                {
                    if (loginDataList[i].Task != 0 && loginDataList[i].Task != 2)
                        continue;
                }
                DataRow dataRow = dtSchedule.NewRow();
                dataRow["Name"] = loginDataList[i].Name;
                dataRow["Phone"] = loginDataList[i].Phone;
                for (int j = 0; j < dateWeekList.Count; j++)
                {
                    dataRow[dateWeekList[j].date + "Date"] = dateWeekList[j].date;
                    dataRow[dateWeekList[j].date + "OnTime"] = "";
                    dataRow[dateWeekList[j].date + "OffTime"] = "";
                }

                dtSchedule.Rows.Add(dataRow);
            }

            ///데이터 입력
            foreach (DataRow data in dtSchedule.Rows)
            {
                if (scheduleDatas.ContainsKey(data["Phone"].ToString()))
                {
                    List<ScheduleData> scheduleLIst = new List<ScheduleData>();
                    scheduleDatas.TryGetValue(data["Phone"].ToString(), out scheduleLIst);
                    int j = 0;
                    for (int i = 0; (i < dateWeekList.Count && j<scheduleLIst.Count); i++)
                    {
                        if (data[dateWeekList[i].date + "Date"].Equals(scheduleLIst[j].Date))
                        {
                            data[dateWeekList[i].date + "OnTime"] = scheduleLIst[j].OnTime;
                            data[dateWeekList[i].date + "OffTime"] = scheduleLIst[j].OffTime;
                            j++;
                        }
                    }
                }
            }
        }

         /// <summary>
        /// 년, 월, 일을 받아서 해당 주의 날짜와 요일을 List로 반환
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public List<DateWeek> getWeekOfFirstDay(int year,int month,int day)
        {
            string[] dayOfWeek = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            string[] dayOfWeekKr = { "월요일", "화요일", "수요일", "목요일", "금요일", "토요일", "일요일" };
            
            List<DateWeek> dateWeekList = new List<DateWeek>();
            int dayNum = 0;
            
            DateTime dateTime = new DateTime(year, month, day);

            //전달받은 날짜의 요일을 확인
            for (int i = 0; i < dayOfWeek.Length; i++)
            {
                //요일에 따라서 해당 날짜에서 뺄값을 지정
                if (dateTime.DayOfWeek.ToString().Substring(0, 3) == dayOfWeek[i])
                {
                    dayNum = i;
                }
            }
            //주 첫날짜 구하기
            day = day - dayNum;

            //날짜가 0보다 작으면 전 월의 마지막 날짜를 구해서 계산
            if (day < 1)
            {
                month -= 1;
                day = DateTime.DaysInMonth(year, month) + day;
            }

            //구해진 날짜와 요일을 리스트에 입력
            for (int i = 0; i < 7; i++)
            {
                DateWeek dateWeek = new DateWeek(year+"-" + month.ToString().PadLeft(2, '0') + "-" + (day + i).ToString().PadLeft(2,'0'), dayOfWeekKr[i]);
                dateWeekList.Add(dateWeek);
            }

            return dateWeekList;
        }

        /// <summary>
        /// 시작 날짜와 마지막 날짜를 받아서 요일/날짜  리스트를 반환
        /// </summary>
        /// <param name="startDate">시작날짜</param>
        /// <param name="endDate">마지막날짜</param>
        /// <returns>요일/날짜 리스트</returns>
        public List<DateWeek> getWeekDateList(string startDate, string endDate)
        {
            List<DateWeek> dateWeekList = new List<DateWeek>();
            string[] dayOfWeek = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            string[] dayOfWeekKr = { "월요일", "화요일", "수요일", "목요일", "금요일", "토요일", "일요일" };

            string[] startSwap = startDate.Split('-');
            int startYear = Convert.ToInt32(startSwap[0]), startMonth = Convert.ToInt32(startSwap[1]), startDay = Convert.ToInt32(startSwap[2]);
            string[] endSwap = endDate.Split('-');
            int endYear = Convert.ToInt32(endSwap[0]), endMonth = Convert.ToInt32(endSwap[1]), endDay = Convert.ToInt32(endSwap[2]);

            if (startDay > endDay)
                return null;

            for(int i=startDay; i<=endDay; i++)
            {
                DateWeek dateWeek = new DateWeek();
                DateTime dateTime = new DateTime(startYear, startMonth, i);
                for (int j = 0; j < dayOfWeek.Length; j++)
                {
                    if (dateTime.DayOfWeek.ToString().Substring(0, 3) == dayOfWeek[j])
                    {
                        dateWeek.week = dayOfWeekKr[j];
                        break;
                    }
                }
                    dateWeek.date = startYear + "-" + startMonth.ToString().PadLeft(2, '0') + "-" + i.ToString().PadLeft(2, '0');

                    dateWeekList.Add(dateWeek);
            }


            return dateWeekList;
        }

        /// <summary>
        /// 문자분리
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
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
    }
}
