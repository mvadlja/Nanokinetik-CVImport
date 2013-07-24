using System;
using System.Web.UI;
using AspNetUIFramework.ExcelExport;

namespace AspNetUI.ucControls
{
    public partial class ExcelExport : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isPureHtml = false;
            isPureHtml = Convert.ToBoolean(Request.QueryString["PureHtml"]);

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Export_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
            Response.Charset = "";

            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter(System.Globalization.CultureInfo.CreateSpecificCulture("hr-HR"));
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

       
            //Response.ContentEncoding = System.Text.Encoding.UTF8;

            //&#154 š

            //ExcelRepository.HTML = ExcelRepository.HTML.Replace("š", "&#154");
            //ExcelRepository.HTML = ExcelRepository.HTML.Replace("Š", "&#138");
            //ExcelRepository.HTML = ExcelRepository.HTML.Replace("ž", "&#158");
            //ExcelRepository.HTML = ExcelRepository.HTML.Replace("Ž", "&#142");
            //ExcelRepository.HTML = ExcelRepository.HTML.Replace("č", "&#232");
            //ExcelRepository.HTML = ExcelRepository.HTML.Replace("Č", "&#200");
            //ExcelRepository.HTML = ExcelRepository.HTML.Replace("Ć", "&#198");
            //ExcelRepository.HTML = ExcelRepository.HTML.Replace("ć", "&#230");
            //ExcelRepository.HTML = ExcelRepository.HTML.Replace("đ", "&#240");
            //ExcelRepository.HTML = ExcelRepository.HTML.Replace("Đ", "&#208");


            if (isPureHtml)
            {
                Response.Write(ExcelRepository.HTML);
            }
            else
            {
                System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                ExcelRepository.Control.RenderControl(htmlWrite);
                Response.Write(stringWrite.ToString());
                
            }

            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }
    }
}