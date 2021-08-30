using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DayPilot.Web.Mvc.Recurrence;

namespace BillCalend.Test
{
    /// <summary>
    /// Summary description for EventManager
    /// </summary>
    public class EventManager
    {
        private Controller controller;
        private string key;

        public EventManager()
        {

        }

        public EventManager(Controller controller, string key)
        {
            this.controller = controller;
            this.key = key;

            if (this.controller.Session[key] == null)
            {
                switch (key)
                {
                    case "dps_recurring":
                        this.controller.Session[key] = generateDataRecurring();
                        break;
                    case "dps_timesheet":
                        this.controller.Session[key] = generateDataTimesheet();
                        break;
                    case "dps_milestones":
                        this.controller.Session[key] = generateDataMilestone();
                        break;
                    default:
                        this.controller.Session[key] = generateData();
                        break;
                }
            }
        }

        public EventManager(Controller controller) : this(controller, "default")
        {
        }

        private DataTable generateData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("text", typeof(string));
            dt.Columns.Add("start", typeof(DateTime));
            dt.Columns.Add("end", typeof(DateTime));
            dt.Columns.Add("resource", typeof(string));
            dt.Columns.Add("color", typeof(string));
            dt.Columns.Add("allday", typeof(bool));

            dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };

            DataRow dr;

            dr = dt.NewRow();
            dr["id"] = 1;
            dr["start"] = Convert.ToDateTime("00:01").AddDays(1);
            dr["end"] = Convert.ToDateTime("00:01").AddDays(1);
            dr["text"] = "Event 1";
            dr["resource"] = "A";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 2;
            dr["start"] = Convert.ToDateTime("16:00").AddDays(1);
            dr["end"] = Convert.ToDateTime("17:00").AddDays(1);
            dr["text"] = "Event 2";
            dr["resource"] = "A";
            dr["color"] = "green";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 3;
            dr["start"] = Convert.ToDateTime("14:15").AddDays(2);
            dr["end"] = Convert.ToDateTime("18:45").AddDays(2);
            dr["text"] = "Event 3";
            dr["resource"] = "A";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 4;
            dr["start"] = Convert.ToDateTime("16:30").AddDays(1);
            dr["end"] = Convert.ToDateTime("17:30").AddDays(1);
            dr["text"] = "Sales Dept. Meeting Once Again";
            dr["resource"] = "B";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 5;
            dr["start"] = Convert.ToDateTime("8:00").AddDays(1);
            dr["end"] = Convert.ToDateTime("9:00").AddDays(1);
            dr["text"] = "Event 4";
            dr["resource"] = "B";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 6;
            dr["start"] = Convert.ToDateTime("14:00").AddDays(1);
            dr["end"] = Convert.ToDateTime("20:00").AddDays(1);
            dr["text"] = "Event 6";
            dr["resource"] = "C";
            dt.Rows.Add(dr);


            dr = dt.NewRow();
            dr["id"] = 7;
            dr["start"] = Convert.ToDateTime("11:00").AddDays(1);
            dr["end"] = Convert.ToDateTime("13:14").AddDays(1);
            dr["text"] = "Unicode test: 公曆 (requires Unicode fonts on the client side)";
            dr["color"] = "red";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 8;
            dr["start"] = Convert.ToDateTime("13:14").AddDays(-1);
            dr["end"] = Convert.ToDateTime("14:05").AddDays(-1);
            dr["text"] = "Event 8";
            dr["resource"] = "C";
            dt.Rows.Add(dr);


            dr = dt.NewRow();
            dr["id"] = 9;
            dr["start"] = Convert.ToDateTime("13:14").AddDays(7);
            dr["end"] = Convert.ToDateTime("14:05").AddDays(7);
            dr["text"] = "Event 9";
            dr["resource"] = "C";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 10;
            dr["start"] = Convert.ToDateTime("13:14").AddDays(-7);
            dr["end"] = Convert.ToDateTime("14:05").AddDays(-7);
            dr["text"] = "Event 10";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 11;
            dr["start"] = Convert.ToDateTime("00:00").AddDays(8);
            dr["end"] = Convert.ToDateTime("00:00").AddDays(15);
            dr["text"] = "Event 11";
            dr["resource"] = "D";
            dr["allday"] = true;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 12;
            dr["start"] = Convert.ToDateTime("00:00").AddDays(-2);
            dr["end"] = Convert.ToDateTime("00:00").AddDays(-1);
            dr["text"] = "Event 12";
            dr["resource"] = "D";
            dr["allday"] = true;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 13;
            dr["start"] = DateTime.Now.AddDays(-7);
            dr["end"] = DateTime.Now.AddDays(14);
            dr["text"] = "Event 13";
            dr["resource"] = "B";
            dr["allday"] = true;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 14;
            dr["start"] = Convert.ToDateTime("7:45:00").AddDays(1);
            dr["end"] = Convert.ToDateTime("8:30:00").AddDays(1);
            dr["text"] = "Event 14";
            dr["resource"] = "D";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 16;
            dr["start"] = Convert.ToDateTime("8:30:00").AddDays(3);
            dr["end"] = Convert.ToDateTime("9:00:00").AddDays(3);
            dr["text"] = "Event 16";
            dr["resource"] = "D";
            dt.Rows.Add(dr);


