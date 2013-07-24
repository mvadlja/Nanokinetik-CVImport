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

namespace AspNetUI.Views
{
    public partial class SSIView : FormHolder
    {
        MasterMain m = null;

        ISubstance_s_PKOperations _substance_s_PKOperations;

        private SSIRepository SSIRep
        {
            get { return (SSIRepository)Session["SSIRepository"]; }
            set { Session["SSIRepository"] = value; }
        }

        // View initialization
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Request.QueryString["f"] == null) Response.Redirect("~/Views/Business/SSIView.aspx?f=l");
            /*
            if (Request.QueryString["f"] != null)
            {
                if (Request.QueryString["f"].ToString() == "d")
                    if (Request.QueryString["id"].ToString() == null)
                        Response.Redirect("~/Views/Business/SSIView.aspx?f=d&id=1");
            }
            else 
            {
                Response.Redirect("~/Views/Business/SSIView.aspx?f=dn");    
            }
              */     
            m = (MasterMain)Page.Master;

            _substance_s_PKOperations = new Substance_s_PKDAL();
        }
        
        // Context menu click handler for this form
        public override void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";

            switch (e.EventType)
            {
                // Start new entity 
                case ContextMenuEventTypes.New:

                    SSIForm_details1.ShowForm("");
                    SSIForm_details1.BindForm(null, "");

                    MasterPage.ContextMenu.SetContextMenuItemsVisible(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), new ContextMenuItem(ContextMenuEventTypes.Save, "Save entity") });

                    break;
                // Save current entity
                case ContextMenuEventTypes.Save:

                    object result;

                    if (SSIForm_details1.ValidateForm(""))
                    {

                        result = SSIForm_details1.SaveForm(id, "");
                        SSIRep.SaveToDb(result as Substance_s_PK);

                        Response.Redirect("~\\Views\\Business\\SSIView.aspx?f=l");

                        SSIForm_details1.HideForm("");

                        MasterPage.ContextMenu.SetContextMenuItemsVisible(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.New, "New entry") });
                    }
                    break;
                // Cancel operation
                case ContextMenuEventTypes.Cancel:
                    Response.Redirect("~\\Views\\Business\\SSIView.aspx?f=l");

                    SSIForm_details1.HideForm("");

                    MasterPage.ContextMenu.SetContextMenuItemsVisible(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.New, "New entry") });

                    break;
                case ContextMenuEventTypes.Delete:
                    if (id != null)
                    {
                        Substance_s_PK substance = _substance_s_PKOperations.GetEntity(id);
                        if (substance != null)
                        {
                            SSIRep.DeleteAssociatedObjects(null);
                            SSIRep.SaveToDb(substance);
                            _substance_s_PKOperations.Delete(Convert.ToInt32(id));
                        }
                        Response.Redirect("~/Views/Business/SSIView.aspx?f=l");
                    }

                    break;
                default:

                    break;
            }
        }

        // Displays correct form 
        public override void ShowSelectedForm()
        {
            if (Request.QueryString["f"] != null)
            {
                switch (Request.QueryString["f"].ToString())
                {
                    case "l": // list view
                        SSIForm_details1.HideForm("");

                        MasterPage.ContextMenu.SetContextMenuItemsVisible(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.New, "New Entry") });
                     
                        break;
                    case "d": // details view        
                        string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";
                        SSIForm_details1.ShowForm("");
                        SSIForm_details1.BindForm(id, "");

                        MasterPage.ContextMenu.SetContextMenuItemsVisible(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), new ContextMenuItem(ContextMenuEventTypes.Save, "Save"), new ContextMenuItem(ContextMenuEventTypes.Delete, "Delete") });

                        break;
                    case "dn": // new entry                      
                        SSIForm_details1.ShowForm("");
                        SSIForm_details1.BindForm(null, "");

                        MasterPage.ContextMenu.SetContextMenuItemsVisible(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), new ContextMenuItem(ContextMenuEventTypes.Save, "Save") });

                        break;
                }

            }

        }


    }
}