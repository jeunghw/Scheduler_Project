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
        DataTable dataTable = new DataTable();
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
            wageMenger.ExportToExcel(DGSchedule, dataTable);

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //데이터테이블과 데이터베이스 연결
            dataTable = new DataTable();
            dataTable = wageMenger.MappingDataTable(year.Text, month.Text);

            DGSchedule.ItemsSource = dataTable.DefaultView;    //데이터 테이블 데이터 그리드 연동

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (dataTable.Rows.Count == 0)
            {
                return;
            }

            wageMenger.SaveDataTable(dataTable);

            Search_Click(this, null);
        }
    }
}