            dr = dt.NewRow();
            dr["id"] = 17;
            dr["start"] = Convert.ToDateTime("8:00:00").AddDays(1);
            dr["end"] = Convert.ToDateTime("8:00:01").AddDays(1);
            dr["text"] = "Event 17";
            dr["resource"] = "D";
            dt.Rows.Add(dr);

            //DataTable dt = new DataTable();
            //DateTime dts = DateTime.Now;
            ////dt = FilteredData(dts, dts);

            //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [event]", ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString);
            ////da.SelectCommand.Parameters.AddWithValue("start", start);
            ////da.SelectCommand.Parameters.AddWithValue("end", end);

            //da.Fill(dt);

            return dt;
        }

        private DataTable generateDataRecurring()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("text", typeof(string));
            dt.Columns.Add("start", typeof(DateTime));
            dt.Columns.Add("end", typeof(DateTime));
            dt.Columns.Add("resource", typeof(string));
            dt.Columns.Add("color", typeof(string));
            dt.Columns.Add("allday", typeof(bool));
            dt.Columns.Add("recurrence", typeof(string));

            dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };

            DataRow dr;

            dr = dt.NewRow();
            dr["id"] = 1;
            dr["start"] = Convert.ToDateTime("10:00");
            dr["end"] = Convert.ToDateTime("11:30");
            dr["text"] = "Daily";
            dr["resource"] = "A";
            dr["recurrence"] = RecurrenceRule.FromDateTime(Convert.ToString(dr["id"]), Convert.ToDateTime(dr["start"])).Daily().Times(15).Encode();
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 2;
            dr["start"] = Convert.ToDateTime("13:00");
            dr["end"] = Convert.ToDateTime("18:00");
            dr["text"] = "Weekly";
            dr["resource"] = "B";
            dr["color"] = "green";
            dr["recurrence"] = RecurrenceRule.FromDateTime(Convert.ToString(dr["id"]), Convert.ToDateTime(dr["start"])).Weekly().Times(5).Encode();
            dt.Rows.Add(dr);


            return dt;
        }

        private DataTable generateDataTimesheet()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("text", typeof(string));
            dt.Columns.Add("start", typeof(DateTime));
            dt.Columns.Add("end", typeof(DateTime));

            dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };

            DataRow dr;

            dr = dt.NewRow();
            dr["id"] = 1;
            dr["start"] = Convert.ToDateTime("09:00");
            dr["end"] = Convert.ToDateTime("12:00");
            dr["text"] = "Event 1";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 2;
            dr["start"] = Convert.ToDateTime("10:00").AddDays(2);
            dr["end"] = Convert.ToDateTime("14:00").AddDays(2);
            dr["text"] = "Event 2";
            dt.Rows.Add(dr);


            return dt;
        }

        private DataTable generateDataMilestone()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("text", typeof(string));
            dt.Columns.Add("start", typeof(DateTime));
            dt.Columns.Add("end", typeof(DateTime));
            dt.Columns.Add("resource", typeof(string));
            dt.Columns.Add("color", typeof(string));
            dt.Columns.Add("milestone", typeof(bool));

            dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };

            DataRow dr;

            dr = dt.NewRow();
            dr["id"] = 1;
            dr["start"] = Convert.ToDateTime("00:00").AddDays(1);
            dr["end"] = Convert.ToDateTime("00:00").AddDays(3);
            dr["text"] = "Event 1";
            dr["resource"] = "A";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 3;
            dr["start"] = Convert.ToDateTime("00:00").AddDays(2);
            dr["end"] = Convert.ToDateTime("00:00").AddDays(4);
            dr["text"] = "Event 2";
            dr["resource"] = "B";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 5;
            dr["start"] = Convert.ToDateTime("00:00").AddDays(1);
            dr["end"] = Convert.ToDateTime("00:00").AddDays(5);
            dr["text"] = "Event 3";
            dr["resource"] = "C";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["id"] = 11;
            dr["start"] = Convert.ToDateTime("00:00").AddDays(8);
            dr["text"] = "Milestone 1";
            dr["resource"] = "D";
            dr["milestone"] = true;
            dt.Rows.Add(dr);

            return dt;
        }

        public DataTable Data
        {
            get { return (DataTable)controller.Session[key]; }
        }

        public DataTable FilteredData(DateTime start, DateTime end)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [event] WHERE NOT (([eventend] <= @start) OR ([eventstart] >= @end))", ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("start", start);
            da.SelectCommand.Parameters.AddWithValue("end", end);

            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }

        public void EventEdit(string id, string name, DateTime start, DateTime end, string resource, string recurrence)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE [event] SET [name] = @name, [eventstart] = @start, [eventend] = @end, [resource] = @resource, [recurrence] = @recurrence WHERE [id] = @id", con);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("start", start);
                cmd.Parameters.AddWithValue("end", end);
                cmd.Parameters.AddWithValue("resource", resource);
                cmd.Parameters.AddWithValue("recurrence", (object) recurrence ?? DBNull.Value);
                cmd.ExecuteNonQuery();

            }
        }

        public void EventEdit(string id, string name, DateTime start, DateTime end, string resource)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE [event] SET [name] = @name, [eventstart] = @start, [eventend] = @end, [resource] = @resource WHERE [id] = @id", con);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("start", start);
                cmd.Parameters.AddWithValue("end", end);
                cmd.Parameters.AddWithValue("resource", resource);
                cmd.ExecuteNonQuery();

            }
        }

        public DataTable GetResources()
        {
            return GetResources("name");
        }

        public DataTable GetResources(string orderBy)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [resource]", ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dt.DefaultView.Sort = orderBy;

            return dt.DefaultView.ToTable();
            
        }

        public void EventMove(string id, DateTime start, DateTime end)
        {
            DataRow dr = Data.Rows.Find(id);
            if (dr != null)
            {
                dr["start"] = start;
                dr["end"] = end;
                Data.AcceptChanges();
            }
            else // external drag&drop
            {

            }
        }

        public void EventMove(string id, DateTime start, DateTime end, string resource)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE [event] SET [eventstart] = @start, [eventend] = @end, [resource] = @resource WHERE [id] = @id", con);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("start", start);
                cmd.Parameters.AddWithValue("end", end);
                cmd.Parameters.AddWithValue("resource", resource);
                cmd.ExecuteNonQuery();

            }
        }

        public Event Get(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return null;
            }

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [event] WHERE id = @id", ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("id", id);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                return new Event
                           {
                               Id = id,
                               Text = (string) dr["name"],
                               Start = (DateTime) dr["eventstart"],
                               End = (DateTime) dr["eventend"],
                               Resource = new SelectList(ResourceSelectList(), "Value", "Text", dr["resource"]),
                               Recurrence = dr.IsNull("recurrence") ? null : (string) dr["recurrence"]
                           };
            }
            return null;
        }

        public IEnumerable<SelectListItem> ResourceSelectList()
        {
            return
                GetResources().AsEnumerable().Select(u => new SelectListItem
                                                              {
                                                                  Value = Convert.ToString(u.Field<int>("id")),
                                                                  Text = u.Field<string>("name")
                                                              });
        }

        internal void EventCreate(DateTime start, DateTime end, string text, string resource, string recurrenceJson)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO [event] (eventstart, eventend, name, resource) VALUES (@start, @end, @name, @resource); ", con);  // SELECT SCOPE_IDENTITY();
                cmd.Parameters.AddWithValue("start", start);
                cmd.Parameters.AddWithValue("end", end);
                cmd.Parameters.AddWithValue("name", text);
                cmd.Parameters.AddWithValue("resource", resource);
                //cmd.Parameters.AddWithValue("recurrence", recurrence);
                cmd.ExecuteScalar();

                cmd = new SqlCommand("select @@identity;", con);
                int id = Convert.ToInt32(cmd.ExecuteScalar());

                RecurrenceRule rule = RecurrenceRule.FromJson(id.ToString(), start, recurrenceJson);
                string recurrenceString = rule.Encode();
                if (!String.IsNullOrEmpty(recurrenceString))
                {
                    cmd = new SqlCommand("update [event] set [recurrence] = @recurrence where [id] = @id", con);
                    cmd.Parameters.AddWithValue("recurrence", rule.Encode());
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal void EventCreate(DateTime start, DateTime end, string text, string resource)
        {
            EventCreate(start, end, text, resource, Guid.NewGuid().ToString());
        }

        public class Event
        {
            public string Id { get; set; }
            public string Text { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public SelectList Resource { get; set; }
            public string Recurrence { get; set; }
        }

        public void EventDelete(string id)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM [event] WHERE id = @id", con);
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public void EventCreateException(DateTime start, DateTime end, string text, string resource, string encodedRecurrence)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO [event] (eventstart, eventend, name, resource, recurrence) VALUES (@start, @end, @name, @resource, @recurrence); ", con);  // SELECT SCOPE_IDENTITY();
                cmd.Parameters.AddWithValue("start", start);
                cmd.Parameters.AddWithValue("end", end);
                cmd.Parameters.AddWithValue("name", text);
                cmd.Parameters.AddWithValue("resource", resource);
                cmd.Parameters.AddWithValue("recurrence", encodedRecurrence);
                cmd.ExecuteScalar();

            }

        }
    }

}