using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUIFramework;

namespace AspNetUI.Views.Shared.Template
{
    public abstract class FormPage : DefaultPage
    {
        #region Properties

        public Panel PnlForm;
        public Panel PnlFooter;
        public FormType FormType;

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            PageType = PageType.Form;

            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        #endregion

        #region Page override methods

        /// <summary>
        /// Securifies page specifics
        /// </summary>
        /// <returns>True, if page is secured, i.e. page security is resolved (enabled/disabled actions). False, if page security is yet to be determined.</returns>
        public override bool SecurityPageSpecific()
        {
            base.SecurityPageSpecific();
           
            var rootLocation = Support.LocationManager.Instance.GetLocationByName("Root", Support.CacheManager.Instance.AppLocations);
            if (rootLocation != null)
            {
                if (!SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation)) StyleHelper.DisableLinkButtonsWithCssClass(PnlFooter, "downloadAttachment");
                else StyleHelper.EnableLinkButtonsWithCssClass(PnlFooter, "downloadAttachment");
            }
            
            return false;
        }

        public virtual void SecurityPageSpecificMy(bool? isResponsibleUser)
        {
            var rootLocation = Support.LocationManager.Instance.GetLocationByName("Root", Support.CacheManager.Instance.AppLocations);
            if (rootLocation != null)
            {   
                bool isPermittedEditMy = SecurityHelper.IsPermitted(Permission.EditMy, rootLocation) && isResponsibleUser.HasValue && isResponsibleUser.Value;
                if (isPermittedEditMy)
                {
                    SecurityHelper.SetControlsForReadWrite(
                                  MasterPage.ContextMenu,
                                  new[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save") },
                                  new List<Panel> { PnlForm },
                                  new Dictionary<Panel, List<string>> { { PnlFooter, new List<string> { "Save" } } }
                              );
                }
               
                bool isPermittedSaveAsMy = SecurityHelper.IsPermitted(Permission.SaveAsMy, rootLocation) && isResponsibleUser.HasValue && isResponsibleUser.Value;
                if (isPermittedSaveAsMy)
                {
                    MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });
                }
                
                if (!SecurityHelper.IsPermitted(Permission.CreateManualAlerts, rootLocation))
                {
                    bool isPermittedCreateManualAlertsMy = SecurityHelper.IsPermitted(Permission.CreateManualAlertsMy, rootLocation) && isResponsibleUser.HasValue && isResponsibleUser.Value;
                    if (isPermittedCreateManualAlertsMy) SecurityHelper.SetReminderControlsForReadWrite(PnlForm);
                    else SecurityHelper.SetReminderControlsForRead(PnlForm);
                }
                else SecurityHelper.SetReminderControlsForReadWrite(PnlForm);
                
            }
        }

        public override void LoadFormVariables()
        {
            PageType = PageType.Form;

            base.LoadFormVariables();
        }

        public override object DeleteItem(object arg)
        {
            return base.DeleteItem(arg);
        }

        #endregion

        #region Page virtual methods

        public virtual void LoadActionQuery()
        {
            switch (Request.QueryString["Action"])
            {
                case "New":
                    FormType = FormType.New;
                    break;
                case "Edit":
                    FormType = FormType.Edit;
                    break;
                case "SaveAs":
                    FormType = FormType.SaveAs;
                    break;
            }
        }

        #endregion

        #region Support methods

        public void AssociatePanels(Panel pnlForm, Panel pnlFooter)
        {
            PnlForm = pnlForm;
            PnlFooter = pnlFooter;
        }

        #endregion
    }
}