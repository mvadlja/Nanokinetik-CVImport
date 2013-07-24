using System;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;
using LocationManager = AspNetUIFramework.LocationManager;
using CacheManager = AspNetUIFramework.CacheManager;


namespace AspNetUI.Views
{
    public partial class ASView : FormHolder
    {
        MasterMain m;

        private SSIRepository SSIRep
        {
            get { return (SSIRepository)Session["SSIRepository"]; }
            set { Session["SSIRepository"] = value; }
        }

        // View initialization
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Request.QueryString["f"] != "dn" && Request.QueryString["f"] != "d") Response.Redirect("~/Views/ApprovedSubstanceView/List.aspx");

            m = (MasterMain)Page.Master;

            ASForm_details1.OnSaveButtonClick += ASForm_details1_OnSaveButtonClick;
            ASForm_details1.OnCancelButtonClick += ASForm_details1_OnCancelButtonClick;

            if (m != null)
            {
                XmlLocation xmlLocation = LocationManager.Instance.GetLocationByName("Level3-ApprovedSubstance", CacheManager.Instance.AppLocations);
                if (xmlLocation != null)
                {
                    var topLevelParent = m.FindTopLevelParent(xmlLocation);
                    m.CurrentLocation = xmlLocation;
                    m.TopMenu.GenerateNewTopMenu(CacheManager.Instance.AppLocations, topLevelParent, xmlLocation);
                }
            }
        }

        void ASForm_details1_OnCancelButtonClick(object sender, FormDetailsEventArgs e)
        {
            DoCancelAction();
        }

        void ASForm_details1_OnSaveButtonClick(object sender, FormDetailsEventArgs e)
        {
            DoSaveAction();
        }

        public override void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                case ContextMenuEventTypes.Back:
                    Response.Redirect("~/Views/ApprovedSubstanceView/List.aspx");
                    break;

                case ContextMenuEventTypes.Save:
                    DoSaveAction();
                    break;

                case ContextMenuEventTypes.New:
                    ASForm_details1.ShowForm("");
                    ASForm_details1.BindForm(null, "");

                    MasterPage.ContextMenu.SetContextMenuItemsVisible(new[] { new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), new ContextMenuItem(ContextMenuEventTypes.Save, "Save") });
                    break;

                case ContextMenuEventTypes.Cancel:
                    DoCancelAction();
                    break;
            }
        }

        private void DoSaveAction()
        {
            if (ASForm_details1.ValidateForm(""))
            {
                string idAS = Request.QueryString["idAS"];
                object result = ASForm_details1.SaveForm(idAS, "");
                if (result != null)
                {
                    SSIRep.SaveASToDb((int)(result as Approved_substance_PK).approved_substance_PK);
                }

                Response.Redirect("~/Views/ApprovedSubstanceView/List.aspx");
            }
        }

        private void DoCancelAction()
        {
            Response.Redirect("~/Views/ApprovedSubstanceView/List.aspx");
        }

        public override void ShowSelectedForm()
        {
            string id = Request.QueryString["idAS"] ?? Request.QueryString["id"];
            if (Request.QueryString["f"] != null)
            {
                switch (Request.QueryString["f"])
                {
                    case "d":
                    case "dn": 
                        ASForm_details1.ShowForm("");
                        ASForm_details1.BindForm(id, "");

                        MasterPage.ContextMenu.SetContextMenuItemsVisible(new[] { new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), new ContextMenuItem(ContextMenuEventTypes.Save, "Save") });
                        break;
                }
            }
        }
    }
}