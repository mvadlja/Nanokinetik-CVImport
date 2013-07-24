using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUIFramework;
using xEVMPD;
using Ready.Model;
using System.Configuration;
using System.IO;
using EVMessage.Xevprm;

namespace AspNetUI.Views.Shared.UserControl.Popup
{
    public partial class XevprmValidationErrorPopup : System.Web.UI.UserControl
    {
        #region Declarations

        public virtual event EventHandler<FormEventArgs<int>> OnOkButtonClick;
        public virtual event EventHandler<EventArgs> OnCancelButtonClick;

        public virtual event EventHandler<FormEventArgs<int>> OnValidationSuccessful;

        #endregion


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

        private int XevprmMessagePk
        {
            get { return (int)ViewState["XevprmValidationErrorPopup_XevprmMessagePk"]; }
            set { ViewState["XevprmValidationErrorPopup_XevprmMessagePk"] = value; }
        }

        private XevprmEntityType XevprmEntityType
        {
            get { return (XevprmEntityType)ViewState["XevprmValidationErrorPopup_XevprmEntityType"]; }
            set { ViewState["XevprmValidationErrorPopup_XevprmEntityType"] = value; }
        }

        private int? XevprmEntityPk
        {
            get { return (int?)ViewState["XevprmValidationErrorPopup_XevprmEntityPk"]; }
            set { ViewState["XevprmValidationErrorPopup_XevprmEntityPk"] = value; }
        }

        private bool IsValidationSuccessful
        {
            get { return (bool)ViewState["XevprmValidationErrorPopup_IsValidationSuccessful"]; }
            set { ViewState["XevprmValidationErrorPopup_IsValidationSuccessful"] = value; }
        }

        private bool ValidationLinksEnabled
        {
            get { return (bool)ViewState["XevprmValidationErrorPopup_ValidationLinksEnabled"]; }
            set { ViewState["XevprmValidationErrorPopup_ValidationLinksEnabled"] = value; }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PopupControls_Entity_Container.Style["display"] = "none";
        }

        #endregion

        #region Form methods

        #region Initialize

        public void ShowModalForm(int xevprmMessagePk)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            XevprmMessagePk = xevprmMessagePk;
            XevprmEntityType = XevprmEntityType.NULL;
            XevprmEntityPk = null;

            ValidationLinksEnabled = true;

            InitForm(null);
            BindForm();

            if (IsValidationSuccessful && OnValidationSuccessful != null)
            {
                OnValidationSuccessful(this, new FormEventArgs<int>(XevprmMessagePk));
            }
        }

        public void ShowModalForm(int xevprmMessagePk, XevprmEntityType xevprmEntityType, int? xevprmEntityPk)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            XevprmMessagePk = xevprmMessagePk;
            XevprmEntityType = xevprmEntityType;
            XevprmEntityPk = xevprmEntityPk;

            ValidationLinksEnabled = true;

            InitForm(null);
            BindForm();

