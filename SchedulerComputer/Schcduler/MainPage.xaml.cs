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
        
        public MainPage()
        {
            InitializeComponent();

            TransitionPage.frame = fmInformation;
            btnWage.Background = Brushes.Lavender;
            TransitionPage.TransitionFrame(0);
        }

        private void btnTab1_Click(object sender, RoutedEventArgs e)
        {
            btnWage.Background = Brushes.Lavender;
            btnSingUP.Background = null;
            btnSchedule.Background = null;
            TransitionPage.TransitionFrame(0);
        }

        private void btnTab2_Click(object sender, RoutedEventArgs e)
        {
            btnSingUP.Background = Brushes.Lavender;
            btnWage.Background = null;
            btnSchedule.Background = null;
            TransitionPage.TransitionFrame(1);
        }

        private void btnTab3_Click(object sender, RoutedEventArgs e)
        {
            btnSchedule.Background = Brushes.Lavender;
            btnSingUP.Background = null;
            btnWage.Background = null;
            TransitionPage.TransitionFrame(2);
        }

        private void btnlogout_Click(object sender, RoutedEventArgs e)
        {
            TransitionPage.TransitionPages(0);
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //현재 자용자의 이름 표시
            lbName.Content = MemberData.GetMemberData.Name;
            AuthorityManager authorityManager = new AuthorityManager();

            if (authorityManager.SignUpCheck(btnSingUP) == 1)
            {
                btnTab2_Click(this, null);
            }
            if (authorityManager.AuthorityCheck(btnWage) == 1)
            {
                btnTab1_Click(this, null);
            }
            if(authorityManager.ScheduleCheck(btnSchedule) == 1)
            {
                btnTab3_Click(this, null);
            }
 
        }
    }
}
