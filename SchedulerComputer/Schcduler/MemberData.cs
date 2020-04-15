using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    class MemberData
    {
        private static MemberData memberData = new MemberData();
        private MemberData() 
        {
            Phone = "";
            Password = "";
            Name = "";
            Wage = "";
            AuthorityData = new AuthorityData();
            Task = -1;
        }
        public string Phone { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// 사용자이름
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 시급
        /// </summary>
        public string Wage { get; set; }
        /// <summary>
        /// 권한
        /// </summary>
        public AuthorityData AuthorityData { get; set; }
        /// <summary>
        /// 직무
        /// </summary>
        public int Task { get; set; }

        public static MemberData GetMemberData { get => memberData; }
    }
}
