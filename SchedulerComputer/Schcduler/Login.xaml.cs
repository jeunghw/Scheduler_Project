﻿using System;
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
        MemberManager memberManager = new MemberManager();
        WageManger wageMenger = new WageManger();
        EncryptionManager encryptionManager = new EncryptionManager();

        public Login()
        {
            InitializeComponent();

            tbId.Focus();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            if (tbId.Text.Trim().Length < 10 || pbPassword.Password.Trim() == "")
            {
                return;
            }

            LoginData inputLoginData = new LoginData();
            inputLoginData.Phone = tbId.Text.Trim();
            inputLoginData.Password = pbPassword.Password.Trim();

            LoginData loginData = new LoginData();

            loginData = memberManager.Select(inputLoginData);

            if (loginData.Phone.Equals(""))
            {
                MyMessageBox.createMessageBox(1, "핸드폰번호가 없습니다.", "");
                return;
            }
            if (!loginData.Password.Equals(encryptionManager.EncryptionPassword(inputLoginData)))
            {
                MyMessageBox.createMessageBox(1, "비밀번호가 틀렸습니다.", "");
                return;
            }

            memberManager.SetMemberData(loginData);

            tbId.Clear();
            pbPassword.Clear();

            TransitionPage.TransitionPages(1);
            TransitionPage.TransitionFrame(0);
        }


        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            LoginData loginData = new LoginData();

            if (tbId.Text.Trim().Length < 10)
            {
                MyMessageBox.createMessageBox(1, "핸드폰번호를 확인하세요.", "");
                tbId.Focus();
                return;
            }

            loginData.Phone = tbId.Text.Trim();

            //년도가 바뀌면 자동으로 테이블을 생성
            memberManager.Create(loginData);

            wageMenger.OnWork(loginData);
            tbId.Clear();
        }

        private void btnOffInput_Click(object sender, RoutedEventArgs e)
        {
            LoginData loginData = new LoginData();
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("HH:mm");

            if (tbId.Text.Trim().Length < 10)
            {
                MyMessageBox.createMessageBox(1, "핸드폰번호를 확인하세요.", "");
                tbId.Focus();
                return;
            }

            loginData.Phone = tbId.Text.Trim();

            if (wageMenger.OffWork(loginData) == -1)
                return;

            string[] swapTime = time.Split(':');
            string[] swapDate = date.Split('-');

            if (Convert.ToInt32(swapTime[0]) < 5)
            {
                swapDate[2] = (Convert.ToInt32(swapDate[2]) - 1).ToString();
                date = swapDate[0] + "-" + swapDate[1] + "-" + swapDate[2];

            }

            wageMenger.WageCalculation(loginData.Phone, date);
            tbId.Clear();
        }

        private void pbPassword_KeyDown(object sender, KeyEventArgs e)
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
                pbPassword.Focus();
            }
        }

        private void tbId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
