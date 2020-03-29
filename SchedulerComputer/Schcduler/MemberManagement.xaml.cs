using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
        SQLiteManager sqliteManager = MainWindow.GetSqliteManager();
        MySQLManager mysqlManager = MainWindow.GetMySQLManager();
        LoginData loginData = new LoginData();
        MemberManager memberManager = new MemberManager();

        public Join()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitCBJoin();
            initCBSecession();

            if (MemberData.GetMemberData.AuthorityData.Authority == 1)
            {
                btnSecession.IsEnabled = true;
            }
            else
            {
                btnSecession.IsEnabled = false;
            }
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

            if (loginData.Phone.Equals(memberManager.Select(loginData).Phone))
            {
                MyMessageBox.createMessageBox(1, "핸드폰번호가 중복됩니다.", "");
                return;
            }

            memberManager.Insert(loginData);
            memberManager.Create(loginData);

            txtName.Clear();
            txtPassword.Clear();
            txtPhone.Clear();
            tbWage.Clear();

            TransitionPage.pgMain.InitComboBox();
            initCBSecession();

            MessageBox.Show("가입이 완료되었습니다.");

        }

        private void btnSecession_Click(object sender, RoutedEventArgs e)
        {
            string sql = "where Phone=\"" + tbPhone2.Text + "\"";

            sqliteManager.DBOpen();

            if (MyMessageBox.createMessageBox(2, "이름 : "+cbName.Text+" 핸드폰번호 : "+tbPhone2.Text+" 사용자를 삭제하시겠습니까?", "회원삭제확인")==0)
            {
                sqliteManager.Delete(SQLiteData.TableMember, sql);
                sqliteManager.Drop(tbPhone2.Text);

                Thread thread = new Thread(() => MainWindow.runThread(3, MySQLData.TableMember, sql));
                thread.Start();
            }

            sqliteManager.DBClose();

            initCBSecession();
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
        private void InitCBJoin()
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

        List<LoginData> loginDataList= new List<LoginData>();
        private void initCBSecession()
        {
            MemberManager memberManager = new MemberManager();
            List<ComboBoxItem> NameItems = new List<ComboBoxItem>();

            loginDataList = memberManager.SelectName();

            foreach (LoginData data in loginDataList)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = data.Name;
                NameItems.Add(item);
            }

            cbName.SelectedIndex = 0;
            cbName.ItemsSource = NameItems;

        }

        private void cbName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbName.SelectedIndex == -1)
            {
                tbPhone2.Text = loginDataList[0].Phone;
                cbName.SelectedIndex = 0;
            }
            else
            {
                tbPhone2.Text = loginDataList[cbName.SelectedIndex].Phone;
            }
        }
    }
}
