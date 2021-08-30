using Intuit.Ipp.OAuth2PlatformClient;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Net;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.DataService;
using Intuit.Ipp.Security;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using BillCalend.Model;
using BillCalend.Util;
using System.Web.Script.Serialization;
using DayPilot.Web.Mvc.Enums.Calendar;
using DayPilot.Web.Mvc.Enums;
using DayPilot.Web.Mvc.Events;
using DayPilot.Web.Mvc;
using Stripe;
using System.Threading;

namespace BillCalend.Controllers
{
    public class SelVals
    {
        public int selectedIndex { get; set; }
        public string discOpt { get; set; }
    }

    public class AppController : Controller
    {
        public static string clientid = ConfigurationManager.AppSettings["clientid"];
        public static string clientsecret = ConfigurationManager.AppSettings["clientsecret"];
        public static string redirectUrl = ConfigurationManager.AppSettings["redirectUrl"];
        public static string environment = ConfigurationManager.AppSettings["appEnvironment"];

        public static OAuth2Client auth2Client = new OAuth2Client(clientid, clientsecret, redirectUrl, environment);

        private static ServiceContext srvCnxt;
        private static DataService dataService;
        private static List<Intuit.Ipp.Data.Bill> lCurBills;
        private static DbAccessor dbAccessor = new DbAccessor();
        //private string sFlAppBills = @"C:\temp\appbills.txt";
        private string sFlAppBills = "appbills.txt";
        private static string accessToken;

        /// <summary>
        /// Use the Index page of App controller to get all endpoints from discovery url
        /// </summary>
        public ActionResult Index()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Session.Clear();
            Session.Abandon();
            Request.GetOwinContext().Authentication.SignOut("Cookies");
            return View();
        }

        /// <summary>
        /// Start Auth flow
        /// </summary>
        public ActionResult InitiateAuth(string submitButton)
        {
            submitButton = "Connect to QuickBooks";
            switch (submitButton)
            {
                case "Connect to QuickBooks":
                    List<OidcScopes> scopes = new List<OidcScopes>();
                    scopes.Add(OidcScopes.Accounting);
                    //scopes.Add(OidcScopes.Profile);
                    scopes.Add(OidcScopes.OpenId);
                    scopes.Add(OidcScopes.Email);
                    string authorizeUrl = auth2Client.GetAuthorizationURL(scopes);
                    return Redirect(authorizeUrl);
                default:
                    return (View());
            }
        }
        public ActionResult SignOut()
        {
            Session.Abandon();
            Session.Clear();
            auth2Client.RevokeTokenAsync(accessToken);
            return View("Index");
        }
        public async Task<ActionResult> Disconnect()
        {
            await auth2Client.RevokeTokenAsync(accessToken);
            //return RedirectToAction("InitiateAuth", "App");
            return RedirectToAction("ApiCallService", "App");

        }
        public ActionResult RedirectToConnection()
        {
            return RedirectToAction("InitiateAuth", "App");

        }

        static public string GetUserName()
        {
            var userInfoResp = auth2Client.GetUserInfoAsync(accessToken).Result;
            var email = userInfoResp.Claims.Where(x => x.Type == "email").Select(x => x.Value).SingleOrDefault();
            var emailClientId = email + auth2Client.ClientID;
            return emailClientId;
        }

        private List<string> FilterPercentTerms(ref System.Collections.ObjectModel.ReadOnlyCollection<Term> terms)
        {
            List<string> lsTerms = new List<string>();
            foreach (Intuit.Ipp.Data.Term term in terms)
            {
                int ind = term.Name.IndexOf("%");
                if (ind != -1)
                {
                    string sNum = term.Name.Substring(0, ind);
                    if (sNum.All(Char.IsDigit))
                        lsTerms.Add(term.Id);
                }
            }

            return lsTerms;
        }

        private List<Intuit.Ipp.Data.Bill> SelectBillByTerms(ref System.Collections.ObjectModel.ReadOnlyCollection<Intuit.Ipp.Data.Bill> bills, ref List<string> terms)
        {
            List<Intuit.Ipp.Data.Bill> selBills = new List<Intuit.Ipp.Data.Bill>();
            foreach (Intuit.Ipp.Data.Bill bill in bills)
            {
                string termVal = bill.SalesTermRef?.Value?.ToString() ?? "";
                if (terms.Contains(termVal))
                    selBills.Add(bill);
            }

            return selBills;
        }

