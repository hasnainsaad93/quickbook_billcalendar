using Stripe;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BillCalend
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // This is your real test secret API key.
            StripeConfiguration.ApiKey = "sk_test_51JHY9HDYEkAcuraiKoi4gXujfbymA08cHJ3ayYazYfsD0gucvNwJaOJZCsS85ldcgYPNJdMrRcwttnrwKaOZHVJ000KEmX5gjn";
            //StripeConfiguration.ApiKey = "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";
            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_EndRequest(object sender, System.EventArgs e)
        {
            // If the user is not authorised to see this page or access this function, send them to the error page.
            if (Response.StatusCode == 401)
            {
                Response.ClearContent();
                Response.RedirectToRoute("ErrorHandler", (RouteTable.Routes["ErrorHandler"] as Route).Defaults);
            }
        }

    }
}
