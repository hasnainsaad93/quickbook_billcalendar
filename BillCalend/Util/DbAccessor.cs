using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BillCalend.Model;
using BillCalend.Models;

namespace BillCalend.Util
{
    public class DbAccessor
    {
        ApplicationDbContext context;

        public DbAccessor()
        {
            context = new ApplicationDbContext();
        }

        public void GetAddedBills(ref List<string> lsIds, string username)
        {
            List<AddedBill> appbills = context.AppBills.Where(s => s.username == username).ToList();
            foreach (AddedBill b in appbills)
                lsIds.Add(b.bill_id.ToString());
        }

        public List<Event> GetEvents(string username)
        {
            List<Event> events = context.Events.Where(s => s.username == username).ToList();
            return events;
        }

        public void SaveDbAddedBill(Intuit.Ipp.Data.Bill bill, string emailClientId)
        {
            AddedBill billWr = new AddedBill();
            billWr.bill_id = Int32.Parse(bill.Id);
            billWr.bill_num = bill.DocNumber;
            billWr.username = emailClientId;
            context.AppBills.Add(billWr);
            context.SaveChanges();
        }

        public void SaveDbAddedEvent(DateTime start, DateTime end, string text, string emailClientId)
        {
            Event evn = new Event();
            evn.start = start;
            evn.end = end;
            evn.name = text;
            evn.username = emailClientId;
            context.Events.Add(evn);
            context.SaveChanges();
        }

        public UserInfo ReturnUserInfo(string sub, string email, string realmid, string authcode)
        {
            List<UserInfo> listUserInfo = context.UserInfoes.Where(x => x.RealmId == realmid).ToList();
            listUserInfo = listUserInfo.Where(x => x.PricingCode == "005").ToList();

            if (listUserInfo != null && listUserInfo.Count > 0)
            {
                return listUserInfo.FirstOrDefault();
            }
            else
            {
                List<UserInfo> listUserInfoInner = context.UserInfoes.Where(x => x.Sub == sub && x.Email == email && x.RealmId == realmid).ToList();
                if (listUserInfoInner != null && listUserInfoInner.Count > 0)
                {
                    return listUserInfoInner.FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
        }
        public void CreateUserInfo(string sub, string email, string realmid, string authcode, string pricingoption)
        {
            UserInfo userInfo = new UserInfo();
            userInfo.AuthCode = authcode;
            userInfo.RealmId = realmid;
            userInfo.Email = email;
            userInfo.Sub = sub;
            userInfo.PricingCode = pricingoption;
            userInfo.PricingStartDate = DateTime.Now;
            userInfo.PricingEndDate = DateTime.Now.AddMonths(1);
            context.UserInfoes.Add(userInfo);
            context.SaveChanges();
        }
    }
}