using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.NanokinetikEDMS;
using AspNetUIFramework;
using PossGrid;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.Views.Shared.UserControl.Popup
{
    public enum EDMSDocumentVersion
    {
        SelectedVersion,
        Current
    }

    public partial class BrowseEDMSPopup : System.Web.UI.UserControl
    {
        #region Declarations

        private const int NumLayoutToKeep = 5;
        private string _gridId;
        public Template.Default MasterPage;

        private IUser_grid_settings_PKOperations _userGridSettingsOperations;
        private IUSEROperations _userOperations;

        public virtual event EventHandler<FormEventArgs<Tuple<EDMSDocument, EDMSDocumentVersion>>> OnOkButtonClick;
        public virtual event EventHandler<EventArgs> OnCancelButtonClick;

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

        private string _documentIdToLink
        {
            get { return hfSelectedDocumentId.Value; }
            set { hfSelectedDocumentId.Value = value; }
        }

        private string _documentVersion
        {
            get { return (string)ViewState["_documentVersion"]; }
            set { ViewState["_documentVersion"] = value; }
        }

        private bool EDMSLayoutLoaded
        {
            get { return Session["Layout" + _gridId] != null && (bool)Session["Layout" + _gridId]; }
            set { Session["Layout" + _gridId] = value; }
        }

        private bool _EDMSGridRowSelected
        {
            get { return ViewState["_EDMSGridRowSelected"] != null && (bool)ViewState["_EDMSGridRowSelected"]; }
            set { ViewState["_EDMSGridRowSelected"] = value; }
        }

        private EDMSDocument EDMSAttachment
        {
            get { return (EDMSDocument)ViewState["EDMSAttachment"]; }
            set { ViewState["EDMSAttachment"] = value; }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            BindEventHandlers();

            PopupControls_Entity_Container.Style["display"] = "none";

            _userGridSettingsOperations = new User_grid_settings_PKDAL();
            _userOperations = new USERDAL();

            EDMSGrid.GridVersion = EDMSGrid.GridVersion + "Popup";

            _gridId = EDMSGrid.GridId + "_" + EDMSGrid.GridVersion;

            MasterPage = (Template.Default)Page.Master;

            if (!EDMSLayoutLoaded)
            {
                var userGridSettings = _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
                if (userGridSettings != null && !String.IsNullOrWhiteSpace(userGridSettings.grid_layout))
                {
                    EDMSGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
                    EDMSLayoutLoaded = true;
                }
            }
        }

        private void BindEventHandlers()
        {
            btnLinkDocument.Click += btnLinkDocument_Click;

            btnSaveLayout.Click += btnSaveLayout_Click;
            btnClearLayout.Click += btnClearLayout_Click;
            btnReset.Click += btnResetLayout_Click;
            btnExport.Click += btnExport_Click;
            btnColumns.Click += btnColumns_OnClick;
            ColumnsPopup.OnOkButtonClick += ColumnsPopup_OnOkButtonClick;

            EDMSGrid.OnRebindRequired += EDMSGrid_OnRebindRequired;
            EDMSGrid.OnHtmlRowPrepared += EDMSGrid_OnHtmlRowPrepared;
            EDMSGrid.OnHtmlCellPrepared += EDMSGrid_OnHtmlCellPrepared;
            EDMSGrid.OnExcelCellPrepared += EDMSGrid_OnExcelCellPrepared;
            EDMSGrid.OnLoadClientLayout += EDMSGrid_OnLoadClientLayout;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
            {
                scriptManager.RegisterPostBackControl(btnExport);
            }

            BindGrid();
        }

        #endregion

        #region Form methods

        #region Initialize

        public void ShowModalForm()
        {
            PopupControls_Entity_Container.Style["display"] = "inline";
            EDMSAttachment = new EDMSDocument();
        }

        void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
            SetFormControlsDefaults(null);
        }

        public void ClearForm(string arg)
        {

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

        void BindForm(object arg)
        {

        }

        private void BindGrid()
        {
            var ds = GetGridDataSet();
            EDMSGrid.DataSource = ds.Tables[0].DefaultView;
            EDMSGrid.DataBind();

            MasterPage.UpCommon.Update();
        }

        private DataSet GetGridDataSet()
        {
            var filters = GetFilters();

            var folderId = !string.IsNullOrWhiteSpace(hfSelectedFolderId.Value) ? hfSelectedFolderId.Value : string.Empty;
            var username = SessionManager.Instance.CurrentUser.Username; 

            var itemCount = 0;
            var ds = EDMS.EdmsGetFolderContent(folderId, username, filters, out itemCount);

            try
            {
                var filterExpression = ConvertFiltersToExpression(filters);
                ds.Tables[0].DefaultView.RowFilter = filterExpression;

                string sort;
                if (EDMSGrid.MainSortingColumn != null && EDMSGrid.SecondSortingColumn == null)
                {
                    sort = string.Format("{0} {1}", EDMSGrid.MainSortingColumn, (EDMSGrid.MainSortingOrder == SortOrder.ASC ? SortOrder.ASC : SortOrder.DESC));
                    ds.Tables[0].DefaultView.Sort = sort;
                }

                if (EDMSGrid.SecondSortingColumn != null)
                {
                    sort = string.Format("{0} {1}", EDMSGrid.SecondSortingColumn, (EDMSGrid.SecondSortingOrder == SortOrder.ASC ? SortOrder.ASC : SortOrder.DESC));
                    ds.Tables[0].DefaultView.Sort += sort;
                    EDMSGrid.MainSortingColumn = null;
                }
            }
            catch (Exception ex)
            {
                // Bad filter format. Ignore
            }

            return ds;
        }

        private string ConvertFiltersToExpression(Dictionary<string, string> filters)
        {
            var filterExpression = string.Empty;
            
            foreach (var filter in filters)
            {
                if (filter.Key.In("ModifyDate")) filterExpression = filterExpression + ("CONVERT(" + filter.Key + ", System.String)" + " LIKE '%" + filter.Value + "%' AND ");
                else filterExpression = filterExpression + (filter.Key + " LIKE '%" + filter.Value + "%' AND ");
            }

            if (filterExpression.Length > 4) filterExpression = filterExpression.Substring(0, filterExpression.Length - 4);

            return filterExpression;
        }

        #endregion

        #region Validate

        public bool ValidateForm(string arg)
        {
            ClearValidationErrors();

            var errorMessage = String.Empty;

            if (string.IsNullOrWhiteSpace(_documentIdToLink)) errorMessage = "<div style='text-align: center; margin-bottom: 20px;'>Please select the Document to link!</div>";

            // If errors were found, showing them in modal popup
            if (!string.IsNullOrEmpty(errorMessage))
            {
                MasterPage.ModalPopup.ShowModalPopup("Error!", errorMessage);
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

        #region Grid

        void EDMSGrid_OnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {

        }

        void EDMSGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            var dataKey = EDMSGrid.DataKeys[e.Row.RowIndex];
            if (dataKey != null && _documentIdToLink == Convert.ToString(dataKey.Value))
            {
                var row = EDMSGrid.Rows[e.Row.RowIndex];

                foreach (TableCell cell in row.Cells)
                {
                    cell.BackColor = Color.Yellow;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var btnDownload = (ImageButton)e.Row.FindControl("ibDownloadEDMSAttachment");
                if (btnDownload != null && !string.IsNullOrWhiteSpace(btnDownload.CommandArgument))
                {
                    btnDownload.Attributes.Add("onclick", "SaveTheDownloadBtnAttach('" + btnDownload.ClientID + "," + btnDownload.CommandArgument + "');");
                }
            }
        }

        void EDMSGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void EDMSGrid_OnHtmlCellPrepared(object sender, PossGridCellEventArgs e)
        {

        }

        void EDMSGrid_OnLoadClientLayout(object sender, ClientLayoutEventArgs args)
        {
            var userGridSettings = _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            if (userGridSettings != null && !String.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                args.ClientLayoutString = userGridSettings.grid_layout;
            }
        }

        #endregion

        void btnLinkDocument_Click(object sender, EventArgs e)
        {
            if (ValidateForm(null))
            {
                var username = SessionManager.Instance.CurrentUser.Username;

                var _formatType = formatType.ORIGINAL;
                if (rbDocumentType.SelectedValue.ToLower() == "pdf") _formatType = formatType.PDF;

                if (rbDocumentVersion.SelectedValue.ToLower() == EDMSDocumentVersion.Current.ToString().ToLower()) _documentVersion = EDMSDocumentVersion.Current.ToString().ToUpper();
               
                try
                {
                    using (var edmsWsClient = new EDMS_WSClient())
                    {
                        EDMSAttachment = edmsWsClient.getDocument(_documentIdToLink, username, _documentVersion, _formatType);
                    }
                    PopupControls_Entity_Container.Style["display"] = "none";

                    if (OnOkButtonClick != null)
                    {
                        var documentVersion = rbDocumentVersion.SelectedValue.ToLower() == EDMSDocumentVersion.Current.ToString().ToLower() ? EDMSDocumentVersion.Current : EDMSDocumentVersion.SelectedVersion;
                        var tuple = new Tuple<EDMSDocument, EDMSDocumentVersion>(EDMSAttachment, documentVersion);
                        _documentIdToLink = string.Empty;
                        _EDMSGridRowSelected = false;
                        OnOkButtonClick(sender, new FormEventArgs<Tuple<EDMSDocument, EDMSDocumentVersion>>(tuple));
                    }
                }
                catch (Exception ex)
                {
                    MasterPage.ModalPopup.ShowModalPopup("Error", "Error downloading file! Please try again or contact your system administrator.");
                    return;
                }
            }
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in EDMSGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("DocumentId"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (EDMSGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
                    {
                        ColumnsPopup.SelectedColumns.Add(new ListItem(caption, (column as IFilteredColumn).FieldName));
                    }
                    else
                    {
                        ColumnsPopup.AvailableColumns.Add(new ListItem(caption, (column as IFilteredColumn).FieldName));
                    }
                }
            }

            ColumnsPopup.ShowModalForm();
        }

        public void ColumnsPopup_OnOkButtonClick(object sender, EventArgs e)
        {
            EDMSGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();
            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            EDMSGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);

            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = EDMSGrid.GetClientLayoutString(),
                timestamp = DateTime.Now,
                ql_visible = true,
                isdefault = true,
                display_name = "SavedLayout"
            };

            userGridSettings = _userGridSettingsOperations.Save(userGridSettings);
            if (userGridSettings != null)
            {
                _userGridSettingsOperations.SetDefaultAndKeepFirstNLayouts(
                    Thread.CurrentPrincipal.Identity.Name,
                    _gridId,
                    userGridSettings.user_grid_settings_PK,
                    NumLayoutToKeep);
            }
        }

        void btnExport_Click(object sender, EventArgs e)
        {
            var EDMSdv= EDMSGrid.DataSource as DataView;
            if (EDMSdv != null) EDMSGrid.ExportDataToXlsx(EDMSdv.ToTable(), new ExcellExportOptions("grid"));
        }

        void btnResetLayout_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            EDMSGrid.ResetVisibleColumns();
            EDMSGrid.SecondSortingColumn = null;
            EDMSGrid.MainSortingOrder = PossGrid.SortOrder.ASC;
            BindGrid();
        }

        public void btnOk_OnClick(object sender, FormEventArgs<Tuple<EDMSDocument, EDMSDocumentVersion>> e)
        {
            if (ValidateForm(null))
            {
                SaveForm(null);
                PopupControls_Entity_Container.Style["display"] = "none";

                if (OnOkButtonClick != null)
                {
                    OnOkButtonClick(sender, e);
                }
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

        protected void btnBindGrid_OnClick(object sender, EventArgs e)
        {
            // Fake postback for grid bind
        }

        protected void DocumentRow_OnClick(object sender, EventArgs e)
        {
            var selectButton = sender as LinkButton;
            if (selectButton == null) return;

            var commandNameString = Convert.ToString(selectButton.CommandName);
            var commandArgumentString = Convert.ToString(selectButton.CommandArgument);

            if (commandNameString == Constant.CommandArgument.Select)
            {
                var args = commandArgumentString.Split('|');
                if (!_EDMSGridRowSelected || args[0] != _documentIdToLink)
                {
                    _documentIdToLink = args[0];
                    _documentVersion = args[1];
                    _EDMSGridRowSelected = true;
                }
                else
                {
                    _documentIdToLink = string.Empty;
                    _documentVersion = string.Empty;
                    _EDMSGridRowSelected = false;
                }
            }
        }

        #endregion

        #region Support methods

        private Dictionary<string, string> GetFilters()
        {
            var filters = EDMSGrid.GetFilters();

            return filters;
        }

        private DataTable PrepareDataForExport(DataTable projectDataTable)
        {
            if (projectDataTable == null || projectDataTable.Rows.Count == 0) return projectDataTable;

            return projectDataTable;
        }

        public string HandleMissing(object value)
        {
            if (value != null && value.ToString().Trim() != "")
            {
                return value.ToString();
            }

            return "Missing";
        }

        public string HandleDocumentArguments(object attachmentPk, object EDMSDocumentId, object EDMSBindingRule, object EDMSAttachmentFormat)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(EDMSDocumentId))) return Convert.ToString(attachmentPk);
            else
            {
                return string.Format("{0}|{{{1};{2};{3}}}", attachmentPk, EDMSDocumentId, EDMSBindingRule, EDMSAttachmentFormat);
            }
        }

        #endregion

        #region Security

        #endregion
    }
}