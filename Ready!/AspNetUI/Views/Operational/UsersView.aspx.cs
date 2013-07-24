using System;
using AspNetUIFramework;

namespace AspNetUI.Views
{
    public partial class UsersView : FormHolder
    {
        MasterMain m;

        // View initialization
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            m = (MasterMain)Page.Master;

            if (m != null)
            {
                XmlLocation xmlLocation = LocationManager.Instance.GetLocationByName("Level3-User", CacheManager.Instance.AppLocations);
                if (xmlLocation != null)
                {
                    var topLevelParent = m.FindTopLevelParent(xmlLocation);
                    m.CurrentLocation = xmlLocation;
                    m.TopMenu.GenerateNewTopMenu(CacheManager.Instance.AppLocations, topLevelParent, xmlLocation);
                }
            }
        }

        // Context menu click handler for this form
        public override void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            string id = Request.QueryString["id"];

            switch (e.EventType)
            {
                // Back
                case ContextMenuEventTypes.Back:
                    Response.Redirect("~/Views/UserView/List.aspx");

                    break;
                // New entry
                case ContextMenuEventTypes.New:

                    UserForm_details1.ShowForm("");
                    UserForm_details1.BindForm(null, "");

                    MasterPage.ContextMenu.SetContextMenuItemsVisible(new[] { new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), new ContextMenuItem(ContextMenuEventTypes.Save, "Save entity") });

                    break;

                // Save current entity 
                case ContextMenuEventTypes.Save:
                    if (UserForm_details1.ValidateForm(""))
                    {
                        string tid = Request.QueryString["id"];

                        if (tid != "") { UserForm_details1.SaveForm(tid, ""); }
                        else { UserForm_details1.SaveForm(null, ""); }

                        Response.Redirect("~/Views/UserView/List.aspx");
                    }

                    break;

                // Cancel operation
                case ContextMenuEventTypes.Cancel:
                    Response.Redirect("~/Views/UserView/List.aspx");

                    break;
            }
        }

        // Displays correct form (from MasterPage.ViewStateController.SelectedForm property)
        public override void ShowSelectedForm()
        {
            if (Request.QueryString["f"] != null)
            {
                switch (Request.QueryString["f"])
                {
                    case "l": // list view
                        tabMenuContainer.Visible = false;
                        UserForm_details1.HideForm("");

                        MasterPage.ContextMenu.SetContextMenuItemsVisible(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New entry") });

                        break;
                    case "d": // details view                      
                        UserForm_details1.ShowForm("");
                        UserForm_details1.BindForm(Request.QueryString["id"], "");

                        MasterPage.ContextMenu.SetContextMenuItemsVisible(new[] { new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), new ContextMenuItem(ContextMenuEventTypes.Save, "Save") });

                        break;
                    case "dn": // details view new entry   
                        UserForm_details1.ShowForm("");
                        UserForm_details1.BindForm(null, "");

                        MasterPage.ContextMenu.SetContextMenuItemsVisible(new[] { new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), new ContextMenuItem(ContextMenuEventTypes.Save, "Save") });

                        break;
                }
            }
            else
            {
                tabMenuContainer.Visible = false;
                UserForm_details1.HideForm("");

                MasterPage.ContextMenu.SetContextMenuItemsVisible(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New entry") });
            }
        }
    }
}