using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using GEM2Common;
using CommonTypes;
using Ready.Model;
using AspNetUI.Support;
using LocationManager = AspNetUIFramework.LocationManager;
using CacheManager = AspNetUIFramework.CacheManager;

namespace AspNetUI.Views
{
    public partial class ASProperties : FormHolder
    {
        MasterMain m = null;
        IApproved_substance_PKOperations _approvedSubstanceOperations;

        private SSIRepository SSIRep
        {
            get { return (SSIRepository)Session["SSIRepository"]; }
            set { Session["SSIRepository"] = value; }
        }

        // View initialization
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Request.QueryString["f"] == null)
            {
                Response.Redirect("~/Views/Operational/ASProperties.aspx?f=l");
            }

            m = (MasterMain)Page.Master;
            _approvedSubstanceOperations = new Approved_substance_PKDAL();

            if (Request.QueryString["f"] != null && Request.QueryString["idAS"] != null)
            {
                tabMenu.GenerateLevel4TabItemsByRights(CacheManager.Instance.AppLocations, m.CurrentLocation, "p", Request.QueryString["idAS"]);
                tabMenu.SelectItem(m.CurrentLocation, CacheManager.Instance.AppLocations);
            }

            ASForm_details1.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(ASForm_details1_OnCancelButtonClick);
            ASForm_details1.OnSaveButtonClick += new EventHandler<FormDetailsEventArgs>(ASForm_details1_OnSaveButtonClick);
        }

        void ASForm_details1_OnSaveButtonClick(object sender, FormDetailsEventArgs e)
        {
            DoSaveAction();
        }

        void ASForm_details1_OnCancelButtonClick(object sender, FormDetailsEventArgs e)
        {
            DoCancelAction();
        }

        // Context menu click handler for this form
        public override void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {

            switch (e.EventType)
            {
                // Back
                case ContextMenuEventTypes.Back:
                    //XmlLocation parentLocation = LocationManager.Instance.GetLocationByName(m.CurrentLocation.ParentLocationID, CacheManager.Instance.AppLocations);

                    //Response.Redirect(parentLocation.LocationUrl);
                    Response.Redirect("~/Views/Operational/ASView.aspx?f=l");
                    break;

                case ContextMenuEventTypes.Save:
                    DoSaveAction();
                    break;

                case ContextMenuEventTypes.New:
                    ASForm_list1.HideForm("");
                    ASForm_details1.ShowForm("");
                    ASForm_details1.BindForm(null, "");

                    MasterPage.ContextMenu.SetContextMenuItemsVisible(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), new ContextMenuItem(ContextMenuEventTypes.Save, "Save") });

                    break;
                case ContextMenuEventTypes.Edit:
                    string idASEdit = Request.QueryString["idAS"];
                    ASForm_list1.HideForm("");
                    ASForm_details1.ShowForm("");
                    ASForm_details1.BindForm(idASEdit, "");

                    MasterPage.ContextMenu.SetContextMenuItemsVisible(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), new ContextMenuItem(ContextMenuEventTypes.Save, "Save") });

                    break;

                case ContextMenuEventTypes.Cancel:
                    DoCancelAction();
                    break;

                default:
                    break;
            }
        }

        private void DoSaveAction()
        {
            object result;
            if (ASForm_details1.ValidateForm(""))
            {
                string idAS = Request.QueryString["idAS"];
                result = ASForm_details1.SaveForm(idAS, "");
                if (result != null)
                {
                    SSIRep.SaveASToDb((int)(result as Approved_substance_PK).approved_substance_PK);
                }

                tabMenuContainer.Visible = false;
                ASForm_list1.ShowForm("");
                ASForm_details1.HideForm("");
                ASForm_list1.BindForm("", "");
                MasterPage.ContextMenu.SetContextMenuItemsVisible(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit"), new ContextMenuItem(ContextMenuEventTypes.Back, "Back") });
            }
        }

        private void DoCancelAction()
        {
            string idAS1 = Request.QueryString["idAS"];
            if (!string.IsNullOrEmpty(idAS1))
            {
                Response.Redirect("~/Views/Operational/ASProperties.aspx?f=l&idAS=" + idAS1);
            }
            else
            {
                Response.Redirect("~/Views/Operational/ASView.aspx?f=l");
            }

            tabMenuContainer.Visible = false;
            ASForm_list1.ShowForm("");
            ASForm_details1.HideForm("");
            ASForm_list1.BindForm("", "");
            MasterPage.ContextMenu.SetContextMenuItemsVisible(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.New, "New entry") });
        }

        // Displays correct form 
        public override void ShowSelectedForm()
        {
            if (Request.QueryString["f"] != null)
            {
                switch (Request.QueryString["f"])
                {
                    case "l":
                        if (!string.IsNullOrEmpty(Request.QueryString["idAS"]))
                        {
                            var aSubstance = _approvedSubstanceOperations.GetEntity(Request.QueryString["idAS"]);
                            if (aSubstance != null)
                            {
                                lblName.Visible = true;
                                lblName.ControlValue = aSubstance.substancename;
                            }
                        }
                        ASForm_list1.ShowForm("");
                        ASForm_details1.HideForm("");
                        ASForm_list1.BindForm("", "");
                        MasterPage.ContextMenu.SetContextMenuItemsVisible(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit"), new ContextMenuItem(ContextMenuEventTypes.Back, "Back") });
                        break;

                    case "d": // details edit view
                        string idAS = Request.QueryString["idAS"];
                        ASForm_list1.HideForm("");
                        ASForm_details1.ShowForm("");
                        ASForm_details1.BindForm(idAS, "");

                        MasterPage.ContextMenu.SetContextMenuItemsVisible(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), new ContextMenuItem(ContextMenuEventTypes.Save, "Save") });

                        break;

                    case "dn": // details new view
                        ASForm_list1.HideForm("");
                        ASForm_details1.ShowForm("");
                        ASForm_details1.BindForm(null, "");

                        MasterPage.ContextMenu.SetContextMenuItemsVisible(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), new ContextMenuItem(ContextMenuEventTypes.Save, "Save") });

                        break;
                }
            }
        }


    }
}