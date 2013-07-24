using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetUI.Views.PartialShared.Operational
{
    public partial class IPPGridViewPager : System.Web.UI.UserControl
    {

       private int itemPerPage = -1;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }



        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (this.itemPerPage == 15) PerPage15.Style["border-bottom"] = "none"; else PerPage15.Style["border-bottom"] = "1px dotted black";
            if (this.itemPerPage == 50) PerPage50.Style["border-bottom"] = "none"; else PerPage50.Style["border-bottom"] = "1px dotted black";
            if (this.itemPerPage == 100) PerPage100.Style["border-bottom"] = "none"; else PerPage100.Style["border-bottom"] = "1px dotted black";
        }
    }
}