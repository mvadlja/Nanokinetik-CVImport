using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AspNetUIFramework;

namespace AspNetUI.Views.Shared.UserControl
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

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void ContextMenuItem_Click(object sender, EventArgs e)
        {
            var eventType = (ContextMenuEventTypes)Enum.Parse(typeof(ContextMenuEventTypes), ((LinkButton)sender).CommandArgument);
            OnContextMenuItemClick(sender, new ContextMenuEventArgs(eventType));
        }

        #region IContextMenu Members


        public void SetContextMenuItemsEnabled(ContextMenuItem[] contextMenuItems)
        {
            var contextMenuRow = (HtmlControl)FindControl("ContextMenuLayout");

            foreach (Control _contextMenuItem in contextMenuRow.Controls)
            {
                if (!(_contextMenuItem is WebControl)) continue;
                var contextMenuItem = ((WebControl)_contextMenuItem);

                foreach (var menuItem in contextMenuItems)
                {
                    if ((contextMenuItem.ID == string.Format("lbt{0}", menuItem.EventType)) || contextMenuItem.ID == "additionalContextMenuItems")
                    {
                        contextMenuItem.Enabled = true;
                        contextMenuItem.Attributes.CssStyle.Add("cursor", "pointer");
                        contextMenuItem.Attributes.CssStyle.Add("color", "black !important");
                        contextMenuItem.Attributes["title"] = menuItem.ToolTip;

                        foreach (var webControl in contextMenuItem.Controls.OfType<WebControl>()) webControl.Enabled = false;

                        break;
                    }
                }
            }
        }

        public void SetContextMenuItemsDisabled(ContextMenuItem[] contextMenuItems) {
            var contextMenuRow = (HtmlControl)this.FindControl("ContextMenuLayout");

            foreach (Control contextMenuItem in contextMenuRow.Controls)
            {
                if (contextMenuItems.Any(menuItem => (contextMenuItem.ID == string.Format("lbt{0}", menuItem.EventType)) || contextMenuItem.ID == "additionalContextMenuItems"))
                {
                    if (contextMenuItem is LinkButton) {
                        var disabledButton = (contextMenuItem as LinkButton);
                        disabledButton.Enabled = false;
                        disabledButton.Attributes.CssStyle.Add("cursor", "default");
                        disabledButton.Attributes.CssStyle.Add("color", "gray");
                    }
                        
                    foreach (var webControl in contextMenuItem.Controls.OfType<WebControl>()) webControl.Enabled = false;
                }
            }
        }

        public void SetContextMenuItemsVisible(ContextMenuItem[] contextMenuItems)
        {
            var contextMenuRow = (HtmlControl)this.FindControl("ContextMenuLayout");

            foreach (Control contextMenuItem in contextMenuRow.Controls)
            {
                if (contextMenuItem is LinkButton)
                {
                    var visible = false;

                    foreach (var menuItem in contextMenuItems)
                    {
                        if (contextMenuItem.ID == string.Format("lbt{0}", menuItem.EventType))
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

            ContextMenuLayout.Visible = contextMenuItems.Length > 0;
        }

        #endregion
    }
}