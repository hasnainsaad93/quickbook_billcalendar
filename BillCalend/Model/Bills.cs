using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Events;

namespace BillCalend.Model
{
    public class Bills
    {
        public int rowsCount;
        public List<Intuit.Ipp.Data.Bill> billRec = new List<Intuit.Ipp.Data.Bill>();
        public System.Collections.ObjectModel.ReadOnlyCollection<Intuit.Ipp.Data.Term> terms;
        public string errorMes;
        public bool bApplySuccess = false;
    }
}