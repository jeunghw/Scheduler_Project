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
        static Login pgLogin = new Login();
        static MainPage pgMain = new MainPage();
        static public MainWindow window;
        static public Frame frame;

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
                frame.Navigate(new WageManagement());
            }
            else if (x == 1)
            {
                frame.Navigate(new Join());
            }
            else if (x == 2)
            {
                
            }
        }
    }
}
