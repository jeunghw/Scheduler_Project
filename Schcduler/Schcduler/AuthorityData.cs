using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schcduler
{
    public class AuthorityData
    {
        public AuthorityData()
        {
            Authority = -1;
            SignUp = -1;
            Modify = -1;
            Search = -1;
            Remove = -1;
        }
        /// <summary>
        /// 권한
        /// 0 : 프로그램 관리자
        /// 1 : 관리자
        /// 2 : 매니저
        /// 3 : 일반직원
        /// </summary>
        public int Authority { get; set; }
        /// <summary>
        /// 회원가입 권한
        /// 프로그램관리자, 관리자, 매니저 가능
        /// </summary>
        public int SignUp { get; set; }
        /// <summary>
        /// 수정 권한
        /// 관리자, 매니저, 본인
        /// </summary>
        public int Modify { get; set; }
        /// <summary>
        /// 검색 권한
        /// 관리자, 매니저
        /// </summary>
        public int Search { get; set; }
        /// <summary>
        /// 삭제권한
        /// 프로그램관리자, 관리자
        /// </summary>
        public int Remove { get; set; }
    }
}
