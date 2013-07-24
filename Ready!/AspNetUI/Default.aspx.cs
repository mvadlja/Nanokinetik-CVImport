using System;

namespace AspNetUI
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/ActivityView/List.aspx?EntityContext=Activity");
        }
    }
}