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

namespace AspNetUI.Views.ReceivedMessageView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;

        private IRecieved_message_PKOperations _recievedMessageOperations;

        #endregion

        #region Properties

        private int? _recievedMessagePkToDelete
        {
            get { return (int?)ViewState["_recievedMessagePkToDelete"]; }
            set { ViewState["_recievedMessagePkToDelete"] = value; }
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

            _recievedMessageOperations = new Recieved_message_PKDAL();
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

            ReceivedMessageGrid.OnRebindRequired += ReceivedMessageGrid_OnRebindRequired;
            ReceivedMessageGrid.OnHtmlRowPrepared += ReceivedMessageGrid_OnHtmlRowPrepared;
            ReceivedMessageGrid.OnHtmlCellPrepared += ReceivedMessageGrid_OnHtmlCellPrepared;
            ReceivedMessageGrid.OnExcelCellPrepared += ReceivedMessageGrid_OnExcelCellPrepared;
            ReceivedMessageGrid.OnLoadClientLayout += ReceivedMessageGrid_OnLoadClientLayout;
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
            if (ReceivedMessageGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(ReceivedMessageGrid.SecondSortingColumn, ReceivedMessageGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (ReceivedMessageGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(ReceivedMessageGrid.MainSortingColumn, ReceivedMessageGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("recieved_message_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _recievedMessageOperations.GetListFormDataSet(filters, ReceivedMessageGrid.CurrentPage, ReceivedMessageGrid.PageSize, gobList, out itemCount);
            }

            ReceivedMessageGrid.TotalRecords = itemCount;

            if (ReceivedMessageGrid.CurrentPage > ReceivedMessageGrid.TotalPages || (ReceivedMessageGrid.CurrentPage == 0 && ReceivedMessageGrid.TotalPages > 0))
            {
                if (ReceivedMessageGrid.CurrentPage > ReceivedMessageGrid.TotalPages) ReceivedMessageGrid.CurrentPage = ReceivedMessageGrid.TotalPages; else ReceivedMessageGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    ds = _recievedMessageOperations.GetListFormDataSet(filters, ReceivedMessageGrid.CurrentPage, ReceivedMessageGrid.PageSize, gobList, out itemCount);
                }
            }

            ReceivedMessageGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            ReceivedMessageGrid.DataBind();

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
                _recievedMessageOperations.Delete(entityPk);
                Response.Redirect("~/Views/ReceivedMessageView/List.aspx?EntityContext=" + EntityContext.ReceivedMessagesStats);
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
                _recievedMessagePkToDelete = ValidationHelper.IsValidInt(commandArgumentString) ? int.Parse(commandArgumentString) : (int?)null;
                if (_recievedMessagePkToDelete.HasValue) mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
            }
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_recievedMessagePkToDelete);
        }

        #endregion

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {

        }

        #endregion

        #region Grid

        void ReceivedMessageGrid_OnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {

        }

        void btnExport_Click(object sender, EventArgs e)
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>() { };
            if (ReceivedMessageGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(ReceivedMessageGrid.SecondSortingColumn, ReceivedMessageGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (ReceivedMessageGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(ReceivedMessageGrid.MainSortingColumn, ReceivedMessageGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("recieved_message_PK", GEMOrderByType.DESC));

            int itemCount;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _recievedMessageOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            ReceivedMessageGrid["recieved_message_PK"].Visible = true;
            if (ds != null) ReceivedMessageGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            ReceivedMessageGrid["recieved_message_PK"].Visible = false;

        }

        void ReceivedMessageGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            var lbtnDelete = e.FindControl("lbtnDeleteEntity") as LinkButton;

            if (lbtnDelete == null) return;

            var scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
            {
                scriptManager.RegisterAsyncPostBackControl(lbtnDelete);
            }
        }

        void ReceivedMessageGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void ReceivedMessageGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!ReceivedMessageGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = ReceivedMessageGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (ReceivedMessageGrid.SortCount > 1 && e.FieldName == ReceivedMessageGrid.MainSortingColumn)
                return;
        }

        void ReceivedMessageGrid_OnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
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
                location =  Support.LocationManager.Instance.GetLocationByName("Level3-ReceivedMsgStatsList", Support.CacheManager.Instance.AppLocations);
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
                location =  Support.LocationManager.Instance.GetLocationByName("Level3-ReceivedMsgStatsList", Support.CacheManager.Instance.AppLocations);
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
            var filters = ReceivedMessageGrid.GetFilters();

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