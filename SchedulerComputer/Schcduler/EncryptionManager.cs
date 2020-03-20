using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    class EncryptionManager
    {
        /// <summary>
        /// 문자열을 받아서 암호화한 문자열로 돌려줌
        /// </summary>
        /// <param name="loginData">비밀번호</param>
        /// <returns></returns>
        public string EncryptionPassword(LoginData loginData)
        {
            string result = "";

            SHA256Managed sHA256Managed = new SHA256Managed();
            result = Convert.ToBase64String(sHA256Managed.ComputeHash(Encoding.UTF8.GetBytes(loginData.Phone+loginData.Password)));

            return result;
        }
    }
}
