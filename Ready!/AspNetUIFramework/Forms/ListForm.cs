using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonTypes;

namespace AspNetUIFramework
{
    public abstract class ListForm : UserControl, IFormList
    {
        public enum ListPermissionType
        {
            NONE,
            READ,
            READ_WRITE
        }

        private ListPermissionType _permission = ListPermissionType.NONE;

        public ListForm() : base() { }

        #region Properties

        public FormHolder FormHolder
        {
            get { return this.Page as FormHolder; }
        }

        public string AssociatedGridViewID
        {
            get { return ViewState["AssociatedGridViewID_" + this.ID] == null ? "gvData" : ViewState["AssociatedGridViewID_" + this.ID].ToString(); }
            set { ViewState["AssociatedGridViewID_" + this.ID] = value; }
        }

        public GridView AssociatedGridView
        {
            get { return FindControl(AssociatedGridViewID) as GridView; }
        }

        public string AssociatedGridViewPagerID
        {
            get { return ViewState["AssociatedGridViewPagerID_" + this.ID] == null ? "gvPager" : ViewState["AssociatedGridViewPagerID_" + this.ID].ToString(); }
            set { ViewState["AssociatedGridViewPagerID_" + this.ID] = value; }
        }

        public IGridViewPager AssociatedGridViewPager
        {
            get { return FindControl(AssociatedGridViewPagerID) as IGridViewPager; }
        }

        public ListPermissionType Permission
        {
            get { return _permission; }
        }

        // some list panels got both: list and details
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

        #region IFormList members

        public virtual event EventHandler<FormListEventArgs> OnListItemSelected;

        public virtual void Search(string arg)
        {
            SetDefaultGVSettings();
            BindForm(null, arg);
        }

        public virtual void DeleteItem(object id, string arg)
        {
            // If this was last record on page, decrement current page
            if (AssociatedGridViewPager != null)
            {
                if (AssociatedGridViewPager.CurrentPage > 1 && AssociatedGridView.Rows.Count == 1) AssociatedGridViewPager.CurrentPage--;
            }

            BindForm(null, "");
        }

        public virtual void SetDefaultGVSettings()
        {
            IGridViewPager gvPager = AssociatedGridViewPager;
            if (AssociatedGridViewPager != null)
            {
                gvPager.CurrentPage = 1;
                gvPager.RecordsPerPage = 50;
                gvPager.SortReverseOrder = false;
                gvPager.TotalRecordsCount = 0;
                gvPager.Visible = false;

                //AssociatedGridView.SelectedIndex = -1;
            }
        }

        public virtual void ShowFormWithoutClearing()
        {
            this.Visible = true;
        }

        #endregion

        #region IFormCommon members

        public virtual void ShowForm(string arg)
        {
            ClearForm("");
            SetDefaultGVSettings();

            this.Visible = true;
        }

        public virtual void HideForm(string arg)
        {
            this.Visible = false;
        }

        public abstract void ClearForm(string arg);

        public abstract void FillDataDefinitions(string arg);

        // SECURITY 
        public abstract ListPermissionType CheckAccess();

        protected virtual void SetAccessState() {
            switch (_permission) {
                case ListPermissionType.NONE:
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
                case ListPermissionType.READ:
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
                case ListPermissionType.READ_WRITE:
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
        
        public virtual void BindForm(object id, string arg)
        {
            if (AssociatedGridViewPager != null)
            {
                AssociatedGridViewPager.BindGridViewPager();
                AssociatedGridView.DataBind();

                if (AssociatedGridView.Rows.Count == 0) AssociatedGridViewPager.Visible = false;
                else AssociatedGridViewPager.Visible = true;
            }
        }

        public abstract bool ValidateForm(string arg);

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
                _permission = ListPermissionType.READ_WRITE;
            }
            else
            {
                _permission = CheckAccess();
            }

            
            SetAccessState();

            IGridViewPager gvPager = AssociatedGridViewPager;
            if (AssociatedGridViewPager != null)
            {

                gvPager.PageChanged += new EventHandler<PageChangedEventArgs>(gvPager_PageChanged);
                SetDefaultGVSettings();

                GridView gvData = AssociatedGridView;

                gvData.RowCreated += new GridViewRowEventHandler(gvData_RowCreated);
                gvData.Sorting += new GridViewSortEventHandler(gvData_Sorting);
                gvData.SelectedIndexChanging += new GridViewSelectEventHandler(gvData_SelectedIndexChanging);
                gvData.RowDeleting += new GridViewDeleteEventHandler(gvData_RowDeleting);
                gvData.RowDataBound += new GridViewRowEventHandler(gvData_RowDataBound);
            }


            
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
                _permission = ListPermissionType.READ_WRITE;
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
                if (PostbackManager.Instance.PostbackControlID.Contains("ibtDeleteItem") && PostbackManager.Instance.PostbackControlID.Contains(this.GetType().BaseType.Name))
                    gvData_RowDeleting(this, new GridViewDeleteEventArgs(Convert.ToInt32(PostbackManager.Instance.PostbackArgument)));

                if (Permission != ListPermissionType.READ_WRITE)
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
                    //            {
                    //                postBackControlName = ctlName;
                    //            }
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
        }

