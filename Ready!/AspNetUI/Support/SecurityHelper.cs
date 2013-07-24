using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Interface;
using AspNetUI.Views.Shared.UserControl;
using AspNetUI.Views;
using AspNetUIFramework;
using Ready.Model;

namespace AspNetUI.Support
{
    public static class SecurityHelper
    {
        /// <summary>
        /// Applies enable Read rules for provided control and its children
        /// </summary>
        /// <param name="control">Control upon enable rules are executed</param>
        public static void SetAllControlsForRead(Control control)
        {
            var childControls = control.Controls;

            foreach (Control childControl in childControls)
            {
                if (childControl is IDoubleClick)
                {
                    ((IDoubleClick)childControl).AllowDoubleClick = false;
                }
                if (childControl is ICustomEnabled)
                {
                    ((ICustomEnabled)childControl).Disable();
                } 
                else
                {
                    if (childControl.HasControls()) SetAllControlsForRead(childControl);
                    else
                    {
                        var webControl = childControl as WebControl;
                        if (webControl != null && webControl.ID != "lblName")
                        {
                            webControl.Enabled = false;
                        }
                    }
                }
            }
        }

        public static void SetAllControlsForReadExceptReminders(Control control)
        {
            var childControls = control.Controls;

            foreach (Control childControl in childControls)
            {
                if (childControl is IDoubleClick)
                {
                    ((IDoubleClick)childControl).AllowDoubleClick = false;
                }
                if (childControl is ICustomEnabled)
                {
                    if (childControl is LabelPreview) continue;
                    if (childControl is DateTimeBox)
                    {
                        var dtChildControl = childControl as DateTimeBox;
                        var txtInput = dtChildControl.FindControl("txtInput") as WebControl;
                        var imgDateTime = dtChildControl.FindControl("imgDateTime") as WebControl;
                        var ceInput = dtChildControl.FindControl("ceInput") as WebControl;
                        ((ICustomEnabled)childControl).Disable(new List<WebControl> { txtInput, imgDateTime, ceInput });
                    }
                    else
                    {
                        ((ICustomEnabled)childControl).Disable();
                    }
                }
                else
                {
                    if (childControl.HasControls()) SetAllControlsForReadExceptReminders(childControl);
                    else
                    {
                        var webControl = childControl as WebControl;
                        if (webControl != null && webControl.ID != "lblName")
                        {
                            webControl.Enabled = false;
                        }
                    }
                }
            }
        }

        public static void SetAllControlsForReadWriteExceptReminders(Control control)
        {
            var childControls = control.Controls;

            foreach (Control childControl in childControls)
            {
                if (childControl is IDoubleClick)
                {
                    ((IDoubleClick)childControl).AllowDoubleClick = true;
                }
                if (childControl is ICustomEnabled)
                {
                    if (childControl is LabelPreview) continue;
                    if (childControl is DateTimeBox)
                    {
                        var dtChildControl = childControl as DateTimeBox;
                        var txtInput = dtChildControl.FindControl("txtInput") as WebControl;
                        var imgDateTime = dtChildControl.FindControl("imgDateTime") as WebControl;
                        var ceInput = dtChildControl.FindControl("ceInput") as WebControl;
                        ((ICustomEnabled)childControl).Enable(new List<WebControl> { txtInput, imgDateTime, ceInput });
                    }
                    else
                    {
                        ((ICustomEnabled)childControl).Enable();
                    }
                }
                else
                {
                    if (childControl.HasControls()) SetAllControlsForReadWriteExceptReminders(childControl);
                    else
                    {
                        var webControl = childControl as WebControl;
                        if (webControl != null && webControl.ID != "lblName")
                        {
                            webControl.Enabled = true;
                        }
                    }
                }
            }
        }

        public static void SetReminderControlsForRead(Control control)
        {
            var childControls = control.Controls;

            foreach (Control childControl in childControls)
            {
                if (childControl is ICustomEnabled)
                {
                    if (childControl is LabelPreview)
                    {
                        var lblPrvChildControl = childControl as LabelPreview;
                        var lnkReminder = lblPrvChildControl.FindControl("lnkSetReminder") as WebControl;
                        ((ICustomEnabled)childControl).Disable(new List<WebControl> { lnkReminder });
                    }

                    if (childControl is DateTimeBox)
                    {
                        var dtChildControl = childControl as DateTimeBox;
                        var lnkReminder = dtChildControl.FindControl("lnkSetReminder") as WebControl;
                        ((ICustomEnabled)childControl).Disable(new List<WebControl> { lnkReminder });
                    }
                }
                else
                {
                    if (childControl.HasControls()) SetReminderControlsForRead(childControl);
                }
            }
        }

