using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Template;
using AspNetUIFramework;
using GEM2Common;
using Ready.Model;

namespace AspNetUI.Views.SentMessageView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;

        private ISent_message_PKOperations _sentMessageOperations;

        #endregion

        #region Properties

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

        #endregion

        #region Form methods

        #region Initialize

        public override void LoadFormVariables()
        {
            base.LoadFormVariables();
            LoadActionQuery();

            _sentMessageOperations = new Sent_message_PKDAL();
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }

            switch (ListType)
            {
                case ListType.List:
                    btnExport.Click += btnExport_Click;

                    break;
            }

            SentMessageReportGrid.OnRebindRequired += SentMessageReportGrid_OnRebindRequired;
            SentMessageReportGrid.OnHtmlRowPrepared += SentMessageReportGrid_OnHtmlRowPrepared;
            SentMessageReportGrid.OnHtmlCellPrepared += SentMessageReportGrid_OnHtmlCellPrepared;
            SentMessageReportGrid.OnExcelCellPrepared += SentMessageReportGrid_OnExcelCellPrepared;
            SentMessageReportGrid.OnLoadClientLayout += SentMessageReportGrid_OnLoadClientLayout;
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
            if (SentMessageReportGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(SentMessageReportGrid.SecondSortingColumn, SentMessageReportGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (SentMessageReportGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(SentMessageReportGrid.MainSortingColumn, SentMessageReportGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("sent_message_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _sentMessageOperations.GetListFormDataSet(filters, SentMessageReportGrid.CurrentPage, SentMessageReportGrid.PageSize, gobList, out itemCount);
            }

            SentMessageReportGrid.TotalRecords = itemCount;

            if (SentMessageReportGrid.CurrentPage > SentMessageReportGrid.TotalPages || (SentMessageReportGrid.CurrentPage == 0 && SentMessageReportGrid.TotalPages > 0))
            {
                if (SentMessageReportGrid.CurrentPage > SentMessageReportGrid.TotalPages) SentMessageReportGrid.CurrentPage = SentMessageReportGrid.TotalPages; else SentMessageReportGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    ds = _sentMessageOperations.GetListFormDataSet(filters, SentMessageReportGrid.CurrentPage, SentMessageReportGrid.PageSize, gobList, out itemCount);
                }
            }

            SentMessageReportGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            SentMessageReportGrid.DataBind();

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

        }

        #endregion

        #endregion

        #region Event handlers

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {

        }

        #endregion

        #region Grid

        void SentMessageReportGrid_OnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {

        }

        void btnExport_Click(object sender, EventArgs e)
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>() { };
            if (SentMessageReportGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(SentMessageReportGrid.SecondSortingColumn, SentMessageReportGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (SentMessageReportGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(SentMessageReportGrid.MainSortingColumn, SentMessageReportGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("sent_message_PK", GEMOrderByType.DESC));

            int itemCount;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _sentMessageOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            SentMessageReportGrid["sent_message_PK"].Visible = true;
            if (ds != null) SentMessageReportGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            SentMessageReportGrid["sent_message_PK"].Visible = false;

        }

        void SentMessageReportGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            var lbtnDelete = e.FindControl("lbtnDeleteEntity") as LinkButton;

            if (lbtnDelete == null) return;

            var scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
            {
                scriptManager.RegisterAsyncPostBackControl(lbtnDelete);
            }
        }

        void SentMessageReportGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void SentMessageReportGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!SentMessageReportGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = SentMessageReportGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (SentMessageReportGrid.SortCount > 1 && e.FieldName == SentMessageReportGrid.MainSortingColumn)
                return;
        }

        void SentMessageReportGrid_OnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
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
                location =  Support.LocationManager.Instance.GetLocationByName("Level3-SentMsgStatsList", Support.CacheManager.Instance.AppLocations);
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
                location =  Support.LocationManager.Instance.GetLocationByName("Level3-SentMsgStatsList", Support.CacheManager.Instance.AppLocations);
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
            var filters = SentMessageReportGrid.GetFilters();

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