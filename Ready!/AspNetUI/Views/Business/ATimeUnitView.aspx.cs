using System;
using System.Web.UI;
using AspNetUIFramework;
using Ready.Model;
using CacheManager = AspNetUIFramework.CacheManager;

namespace AspNetUI.Views
{
    public partial class ATimeUnitView : FormHolder
    {
        MasterMain m = null;
        IActivity_PKOperations _activityOperations;
        ITime_unit_PKOperations _time_unit_PKOperations;

        private string _idLay;

        // View initialization
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Request.QueryString["f"] == null) Response.Redirect("~/Views/ActivityView/List.aspx");

            m = (MasterMain)Page.Master;
            _idLay = !string.IsNullOrWhiteSpace(Request["idLay"]) ? "idLay=" + Request["idLay"] : string.Empty;

            if (!IsPostBack)
            {
                hideBorderRow(Page.Controls, 0);
            }

            if (Request.QueryString["f"] != null)
            {

                if (Request.QueryString["idAct"] != null)
                {
                    tabMenu.GenerateLevel4TabItemsByRights(CacheManager.Instance.AppLocations, m.CurrentLocation, "l", Request.QueryString["idAct"].ToString());
                }
                else if (Request.QueryString["id"] != null)
                {
                    tabMenu.GenerateLevel4TabItemsByRights(CacheManager.Instance.AppLocations, m.CurrentLocation, "l", Request.QueryString["id"]);
                }
                else
                {
                    tabMenu.GenerateLevel4TabItemsByRights(CacheManager.Instance.AppLocations, m.CurrentLocation, "l", "1");
                }

                tabMenu.SelectItem(m.CurrentLocation, CacheManager.Instance.AppLocations);
            }
            _activityOperations = new Activity_PKDAL();
            _time_unit_PKOperations = new Time_unit_PKDAL();

