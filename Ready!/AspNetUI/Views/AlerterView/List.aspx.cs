using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using AspNetUIFramework;
using GEM2Common;
using PossGrid;
using Ready.Model;

namespace AspNetUI.Views.AlerterView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int? _idProd;
        private int? _idAuthProd;
        private int? _idProj;
        private int? _idTask;
        private int? _idDoc;
        private int? _idAct;
        private int? _idSearch;
        private int _sortCount;
        private bool _flip = true;
        private bool _isRed;
        private const int NumLayoutToKeep = 5;
        private string _gridId;

        private IReminder_PKOperations _reminderOperations;
        private IAuthorisedProductOperations _authorisedProductOperations;
        private ITask_PKOperations _taskOperations;
        private IActivity_product_PKOperations _activityProductMnOperations;
        private IProduct_PKOperations _productOperations;
        private IUSEROperations _userOperations;
        private IUser_grid_settings_PKOperations _userGridSettingsOperations;
        private IDocument_PKOperations _documentOperations;
        private IActivity_PKOperations _activityOperations;
        private ITask_name_PKOperations _taskNameOperations;
        private IProject_PKOperations _projectOperations;
        private IReminder_repeating_mode_PKOperations _reminderRepeatingModeOperations;
        private IAlert_saved_search_PKOperations _alertSavedSearchOperations;

        #endregion

        #region Properties

        private int? _alerterPkToDelete
        {
            get { return (int?)ViewState["_alerterPkToDelete"]; }
            set { ViewState["_alerterPkToDelete"] = value; }
        }

        private int? _alerterPkToChangeUserStatus
        {
            get { return (int?)ViewState["_alerterPkToChangeUserStatus"]; }
            set { ViewState["_alerterPkToChangeUserStatus"] = value; }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LoadFormVariables();
            BindEventHandlers();
            GenerateContextMenuItems();
            AssociatePanels(pnlSearch, pnlGrid);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
 
            var scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
            {
                scriptManager.RegisterPostBackControl(btnExport);
                scriptManager.RegisterPostBackControl(btnExportLower);

            }

            if (!IsPostBack)
            {
                InitForm(null);
                BindForm(null);
            }

            if (ListType == ListType.Search)
            {
                if (IsPostbackFromGrid() != false)
                {
                    BindGrid();
                }
            }
            else
            {
                BindGrid();
            }

            SetFormControlsDefaults(null);
            SecurityPageSpecific();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            GenerateTabMenuItems();
            MasterPage.UpTopMenu.Update();
        }

        #endregion

        #region Form methods

        #region Initialize

        public override void LoadFormVariables()
        {
            base.LoadFormVariables();
            LoadActionQuery();

            _idDoc = ValidationHelper.IsValidInt(Request.QueryString["idDoc"]) ? int.Parse(Request.QueryString["idDoc"]) : (int?)null;
            _idAuthProd = ValidationHelper.IsValidInt(Request.QueryString["idAuthProd"]) ? int.Parse(Request.QueryString["idAuthProd"]) : (int?)null;
            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;
            _idAct = ValidationHelper.IsValidInt(Request.QueryString["idAct"]) ? int.Parse(Request.QueryString["idAct"]) : (int?)null;
            _idTask = ValidationHelper.IsValidInt(Request.QueryString["idTask"]) ? int.Parse(Request.QueryString["idTask"]) : (int?)null;
            _idProj = ValidationHelper.IsValidInt(Request.QueryString["idProj"]) ? int.Parse(Request.QueryString["idProj"]) : (int?)null;
            _idSearch = ValidationHelper.IsValidInt(Request.QueryString["idSearch"]) ? int.Parse(Request.QueryString["idSearch"]) : (int?)null;

            _reminderOperations = new Reminder_PKDAL();
            _authorisedProductOperations = new AuthorisedProductDAL();
            _taskOperations = new Task_PKDAL();
            _productOperations = new Product_PKDAL();
            _activityProductMnOperations = new Activity_product_PKDAL();
            _userOperations = new USERDAL();
            _userGridSettingsOperations = new User_grid_settings_PKDAL();
            _documentOperations = new Document_PKDAL();
            _activityOperations = new Activity_PKDAL();
            _taskNameOperations = new Task_name_PKDAL();
            _projectOperations = new Project_PKDAL();
            _reminderRepeatingModeOperations = new Reminder_repeating_mode_PKDAL();
            _alertSavedSearchOperations = new Alert_saved_search_PKDAL();

            if (ListType == ListType.Search)
            {
                AlerterGrid.GridVersion = AlerterGrid.GridVersion + ListType.ToString();
            }
            else if (EntityContext != EntityContext.Default)
            {
                AlerterGrid.GridVersion = AlerterGrid.GridVersion + EntityContext.ToString();
            }

            _gridId = AlerterGrid.GridId + "_" + AlerterGrid.GridVersion;
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
                mpDelete.OnYesButtonClick += mpDelete_OnYesButtonClick;

                AlerterUserStatusPopup.OnOkButtonClick += AlerterUserStatusPopupOnOnOkButtonClick;
            }

            switch (ListType)
            {
                case ListType.List:
                    btnToggleAlert.Click += btnToggleAlert_Click;
                    btnSaveLayout.Click += btnSaveLayout_Click;
                    btnClearLayout.Click += btnClearLayout_Click;
                    btnExport.Click += btnExport_Click;
                    btnColumns.Click += btnColumns_OnClick;
                    ColumnsPopup.OnOkButtonClick += ColumnsPopup_OnOkButtonClick;
                    btnReset.Click += btnReset_Click;

                    break;

                case ListType.Search:
                    txtSrProduct.Searcher.OnListItemSelected += ProductSearcher_OnListItemSelected;
                    txtSrAuthorisedProduct.Searcher.OnListItemSelected += AuthorisedProductSearcher_OnListItemSelected;
                    txtSrProject.Searcher.OnListItemSelected += ProjectSearcher_OnListItemSelected;
                    txtSrActivity.Searcher.OnListItemSelected += ActivitySearcher_OnListItemSelected;
                    txtSrTask.Searcher.OnListItemSelected += TaskSearcher_OnListItemSelected;
                    txtSrDocument.Searcher.OnListItemSelected += DocumentSearcher_OnListItemSelected;
                    QuickLinksPopup.OnOkButtonClick += QuickLinksPopup_OnOkButtonClick;

                    btnExportLower.Click += btnExport_Click;
                    break;
            }

            AlerterGrid.OnRebindRequired += AlerterGridOnRebindRequired;
            AlerterGrid.OnHtmlRowPrepared += AlerterGridOnHtmlRowPrepared;
            AlerterGrid.OnHtmlCellPrepared += AlerterGridOnHtmlCellPrepared;
            AlerterGrid.OnExcelCellPrepared += AlerterGridOnExcelCellPrepared;
            AlerterGrid.OnLoadClientLayout += AlerterGridOnLoadClientLayout;
        }

        void btnToggleAlert_Click(object sender, EventArgs e)
        {
            btnToggleAlert.Text = btnToggleAlert.Text.Trim().ToLower() == "show my" ? "Show all" : "Show my";
            BindGrid();
        }

        void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
        }

        #endregion

        #region Fill

        void ClearForm(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    break;
                case ListType.Search:
                    ClearSearch();
                    break;
            }
        }

        void FillFormControls(object arg)
        {
            btnToggleAlert.Visible = SecurityHelper.IsPermitted(Permission.ShowAlertToggleButton);
            var showAll = Request.QueryString["ShowAll"];
            if (!IsPostBack && !string.IsNullOrWhiteSpace(showAll))
            {
                if (showAll == "True") btnToggleAlert.Text = "Show my";
                else if (showAll == "False") btnToggleAlert.Text = "Show all";
            }

            switch (ListType)
            {
                case ListType.List:
                    break;

                case ListType.Search:
                    BindReminderRepeatingMode();
                    break;
            }
        }

        void SetFormControlsDefaults(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    HideSearch();

                    if (EntityContext == EntityContext.Product) HandleEntityContextProduct();
                    else if (EntityContext == EntityContext.AuthorisedProduct) HandleEntityContextAuthorisedProduct();
                    else if (EntityContext == EntityContext.Product) HandleEntityContextProduct();
                    else if (EntityContext == EntityContext.Document) HandleEntityContextDocument();
                    else if (EntityContext == EntityContext.Project) HandleEntityContextProject();
                    else if (EntityContext == EntityContext.Task) HandleEntityContextTask();
                    else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) HandleEntityContextActivity();

                    break;

                case ListType.Search:
                    if (!IsPostBack)
                    {
                        var clear = Request.QueryString["Clear"];
                        if (_idSearch.HasValue && string.IsNullOrWhiteSpace(clear)) ShowAll();
                        else ShowSearch();
                        btnExportLower.Visible = _idSearch.HasValue && string.IsNullOrWhiteSpace(clear);
                        btnDeleteSearch.Visible = (string.IsNullOrWhiteSpace(clear) || clear == "true") && _idSearch.HasValue;
                    }

                    btnSaveLayout.Visible = false;
                    btnClearLayout.Visible = false;
                    btnExport.Visible = false;
                    btnColumns.Visible = false;
                    btnReset.Visible = false;

                    break;
            }

            BindDynamicControls(null);
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            var userGridSettings = _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            if (userGridSettings != null && !String.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                AlerterGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
            } 
            
            if (!_idSearch.HasValue || Request.QueryString["Clear"] == "true") return;

            var savedAlertSearch = _alertSavedSearchOperations.GetEntity(_idSearch);

            if (savedAlertSearch == null) return;

            if (ListType == ListType.Search)
            {
                BindProduct(savedAlertSearch.product_FK);
                BindAuthorisedProduct(savedAlertSearch.ap_FK);
                BindDocument(savedAlertSearch.document_FK);
                BindProject(savedAlertSearch.project_FK);
                BindActivity(savedAlertSearch.activity_FK);
                BindTask(savedAlertSearch.task_FK);
                if (savedAlertSearch.send_mail.HasValue) rbYnSendEmail.SelectedValue = savedAlertSearch.send_mail.Value;
                ddlReminderRepeatMode.SelectedId = savedAlertSearch.reminder_repeating_mode_FK;
            }

            AlerterGrid.SetClientLayout(savedAlertSearch.gridLayout);
        }

        private void BindAuthorisedProduct(int? apFk)
        {
            var authorisedProduct = _authorisedProductOperations.GetEntity(apFk);
            if (authorisedProduct == null || authorisedProduct.ap_PK == null) return;

            txtSrAuthorisedProduct.SelectedEntityId = authorisedProduct.ap_PK;
            txtSrAuthorisedProduct.Text = !string.IsNullOrWhiteSpace(authorisedProduct.product_name) ? authorisedProduct.product_name : Constant.MissingValue;
        }

        private void BindDocument(int? documentFk)
        {
            var document = _documentOperations.GetEntity(documentFk);
            if (document == null || document.document_PK == null) return;

            txtSrDocument.SelectedEntityId = document.document_PK;
            txtSrDocument.Text = !string.IsNullOrWhiteSpace(document.name) ? document.name : Constant.MissingValue;
        }

        private void BindProject(int? projectFk)
        {
            var project = _projectOperations.GetEntity(projectFk);
            if (project == null || project.project_PK == null) return;

            txtSrProject.SelectedEntityId = project.project_PK;
            txtSrProject.Text = !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.MissingValue;
        }

        private void BindActivity(int? activityFk)
        {
            var activity = _activityOperations.GetEntity(activityFk);
            if (activity == null || activity.activity_PK == null) return;

            txtSrActivity.SelectedEntityId = activity.activity_PK;
            txtSrActivity.Text = !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.MissingValue;
        }

        private void BindProduct(int? productFk)
        {
            var product = _productOperations.GetEntity(productFk);
            if (product == null || product.product_PK == null) return;

            txtSrProduct.SelectedEntityId = product.product_PK;
            txtSrProduct.Text = product.GetNameFormatted();
        }

        private void BindTask(int? taskFk)
        {
            var task = _taskOperations.GetEntity(taskFk);
            if (task == null || task.task_PK == null || task.task_name_FK == null) return;
            var taskName = _taskNameOperations.GetEntity(task.task_name_FK);

            txtSrTask.SelectedEntityId = task.task_PK;
            txtSrTask.Text = !string.IsNullOrWhiteSpace(taskName.task_name) ? taskName.task_name : Constant.MissingValue;
        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (AlerterGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(AlerterGrid.SecondSortingColumn, AlerterGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (AlerterGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(AlerterGrid.MainSortingColumn, AlerterGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("reminder_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _reminderOperations.GetListFormSearchDataSet(filters, AlerterGrid.CurrentPage, AlerterGrid.PageSize, gobList, out itemCount);
                else ds = _reminderOperations.GetListFormDataSet(filters, AlerterGrid.CurrentPage, AlerterGrid.PageSize, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _reminderOperations.GetListFormSearchDataSet(filters, AlerterGrid.CurrentPage, AlerterGrid.PageSize, gobList, out itemCount);
            }

            AlerterGrid.TotalRecords = itemCount;

            if (AlerterGrid.CurrentPage > AlerterGrid.TotalPages || (AlerterGrid.CurrentPage == 0 && AlerterGrid.TotalPages > 0))
            {
                if (AlerterGrid.CurrentPage > AlerterGrid.TotalPages) AlerterGrid.CurrentPage = AlerterGrid.TotalPages; else AlerterGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    if (_idSearch.HasValue) ds = _reminderOperations.GetListFormSearchDataSet(filters, AlerterGrid.CurrentPage, AlerterGrid.PageSize, gobList, out itemCount);
                    else ds = _reminderOperations.GetListFormDataSet(filters, AlerterGrid.CurrentPage, AlerterGrid.PageSize, gobList, out itemCount);

                }
                else if (ListType == ListType.Search)
                {
                    ds = _reminderOperations.GetListFormSearchDataSet(filters, AlerterGrid.CurrentPage, AlerterGrid.PageSize, gobList, out itemCount);
                }
            }

            AlerterGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            AlerterGrid.DataBind();

            MasterPage.UpCommon.Update();
        }

        private void BindDynamicControls(object args)
        {
            if (ListType == ListType.Search) subtabs.Controls.Clear();
        }
        private void BindReminderRepeatingMode()
        {
            var reminderRepeatingModeList = _reminderRepeatingModeOperations.GetEntities();
            ddlReminderRepeatMode.Fill(reminderRepeatingModeList, "name", "reminder_repeating_mode_PK");
            ddlReminderRepeatMode.SortItemsByText();
        }

        #endregion

        #region Validate

        void ValidateForm(object arg)
        {

        }

        #endregion

        #region Save

        void SaveForm(object arg)
        {

        }

        #endregion

        #region Delete

        private void DeleteEntity(int? entityPk)
        {
            if (!SecurityHelper.IsPermitted(Permission.DeleteAlerter)) return;

            if (entityPk.HasValue)
            {
                try
                {
                    var reminder = _reminderOperations.GetEntity(entityPk);

                    if (reminder != null)
                    {
                        if (reminder.is_automatic == true)
                        {
                            _reminderOperations.DismissReminder(entityPk.Value);
                        }
                        else
                        {
                            _reminderOperations.Delete(entityPk);
                        }

                        var showAll = btnToggleAlert.Text.Trim().ToLower() == "show all" ? "&ShowAll=False" : "&ShowAll=True";
                        if (!SecurityHelper.IsPermitted(Permission.ShowAlertToggleButton)) showAll = string.Empty;

                        if (EntityContext == EntityContext.Alerter) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}{1}", EntityContext, showAll));
                        else if (EntityContext == EntityContext.AuthorisedProduct && _idAuthProd.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&idAuthProd={1}{2}", EntityContext, _idAuthProd, showAll));
                        else if (EntityContext == EntityContext.Product && _idProd.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&idProd={1}{2}", EntityContext, _idProd, showAll));
                        else if (EntityContext == EntityContext.Project && _idProj.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&idProj={1}{2}", EntityContext, _idProj, showAll));
                        else if (EntityContext == EntityContext.Task && _idTask.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&idTask={1}{2}", EntityContext, _idTask, showAll));
                        else if (EntityContext == EntityContext.Document && _idDoc.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&idDoc={1}{2}", EntityContext, _idDoc, showAll));
                        else if (EntityContext == EntityContext.Activity && _idAct.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&idAct={1}{2}", EntityContext, _idAct, showAll));
                        else if (EntityContext == EntityContext.ActivityMy && _idAct.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&idAct={1}{2}", EntityContext, _idAct, showAll));

                        Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}{1}", EntityContext.Alerter, showAll));
                    }
                }
                catch (Exception e)
                {
                    if (MasterPage != null) MasterPage.ModalPopup.ShowModalPopup("Message", "<br /><center>Can't be deleted.</center><br />");
                }
            }
        }

        #endregion

        #endregion

        #region Event handlers

        #region Actions

        public void btnEditEntity_OnClick(object sender, EventArgs e)
        {
            var editButton = sender as ImageButton;
            if (editButton == null) return;

            if (editButton.CommandName == Constant.CommandArgument.Edit && ValidationHelper.IsValidInt(editButton.CommandArgument))
            {
                if (SecurityHelper.IsPermitted(Permission.EditAlerter))
                {
                    MasterPage.OneTimePermissionToken = Permission.View;
                    var idAlert = editButton.CommandArgument;
                    var showAll = btnToggleAlert.Text.Trim().ToLower() == "show all" ? "&ShowAll=False" : "&ShowAll=True";
                    if (!SecurityHelper.IsPermitted(Permission.ShowAlertToggleButton)) showAll = string.Empty;

                    var query = Request.QueryString["idLay"] != null ? string.Format("&idLay={0}", Request.QueryString["idLay"]) : null;

                    if (EntityContext == EntityContext.Alerter)
                    {
                        string fromQuery = string.Empty;
                        if (ListType == ListType.Search) fromQuery = "&From=AlertSearch";
                        
                        Response.Redirect(string.Format("~/Views/AlerterView/Form.aspx?EntityContext={0}&Action=Edit&idAlert={1}{2}{3}{4}", EntityContext.Alerter, idAlert, showAll, query, fromQuery));
                    }
                    else if (EntityContext == EntityContext.AuthorisedProduct && _idAuthProd.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/Form.aspx?EntityContext={0}&idAuthProd={1}&Action=Edit&idAlert={2}&From=AuthProdAlertList{3}", EntityContext.Alerter, _idAuthProd, idAlert, showAll));
                    else if (EntityContext == EntityContext.Product && _idProd.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/Form.aspx?EntityContext={0}&idProd={1}&Action=Edit&idAlert={2}&From=ProdAlertList{3}", EntityContext.Alerter, _idProd, idAlert, showAll));
                    else if (EntityContext == EntityContext.Project && _idProj.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/Form.aspx?EntityContext={0}&idProj={1}&Action=Edit&idAlert={2}&From=ProjAlertList{3}", EntityContext.Alerter, _idProj, idAlert, showAll));
                    else if (EntityContext == EntityContext.Task && _idTask.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/Form.aspx?EntityContext={0}&idTask={1}&Action=Edit&idAlert={2}&From=TaskAlertList{3}", EntityContext.Alerter, _idTask, idAlert, showAll));
                    else if (EntityContext == EntityContext.Document && _idDoc.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/Form.aspx?EntityContext={0}&idDoc={1}&Action=Edit&idAlert={2}&From=DocAlertList{3}", EntityContext.Alerter, _idDoc, idAlert, showAll));
                    else if (EntityContext == EntityContext.Activity && _idAct.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/Form.aspx?EntityContext={0}&idAct={1}&Action=Edit&idAlert={2}&From=ActAlertList{3}", EntityContext.Alerter, _idAct, idAlert, showAll));
                    else if (EntityContext == EntityContext.ActivityMy && _idAct.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/Form.aspx?EntityContext={0}&idAct={1}&Action=Edit&idAlert={2}&From=ActMyAlertList{3}", EntityContext.Alerter, _idAct, idAlert, showAll));

                    Response.Redirect(string.Format("~/Views/AlerterView/Form.aspx?EntityContext={0}&Action=Edit&idAlert={1}{2}", EntityContext.Alerter, idAlert, showAll));
                }
            }
        }

        public void btnDeleteEntity_OnClick(object sender, EventArgs e)
        {
            var deleteButton = sender as ImageButton;
            if (deleteButton == null) return;

            var commandNameString = Convert.ToString(deleteButton.CommandName);
            var commandArgumentString = Convert.ToString(deleteButton.CommandArgument);

            if (commandNameString == Constant.CommandArgument.Delete)
            {
                _alerterPkToDelete = ValidationHelper.IsValidInt(commandArgumentString) ? int.Parse(commandArgumentString) : (int?)null;
                if (_alerterPkToDelete.HasValue) mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
            }
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_alerterPkToDelete);
        }

        #endregion

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                case ContextMenuEventTypes.Back:
                    {
                        if (EntityContext == EntityContext.Document) Response.Redirect(string.Format("~/Views/DocumentViewAll/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.AuthorisedProduct) Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Product) Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Project) Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Task) Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}", EntityContext));

                        var location = Support.LocationManager.GetFirstAuthorisedLocation();
                        Response.Redirect(location.location_url);
                    }
                    break;
            }
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in AlerterGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("reminder_PK"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (AlerterGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
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
            AlerterGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();

            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            AlerterGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            
            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = AlerterGrid.GetClientLayoutString(),
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
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (AlerterGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(AlerterGrid.SecondSortingColumn, AlerterGrid.SecondSortingOrder == SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (AlerterGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(AlerterGrid.MainSortingColumn, AlerterGrid.MainSortingOrder == SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("reminder_PK", GEMOrderByType.DESC));

            int itemCount;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _reminderOperations.GetListFormSearchDataSet(filters, AlerterGrid.CurrentPage, AlerterGrid.PageSize, gobList, out itemCount); // Quick link
                else ds = _reminderOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _reminderOperations.GetListFormSearchDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            AlerterGrid["reminder_PK"].Visible = true;
            AlerterGrid["Action"].Visible = false;
            if (ds != null) AlerterGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new ExcellExportOptions("grid"));
            AlerterGrid["reminder_PK"].Visible = false;
            AlerterGrid["Action"].Visible = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            AlerterGrid.ResetVisibleColumns();
            AlerterGrid.SecondSortingColumn = null;
            AlerterGrid.MainSortingOrder = PossGrid.SortOrder.ASC;
            BindGrid();
        }

        #endregion

        #region Grid

        void AlerterGridOnExcelCellPrepared(object sender, PossGridExportCellRenderArgs args)
        {

        }

        void AlerterGridOnHtmlRowPrepared(object sender, PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            string colorStatus = Convert.ToString(e.GetValue("ReminderUserStatus"));

            var pnlStatusColor = e.FindControl("pnlStatusColor") as HtmlGenericControl;
            if (pnlStatusColor != null)
            {
                switch (colorStatus)
                {
                    case "Done":
                    case "Canceled":
                        pnlStatusColor.Attributes.Add("class", "statusBlack");
                        break;
                    case "Pending":
                        pnlStatusColor.Attributes.Add("class", "statusYellow");
                        break;
                    case "Over due":
                        pnlStatusColor.Attributes.Add("class", "statusRed");
                        break;
                    default:
                        pnlStatusColor.Attributes.Add("class", "statusBlack");
                        break;
                }
            }

            if (Convert.ToString(e.GetValue("OverDue")) == "Yes")
            {
                foreach (var item in new[] { "hlName", "hlType" })
                {
                    var control = e.FindControl(item) as WebControl;
                    if (control != null) control.ForeColor = ColorTranslator.FromHtml("#ff0000");
                }

                e.Row.CssClass = e.Row.DataItemIndex % 2 == 0 ? "dxgvDataRow_readyRed" : "dxgvDataRowAlt_readyRed";
                e.Row.ForeColor = Color.Red;
                _isRed = true;
            }
            else
            {
                e.Row.ForeColor = Color.Black;
                _isRed = false;
            }

            var relatedEntityFk = ValidationHelper.IsValidInt(Convert.ToString(e.GetValue("related_entity_FK"))) ? (int?)Convert.ToInt32(e.GetValue("related_entity_FK")) : null;
            var entityFk = ValidationHelper.IsValidInt(Convert.ToString(e.GetValue("entity_FK"))) ? (int?)Convert.ToInt32(e.GetValue("entity_FK")) : null;
            var reminderType = Convert.ToString(e.GetValue("reminder_type"));

            var pnlProducts = e.FindControl("pnlProducts") as Panel;
            if (pnlProducts != null && entityFk.HasValue && !string.IsNullOrWhiteSpace(reminderType))
            {
                BindProducts(pnlProducts, reminderType, entityFk.Value, relatedEntityFk, _isRed);
            }
        }

        void AlerterGridOnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void AlerterGridOnHtmlCellPrepared(object sender, PossGridCellEventArgs e)
        {
            if (!AlerterGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = AlerterGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (AlerterGrid.SortCount > 1 && e.FieldName == AlerterGrid.MainSortingColumn)
                return;

            if (e.FieldName != "reminder_dates" && e.FieldName != "related_attribute_value" &&
                e.FieldName != "time_before_activation" && e.FieldName != "related_attribute_value")
            {
                e.Cell.CssClass = _flip ? "sorted_column_even" : "sorted_column_odd";
            }
            else
            {
                e.Cell.CssClass = _flip ? "sorted_column_even_right" : "sorted_column_odd_right";
            }
        }

        void AlerterGridOnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
        {

        }

        #endregion

        #region User status

        protected void BtnStatusOnClick(object sender, EventArgs e)
        {
            var btnStatus = sender as LinkButton;
            if (btnStatus == null || !ValidationHelper.IsValidInt(btnStatus.CommandArgument)) return;

            _alerterPkToChangeUserStatus = int.Parse(btnStatus.CommandArgument);

            var reminder = _reminderOperations.GetEntity(_alerterPkToChangeUserStatus);

            if (reminder == null) return;

            AlerterUserStatusPopup.ShowModalPopup(reminder.reminder_user_status_FK);
        }

        private void AlerterUserStatusPopupOnOnOkButtonClick(object sender, FormEventArgs<int?> formEventArgs)
        {
            var reminder = _reminderOperations.GetEntity(_alerterPkToChangeUserStatus);

            if (reminder == null) return;

            reminder.reminder_user_status_FK = formEventArgs.Data;

            _reminderOperations.Save(reminder);

            BindGrid();
        }

        #endregion

        #region Product searcher

        /// <summary>
        /// Handles product list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ProductSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var product = _productOperations.GetEntity(e.Data);

            if (product == null || product.product_PK == null) return;

            txtSrProduct.Text = product.name;
            txtSrProduct.SelectedEntityId = product.product_PK;
        }

        #endregion

        #region Authorised product searcher

        /// <summary>
        /// Handles authorised product list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AuthorisedProductSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var authorisedProduct = _authorisedProductOperations.GetEntity(e.Data);

            if (authorisedProduct == null || authorisedProduct.ap_PK == null) return;

            txtSrAuthorisedProduct.Text = authorisedProduct.product_name;
            txtSrAuthorisedProduct.SelectedEntityId = authorisedProduct.ap_PK;
        }

        #endregion

        #region Document searcher

        /// <summary>
        /// Handles document list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DocumentSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var document = _documentOperations.GetEntity(e.Data);

            if (document == null || document.document_PK == null) return;

            txtSrDocument.Text = !String.IsNullOrWhiteSpace(document.name) ? document.name : Constant.MissingValue;
            txtSrDocument.SelectedEntityId = document.document_PK;
        }

        #endregion

        #region Task searcher

        /// <summary>
        /// Handles task list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TaskSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var task = _taskOperations.GetEntity(e.Data);

            if (task == null || task.task_PK == null || task.task_name_FK == null) return;

            var taskName = _taskNameOperations.GetEntity(task.task_name_FK);

            if (!string.IsNullOrWhiteSpace(taskName.task_name)) txtSrTask.Text = taskName.task_name;
            else txtSrTask.Text = "Missing";
            txtSrTask.SelectedEntityId = task.task_PK;
        }

        #endregion

        #region Project searcher

        /// <summary>
        /// Handles project list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ProjectSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var project = _projectOperations.GetEntity(e.Data);

            if (project == null || project.project_PK == null) return;

            txtSrProject.Text = project.name;
            txtSrProject.SelectedEntityId = project.project_PK;
        }

        #endregion

        #region Activity searcher

        /// <summary>
        /// Handles activity list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ActivitySearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var actvivity = _activityOperations.GetEntity(e.Data);

            if (actvivity == null || actvivity.activity_PK == null) return;

            txtSrActivity.Text = actvivity.name;
            txtSrActivity.SelectedEntityId = actvivity.activity_PK;
        }

        #endregion

        #region Search buttons

        public void btnSearchClick(object sender, EventArgs e)
        {
            pnlGrid.AddCssClass("display-block");
            btnExportLower.Visible = true;
            BindGrid();
        }

        public void btnClearClick(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&Action=Search&Clear=true{1}", EntityContext.Alerter, (_idSearch.HasValue ? "&idSearch=" + _idSearch : string.Empty)));
        }

        #endregion

        #region Quick links

        public void btnSaveSearchClick(object sender, EventArgs e)
        {
            QuickLink quickLink = null;
            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                var savedSearch = _alertSavedSearchOperations.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
                if (savedSearch != null)
                {
                    quickLink = new QuickLink
                    {
                        Name = savedSearch.displayName,
                        IsPublic = savedSearch.isPublic
                    };
                }
            }

            QuickLinksPopup.ShowModalForm(quickLink);
        }

        public void btnDeleteSearchClick(object sender, EventArgs e)
        {
            if (!ValidationHelper.IsValidInt(Request.QueryString["idSearch"])) return;

            _alertSavedSearchOperations.Delete(Convert.ToInt32(Request.QueryString["idSearch"]));
            Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&Action=Search", EntityContext.Alerter));
        }

        protected void QuickLinksPopup_OnOkButtonClick(object sender, FormEventArgs<QuickLink> e)
        {
            Alert_saved_search_PK savedAlertSearch = null;

            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                savedAlertSearch = _alertSavedSearchOperations.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
            }

            if (savedAlertSearch == null)
            {
                savedAlertSearch = new Alert_saved_search_PK();
            }

            savedAlertSearch.product_FK = txtSrProduct.SelectedEntityId;
            savedAlertSearch.ap_FK = txtSrAuthorisedProduct.SelectedEntityId;
            savedAlertSearch.document_FK = txtSrDocument.SelectedEntityId;
            savedAlertSearch.project_FK = txtSrProject.SelectedEntityId;
            savedAlertSearch.activity_FK = txtSrActivity.SelectedEntityId;
            savedAlertSearch.task_FK = txtSrTask.SelectedEntityId;
            savedAlertSearch.reminder_repeating_mode_FK = ddlReminderRepeatMode.SelectedId;
            savedAlertSearch.send_mail = rbYnSendEmail.SelectedValue;

            savedAlertSearch.gridLayout = AlerterGrid.GetClientLayoutString();

            var quickLink = e.Data;
            if (quickLink != null)
            {
                savedAlertSearch.displayName = quickLink.Name;
                savedAlertSearch.isPublic = quickLink.IsPublic.HasValue && quickLink.IsPublic.Value;
            }

            var user = _userOperations.GetUserByUsername(SessionManager.Instance.CurrentUser.Username);
            if (user != null)
            {
                savedAlertSearch.user_FK = user.Person_FK;
            }

            savedAlertSearch = _alertSavedSearchOperations.Save(savedAlertSearch);
            Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&Action=Search&idSearch={1}", EntityContext.Alerter, savedAlertSearch.alert_saved_search_PK));
        }

        #endregion

        #endregion

        #region Support methods

        private void GenerateContextMenuItems()
        {
            var contextMenu = new ContextMenuItem[] { };

            if (ListType == ListType.List)
            {
                if (EntityContext != EntityContext.Alerter)
                {
                    contextMenu_ContextMenuLayout.Visible = false;
                    contextMenu = new[] { new ContextMenuItem(ContextMenuEventTypes.Back, "Back") };
                }
            }

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contextMenu);
        }

        private void GenerateTabMenuItems()
        {
            Location_PK location = null;

            if (ListType == ListType.List)
            {
                if (EntityContext == EntityContext.Alerter)
                {
                    location = Support.LocationManager.Instance.GetLocationByName("ReminderList", Support.CacheManager.Instance.AppLocations);
                    tabMenu.Visible = false;
                    if (location != null)
                    {
                        MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                        MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                    }
                    return;
                }

                if (EntityContext == EntityContext.AuthorisedProduct) location = Support.LocationManager.Instance.GetLocationByName("AuthProdAlertList", Support.CacheManager.Instance.AppLocations);
                else if (EntityContext == EntityContext.Activity) location = Support.LocationManager.Instance.GetLocationByName("ActAlertList", Support.CacheManager.Instance.AppLocations);
                else if (EntityContext == EntityContext.ActivityMy) location = Support.LocationManager.Instance.GetLocationByName("ActMyAlertList", Support.CacheManager.Instance.AppLocations);
                else if (EntityContext == EntityContext.Product) location = Support.LocationManager.Instance.GetLocationByName("ProdAlertList", Support.CacheManager.Instance.AppLocations);
                else if (EntityContext == EntityContext.Task) location = Support.LocationManager.Instance.GetLocationByName("TaskAlertList", Support.CacheManager.Instance.AppLocations);
                else if (EntityContext == EntityContext.Project) location = Support.LocationManager.Instance.GetLocationByName("ProjAlertList", Support.CacheManager.Instance.AppLocations);
                else if (EntityContext == EntityContext.Document) location = Support.LocationManager.Instance.GetLocationByName("DocAlertList", Support.CacheManager.Instance.AppLocations);

                MasterPage.TabMenu.TabControls.Clear();
                tabMenu.Visible = true;
                if (location != null)
                {
                    tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                    tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                }
            }
        }

        private DataTable PrepareDataForExport(DataTable alerterDataTable)
        {
            return alerterDataTable;
        }

        public string HandleMissing(object value)
        {
            if (value != null && value.ToString().Trim() != "")
            {
                return value.ToString();
            }

            return "Missing";
        }

        private Dictionary<string, string> GetFilters()
        {
            var filters = AlerterGrid.GetFilters();

            switch (ListType)
            {
                case ListType.List:
                    if (_idSearch.HasValue && !IsPostBack)
                    {
                        Alert_saved_search_PK savedAlertSearch = _alertSavedSearchOperations.GetEntity(_idSearch);
                        FillFilters(savedAlertSearch, filters);
                    }
                    else
                    {
                        if (btnToggleAlert.Visible)
                        {
                            var showAll = Request.QueryString["ShowAll"];
                            if (!IsPostBack && !string.IsNullOrWhiteSpace(showAll))
                            {
                                if (showAll == "True") filters.Add("IsPrivate", "0");
                                else if (showAll == "False")
                                {
                                    filters.Add("IsPrivate", "1");
                                    AddResponsibleUserFilter(filters);
                                }
                            }
                            else
                            {
                                if (btnToggleAlert.Text.Trim().ToLower() == "show all")
                                {
                                    filters.Add("IsPrivate", "1");
                                    AddResponsibleUserFilter(filters);
                                }
                                else filters.Add("IsPrivate", "0");
                            }
                        }
                        else
                        {
                            filters.Add("IsPrivate", "1");
                            AddResponsibleUserFilter(filters);
                        }

                        if (EntityContext == EntityContext.Product)
                        {
                            filters.Add("EntityContext", EntityContext.ToString());
                            filters.Add("EntityPk", Convert.ToString(_idProd));
                        }
                        else if (EntityContext == EntityContext.AuthorisedProduct)
                        {
                            filters.Add("EntityContext", EntityContext.ToString());
                            filters.Add("EntityPk", Convert.ToString(_idAuthProd));
                        }
                        else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy)
                        {
                            filters.Add("EntityContext", EntityContext.Activity.ToString());
                            filters.Add("EntityPk", Convert.ToString(_idAct));
                        }
                        else if (EntityContext == EntityContext.Task)
                        {
                            filters.Add("EntityContext", EntityContext.ToString());
                            filters.Add("EntityPk", Convert.ToString(_idTask));
                        }
                        else if (EntityContext == EntityContext.Project)
                        {
                            filters.Add("EntityContext", EntityContext.ToString());
                            filters.Add("EntityPk", Convert.ToString(_idProj));
                        }
                        else if (EntityContext == EntityContext.Document)
                        {
                            filters.Add("EntityContextContains", EntityContext.ToString());
                            filters.Add("EntityPk", Convert.ToString(_idDoc));
                        }
                    }
                    break;

                case ListType.Search:
                    FillFilters(filters);
                    break;
            }

            return filters;
        }

        private void FillFilters(Dictionary<string, string> filters)
        {
            if (txtSrProduct.SelectedEntityId.HasValue) filters.Add("SearchProductPk", txtSrProduct.SelectedEntityId.Value.ToString());
            if (txtSrAuthorisedProduct.SelectedEntityId.HasValue) filters.Add("SearchAuthorisedProductPk", txtSrAuthorisedProduct.SelectedEntityId.Value.ToString());
            if (txtSrDocument.SelectedEntityId.HasValue) filters.Add("SearchDocumentPk", txtSrDocument.SelectedEntityId.Value.ToString());
            if (txtSrProject.SelectedEntityId.HasValue) filters.Add("SearchProjectPk", txtSrProject.SelectedEntityId.Value.ToString());
            if (txtSrActivity.SelectedEntityId.HasValue) filters.Add("SearchActivityPk", txtSrActivity.SelectedEntityId.Value.ToString());
            if (txtSrTask.SelectedEntityId.HasValue) filters.Add("SearchTaskPk", txtSrTask.SelectedEntityId.Value.ToString());
            if (ddlReminderRepeatMode.SelectedId.HasValue) filters.Add("SearchReminderRepeatingModePk", ddlReminderRepeatMode.SelectedId.Value.ToString());
            if (rbYnSendEmail.SelectedItem != null) filters.Add("SearchSendEmail", rbYnSendEmail.SelectedValue != null && rbYnSendEmail.SelectedValue.Value ? "1" : "0"); else filters.Add("SearchSendEmail", "");
        }

        private void FillFilters(Alert_saved_search_PK savedAlertSearch, Dictionary<string, string> filters)
        {
            if (savedAlertSearch.product_FK.HasValue) filters.Add("SearchProductPk", savedAlertSearch.product_FK.Value.ToString());
            if (savedAlertSearch.ap_FK.HasValue) filters.Add("SearchAuthorisedProductPk", savedAlertSearch.ap_FK.Value.ToString());
            if (savedAlertSearch.document_FK.HasValue) filters.Add("SearchDocumentPk", savedAlertSearch.document_FK.Value.ToString());
            if (savedAlertSearch.product_FK.HasValue) filters.Add("SearchProjectPk", savedAlertSearch.product_FK.Value.ToString());
            if (savedAlertSearch.activity_FK.HasValue) filters.Add("SearchActivityPk", savedAlertSearch.activity_FK.Value.ToString());
            if (savedAlertSearch.task_FK.HasValue) filters.Add("SearchTaskPk", savedAlertSearch.task_FK.Value.ToString());
            if (savedAlertSearch.reminder_repeating_mode_FK.HasValue) filters.Add("SearchReminderRepeatingModePk", savedAlertSearch.reminder_repeating_mode_FK.Value.ToString());
            if (savedAlertSearch.send_mail.HasValue) filters.Add("SearchSendEmail", savedAlertSearch.send_mail.Value ? "1" : "0"); else filters.Add("SearchSendEmail", "");
        }

        private void BindProducts(Panel pnlProducts, string reminderType, int entityFk, int? relatedEntityFk, bool isRed)
        {
            DataSet ds;
            Product_PK product;
            List<Product_PK> productList;
            switch (reminderType)
            {
                case "Authorised product":
                    AuthorisedProduct authorisedProduct = _authorisedProductOperations.GetEntity(entityFk);
                    if (authorisedProduct != null)
                    {
                        product = _productOperations.GetEntity(authorisedProduct.product_FK);
                        AddProductLink(pnlProducts, product, isRed);
                    }
                    break;
                case "Activity":
                    ds = _activityProductMnOperations.GetProductsByActivity(entityFk);
                    AddProductLink(pnlProducts, ds, isRed);
                    break;
                case "Product":
                    product = _productOperations.GetEntity(entityFk);
                    AddProductLink(pnlProducts, product, isRed);
                    break;
                case "P Document":
                    productList = _productOperations.GetProductsByPDocument(entityFk, relatedEntityFk);
                    foreach (var item in productList)
                    {
                        AddProductLink(pnlProducts, item, isRed);
                    }
                    break;
                case "AP Document":
                    productList = _productOperations.GetProductsByApDocument(entityFk, relatedEntityFk);
                    foreach (var item in productList)
                    {
                        AddProductLink(pnlProducts, item, isRed);
                    }
                    break;
                case "A Document":
                    productList = _productOperations.GetProductsByADocument(entityFk, relatedEntityFk);
                    foreach (var item in productList)
                    {
                        AddProductLink(pnlProducts, item, isRed);
                    }
                    break;
                case "Task":
                    Task_PK task = _taskOperations.GetEntity(entityFk);
                    if (task != null && task.activity_FK.HasValue)
                    {
                        ds = _activityProductMnOperations.GetProductsByActivity(task.activity_FK);
                        AddProductLink(pnlProducts, ds, isRed);
                    }
                    break;
            }
        }

        private void AddProductLink(Panel pnlProducts, DataSet productDs, bool isRed)
        {
            var productDt = productDs.Tables.Count > 0 ? productDs.Tables[0] : null;
            if (productDt == null || !productDt.Columns.Contains("product_PK") || !productDt.Columns.Contains("ProductName")) return;

            foreach (DataRow dr in productDt.Rows)
            {
                var hl = new HyperLink
                {
                    NavigateUrl = string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, dr["product_PK"]),
                    Text = dr["ProductName"].ToString(),
                    Target = "_blank"
                };
                if (isRed) hl.ForeColor = ColorTranslator.FromHtml("#ff0000");
                pnlProducts.Controls.Add(hl);
                pnlProducts.Controls.Add(new LiteralControl("<br />"));
            }
        }

        private void AddProductLink(Panel pnlProducts, Product_PK product, bool isRed)
        {
            if (product == null) return;

            var hl = new HyperLink
            {
                NavigateUrl = string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, product.product_PK),
                Text = product.GetNameFormatted(string.Empty),
                Target = "_blank"
            };
            if (isRed) hl.ForeColor = ColorTranslator.FromHtml("#ff0000");
            pnlProducts.Controls.Add(hl);
            pnlProducts.Controls.Add(new LiteralControl("<br />"));
        }

        private void AddResponsibleUserFilter(Dictionary<string, string> filters)
        {
            var user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null && user.Person_FK.HasValue)
            {
                filters.Add("ResponsibleUserFk", user.Person_FK.Value.ToString());
            }
        }

        private void HandleEntityContextProduct()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Product:";

            var product = _productOperations.GetEntity(_idProd);

            lblPrvParentEntity.Text = product != null && !string.IsNullOrWhiteSpace(product.name) ? product.name : Constant.ControlDefault.LbPrvText;
        }

        private void HandleEntityContextAuthorisedProduct()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Authorised product:";

            var authorisedProduct = _authorisedProductOperations.GetEntity(_idAuthProd);

            lblPrvParentEntity.Text = authorisedProduct != null && !string.IsNullOrWhiteSpace(authorisedProduct.product_name) ? authorisedProduct.product_name : Constant.ControlDefault.LbPrvText;
        }

        private void HandleEntityContextDocument()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Document:";

            var document = _documentOperations.GetEntity(_idDoc);

            lblPrvParentEntity.Text = document != null && !string.IsNullOrWhiteSpace(document.name) ? document.name : Constant.ControlDefault.LbPrvText;
        }

        private void HandleEntityContextProject()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Project:";

            var project = _projectOperations.GetEntity(_idProj);

            lblPrvParentEntity.Text = project != null && !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.ControlDefault.LbPrvText;
        }

        private void HandleEntityContextActivity()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Activity:";

            var activity = _activityOperations.GetEntity(_idAct);

            lblPrvParentEntity.Text = activity != null && !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.ControlDefault.LbPrvText;
        }

        private void HandleEntityContextTask()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Task:";

            var task = _taskOperations.GetEntity(_idTask);

            if (task == null) return;

            var taskName = _taskNameOperations.GetEntity(task.task_name_FK);

            lblPrvParentEntity.Text = taskName != null && !string.IsNullOrWhiteSpace(taskName.task_name) ? taskName.task_name : Constant.ControlDefault.LbPrvText;
        }

        private void ClearSearch()
        {
            txtSrProduct.Text = string.Empty;
            txtSrAuthorisedProduct.Text = string.Empty;
            txtSrDocument.Text = string.Empty;
            txtSrProject.Text = string.Empty;
            txtSrActivity.Text = string.Empty;
            txtSrTask.Text = string.Empty;
            ddlReminderRepeatMode.SelectedValue = string.Empty;
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            base.SecurityPageSpecific();

            if (SecurityHelper.IsPermitted(Permission.EditAlerter))
            {
                SecurityHelper.EnableImageButtonsWithCommandName(PnlGrid, "Edit");
            }
            else SecurityHelper.DisableImageButtonsWithCommandName(PnlGrid, "Edit");

            if (SecurityHelper.IsPermitted(Permission.DeleteAlerter))
            {
                SecurityHelper.EnableImageButtonsWithCommandName(PnlGrid, "Delete");
            }
            else SecurityHelper.DisableImageButtonsWithCommandName(PnlGrid, "Delete");

            return true;
        }

        #endregion
    }
}