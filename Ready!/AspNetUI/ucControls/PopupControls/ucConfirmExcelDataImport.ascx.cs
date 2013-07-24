using System;
using System.Data;
using System.Web.UI;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucConfirmExcelDataImport : UserControl
    {
        public event EventHandler<EventArgs> OnExcelDataImportBtnConfirm_Click;

        #region Properties

        public string ModalPopupContainerWidth
        {
            get { return PopupControls_ConfirmExcelDataImport_Container.Style["Width"]; }
            set { PopupControls_ConfirmExcelDataImport_Container.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return PopupControls_ConfirmExcelDataImport_Container.Style["Height"]; }
            set { PopupControls_ConfirmExcelDataImport_Container.Style["Height"] = value; }
        }

        public string ModalPopupContainerBodyPadding
        {
            get { return modalPopupContainerBody.Style["padding"]; }
            set { modalPopupContainerBody.Style["padding"] = value; }
        }

        private string ImportDataSessionID
        {
            get { return ViewState["ImportDataSessionID"] != null ? (string)ViewState["ImportDataSessionID"] : String.Empty; }
            set { ViewState["ImportDataSessionID"] = value; }
        }

        #endregion

        #region Operations

        public void ShowModalPopup(string importDataSessionID)
        {
            if (!String.IsNullOrEmpty(importDataSessionID))
            {
                if (Session[importDataSessionID] != null)
                {
                    ImportDataSessionID = importDataSessionID;
                    DataTable importData = ((DataTable) Session[importDataSessionID]);
                    DataTable importDataForConfirmation;
                    if (importData.Rows != null && importData.Rows.Count > 5)
                    {
                        importDataForConfirmation = importData.Clone();
                        for (int i = 0; i < 5; i++)
                        {
                            importDataForConfirmation.ImportRow(importData.Rows[i]);
                        }
                    }
                    else
                    {
                        importDataForConfirmation = importData.Copy();
                    }

                    gvData.AutoGenerateColumns = true;
                    gvData.DataSource = importDataForConfirmation;
                    gvData.DataBind();
                }
            }

            PopupControls_ConfirmExcelDataImport_Container.Style["display"] = "inline";
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_ConfirmExcelDataImport_Container.Style["display"] = "none";

            base.OnInit(e);
        }

        #endregion

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (OnExcelDataImportBtnConfirm_Click != null)
            {
                PopupControls_ConfirmExcelDataImport_Container.Style["display"] = "none";
                OnExcelDataImportBtnConfirm_Click(sender, e);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session[ImportDataSessionID] = null;
            PopupControls_ConfirmExcelDataImport_Container.Style["display"] = "none";
        }
    }
}