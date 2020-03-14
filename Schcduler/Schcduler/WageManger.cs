using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;

namespace Schcduler
{
    class WageManger
    {
        DBConn dBConn = MainWindow.GetDBConn();
        String sql;
        int result;
        SQLiteDataReader rdr;
        public List<ScheduleData> scheduleDatasLists = new List<ScheduleData>();
        public DataTable MappingData(string sql)
        {
            //데이터테이블과 데이터 리스트 연결
            /*DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Date", typeof(string));
            dataTable.Columns.Add("OnTime", typeof(string));
            dataTable.Columns.Add("OffTime", typeof(string));
            dataTable.Columns.Add("Time", typeof(string));
            dataTable.Columns.Add("RestTime", typeof(string));
            dataTable.Columns.Add("ExtensionTime", typeof(string));
            dataTable.Columns.Add("NightTime", typeof(string));
            dataTable.Columns.Add("TotalTime", typeof(string));
            dataTable.Columns.Add("Wage", typeof(string));
            dataTable.Columns.Add("RestWage", typeof(string));
            dataTable.Columns.Add("ExtensionWage", typeof(string));
            dataTable.Columns.Add("NightWage", typeof(string));
            dataTable.Columns.Add("TotalWage", typeof(string));

            foreach (ScheduleData data in list)
            {
                dataTable.Rows.Add(data);
            }*/

            DataTable dataTable = new DataTable();
            DataSet ds = new DataSet();
            dBConn.DBOpen();
            dBConn.DBSelect2(sql).Fill(ds);
            dataTable = ds.Tables[0];

            int TotalWage = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                TotalWage += Convert.ToInt32(row["TotalWage"]);
            }


            DataRow datarow = dataTable.NewRow();
            datarow["NightWage"] = "합계";
            datarow["TotalWage"] = TotalWage;

            dataTable.Rows.Add(datarow);

            return dataTable;
        }

        /// <summary>
        /// insert쿼리를 전송
        /// </summary>
        /// <param name="number">
        /// int형식으로 전송테이블을 선택
        /// 1 : Membertable
        /// 2 : Schedultable
        /// </param>
        /// <returns></returns>
        public int insertTime()
        {
            sql = "select * from " + DBInfo.TableSchedule + " where phone = \"" + LoginData.GetLoginData.LoginPhone + "\" and date = \"" + DateTime.Now.ToString("yyyy-MM-dd") + "\"";

            scheduleDatasLists = selectSchedule(sql);

            if (scheduleDatasLists.Count != 0)
            {
                return 0;
            }
            else
            {
                sql = "insert into " + DBInfo.TableSchedule + " (Phone, Date, OnTime)  values(\"" + LoginData.GetLoginData.LoginPhone
                    + "\", date(\'now\',\'localtime\'), \"" + DateTime.Now.ToString("HH:mm") + "\")";
            }
            dBConn.DBOpen();
            result = dBConn.DBManipulation(sql);
            dBConn.DBClose();

            return result;
        }

        public int updateTime()
        {
            sql = "select * from " + DBInfo.TableSchedule + " where phone = \"" + LoginData.GetLoginData.LoginPhone + "\" and date = \"" + DateTime.Now.ToString("yyyy-MM-dd") + "\"";

            scheduleDatasLists = selectSchedule(sql);

            if (scheduleDatasLists.Count == 0)
            {
                return 0;
            }
            else
            {
                sql = " update " + DBInfo.TableSchedule + " set OffTime = \"" + DateTime.Now.ToString("HH:mm") + "\" where Phone = \""
                    + LoginData.GetLoginData.LoginPhone + "\" and Date = \"" + DateTime.Now.ToString("yyyy-MM-dd") + "\"";
            }

            dBConn.DBOpen();
            result = dBConn.DBManipulation(sql);
            dBConn.DBClose();

            return result;
        }

        /// <summary>
        /// 스케줄테이블에서 select해오는 메소드
        /// </summary>
        /// <returns></returns>
        public List<ScheduleData> selectSchedule(string sql)
        {
            dBConn.DBOpen();
            rdr = dBConn.DBSelect(sql);

            ScheduleData scheduleData;
            List<ScheduleData> scheduleDatasList = new List<ScheduleData>();

            while (rdr.Read())
            {
                scheduleData = new ScheduleData();

                scheduleData.Phone = rdr["phone"].ToString();
                scheduleData.Date = rdr["date"].ToString();
                scheduleData.OnTime = rdr["ontime"].ToString();
                scheduleData.OffTime = rdr["offtime"].ToString();

                if (!scheduleData.Phone.Equals(""))
                {
                    scheduleDatasList.Add(scheduleData);
                }
            }

            dBConn.DBClose();

            return scheduleDatasList;
        }

