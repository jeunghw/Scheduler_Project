using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Schcduler
{
    public class MyMessageBox
    {
        public MyMessageBox() { }

        /// <summary>
        /// 메세지박스 재정의해서 사용
        /// </summary>
        /// <param name="number">메세지박스종류선택</param>
        /// <param name="Contents">내용</param>
        /// <param name="title">제목</param>
        public static void createMessageBox(int number, string Contents, string title)
        {
            if (number == 1)
            {
                MessageBox.Show(Contents, title);
            }

        }
    }
}
