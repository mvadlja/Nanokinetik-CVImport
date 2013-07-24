using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Linq.Expressions;

namespace AspNetUI.TimeUnitGrid
{
    public class LinkHelper
    {
        public static string GetSortLink(string columnTitle, string columnName, String viewId)
        {
            string url = LinkHelper.getQueryStringArray(new string[] { "sortColumn", "sortOrder" });
            string queryStringColumnName = System.Web.HttpContext.Current.Request.QueryString["sortColumn"];
            string queryStringSortOrder = System.Web.HttpContext.Current.Request.QueryString["sortOrder"];
            string mainSortColumn = (string)HttpContext.Current.Session[viewId + "_MainSortColumn"];
            string mainSortColumnOrder = System.Web.HttpContext.Current.Request.QueryString["mainSortColumnOrder"];

            string groupSortColumn = (string)HttpContext.Current.Session[viewId + "_GroupSortColumn"];
            string groupSortColumnOrder = System.Web.HttpContext.Current.Request.QueryString["groupSortColumnOrder"];

            string content = "";

            if (!String.IsNullOrWhiteSpace(queryStringColumnName) && queryStringColumnName.ToLower() == columnName.ToLower())
            {
                if (queryStringSortOrder.ToLower().Contains("desc"))
                {
                    content = "<a class=\"desc\" href=\"javascript:RefreshGrid('" + url + "sortColumn=" + columnName + "&sortOrder=asc');\">" + columnTitle + "</a>";
                }
                else
                {
                    content = "<a class=\"asc\" href=\"javascript:RefreshGrid('" + url + "sortColumn=" + columnName + "&sortOrder=desc');\">" + columnTitle + "</a>";
                }
            }
            else if (mainSortColumn.ToLower() == columnName.ToLower())
            {
                if (!String.IsNullOrWhiteSpace(mainSortColumnOrder) && mainSortColumnOrder.ToLower().Contains("desc"))
                {
                    url = LinkHelper.getQueryStringArray(new string[] { "sortColumn", "sortOrder", "mainSortColumnOrder" });
                    content = "<a class=\"desc\" href=\"javascript:RefreshGrid('" + url + "mainSortColumnOrder=asc');\">" + columnTitle + "</a>";
                }
                else
                {
                    url = LinkHelper.getQueryStringArray(new string[] { "sortColumn", "sortOrder", "mainSortColumnOrder" });
                    content = "<a class=\"asc\" href=\"javascript:RefreshGrid('" + url + "mainSortColumnOrder=desc');\">" + columnTitle + "</a>";
                }
            }
            else if (groupSortColumn != null && groupSortColumn.ToLower() == columnName.ToLower())
            {
                if (!String.IsNullOrWhiteSpace(groupSortColumnOrder) && groupSortColumnOrder.ToLower().Contains("asc"))
                {
                    url = LinkHelper.getQueryStringArray(new string[] { "groupSortColumnOrder" });
                    content = "<a class=\"asc\" href=\"javascript:RefreshGrid('" + url + "groupSortColumnOrder=desc');\">" + columnTitle + "</a>";
                }
                else
                {
                    url = LinkHelper.getQueryStringArray(new string[] { "groupSortColumnOrder" });
                    content = "<a class=\"desc\" href=\"javascript:RefreshGrid('" + url + "groupSortColumnOrder=asc');\">" + columnTitle + "</a>";
                }
            }
            else
            {
                content = "<a href=\"javascript:RefreshGrid('" + url + "sortColumn=" + columnName + "&sortOrder=asc');\">" + columnTitle + "</a>";
            }

            return content;
        }

        

        public static string getIsSorted(string columnName, string viewId)
        {
            string content = "";

            string queryStringColumnName = System.Web.HttpContext.Current.Request.QueryString["sortColumn"];
            string mainSortColumn = (string)HttpContext.Current.Session[viewId + "_MainSortColumn"];
            string groupSortColumn = (string)HttpContext.Current.Session[viewId + "_GroupSortColumn"];
            if ((!String.IsNullOrWhiteSpace(queryStringColumnName) && queryStringColumnName.ToLower() == columnName.ToLower()) ||
                
                (!String.IsNullOrWhiteSpace(groupSortColumn) && groupSortColumn.ToLower() == columnName.ToLower()))
            {
                content = "sorted";
            }
            return content;
        }

        public static string getQueryStringArray(string[] escapeParams = null)
        {
            string url = "";
            string queryString = System.Web.HttpContext.Current.Request.QueryString.ToString();
            string[] queryStringArray = queryString.Split('&');

            if (queryStringArray.Count() > 1)
            {
                foreach (string queryStringParam in queryStringArray)
                {
                    string[] queryStringParamsArray = queryStringParam.Split('=');

                    if (escapeParams != null && escapeParams.Contains(queryStringParamsArray[0]) == false && queryStringParamsArray.Length == 2)
                    {
                        url += queryStringParamsArray[0] + "=" + queryStringParamsArray[1] + "&";
                    }
                }
            }
            else if (!String.IsNullOrWhiteSpace(queryString))
            {
                string[] queryStringParamsArray = queryString.Split('=');

                if (escapeParams != null && escapeParams.Contains(queryStringParamsArray[0]) == false && queryStringParamsArray.Length == 2)
                {
                    url += queryStringParamsArray[0] + "=" + queryStringParamsArray[1] + "&";
                }
            }
            if (url.StartsWith("=&")) url=url.Substring(2);
            return url;
        }

        public static string GetQueryStringArrayInclusive(string[] includeParams = null)
        {
            string url = "";
            string queryString = System.Web.HttpContext.Current.Request.QueryString.ToString();
            string[] queryStringArray = queryString.Split('&');

            if (queryStringArray.Count() > 1)
            {
                foreach (string queryStringParam in queryStringArray)
                {
                    string[] queryStringParamsArray = queryStringParam.Split('=');

                    if (includeParams != null && includeParams.Contains(queryStringParamsArray[0]) && queryStringParamsArray.Length == 2)
                    {
                        url += queryStringParamsArray[0] + "=" + queryStringParamsArray[1] + "&";
                    }
                }
            }
            else if (!String.IsNullOrWhiteSpace(queryString))
            {
                string[] queryStringParamsArray = queryString.Split('=');

                if (includeParams != null && includeParams.Contains(queryStringParamsArray[0]) && queryStringParamsArray.Length == 2)
                {
                    url += queryStringParamsArray[0] + "=" + queryStringParamsArray[1] + "&";
                }
            }

            return url;
        }

        public static string MakeValidId(String id)
        {
            return id.Replace(".", "_").Replace(" ", "-").Replace(":", "_");
        }
    }
}
