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
        LoginData loginData = new LoginData();
        JoinManager joinManager = new JoinManager();

        public Join()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitComboBox();
        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            if (txtPhone.Text.Trim().Length < 10 || txtPassword.Text.Trim().Equals("") || txtName.Text.Trim().Equals("") || tbWage.Text.Trim().Equals(""))
            {
                return;
            }

            loginData.Phone = txtPhone.Text.Trim();
            loginData.Password = txtPassword.Text.Trim();
            loginData.Name = txtName.Text.Trim();
            loginData.Wage = tbWage.Text.Trim();
            loginData.Authority = cbAuthority.SelectedIndex+1; 

            if (loginData.Phone.Equals(loginManager.Select(loginData).Phone))
            {
                MyMessageBox.createMessageBox(1, "핸드폰번호가 중복됩니다.", "");
                return;
            }

            joinManager.Create(loginData);
            joinManager.Insert(loginData);

            txtName.Clear();
            txtPassword.Clear();
            txtPhone.Clear();
            tbWage.Clear();

            MessageBox.Show("가입이 완료되었습니다.");

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

        /// <summary>
        /// 콤보박스 초기화
        /// </summary>
        private void InitComboBox()
        {
            List<ComboBoxItem> AuthorityItems = new List<ComboBoxItem>();

            string[] Authority = { "관리자", "매니저", "일반직원" };

            for (int i = 0; i< Authority.Length; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = Authority[i];
                AuthorityItems.Add(item);
            }

            cbAuthority.ItemsSource = AuthorityItems;
            cbAuthority.SelectedIndex = 2;

        }
    }
}
