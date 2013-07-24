using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetUI.Views.PartialShared.ControlTemplates
{
    public partial class GridView_CT : System.Web.UI.WebControls.GridView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.RowCreated += new GridViewRowEventHandler(GridView_CT_RowCreated);
            this.RowDataBound += new GridViewRowEventHandler(GridView_CT_RowDataBound);
        }

        void GridView_CT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            throw new NotImplementedException();
        }

        void GridView_CT_RowCreated(object sender, GridViewRowEventArgs e)
        {

            throw new NotImplementedException();
        }
    }
}