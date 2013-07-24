using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonTypes;

namespace AspNetUIFramework
{
    public abstract class DetailsForm : UserControl, IFormDetails
    {
        #region Properties

        public enum DetailsPermissionType
        {
            NONE,
            READ,
            WRITE,
            READ_WRITE
        }

        private DetailsPermissionType _permission = DetailsPermissionType.NONE;

        public FormHolder FormHolder
        {
            get { return this.Page as FormHolder; }
        }

        public string AssociatedDetailsPanelID
        {
            get { return ViewState["AssociatedPanelID_" + this.ID] == null ? "pnlDataDetails" : ViewState["AssociatedPanelID_" + this.ID].ToString(); }
            set { ViewState["AssociatedPanelID_" + this.ID] = value; }
        }

        public string AssociatedListPanelID
        {
            get { return ViewState["AssociatedListPanelID_" + this.ID] == null ? "pnlDataList" : ViewState["AssociatedListPanelID_" + this.ID].ToString(); }
            set { ViewState["AssociatedListPanelID_" + this.ID] = value; }
        }

        public Panel AssociatedDetailsPanel
        {
            get { return FindControl(AssociatedDetailsPanelID) as Panel; }
        }

        public Panel AssociatedListPanel
        {
            get { return FindControl(AssociatedListPanelID) as Panel; }
        }

        protected DetailsPermissionType Permission
        {
            get { return _permission; }
        }

        protected bool IsInitialHashWritten
        {
            get { return ((bool?)ViewState["IsInitialHasWritten"]) ?? false; }
            set { ViewState["IsInitialHasWritten"] = value; }
        }

        protected List<int> InitialFormHash
        {
            get
            {
                if (ViewState["InitialFormHash"] == null) return new List<int>();
                return (List<int>)ViewState["InitialFormHash"];
            }
            set
            {
                ViewState["InitialFormHash"] = value;
            }
        }

        #endregion

        #region IFormDetails members

        public object SaveFormWithSecurityCheck(object id, string arg)
        {
            if (Permission == DetailsPermissionType.READ_WRITE)
            {
                return SaveForm(id, arg);
            }
            Response.Redirect("~/Views/Business/PRODUCTView.aspx?f=l");
            return null;
        }

        public abstract object SaveForm(object id, string arg);

        #endregion

        #region IFormCommon members

        public virtual void ShowForm(string arg)
        {
            if (arg != "WITHOUT_CLEAR")
            {
                ClearForm("");
            }

            this.Visible = true;
        }

        public virtual void HideForm(string arg)
        {
            this.Visible = false;
        }

        public abstract void ClearForm(string arg);

        public abstract void FillDataDefinitions(string arg);

        public abstract void BindForm(object id, string arg);

        public abstract bool ValidateForm(string arg);

        //SECURITY
        public abstract DetailsPermissionType CheckAccess();

        protected virtual void SetAccessState()
        {
            switch (_permission)
            {
                case DetailsPermissionType.NONE:
                    if (AssociatedDetailsPanel != null)
                    {
                        AssociatedDetailsPanel.Enabled = false;
                    }
                    if (AssociatedListPanel != null)
                    {
                        //AssociatedListPanel.Enabled = false;
                    }
                    disableSpecialControls();
                    break;
                case DetailsPermissionType.READ:
                    if (AssociatedDetailsPanel != null)
                    {
                        AssociatedDetailsPanel.Enabled = false;
                    }
                    if (AssociatedListPanel != null)
                    {
                        //AssociatedListPanel.Enabled = false;
                    }

                    disableSpecialControls();
                    break;
                case DetailsPermissionType.READ_WRITE:
                    if (AssociatedDetailsPanel != null)
                    {
                        AssociatedDetailsPanel.Enabled = true;
                    }
                    if (AssociatedListPanel != null)
                    {
                        AssociatedListPanel.Enabled = true;
                    }
                    enableSpecialControls();
                    break;
            }
        }

        #endregion

        #region ASP.NET events

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            bool VIPaccess = false;
            AppUser user = null;
            if (FormHolder != null)
            {
                user = FormHolder.MasterPage.CurrentUser;
            }

            if (user == null) user = SessionManager.Instance.CurrentUser;

            if (user != null)
            {
                string[] roles = user.Roles;
                if (roles != null && roles.Contains("admin")) VIPaccess = true;
            }
            if (VIPaccess)
            {
                _permission = DetailsPermissionType.READ_WRITE;
            }
            else
            {
                _permission = CheckAccess();
            }
            SetAccessState();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            bool VIPaccess = false;
            AppUser user = null;
            if (FormHolder != null)
            {
                user = FormHolder.MasterPage.CurrentUser;
            }

            if (user == null) user = SessionManager.Instance.CurrentUser;

            if (user != null)
            {
                string[] roles = user.Roles;
                if (roles != null && roles.Contains("admin")) VIPaccess = true;
            }
            if (VIPaccess)
            {
                _permission = DetailsPermissionType.READ_WRITE;
            }
            else
            {
                _permission = CheckAccess();
            }