        private void AddDiscountToBill(Intuit.Ipp.Data.Bill bill, double discount)
        {
            Intuit.Ipp.Data.Bill objBill = new Intuit.Ipp.Data.Bill();

            objBill.Id = bill.Id;
            objBill.SyncToken = bill.SyncToken;

            objBill.VendorRef = new ReferenceType();
            objBill.VendorRef = bill.VendorRef;
            objBill.sparse = true;
            objBill.sparseSpecified = true;

            List<Line> lineList = new List<Line>();
            // copy existing lines
            List<string> accountsRefLs = new List<string>();
            int linesCount = bill.Line.Count();
            for (int i = 0; i < linesCount; i++)
            {
                Line line0 = new Line();
                line0.AnyIntuitObject = bill.Line[i].AnyIntuitObject;
                line0.DetailType = bill.Line[i].DetailType;
                line0.DetailTypeSpecified = true;
                ((AccountBasedExpenseLineDetail)line0.AnyIntuitObject).AccountRef.Value =
                    ((AccountBasedExpenseLineDetail)bill.Line[i].AnyIntuitObject).AccountRef.Value;
                //
                //var firstItemAccn = ((AccountBasedExpenseLineDetail)selBills[0].Line[0].AnyIntuitObject).AccountRef.Value;
                accountsRefLs.Add(((AccountBasedExpenseLineDetail)bill.Line[i].AnyIntuitObject).AccountRef.Value);
                line0.Amount = bill.Line[i].Amount;
                line0.AmountSpecified = true;
                lineList.Add(line0);
            }

            // add discount line
            Line line = new Line();
            line.Id = "-1";
            line.AnyIntuitObject = bill.Line[0].AnyIntuitObject;
            line.DetailType = bill.Line[0].DetailType;
            line.DetailTypeSpecified = true;
            ((AccountBasedExpenseLineDetail)line.AnyIntuitObject).AccountRef.Value = "132";
            line.Amount = -(Decimal)discount;
            line.AmountSpecified = true;
            line.Description = "desc";
            lineList.Add(line);

            objBill.Line = lineList.ToArray();

            Intuit.Ipp.Data.Bill UpdateEntity = dataService.Update<Intuit.Ipp.Data.Bill>(objBill);

            QueryService<Intuit.Ipp.Data.Bill> querySvc = new QueryService<Intuit.Ipp.Data.Bill>(srvCnxt);
            var qbBills1 = querySvc.ExecuteIdsQuery("SELECT * FROM Bill where id = '" + bill.Id + "'").FirstOrDefault();

            //((AccountBasedExpenseLineDetail)objBill1.Line[0].AnyIntuitObject).AccountRef.Value = "13";
            // restore accounts refs (rewrite)
            for (int i = 0; i < linesCount; i++)
                ((AccountBasedExpenseLineDetail)qbBills1.Line[i].AnyIntuitObject).AccountRef.Value = accountsRefLs[i];

            Intuit.Ipp.Data.Bill UpdateEntity1 = dataService.Update<Intuit.Ipp.Data.Bill>(qbBills1);
        }

        // saves id's of applied discount bills to txt file
        private void SaveAppliedBills(ref List<Intuit.Ipp.Data.Bill> bills)
        {
            StreamWriter writer;
            if (!System.IO.File.Exists(sFlAppBills))
                writer = new StreamWriter(sFlAppBills);
            else
                writer = System.IO.File.AppendText(sFlAppBills);

            foreach (var bill in bills)
            {
                var sToWrite = bill.Id + " " + bill.DocNumber;
                writer.WriteLine(sToWrite);
            }
            writer.Close();
        }

