using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BillCalend.Controllers
{
    public class MessgController : Controller
    {
        static public BillCalend.Model.Mess mess = new Model.Mess();

        // GET: Messg
        public ActionResult Index()
        {
            return View(mess);
        }

    }
}