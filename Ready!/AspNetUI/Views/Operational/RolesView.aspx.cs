using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using GEM2Common;
using CommonTypes;
using Kmis.Model;

namespace AspNetUI.Views
{
    public partial class RolesView : FormHolder
    {
        // View initialization
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Sets initial form
            InitialForm = RoleForm_list1.ID;

            // Subscribes to list item selected
            RoleForm_list1.OnListItemSelected += new EventHandler<FormListEventArgs>(RoleForm_list1_OnListItemSelected);
        }

        // Context menu click handler for this form
        public override void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                // Start new entity
                case ContextMenuEventTypes.New:
                    MasterPage.ViewStateController.RemoveAllEntityIDs();
                    MasterPage.ViewStateController.SelectedForm = RoleForm_details1.ID;
                    ShowSelectedForm();
                    break;
                // Save current entity
                case ContextMenuEventTypes.Save:
                    if (MasterPage.ViewStateController.SelectedForm == RoleForm_details1.ID)
                    {
                        object result = null;

                        // update
                        if (MasterPage.ViewStateController.CheckIfEntityExists(0))
                        {
                            if (RoleForm_details1.ValidateForm(MasterPage.ViewStateController.SelectedEntityIDs[0].ToString()))
                            {
                                result = RoleForm_details1.SaveForm(MasterPage.ViewStateController.SelectedEntityIDs[0], "");
                                MasterPage.ViewStateController.RemoveAllEntityIDs();
                                MasterPage.ViewStateController.SelectedForm = RoleForm_list1.ID;
                                ShowSelectedForm();
                            }
                        }
                        // insert
                        else
                        {
                            if (RoleForm_details1.ValidateForm(""))
                            {
                                result = RoleForm_details1.SaveForm(null, "");
                                MasterPage.ViewStateController.RemoveAllEntityIDs();
                                MasterPage.ViewStateController.SelectedForm = RoleForm_list1.ID;
                                ShowSelectedForm();
                            }
                        }
                    }
                    break;
                // Cancel operation
                case ContextMenuEventTypes.Cancel:
                    if (MasterPage.ViewStateController.SelectedForm == RoleForm_details1.ID)
                    {
                        MasterPage.ViewStateController.RemoveAllEntityIDs();
                        MasterPage.ViewStateController.SelectedForm = RoleForm_list1.ID;
                        ShowSelectedForm();
                    }
                    break;
                default:
                    break;
            }
        }

        // Displays correct form (from MasterPage.ViewStateController.SelectedForm property)
        public override void ShowSelectedForm()
        {
            if (MasterPage.ViewStateController.SelectedForm == RoleForm_list1.ID)
            {
                RoleForm_list1.ShowForm("");
                RoleForm_details1.HideForm("");

                RoleForm_list1.BindForm(null, "");

                // Sets context menu items for this form
                MasterPage.ContextMenu.SetContextMenuItemsVisible(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });
            }
            else if (MasterPage.ViewStateController.SelectedForm == RoleForm_details1.ID)
            {
                RoleForm_list1.HideForm("");

                // item selected
                if (MasterPage.ViewStateController.CheckIfEntityExists(0))
                {
                    RoleForm_details1.ShowForm(MasterPage.ViewStateController.SelectedEntityIDs[0].ToString());
                    RoleForm_details1.BindForm(MasterPage.ViewStateController.SelectedEntityIDs[0], "");
                }
                // new item
                else
                {
                    RoleForm_details1.ShowForm("");
                    RoleForm_details1.BindForm(null, "");
                }

                MasterPage.ContextMenu.SetContextMenuItemsVisible(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save entity"), new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel") });
            }
        }

        // List item selected handler (opens existing entity details)
        void RoleForm_list1_OnListItemSelected(object sender, FormListEventArgs e)
        {
            MasterPage.ViewStateController.RemoveAllEntityIDs();
            MasterPage.ViewStateController.AddSelectedEntityID(e.DataItemID);
            MasterPage.ViewStateController.SelectedForm = RoleForm_details1.ID;
            ShowSelectedForm();
        }
    }
}