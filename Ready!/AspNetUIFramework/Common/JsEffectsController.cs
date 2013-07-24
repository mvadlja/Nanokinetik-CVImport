using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace AspNetUIFramework
{
    public static class JsEffectsController
    {
        public static void GvRowMouseOverEffect(GridViewRow gvRow)
        {
            gvRow.Attributes["onmouseover"] = "MouseIn(this)";
            gvRow.Attributes["onmouseout"] = "MouseOut(this)";
        }
    }
}
