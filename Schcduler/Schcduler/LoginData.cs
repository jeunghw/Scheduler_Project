using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    class LoginData
    {
        private static LoginData loginData = new LoginData();

        private LoginData() { }
        /// <summary>
        /// 핸드폰번호
        /// </summary>
        public String LoginPhone { get; set; }
        public String LoginPassword { get; set; }
        /// <summary>
        /// 사용자이름
        /// </summary>
        public string UserName { get; set; }
        public string Wage { get; set; }
        public static LoginData GetLoginData { get => loginData; }
    }
}
