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
        public static int createMessageBox(int number, string Contents, string title)
        {
            int result = -1;

            if (number == 1)
            {
                MessageBox.Show(Contents, title);
                result = 0;

            }
            else if(number==2)
            {
                if(MessageBox.Show(Contents, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    result = 0;
                }
                else
                {
                    result = 1;
                }
            }

            return result;
        }
    }
}
