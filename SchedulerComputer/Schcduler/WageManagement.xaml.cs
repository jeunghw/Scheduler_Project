﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schcduler
{
    /// <summary>
    /// WageManagement.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WageManagement : Page
    {
        WageManger wageMenger = new WageManger();
        DataTable dataTable = new DataTable("dtScheduler");

        public WageManagement()
        {
            InitializeComponent();

            this.ShowsNavigationUI = false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(MemberData.GetMemberData.AuthorityData.Authority == 3)
            {
                btnAddRow.IsEnabled = false;
                btnDelete.IsEnabled = false;
                Save.IsEnabled = false;
                cbName.IsEnabled = false;
                cbDay.IsEnabled = false;
            }
            InitYearComboBox();
            InitMonthComboBox();
            InitNameComboBox();
        }

        /// <summary>
        /// 년도 콤보박스 초기화
        /// </summary>
        private void InitYearComboBox()
        {
            
            List<ComboBoxItem> yearItems = new List<ComboBoxItem>();

            int years = Convert.ToInt32(DateTime.Now.ToString("yyyy"));

            for (int i = years - 5; i < years + 6; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i.ToString();
                yearItems.Add(item);
            }

            year.ItemsSource = yearItems;
            year.SelectedIndex = 5;
        }
        /// <summary>
        /// 달 콤보박스 초기화
        /// </summary>
        private void InitMonthComboBox()
        {
            List<ComboBoxItem> monthItems = new List<ComboBoxItem>();

            for (int i = 1; i < 13; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i.ToString();
                monthItems.Add(item);
            }

            month.ItemsSource = monthItems;
            month.SelectedIndex = Convert.ToInt32(DateTime.Now.ToString("MM")) - 1;
        }

        /// <summary>
        /// 이름콤보박스 초기화
        /// </summary>
        public void InitNameComboBox()
        {
            List<LoginData> loginDataList = CommData.GetCommData().getLoginDataList();

            List<ComboBoxItem> NameItems = new List<ComboBoxItem>();

            foreach (LoginData data in loginDataList)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = data.Name;
                NameItems.Add(item);
            }

            cbName.ItemsSource = NameItems;
            //콤보박스 인덱스를 자기자신으로 설정
            //현재 로그인된 사용자의 핸드폰번호로 loginDataList에서 검색해서 해당 객체를 가져온뒤 해당 객체가 몇번째 인덱스인지 검색
            cbName.SelectedIndex = loginDataList.IndexOf(loginDataList.Find(x => x.Phone.Contains(MemberData.GetMemberData.Phone)));
        }

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            wageMenger.ExportToExcel(DGWage, dataTable);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //데이터테이블과 데이터베이스 연결
            dataTable = new DataTable();
            dataTable = wageMenger.MappingDataTable(year.Text, month.Text);

            DGWage.ItemsSource = dataTable.DefaultView;    //데이터 테이블 데이터 그리드 연동

            month_SelectionChanged(this, null);
            btnAddRow.IsEnabled = true;
            cbDay.IsEnabled = true;

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (dataTable.Rows.Count == 0)
            {
                return;
            }

            wageMenger.SaveDataTable(dataTable);
            btnAddRow.IsEnabled = true;
            Search_Click(this, null);
        }

        private void btnAddRow_Click(object sender, RoutedEventArgs e)
        {
            string date = year.Text + "-" + month.Text.PadLeft(2,'0') + "-" + cbDay.Text.PadLeft(2, '0');
            wageMenger.AddDataRow(dataTable, date);
            month_SelectionChanged(this, null);
        }

        private void month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!year.Text.Equals("") && !month.Text.Equals(""))
            {
                //일 콤보박스 초기화
                List<ComboBoxItem> dayItems = new List<ComboBoxItem>();

                //선택된 년도 달의 마지막 날을 가져옴
                int lastDay = DateTime.DaysInMonth(Convert.ToInt32(year.Text), Convert.ToInt32(month.Text));

                //데이터 그리드에 현재 있는 날을 가져옴
                List<int> daysList = wageMenger.GetDataTableDayList(dataTable);

                for (int i = 1; i <= lastDay; i++)
                {
                    if (!daysList.Contains(i))
                    {
                        ComboBoxItem item = new ComboBoxItem();
                        item.Content = i.ToString();
                        dayItems.Add(item);
                    }
                }

                cbDay.ItemsSource = dayItems;
                cbDay.SelectedIndex = 0;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DGWage.SelectedIndex < 0)
            {
                DGWage.SelectedIndex = 0;
            }
            if (DGWage.SelectedIndex == (dataTable.Rows.Count - 1))
            {
                return;
            }
            btnAddRow.IsEnabled = false;

            wageMenger.DeleteDataTableRow(dataTable, DGWage.SelectedIndex);

            wageMenger.DeleteDataTable(dataTable);

            Save_Click(this,null);
        }

    }
}
