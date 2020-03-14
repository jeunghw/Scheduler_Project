using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    public class MemberData
    {
        public MemberData()
        {
            Phone = "";
            Password = "";
            Name = "";
            Wage = "";
        }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Wage { get; set; }
    }
}
