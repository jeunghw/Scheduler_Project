using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    public class LoginData
    {
        public LoginData()
        {
            Phone = "";
            Password = "";
            Name = "";
            Wage = "";
            Authority = -1;
        }

        /// <summary>
        /// 핸드폰번호
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 비밀번호
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 이름
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 시급
        /// </summary>
        public string Wage { get; set; }
        /// <summary>
        /// 권한
        /// 0 : 프로그램 관리자
        /// 1 : 관리자
        /// 2 : 매니저
        /// 3 : 일반직원
        /// </summary>
        public int Authority { get; set; }
    }
}
