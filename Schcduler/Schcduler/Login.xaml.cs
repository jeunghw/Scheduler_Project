using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Login.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Login : Page
    {

        LoginManager loginManager = new LoginManager();
        WageManger wageMenger = new WageManger();

        public Login()
        {
            InitializeComponent();

            tbId.Focus();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (tbId.Text.Trim().Length < 10 || tbPassword.Text.Trim() == "")
            {
                return;
            }
            LoginData.GetLoginData.LoginPhone = tbId.Text.Trim();
            LoginData.GetLoginData.LoginPassword = tbPassword.Text.Trim();

            MemberData dbmember = loginManager.selectMember();

            LoginData.GetLoginData.Wage = dbmember.Wage;
            LoginData.GetLoginData.UserName = dbmember.Name;

            if (dbmember.Phone.Equals(""))
            {
                MyMessageBox.createMessageBox(1, "핸드폰번호가 없습니다.", "");
                return;
            }
            if (!dbmember.Password.Equals(LoginData.GetLoginData.LoginPassword))
            {
                MyMessageBox.createMessageBox(1, "비밀번호가 틀렸습니다.", "");
                return;
            }

            tbId.Clear();
            tbPassword.Clear();

            TransitionPage.TransitionPages(1);
            TransitionPage.TransitionFrame(0);
        }

        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            if (tbId.Text.Trim().Length < 10)
            {
                MyMessageBox.createMessageBox(1, "핸드폰번호를 확인하세요.", "");
                tbId.Focus();
                return;
            }

            LoginData.GetLoginData.LoginPhone = tbId.Text.Trim();

            Button btn = sender as Button;

            wageMenger.insertTime();
        }

        private void btnOffInput_Click(object sender, RoutedEventArgs e)
        {
            if (tbId.Text.Trim().Length < 10)
            {
                MyMessageBox.createMessageBox(1, "핸드폰번호를 확인하세요.", "");
                tbId.Focus();
                return;
            }

            LoginData.GetLoginData.LoginPhone = tbId.Text.Trim();

            Button btn = sender as Button;

            if(wageMenger.updateTime()==0)
            {
                return;
            }
            wageMenger.WageCalculationt(wageMenger.scheduleDatasLists[0].OnTime, DateTime.Now.ToString("HH:mm"), DateTime.Now.ToString("yyyy-MM-dd"));
        }

        private void tbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                btnLogin_Click(this, null);
            }
        }

        private void tbId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                tbPassword.Focus();
            }
        }

        private void tbId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