            if (IsValidationSuccessful && OnValidationSuccessful != null)
            {
                OnValidationSuccessful(this, new FormEventArgs<int>(XevprmMessagePk));
            }
        }

        public void ShowModalForm(ValidationResult validationResult, bool validationLinksEnabled = true)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            ValidationLinksEnabled = validationLinksEnabled;

            InitForm(null);
            BindForm(validationResult);
        }

        void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
            SetFormControlsDefaults(null);
        }

        public void ClearForm(string arg)
        {
            divValidationErrors.Controls.Clear();
        }

        #endregion

        #region Fill

        private void FillFormControls(object args)
        {
        }

        void SetFormControlsDefaults(object arg)
        {
        }

        #endregion

        #region Bind

        void BindForm(ValidationResult validationResult)
        {
            IsValidationSuccessful = true;
            btnValidate.Visible = false;

            if (!validationResult.IsSuccess)
            {
                IsValidationSuccessful = false;
            }

            if (validationResult.Exceptions.Count > 0)
            {
                divValidationErrors.Controls.Add(new LiteralControl("<div class=\"xevprm-val-pop-holder\">"));

                divValidationErrors.Controls.Add(new LiteralControl("<div style=\"text-align:center;\">"));
                divValidationErrors.Controls.Add(new LiteralControl("Error at validation. Please contact your system administrator."));
                divValidationErrors.Controls.Add(new LiteralControl("</div></br>"));

                foreach (var exception in validationResult.Exceptions)
                {
                    divValidationErrors.Controls.Add(new LiteralControl("<div>"));
                    divValidationErrors.Controls.Add(new LiteralControl(exception.Message));
                    divValidationErrors.Controls.Add(new LiteralControl("</div>"));
                }
                divValidationErrors.Controls.Add(new LiteralControl("</div>"));
            }
            else
            {
                divValidationErrors.Controls.Add(new LiteralControl("<div class=\"xevprm-val-pop-holder\">"));

                ShowValidationErrors(validationResult.XevprmValidationTree, divValidationErrors, 0);

                divValidationErrors.Controls.Add(new LiteralControl("</div>"));
            }


            if (IsValidationSuccessful)
            {
                divValidationErrors.InnerHtml = "<div style='text-align:center'><br/>Validation successful.<br/></div>";
            }
        }

        void BindForm()
        {
            IsValidationSuccessful = true;

            foreach (var validationResult in XevprmXml.ValidateXevprm(XevprmMessagePk, XevprmEntityType, XevprmEntityPk))
            {
                if (!validationResult.IsSuccess)
                {
                    IsValidationSuccessful = false;
                    btnValidate.Visible = true;
                }

                if (validationResult.Exceptions.Count > 0)
                {
                    divValidationErrors.Controls.Add(new LiteralControl("<div class=\"xevprm-val-pop-holder\">"));

                    divValidationErrors.Controls.Add(new LiteralControl("<div style=\"text-align:center;\">"));
                    divValidationErrors.Controls.Add(new LiteralControl("Error at validation. Please contact your system administrator."));
                    divValidationErrors.Controls.Add(new LiteralControl("</div></br>"));

                    foreach (var exception in validationResult.Exceptions)
                    {
                        divValidationErrors.Controls.Add(new LiteralControl("<div>"));
                        divValidationErrors.Controls.Add(new LiteralControl(exception.Message));
                        divValidationErrors.Controls.Add(new LiteralControl("</div>"));
                    }
                    divValidationErrors.Controls.Add(new LiteralControl("</div>"));
                }
                else
                {
                    divValidationErrors.Controls.Add(new LiteralControl("<div class=\"xevprm-val-pop-holder\">"));

                    ShowValidationErrors(validationResult.XevprmValidationTree, divValidationErrors, 0);

                    divValidationErrors.Controls.Add(new LiteralControl("</div>"));
                }
            }

            if (IsValidationSuccessful)
            {
                divValidationErrors.InnerHtml = "<div style='text-align:center'><br/>Validation successful.<br/></div>";
            }
        }

        private void ShowValidationErrors(EVMessage.Xevprm.TreeNode<XevprmValidationTreeNode> treeNode, Control control, int level)
        {
            if (treeNode == null) return;

            control.Controls.Add(new LiteralControl("<div class=\"xevprm-val-pop-holder" + level + "\">"));

            control.Controls.Add(new LiteralControl("<div class=\"xevprm-val-pop-holder-entity\">"));

            if (treeNode.Value.ReadyEntity is AuthorisedProduct)
            {
                string entityName = (treeNode.Value.ReadyEntity as AuthorisedProduct).product_name;

                if (string.IsNullOrWhiteSpace(entityName))
                {
                    entityName = Constant.UnknownValue;
                }

                entityName = string.Format("Authorised product: <b>{0}</b>", entityName);

                control.Controls.Add(new LiteralControl(entityName));
            }
            else if (treeNode.Value.ReadyEntity is Product_PK)
            {
                string entityName = (treeNode.Value.ReadyEntity as Product_PK).name;

                if (string.IsNullOrWhiteSpace(entityName))
                {
                    entityName = Constant.UnknownValue;
                }

                entityName = string.Format("Product: <b>{0}</b>", entityName);

                control.Controls.Add(new LiteralControl(entityName));
            }
            else if (treeNode.Value.ReadyEntity is Pharmaceutical_product_PK)
            {
                string entityName = (treeNode.Value.ReadyEntity as Pharmaceutical_product_PK).name;

                if (string.IsNullOrWhiteSpace(entityName))
                {
                    entityName = Constant.UnknownValue;
                }

                entityName = string.Format("Pharmaceutical product: <b>{0}</b>", entityName);

                control.Controls.Add(new LiteralControl(entityName));
            }
            else if (treeNode.Value.ReadyEntity is Activeingredient_PK)
            {
                string entityName = (treeNode.Value.ReadyEntity as Activeingredient_PK).concise;

                if (string.IsNullOrWhiteSpace(entityName))
                {
                    entityName = Constant.UnknownValue;
                }

                entityName = string.Format("Active ingredient: <b>{0}</b>", entityName);

                control.Controls.Add(new LiteralControl(entityName));
            }
            else if (treeNode.Value.ReadyEntity is Excipient_PK)
            {
                string entityName = (treeNode.Value.ReadyEntity as Excipient_PK).concise;

                if (string.IsNullOrWhiteSpace(entityName))
                {
                    entityName = Constant.UnknownValue;
                }

                entityName = string.Format("Excipient: <b>{0}</b>", entityName);

                control.Controls.Add(new LiteralControl(entityName));
            }
            else if (treeNode.Value.ReadyEntity is Adjuvant_PK)
            {
                string entityName = (treeNode.Value.ReadyEntity as Adjuvant_PK).concise;

                if (string.IsNullOrWhiteSpace(entityName))
                {
                    entityName = Constant.UnknownValue;
                }

                entityName = string.Format("Adjuvant: <b>{0}</b>", entityName);

                control.Controls.Add(new LiteralControl(entityName));
            }

            control.Controls.Add(new LiteralControl("</div>"));

            var distinctXevprmValidationExceptions = treeNode.Value.XevprmValidationExceptions.GroupBy(x => x.XevprmValidationRuleId).Select(x => x.First());

            int rowindex = 0;
            foreach (var xevprmValidationException in distinctXevprmValidationExceptions)
            {
                AddValidationErrorListItem(xevprmValidationException, control, rowindex);
                rowindex++;
            }

            foreach (var child in treeNode.Children)
            {
                ShowValidationErrors(child, control, level + 1);
            }

            control.Controls.Add(new LiteralControl("</div>"));
        }

        private void AddValidationErrorListItem(XevprmValidationException xevprmValidationException, Control control, int rowindex)
        {
            if (rowindex % 2 == 0)
            {
                control.Controls.Add(new LiteralControl("<div class=\"xevprm-val-pop-br\">"));
            }
            else
            {
                control.Controls.Add(new LiteralControl("<div class=\"xevprm-val-pop-br-alternate\">"));
            }

            control.Controls.Add(new LiteralControl("<div class=\"xevprm-val-pop-br-id\">"));

            control.Controls.Add(new LiteralControl(xevprmValidationException.XevprmBusinessRule));

            control.Controls.Add(new LiteralControl("</div>"));

            control.Controls.Add(new LiteralControl("<div class=\"xevprm-val-pop-br-link\">"));

            var validationErrorHyperLink = new HyperLink
            {
                Text = xevprmValidationException.ReadyMessage,
                NavigateUrl = xevprmValidationException.NavigateUrl,
                Target = "_blank",
                Enabled = ValidationLinksEnabled
            };

            control.Controls.Add(validationErrorHyperLink);

            control.Controls.Add(new LiteralControl("</div>"));

            control.Controls.Add(new LiteralControl("</div>"));
        }

        #endregion

        #region Validate

        public bool ValidateForm(string arg)
        {
            ClearValidationErrors();

            var errorMessage = String.Empty;

            // If errors were found, showing them in modal popup
            if (!string.IsNullOrEmpty(errorMessage))
            {
                var masterPage = (Template.Default)Page.Master;

                if (masterPage != null)
                {
                    masterPage.ModalPopup.ShowModalPopup("Error!", errorMessage);
                }

                return false;
            }

            return true;
        }

        private void ClearValidationErrors()
        {
        }

        #endregion

        #region Save

        public object SaveForm(object args)
        {
            return null;
        }

        #endregion

        #endregion

        #region Event handlers

        protected void btnValidate_OnClick(object sender, EventArgs e)
        {
            btnValidate.Visible = false;
            divValidationErrors.Controls.Clear();

            BindForm();

            if (IsValidationSuccessful && OnValidationSuccessful != null)
            {
                OnValidationSuccessful(this, new FormEventArgs<int>(XevprmMessagePk));
            }
        }

        protected void btnExportToExcel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Business/ValidationReport.aspx?msgID=" + XevprmMessagePk, "_blank", "");
        }

        public void btnOk_OnClick(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            if (OnOkButtonClick != null)
            {
                OnOkButtonClick(sender, new FormEventArgs<int>(XevprmMessagePk));
            }
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            if (OnCancelButtonClick != null)
            {
                OnCancelButtonClick(sender, e);
            }
        }

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            if (OnCancelButtonClick != null)
            {
                OnCancelButtonClick(sender, e);
            }
        }

        #endregion
    }
}