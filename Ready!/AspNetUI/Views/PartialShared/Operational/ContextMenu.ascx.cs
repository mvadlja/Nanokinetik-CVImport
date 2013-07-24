using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AspNetUIFramework;
using System.Collections.Generic;

namespace AspNetUI.Support
{
    public partial class ContextMenu : System.Web.UI.UserControl, IContextMenu
    {
        #region Properties

        public event EventHandler<ContextMenuEventArgs> OnContextMenuItemClick;

        public LinkButton LbtBack { get { return lbtBack; } }
        public LinkButton LbtNew { get { return lbtNew; } }
        public LinkButton LbtSave { get { return lbtSave; } }
        public LinkButton LbtCancel { get { return lbtCancel; } }
        public LinkButton LbtDelete { get { return lbtDelete; } }
        public LinkButton LbtEdit { get { return lbtEdit; } }
        public LinkButton LbtSaveAs { get { return lbtSaveAs; } }
        public LinkButton LbtNextItem { get { return lbtNextItem; } }
        public LinkButton LbtPreviousItem { get { return lbtPreviousItem; } }


        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            //if (!IsPostBack) ContextMenuLayout.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void ContextMenuItem_Click(object sender, EventArgs e)
        {
            ContextMenuEventTypes eventType = (ContextMenuEventTypes)Enum.Parse(typeof(ContextMenuEventTypes), ((LinkButton)sender).CommandArgument);
            OnContextMenuItemClick(sender, new ContextMenuEventArgs(eventType));
        }

        #region IContextMenu Members

        public void SetContextMenuItemsEnabled(ContextMenuItem[] contextMenuItems)
        {
            bool enabled = false;
            HtmlControl contextMenuRow = (HtmlControl)this.FindControl("ContextMenuLayout");

            foreach (Control _contextMenuItem in contextMenuRow.Controls)
            {
                if (!(_contextMenuItem is HtmlControl)) continue;
                HtmlControl contextMenuItem = ((HtmlControl)_contextMenuItem);

                enabled = false;

                foreach (ContextMenuItem menuItem in contextMenuItems)
                {
                    if ((contextMenuItem.ID == "lbt" + menuItem.EventType.ToString()) || contextMenuItem.ID=="additionalContextMenuItems")
                    {
                        contextMenuItem.Disabled = false;
                        enabled = true;
                        contextMenuItem.Attributes["title"] = menuItem.ToolTip;
                        break;
                    }
                }

                if (!enabled)
                {
                    contextMenuItem.Disabled = true;

                    foreach (Control c in contextMenuItem.Controls)
                    {
                        if (c is WebControl) ((WebControl)c).Enabled = false;
                    }
                }
            }
        }

        public void SetContextMenuItemsDisabled(ContextMenuItem[] contextMenuItems) {
            HtmlControl contextMenuRow = (HtmlControl)this.FindControl("ContextMenuLayout");

            foreach (Control contextMenuItem in contextMenuRow.Controls){

                foreach (ContextMenuItem menuItem in contextMenuItems)
                {
                    if ((contextMenuItem.ID == "lbt" + menuItem.EventType.ToString()) || contextMenuItem.ID == "additionalContextMenuItems")
                    {
                        //contextMenuItem.Disabled = true;
                        if (contextMenuItem is LinkButton) {
                            LinkButton disabledButton = (contextMenuItem as LinkButton);
                            disabledButton.Enabled = false;
                            disabledButton.Attributes.CssStyle.Add("cursor", "default");
                            disabledButton.Attributes.CssStyle.Add("color", "gray");
                            (contextMenuItem as LinkButton).Enabled = false;
                        }
                        
                        foreach (Control c in contextMenuItem.Controls)
                        {
                            if (c is WebControl) ((WebControl)c).Enabled = false;
                        }

                        break;
                    }
                }

            }
        }

        public void SetContextMenuItemsVisible(ContextMenuItem[] contextMenuItems)
        {
            bool visible = false;
            HtmlControl contextMenuRow = (HtmlControl)this.FindControl("ContextMenuLayout");

            foreach (Control contextMenuItem in contextMenuRow.Controls)
            {
                if (contextMenuItem is LinkButton)
                {
                    visible = false;

                    foreach (ContextMenuItem menuItem in contextMenuItems)
                    {
                        if (contextMenuItem.ID == "lbt" + menuItem.EventType.ToString())
                        {
                            contextMenuItem.Visible = true;
                            visible = true;
                            (contextMenuItem as LinkButton).Attributes["title"] = menuItem.ToolTip;
                            break;
                        }
                    }

                    if (!visible) contextMenuItem.Visible = false;
                }
            }

            if (contextMenuItems.Length > 0) ContextMenuLayout.Visible = true;
            else ContextMenuLayout.Visible = false;
        }


        public void SetContextMenuItemsEnabledNew(ContextMenuItem[] contextMenuItems)
        {
            var contextMenuRow = (HtmlControl)FindControl("ContextMenuLayout");

            foreach (Control _contextMenuItem in contextMenuRow.Controls)
            {
                if (!(_contextMenuItem is WebControl)) continue;
                var contextMenuItem = ((WebControl)_contextMenuItem);

                foreach (ContextMenuItem menuItem in contextMenuItems.Where(menuItem => (contextMenuItem.ID == "lbt" + menuItem.EventType.ToString()) || contextMenuItem.ID == "additionalContextMenuItems"))
                {
                    contextMenuItem.Enabled = true;
                    contextMenuItem.Attributes.CssStyle.Add("cursor", "pointer");
                    contextMenuItem.Attributes.CssStyle.Add("color", "black !important");
                    contextMenuItem.Attributes["title"] = menuItem.ToolTip;

                    break;
                }
            }
        }

        public void SetContextMenuItemsDisabledNew(ContextMenuItem[] contextMenuItems)
        {
            var contextMenuRow = (HtmlControl)this.FindControl("ContextMenuLayout");

            foreach (Control contextMenuItem in contextMenuRow.Controls)
            {
                foreach (ContextMenuItem menuItem in contextMenuItems)
                {
                    if ((contextMenuItem.ID == "lbt" + menuItem.EventType.ToString()) || contextMenuItem.ID == "additionalContextMenuItems")
                    {
                        if (contextMenuItem is LinkButton)
                        {
                            var disabledButton = (contextMenuItem as LinkButton);
                            disabledButton.Enabled = false;
                            disabledButton.Attributes.CssStyle.Add("cursor", "default");
                            disabledButton.Attributes.CssStyle.Add("color", "gray");
                        }

                        foreach (Control c in contextMenuItem.Controls)
                        {
                            if (c is WebControl) ((WebControl)c).Enabled = false;
                        }

                        break;
                    }
                }
            }
        }

        #endregion
    }
}