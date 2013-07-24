using System;
using System.Configuration;

namespace AspNetUI
{
    public partial class Error : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string guid = String.Empty;
            string msg = String.Empty;

            if (Request["guid"] != null) guid = Server.UrlDecode(Request["guid"]);
            if (Request["msg"] != null) msg = Server.UrlDecode(Request["msg"]);

            lblError.Text = string.Format("<hr/>Error ID: {0}<hr/>{1}<br />", guid, msg);

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ApplicationVersion"]))
            {
                Label1.Text = string.Format("v{0} ", ConfigurationManager.AppSettings["ApplicationVersion"]);
        }
        }
    }
}
