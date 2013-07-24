using System;
using System.Data;
using System.Configuration;
using System.Web;

namespace AspNetUIFramework.ExcelExport
{
    /// <summary>
    /// Summary description for ExcelExporter
    /// </summary>
    public static class ExcelExporter
    {
        public static void ExportToExcel(DataTable dt, string q, string title)
        {
            DataTableHTML dtHtml = new DataTableHTML();
            dtHtml.Export(dt, q, title);

            ExcelRepository.HTML = dtHtml.HtmlTable.ToString();
            HttpContext.Current.Response.Redirect("~/ucControls/ExcelExport.aspx?PureHtml=true");
        }

        public static void ExportToExcelDetails(DataTable dt, string q, string title)
        {
            DataTableHTML dtHtml = new DataTableHTML();
            dtHtml.ExportDetails(dt, q, title);

            ExcelRepository.HTML = dtHtml.HtmlTable.ToString();
            HttpContext.Current.Response.Redirect("~/ucControls/ExcelExport.aspx?PureHtml=true");
        }
    }
}
