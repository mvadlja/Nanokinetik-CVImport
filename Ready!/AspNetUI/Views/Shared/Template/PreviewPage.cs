using System;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUIFramework;

namespace AspNetUI.Views.Shared.Template
{
    public abstract class PreviewPage : DefaultPage
    {
        #region Properties

        public Panel PnlProperties;
        public Panel PnlFooter;
        public PreviewType PreviewType;

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            PageType = PageType.Preview;

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

            return false;
        }

        public virtual void SecurityPageSpecificMy(bool? isResponsibleUser)
        {
            var rootLocation = Support.LocationManager.Instance.GetLocationByName("Root", Support.CacheManager.Instance.AppLocations);
            if (rootLocation != null)
            {
                bool isPermittedEditMy = SecurityHelper.IsPermitted(Permission.EditMy, rootLocation) && isResponsibleUser.HasValue && isResponsibleUser.Value;
                if (isPermittedEditMy) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit") });

                if (!SecurityHelper.IsPermitted(Permission.CreateManualAlerts, rootLocation))
                {
                    bool isPermittedCreateManualAlertsMy = SecurityHelper.IsPermitted(Permission.CreateManualAlertsMy, rootLocation) && isResponsibleUser.HasValue && isResponsibleUser.Value;
                    if (isPermittedCreateManualAlertsMy) SecurityHelper.SetReminderControlsForReadWrite(PnlProperties);
                    else SecurityHelper.SetReminderControlsForRead(PnlProperties);
                }
                else SecurityHelper.SetReminderControlsForReadWrite(PnlProperties);

                bool isPermittedSaveAsMy = SecurityHelper.IsPermitted(Permission.SaveAsMy, rootLocation) && isResponsibleUser.HasValue && isResponsibleUser.Value;
                if (isPermittedSaveAsMy) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });

                bool isPermittedDeleteMy = SecurityHelper.IsPermitted(Permission.DeleteMy, rootLocation) && isResponsibleUser.HasValue && isResponsibleUser.Value;
                if (isPermittedDeleteMy) StyleHelper.EnableLinkButtonsWithCssClass(PnlFooter, "Delete");
            }
        }

        #endregion

        #region Page virtual methods

        public virtual void LoadActionQuery()
        {
            switch (Request.QueryString["Action"])
            {
                case "Preview":
                    PreviewType = PreviewType.Preview;
                    break;
                default:
                    PreviewType = PreviewType.Preview;
                    break;
            }
        }

        #endregion

        #region Support methods

        public void AssociatePanels(Panel pnlProperties, Panel pnlFooter)
        {
            PnlProperties = pnlProperties;
            PnlFooter = pnlFooter;
        }

        #endregion
    }
}