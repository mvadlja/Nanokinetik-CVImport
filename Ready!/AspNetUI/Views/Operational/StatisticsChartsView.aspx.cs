using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using GEM2Common;
using CommonTypes;
using Kmis.Model;
using System.Drawing;
using System.Globalization ;
using Ready.Model;
using System.Data;
using System.Threading;

using System.Web.UI.DataVisualization.Charting;

namespace AspNetUI.Views.Business
{
    public partial class StatisticsChartsView : FormHolder
    {
        MasterMain m = null;
        // View initialization
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            m = (MasterMain)Page.Master;
            this.ctlTimePeriod.InputValueChanged += new EventHandler<ValueChangedEventArgs>(ctlTimePeriod_InputValueChanged);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (Request.QueryString["chart"] != null)
            {
                switch (Request.QueryString["chart"].ToString())
                {
                    case "receivedMessages": MakeReceivedChart(); break;
                    case "sentMessages": MakeSentChart(); break;
                    case "ackByType": MakeAckChart(); break;
                    case "ackByTypeTime": MakeACKByTimeChart(); break;
                    case "successStatus": MakeStatusChart(); break;
                    case "messageTimeStatus": TimesPerType(); break;
                }
            }
            base.OnLoad(e);
        }
        // Context menu click handler for this form
        public override void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";

