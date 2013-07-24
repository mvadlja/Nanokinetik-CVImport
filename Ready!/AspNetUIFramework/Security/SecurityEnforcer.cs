using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CommonTypes;

namespace AspNetUIFramework
{
    public class SecurityEnforcer
    {
        public static readonly SecurityEnforcer Instance = new SecurityEnforcer();
        private SecurityEnforcer() { }

        // Recursivly handles controls state by rights by calling FormModeControler.MakeControlReadonly
        public void EnforceRights(Control c, RightTypes rights)
        {
            if (rights == RightTypes.Delete)
            {
                // All rights, skip proccess
                return;
            }

            if (c is GridView)
            {
                GridView gv = c as GridView;

                foreach (DataControlField dcf in gv.Columns)
                {
                    if (dcf.HeaderText == "Delete" && rights < RightTypes.Delete)
                    {
                        dcf.Visible = false;
                    }
                }
            }
            else if (c is WebControl)
            {
                WebControl wc = c as WebControl;

                switch (rights)
                {
                    // Readonly
                    case RightTypes.Read:
                        FormModeControler.Instance.MakeControlReadonly(wc);
                        break;
                }
            }
            else if (c is HtmlControl)
            {
                HtmlControl hc = c as HtmlControl;

                switch (rights)
                {
                    // Readonly
                    case RightTypes.Read:
                        FormModeControler.Instance.MakeControlReadonly(hc);
                        break;
                }
            }

            // Recursion
            foreach (Control cChild in c.Controls)
            {
                EnforceRights(cChild, rights);
            }
        }
    }
}
