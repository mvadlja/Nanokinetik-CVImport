using System;
using System.Web.UI;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucExporter : UserControl
    {
        #region Properties

        public string ModalPopupContainerWidth
        {
            get { return PopupControls_Entity_Container.Style["Width"]; }
            set { PopupControls_Entity_Container.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return PopupControls_Entity_Container.Style["Height"]; }
            set { PopupControls_Entity_Container.Style["Height"] = value; }
        }

        public string ModalPopupContainerBodyPadding
        {
            get { return modalPopupContainerBody.Style["padding"]; }
            set { modalPopupContainerBody.Style["padding"] = value; }
        }

        public string ExportType
        {
            get { return ViewState["ExportType"] != null ? (string)ViewState["ExportType"] : String.Empty; }
            set { ViewState["ExportType"] = value; }
        }


        #endregion

        public event EventHandler<EventArgs> OnConfirmExport_Click;

        #region Operations

        public void ShowModalForm(string header)
        {
            divHeader.InnerHtml = header;

            PopupControls_Entity_Container.Style["display"] = "inline";

            divHeader.InnerHtml = header;
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            base.OnInit(e);

            btnXlsExport.Click += new ImageClickEventHandler(btnXlsExport_Click);
            btnXlsxExport.Click += new ImageClickEventHandler(btnXlsxExport_Click);
        }

        #endregion

        #region Form methods

        protected void btnClosePopupForm_Click(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
        }

        void btnXlsxExport_Click(object sender, ImageClickEventArgs e)
        {
            ExportType = "XLSX";
            if (OnConfirmExport_Click != null)
            {
                PopupControls_Entity_Container.Style["display"] = "none";
                OnConfirmExport_Click(sender, e);
            }
        }

        void btnXlsExport_Click(object sender, ImageClickEventArgs e)
        {
            ExportType = "XLS";
            if (OnConfirmExport_Click != null)
            {
                PopupControls_Entity_Container.Style["display"] = "none";
                OnConfirmExport_Click(sender, e);
            }
        }

        #endregion
    }
}
