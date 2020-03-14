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
    /// Join.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Join : Page
    {

        string sql;
        DBConn dBConn = MainWindow.GetDBConn();
        LoginManager loginManager = new LoginManager();

        public Join()
        {
            InitializeComponent();
        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            if (txtPhone.Text.Trim().Length < 10 || txtPassword.Text.Trim().Equals("") || txtName.Text.Trim().Equals("") || tbWage.Text.Trim().Equals(""))
            {
                return;
            }

            LoginData.GetLoginData.LoginPhone = txtPhone.Text.Trim();
            LoginData.GetLoginData.LoginPassword = txtPassword.Text.Trim();
            LoginData.GetLoginData.UserName = txtName.Text.Trim();
            LoginData.GetLoginData.Wage = tbWage.Text.Trim();

            if (LoginData.GetLoginData.LoginPhone.Equals(loginManager.selectMember().Phone))
            {
                MyMessageBox.createMessageBox(1, "핸드폰번호가 중복됩니다.", "");
                return;
            }

            sql = "insert into " + DBInfo.TableMember + " values(\"" + LoginData.GetLoginData.LoginPhone + "\",\"" +
                LoginData.GetLoginData.LoginPassword + "\", \"" + LoginData.GetLoginData.UserName + "\",\"" + LoginData.GetLoginData.Wage + "\")";

            dBConn.DBOpen();
            dBConn.DBManipulation(sql);
            dBConn.DBClose();

            txtName.Clear();
            txtPassword.Clear();
            txtPhone.Clear();
            tbWage.Clear();

        }

        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                txtPassword.Focus();
            }
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                txtPhone.Focus();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                tbWage.Focus();
            }
        }

        private void tbWage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                btnJoin_Click(this, null);
            }
        }

        private void txtPassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbWage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