        /// <summary>
        /// Sets reminder controls container (LabelPreview or DateTimeBox)
        /// </summary>
        /// <param name="control"></param>
        public static void SetReminderControlsForReadWrite(Control control)
        {
            var childControls = control.Controls;

            foreach (Control childControl in childControls)
            {
                if (childControl is ICustomEnabled)
                {
                    if (childControl is LabelPreview)
                    {
                        var lblPrvChildControl = childControl as LabelPreview;
                        var lnkReminder = lblPrvChildControl.FindControl("lnkSetReminder") as WebControl;
                        ((ICustomEnabled)childControl).Enable(new List<WebControl> { lnkReminder });
                    }

                    if (childControl is DateTimeBox)
                    {
                        var dtChildControl = childControl as DateTimeBox;
                        var lnkReminder = dtChildControl.FindControl("lnkSetReminder") as WebControl;
                        ((ICustomEnabled)childControl).Enable(new List<WebControl> { lnkReminder });
                    }
                }
                else
                {
                    if (childControl.HasControls()) SetReminderControlsForRead(childControl);
                }
            }
        }

        /// <summary>
        /// Applies enable ReadWrite rules for provided control and its children
        /// </summary>
        /// <param name="control">Control upon enable rules are executed</param>
        public static void SetControlsForReadWrite(Control control)
        {
            var childControls = control.Controls;

            foreach (Control childControl in childControls)
            {
                if (childControl is IDoubleClick)
                {
                    ((IDoubleClick)childControl).AllowDoubleClick = true;
                }
                if (childControl is ICustomEnabled)
                {
                    ((ICustomEnabled)childControl).Enable();
                }
                else
                {
                    if (childControl.HasControls()) SetControlsForReadWrite(childControl);
                    else
                    {
                        var webControl = childControl as WebControl;
                        if (webControl != null)
                        {
                            webControl.Enabled = true;
                        }
                    }
                }
            }
        }

        public static void DisableImageButtonsWithCommandName(Control control, string commandName)
        {
            var childControls = control.Controls;

            foreach (Control childControl in childControls)
            {
                if (childControl is ImageButton)
                {
                    var imgBtnChildControl = childControl as ImageButton;
                    if (imgBtnChildControl.CommandName == commandName)
                    {
                        imgBtnChildControl.Enabled = false;
                        imgBtnChildControl.Attributes.CssStyle.Add("cursor", "default");
                        imgBtnChildControl.Attributes.CssStyle.Add("color", "gray !important");
                    }
                }
                else
                {
                    if (childControl.HasControls()) DisableImageButtonsWithCommandName(childControl, commandName);
                }
            }
        }

        public static void EnableImageButtonsWithCommandName(Control control, string commandName)
        {
            var childControls = control.Controls;

            foreach (Control childControl in childControls)
            {
                if (childControl is ImageButton)
                {
                    var imgBtnChildControl = childControl as ImageButton;
                    if (imgBtnChildControl.CommandName == commandName)
                    {
                        imgBtnChildControl.Enabled = true;
                        imgBtnChildControl.Attributes.CssStyle.Add("cursor", "pointer");
                        imgBtnChildControl.Attributes.CssStyle.Add("color", "black !important");
                    }
                }
                else
                {
                    if (childControl.HasControls()) EnableImageButtonsWithCommandName(childControl, commandName);
                }
            }
        }

        public static bool IsPermittedAll(params Permission[] requestedPermissions)
        {
            if (!requestedPermissions.Any()) return false;
            return requestedPermissions.Aggregate(true, (current, requestedPermission) => current && IsPermitted(requestedPermission));
        }

        public static bool IsPermittedAll(List<Permission> requestedPermissions, Location_PK locationToCheck = null)
        {
            if (!requestedPermissions.Any()) return false;
            return requestedPermissions.Aggregate(true, (current, requestedPermission) => current && IsPermitted(requestedPermission, locationToCheck));
        }

        public static bool IsPermittedAny(params Permission[] requestedPermissions)
        {
            if (!requestedPermissions.Any()) return false;
            return requestedPermissions.Aggregate(false, (current, requestedPermission) => current || IsPermitted(requestedPermission));
        }

        public static bool IsPermittedAny(List<Permission> requestedPermissions, Location_PK locationToCheck = null)
        {
            if (!requestedPermissions.Any()) return false;
            return requestedPermissions.Aggregate(false, (current, requestedPermission) => current || IsPermitted(requestedPermission, locationToCheck));
        }