            SetAccessState();

            if (!IsPostBack)
            {
                ClearForm("");
                FillDataDefinitions("");
            }
            else
            {
                if (Permission != DetailsPermissionType.READ_WRITE)
                {
                    //string postBackControlName = PostbackManager.Instance.PostbackControlID;

                    //if (string.IsNullOrEmpty(postBackControlName))
                    //{
                    //    foreach (string ctlName in Request.Form)
                    //    {
                    //        if (ctlName.Contains("btnDelete") ||
                    //            ctlName.Contains("btnSave") ||
                    //            ctlName.Contains("lbtSave") ||
                    //            ctlName.Contains("lbtSave"))
                    //        {
                    //            postBackControlName = ctlName;
                    //        }
                    //    }
                    //}

                    //if (!string.IsNullOrEmpty(postBackControlName) && (postBackControlName.Contains("Delete") || (postBackControlName.ToLower().Contains("save") && !postBackControlName.ToLower().Contains("saveas"))))
                    //{
                    //    Response.Redirect("~/Views/Business/PRODUCTView.aspx?f=l");
                    //}
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            SetAccessState();
        }

        #endregion

        protected void disableSpecialControls()
        {
            if (AssociatedDetailsPanel != null)
            {
                foreach (Control c in AssociatedDetailsPanel.Controls)
                {
                    if (c is IControlSpecialDisable)
                    {
                        ((IControlSpecialDisable)c).DisableControl();
                    }
                    if (c is Button)
                    {
                        if (((Button)c).Text == "Save")
                        {
                            //((Button)c).Visible = false;
                            Button b = c as Button;
                            b.Enabled = false;
                            b.Attributes.CssStyle.Add("cursor", "default");
                            b.Attributes.CssStyle.Add("color", "gray !important");
                        }
                    }
                }
                if (FormHolder != null)
                {
                    FormHolder.MasterPage.ContextMenu.SetContextMenuItemsDisabled(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save") });
                }

                disableAttachmentPanel();

                Control deleteControl = AssociatedDetailsPanel.FindControl("btnDelete");
                if (deleteControl != null && (deleteControl is Button))
                {
                    (deleteControl as Button).Enabled = false;
                    (deleteControl as Button).Attributes.CssStyle.Add("cursor", "default");
                    (deleteControl as Button).Attributes.CssStyle.Add("color", "gray !important");
                }

                var panels = AssociatedDetailsPanel.Parent.Controls.OfType<Panel>();
                if(panels.Any())
                {
                    DisableReminders(panels);
                }

                var pnlFooter = AssociatedDetailsPanel.Parent.Controls.OfType<Panel>().FirstOrDefault(pnl => pnl.ID == "pnlFooter") ?? AssociatedDetailsPanel.Parent.Parent.Controls.OfType<Panel>().FirstOrDefault(pnl => pnl.ID == "pnlFooter");
                if (pnlFooter != null)
                {
                    DisableButtonsWithCssClass(pnlFooter, "Save");
                }
            }

            if (AssociatedListPanel != null)
            {
                Control deleteControl = AssociatedListPanel.FindControl("btnDelete");
                if (deleteControl != null && (deleteControl is Button))
                {
                    (deleteControl as Button).Enabled = false;
                    (deleteControl as Button).Attributes.CssStyle.Add("cursor", "default");
                    (deleteControl as Button).Attributes.CssStyle.Add("color", "gray !important");
                }

                var panels = AssociatedDetailsPanel.Parent.Controls.OfType<Panel>();
                if(panels.Any())
                {
                    DisableReminders(panels);
                }
            }
        }

        private void DisableReminders(IEnumerable<Panel> panels)
        {
            foreach (var panel in panels)
            {
                var lnkReminders = panel.Controls.OfType<LinkButton>();
                foreach (var lnkReminder in lnkReminders)
                {
                    if (lnkReminder.CssClass.Contains("reminder"))
                    {
                        lnkReminder.Enabled = false;
                        lnkReminder.Attributes.CssStyle.Add("cursor", "default");
                        lnkReminder.Attributes.CssStyle.Add("color", "gray !important");
                    }
                }

                var lnkRemindersIb = panel.Controls.OfType<ImageButton>();
                foreach (var lnkReminder in lnkRemindersIb)
                {
                    if (lnkReminder.CssClass.Contains("reminder"))
                    {
                        lnkReminder.Enabled = false;
                        lnkReminder.Attributes.CssStyle.Add("cursor", "default");
                        lnkReminder.Attributes.CssStyle.Add("color", "gray !important");
                    }
                }
            }
        }

        public static bool DisableButtonsWithCssClass(Panel panel, string cssClass)
        {
            var buttonsToDisable = panel.Controls.OfType<Button>().Where(control => control.CssClass.Contains(cssClass));
            foreach (var buttonToDisable in buttonsToDisable)
            {
                if (buttonToDisable == null) return false;

                //lowerSaveButton.CssClass = "buttonDisabled"; 
                DisableButton(buttonToDisable);
            }

            return true;
        }

        public static void DisableButton(Button buttonToDisable)
        {
            buttonToDisable.Enabled = false;
            buttonToDisable.Attributes.CssStyle.Add("cursor", "default");
            buttonToDisable.Attributes.CssStyle.Add("color", "gray !important");
        }

        private void EnableReminders(IEnumerable<Panel> panels)
        {
            foreach (var panel in panels)
            {
                var lnkReminders = panel.Controls.OfType<LinkButton>();
                foreach (var lnkReminder in lnkReminders)
                {
                    if (lnkReminder.CssClass.Contains("reminder"))
                    {
                        lnkReminder.Enabled = true;
                        lnkReminder.Attributes.CssStyle.Add("cursor", "pointer");
                        lnkReminder.Attributes.CssStyle.Add("color", "black !important");
                    }
                }

                var lnkRemindersIb = panel.Controls.OfType<ImageButton>();
                foreach (var lnkReminder in lnkRemindersIb)
                {
                    if (lnkReminder.CssClass.Contains("reminder"))
                    {
                        lnkReminder.Enabled = true;
                        lnkReminder.Attributes.CssStyle.Add("cursor", "pointer");
                        lnkReminder.Attributes.CssStyle.Add("color", "black !important");
                    }
                }
            }
        }


        protected void enableSpecialControls()
        {
            if (AssociatedDetailsPanel != null)
            {  
                foreach (Control c in AssociatedDetailsPanel.Controls)
                {
                    if (c is IControlSpecialDisable)
                    {
                        ((IControlSpecialDisable)c).EnableControl();
                    }
                    if (c is Button)
                    {
                        if (((Button)c).Text == "Save")
                        {
                            //((Button)c).Visible = true;
                            Button b = c as Button;
                            b.Enabled = true;
                            b.Attributes.CssStyle.Add("cursor", "pointer");
                            b.Attributes.CssStyle.Add("color", "black !important");
                        }
                    } 
                }
                if (FormHolder != null)
                {
                    FormHolder.MasterPage.ContextMenu.SetContextMenuItemsEnabled(new ContextMenuItem[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save") });
                }

                enableAttachmentPanel();

                Control deleteControl = AssociatedDetailsPanel.FindControl("btnDelete");
                if (deleteControl != null && (deleteControl is Button))
                {
                    (deleteControl as Button).Enabled = true;
                    (deleteControl as Button).Attributes.CssStyle.Add("cursor", "pointer");
                    (deleteControl as Button).Attributes.CssStyle.Add("color", "black !important");
                }

                var panels = AssociatedDetailsPanel.Parent.Controls.OfType<Panel>();
                if (panels.Any())
                {
                    EnableReminders(panels);
                }
            }
        }

        protected void disableAttachmentPanel()
        {
            List<Panel> attachmentsPanels = new List<Panel>();

            attachmentsPanels.Add(AssociatedDetailsPanel.FindControl("pnlDataList") as Panel);
            attachmentsPanels.Add(AssociatedDetailsPanel.FindControl("pnlWorkingAttachment") as Panel);

            //Panel attachmentPanel = AssociatedDetailsPanel.FindControl("pnlDataList") as Panel;


            List<Control> uploadFilesPanels = new List<Control>();
            uploadFilesPanels.Add(AssociatedDetailsPanel.FindControl("pnlUploadFiles"));


            foreach (Panel attachmentPanel in attachmentsPanels)
            {
                if (attachmentPanel != null)
                {

                    GridView gvData = attachmentPanel.FindControl("gvData") as GridView;
                    if (gvData == null) gvData = attachmentPanel.FindControl("WorkingAttachment") as GridView;

                    if (gvData != null)
                    {
                        attachmentPanel.Enabled = false;
                        foreach (GridViewRow row in gvData.Rows)
                        {
                            LinkButton removeIcon = row.FindControl("ibtDeleteItem") as LinkButton;
                            if (removeIcon != null)
                            {
                                removeIcon.Visible = false;
                            }
                        }
                    }

                }
            }
            foreach (Control uploadPanel in uploadFilesPanels)
            {
                if (uploadPanel != null)
                {
                    uploadPanel.Visible = false;
                }
            }

        }

        protected void enableAttachmentPanel()
        {
            Panel attachmentPanel = AssociatedDetailsPanel.FindControl("pnlDataList") as Panel;
            Panel uploadPanel = AssociatedDetailsPanel.FindControl("pnlUploadFiles") as Panel;

            if (attachmentPanel != null)
            {
                attachmentPanel.Enabled = true;
                Control removeIcon = attachmentPanel.FindControl("ibtDeleteItem");
                if (removeIcon != null)
                {
                    removeIcon.Visible = true;
                }
            }

            if (uploadPanel != null)
            {
                uploadPanel.Visible = true;
            }

        }
    }
}
