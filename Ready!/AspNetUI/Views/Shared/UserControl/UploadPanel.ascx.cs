using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Interface;
using AjaxControlToolkit;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class UploadPanel : System.Web.UI.UserControl, ILastChange, IArticle57Relevant, IXevprmValidationError
    {
        #region Declarations

        public event EventHandler<EventArgs> OnDelete;
        public event EventHandler<EventArgs> OnCheckOutInCancel;

        #endregion

        #region Properties

        public System.Web.UI.HtmlControls.HtmlGenericControl DivUploadPanel
        {
            get { return divUploadPanel; }
        }

        public System.Web.UI.HtmlControls.HtmlGenericControl DivHrHolder
        {
            get { return divHrHolder; }
        }

        public Label LblName
        {
            get { return lblName; }
        }

        public Label LblError
        {
            get { return lblError; }
        }

        public string Label
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        public bool Required
        {
            get { return spanRequired.Visible; }
            set { spanRequired.Visible = value; }
        }

        public Unit LabelWidth
        {
            get { return lblName.Width; }
            set { lblName.Width = value; }
        }

        public string ValidationError
        {
            get { return lblError.Text; }
            set { lblError.Text = value; }
        }

        public List<int> OldValue
        {
            get { return ViewState["OldValue"] != null ? (List<int>)ViewState["OldValue"] : null; }
            set { ViewState["OldValue"] = value; }
        }

        public GridView GvData
        {
            get { return gvData; }
        }

        public Panel PnlUploadFilesMain
        {
            get { return pnlUploadFilesMain; }
        }

        public Panel PnlUploadedFiles
        {
            get { return pnlUploadedFiles; }
        }

        public Panel PnlAsyncUploadControl
        {
            get { return pnlAsyncUploadControl; }
        }

        public Panel PnlThrobber
        {
            get { return pnlThrobber; }
        }

        public AsyncFileUpload AsyncFileUpload
        {
            get { return asyncFileUpload; }
        }

        public bool IsModified
        {
            get
            {
                return !ListOperations.ListsEquals(AttachmentIdOldValue, AttachmentIdNewValue);
            }
        }

        public Guid AttachmentSessionId
        {
            get
            {
                if (ViewState["AttachmentSessionId"] == null)
                {
                    ViewState["AttachmentSessionId"] = Guid.NewGuid();
                }

                return (Guid)ViewState["AttachmentSessionId"];
            }
        }

        public bool RefreshAttachments
        {
            get { return ViewState["RefreshAttachments"] == null || (bool)ViewState["RefreshAttachments"]; }
            set { ViewState["RefreshAttachments"] = value; }
        }

        public int NumberOfUploadedAttachments
        {
            get { return ViewState["NumberOfUploadedAttachments"] != null ? (int)ViewState["NumberOfUploadedAttachments"] : 0; }
            set { ViewState["NumberOfUploadedAttachments"] = value; }
        }

        public List<int> AttachmentIdOldValue
        {
            get { return ViewState["AttachmentIdOldValue"] != null ? (List<int>)ViewState["AttachmentIdOldValue"] : null; }
            set { ViewState["AttachmentIdOldValue"] = value; }
        }

        public List<int> AttachmentIdNewValue
        {
            get { return ViewState["AttachmentIdNewValue"] != null ? (List<int>)ViewState["AttachmentIdNewValue"] : null; }
            set { ViewState["AttachmentIdNewValue"] = value; }
        }

        public List<int> AttachmentPkListToDelete
        {
            get { return ViewState["AttachmentPkListToDelete"] != null ? (List<int>)ViewState["AttachmentPkListToDelete"] : new List<int>(); }
            set { ViewState["AttachmentPkListToDelete"] = value; }
        }

        public bool IsEmpty
        {
            get { return NumberOfUploadedAttachments == 0; }
        }

        public Dictionary<int,int> AttachmentCheckInAttachmentId
        {
            get { return ViewState["AttachmentCheckInAttachmentId"] != null ? (Dictionary<int, int>)ViewState["AttachmentCheckInAttachmentId"] : new Dictionary<int, int>(); }
            set { ViewState["AttachmentCheckInAttachmentId"] = value; }
        }

        #endregion

        #region Page methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                RefreshAttachments = true;
            }
        }

        #endregion

        #region Event handlers

        protected void ImgBtnDeleteAttachment_Click(object sender, ImageClickEventArgs e)
        {
            if (OnDelete != null) OnDelete(sender, e);
        }

        #endregion

        #region Support methods

        public string HandleDocumentArguments(object attachmentPk, object EDMSDocumentId, object EDMSBindingRule, object EDMSAttachmentFormat)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(EDMSDocumentId))) return Convert.ToString(attachmentPk);
            else
            {
                return string.Format("{0}|{{{1};{2};{3}}}", attachmentPk, EDMSDocumentId, EDMSBindingRule, EDMSAttachmentFormat);
            }
        }

        #endregion
    }
}