        public void ExportToExcel(DataGrid dg)
        {
            Excel.Application excel;
            Excel.Workbook workbook;

            /*if(dg.Items.Count==0)
            {
                MessageBox.Show("데이터가 없습니다.");
                return;
            }

            int x = dg.Items.Count;
            Console.WriteLine(dg.Items[0]);

            try
            {
                int cellRowIndex = 1;
                int cellColumnIndex = 1;

                for(int col=0;col<dg.Columns.Count;col++)
                {
                    worksheet.Cells[cellRowIndex, cellColumnIndex] = dg.Columns[col].Header;
                    cellColumnIndex++;
                }

                cellColumnIndex = 1;
                cellRowIndex++;

                for (int row = 0; row > dg.Items.Count - 1;row++)
                {
                    for(int col=0;col<dg.Columns.Count;col++)
                    {
                        //worksheet.Cells[cellRowIndex, cellColumnIndex] = 
                    }
                }

            }
            catch
            {

            }*/
        }
        public void saveDataTable(DataTable dt)
        {
            DataTable dtChanges = dt.GetChanges(DataRowState.Modified);

            if (dtChanges != null)
            {
                dBConn.DBOpen();
                foreach (DataRow dr in dtChanges.Rows)
                {

                    if ((!dr["OnTime"].ToString().Contains(":") && dr["OnTime"].ToString().Length != 4) || (!dr["OffTime"].ToString().Contains(":") && dr["OffTime"].ToString().Length != 4)
                        || (dr["OnTime"].ToString().Contains(":") && dr["OnTime"].ToString().Length > 5) || (dr["OffTime"].ToString().Contains(":") && dr["OffTime"].ToString().Length > 5))
                    {
                        return;
                    }
                    else
                    {
                        string[] swap = splitString(dr["OnTime"].ToString());
                        int Time, Minute;
                        Time = Convert.ToInt32(swap[0]);
                        Minute = Convert.ToInt32(swap[1]);

                        if (Time < 0 || Time > 27 || Minute > 60 || Minute < 0)
                        {
                            return;
                        }

                        swap = splitString(dr["OffTime"].ToString());
                        Time = Convert.ToInt32(swap[0]);
                        Minute = Convert.ToInt32(swap[1]);

                        if (Time < 0 || Time > 27 || Minute > 60 || Minute < 0)
                        {
                            return;
                        }
                    }

                    sql = "update " + DBInfo.TableSchedule + " set OnTime = \"" + dr["OnTime"] + "\", OffTime = \"" + dr["OffTime"] + "\" where Phone = \"" + dr["Phone"] + "\" and Date = \"" + dr["Date"] + "\"";
                    dBConn.DBManipulation(sql);
                    WageCalculationt(dr["Ontime"].ToString(), dr["OffTime"].ToString(), dr["Date"].ToString());

                }
                dBConn.DBClose();
            }

        }

