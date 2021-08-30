using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Data;
using DayPilot.Web.Mvc.Enums;
using DayPilot.Web.Mvc.Events;
using DayPilot.Web.Mvc.Events.Common;
using DayPilot.Web.Mvc.Events.Month;
using DayPilot.Web.Mvc.Export;
using BillCalend.Util;

namespace BillCalend.Controllers
{
    [HandleError]
    public class MonthController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }       

        public ActionResult Height100Pct()
        {
            return View();
        }       
        
        public ActionResult Notify()
        {
            return View();
        }            
                
        public ActionResult CssContinue()
        {
            return RedirectToActionPermanent("EventCssContinue");
        }            
        
        public ActionResult EventCssContinue()
        {
            return View();
        }            
        
        public ActionResult ThemeTransparent()
        {
            return View();
        }           
        public ActionResult ThemeTraditional()
        {
            return View();
        }        

        public ActionResult ThemeGreen()
        {
            return View();
        }        

        public ActionResult ThemeBlue()
        {
            return View();
        }        

        public ActionResult ThemeWhite()
        {
            return View();
        }        
        
        public ActionResult ActiveAreas()
        {
            return RedirectToActionPermanent("EventActiveAreas");
        }

        public ActionResult EventActiveAreas()
        {
            return View();
        }

        public ActionResult JQuery()
        {
            return View();
        }

        public ActionResult GoogleLike()
        {
            return RedirectToActionPermanent("ThemeGoogleLike");
        }
        
        public ActionResult ThemeGoogleLike()
        {
            return View();
        }
        
        public ActionResult DropDown()
        {
            return View();
        }        

        public ActionResult EventMoveToPosition()
        {
            return View();
        }

        
        public ActionResult EventSelecting()
        {
            return View();
        }
        
        public ActionResult EventDeleting()
        {
            return View();
        }
        
        public ActionResult AutoRefresh()
        {
            return View();
        }

        public ActionResult Message()
        {
            return View();
        }

        public ActionResult Weekends()
        {
            return View();
        }

        public ActionResult NextPrevious()
        {
            return View();
        }     
        
        public ActionResult RecurringEvents()
        {
            return View();
        }

        public ActionResult EventCreating()
        {
            return View();
        }

        public ActionResult EventMoving()
        {
            return View();
        }

        public ActionResult EventResizing()
        {
            return View();
        }

        public ActionResult ContextMenu()
        {
            return RedirectToActionPermanent("EventContextMenu");
        }

        public ActionResult EventContextMenu()
        {
            return View();
        }

        public ActionResult StartEndTime()
        {
            return RedirectToActionPermanent("EventStartEndTime");
        }

        public ActionResult EventStartEndTime()
        {
            return View();
        }

        public ActionResult Today()
        {
            return View();
        }

        public ActionResult EventBubble()
        {
            return View();
        }

        public ActionResult EventDoubleClick()
        {
            return View();
        }

        public ActionResult Weeks()
        {
            return View();
        }

        public ActionResult ImageExportClientSide()
        {
            return View();
        }

        public ActionResult ImageExportServerSide()
        {
            return View();
        }

        public FileStreamResult ImageExportPng()
        {
            MonthExporter exporter = new MonthExporter(new DayPilotMonthConfig());

            exporter.EventsFromEnumerable(new List<EventData>()
            {     
                new EventData { Start = DateTime.Parse("2017-03-05T12:15:00"), End = DateTime.Parse("2017-03-05T14:30:00"), Id = "1", Text = "Event 1"}
            }, new EventFieldMappings
            {
                StartField = "Start",
                EndField = "End",
                IdField = "Id",
                TextField = "Text"
            });
            MemoryStream stream = exporter.Export(ImageFormat.Png);
            return new FileStreamResult(stream, "image/png");
        }

        public class EventData
        {
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public string Id { get; set; }
            public string Text { get; set; }
        }

        public ActionResult ThemeSilver()
        {
            return RedirectToAction("ThemeTraditional");
        }

        public ActionResult Backend()
        {
            return new Dpm().CallBack(this);
        }

        public class Dpm : DayPilotMonth
        {

            protected override void OnBeforeEventRender(BeforeEventRenderArgs e)
            {
                if (Id == "dpm_areas")
                {
                    e.Areas.Add(new Area().Width(17).Bottom(9).Right(2).Top(3).CssClass("event_action_delete").JavaScript("dpm_areas.commandCallBack('delete', {id:e.value() });"));
                    e.Areas.Add(new Area().Width(17).Bottom(9).Right(19).Top(3).CssClass("event_action_menu").ContextMenu("menu"));
                }

                if (e.Recurrent)
                {
                    e.Html += " (R)";
                }

            }
            
            protected override void OnBeforeCellRender(BeforeCellRenderArgs e)
            {
                if (Id == "dpm_today" && e.Start == DateTime.Today)
                {
                    e.HeaderBackColor = "#ff6666";
                    e.BackgroundColor = "#ffaaaa";
                }
                
            }

            protected override void OnEventBubble(EventBubbleArgs e)
            {
                e.BubbleHtml = "Showing event details for: " + e.Id;
            }

            protected override void OnTimeRangeSelected(TimeRangeSelectedArgs e)
            {
                new EventManager(Controller).EventCreate(e.Start, e.End, "Default name", "A");
                Update();
            }

            protected override void OnEventMove(EventMoveArgs e)
            {
                new EventManager(Controller).EventMove(e.Id, e.NewStart, e.NewEnd);
                if (Id == "dpm_position")
                {
                    UpdateWithMessage("Moved to position: " + e.Position);
                }
                else
                {
                    UpdateWithMessage("Event moved.");
                }
            }

            protected override void OnEventClick(EventClickArgs e)
            {
                UpdateWithMessage("Event clicked: " + e.Text);
            }

            protected override void OnEventResize(EventResizeArgs e)
            {
                new EventManager(Controller).EventMove(e.Id, e.NewStart, e.NewEnd);
                Update();
            }

            protected override void OnEventMenuClick(EventMenuClickArgs e)
            {
                switch (e.Command)
                {
                    case "Delete":
                        new EventManager(Controller).EventDelete(e.Id);
                        Update();
                        break;
                }
            }

            protected override void OnEventDelete(EventDeleteArgs e)
            {
                new EventManager(Controller).EventDelete(e.Id);
                Update();
            }

            protected override void OnCommand(CommandArgs e)
            {
                switch (e.Command)
                {
                    case "navigate":
                        DateTime start = (DateTime)e.Data["start"];
                        StartDate = start;
                        Update(CallBackUpdateType.Full);
                        break;
                    case "selected":
                        if (SelectedEvents.Count > 0)
                        {
                            EventInfo ei = SelectedEvents[0];
                            SelectedEvents.RemoveAt(0);
                            UpdateWithMessage("Event removed from selection: " + ei.Text);
                        }

                        break;
                    case "weekend":
                        ShowWeekend = (string)e.Data["status"] == "yes";
                        Update(CallBackUpdateType.Full);
                        break;
                    case "refresh":
                        UpdateWithMessage("Refreshed", CallBackUpdateType.Full);
                        break;
                    case "previous":
                        StartDate = StartDate.AddMonths(-1);
                        Update(CallBackUpdateType.Full);
                        break;
                    case "today":
                        StartDate = DateTime.Today;
                        Update(CallBackUpdateType.Full);
                        break;
                    case "next":
                        StartDate = StartDate.AddMonths(1);
                        Update(CallBackUpdateType.Full);
                        break;
                    case "delete":
                        string id = (string) e.Data["id"];
                        new EventManager(Controller).EventDelete(id);
                        Update(CallBackUpdateType.EventsOnly);
                        break;
                }

            }

            protected override void OnInit(InitArgs initArgs)
            {
                Update(CallBackUpdateType.Full); 
            }

            protected override void OnBeforeHeaderRender(BeforeHeaderRenderArgs e)
            {
            }

            protected override void OnFinish()
            {
                // only load the data if an update was requested by an Update() call
                if (UpdateType == CallBackUpdateType.None)
                {
                    return;
                }

                // this select is a really bad example, no where clause
                if (Id == "dpm_recurring")
                {
                    Events = new EventManager(Controller, "recurring").Data.AsEnumerable();
                    DataRecurrenceField = "recurrence";
                }
                else
                {
                    Events = new EventManager(Controller).Data.AsEnumerable();
                }


                //no need to override the default field names
                DataStartField = "start";
                DataEndField = "end";
                DataTextField = "text";
                DataIdField = "id";
            }

        }


    }
}
