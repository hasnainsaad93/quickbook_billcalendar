using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BillCalend.Model
{
    public class UserInfo
    {
        [Key]
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Sub { get; set; }
        public string RealmId { get; set; }
        public string AuthCode { get; set; }
        public string PricingCode { get; set; }
        public DateTime PricingStartDate { get; set; }
        public DateTime PricingEndDate { get; set; }

    }
}