        //List로 데이터 보여줄때 사용(사용 X)
        /*private List<ScheduleData> WageCalculationt(List<ScheduleData> list)
        {
            foreach (ScheduleData data in list)
            {
                //출근,퇴근 시간, 분 분리
                int OnTime, OnMinute, OffTime, OffMinute;

                string[] swap = data.OnTime.Split(":");

                OnTime = Convert.ToInt32(swap[0]);
                OnMinute = Convert.ToInt32(swap[1]);

                swap = data.OffTime.Split(":");

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
                    if(OffTime < 22)
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
                        data.Time = Time + ":" + Minute;
                    }
                    //야간시간
                    else
                    {
                        Time = 22 - OnTime;
                        Time -= 1;
                        Minute = 60 - OnMinute;

                        if(Minute >= 60)
                        {
                            Time += 1;
                            Minute -= 60;
                        }

                        data.Time = Time + ":" + Minute;

                        Time = OffTime - 22;
                        Minute = OffMinute;
                        data.NightTime = Time + ":" + Minute;
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
                        data.Time = Time + ":" + Minute;
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
                        data.NightTime = Time + ":" + Minute;
                    }
                }

                //총 시간
                if (!data.Time.Equals(""))
                {
                    swap = data.Time.Split(":");
                    Time = Convert.ToInt32(swap[0]);
                    Minute = Convert.ToInt32(swap[1]);
                }

                if (!data.NightTime.Equals(""))
                {
                    swap = data.NightTime.Split(":");
                    Time += Convert.ToInt32(swap[0]);
                    Minute += Convert.ToInt32(swap[1]);
                }

                if(Minute >= 60)
                {
                    Time += 1;
                    Minute -= 60;
                }
                data.TotalTime = Time + ":" + Minute;

                //휴계시간
                if((Time / 4)>0)
                {
                    Minute = (Time / 4) * 30;
                    Time = 0;
                    if(Minute >= 60)
                    {
                        Time += 1;
                        Minute -= 60;
                    }
                    data.RestTime = Time + ":" + Minute;
                }

                //연장시간
                swap = data.TotalTime.Split(":");
                Time = Convert.ToInt32(swap[0]);
                Minute = Convert.ToInt32(swap[1]);
                if(Time > 8)
                {
                    Time -=8;
                    data.ExtensionTime = Time + ":" + Minute;

                    //일반시간에서 연장시간 빼기
                    int Time1, Minute1;
                    swap = data.Time.Split(":");
                    Time = Convert.ToInt32(swap[0]);
                    Minute = Convert.ToInt32(swap[1]);

                    swap = data.ExtensionTime.Split(":");
                    Time1 = Convert.ToInt32(swap[0]);
                    Minute1 = Convert.ToInt32(swap[1]);

                    Time -= Time1;

                    if(Minute < Minute1)
                    {
                        Time -= 1;
                        Minute += 60;
                    }

                    Minute -= Minute1;

                    data.Time = Time + ":" + Minute;
                }

                //일반시급
                if (!data.Time.Equals(""))
                {
                    swap = data.Time.Split(":");
                    Time = Convert.ToInt32(swap[0]);
                    Minute = Convert.ToInt32(swap[1]);

                    data.Wage = ((Convert.ToInt32(LoginData.GetLoginData.Wage) * Time) + (Convert.ToInt32(LoginData.GetLoginData.Wage) * Minute) / 60).ToString();
                }
                else
                {
                    data.Wage = "0";
                }

                //휴계시급
                if (!data.RestTime.Equals(""))
                {
                    swap = data.RestTime.Split(":");
                    Time = Convert.ToInt32(swap[0]);
                    Minute = Convert.ToInt32(swap[1]);

                    data.RestWage = ((Convert.ToInt32(LoginData.GetLoginData.Wage) * Time) + (Convert.ToInt32(LoginData.GetLoginData.Wage) * Minute) / 60).ToString();
                }
                else
                {
                    data.RestWage = "0";
                }

                //연장시급
                if (!data.ExtensionTime.Equals(""))
                {
                    swap = data.ExtensionTime.Split(":");
                    Time = Convert.ToInt32(swap[0]);
                    Minute = Convert.ToInt32(swap[1]);

                    data.ExtensionWage = string.Format("{0:F0}", ((Convert.ToInt32(LoginData.GetLoginData.Wage) * Time) + (Convert.ToInt32(LoginData.GetLoginData.Wage) * Minute) / 60) * 1.5);
                }
                else
                {
                    data.ExtensionWage = "0";
                }
                //야간시급
                if (!data.NightTime.Equals(""))
                {
                    swap = data.NightTime.Split(":");
                    Time = Convert.ToInt32(swap[0]);
                    Minute = Convert.ToInt32(swap[1]);

                    data.NightWage = string.Format("{0:F0}", ((Convert.ToInt32(LoginData.GetLoginData.Wage) * Time) + (Convert.ToInt32(LoginData.GetLoginData.Wage) * Minute) / 60) * 1.5);
                }
                else
                {
                    data.NightWage = "0";
                }

                //총 시급
                data.TotalWage = (Convert.ToInt32(data.Wage) + Convert.ToInt32(data.ExtensionWage) + Convert.ToInt32(data.NightWage) - Convert.ToInt32(data.RestWage)).ToString();
            }

            return list;
        }*/

        public void WageCalculationt(string InputOnTime, string InputOffTime, string InputDate)
        {
            sql = "select Wage from " + DBInfo.TableMember + " where phone=\"" + LoginData.GetLoginData.LoginPhone + "\"";
            dBConn.DBOpen();
            rdr = dBConn.DBSelect(sql);

            rdr.Read();
            LoginData.GetLoginData.Wage = rdr["wage"].ToString();

            dBConn.DBClose();

            ScheduleData scheduleData = new ScheduleData();
            //출근,퇴근 시간, 분 분리
            int OnTime, OnMinute, OffTime, OffMinute;

            string[] swap = splitString(InputOnTime);

            OnTime = Convert.ToInt32(swap[0]);
            OnMinute = Convert.ToInt32(swap[1]);

            swap = splitString(InputOffTime);

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
                    scheduleData.Time = Time + ":" + Minute;
                }
                //야간시간
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

                    scheduleData.Time = Time + ":" + Minute;

