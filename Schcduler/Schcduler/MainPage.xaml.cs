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
            TransitionPage.TransitionFrame(0);
        }

        private void btnTab1_Click(object sender, RoutedEventArgs e)
        {
            TransitionPage.TransitionFrame(0);
        }

        private void btnTab2_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
