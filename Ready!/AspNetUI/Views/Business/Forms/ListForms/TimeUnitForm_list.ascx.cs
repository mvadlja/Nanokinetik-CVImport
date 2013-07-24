using System;
using System.Configuration;
using System.Web.UI;

namespace AspNetUI.Views
{
    public partial class TimeUnitForm_list : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            var scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
            {
                scriptManager.RegisterPostBackControl(btnExport);
                scriptManager.RegisterPostBackControl(ucExporter);
            }

            var applicationUrl = ConfigurationManager.AppSettings["ApplicationURL"];
            var applicationUrlSecure = ConfigurationManager.AppSettings["ApplicationURLSecure"];
            var appVirtualPath = ConfigurationManager.AppSettings["AppVirtualPath"];

            var url = Request.Url.Authority;
            var scheme = Request.Url.Scheme;

            if (scheme == "http") url = applicationUrl;
            if (scheme == "https") url = applicationUrlSecure;

            var applicationUrlWithScheme = string.Format("{0}://{1}{2}", scheme, url, appVirtualPath);
            var callbackInvocation = string.Format("{0}/Services/TimeUnitGrid.ashx", applicationUrlWithScheme);

            ClientScriptManager mgr = Page.ClientScript;
            Type cstype = GetType();

            if (!mgr.IsClientScriptBlockRegistered(cstype, "callbackInovcation"))
            {
                ScriptManager.RegisterClientScriptBlock(this, cstype, "callbackInovcation", "var timUnitInvokeCallback=\"" + callbackInvocation + "\";", true);
            }
        }

        protected class SummaryState
        {
            public int Hours = 0;
            public int Minutes = 0;
            public override string ToString()
            {
                return String.Format("{0:00}:{1:00}", Hours.ToString(), Minutes.ToString());
            }
        }
    }
}