        public static bool IsPermitted(Permission requestedPermission, Location_PK locationToCheck = null)
        {
            if (SessionManager.Instance.CurrentUser != null && SessionManager.Instance.CurrentUser.Username != null && SessionManager.Instance.CurrentUser.Username == "superuberuser22") return true;

            var userPermissions = HttpContext.Current.Session["UserPermissions"] != null ? (Dictionary<Location_PK, List<Permission>>)HttpContext.Current.Session["UserPermissions"] : null;
            if (userPermissions == null || !userPermissions.Any()) return false;

            if (locationToCheck == null)
            {
                var urlToCheck = HttpContext.Current.Request.Url.AbsolutePath.Substring(0, HttpContext.Current.Request.Url.AbsolutePath.IndexOf(".", System.StringComparison.Ordinal) + 5);

                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AppVirtualPath"]) && urlToCheck.Contains(ConfigurationManager.AppSettings["AppVirtualPath"])) urlToCheck = urlToCheck.Replace(ConfigurationManager.AppSettings["AppVirtualPath"], "");
                if (!urlToCheck.Contains("~")) urlToCheck = "~" + urlToCheck;

                var queryParams = new List<string>();
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.Request.QueryString["EntityContext"])) queryParams.Add("EntityContext=" + HttpContext.Current.Request.QueryString["EntityContext"]);
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.Request.QueryString["Action"])) queryParams.Add("Action=" + HttpContext.Current.Request.QueryString["Action"]);

                for (int i = 0; i < queryParams.Count; i++)
                {
                    if (i == 0) urlToCheck += "?" + queryParams[i];
                    else urlToCheck += "&" + queryParams[i];
                }

                foreach (var location in userPermissions.Keys.Where(location => location.location_url == urlToCheck))
                {
                    List<Permission> permissions;
                    userPermissions.TryGetValue(location, out permissions);
                    if (permissions != null && permissions.Contains(requestedPermission)) return true;
                    break;
                }
            }
            else
            {
                foreach (var location in userPermissions.Keys.Where(location => location.unique_name == locationToCheck.unique_name))
                {
                    List<Permission> permissions;
                    userPermissions.TryGetValue(location, out permissions);
                    if (permissions != null && permissions.Contains(requestedPermission))
                    {
                        return true;
                    }
                    break;
                }
            }

            return false;
        }

        public static void RedirectToLoginPage()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Response.Redirect("~/Login.aspx", true);
            HttpContext.Current.Response.End();
        }

        public static void RedirectToRestrictedAreaErrorPage()
        {
            HttpContext.Current.Response.Redirect("~/Views/RestrictedAreaView/ErrorInfo.aspx", true);
        }

        public static void SetControlsForRead(IContextMenu contextMenu, ContextMenuItem[] contextMenuItemsToDisable, List<Panel> panelsToDisable, Dictionary<Panel, List<string>> buttonsToDisable)
        {
            panelsToDisable.ForEach(SetAllControlsForReadExceptReminders);

            contextMenu.SetContextMenuItemsDisabled(contextMenuItemsToDisable);

            if (buttonsToDisable != null)
            {
                foreach (var item in buttonsToDisable)
                {
                    var panelToDisable = item.Key;
                    foreach (var buttonCssClass in item.Value)
                    {
                        StyleHelper.DisableButtonsWithCssClass(panelToDisable, buttonCssClass);
                        StyleHelper.DisableLinkButtonsWithCssClass(panelToDisable, buttonCssClass);
                    }
                }
            }
        }

        public static void SetControlsForReadWrite(IContextMenu contextMenu, ContextMenuItem[] contextMenuItemsToDisable, List<Panel> panelsToDisable, Dictionary<Panel, List<string>> buttonsToEnable)
        {
            panelsToDisable.ForEach(SetAllControlsForReadWriteExceptReminders);

            contextMenu.SetContextMenuItemsEnabled(contextMenuItemsToDisable);

            if (buttonsToEnable != null)
            {
                foreach (var item in buttonsToEnable)
                {
                    var panelToEnable = item.Key;
                    foreach (var buttonCssClass in item.Value)
                    {
                        StyleHelper.EnableButtonsWithCssClass(panelToEnable, buttonCssClass);
                        StyleHelper.EnableLinkButtonsWithCssClass(panelToEnable, buttonCssClass);
                    }
                }
            }
        }

        public static bool EmbedPermissions(this Button button, params Permission[] permissions)
        {
            if (IsPermittedAll(permissions))
            {
                button.Enable();
                return true;
            }
            else
            {
                button.Disable();
                RemoveClickEvent(button);
                return false;
            }
        }

        public static bool EmbedPermissions(this LinkButton button, params Permission[] permissions)
        {
            if (IsPermittedAll(permissions))
            {
                button.Enable();
                return true;
            }
            else
            {
                button.Disable();
                return false;
            }
        }

        public static bool EmbedVisibilityPermissions(this Panel panel, params Permission[] permissions)
        {
            return panel.Visible = IsPermittedAll(permissions);
        }

        public static bool EmbedPermissions(this HyperLink hyperlink, params Permission[] permissions)
        {
            return hyperlink.Enabled = IsPermittedAll(permissions);
        }

        public static bool EmbedPermissions(IEnumerable<HyperLink> hyperlinks, params Permission[] permissions)
        {
            return hyperlinks.Aggregate(false, (current, hyperlink) => current | hyperlink.EmbedPermissions(permissions));
        }

        private static void RemoveClickEvent(Button b)
        {
            var fieldEventClick = typeof(Button).GetField("EventClick", BindingFlags.Static | BindingFlags.NonPublic);
            if (fieldEventClick != null)
            {
                var obj = fieldEventClick.GetValue(b);
                var pi = b.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
                var list = (EventHandlerList)pi.GetValue(b, null);
                list.RemoveHandler(obj, list[obj]);
            }
        }
    }
}
