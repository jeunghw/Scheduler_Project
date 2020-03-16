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
            //권한 관련
            AuthorityManager authorityManager = new AuthorityManager();
            loginDataList = authorityManager.SelectName();

            List<ComboBoxItem> NameItems = new List<ComboBoxItem>();

            foreach(LoginData data in loginDataList)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = data.Name;
                NameItems.Add(item);
            }

            cbName.ItemsSource = NameItems;
            cbName.SelectedIndex = 0;

            if(MemberData.GetMemberData.AuthorityData.Authority==0)
            {
                btnSchedule.Visibility = Visibility.Hidden;
                btnTab2_Click(this, null);
            }
            else
            {
                btnSchedule.Visibility = Visibility.Visible;
                btnTab1_Click(this, null);
            }

            if (MemberData.GetMemberData.AuthorityData.Authority==3)
            {
                cbName.Visibility = Visibility.Hidden;
            }
            else if(MemberData.GetMemberData.AuthorityData.Authority==0)
            {
                cbName.Visibility = Visibility.Hidden;
            }
            else
            {
                cbName.Visibility = Visibility.Visible;
            }

            if (MemberData.GetMemberData.AuthorityData.SignUp == 1)
            {
                btnSingUP.Visibility = Visibility.Hidden;
            }
            else
            {
                btnSingUP.Visibility = Visibility.Visible;
            }
        }
    }
}