        // reads id's of applied discount bills from txt file
        private void ReadAppliedBills(ref List<string> lsIds)
        {
            using (StreamReader file = new StreamReader(sFlAppBills))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    string[] subs = ln.Split(' ');
                    lsIds.Add(subs[0]);
                }
                file.Close();
            }
        }

        // deletes applied bills (lBillId) from list of bill (bills)
        // returns bills
        private void SubstractAddedBills(ref List<Intuit.Ipp.Data.Bill> bills, List<string> lBillId)
        {
            for (int i = 0; i < lBillId.Count(); i++)
            {
                var foundBill = bills.Where(s => s.Id == lBillId[i]).FirstOrDefault();
                if (foundBill != null)
                    bills.Remove(foundBill);
            }
        }

        [HttpPost]
        [Route("CreatePaymentIntent")]
        public ActionResult CreatePaymentIntent(PaymentIntentCreateRequest request)
        {
            var paymentIntents = new PaymentIntentService();
            var paymentIntentModel = new PaymentIntentCreateOptions
            {
                Amount = CalculateOrderAmount(request),
                Currency = "usd",
            };

            Session["PricingOption"] = request.Items;

            var paymentIntent = paymentIntents.Create(paymentIntentModel);
            return Json(new { clientSecret = paymentIntent.ClientSecret });
        }
        public class PaymentIntentCreateRequest
        {
            [JsonProperty("items")]
            public string Items { get; set; }
        }

        private int CalculateOrderAmount(PaymentIntentCreateRequest items)
        {
            if (items.Items == "001")
                return 0;
            else if (items.Items == "002")
                return 10 * 100;
            else if (items.Items == "003")
                return 15 * 100;
            else if (items.Items == "004")
                return 20 * 100;
            else if (items.Items == "005")
                return 45 * 100;
            else
                return 0;
        }
        public ActionResult ValidateLogin()
        {
            string realmId = Session["realmId"].ToString();
            string authcode = Session["authcode"].ToString();

            var principal = User as ClaimsPrincipal;
            OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(principal.FindFirst("access_token").Value);
            accessToken = principal.FindFirst("access_token").Value;

            // Create a ServiceContext with Auth tokens and realmId
            ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
            serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
            srvCnxt = serviceContext;

            var userInfoResp = auth2Client.GetUserInfoAsync(accessToken).Result;

            var sub = userInfoResp.Claims.Where(x => x.Type == "sub").Select(x => x.Value).SingleOrDefault();
            var email = userInfoResp.Claims.Where(x => x.Type == "email").Select(x => x.Value).SingleOrDefault();

            BillCalend.Model.UserInfo userInfo = dbAccessor.ReturnUserInfo(sub, email, realmId, authcode);

            if (userInfo != null)
            {
                if (userInfo.PricingEndDate > DateTime.Now)
                {
                    return RedirectToAction("ApiCallService", "App");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        [Route("FinalizePayment/{id}")]
        public ActionResult FinalizePayment(int id)
        {
            string pricingoption = "";
            if (id == 1)
            {
                pricingoption = "001";
            }
            else
            {
                pricingoption = Session["PricingOption"].ToString();
            }
            string realmId = Session["realmId"].ToString();
            string authcode = Session["authcode"].ToString();

            var principal = User as ClaimsPrincipal;
            OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(principal.FindFirst("access_token").Value);
            accessToken = principal.FindFirst("access_token").Value;

            // Create a ServiceContext with Auth tokens and realmId
            ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
            serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
            srvCnxt = serviceContext;

            var userInfoResp = auth2Client.GetUserInfoAsync(accessToken).Result;

            var sub = userInfoResp.Claims.Where(x => x.Type == "sub").Select(x => x.Value).SingleOrDefault();
            var email = userInfoResp.Claims.Where(x => x.Type == "email").Select(x => x.Value).SingleOrDefault();

            dbAccessor.CreateUserInfo(sub, email, realmId, authcode, pricingoption);

            return Json(new { response = true }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// QBO API Request
        /// </summary>
        public ActionResult ApiCallService()
        {
            EventManager.calenFormLoaded = true;

            Bills bills = new Bills();

            if (Session["realmId"] != null)
            {
                string realmId = Session["realmId"].ToString();
                string authcode = Session["authcode"].ToString();
                try
                {
                    var principal = User as ClaimsPrincipal;
                    OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(principal.FindFirst("access_token").Value);
                    accessToken = principal.FindFirst("access_token").Value;

                    // Create a ServiceContext with Auth tokens and realmId
                    ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                    serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                    srvCnxt = serviceContext;

                    // Create a QuickBooks QueryService using ServiceContext
                    //QueryService<CompanyInfo> querySvc = new QueryService<CompanyInfo>(serviceContext);
                    //CompanyInfo companyInfo = querySvc.ExecuteIdsQuery("SELECT * FROM CompanyInfo").FirstOrDefault();
                    DataService dataSvc = new DataService(serviceContext);
                    dataService = dataSvc;

                    var userInfoResp = auth2Client.GetUserInfoAsync(accessToken).Result;

                    var sub = userInfoResp.Claims.Where(x => x.Type == "sub").Select(x => x.Value).SingleOrDefault();
                    var email = userInfoResp.Claims.Where(x => x.Type == "email").Select(x => x.Value).SingleOrDefault();

                    BillCalend.Model.UserInfo userInfo = dbAccessor.ReturnUserInfo(sub, email, realmId, authcode);

                    QueryService<Intuit.Ipp.Data.Bill> querySvc = new QueryService<Intuit.Ipp.Data.Bill>(serviceContext);
                    var qbBills = querySvc.ExecuteIdsQuery("SELECT * FROM Bill");

                    // read applied bills before
                    // calc app identification string (email + ClientID)

                    //var emailClientId = GetUserName();
                    //List<string> lsBillId = new List<string>();
                    //dbAccessor.GetAddedBills(ref lsBillId, emailClientId);

                    //delete applied bills
                    List<Bill> selBills = qbBills.ToList();
                    //SubstractAddedBills(ref selBills, lsBillId);

                    if (userInfo.PricingCode == "002")
                        selBills = selBills.OrderByDescending(x => x.MetaData.CreateTime).Take(5).ToList();
                    else if (userInfo.PricingCode == "003")
                        selBills = selBills.OrderByDescending(x => x.MetaData.CreateTime).Take(10).ToList();
                    else if (userInfo.PricingCode == "004")
                        selBills = selBills.OrderByDescending(x => x.MetaData.CreateTime).Take(15).ToList();
                    


                    lCurBills = selBills; // save bills
                    bills.rowsCount = selBills.Count();
                    bills.billRec = selBills;

                    return View(bills);
                }
                catch (Exception ex)
                {
                    bills.errorMes = "QBO API call Failed!" + " Error message: " + ex.Message;
                    return View(bills);
                }
            }
            else
            {
                bills.errorMes = "QBO API call Failed!";
                return View(bills);
            }
        }

        public JsonResult GetAllBills()
        {
            EventManager.calenFormLoaded = true;

            Bills bills = new Bills();

            if (Session["realmId"] != null)
            {
                string realmId = Session["realmId"].ToString();
                string authcode = Session["authcode"].ToString();
                try
                {
                    var principal = User as ClaimsPrincipal;
                    OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(principal.FindFirst("access_token").Value);
                    accessToken = principal.FindFirst("access_token").Value;

                    // Create a ServiceContext with Auth tokens and realmId
                    ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                    serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                    srvCnxt = serviceContext;

                    // Create a QuickBooks QueryService using ServiceContext
                    //QueryService<CompanyInfo> querySvc = new QueryService<CompanyInfo>(serviceContext);
                    //CompanyInfo companyInfo = querySvc.ExecuteIdsQuery("SELECT * FROM CompanyInfo").FirstOrDefault();
                    DataService dataSvc = new DataService(serviceContext);
                    dataService = dataSvc;

                    var userInfoResp = auth2Client.GetUserInfoAsync(accessToken).Result;

                    var sub = userInfoResp.Claims.Where(x => x.Type == "sub").Select(x => x.Value).SingleOrDefault();
                    var email = userInfoResp.Claims.Where(x => x.Type == "email").Select(x => x.Value).SingleOrDefault();

                    BillCalend.Model.UserInfo userInfo = dbAccessor.ReturnUserInfo(sub, email, realmId, authcode);

                    QueryService<Intuit.Ipp.Data.Bill> querySvc = new QueryService<Intuit.Ipp.Data.Bill>(serviceContext);
                    var qbBills = querySvc.ExecuteIdsQuery("SELECT * FROM Bill");

                    // read applied bills before
                    // calc app identification string (email + ClientID)

                    //var emailClientId = GetUserName();
                    //List<string> lsBillId = new List<string>();
                    //dbAccessor.GetAddedBills(ref lsBillId, emailClientId);

                    //delete applied bills
                    List<Bill> selBills = qbBills.ToList();

                    if (userInfo.PricingCode == "002")
                        selBills = selBills.OrderByDescending(x => x.MetaData.CreateTime).Take(5).ToList();
                    else if (userInfo.PricingCode == "003")
                        selBills = selBills.OrderByDescending(x => x.MetaData.CreateTime).Take(10).ToList();
                    else if (userInfo.PricingCode == "004")
                        selBills = selBills.OrderByDescending(x => x.MetaData.CreateTime).Take(15).ToList();

                    QueryService<Intuit.Ipp.Data.Term> queryTerm = new QueryService<Intuit.Ipp.Data.Term>(serviceContext);
                    var termList = queryTerm.ExecuteIdsQuery("SELECT * FROM Term");

                    foreach (var item in selBills)
                    {
                        if (item.SalesTermRef != null)
                        {
                            var term = termList.Where(x => x.Id == item.SalesTermRef.Value);
                            item.SalesTermRef.name = term.FirstOrDefault() == null ? "" : term.FirstOrDefault().Name;
                        }
                    }
                    //SubstractAddedBills(ref selBills, lsBillId);

                    lCurBills = selBills; // save bills
                    bills.rowsCount = selBills.Count();
                    bills.billRec = selBills;

                    return Json(bills, JsonRequestBehavior.AllowGet);
                    //return View(bills);
                }
                catch (Exception ex)
                {
                    bills.errorMes = "QBO API call Failed!" + " Error message: " + ex.Message;
                    return Json(bills, JsonRequestBehavior.AllowGet);
                    //return View(bills);
                }
            }
            else
            {
                bills.errorMes = "QBO API call Failed!";
                return Json(bills, JsonRequestBehavior.AllowGet);
                //return View(bills);
            }
        }

        /// <summary>
        /// Use the Index page of App controller to get all endpoints from discovery url
        /// </summary>
        public ActionResult Error()
        {
            return View("Error");
        }

        /// <summary>
        /// Action that takes redirection from Callback URL
        /// </summary>
        public ActionResult Tokens()
        {
            return View("Tokens");
        }

        public JsonResult AddBill(string passVals)
        {
            SelVals vals = new JavaScriptSerializer().Deserialize<SelVals>(passVals);
            // calc discount days (will be added to txn Data)
            int addDays = 0;
            switch (vals.discOpt)
            {
                case "Immdt":
                case "Net15":
                case "Net30":
                case "Net60":
                    addDays = 0;
                    break;
                case "2P10Net30":
                    addDays = 10;
                    break;
                case "2P15Net30":
                    addDays = 15;
                    break;
                case "10Net30":
                    addDays = 10;
                    break;
                case "15Net30":
                    addDays = 15;
                    break;
                case "60Net60":
                    addDays = 60;
                    break;
                default:
                    break;
            }

            DateTime txnDate = lCurBills[vals.selectedIndex].TxnDate;
            DateTime discDate = txnDate.AddDays(addDays);

            var billNum = lCurBills[vals.selectedIndex].DocNumber;
            // calc app identification string (email + ClientID)
            var emailClientId = GetUserName();

            // Save added bill to DB
            dbAccessor.SaveDbAddedBill(lCurBills[vals.selectedIndex], emailClientId);
            // Save added event
            var eventText = "Invoice #" + billNum;
            dbAccessor.SaveDbAddedEvent(discDate, discDate.AddHours(1), eventText, emailClientId);

            var data = new
            {
                start = discDate.ToString("yyyy-MM-ddTHH:mm:ss"),   // start event
                end = discDate.AddHours(1).ToString("yyyy-MM-ddTHH:mm:ss"),     // end date
                billNum = billNum
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}