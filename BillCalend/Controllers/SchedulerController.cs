using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Data;
using DayPilot.Web.Mvc.Enums;
using DayPilot.Web.Mvc.Events.Scheduler;
using DayPilot.Web.Mvc.Recurrence;
using BillCalend.Util;
//using TutorialCS;

namespace BillCalend.Controllers
{
    public class SchedulerController : Controller
    {
        //
        // GET: /Scheduler/

        public ActionResult Backend()
        {
            return new Dps().CallBack(this);
        }

        class Dps : DayPilotScheduler
        {
            protected override void OnInit(InitArgs e)
            {
                LoadResources();

                UpdateWithMessage("Welcome!", CallBackUpdateType.Full);
            }

            private void LoadResources() {
                Resources.Clear();
                foreach (DataRow r in new EventManager().GetResources().Rows)
                {
                    Resource res = new Resource((string)r["name"], Convert.ToString(r["id"]));
                    Resources.Add(res);
                }
            }

            protected override void OnEventResize(EventResizeArgs e)
            {
                if (e.Recurrent && !e.RecurrentException)
                {
                    new EventManager().EventCreateException(e.NewStart, e.NewEnd, e.Text, e.Resource, RecurrenceRule.EncodeExceptionModified(e.RecurrentMasterId, e.OldStart));
                    UpdateWithMessage("Recurrence exception was created.");
                }
                else
                {
                    new EventManager().EventMove(e.Id, e.NewStart, e.NewEnd, e.Resource);
                    UpdateWithMessage("The event was resized.");
                }
            }

            protected override void OnEventMove(EventMoveArgs e)
            {
                if (e.Recurrent && !e.RecurrentException)
                {
                    new EventManager().EventCreateException(e.NewStart, e.NewEnd, e.Text, e.NewResource, RecurrenceRule.EncodeExceptionModified(e.RecurrentMasterId, e.OldStart));
                    UpdateWithMessage("Recurrence exception was created.");
                }
                else
                {
                    new EventManager().EventMove(e.Id, e.NewStart, e.NewEnd, e.NewResource);
                    UpdateWithMessage("The event was moved.");
                }
                
            }

            protected override void OnCommand(CommandArgs e)
            {
                switch (e.Command)
                {
                    case "refresh":
                        Update();
                        break;
                }
            }


            protected override void OnBeforeEventRender(BeforeEventRenderArgs e)
            {
                if (e.Recurrent)
                {
                    if (e.RecurrentException)
                    {
                        e.Areas.Add(new Area().Right(5).Top(5).Visible().CssClass("area_recurring_ex"));
                    }
                    else
                    {
                        e.Areas.Add(new Area().Right(5).Top(5).Visible().CssClass("area_recurring"));
                    }
                }
            }

            protected override void OnFinish()
            {
                if (UpdateType == CallBackUpdateType.None)
                {
                    return;
                }

                Events = new EventManager().FilteredData(StartDate, StartDate.AddDays(Days)).AsEnumerable();

                DataIdField = "id";
                DataTextField = "name";
                DataStartField = "eventstart";
                DataEndField = "eventend";
                DataResourceField = "resource";
                DataRecurrenceField = "recurrence";
            }
        }

    }
}
