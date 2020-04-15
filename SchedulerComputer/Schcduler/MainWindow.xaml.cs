using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        static SQLiteManager sqliteManager = new SQLiteManager();
        static MySQLManager mysqlManager = new MySQLManager();
        private static object lockObject = new object();

        public MainWindow()
        {
            InitializeComponent();

            TransitionPage.window = this;
            TransitionPage.TransitionPages(0);
        }


        public static SQLiteManager GetSqliteManager()
        {
            return sqliteManager;
        }

        public static MySQLManager GetMySQLManager()
        {
            return mysqlManager;
        }

        /// <summary>
        /// 스레드 함수(MySql 쿼리문 실행)
        /// </summary>
        /// <param name="number">
        /// 1: select
        /// 2: update
        /// 3: delete
        /// 4: insert
        /// </param>
        /// <param name="tableName">테이블명</param>
        /// <param name="sql">추가sql</param>
        public static void runThread(int number, string tableName, string sql)
        {
            Monitor.Enter(lockObject);
            try
            {

                mysqlManager.DBOpen();

                if (number == 1)
                {
                    mysqlManager.Select(tableName, sql);
                }
                else if (number == 2)
                {
                    mysqlManager.Update(tableName, sql);
                }
                else if (number == 3)
                {
                    mysqlManager.Delete(tableName, sql);
                }
                else if(number == 4)
                {
                    mysqlManager.Insert(tableName, sql);
                }
                mysqlManager.DBClose();
            }
            finally
            {
                Monitor.Exit(lockObject);
            }
        }
    }
}