            ATimeUnitForm_details1.OnSaveButtonClick += new EventHandler<FormDetailsEventArgs>(ATimeUnitForm_details1_OnSaveClick);
            ATimeUnitForm_details1.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(ATimeUnitForm_details1_OnCancelClick);
        }

        public void ATimeUnitForm_details1_OnSaveClick(object sender, FormDetailsEventArgs e)
        {
            SaveAndRedirect();
        }

        public void ATimeUnitForm_details1_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            DoCancelAction();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Request.QueryString["f"] != null && Request.QueryString["f"] == "sa")
                ShowSelectedForm();
        }

        public override void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            string idAct = Request.QueryString["idAct"];

            switch (e.EventType)
            {
                // Back
                case ContextMenuEventTypes.Back:
                    if (ValidationHelper.IsValidInt(idAct))
                    {
                        Response.Redirect(string.Format("~/Views/ActivityView/List.aspx{0}", (!string.IsNullOrWhiteSpace(_idLay) ? "?" + _idLay : string.Empty)));
                    }


                    break;
                // Edit form
                case ContextMenuEventTypes.Edit:
                    ATimeUnitForm_details1.ShowForm("");
                    ATimeUnitForm_details1.BindForm(Request.QueryString["idtu"], "");

                    MasterPage.ContextMenu.SetContextMenuItemsVisible(new[] { new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), new ContextMenuItem(ContextMenuEventTypes.Save, "Save entity") });

                    break;
                // Start new entity 
                case ContextMenuEventTypes.New:
                    if (ValidationHelper.IsValidInt(idAct))
                    {
                        Response.Redirect(string.Format("~/Views/Business/ATimeUnitView.aspx?f=d&idAct={0}", idAct));
                    }

                    break;
                // Save current entity
                case ContextMenuEventTypes.Save:
                    SaveAndRedirect();

                    break;

                //Save As
                case ContextMenuEventTypes.SaveAs:
                    if (!string.IsNullOrEmpty(Request.QueryString["idtu"]))
                    {
                        Response.Redirect("~/Views/Business/ATimeUnitView.aspx?f=sa&sa=1&idtu=" + Request.QueryString["idtu"] + "&idAct=" + Request.QueryString["idAct"]);
                    }
                    break;
                // Cancel operation
                case ContextMenuEventTypes.Cancel:

                    DoCancelAction();

                    break;

                case ContextMenuEventTypes.Delete:
                    if (Request.QueryString["idtu"] != null)
                    {
                        _time_unit_PKOperations.Delete(Convert.ToInt32(Request.QueryString["idtu"]));

                        if (Request.QueryString["idAct"] != null)
                        {
                            Response.Redirect("~\\Views\\Business\\ATimeUnitView.aspx?f=l&idAct=" + Request.QueryString["idAct"]);
                        }
                        else
                        {
                            Response.Redirect("~\\Views\\Business\\ATimeUnitView.aspx?f=l");
                        }
                    }

                    break;
            }

            Response.Redirect(string.Format("~/Views/ActivityView/List.aspx{0}", (!string.IsNullOrWhiteSpace(_idLay) ? "?" + _idLay : string.Empty)));
        }

        private void SaveAndRedirect()
        {
            if (ATimeUnitForm_details1.ValidateForm(""))
            {
                string idtu = Request.QueryString["idtu"] == null || Request.QueryString["f"] == "dn" ? null : Request.QueryString["idtu"];
                var result = ATimeUnitForm_details1.SaveForm(idtu, "");

                if (result != null)
                {
                    Response.Redirect("~/Views/Business/TimeUnitProperties.aspx?f=p&idtu=" + ((Time_unit_PK)result).time_unit_PK);
                }
                Response.Redirect(string.Format("~/Views/ActivityView/List.aspx{0}", (!string.IsNullOrWhiteSpace(_idLay) ? "?" + _idLay : string.Empty)));
            }
        }

        // Displays correct form 
        public override void ShowSelectedForm()
        {
            String idAct = Request.QueryString["idAct"];
            if (Request.QueryString["f"] != null)
            {
                if (!string.IsNullOrEmpty(idAct))
                {
                    Activity_PK act = _activityOperations.GetEntity(Int32.Parse(Request.QueryString["idAct"]));
                    if (act != null)
                    {
                        lblName.Visible = true;
                        lblName.ControlValue = act.name;
                    }
                }
                switch (Request.QueryString["f"])
                {
                    case "l": // list view

                        ATimeUnitForm_details1.HideForm("");

                        MasterPage.ContextMenu.SetContextMenuItemsVisible(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New Entry"), new ContextMenuItem(ContextMenuEventTypes.Back, "Back") });

                        break;
                    case "d": // details view                      
                        ATimeUnitForm_details1.ShowForm("");
                        ATimeUnitForm_details1.BindForm(null, "");
                        MasterPage.ContextMenu.SetContextMenuItemsVisible(new[] { new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), new ContextMenuItem(ContextMenuEventTypes.Save, "Save") });

                        break;
                    case "sa": // save as view  
                        if (!string.IsNullOrEmpty(idAct))
                        {
                            Activity_PK act = _activityOperations.GetEntity(Int32.Parse(Request.QueryString["idAct"]));
                            if (act != null)
                            {
                                lblName.Visible = true;
                                lblName.ControlValue = act.name;
                            }
                        }
                        ATimeUnitForm_details1.ShowForm("");
                        ATimeUnitForm_details1.BindForm(Request.QueryString["idtu"].ToString(), "sa");
                        
                        MasterPage.ContextMenu.SetContextMenuItemsVisible(new[] { new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), new ContextMenuItem(ContextMenuEventTypes.Save, "Save") });

                        break;
                    case "dn": // new entry                      
                        ATimeUnitForm_details1.ShowForm("");
     
                        ATimeUnitForm_details1.BindForm(null, "");
                        
                        MasterPage.ContextMenu.SetContextMenuItemsVisible(new[] { new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), new ContextMenuItem(ContextMenuEventTypes.Save, "Save") });

                        break;
                    case "p": // preview      

                        if (!string.IsNullOrEmpty(idAct))
                        {
                            Activity_PK act = _activityOperations.GetEntity(Int32.Parse(idAct));
                            if (act != null)
                            {
                                lblName.Visible = true;
                                lblName.ControlValue = act.name;
                            }
                        }

                        ATimeUnitForm_details1.ShowForm("");

                        ATimeUnitForm_details1.BindForm("p" + Request.QueryString["idtu"], "");
                        
                        MasterPage.ContextMenu.SetContextMenuItemsVisible(new[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit"), new ContextMenuItem(ContextMenuEventTypes.Back, "Back"), new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });

                        break;
                }

            }
        }

        private void hideBorderRow(ControlCollection controls, int depth)
        {
            foreach (Control control in controls)
            {
                if (control.ID == "level2TabsRowBorder") control.Visible = false;
                hideBorderRow(control.Controls, depth + 1);
            }
        }

        private void DoCancelAction()
        {
            var idAct = Request.QueryString["idAct"];
            var f = Request.QueryString["f"];
            var from = Request.QueryString["From"];

            if (f == "d")
            {
                switch (from)
                {
                    case "ActList":
                        Response.Redirect(string.Format("~/Views/ActivityView/List.aspx{0}", (!string.IsNullOrWhiteSpace(_idLay) ? "?" + _idLay : string.Empty)));
                        break;
                    case "ActSearch":
                        Response.Redirect("~/Views/ActivityView/List.aspx?Action=Search");
                        break;
                }

                if (ValidationHelper.IsValidInt(idAct))
                {
                    Response.Redirect(string.Format("~/Views/Business/ATimeUnitView.aspx?f=l&idAct={0}", idAct));
                }
            }
            else if (f == "dn")
            {
                switch (from)
                {
                    case "TimeUnit-ActPreview":
                        var idTimeUnit = Request.QueryString["idTimeUnit"] ?? Request.QueryString["idtu"];
                        if (ValidationHelper.IsValidInt(idTimeUnit))
                        {
                            Response.Redirect(string.Format("~/Views/Business/APropertiesView.aspx?f=l&idtu={0}", idTimeUnit));
                        }
                        break;
                    case "SubUnit-ActPreview":
                        var idSubUnit = Request.QueryString["idSubUnit"] ?? Request.QueryString["suid"]; 
                            if (ValidationHelper.IsValidInt(idAct) && ValidationHelper.IsValidInt(idSubUnit))
                            {
                                Response.Redirect(string.Format("~/Views/Business/SUAPropertiesView.aspx?f=l&suid={0}&idAct={1}", idSubUnit, idAct));
                            }
                        break;
                    case "ActPreview":
                        if (ValidationHelper.IsValidInt(idAct))
                        {
                            Response.Redirect(string.Format("~/Views/Business/APropertiesView.aspx?f=l&idAct={0}{1}", idAct, (!string.IsNullOrWhiteSpace(_idLay) ? "&" + _idLay : string.Empty)));
                          
                        }
                        break;
                }
            }

            Response.Redirect(string.Format("~/Views/ActivityView/List.aspx{0}", (!string.IsNullOrWhiteSpace(_idLay) ? "?" + _idLay : string.Empty)));
        }
    }
}