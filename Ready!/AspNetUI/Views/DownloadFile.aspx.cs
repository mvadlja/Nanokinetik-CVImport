using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetUI.Views
{
    public partial class DownloadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string fullDirectory = Session["InvoicesIncDocument"].ToString();

            string imeFile = fullDirectory.Substring(fullDirectory.LastIndexOf("\\") + 1);

            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + imeFile + "");
            Response.TransmitFile(fullDirectory);
            Response.End();
        }
    }
}