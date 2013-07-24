using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Template;
using AspNetUIFramework;
using GEM2Common;
using PossGrid;
using Ready.Model;

namespace AspNetUI.Views.AuditTrailView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;
        private int? _idAct;
        private int? _idProd;
        private int? _idAuthProd;
        private int? _idDoc;
        private int? _idPharmProd;
        private int? _idAlert;

        private IAuditingMasterOperations _auditMasterOperations;
        private IAuditingDetailOperations _auditingDetailOperations;
        private IActivity_PKOperations _activityOperations;
        private IProduct_PKOperations _productOperations;
        private IAuthorisedProductOperations _authorisedProductOperations;
        private IPharmaceutical_product_PKOperations _pharmaceuticalProductOperations;
        private IDocument_PKOperations _documentOperations;
        private IReminder_PKOperations _reminderOperations;

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

            if (!IsPostBack)
            {
                InitForm(null);
                BindForm(null);
            }

            BindGridMaster();
            BindGridDetails(null);
            SetFormControlsDefaults(null);
            SecurityPageSpecific();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            GenerateTabMenuItems(); ;
            MasterPage.UpTopMenu.Update();
        }

        #endregion

        #region Form methods

        #region Initialize

        public override void LoadFormVariables()
        {
            base.LoadFormVariables();
            LoadActionQuery();

            _idAuthProd = ValidationHelper.IsValidInt(Request.QueryString["idAuthProd"]) ? int.Parse(Request.QueryString["idAuthProd"]) : (int?)null;
            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;
            _idDoc = ValidationHelper.IsValidInt(Request.QueryString["idDoc"]) ? int.Parse(Request.QueryString["idDoc"]) : (int?)null;
            _idPharmProd = ValidationHelper.IsValidInt(Request.QueryString["idPharmProd"]) ? int.Parse(Request.QueryString["idPharmProd"]) : (int?)null;
            _idAct = ValidationHelper.IsValidInt(Request.QueryString["idAct"]) ? int.Parse(Request.QueryString["idAct"]) : (int?)null;
            _idAlert = ValidationHelper.IsValidInt(Request.QueryString["idAlert"]) ? int.Parse(Request.QueryString["idAlert"]) : (int?)null;

            _activityOperations = new Activity_PKDAL();
            _productOperations = new Product_PKDAL();
            _authorisedProductOperations = new AuthorisedProductDAL();
            _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
            _documentOperations = new Document_PKDAL();
            _reminderOperations = new Reminder_PKDAL();
            _auditMasterOperations = new AuditingMasterDAL();
            _auditingDetailOperations = new AuditingDetailDAL();

            MasterGrid.GridVersion = MasterGrid.GridVersion + EntityContext.ToString();
            DetailsGrid.GridVersion = DetailsGrid.GridVersion + EntityContext.ToString();
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

                    break;
            }

            MasterGrid.OnRebindRequired += MasterGrid_OnRebindRequired;
            MasterGrid.OnHtmlRowPrepared += MasterGrid_OnHtmlRowPrepared;
            MasterGrid.OnHtmlCellPrepared += MasterGrid_OnHtmlCellPrepared;
            MasterGrid.OnExcelCellPrepared += MasterGrid_OnExcelCellPrepared;
            MasterGrid.OnLoadClientLayout += MasterGrid_OnLoadClientLayout;

            DetailsGrid.OnRebindRequired += DetailsGrid_OnRebindRequired;
            DetailsGrid.OnHtmlRowPrepared += DetailsGrid_OnHtmlRowPrepared;
            DetailsGrid.OnHtmlCellPrepared += DetailsGrid_OnHtmlCellPrepared;
            DetailsGrid.OnExcelCellPrepared += DetailsGrid_OnExcelCellPrepared;
            DetailsGrid.OnLoadClientLayout += DetailsGrid_OnLoadClientLayout;
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
                    hfSessionToken.Value = string.Empty;
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
                    if (EntityContext == EntityContext.Product) HandleEntityContextProduct();
                    else if (EntityContext == EntityContext.AuthorisedProduct) HandleEntityContextAuthorisedProduct();
                    else if (EntityContext == EntityContext.Product) HandleEntityContextProduct();
                    else if (EntityContext == EntityContext.PharmaceuticalProduct) HandleEntityContextPharmaceuticalProduct();
                    else if (EntityContext == EntityContext.Document) HandleEntityContextDocument();
                    else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) HandleEntityContextActivity();
                    else if (EntityContext == EntityContext.Alerter) HandleEntityContextAlerter();

                    break;
            }
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {

        }

        private void BindGridMaster()
        {
            var filters = GetFiltersMaster();

            var gobList = new List<GEMOrderBy>();
            if (MasterGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(MasterGrid.SecondSortingColumn, MasterGrid.SecondSortingOrder == SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (MasterGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(MasterGrid.MainSortingColumn, MasterGrid.MainSortingOrder == SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("ChangeDate", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _auditMasterOperations.GetListFormDataSet(filters, MasterGrid.CurrentPage, MasterGrid.PageSize, gobList, out itemCount);
            }

            MasterGrid.TotalRecords = itemCount;

            if (MasterGrid.CurrentPage > MasterGrid.TotalPages || (MasterGrid.CurrentPage == 0 && MasterGrid.TotalPages > 0))
            {
                if (MasterGrid.CurrentPage > MasterGrid.TotalPages) MasterGrid.CurrentPage = MasterGrid.TotalPages; else MasterGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    ds = _auditMasterOperations.GetListFormDataSet(filters, MasterGrid.CurrentPage, MasterGrid.PageSize, gobList, out itemCount);
                }
            }

            MasterGrid.DataSource = ds != null && ds.Tables.Count > 0 ? ds.Tables[0].DefaultView : null;
            MasterGrid.DataBind();

            MasterPage.UpCommon.Update();
        }

        private void BindGridDetails(string sessionToken)
        {
            var filters = GetFiltersDetails();
            if (!string.IsNullOrWhiteSpace(sessionToken)) filters.Add("SessionToken", sessionToken);
            else if (!string.IsNullOrWhiteSpace(hfSessionToken.Value)) filters.Add("SessionToken", hfSessionToken.Value);

            var gobList = new List<GEMOrderBy>();
            if (DetailsGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(DetailsGrid.SecondSortingColumn, DetailsGrid.SecondSortingOrder == SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (DetailsGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(DetailsGrid.MainSortingColumn, DetailsGrid.MainSortingOrder == SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("ColumnName", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _auditingDetailOperations.GetColumnValues(filters, DetailsGrid.CurrentPage, DetailsGrid.PageSize, gobList, out itemCount);
            }

            DetailsGrid.TotalRecords = itemCount;

            if (DetailsGrid.CurrentPage > DetailsGrid.TotalPages || (DetailsGrid.CurrentPage == 0 && DetailsGrid.TotalPages > 0))
            {
                if (DetailsGrid.CurrentPage > DetailsGrid.TotalPages) DetailsGrid.CurrentPage = DetailsGrid.TotalPages; else DetailsGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    ds = _auditingDetailOperations.GetColumnValues(filters, DetailsGrid.CurrentPage, DetailsGrid.PageSize, gobList, out itemCount);
                }
            }

            DetailsGrid.DataSource = ds != null && ds.Tables.Count > 0 ? ds.Tables[0].DefaultView : null;
            DetailsGrid.DataBind();

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
                case ContextMenuEventTypes.Back:
                    {
                        if (EntityContext == EntityContext.AuthorisedProduct) Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Product) Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.PharmaceuticalProduct) Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Document) Response.Redirect(string.Format("~/Views/DocumentViewAll/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Activity) Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.ActivityMy) Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Alerter) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}", EntityContext));
                    }
                    break;
            }
        }

        public void Details_OnCLick(object sender, EventArgs e)
        {
            var lb = (LinkButton)sender;
            String sessionToken = lb.CommandArgument;
            hfSessionToken.Value = sessionToken;
            BindGridDetails(sessionToken);
        }

        #endregion

        #region Grid

        void MasterGrid_OnExcelCellPrepared(object sender, PossGridExportCellRenderArgs args)
        {

        }

        void MasterGrid_OnHtmlRowPrepared(object sender, PossGridRowEventArgs e)
        {

        }

        void MasterGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGridMaster();
        }

        void MasterGrid_OnHtmlCellPrepared(object sender, PossGridCellEventArgs e)
        {
            if (!MasterGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = MasterGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (MasterGrid.SortCount > 1 && e.FieldName == MasterGrid.MainSortingColumn)
                return;
        }

        void MasterGrid_OnLoadClientLayout(object sender, ClientLayoutEventArgs args)
        {

        }

        void DetailsGrid_OnExcelCellPrepared(object sender, PossGridExportCellRenderArgs args)
        {

        }

        void DetailsGrid_OnHtmlRowPrepared(object sender, PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            string oldValue = Convert.ToString(e.GetValue("OldValue"));

            var oldValueParts = oldValue.Split(new[] { "|||" }, StringSplitOptions.None);
            var pnlOldValue = e.FindControl("pnlOldValue") as Panel;
            if (pnlOldValue != null)
            {
                pnlOldValue.Controls.Clear();
                foreach (string s in oldValueParts)
                {
                    var tempString = s;
                    if (ValidationHelper.IsValidDateTime(s, CultureInfoHr))
                    {
                        tempString = Convert.ToDateTime(s).ToString("dd.MM.yyyy");
                    }
                    pnlOldValue.Controls.Add(new LiteralControl(tempString));
                    pnlOldValue.Controls.Add(new LiteralControl("<br />"));
                }
            }

            string newValue = Convert.ToString(e.GetValue("NewValue"));

            var newValueParts = newValue.Split(new[] { "|||" }, StringSplitOptions.None);

            var pnlNewValue = e.FindControl("pnlNewValue") as Panel;
            if (pnlNewValue != null)
            {
                pnlNewValue.Controls.Clear();
                foreach (string s in newValueParts)
                {
                    var tempString = s;
                    if (ValidationHelper.IsValidDateTime(s, CultureInfoHr))
                    {
                        tempString = Convert.ToDateTime(s).ToString("dd.MM.yyyy");
                    }
                    pnlNewValue.Controls.Add(new LiteralControl(tempString));
                    pnlNewValue.Controls.Add(new LiteralControl("<br />"));
                }
            }
        }

        void DetailsGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGridDetails(null);
        }

        void DetailsGrid_OnHtmlCellPrepared(object sender, PossGridCellEventArgs e)
        {
            if (!DetailsGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = DetailsGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (DetailsGrid.SortCount > 1 && e.FieldName == DetailsGrid.MainSortingColumn)
                return;
        }

        void DetailsGrid_OnLoadClientLayout(object sender, ClientLayoutEventArgs args)
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
                contexMenu = new[] { new ContextMenuItem(ContextMenuEventTypes.Back, "Back") };
            }

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contexMenu);
        }

        private void GenerateTabMenuItems()
        {
            Location_PK location = null;

            if (ListType == ListType.List)
            {
                switch (EntityContext)
                {
                    case EntityContext.Default:
                        location = Support.LocationManager.Instance.GetLocationByName("DocList", Support.CacheManager.Instance.AppLocations);

                        MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                        MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                        tabMenu.Visible = false;
                        break;
                    case EntityContext.Product:
                    case EntityContext.AuthorisedProduct:
                    case EntityContext.PharmaceuticalProduct:
                    case EntityContext.Activity:
                    case EntityContext.ActivityMy:
                    case EntityContext.Document:
                        switch (EntityContext)
                        {
                            case EntityContext.Product:
                                location = Support.LocationManager.Instance.GetLocationByName("ProdAuditTrailList", Support.CacheManager.Instance.AppLocations);
                                break;
                            case EntityContext.AuthorisedProduct:
                                location = Support.LocationManager.Instance.GetLocationByName("AuthProdAuditTrailList", Support.CacheManager.Instance.AppLocations);
                                break;
                            case EntityContext.PharmaceuticalProduct:
                                location = Support.LocationManager.Instance.GetLocationByName("PharmProdAuditTrailList", Support.CacheManager.Instance.AppLocations);
                                break;
                            case EntityContext.Document:
                                location = Support.LocationManager.Instance.GetLocationByName("DocAuditTrailList", Support.CacheManager.Instance.AppLocations);
                                break;
                            case EntityContext.ActivityMy:
                                location = Support.LocationManager.Instance.GetLocationByName("ActMyAuditTrailList", Support.CacheManager.Instance.AppLocations);
                                break;
                            case EntityContext.Activity:
                                location = Support.LocationManager.Instance.GetLocationByName("ActAuditTrailList", Support.CacheManager.Instance.AppLocations);
                                break;

                        }
                        MasterPage.TabMenu.TabControls.Clear();
                        tabMenu.Visible = true;
                        tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                        tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                        break;
                    case EntityContext.Alerter:
                        location = Support.LocationManager.Instance.GetLocationByName("AlertAuditTrailList", Support.CacheManager.Instance.AppLocations);
                        var locationNamesToGenerate = new List<string> { "AlertAuditTrailList", "ReminderFormEdit" };

                        if (location != null)
                        {
                            MasterPage.OneTimePermissionToken = Permission.ViewComradeTab;
                            tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location, locationNamesToGenerate);
                            tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                        }
                        break;
                }
            }
        }

        public string HandleMissing(object value)
        {
            if (value != null && value.ToString().Trim() != "")
            {
                return value.ToString();
            }

            return "Missing";
        }

        private Dictionary<string, string> GetFiltersMaster()
        {
            var filters = MasterGrid.GetFilters();

            switch (ListType)
            {
                case ListType.List:
                    if (EntityContext == EntityContext.Product)
                    {
                        filters.Add("QueryBy", "PRODUCT");
                        filters.Add("EntityPk", Convert.ToString(_idProd));
                    }
                    else if (EntityContext == EntityContext.AuthorisedProduct)
                    {
                        filters.Add("QueryBy", "AUTHORISED_PRODUCT");
                        filters.Add("EntityPk", Convert.ToString(_idAuthProd));
                    }
                    else if (EntityContext == EntityContext.PharmaceuticalProduct)
                    {
                        filters.Add("QueryBy", "PHARMACEUTICAL_PRODUCT");
                        filters.Add("EntityPk", Convert.ToString(_idPharmProd));
                    }
                    else if (EntityContext == EntityContext.Document)
                    {
                        filters.Add("QueryBy", "DOCUMENT");
                        filters.Add("EntityPk", Convert.ToString(_idDoc));
                    }
                    else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy)
                    {
                        filters.Add("QueryBy", "ACTIVITY");
                        filters.Add("EntityPk", Convert.ToString(_idAct));
                    }
                    else if (EntityContext == EntityContext.Alerter)
                    {
                        filters.Add("QueryBy", "REMINDER");
                        filters.Add("EntityPk", Convert.ToString(_idAlert));
                    }
                    break;
            }

            return filters;
        }

        private Dictionary<string, string> GetFiltersDetails()
        {
            var filters = DetailsGrid.GetFilters();

            switch (ListType)
            {
                case ListType.List:
                    if (EntityContext == EntityContext.Product)
                    {
                        filters.Add("QueryBy", "PRODUCT");
                        filters.Add("EntityPk", Convert.ToString(_idProd));
                    }
                    else if (EntityContext == EntityContext.AuthorisedProduct)
                    {
                        filters.Add("QueryBy", "AUTHORISED_PRODUCT");
                        filters.Add("EntityPk", Convert.ToString(_idAuthProd));
                    }
                    else if (EntityContext == EntityContext.PharmaceuticalProduct)
                    {
                        filters.Add("QueryBy", "PHARMACEUTICAL_PRODUCT");
                        filters.Add("EntityPk", Convert.ToString(_idPharmProd));
                    }
                    else if (EntityContext == EntityContext.Document)
                    {
                        filters.Add("QueryBy", "DOCUMENT");
                        filters.Add("EntityPk", Convert.ToString(_idDoc));
                    }
                    else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy)
                    {
                        filters.Add("QueryBy", "ACTIVITY");
                        filters.Add("EntityPk", Convert.ToString(_idAct));
                    }
                    else if (EntityContext == EntityContext.Alerter)
                    {
                        filters.Add("QueryBy", "REMINDER");
                        filters.Add("EntityPk", Convert.ToString(_idAlert));
                    }
                    break;
            }

            return filters;
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

        private void HandleEntityContextPharmaceuticalProduct()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Pharmaceutical product:";

            var pharmaceuticalProduct = _pharmaceuticalProductOperations.GetEntity(_idPharmProd);

            lblPrvParentEntity.Text = pharmaceuticalProduct != null && !string.IsNullOrWhiteSpace(pharmaceuticalProduct.name) ? pharmaceuticalProduct.name : Constant.ControlDefault.LbPrvText;
        }

        private void HandleEntityContextDocument()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Document:";

            var document = _documentOperations.GetEntity(_idDoc);

            lblPrvParentEntity.Text = document != null && !string.IsNullOrWhiteSpace(document.name) ? document.name : Constant.ControlDefault.LbPrvText;
        }

        private void HandleEntityContextActivity()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Activity:";

            var activity = _activityOperations.GetEntity(_idAct);

            lblPrvParentEntity.Text = activity != null && !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.ControlDefault.LbPrvText;
        }

        private void HandleEntityContextAlerter()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Alert:";

            var alerter = _reminderOperations.GetEntity(_idAlert);

            lblPrvParentEntity.Text = alerter != null ? string.Format("{0}: {1}", alerter.reminder_type, alerter.reminder_name) : Constant.ControlDefault.LbPrvText;
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