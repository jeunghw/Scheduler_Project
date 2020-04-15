using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Schcduler
{
    /// <summary>
    /// ScheduleManagement.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ScheduleManagement : Page
    {
        ScheduleManager scheduleManager = new ScheduleManager();
        DataTable dataTable;

        public ScheduleManagement()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            inItDGSchedule(); 
            inItcbYear();
            inItcbMonth();
            inItcbTask();
        }

        private void inItDGSchedule()
        {
            string[] dayOfWeek = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            string[] date = DateTime.Now.ToString("yyyy-MM-dd").Split('-');
            int year = Convert.ToInt32(date[0]), month = Convert.ToInt32(date[1]), day = Convert.ToInt32(date[2]);

            List<DateWeek> dateWeekList = scheduleManager.getWeekOfFirstDay(year, month, day);

            if (DGSchedule.Columns.Count < 3)
            {
                //해당 형식은 UI부분은 요일(날짜) 및 하위 출근/퇴근 형식으로 나오지만 저장이 되는지 알수없음(보여주는 기능은 작동하지만 DataTable와의 연동 실패)

                /*TextBlock[] textBlocks = { tbMonDate, tbTueDate, tbWedDate,tbThuDate, tbFriDate, tbSatDate, tbSunDate };

                for (int i = 0; i < dateWeekList.Count; i++)
                {
                    string[] swap = dateWeekList[i].date.Split('-');
                    string headerDate = "(" + swap[1] + "/" + swap[2] + ")";

                    textBlocks[i].Text = dateWeekList[i].week + headerDate;
                }*/

                //해당 형식은 데이터 바인딩은 편하지만 UI부분이 요일(날짜) 형식으로 나와 그 아래컬럼으로 출근 퇴근을 작성해야해서 개선이 필요(기능은 완료)
                //데이터그리드 컬럼 생성 및 추가
                for (int i = 0; i < dateWeekList.Count; i++)
                {
                    DataGridTextColumn column1 = new DataGridTextColumn();

                    string[] swap = dateWeekList[i].date.Split('-');
                    string headerDate = swap[1] + "/" + swap[2];

                    column1.Header = "Date";
                    column1.Binding = new Binding(dateWeekList[i].date + "Date");
                    DGSchedule.Columns.Add(column1);
                    column1.Visibility = Visibility.Hidden;

                    DataGridTextColumn column2 = new DataGridTextColumn();


                    column2.Header = dateWeekList[i].week + "\r\n" + "출근";
                    column2.Binding = new Binding(dateWeekList[i].date + "OnTime");
                    DGSchedule.Columns.Add(column2);

                    DataGridTextColumn column3 = new DataGridTextColumn();

                    column3.Header = headerDate + "\r\n" + "퇴근";
                    column3.Binding = new Binding(dateWeekList[i].date + "OffTime");
                    DGSchedule.Columns.Add(column3);

                }
                dataTable = scheduleManager.inItDataTable(dateWeekList, "",-1);
                DGSchedule.ItemsSource = dataTable.DefaultView;
            }
        }

        //데이터 테이블을 검색된 날짜에 맞춰서 재구축
        private void setDGSchedule(string startDate, string endDate)
        {
            List<DateWeek> dateWeekList = scheduleManager.getWeekDateList(startDate, endDate);

            //데이터 그리드를 지움
            int count = DGSchedule.Columns.Count;
            

            for (int i = 2; i < count; i++)
            {
                DGSchedule.Columns.RemoveAt(2);
            }

            for (int i = 0; i < dateWeekList.Count; i++)
            {
                DataGridTextColumn column1 = new DataGridTextColumn();

                string[] swap = dateWeekList[i].date.Split('-');
                string headerDate = swap[1] + "/" + swap[2];

                column1.Header = "Date";
                column1.Binding = new Binding(dateWeekList[i].date + "Date");
                DGSchedule.Columns.Add(column1);
                column1.Visibility = Visibility.Hidden;

                DataGridTextColumn column2 = new DataGridTextColumn();


                column2.Header = dateWeekList[i].week + "\r\n" + "출근";
                column2.Binding = new Binding(dateWeekList[i].date + "OnTime");
                DGSchedule.Columns.Add(column2);

                DataGridTextColumn column3 = new DataGridTextColumn();

                column3.Header = headerDate + "\r\n" + "퇴근";
                column3.Binding = new Binding(dateWeekList[i].date + "OffTime");
                DGSchedule.Columns.Add(column3);

            }

            string phone = "";
            if(cbName.SelectedIndex==0)
            {
                phone = "";
            }
            else
            {
                phone = loginDataTaskList[cbName.SelectedIndex-1].Phone;
            }


            dataTable = scheduleManager.inItDataTable(dateWeekList, phone, cbTask.SelectedIndex);
            DGSchedule.ItemsSource = dataTable.DefaultView;
        }

        List<LoginData> loginDataTaskList;

        /// <summary>
        /// 이름 콤보박스 초기화
        /// </summary>
        private void inItcbName(int task)
        {
            List<LoginData> loginDataList = CommData.GetCommData().getLoginDataList();
            loginDataTaskList = new List<LoginData>();

            if (task == 0)
            {
                loginDataTaskList = loginDataList;
            }
            else if(task == 1)
            {
                for(int i=0; i<loginDataList.Count; i++)
                {
                    if (loginDataList[i].Task == 0 || loginDataList[i].Task == 1)
                    {
                        loginDataTaskList.Add(loginDataList[i]);
                    }
                }
            }
            else if(task == 2)
            {
                for (int i = 0; i < loginDataList.Count; i++)
                {
                    if (loginDataList[i].Task == 0 || loginDataList[i].Task == 2)
                    {
                        loginDataTaskList.Add(loginDataList[i]);
                    }
                }
            }

            List<ComboBoxItem> NameItems = new List<ComboBoxItem>();
            NameItems.Add(new ComboBoxItem {Content = "전체" });

            foreach (LoginData data in loginDataTaskList)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = data.Name;
                NameItems.Add(item);
            }

            cbName.ItemsSource = NameItems;
            cbName.SelectedIndex = 0;
            //콤보박스 인덱스를 자기자신으로 설정
            //현재 로그인된 사용자의 핸드폰번호로 loginDataList에서 검색해서 해당 객체를 가져온뒤 해당 객체가 몇번째 인덱스인지 검색
            //cbName.SelectedIndex = loginDataList.IndexOf(loginDataList.Find(x => x.Phone.Contains(MemberData.GetMemberData.Phone)));
        }

        /// <summary>
        /// 분류 콤보박스 초기화
        /// </summary>
        private void inItcbTask()
        {
            List<ComboBoxItem> taskItems = new List<ComboBoxItem>();

            ComboBoxItem item0 = new ComboBoxItem();
            ComboBoxItem item1 = new ComboBoxItem();
            ComboBoxItem item2 = new ComboBoxItem();
            item0.Content = "전체";
            item1.Content = "홀";
            item2.Content = "주방";
            taskItems.Add(item0);
            taskItems.Add(item1);
            taskItems.Add(item2);

            cbTask.ItemsSource = taskItems;
            cbTask.SelectedIndex = 0; 
        }

        /// <summary>
        /// 년도 콤보박스 초기화
        /// </summary>
        private void inItcbYear()
        {

            List<ComboBoxItem> yearItems = new List<ComboBoxItem>();

            int years = Convert.ToInt32(DateTime.Now.ToString("yyyy"));

            for (int i = years - 5; i < years + 6; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i.ToString();
                yearItems.Add(item);
            }

            cbyear.ItemsSource = yearItems;
            cbyear.SelectedIndex = 5;
        }
        /// <summary>
        /// 달 콤보박스 초기화
        /// </summary>
        private void inItcbMonth()
        {
            List<ComboBoxItem> monthItems = new List<ComboBoxItem>();

            for (int i = 1; i < 13; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i.ToString();
                monthItems.Add(item);
            }

            cbmonth.ItemsSource = monthItems;
            cbmonth.SelectedIndex = Convert.ToInt32(DateTime.Now.ToString("MM")) - 1;
            inItcbDays();
        }

        private void inItcbDays()
        {
            if (!cbyear.Text.Equals("") && !cbmonth.Text.Equals(""))
            {
                //일 콤보박스 초기화
                List<ComboBoxItem> dayItems1 = new List<ComboBoxItem>();
                List<ComboBoxItem> dayItems2 = new List<ComboBoxItem>();

                //선택된 년도 달의 마지막 날을 가져옴
                int lastDay = DateTime.DaysInMonth(Convert.ToInt32(cbyear.Text), Convert.ToInt32(cbmonth.Text));

                for (int i = 1; i <= lastDay; i++)
                {
                    ComboBoxItem item1 = new ComboBoxItem();
                    ComboBoxItem item2 = new ComboBoxItem();
                    item1.Content = i.ToString();
                    item2.Content = i.ToString();
                    dayItems1.Add(item1);
                    dayItems2.Add(item2);
                }

                cbDay1.ItemsSource = dayItems1;
                cbDay1.SelectedIndex = 0;

                cbDay2.ItemsSource = dayItems2;
                cbDay2.SelectedIndex = dayItems2.Count-1;

                cbDay1.IsEnabled = true;
                cbDay2.IsEnabled = true;
            }
        }

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            scheduleManager.SaveDataTable(dataTable);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string startDate = cbyear.Text + "-" + cbmonth.Text.ToString().PadLeft(2, '0') + "-" + cbDay1.Text.ToString().PadLeft(2, '0');
            string endDate = cbyear.Text + "-" + cbmonth.Text.ToString().PadLeft(2, '0') + "-" + cbDay2.Text.ToString().PadLeft(2, '0');

            setDGSchedule(startDate, endDate);
        }

        private void cbTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            inItcbName(cbTask.SelectedIndex);
        }
    }
}
