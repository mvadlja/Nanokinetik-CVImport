using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using AspNetUIFramework;
using GEM2Common;
using Ready.Model;

namespace AspNetUI.Views.ServiceLogView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;

        private IService_log_PKOperations _serviceLogOperations;

        #endregion

        #region Properties

        private int? _serviceLogPkToDelete
        {
            get { return (int?)ViewState["_serviceLogPkToDelete"]; }
            set { ViewState["_serviceLogPkToDelete"] = value; }
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
            }

            if (!IsPostBack)
            {
                InitForm(null);
                BindForm(null);
            }

            BindGrid();
            SetFormControlsDefaults(null);
            SecurityPageSpecific();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            GenerateTabMenuItems();
            GenerateTopMenuItems();
            MasterPage.UpTopMenu.Update();
        }

        #endregion

        #region Form methods

        #region Initialize

        public override void LoadFormVariables()
        {
            base.LoadFormVariables();
            LoadActionQuery();

            _serviceLogOperations = new Service_log_PKDAL();
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
                mpDelete.OnYesButtonClick += mpDelete_OnYesButtonClick;
            }

            switch (ListType)
            {
                case ListType.List:
                    btnExport.Click += btnExport_Click;

                    break;
            }

            ServiceLogGrid.OnRebindRequired += ServiceLogGrid_OnRebindRequired;
            ServiceLogGrid.OnHtmlRowPrepared += ServiceLogGrid_OnHtmlRowPrepared;
            ServiceLogGrid.OnHtmlCellPrepared += ServiceLogGrid_OnHtmlCellPrepared;
            ServiceLogGrid.OnExcelCellPrepared += ServiceLogGrid_OnExcelCellPrepared;
            ServiceLogGrid.OnLoadClientLayout += ServiceLogGrid_OnLoadClientLayout;
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
            }
        }

        void FillFormControls(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    break;
            }
        }

        void SetFormControlsDefaults(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    break;
            }
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {

        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (ServiceLogGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(ServiceLogGrid.SecondSortingColumn, ServiceLogGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (ServiceLogGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(ServiceLogGrid.MainSortingColumn, ServiceLogGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("service_log_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _serviceLogOperations.GetListFormDataSet(filters, ServiceLogGrid.CurrentPage, ServiceLogGrid.PageSize, gobList, out itemCount);
            }

            ServiceLogGrid.TotalRecords = itemCount;

            if (ServiceLogGrid.CurrentPage > ServiceLogGrid.TotalPages || (ServiceLogGrid.CurrentPage == 0 && ServiceLogGrid.TotalPages > 0))
            {
                if (ServiceLogGrid.CurrentPage > ServiceLogGrid.TotalPages) ServiceLogGrid.CurrentPage = ServiceLogGrid.TotalPages; else ServiceLogGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    ds = _serviceLogOperations.GetListFormDataSet(filters, ServiceLogGrid.CurrentPage, ServiceLogGrid.PageSize, gobList, out itemCount);
                }
            }

            ServiceLogGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            ServiceLogGrid.DataBind();

            MasterPage.UpCommon.Update();
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
            if (entityPk.HasValue)
            {
                _serviceLogOperations.Delete(entityPk);
                Response.Redirect("~/Views/ServiceLogView/List.aspx?EntityContext=" + EntityContext.ServiceLogStats);
            }

            MasterPage.ModalPopup.ShowModalPopup("Error!", "Could not delete entity! Contact your system administrator.");
        }

        #endregion

        #endregion

        #region Event handlers

        #region Delete

        public void btnDeleteEntity_OnClick(object sender, EventArgs e)
        {
            var deleteButton = sender as ImageButton;
            if (deleteButton == null) return;

            var commandNameString = Convert.ToString(deleteButton.CommandName);
            var commandArgumentString = Convert.ToString(deleteButton.CommandArgument);

            if (commandNameString == Constant.CommandArgument.Delete)
            {
                _serviceLogPkToDelete = ValidationHelper.IsValidInt(commandArgumentString) ? int.Parse(commandArgumentString) : (int?)null;
                if (_serviceLogPkToDelete.HasValue) mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
            }
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_serviceLogPkToDelete);
        }

        #endregion

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {

        }

        #endregion

        #region Grid

        void ServiceLogGrid_OnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {

        }

        void btnExport_Click(object sender, EventArgs e)
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>() { };
            if (ServiceLogGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(ServiceLogGrid.SecondSortingColumn, ServiceLogGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (ServiceLogGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(ServiceLogGrid.MainSortingColumn, ServiceLogGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("service_log_PK", GEMOrderByType.DESC));

            int itemCount;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _serviceLogOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            ServiceLogGrid["service_log_PK"].Visible = true;
            if (ds != null) ServiceLogGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            ServiceLogGrid["service_log_PK"].Visible = false;

        }

        void ServiceLogGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            var lbtnDelete = e.FindControl("lbtnDeleteEntity") as LinkButton;

            if (lbtnDelete == null) return;

            var scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
            {
                scriptManager.RegisterAsyncPostBackControl(lbtnDelete);
            }
        }

        void ServiceLogGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void ServiceLogGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!ServiceLogGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = ServiceLogGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (ServiceLogGrid.SortCount > 1 && e.FieldName == ServiceLogGrid.MainSortingColumn)
                return;
        }

        void ServiceLogGrid_OnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
        {

        }

        #endregion

        #endregion

        #region Support methods

        private void GenerateContextMenuItems()
        {

        }

        private void GenerateTabMenuItems()
        {
            Location_PK location;

            if (ListType == ListType.List)
            {
                location =  Support.LocationManager.Instance.GetLocationByName("Level3-ServiceLogStatsList", Support.CacheManager.Instance.AppLocations);
                MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                tabMenu.Visible = false;
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location = null;

            if (ListType == ListType.List)
            {
                location =  Support.LocationManager.Instance.GetLocationByName("Level3-ServiceLogStatsList", Support.CacheManager.Instance.AppLocations);
            }

            var topLevelParent = MasterPage.FindTopLevelParent(location);

            MasterPage.CurrentLocation = location;
            MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
        }

        private DataTable PrepareDataForExport(DataTable atcDataTable)
        {
            if (atcDataTable == null || atcDataTable.Rows.Count == 0) return atcDataTable;

            return atcDataTable;
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
            var filters = ServiceLogGrid.GetFilters();

            return filters;
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            return true;
        }

        #endregion
    }
}