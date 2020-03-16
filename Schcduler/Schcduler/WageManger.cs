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
        DBConn dBConn = MainWindow.GetDBConn();


        public ScheduleData Select(string phone, string date)
        {
            SQLiteCommand command;
            SQLiteDataReader reader;
            ScheduleData scheduleData = new ScheduleData();
            string sql = "where Date=\""+date+"\"";

            dBConn.DBOpen();

            command = dBConn.Select(phone, sql);
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

        public int Update(string phone, ScheduleData scheduleData)
        {
            int result = -1;
            string sql = "Time = \"" + scheduleData.Time + "\", RestTime = \"" + scheduleData.RestTime + "\", ExtensionTime = \"" + scheduleData.ExtensionTime + "\", " +
               "NightTime = \"" + scheduleData.NightTime + "\", TotalTime = \"" + scheduleData.TotalTime + "\", Wage = \"" + scheduleData.Wage + "\", RestWage = \"" + scheduleData.RestWage + 
               "\", " +"ExtensionWage = \"" + scheduleData.ExtensionWage + "\", NightWage = \"" + scheduleData.NightWage + "\", TotalWage = \"" + scheduleData.TotalWage + "\"" +
               " where Date = \"" + scheduleData.Date + "\"";

            dBConn.DBOpen();
            dBConn.Update(phone, sql);
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

            if (!CheckDate(loginData))
            {
                dBConn.DBOpen();
                dBConn.Insert(loginData.Phone, sql);
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
            string sql = "OffTime =\"" + DateTime.Now.ToString("HH:mm") + "\" where Date=\"" + DateTime.Now.ToString("yyyy-MM-dd") + "\"";

            if (CheckDate(loginData))
            {
                dBConn.DBOpen();
                dBConn.Update(loginData.Phone, sql);
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
        private bool CheckDate(LoginData loginData)
        {
            SQLiteCommand command;
            SQLiteDataReader reader;
            bool result = false;
            string sql = " where Date=\""+DateTime.Now.ToString("yyyy-MM-dd")+"\"";
            
            dBConn.DBOpen();
            command = dBConn.Select(loginData.Phone, sql);

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
            catch(Exception ex)
            {
                Console.WriteLine("날짜체크실패 : " + ex.Message);
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

            string[] swap = splitString(scheduleData.OnTime);

            OnTime = Convert.ToInt32(swap[0]);
            OnMinute = Convert.ToInt32(swap[1]);

            swap = splitString(scheduleData.OffTime);

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

                scheduleData.Wage = ((Wage * Time) + (Wage * Minute) / 60).ToString();
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

                scheduleData.RestWage = ((Wage * Time) + (Wage * Minute) / 60).ToString();
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

                scheduleData.ExtensionWage = string.Format("{0:F0}", ((Wage * Time) + (Wage * Minute) / 60) * 1.5);
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
            string sql = "where Date LIKE \"" + yaer + "-" + month.PadLeft(2,'0') + "-" + "%\"";
            string phone ="";

            if(MemberData.GetMemberData.AuthorityData.Authority==3)
            {
                phone = MemberData.GetMemberData.Phone;
            }
            else
            {
                int index = TransitionPage.pgMain.cbName.SelectedIndex;
                phone = TransitionPage.pgMain.loginDataList[index].Phone;
                
            }

            dBConn.DBOpen();

            command = dBConn.Select(phone, sql);
            adapter = new SQLiteDataAdapter(command);
            adapter.Fill(dataSet);
            dataTable = dataSet.Tables[0];

            int TotalWage = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                if(!row["TotalWage"].ToString().Equals(""))
                {
                    TotalWage += Convert.ToInt32(row["TotalWage"]);
                }
            }

            DataRow datarow = dataTable.NewRow();
            datarow["NightWage"] = "합계";
            datarow["TotalWage"] = TotalWage;

            dataTable.Rows.Add(datarow);

            dBConn.DBClose();

            return dataTable;
        }

        /// <summary>
        /// 데이터 테이블의 변경사항만 DB에 저장
        /// </summary>
        /// <param name="dataTable"></param>
        public void SaveDataTable(DataTable dataTable)
        {
            DataTable dtChanges = dataTable.GetChanges(DataRowState.Modified);

            if (dtChanges != null)
            {
                dBConn.DBOpen();
                foreach (DataRow data in dtChanges.Rows)
                {
                    if ((!data["OnTime"].ToString().Contains(":") && data["OnTime"].ToString().Length != 4) || (!data["OffTime"].ToString().Contains(":") && data["OffTime"].ToString().Length != 4)
                        || (data["OnTime"].ToString().Contains(":") && data["OnTime"].ToString().Length > 5) || (data["OffTime"].ToString().Contains(":") && data["OffTime"].ToString().Length > 5))
                    {
                        return;
                    }
                    else
                    {
                        string[] swap = splitString(data["OnTime"].ToString());
                        int Time, Minute;
                        Time = Convert.ToInt32(swap[0]);
                        Minute = Convert.ToInt32(swap[1]);

                        if (Time < 0 || Time > 27 || Minute > 60 || Minute < 0)
                        {
                            return;
                        }

                        swap = splitString(data["OffTime"].ToString());
                        Time = Convert.ToInt32(swap[0]);
                        Minute = Convert.ToInt32(swap[1]);

                        if (Time < 0 || Time > 27 || Minute > 60 || Minute < 0)
                        {
                            return;
                        }
                    }

                    string phone = "";

                    if (MemberData.GetMemberData.AuthorityData.Authority == 3)
                    {
                        phone = MemberData.GetMemberData.Phone;
                    }
                    else
                    {
                        int index = TransitionPage.pgMain.cbName.SelectedIndex;
                        phone = TransitionPage.pgMain.loginDataList[index].Phone;
                    }

                    string sql = "OnTime = \"" + data["OnTime"] + "\", OffTime = \"" + data["OffTime"] + "\" where Date = \"" + data["Date"] + "\"";
                    dBConn.Update(phone, sql);
                    WageCalculation(phone, data["Date"].ToString());
                }
                dBConn.DBClose();
            }

        }

        public void ExportToExcel(DataGrid dataGrid, DataTable dataTable)
        {
            Excel.Application excel = new Excel.Application();
            Excel.Workbook workbook = excel.Workbooks.Add();
            Excel.Worksheet worksheet = excel.ActiveSheet;

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
                saveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

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
                workbook.SaveAs(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + saveFile.FileName);

                workbook.Close();
                excel.Quit();

                releaseObject(worksheet);
                releaseObject(workbook);
                releaseObject(excel);

                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + saveFile.FileName + ".xlsx");
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

