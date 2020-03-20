using System;
using System.Collections.Generic;
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
    /// MainPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainPage : Page
    {
        public List<LoginData> loginDataList;
        public MainPage()
        {
            InitializeComponent();

            TransitionPage.frame = fmInformation;
            TransitionPage.TransitionFrame(0);
        }

        private void btnTab1_Click(object sender, RoutedEventArgs e)
        {
            btnSchedule.Background = Brushes.Lavender;
            btnSingUP.Background = null;
            TransitionPage.TransitionFrame(0);
        }

        private void btnTab2_Click(object sender, RoutedEventArgs e)
        {
            btnSingUP.Background = Brushes.Lavender;
            btnSchedule.Background = null;
            TransitionPage.TransitionFrame(1);
        }

        private void btnlogout_Click(object sender, RoutedEventArgs e)
        {
            TransitionPage.TransitionPages(0);
        }

        private void btnTab3_Click(object sender, RoutedEventArgs e)
        {
            TransitionPage.TransitionFrame(2);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitComboBox();

            //근태관리 패이지 권한
            if (MemberData.GetMemberData.AuthorityData.Authority==0)
            {
                btnSchedule.Visibility = Visibility.Hidden;
                btnTab2_Click(this, null);
            }
            else
            {
                btnSchedule.Visibility = Visibility.Visible;
                btnTab1_Click(this, null);
            }

            //다른사람의 이름 선택 권한
            if (MemberData.GetMemberData.AuthorityData.Search==1)
            {
                cbName.Visibility = Visibility.Hidden;
            }
            else
            {
                cbName.Visibility = Visibility.Visible;
            }

            //회원가입 페이지 권한
            if (MemberData.GetMemberData.AuthorityData.SignUp == 1)
            {
                btnSingUP.Visibility = Visibility.Hidden;
            }
            else
            {
                btnSingUP.Visibility = Visibility.Visible;
            }
        }

        public void InitComboBox()
        {
            MemberManager memberManager = new MemberManager();
            loginDataList = memberManager.SelectName();

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
    }
}
