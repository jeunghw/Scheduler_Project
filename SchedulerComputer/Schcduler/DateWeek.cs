using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    public class DateWeek
    {
        public DateWeek(string date, string week)
        {
            this.week = week;
            this.date = date;
        }

        public DateWeek()
        {

        }
        public string week { get; set; }
        public string date { get; set; }
    }
}