                    Time = OffTime - 22;
                    Minute = OffMinute;
                    scheduleData.NightTime = Time + ":" + Minute;
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
                    scheduleData.Time = Time + ":" + Minute;
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
                    scheduleData.NightTime = Time + ":" + Minute;
                }
            }

            //총 시간
            if (!scheduleData.Time.Equals(""))
            {
                swap = splitString(scheduleData.Time);
                Time = Convert.ToInt32(swap[0]);
                Minute = Convert.ToInt32(swap[1]);
            }

            if (!scheduleData.NightTime.Equals(""))
            {
                swap = splitString(scheduleData.NightTime);
                Time += Convert.ToInt32(swap[0]);
                Minute += Convert.ToInt32(swap[1]);
            }

            if (Minute >= 60)
            {
                Time += 1;
                Minute -= 60;
            }
            scheduleData.TotalTime = Time + ":" + Minute;

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
                scheduleData.RestTime = Time + ":" + Minute;
            }

            //연장시간
            swap = splitString(scheduleData.TotalTime);
            Time = Convert.ToInt32(swap[0]);
            Minute = Convert.ToInt32(swap[1]);
            if (Time > 8)
            {
                Time -= 8;
                scheduleData.ExtensionTime = Time + ":" + Minute;

                //일반시간에서 연장시간 빼기
                int Time1, Minute1;
                swap = splitString(scheduleData.Time);
                Time = Convert.ToInt32(swap[0]);
                Minute = Convert.ToInt32(swap[1]);

                swap = splitString(scheduleData.ExtensionTime);
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
                swap = splitString(scheduleData.Time);
                Time = Convert.ToInt32(swap[0]);
                Minute = Convert.ToInt32(swap[1]);

                scheduleData.Wage = ((Convert.ToInt32(LoginData.GetLoginData.Wage) * Time) + (Convert.ToInt32(LoginData.GetLoginData.Wage) * Minute) / 60).ToString();
            }
            else
            {
                scheduleData.Wage = "0";
            }

            //휴계시급
            if (!scheduleData.RestTime.Equals(""))
            {
                swap = splitString(scheduleData.RestTime);
                Time = Convert.ToInt32(swap[0]);
                Minute = Convert.ToInt32(swap[1]);

                scheduleData.RestWage = ((Convert.ToInt32(LoginData.GetLoginData.Wage) * Time) + (Convert.ToInt32(LoginData.GetLoginData.Wage) * Minute) / 60).ToString();
            }
            else
            {
                scheduleData.RestWage = "0";
            }

            //연장시급
            if (!scheduleData.ExtensionTime.Equals(""))
            {
                swap = splitString(scheduleData.ExtensionTime);
                Time = Convert.ToInt32(swap[0]);
                Minute = Convert.ToInt32(swap[1]);

                scheduleData.ExtensionWage = string.Format("{0:F0}", ((Convert.ToInt32(LoginData.GetLoginData.Wage) * Time) + (Convert.ToInt32(LoginData.GetLoginData.Wage) * Minute) / 60) * 1.5);
            }
            else
            {
                scheduleData.ExtensionWage = "0";
            }
            //야간시급
            if (!scheduleData.NightTime.Equals(""))
            {
                swap = splitString(scheduleData.NightTime);
                Time = Convert.ToInt32(swap[0]);
                Minute = Convert.ToInt32(swap[1]);

                scheduleData.NightWage = string.Format("{0:F0}", ((Convert.ToInt32(LoginData.GetLoginData.Wage) * Time) + (Convert.ToInt32(LoginData.GetLoginData.Wage) * Minute) / 60) * 1.5);
            }
            else
            {
                scheduleData.NightWage = "0";
            }

            //총 시급
            scheduleData.TotalWage = (Convert.ToInt32(scheduleData.Wage) + Convert.ToInt32(scheduleData.ExtensionWage)
                + Convert.ToInt32(scheduleData.NightWage) - Convert.ToInt32(scheduleData.RestWage)).ToString();

            sql = "update " + DBInfo.TableSchedule + " set Time = \"" + scheduleData.Time + "\", RestTime = \"" + scheduleData.RestTime + "\", ExtensionTime = \"" + scheduleData.ExtensionTime + "\", " +
                "NightTime = \"" + scheduleData.NightTime + "\", TotalTime = \"" + scheduleData.TotalTime + "\", Wage = \"" + scheduleData.Wage + "\", RestWage = \"" + scheduleData.RestWage + "\", " +
                "ExtensionWage = \"" + scheduleData.ExtensionWage + "\", NightWage = \"" + scheduleData.NightWage + "\", TotalWage = \"" + scheduleData.TotalWage + "\"" +
                " where Phone = \"" + LoginData.GetLoginData.LoginPhone + "\" and Date = \"" + InputDate + "\"";

            dBConn.DBOpen();
            dBConn.DBManipulation(sql);
            dBConn.DBClose();
        }

        private string[] splitString(string str)
        {
            string[] result = new string[2];

            if (str.Contains(":"))
            {
                result = str.Split(':');
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

