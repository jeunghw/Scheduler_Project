using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Schcduler
{
    public class TransitionPage
    {
        public static Login pgLogin = new Login();
        public static MainPage pgMain = new MainPage();
        public static MainWindow window;
        public static Frame frame;
        public static WageManagement wageManagement = new WageManagement();
        public static ScheduleManagement scheduleManagement = new ScheduleManagement();
        public static MemberManagement join = new MemberManagement();
        public static void TransitionPages(int x)
        {
            if (x == 0)
            {
                window.Width = 320;
                window.Height = 180;
                window.Content = pgLogin;
            }
            else if (x == 1)
            {
                window.Width = 1280;
                window.Height = 800;
                window.Content = pgMain;
            }
        }

        public static void TransitionFrame(int x)
        {
            frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;

            if (x == 0)
            {
                frame.Navigate(wageManagement);
            }
            else if (x == 1)
            {
                frame.Navigate(join);
            }
            else if (x == 2)
            {
                frame.Navigate(scheduleManagement);   
            }
        }
    }
}
