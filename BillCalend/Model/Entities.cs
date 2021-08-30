using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillCalend.Models
{
    public class AddedBill
    {
        public int id { get; set; }
        public int bill_id { get; set; }
        public string bill_num { get; set; }
        public string username { get; set; }
    }

    public class Event
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public Int32? resource { get; set; }
        public string recurrence { get; set; }
        public string username { get; set; }
    }
}
