using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace AspNetUIFramework
{
    public class FormModeControler
    {
        public static readonly FormModeControler Instance = new FormModeControler();
        private FormModeControler() { }

        public void MakeControlReadonly(Control c)
        {
            if (c is WebControl)
            {
                WebControl wc = c as WebControl;

                // Find controls with delete function (gridviews with accessable header text "Delete", and buttons with attribute "Delete" or "Save")
                if (wc.Attributes["Operation"] == "Delete") wc.Visible = false;
                if (wc.Attributes["Operation"] == "Save") wc.Visible = false;
                if (wc.Attributes["Operation"] == "New") wc.Visible = false;
                if (wc.Attributes["Operation"] == "CustomEdit") wc.Visible = false;

                if (wc is GridView)
                {
                    GridView gv = wc as GridView;

                    foreach (DataControlField dcf in gv.Columns)
                    {
                        if (dcf.AccessibleHeaderText == "Delete") dcf.Visible = false;
                    }
                }
                else
                {
                    // Recursion
                    foreach (Control cChild in wc.Controls)
                    {
                        MakeControlReadonly(cChild);
                    }
                }
            }
            else if (c is HtmlControl)
            {
                HtmlControl hc = c as HtmlControl;

                // Find controls with delete function (gridviews with accessable header text "Delete", and buttons with attribute "Delete" or "Save")
                if (hc.Attributes["Operation"] == "Delete") hc.Visible = false;
                if (hc.Attributes["Operation"] == "Save") hc.Visible = false;
                if (hc.Attributes["Operation"] == "New") hc.Visible = false;
                if (hc.Attributes["Operation"] == "CustomEdit") hc.Visible = false;

                // Recursion
                foreach (Control cChild in hc.Controls)
                {
                    MakeControlReadonly(cChild);
                }
            }
            else
            {
                // Recursion
                foreach (Control cChild in c.Controls)
                {
                    MakeControlReadonly(cChild);
                }
            }
        }
    }
}
