using System;
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
        string sql;
        DataTable dt = new DataTable();

        public WageManagement()
        {
            InitializeComponent();

            this.ShowsNavigationUI = false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitComboBox();

        }

        /// <summary>
        /// 콤보박스 초기화
        /// </summary>
        private void InitComboBox()
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

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            wageMenger.ExportToExcel(DGSchedule, dt);

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            sql = "select * from " + DBInfo.TableSchedule + " where Phone=\"" + LoginData.GetLoginData.LoginPhone + "\" and Date Like \"" + year.Text + "-" + month.Text.ToString().PadLeft(2, '0') + "-%\"";

            //데이터테이블과 데이터베이스 연결     
            dt = wageMenger.MappingData(sql);

            DGSchedule.ItemsSource = dt.DefaultView;    //데이터 테이블 데이터 그리드 연동

            //데이터테이블에 데이터 리스트 연결, 데이터크리드에 데이터테이블 연결(사용X)
            /*List<ScheduleData> scheduleDatasList = wageMenger.selectSchedule(sql);

            DGSchedule.ItemsSource = wageMenger.MappingData(scheduleDatasList).DefaultView;*/

            //데이터 그리드에 리스트 연결
            /*ScheduleData scheduleData = new ScheduleData();
            int totalWage = 0;

            DGSchedule.Items.Clear();

            foreach (ScheduleData data in scheduleDatasList)
            {
                DGSchedule.Items.Add(data);
                totalWage += Convert.ToInt32(data.TotalWage);
            }

            scheduleData.Date = "합계";
            scheduleData.TotalWage = totalWage.ToString();
            DGSchedule.Items.Add(scheduleData);*/

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (dt.Rows.Count == 0)
            {
                return;
            }
            wageMenger.saveDataTable(dt);

            Search_Click(this, null);
        }
    }
}
