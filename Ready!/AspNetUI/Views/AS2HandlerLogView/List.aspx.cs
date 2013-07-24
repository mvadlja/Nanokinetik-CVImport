using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Template;
using AspNetUIFramework;
using GEM2Common;
using Ready.Model;

namespace AspNetUI.Views.AS2HandlerLogView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;

        private IAs2_handler_log_PKOperations _as2HandlerLogOperations;

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

            _as2HandlerLogOperations = new As2_handler_log_PKDAL();
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

            AS2HandlerLogGrid.OnRebindRequired += As2HandlerLogGridOnRebindRequired;
            AS2HandlerLogGrid.OnHtmlRowPrepared += As2HandlerLogGridOnHtmlRowPrepared;
            AS2HandlerLogGrid.OnHtmlCellPrepared += As2HandlerLogGridOnHtmlCellPrepared;
            AS2HandlerLogGrid.OnExcelCellPrepared += As2HandlerLogGridOnExcelCellPrepared;
            AS2HandlerLogGrid.OnLoadClientLayout += As2HandlerLogGridOnLoadClientLayout;
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
            if (AS2HandlerLogGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(AS2HandlerLogGrid.SecondSortingColumn, AS2HandlerLogGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (AS2HandlerLogGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(AS2HandlerLogGrid.MainSortingColumn, AS2HandlerLogGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("as2_handler_log_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _as2HandlerLogOperations.GetListFormDataSet(filters, AS2HandlerLogGrid.CurrentPage, AS2HandlerLogGrid.PageSize, gobList, out itemCount);
            }

            AS2HandlerLogGrid.TotalRecords = itemCount;

            if (AS2HandlerLogGrid.CurrentPage > AS2HandlerLogGrid.TotalPages || (AS2HandlerLogGrid.CurrentPage == 0 && AS2HandlerLogGrid.TotalPages > 0))
            {
                if (AS2HandlerLogGrid.CurrentPage > AS2HandlerLogGrid.TotalPages) AS2HandlerLogGrid.CurrentPage = AS2HandlerLogGrid.TotalPages; else AS2HandlerLogGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    ds = _as2HandlerLogOperations.GetListFormDataSet(filters, AS2HandlerLogGrid.CurrentPage, AS2HandlerLogGrid.PageSize, gobList, out itemCount);
                }
            }

            AS2HandlerLogGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            AS2HandlerLogGrid.DataBind();

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
            switch (e.EventType)
            {
                default:
                    break;
            }
        }

        #endregion

        #region Grid

        void As2HandlerLogGridOnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {

        }

        void btnExport_Click(object sender, EventArgs e)
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>() { };
            if (AS2HandlerLogGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(AS2HandlerLogGrid.SecondSortingColumn, AS2HandlerLogGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (AS2HandlerLogGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(AS2HandlerLogGrid.MainSortingColumn, AS2HandlerLogGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("as2_handler_log_PK", GEMOrderByType.DESC));

            int itemCount;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _as2HandlerLogOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            AS2HandlerLogGrid["as2_handler_log_PK"].Visible = true;
            if (ds != null) AS2HandlerLogGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            AS2HandlerLogGrid["as2_handler_log_PK"].Visible = false;

        }

        void As2HandlerLogGridOnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            var hlMsgData = e.FindControl("hlMsgData") as HyperLink;
            if (hlMsgData != null)
            {
                hlMsgData.NavigateUrl = "~/Views/ReceivedMessagesReportView/List.aspx?idMsg=" + e.GetValue("received_message_FK");
            }

            var hlEntity = e.FindControl("hlEntity") as HyperLink;
            if (hlEntity != null)
            {
                switch (Convert.ToString(e.GetValue("entity_type_FK")))
                {
                    case "1":
                        hlEntity.NavigateUrl = "~/Views/AuthorisedProductView/Preview.aspx?idAuthProd=" + e.GetValue("entity_FK");
                        break;
                    default:
                        break;
                }
            }
        }

        void As2HandlerLogGridOnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void As2HandlerLogGridOnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!AS2HandlerLogGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = AS2HandlerLogGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (AS2HandlerLogGrid.SortCount > 1 && e.FieldName == AS2HandlerLogGrid.MainSortingColumn)
                return;
        }

        void As2HandlerLogGridOnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
        {

        }

        #endregion

        #endregion

        #region Support methods

        private void GenerateContextMenuItems()
        {
            var contexMenu = new ContextMenuItem[] { };

            if (ListType == ListType.List)
            {
                contexMenu = new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") };
            }

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contexMenu);
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
            var filters = AS2HandlerLogGrid.GetFilters();

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