        #endregion

        #region GridView and GridViewPager events

        public virtual void gvPager_PageChanged(object sender, PageChangedEventArgs e)
        {
            BindForm(null, "");
        }

        public virtual void gvData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                JsEffectsController.GvRowMouseOverEffect(e.Row);
            }
        }

        public virtual void gvData_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == AssociatedGridViewPager.SortOrderBy)
            {
                AssociatedGridViewPager.SortReverseOrder = !AssociatedGridViewPager.SortReverseOrder;
            }
            else
            {
                AssociatedGridViewPager.SortOrderBy = e.SortExpression;
                AssociatedGridViewPager.SortReverseOrder = false;
            }

            BindForm(null, "");
        }

        public virtual void gvData_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            OnListItemSelected(sender, new FormListEventArgs(AssociatedGridView.DataKeys[e.NewSelectedIndex].Value));
        }

        public virtual void gvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (AssociatedGridView != null)
            {
                object deletedValue = AssociatedGridView.DataKeys[e.RowIndex].Value;
                DeleteItem(deletedValue, "");
            }
            else
            {
                DeleteItem(e.RowIndex, "");
            }
        }

        public virtual void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Nothing by default
        }

        #endregion

        #region Setting controls for different permissions

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
                if (panels.Any())
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

                var lnkReminders = AssociatedListPanel.Controls.OfType<LinkButton>();
                foreach (var lnkReminder in lnkReminders)
                {
                    if (lnkReminder.CssClass.Contains("reminder"))
                    {
                        lnkReminder.Enabled = false;
                        lnkReminder.Attributes.CssStyle.Add("cursor", "default");
                        lnkReminder.Attributes.CssStyle.Add("color", "gray !important");
                    }
                } 
                
                var lnkRemindersIb = AssociatedListPanel.Controls.OfType<ImageButton>();
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
            }

            if (AssociatedListPanel != null)
            {
                Control deleteControl = AssociatedListPanel.FindControl("btnDelete");
                if (deleteControl != null && (deleteControl is Button))
                {
                    (deleteControl as Button).Enabled = true;
                    (deleteControl as Button).Attributes.CssStyle.Add("cursor", "pointer");
                    (deleteControl as Button).Attributes.CssStyle.Add("color", "black !important");
                }

                var lnkReminders = AssociatedListPanel.Controls.OfType<LinkButton>();
                foreach (var lnkReminder in lnkReminders)
                {
                    if (lnkReminder.CssClass.Contains("reminder"))
                    {
                        lnkReminder.Enabled = true;
                        lnkReminder.Attributes.CssStyle.Add("cursor", "pointer");
                        lnkReminder.Attributes.CssStyle.Add("color", "black !important");
                    }
                }

                var lnkRemindersIb = AssociatedListPanel.Controls.OfType<ImageButton>();
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

        protected void disableAttachmentPanel() {
            
            Panel attachmentPanel =  AssociatedDetailsPanel.FindControl("pnlDataList") as Panel;
            Panel uploadPanel = AssociatedDetailsPanel.FindControl("pnlUploadFiles") as Panel;

            if (attachmentPanel != null)
            {
                //attachmentPanel.Enabled = false;

                GridView gvData = attachmentPanel.FindControl("gvData") as GridView;
                if (gvData != null)
                {
                    attachmentPanel.Enabled = false;
                    foreach (GridViewRow row in gvData.Rows)
                    {
                        LinkButton removeIcon = row.FindControl("ibtDeleteItem") as LinkButton;
                        if (removeIcon != null) {
                            removeIcon.Visible = false;
                        }
                    }
                }

            }

            if (uploadPanel != null)
            {
                uploadPanel.Visible = false;
            }
        }

        protected void enableAttachmentPanel() {
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

        #endregion

    }
}