            switch (e.EventType)
            {
                // Back
                case ContextMenuEventTypes.Back:

                    Response.Redirect("~/Views/Operational/xEVPRMStats.aspx");

                    break;
                // New entry
                case ContextMenuEventTypes.New:
                    break;

                // Save current entity 
                case ContextMenuEventTypes.Save:
                    break;
                // Cancel operation
                case ContextMenuEventTypes.Cancel:

                    Response.Redirect("~/Views/Business/RemindersView.aspx");

                    break;
                default:

                    break;
            }
        }

        // Displays correct form (from MasterPage.ViewStateController.SelectedForm property)
        public override void ShowSelectedForm()
        {
            if (Request.QueryString["chart"] != null)
            {
                ctlTimeChartType.Visible = false;
                switch (Request.QueryString["chart"].ToString())
                {
                    case "sentMessages":
                        BindTimeFilter(true);
                        break;
                    case "receivedMessages":
                        BindTimeFilter(true);
                        break;
                    case "ackByType":
                        BindTimeFilter(false);
                        break;
                    case "ackByTypeTime":
                        BindTimeFilter(true);
                        break;
                    case "successStatus":
                        BindTimeFilter(false);
                        break;
                    case "messageTimeStatus":
                        BindTimeFilter(false);
                        BindTimeStatsForm();
                        break;
                }
            }

            MasterPage.ContextMenu.SetContextMenuItemsVisible(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.Back, "Back") });

        }


        private void MakeAckChart()
        {

            DateTime startDate;
            DateTime endDate;
            GetTimePeriod(out startDate, out endDate);
            
            List<Recieved_message_PK> messages = (new Recieved_message_PKDAL()).GetMessagesByTimeSpan(startDate, endDate);
            Dictionary<String, int> dataset = new Dictionary<String, int>();
            dataset.Add("ACK01",0);
            dataset.Add("ACK02",0);
            dataset.Add("ACK03",0);
            foreach (Recieved_message_PK message in messages)
            {
                if (message.status == null) continue;
                if (message.msg_type == null || message.msg_type == 1) continue;
                switch (message.status.Value) {
                    case 1: dataset["ACK01"]++; break;
                    case 2: dataset["ACK02"]++; break;
                    case 3: dataset["ACK03"]++; break;
                }

            }

            if (dataset["ACK01"] == 0 && dataset["ACK02"] == 0 && dataset["ACK03"] == 0)
            {
                DisplayChartMessage("There is no data to display for selected time period.");
                return;
            }

            foreach (KeyValuePair<String, int> kvp in dataset)
            {
                ctlChart.Series["mainSeries"].Points.AddXY(kvp.Key, kvp.Value);
            }
         
            ctlChart.Titles.Add("Messages distribution by ACK status");
            ctlChart.Font.Size = 14;
            ctlChart.Series["mainSeries"]["PieLabelStyle"] = "Outside";
            ctlChart.Series["mainSeries"].Label = "#VALX - #VAL (#PERCENT)";
            ctlChart.Series[0].ShadowColor = System.Drawing.Color.FromArgb(120, 125, 182, 240);
            ctlChart.Series[0].ShadowOffset = 10;
            ctlChart.ChartAreas[0].Area3DStyle.Enable3D = true;

            ctlChart.ChartAreas[0].Area3DStyle.Rotation = 120;
            ctlChart.ChartAreas[0].BackColor = Color.FromArgb(255, 237, 237, 237);
            ctlChart.Series["mainSeries"].ChartType = SeriesChartType.Pie;
            ctlChart.Series["mainSeries"].ShadowColor = System.Drawing.Color.Aquamarine;
            ctlChart.Series["mainSeries"].ShadowOffset = 5;
            ctlChart.Series["mainSeries"].ChartArea = "mainChartArea";

            ctlChart.ApplyPaletteColors();
            foreach (var series in ctlChart.Series)
            {
                foreach (var point in series.Points)
                {
                    point.Color = Color.FromArgb(220, point.Color);
                }
            }
        }

        private void MakeSentChart()
        {
            DateTime startDate;
            DateTime endDate;
            GetTimePeriod(out startDate, out endDate);

            List<Sent_message_PK> messages = (new Sent_message_PKDAL()).GetMessagesByTimeSpan(startDate,endDate);

            if (messages.Count < 1)
            {
                DisplayChartMessage("There is no data to display for selected time period.");
                return;
            }

            Dictionary<DateTime, int> dataset = new Dictionary<DateTime, int>();
            foreach (Sent_message_PK message in messages)
            {
                if (message.sent_time == null) continue;
                if (!dataset.ContainsKey(message.sent_time.Value)) dataset.Add(message.sent_time.Value, 0);
                dataset[message.sent_time.Value]++;
            }

            DataTable data = this.GenerateDataSource(dataset, startDate, endDate, ctlGroupBy.ControlValue.ToString());
            if (data.Rows.Count > 50)
            {
                DisplayChartMessage("Groupping period is to small for selected date range. Please select longer groupping period.");
                return;
            }
            ctlChart.Series["mainSeries"].Points.DataBind(data.Rows, "xValue", "yValue", null);
           

            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("hr-HR");
            ctlChart.Titles.Clear();
            ctlChart.Titles.Add("Number of sent messages between "+startDate.Date.ToShortDateString() +" and "+ endDate.AddDays(-1).Date.ToShortDateString());
            Thread.CurrentThread.CurrentCulture = originalCulture;
            
            ctlChart.Font.Size = 14;
            ctlChart.Series[0].ShadowColor = System.Drawing.Color.FromArgb(120, 125, 182, 240);
            ctlChart.Series[0].ShadowOffset = 10;
            ctlChart.ChartAreas[0].Area3DStyle.Enable3D = true;

            ctlChart.ChartAreas[0].Area3DStyle.Rotation = 10;
            ctlChart.Series["mainSeries"].ChartType = SeriesChartType.Column;
            ctlChart.ChartAreas[0].AxisX.Interval = 1;

            ctlChart.ApplyPaletteColors();
            foreach (var series in ctlChart.Series)
            {
                foreach (var point in series.Points)
                {
                    point.Color = Color.FromArgb(220, point.Color);
                }
            }
        }

        private void MakeReceivedChart()
        {
            DateTime startDate;
            DateTime endDate;
            GetTimePeriod(out startDate, out endDate);
           
            List<Recieved_message_PK> messages = (new Recieved_message_PKDAL()).GetMessagesByTimeSpan(startDate, endDate);
            Dictionary<DateTime, int> dataset = new Dictionary<DateTime, int>();
            if (messages.Count < 1)
            {
                DisplayChartMessage("There is no data to display for selected time period.");
                return;
            }
            foreach (Recieved_message_PK message in messages)
            {
                if (message.received_time == null) continue;
                if (!dataset.ContainsKey(message.received_time.Value)) dataset.Add(message.received_time.Value, 0);
                dataset[message.received_time.Value]++;
            }

           
            DataTable data = this.GenerateDataSource(dataset, startDate, endDate, ctlGroupBy.ControlValue.ToString());
            if (data.Rows.Count > 50)
            {
                DisplayChartMessage("Groupping period is to small for selected date range. Please select longer groupping period.");
                return;
            }
            ctlChart.Series["mainSeries"].Points.DataBind(data.Rows, "xValue", "yValue", null);

            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("hr-HR");
            ctlChart.Titles.Clear();
            ctlChart.Titles.Add("Number of received messages between " + startDate.Date.ToShortDateString() + " and " + endDate.AddDays(-1).Date.ToShortDateString());
            Thread.CurrentThread.CurrentCulture = originalCulture;

            ctlChart.Font.Size = 14;
            ctlChart.Series[0].ShadowColor = System.Drawing.Color.FromArgb(120, 125, 182, 240);
            ctlChart.Series[0].ShadowOffset = 10;
            ctlChart.ChartAreas[0].Area3DStyle.Enable3D = true;

            ctlChart.ChartAreas[0].Area3DStyle.Rotation = 10;
            ctlChart.Series["mainSeries"].ChartType = SeriesChartType.Column;
            ctlChart.ChartAreas[0].AxisX.Interval = 1;

            ctlChart.ApplyPaletteColors();
            foreach (var series in ctlChart.Series)
            {
                foreach (var point in series.Points)
                {
                    point.Color = Color.FromArgb(220, point.Color);
                }
            }
        }

        private void MakeACKByTimeChart()
        {
            DateTime startDate;
            DateTime endDate;
            GetTimePeriod(out startDate, out endDate);

            List<Recieved_message_PK> messages = ( new Recieved_message_PKDAL()).GetMessagesByTimeSpan(startDate, endDate);
            Dictionary<String, Dictionary<DateTime, int>> dataset = new Dictionary<String, Dictionary<DateTime, int>>();
            dataset.Add("ACK01", new Dictionary<DateTime, int>());
            dataset.Add("ACK02", new Dictionary<DateTime, int>());
            bool hasData = false;
            dataset.Add("ACK03", new Dictionary<DateTime, int>());
            foreach (Recieved_message_PK message in messages)
            {
                if (message.status == null) continue;
                if (message.msg_type == null || message.msg_type == 1) continue;
                if (message.received_time == null) continue;
               
                switch (message.status.Value)
                {
                    case 1:
                        if (!dataset["ACK01"].ContainsKey(message.received_time.Value)) dataset["ACK01"].Add(message.received_time.Value, 0);
                        dataset["ACK01"][message.received_time.Value]++;
                        hasData = true;
                        break;
                    case 2:
                        if (!dataset["ACK02"].ContainsKey(message.received_time.Value)) dataset["ACK02"].Add(message.received_time.Value, 0);
                        dataset["ACK02"][message.received_time.Value]++;
                        hasData = true;
                        break;
                    case 3:
                        if (!dataset["ACK03"].ContainsKey(message.received_time.Value)) dataset["ACK03"].Add(message.received_time.Value, 0);
                        dataset["ACK03"][message.received_time.Value]++;
                        hasData = true;
                        break;
                }

            }

            if (!hasData)
            {
                DisplayChartMessage("There is no data to display for selected time period.");
                return;
            }

            DataTable dataACK01 = this.GenerateDataSource(dataset["ACK01"], startDate, endDate, ctlGroupBy.ControlValue.ToString());
            DataTable dataACK02 = this.GenerateDataSource(dataset["ACK02"], startDate, endDate, ctlGroupBy.ControlValue.ToString());
            DataTable dataACK03 = this.GenerateDataSource(dataset["ACK03"], startDate, endDate, ctlGroupBy.ControlValue.ToString());
            
            if (dataACK01.Rows.Count > 50)
            {
                DisplayChartMessage("Groupping period is to small for selected date range. Please select longer groupping period.");
                return;
            }


            ctlChart.ChartAreas[0].Area3DStyle.Enable3D = true;
            ctlChart.ChartAreas[0].Area3DStyle.Rotation = 20;
            ctlChart.Series.Clear();
            ctlChart.Series.Add("ACK01");
            ctlChart.Series.Add("ACK02");
            ctlChart.Series.Add("ACK03");
            ctlChart.Series["ACK01"].ChartType = SeriesChartType.StackedColumn;
            ctlChart.Series["ACK02"].ChartType = SeriesChartType.StackedColumn;
            ctlChart.Series["ACK03"].ChartType = SeriesChartType.StackedColumn;

            ctlChart.Series["ACK01"].Points.DataBind(dataACK01.Rows, "xValue", "yValue", null);
            ctlChart.Series["ACK02"].Points.DataBind(dataACK02.Rows, "xValue", "yValue", null);
            ctlChart.Series["ACK03"].Points.DataBind(dataACK03.Rows, "xValue", "yValue", null);

            ctlChart.Legends.Add(new Legend(""));
            ctlChart.ChartAreas[0].AxisX.Interval = 1;

            ctlChart.Series["ACK03"].Color = Color.FromArgb(220, 210, 49, 59);
            ctlChart.Series["ACK02"].Color = Color.FromArgb(220, 150, 199, 197);

            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture; 
            Thread.CurrentThread.CurrentCulture = new CultureInfo("hr-HR");
            ctlChart.Titles.Clear();
            ctlChart.Titles.Add("ACK status distribution for messages received between " + startDate.Date.ToShortDateString() + " and " + endDate.AddDays(-1).Date.ToShortDateString());
            Thread.CurrentThread.CurrentCulture = originalCulture;

            ctlChart.ApplyPaletteColors();
            foreach (var series in ctlChart.Series)
            {
                foreach (var point in series.Points)
                {
                    point.Color = Color.FromArgb(230, point.Color);
                }
            }
        }

        private void MakeStatusChart()
        {

            DateTime startDate;
            DateTime endDate;
            GetTimePeriod(out startDate, out endDate);

            List<Recieved_message_PK> messages = (new Recieved_message_PKDAL()).GetMessagesByTimeSpan(startDate, endDate);
            Dictionary<String, int> dataset = new Dictionary<String, int>();
            dataset.Add("Successfull", 0);
            dataset.Add("Failed", 0);

            foreach (Recieved_message_PK message in messages)
            {
                if (!message.is_successfully_processed.HasValue) continue;
                if (message.msg_type == null ) continue;
                switch (message.is_successfully_processed.Value)
                {
                    case true: dataset["Successfull"]++; break;
                    case false: dataset["Failed"]++; break;
                }
            }

            if (dataset["Successfull"] == 0 && dataset["Failed"] == 0)
            {
                DisplayChartMessage("There is no data to display for selected time period.");
                return;
            }

            foreach (KeyValuePair<String, int> kvp in dataset)
            {
                ctlChart.Series["mainSeries"].Points.AddXY(kvp.Key, kvp.Value);
            }

            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("hr-HR");
            ctlChart.Titles.Clear();
            ctlChart.Titles.Add("Message processed status for messages recieved between " + startDate.Date.ToShortDateString() + " and " + endDate.AddDays(-1).Date.ToShortDateString());
            Thread.CurrentThread.CurrentCulture = originalCulture;

            ctlChart.Font.Size = 14;
            ctlChart.Series["mainSeries"]["PieLabelStyle"] = "Outside";
            ctlChart.Series["mainSeries"].Label = "#VALX - #VAL (#PERCENT)";
            ctlChart.Series[0].ShadowColor = System.Drawing.Color.FromArgb(120, 125, 182, 240);
            ctlChart.Series[0].ShadowOffset = 10;
            ctlChart.ChartAreas[0].Area3DStyle.Enable3D = true;

            ctlChart.ChartAreas[0].Area3DStyle.Rotation = 120;
            ctlChart.ChartAreas[0].BackColor = Color.FromArgb(255, 237, 237, 237);
            ctlChart.Series["mainSeries"].ChartType = SeriesChartType.Pie;
            ctlChart.Series["mainSeries"].ShadowColor = System.Drawing.Color.Aquamarine;
            ctlChart.Series["mainSeries"].ShadowOffset = 5;
            ctlChart.Series["mainSeries"].ChartArea = "mainChartArea";

            ctlChart.ApplyPaletteColors();
            foreach (var series in ctlChart.Series)
            {
                foreach (var point in series.Points)
                {
                    point.Color = Color.FromArgb(220, point.Color);
                }
            }
        }


        private void TimesPerType()
        {
            DateTime startDate;
            DateTime endDate;
            GetTimePeriod(out startDate, out endDate);
            List<int> processedMessages;
            DataSet data = (new Sent_message_PKDAL()).GetMDNDataForTimeStatsByTimeSpan(startDate, endDate);

            Dictionary<String, List<TimeSpan>> groups = new Dictionary<string, List<TimeSpan>>();
            if (ctlTimeChartType.ControlValue.ToString() == "mdn" || ctlTimeChartType.ControlValue.ToString() == "all")
            {
                processedMessages = new List<int>();
                groups.Add("MDN", new List<TimeSpan>());
               
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    if (row["sent_time"] != null && row["sent_time"] is DateTime && row["received_time"] != null && row["received_time"] is DateTime)
                    {
                        if (row["sentType"].ToString() == "0")
                        {
                            if (row["receivedType"].ToString() == "1" && !processedMessages.Contains((int)row["xevmpd_FK"]))
                            {
                                groups["MDN"].Add(((DateTime)row["received_time"]) - ((DateTime)row["sent_time"]));
                                processedMessages.Add((int)row["xevmpd_FK"]);
                            }
                        }
                    }
                }
            }

            if (ctlTimeChartType.ControlValue.ToString() == "all" || ctlTimeChartType.ControlValue.ToString() == "allack" || ctlTimeChartType.ControlValue.ToString() == "ack01")
            {
                groups.Add("ACK01", new List<TimeSpan>());
            }
            if (ctlTimeChartType.ControlValue.ToString() == "all" || ctlTimeChartType.ControlValue.ToString() == "allack" || ctlTimeChartType.ControlValue.ToString() == "ack02")
            {
                groups.Add("ACK02", new List<TimeSpan>());
            }
            if (ctlTimeChartType.ControlValue.ToString() == "all" || ctlTimeChartType.ControlValue.ToString() == "allack" || ctlTimeChartType.ControlValue.ToString() == "ack03")
            {
                groups.Add("ACK03", new List<TimeSpan>());
            }

            data = (new Sent_message_PKDAL()).GetACKDataForTimeStatsByTimeSpan(startDate, endDate);

            processedMessages = new List<int>();
            foreach (DataRow row in data.Tables[0].Rows)
            {
                if (row["sent_time"] != null && row["sent_time"] is DateTime && row["received_time"] != null && row["received_time"] is DateTime)
                {
                    if (row["sentType"].ToString() == "0")
                    {
                      
                        if (row["receivedType"].ToString() == "0" && !processedMessages.Contains((int)row["xevmpd_FK"]))
                        {
                            String type = "";
                            switch ((int)row["receivedStatus"])
                            {
                                case 1: type = "ACK01"; break;
                                case 2: type = "ACK02"; break;
                                case 3: type = "ACK03"; break;
                            }
                            if (groups.ContainsKey(type)){
                                groups[type].Add(((DateTime)row["received_time"]) - ((DateTime)row["sent_time"]));         
                            }
                            processedMessages.Add((int)row["xevmpd_FK"]);
                        }
                    }
                }
            }

            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("hr-HR");
            ctlChart.Titles.Clear();
            ctlChart.Titles.Add("Message response waiting times for messages sent between " + startDate.Date.ToShortDateString() + " and " + endDate.AddDays(-1).Date.ToShortDateString());
            Thread.CurrentThread.CurrentCulture = originalCulture;

            ctlChart.Series.Clear();
            ctlChart.Series.Add(new Series("Max. Time"));
            ctlChart.Series.Add(new Series("Avg. Time"));
            ctlChart.Series.Add(new Series("Min. Time"));

            int pointNr =0;
            bool haveData = false;
            foreach (String groupName in groups.Keys) {
                groups[groupName].Sort();
                if (groups[groupName].Count == 0)
                {
                    groups[groupName].Add(new TimeSpan(0));
                }
                else
                {
                    haveData = true;
                }
                ctlChart.Series["Max. Time"].Points.AddY(groups[groupName][groups[groupName].Count - 1].TotalMilliseconds / 1000.0);
                ctlChart.Series["Min. Time"].Points.AddY(groups[groupName][0].TotalMilliseconds / 1000.0);
                ctlChart.Series["Avg. Time"].Points.AddY((groups[groupName].Sum(p =>p.TotalMilliseconds) / 1000.0)/groups[groupName].Count);
                ctlChart.Series[0].Points[pointNr].AxisLabel = groupName;
                pointNr++;
            }

            if (!haveData)
            {
                DisplayChartMessage("There is no data to display for selected time period.");
                return; 
            }
          
            ctlChart.ChartAreas[0].Area3DStyle.Enable3D = true;
            ctlChart.ChartAreas[0].Area3DStyle.IsClustered = true;

            ctlChart.Series["Max. Time"].ChartType = SeriesChartType.Column;
            ctlChart.Series["Avg. Time"].ChartType = SeriesChartType.Column;
            ctlChart.Series["Min. Time"].ChartType = SeriesChartType.Column;

            ctlChart.Legends.Add(new Legend(""));

            ctlChart.ChartAreas[0].AxisX.Interval = 1;


            ctlChart.Series["Min. Time"].Color = Color.FromArgb(220, 210, 49, 59);
            ctlChart.Series["Avg. Time"].Color = Color.FromArgb(220, 150, 199, 197);

            ctlChart.ChartAreas[0].AxisY.LabelStyle.Format = "# s";
            ctlChart.ApplyPaletteColors();
            foreach (var series in ctlChart.Series)
            {
                foreach (var point in series.Points)
                {
                    point.Color = Color.FromArgb(230, point.Color);
                }
            }
        }

        private void ctlTimePeriod_InputValueChanged(object seneder, EventArgs args)
        {
            if (ctlTimePeriod.ControlValue.ToString() == "custom")
            {
                tblTimePeriod.Visible = true;
                pnlTimeFilter.Style["width"] = "850px";
            }
            else 
            {
                tblTimePeriod.Visible = false;
                pnlTimeFilter.Style["width"] = "200px";
            }
           
        }

        private DataTable GenerateDataSource(Dictionary<DateTime, int> data, DateTime startDate, DateTime endDate, string groupBy)
        {
            DataTable dataSource = new DataTable();
            dataSource.Columns.Add("xValue", typeof(String));
            dataSource.Columns.Add("yValue", typeof(int));

            if (String.IsNullOrEmpty(groupBy))
            {
                if ((endDate - startDate).TotalDays <= 50)
                {
                    groupBy = "day";
                }
                else if ((endDate - startDate).TotalDays <= 200)
                {
                    groupBy = "week";
                }
                else if ((endDate - startDate).TotalDays <= 1000)
                {
                    groupBy = "month";
                }
                else groupBy = "year";
            }

            switch (groupBy)
            {
                case "day":
                        Dictionary<DateTime, int> dataByDay = new Dictionary<DateTime, int>();
                        for (int i = 0; i < (endDate - startDate).TotalDays; i++)
                        {
                            dataByDay.Add(startDate.AddDays(i).Date, 0);
                        }
                        foreach (KeyValuePair<DateTime, int> kvp in data)
                        {
                            dataByDay[kvp.Key.Date] += kvp.Value;
                        }
                        foreach (DateTime date in dataByDay.Keys.OrderBy(key => key))
                        {
                            dataSource.Rows.Add(dataSource.NewRow());
                            dataSource.Rows[dataSource.Rows.Count - 1]["xValue"] = (date.Day).ToString() + ". " + (date.Month).ToString() + ".";
                            dataSource.Rows[dataSource.Rows.Count - 1]["yValue"] = dataByDay[date];
                        }
                    break;
                case "week":
                        Dictionary<DateTime, int> dataByweek = new Dictionary<DateTime, int>();
                        DateTime currentWeek = startDate.StartOfWeek(DayOfWeek.Monday);
                        while (currentWeek < endDate)
                        {
                            dataByweek.Add(currentWeek, 0);
                            currentWeek = currentWeek.AddDays(7);
                        }
                        foreach (KeyValuePair<DateTime, int> kvp in data)
                        {
                            dataByweek[kvp.Key.Date.StartOfWeek(DayOfWeek.Monday)] += kvp.Value;
                        }
                        foreach (DateTime date in dataByweek.Keys.OrderBy(key => key))
                        {
                            dataSource.Rows.Add(dataSource.NewRow());
                            dataSource.Rows[dataSource.Rows.Count - 1]["xValue"] = "wk "+date.WeekNumber() + ", " + (date.Year).ToString() + ".";
                            dataSource.Rows[dataSource.Rows.Count - 1]["yValue"] = dataByweek[date];
                        }
                    break;
                case "month":
                        Dictionary<DateTime, int> dataByMonth = new Dictionary<DateTime, int>();
                        DateTime currentMonth = new DateTime(startDate.Year, startDate.Month, 1);
                        while (currentMonth < endDate)
                        {
                            dataByMonth.Add(currentMonth, 0);
                            currentMonth = currentMonth.AddMonths(1);
                        }
                        foreach (KeyValuePair<DateTime, int> kvp in data)
                        {
                            dataByMonth[new DateTime(kvp.Key.Date.Year, kvp.Key.Date.Month, 1)] += kvp.Value;
                        }
                        foreach (DateTime date in dataByMonth.Keys.OrderBy(key => key))
                        {
                            dataSource.Rows.Add(dataSource.NewRow());
                            dataSource.Rows[dataSource.Rows.Count - 1]["xValue"] = (date.Month).ToString() + ". " + (date.Year).ToString() + ".";
                            dataSource.Rows[dataSource.Rows.Count - 1]["yValue"] = dataByMonth[date];
                        }
                    break;
                case "year":
                        Dictionary<DateTime, int> dataByYear = new Dictionary<DateTime, int>();
                        DateTime currentYear = new DateTime(startDate.Year, 1, 1);
                        while (currentYear < endDate)
                        {
                            dataByYear.Add(currentYear, 0);
                            currentYear = currentYear.AddYears(1);
                        }
                        foreach (KeyValuePair<DateTime, int> kvp in data)
                        {
                            dataByYear[new DateTime(kvp.Key.Date.Year, 1, 1)] += kvp.Value;
                        }
                        foreach (DateTime date in dataByYear.Keys.OrderBy(key => key))
                        {
                            dataSource.Rows.Add(dataSource.NewRow());
                            dataSource.Rows[dataSource.Rows.Count - 1]["xValue"] = (date.Year).ToString() + ". ";
                            dataSource.Rows[dataSource.Rows.Count - 1]["yValue"] = dataByYear[date];
                }
                    break;
            }

            if ((endDate - startDate).TotalDays < 60)
            {
               
            }
            else
            {

                
            }

            return dataSource;
        }

        private void GetTimePeriod(out DateTime startDate, out DateTime endDate)
        {
            startDate = DateTime.Now.Date;
            endDate = startDate.AddDays(1).Date;
            switch (ctlTimePeriod.ControlValue.ToString())
            {
                case "today":
                    startDate = DateTime.Now.Date;
                    endDate = DateTime.Now.AddDays(1).Date;
                    break;
                case "yesterday":
                    startDate = DateTime.Now.AddDays(-1).Date;
                    endDate = DateTime.Now.Date;
                    break;
                case "thisWeek":
                    startDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                    endDate = DateTime.Now.Date.AddDays(1);
                    break;
                case "lastWeek":
                    startDate = DateTime.Now.AddDays(-7).StartOfWeek(DayOfWeek.Monday).Date;
                    endDate = startDate.AddDays(7).Date;
                    break;
                case "thisMonth":
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
                    endDate = startDate.AddMonths(1).Date;
                    break;
                case "lastMonth":
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).Date;
                    endDate = startDate.AddMonths(1);
                    break;
                case "custom":

                    System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("hr-HR");
                    if (!(ValidationHelper.IsValidDateTime(ctlTimePeriodStart.ControlTextValue, cultureInfo)
                          && DateTime.TryParse(ctlTimePeriodStart.ControlTextValue, cultureInfo.DateTimeFormat, DateTimeStyles.None, out startDate))) startDate = DateTime.Now.Date;
                    if (!(ValidationHelper.IsValidDateTime(ctlTimePeriodEnd.ControlTextValue, cultureInfo)
                         && DateTime.TryParse(ctlTimePeriodEnd.ControlTextValue, cultureInfo.DateTimeFormat, DateTimeStyles.None, out endDate))) endDate = startDate;

                    endDate = endDate.AddDays(1).Date;
                    break;
            }
            ctlTimePeriodStart.ControlValue = startDate;
            ctlTimePeriodEnd.ControlValue = endDate.AddDays(-1);
        }

        private void BindTimeFilter(bool groupBy)
        {
            this.pnlTimeFilter.Visible = true;
            this.ctlTimePeriod.ControlBoundItems.Add(new ListItem("Today", "today"));
            this.ctlTimePeriod.ControlBoundItems.Add(new ListItem("Yesterday", "yesterday"));
            this.ctlTimePeriod.ControlBoundItems.Add(new ListItem("This week", "thisWeek"));
            this.ctlTimePeriod.ControlBoundItems.Add(new ListItem("Last week", "lastWeek"));
            this.ctlTimePeriod.ControlBoundItems.Add(new ListItem("This month", "thisMonth"));
            this.ctlTimePeriod.ControlBoundItems.Add(new ListItem("Last Month", "lastMonth"));
            this.ctlTimePeriod.ControlBoundItems.Add(new ListItem("Custom period: ", "custom"));
            if (groupBy)
            {
                this.ctlGroupBy.ControlBoundItems.Add(new ListItem("Day", "day"));
                this.ctlGroupBy.ControlBoundItems.Add(new ListItem("Week", "week"));
                this.ctlGroupBy.ControlBoundItems.Add(new ListItem("Month", "month"));
                this.ctlGroupBy.ControlBoundItems.Add(new ListItem("Year", "year"));
                ctlGroupBy.Visible = true;
            }
            else
            {
                ctlGroupBy.Visible = false;
            }
        }

        private void BindTimeStatsForm() {
            this.ctlTimeChartType.Visible = true;
            this.ctlTimeChartType.ControlBoundItems.Add(new ListItem("All", "all"));
            this.ctlTimeChartType.ControlBoundItems.Add(new ListItem("All ACK", "allack"));
            this.ctlTimeChartType.ControlBoundItems.Add(new ListItem("ACK01", "ack01"));
            this.ctlTimeChartType.ControlBoundItems.Add(new ListItem("ACK02", "ack02"));
            this.ctlTimeChartType.ControlBoundItems.Add(new ListItem("ACK03", "ack03"));
            this.ctlTimeChartType.ControlBoundItems.Add(new ListItem("MDN", "mdn"));
        }

        private void DisplayChartMessage(String message)
        {
            ctlChart.Titles.Clear();

            ctlChart.Titles.Add(message);
            ctlChart.Titles[0].ForeColor = Color.FromArgb(255, 220, 89, 62);
            ctlChart.Titles[0].Font = new Font(ctlChart.Titles[0].Font.FontFamily, 14);
            ctlChart.ChartAreas[0].Visible = false;
            ctlChart.Style["margin-top"] = "70px";
            return;
        }

    }



    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static int WeekNumber(this System.DateTime value)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(value, CalendarWeekRule.FirstFourDayWeek,
                                                                     DayOfWeek.Monday);

        }
    }
}