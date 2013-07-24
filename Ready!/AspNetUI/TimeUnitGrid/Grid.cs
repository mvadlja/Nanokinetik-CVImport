using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.Data;
namespace AspNetUI.TimeUnitGrid
{
    public class Grid
    {
        public class GridViewGroup
        {

            public GridViewGroup()
            {
                data = new DataTable();
                partOnNextPage = false;
                partOnNextPage = false;
                grouppingValue = null;
                groupTitle = "Untitled";
            }

            public DataTable data;
            public String groupTitle;
            public bool partOnNextPage;
            public bool partOnPrevPage;
            public object grouppingValue;
        }

        public String RenderGrid(HttpRequest Request, List<GridViewGroup> groups, int totalRecordsCount, String viewId)
        {
            var visibleColumns = new List<string>() { "time_unit_name", "resuser", "actual_date", "time", "description" };

            StringBuilder builder = new StringBuilder();
            string url = LinkHelper.getQueryStringArray(new string[] { "pageSize" });

            int pageSize = (HttpContext.Current.Session["gridViewItemsPerPage"]) != null ? (int)(HttpContext.Current.Session["gridViewItemsPerPage"]) : 15;

            Pager pagerTop = new Pager();
            builder.AppendLine(pagerTop.RenderPager(Request, totalRecordsCount));
            builder.AppendLine("<table class=\"grid\" id=\"" + viewId + "\" cellpadding=\"0\" cellspacing=\"0\" style=\"table-layout:fixed;\">");

            builder.AppendLine("<tr>");

            if (visibleColumns.Contains("time_unit_name"))
            {
                builder.AppendLine("<th class=\"w12\">");
                builder.AppendLine(LinkHelper.GetSortLink("Name", "time_unit_name", viewId));
                builder.AppendLine("</th>");
            }
            if (visibleColumns.Contains("resuser"))
            {
                builder.AppendLine("<th class=\"w6\">");
                builder.AppendLine(LinkHelper.GetSortLink("Responsible User", "resuser", viewId));
                builder.AppendLine("</th>");
            }
            if (visibleColumns.Contains("actual_date"))
            {
                builder.AppendLine("<th class=\"w6\">");
                builder.AppendLine(LinkHelper.GetSortLink("Actual date", "actual_date", viewId));
                builder.AppendLine("</th>");
            }
            if (visibleColumns.Contains("time"))
            {
                builder.AppendLine("<th class=\"w6\">");
                builder.AppendLine(LinkHelper.GetSortLink("Time", "time", viewId));
                builder.AppendLine("</th>");
            }
            if (visibleColumns.Contains("description"))
            {
                builder.AppendLine("<th class=\"w6\">");
                builder.AppendLine(LinkHelper.GetSortLink("Description", "description", viewId));
                builder.AppendLine("</th>");
            }
            if (visibleColumns.Contains("activity_name"))
            {
                builder.AppendLine("<th class=\"w9\">");
                builder.AppendLine(LinkHelper.GetSortLink("Activity", "activity_name", viewId));
                builder.AppendLine("</th>");
            }
            if (visibleColumns.Contains("activity_description"))
            {
                builder.AppendLine("<th class=\"w5\">");
                builder.AppendLine(LinkHelper.GetSortLink("Activity description", "activity_description", viewId));
                builder.AppendLine("</th>");
            }
            if (visibleColumns.Contains("products"))
            {
                builder.AppendLine("<th class=\"w10\">");
                builder.AppendLine(LinkHelper.GetSortLink("Products", "products", viewId));
                builder.AppendLine("</th>");
            }
            if (visibleColumns.Contains("insertedby"))
            {
                builder.AppendLine("<th class=\"w7\">");
                builder.AppendLine(LinkHelper.GetSortLink("Inserted by", "insertedby", viewId));
                builder.AppendLine("</th>");
            }

            builder.AppendLine("</tr>");
            builder.AppendLine("<tr>");
            
            if (visibleColumns.Contains("time_unit_name"))
            {
                builder.AppendLine("<th>");
                builder.AppendLine("<input type=\"text\" name=\"time_unit_name\" id=\"time_unit_name\"  value=\"" + Request.QueryString["time_unit_name"] + "\" />");
                builder.AppendLine("</th>");
            }
            if (visibleColumns.Contains("resuser"))
            {
                builder.AppendLine("<th>");
                builder.AppendLine("<input type=\"text\" name=\"resuser\"  id=\"resuser\" value=\"" + Request.QueryString["resuser"] + "\" />");
                builder.AppendLine("</th>");
            }
            if (visibleColumns.Contains("actual_date"))
            {
                builder.AppendLine("<th>");
                builder.AppendLine("<input type=\"text\" name=\"actual_date\" id=\"actual_date\" value=\"" + Request.QueryString["actual_date"] + "\" />");
                builder.AppendLine("</th>");
            }
            if (visibleColumns.Contains("time"))
            {
                builder.AppendLine("<th>");
                builder.AppendLine("<input type=\"text\" name=\"time\"  id=\"time\" value=\"" + Request.QueryString["time"] + "\" />");
                builder.AppendLine("</th>");
            }
            if (visibleColumns.Contains("description"))
            {
                builder.AppendLine("<th>");
                builder.AppendLine("<input type=\"text\" name=\"description\" id=\"description\" value=\"" + Request.QueryString["description"] + "\" />");
                builder.AppendLine("</th>");
            }
            if (visibleColumns.Contains("activity_name"))
            {
                builder.AppendLine("<th>");
                builder.AppendLine("<input type=\"text\" name=\"activity_name\" id=\"activity_name\" value=\"" + Request.QueryString["activity_name"] + "\" />");
                builder.AppendLine("</th>");
            }
            if (visibleColumns.Contains("activity_description"))
            {
                builder.AppendLine("<th>");
                builder.AppendLine("<input type=\"text\" name=\"activity_description\" id=\"activity_description\" value=\"" + Request.QueryString["activity_description"] + "\" />");
                builder.AppendLine("</th>");
            }
            if (visibleColumns.Contains("products"))
            {
                builder.AppendLine("<th>");
                builder.AppendLine("<input type=\"text\" name=\"products\"  id=\"products\"  value=\"" + Request.QueryString["products"] + "\" />");
                builder.AppendLine("</th>");
            }
            if (visibleColumns.Contains("insertedby"))
            {
                builder.AppendLine("<th>");
                builder.AppendLine("<input type=\"text\" name=\"insertedby\" id=\"insertedby\" value=\"" + Request.QueryString["insertedby"] + "\" />");
                builder.AppendLine("</th>");
            }
            builder.AppendLine("</tr>");

            builder.AppendLine("<tbody>");

            int dataRowOrder = 0;
            if (groups.Count > 0)
            {
                foreach (GridViewGroup g in groups)
                {
                    if (g.partOnNextPage)
                    {
                        builder.AppendLine("<tr class=\"grouppingRow\"  id=\"" + LinkHelper.MakeValidId(viewId + "_" + g.grouppingValue.ToString()) + "\">");
                        builder.AppendLine("<td colspan=\"9\">" + g.groupTitle + " Continued on the next page</td>");
                        builder.AppendLine("</tr>");
                    }
                    else if (g.partOnPrevPage)
                    {
                        builder.AppendLine("<div class=\"grouppingRow auto-hide\"   id=\"" + LinkHelper.MakeValidId(viewId + "_" + g.grouppingValue.ToString()) + "\">");
                        builder.AppendLine(g.groupTitle);
                        builder.AppendLine("</div>");
                    }
                    else
                    {
                        builder.AppendLine("<tr class=\"grouppingRow\" id=\"" + LinkHelper.MakeValidId(viewId + "_" + g.grouppingValue.ToString()) + "\">");
                        builder.AppendLine("<td colspan=\"9\">" + g.groupTitle + "</td>");
                        builder.AppendLine("</tr>");
                    }
                    foreach (DataRow row in g.data.Rows)
                    {

                        builder.AppendLine("<tr class=\"data-row " + (dataRowOrder % 2 == 0 ? "even" : "odd") + " \">");

                        if (visibleColumns.Contains("time_unit_name"))
                        {
                            builder.AppendLine("<td class=\"" + LinkHelper.getIsSorted("time_unit_name", viewId) + "\">" + row["time_unit_name"] + "</td>");
                        }
                        if (visibleColumns.Contains("resuser"))
                        {
                            builder.AppendLine("<td class=\"" + LinkHelper.getIsSorted("resuser", viewId) + "\">" + row["resuser"] + "</td>");
                        }
                        if (visibleColumns.Contains("actual_date"))
                        {
                            builder.AppendLine("<td class=\"" + LinkHelper.getIsSorted("actual_date", viewId) + "\">" + String.Format("{0:dd.MM.yyyy}", (DateTime)row["actual_date"]) + "</td>");
                        }
                        if (visibleColumns.Contains("time"))
                        {
                            builder.AppendLine("<td class=\"" + LinkHelper.getIsSorted("time", viewId) + "\">" + row["time"] + "</td>");
                        }
                        if (visibleColumns.Contains("description"))
                        {
                            builder.AppendLine("<td class=\"" + LinkHelper.getIsSorted("description", viewId) + "\">" + row["description"] + "</td>");
                        }
                        if (visibleColumns.Contains("activity_name"))
                        {
                            builder.AppendLine("<td class=\"" + LinkHelper.getIsSorted("activity_name", viewId) + "\">" + row["activity_name"] + "</td>");
                        }
                        if (visibleColumns.Contains("activity_description"))
                        {
                            builder.AppendLine("<td class=\"" + LinkHelper.getIsSorted("activity_description", viewId) + "\">" + row["activity_description"] + "</td>");
                        }
                        if (visibleColumns.Contains("products"))
                        {
                            builder.AppendLine("<td class=\"" + LinkHelper.getIsSorted("products", viewId) + "\">" + row["products"] + "</td>");
                        }
                        if (visibleColumns.Contains("insertedby"))
                        {
                            builder.AppendLine("<td class=\"" + LinkHelper.getIsSorted("insertedby", viewId) + "\">" + row["insertedby"] + "</td>");
                        }
                        
                        builder.AppendLine("</tr>");
                        dataRowOrder++;
                    }
                }

            }
            else
            {

                builder.AppendLine("<tr class=\"emptyRow\">");
                builder.AppendLine("<td colspan=\"9\" class=\"no_records\">No data to display</td>");
                builder.AppendLine("</tr>");

            }
            builder.AppendLine("</tbody>");

            builder.AppendLine("</table>");
            builder.AppendLine("<div class=\"per_page\">");


            builder.AppendLine("Per page:");
            builder.AppendLine("<a href=\"javascript:RefreshGrid('" + url + "pageSize=15&updatePageSize=true');\"");
            if (pageSize == 15) builder.AppendLine(" class=\"current\" "); builder.AppendLine(">15</a>");
            builder.AppendLine("<a href=\"javascript:RefreshGrid('" + url + "pageSize=50&updatePageSize=true');\"");
            if (pageSize == 50) builder.AppendLine(" class=\"current\" "); builder.AppendLine(">50</a>");
            builder.AppendLine("<a href=\"javascript:RefreshGrid('" + url + "pageSize=100&updatePageSize=true');\"");
            if (pageSize == 100) builder.AppendLine(" class=\"current\" "); builder.AppendLine(">100</a>");
            builder.AppendLine("</div>");
            Pager bottomPager = new Pager();
            builder.AppendLine(bottomPager.RenderPager(Request, totalRecordsCount));
            builder.AppendLine("<br /><br />");
            return builder.ToString();
        }


    }
}