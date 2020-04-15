using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schcduler
{
    class ScheduleData
    {
        public ScheduleData()
        {
            Date = "";
            Phone = "";
            OnTime = "";
            OffTime = "";
        }
        public string Date { get; set; }
        public string Phone { get; set; }
        public string OnTime { get; set; }
        public string OffTime { get; set; }
    }
}
