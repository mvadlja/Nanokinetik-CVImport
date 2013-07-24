using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.UserControl;
using AspNetUIFramework;
using GEM2Common;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using EVMessage.Xevprm;

namespace AspNetUI.Views.Shared.Form
{
    public partial class PharmaceuticalProductForm : System.Web.UI.UserControl
    {
        #region Declarations

        private CultureInfo _cultureInfoHr;
        private int? _idPharmProd;
        private int? _idProd;
        private bool? _isResponsibleUser;
        private PharmaceuticalProductView.Form _containerPage;

        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;

        private IProduct_PKOperations _productOperations;
        private IPharmaceutical_product_PKOperations _pharmaceuticalProductOperations;
        private IPharmaceuticalProductSubstanceOperations _pharmaceuticalProductSubstanceOperations;
        private IProduct_mn_PKOperations _productPharmaceuticalProductMnOperations;
        private IPharmaceutical_form_PKOperations _pharmaceuticalFormOperations;
        private IMedicaldevice_PKOperations _medicalDeviceOperations;
        private IPp_md_mn_PKOperations _pharmaceuticalProductMedicalDeviceMnOperations;
        private IAdminroute_PKOperations _administrationRouteOperations;
        private IPp_ar_mn_PKOperations _pharmaceuticalProductAdministrationRouteMnOperations;
        private IActiveingredient_PKOperations _activeIngredientOperations;
        private IExcipient_PKOperations _excipientOperations;
        private IAdjuvant_PKOperations _adjuvantOperations;
        private IPerson_PKOperations _personOperations;
        private IUSEROperations _userOperations;
        private ILast_change_PKOperations _lastChangeOperations;
        private ISubstance_PKOperations _substanceOperations;
        private ISsi__cont_voc_PKOperations _ssiOperations;

        public virtual event EventHandler<EventArgs> OnTopMenuChange;

        #endregion

        #region Properties

        public FormType FormType { get; set; }

        public EntityContext EntityContext { get; set; }

        private string PPSessionId
        {
            get
            {
                if (ViewState["PPSessionId"] == null)
                {
                    ViewState["PPSessionId"] = System.Guid.NewGuid().ToString("N");
                }

                return ViewState["PPSessionId"] as string;
            }
            set { ViewState["PPSessionId"] = value; }
        }

        private int? _ppSubstancePkToDelete
        {
            get { return (int?)ViewState["_ppSubstancePkToDelete"]; }
            set { ViewState["_ppSubstancePkToDelete"] = value; }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LoadFormVariables();
            BindEventHandlers();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsPostBack) return;

            if (Visible)
            {
                InitForm(null);

                if (FormType == FormType.Edit || FormType == FormType.SaveAs)
                {
                    BindForm(null);
                }

                SetFormControlsDefaults(null);

                BindGrid();
            }

            SecurityPageSpecific();
        }

        #endregion

        #region Form methods

        #region Initialize

        private void LoadFormVariables()
        {
            FormType = FormType.Unknown;
            EntityContext parsedEntityContext;
            Enum.TryParse(Request.QueryString["EntityContext"], out parsedEntityContext);
            EntityContext = parsedEntityContext;

            if (Page is PharmaceuticalProductView.Form)
            {
                _containerPage = Page as PharmaceuticalProductView.Form;
                _containerPage.LoadFormVariables();
                FormType = _containerPage.FormType;
            }

            _idPharmProd = ValidationHelper.IsValidInt(Request.QueryString["idPharmProd"]) ? int.Parse(Request.QueryString["idPharmProd"]) : (int?)null;
            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;

            _productOperations = new Product_PKDAL();
            _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
            _pharmaceuticalProductSubstanceOperations = new PharmaceuticalProductSubstanceDAL();
            _productPharmaceuticalProductMnOperations = new Product_mn_PKDAL();
            _pharmaceuticalFormOperations = new Pharmaceutical_form_PKDAL();
            _medicalDeviceOperations = new Medicaldevice_PKDAL();
            _pharmaceuticalProductMedicalDeviceMnOperations = new Pp_md_mn_PKDAL();
            _administrationRouteOperations = new Adminroute_PKDAL();
            _pharmaceuticalProductAdministrationRouteMnOperations = new Pp_ar_mn_PKDAL();
            _activeIngredientOperations = new Activeingredient_PKDAL();
            _excipientOperations = new Excipient_PKDAL();
            _adjuvantOperations = new Adjuvant_PKDAL();
            _personOperations = new Person_PKDAL();
            _userOperations = new USERDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _substanceOperations = new Substance_PKDAL();
            _ssiOperations = new Ssi__cont_voc_PKDAL();
        }

        private void BindEventHandlers()
        {
            lbSrProducts.Searcher.OnOkButtonClick += LbSrProducts_OnOkButtonClick;
            lbSrAdministrationRoutes.Searcher.OnOkButtonClick += LbSrAdministrationRoutes_OnOkButtonClick;
            lbSrMedicalDevices.Searcher.OnOkButtonClick += LbSrMedicalDevices_OnOkButtonClick;

            ActiveIngredientsGrid.OnRebindRequired += ActiveIngredientsGrid_OnRebindRequired;
            ActiveIngredientsGrid.OnHtmlRowPrepared += ActiveIngredientsGrid_OnHtmlRowPrepared;
            ActiveIngredientsGrid.OnHtmlCellPrepared += ActiveIngredientsGrid_OnHtmlCellPrepared;

            ExcipientsGrid.OnRebindRequired += ExcipientsGrid_OnRebindRequired;
            ExcipientsGrid.OnHtmlRowPrepared += ExcipientsGrid_OnHtmlRowPrepared;
            ExcipientsGrid.OnHtmlCellPrepared += ExcipientsGrid_OnHtmlCellPrepared;

            AdjuvantsGrid.OnRebindRequired += AdjuvantsGrid_OnRebindRequired;
            AdjuvantsGrid.OnHtmlRowPrepared += AdjuvantsGrid_OnHtmlRowPrepared;
            AdjuvantsGrid.OnHtmlCellPrepared += AdjuvantsGrid_OnHtmlCellPrepared;

            btnAddActiveIngredient.Click += btnAddActiveIngredient_OnClick;
            btnAddExcipient.Click += btnAddExcipient_OnClick;
            btnAddAdjuvant.Click += btnAddAdjuvant_OnClick;

            PPSubstancePopup.OnOkButtonClick += PPSubstancePopup_OnOkButtonClick;
            mpDelete.OnYesButtonClick += mpDelete_OnYesButtonClick;
        }

        public void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
        }

        #endregion

        #region Fill

        void ClearForm(object arg)
        {
            lbSrProducts.Clear();
            txtPharmaceuticalProductName.Text = string.Empty;
            ddlResponsibleUser.SelectedValue = string.Empty;
            txtDescription.Text = string.Empty;
            ddlPharmaceuticalForm.SelectedValue = string.Empty;
            lbSrAdministrationRoutes.Clear();
            lbSrMedicalDevices.Clear();
            txtId.Text = string.Empty;
            txtBookedSlots.Text = string.Empty;
            txtComment.Text = string.Empty;
            PPSessionId = null;
        }

        void FillFormControls(object arg)
        {
            FillDdlResponsibleUser();
            FillDdlPharmaceuticalForm();
        }

        public void SetFormControlsDefaults(object arg)
        {
            if (FormType == FormType.New)
            {
                if (EntityContext == EntityContext.Product)
                {
                    if (_idProd.HasValue)
                    {
                        var product = _productOperations.GetEntity(_idProd);

                        if (_containerPage != null)
                        {
                            _containerPage.LblPrvParentEntity.Visible = true;
                            _containerPage.LblPrvParentEntity.Label = "Product:";
                            _containerPage.LblPrvParentEntity.Text = product.name;
                        }

                        if (product != null && lbSrProducts.LbInput.Items.FindByValue(Convert.ToString(product.product_PK)) == null)
                        {
                            lbSrProducts.LbInput.Items.Add(new ListItem(product.name, Convert.ToString(product.product_PK)));
                        }
                    }
                    else
                    {
                        lbSrProducts.LbInput.Items.Add(new ListItem("<NEW>", "-1"));
                    }
                }

                var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
                ddlResponsibleUser.SelectedValue = user != null ? user.Person_FK : null;

                StylizeArticle57RelevantControls(true);
            }
        }

        private void FillDdlResponsibleUser()
        {
            var responsibleUserList = _personOperations.GetEntitiesByRoleName(Constant.PersonRoleName.ResponsibleUser);
            ddlResponsibleUser.Fill(responsibleUserList, x => x.FullName, x => x.person_PK);
            ddlResponsibleUser.SortItemsByText();
        }

        private void FillDdlPharmaceuticalForm()
        {
            var pharmaceuticalFormList = _pharmaceuticalFormOperations.GetEntities();
            ddlPharmaceuticalForm.Fill(pharmaceuticalFormList, x => x.name, x => x.pharmaceutical_form_PK);
            ddlPharmaceuticalForm.SortItemsByText();
        }

        #endregion

        #region Bind

        public void BindForm(object arg)
        {
            if (!_idPharmProd.HasValue) return;

            var pharmaceuticalProduct = _pharmaceuticalProductOperations.GetEntity(_idPharmProd);

            if (pharmaceuticalProduct == null || !pharmaceuticalProduct.pharmaceutical_product_PK.HasValue) return;

            // Pharmaceutical product
            var pharmaceuticalProductForm = Page as PharmaceuticalProductView.Form;
            if (pharmaceuticalProductForm != null)
            {
                pharmaceuticalProductForm.LblPrvParentEntity.Text = pharmaceuticalProduct.name;
            }

            // Products
            BindProducts(pharmaceuticalProduct.pharmaceutical_product_PK.Value);

            // Pharmaceutical product name
            txtPharmaceuticalProductName.Text = pharmaceuticalProduct.name;

            // Responsible user
            BindResponsibleUser(pharmaceuticalProduct.responsible_user_FK);

            // Product description
            txtDescription.Text = pharmaceuticalProduct.description;

            // Pharmaceutical form
            ddlPharmaceuticalForm.SelectedValue = pharmaceuticalProduct.Pharmform_FK;

            // Administration routes
            BindAdministrationRoutes(pharmaceuticalProduct.pharmaceutical_product_PK.Value);

            // Medical devices
            BindMedicalDevices(pharmaceuticalProduct.pharmaceutical_product_PK.Value);

            // ID
            txtId.Text = pharmaceuticalProduct.ID;

            // Booked slot(s)
            txtBookedSlots.Text = pharmaceuticalProduct.booked_slots;

            // Comment
            txtComment.Text = pharmaceuticalProduct.comments;

            // Active ingredients
            BindActiveIngredients(pharmaceuticalProduct.pharmaceutical_product_PK.Value);

            // Excipients
            BindExcipients(pharmaceuticalProduct.pharmaceutical_product_PK.Value);

            // Adjuvants
            BindAdjuvants(pharmaceuticalProduct.pharmaceutical_product_PK.Value);

            // Coloring
            StylizeArticle57RelevantControls(true);

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if (user != null) _isResponsibleUser = pharmaceuticalProduct.responsible_user_FK == user.Person_FK;

            if (Request.QueryString["XevprmValidation"] != null)
            {
                ValidateFormForXevprm(pharmaceuticalProduct);
            }
        }

        public void BindGrid()
        {
            BindActiveIngredientsGrid();
            BindExcipientsGrid();
            BindAdjuvantsGrid();
        }

        private void BindProducts(int pharmaceuticalProductPk)
        {
            var productList = _productOperations.GetEntitiesByPharmaceuticalProduct(pharmaceuticalProductPk);
            lbSrProducts.Fill(productList, x => x.name, x => x.product_PK);
            lbSrProducts.LbInput.SortItemsByText();
        }

        private void BindAdministrationRoutes(int pharmaceuticalProductPk)
        {
            var administrationRouteList = _administrationRouteOperations.GetEntitiesByPharmaceuticalProduct(pharmaceuticalProductPk);
            lbSrAdministrationRoutes.Fill(administrationRouteList, x => x.adminroutecode, x => x.adminroute_PK);
            lbSrAdministrationRoutes.LbInput.SortItemsByText();
        }

        private void BindMedicalDevices(int pharmaceuticalProductPk)
        {
            var medicalDeviceList = _medicalDeviceOperations.GetEntitiesByPharmaceuticalProduct(pharmaceuticalProductPk);
            lbSrMedicalDevices.Fill(medicalDeviceList, x => x.medicaldevicecode, x => x.medicaldevice_PK);
            lbSrMedicalDevices.LbInput.SortItemsByText();
        }

        private void BindResponsibleUser(int? responsibleUserPk)
        {
            if (FormType == FormType.SaveAs)
            {
                var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
                ddlResponsibleUser.SelectedValue = user != null ? user.Person_FK : null;
            }
            else
            {
                ddlResponsibleUser.SelectedValue = responsibleUserPk;
            }
        }

        private void BindActiveIngredients(int pharmaceuticalProductPk)
        {
            var activeIngredientList = _activeIngredientOperations.GetIngredientsByPPPK(pharmaceuticalProductPk);
            _pharmaceuticalProductSubstanceOperations.DeleteByTypeAndSessionId(PharmaceuticalProductSubstance.SubstanceType.ActiveIngredient.ToString(), PPSessionId);
            var ppSubstanceList = new List<PharmaceuticalProductSubstance>(activeIngredientList.Count);
            foreach (var activeIngredient in activeIngredientList)
            {
                ppSubstanceList.Add(new PharmaceuticalProductSubstance()
                {
                    substancetype = PharmaceuticalProductSubstance.SubstanceType.ActiveIngredient.ToString(),
                    ppsubstance_FK = activeIngredient.activeingredient_PK,
                    pp_FK = activeIngredient.pp_FK,
                    substancecode_FK = activeIngredient.substancecode_FK,
                    concentrationtypecode = activeIngredient.concentrationtypecode,
                    expressedby_FK = activeIngredient.ExpressedBy_FK,
                    lowamountnumervalue = activeIngredient.lowamountnumervalue,
                    lowamountnumerprefix = activeIngredient.lowamountnumerprefix,
                    lowamountnumerunit = activeIngredient.lowamountnumerunit,
                    lowamountdenomvalue = activeIngredient.lowamountdenomvalue,
                    lowamountdenomprefix = activeIngredient.lowamountdenomprefix,
                    lowamountdenomunit = activeIngredient.lowamountdenomunit,
                    highamountnumervalue = activeIngredient.highamountnumervalue,
                    highamountnumerprefix = activeIngredient.highamountnumerprefix,
                    highamountnumerunit = activeIngredient.highamountnumerunit,
                    highamountdenomvalue = activeIngredient.highamountdenomvalue,
                    highamountdenomprefix = activeIngredient.highamountdenomprefix,
                    highamountdenomunit = activeIngredient.highamountdenomunit,
                    concise = activeIngredient.concise,
                    user_FK = activeIngredient.userID,
                    sessionid = PPSessionId,
                    modifieddate = DateTime.Now
                });
            }

            _pharmaceuticalProductSubstanceOperations.SaveCollection(ppSubstanceList);

            BindActiveIngredientsGrid();
        }

        private void BindExcipients(int pharmaceuticalProductPk)
        {
            var exipientList = _excipientOperations.GetExcipientsByPPPK(pharmaceuticalProductPk);
            _pharmaceuticalProductSubstanceOperations.DeleteByTypeAndSessionId(PharmaceuticalProductSubstance.SubstanceType.Excipient.ToString(), PPSessionId);
            var ppSubstanceList = new List<PharmaceuticalProductSubstance>(exipientList.Count);
            foreach (var excipient in exipientList)
            {
                ppSubstanceList.Add(new PharmaceuticalProductSubstance()
                {
                    substancetype = PharmaceuticalProductSubstance.SubstanceType.Excipient.ToString(),
                    ppsubstance_FK = excipient.excipient_PK,
                    pp_FK = excipient.pp_FK,
                    substancecode_FK = excipient.substancecode_FK,
                    concentrationtypecode = excipient.concentrationtypecode,
                    expressedby_FK = excipient.ExpressedBy_FK,
                    lowamountnumervalue = excipient.lowamountnumervalue,
                    lowamountnumerprefix = excipient.lowamountnumerprefix,
                    lowamountnumerunit = excipient.lowamountnumerunit,
                    lowamountdenomvalue = excipient.lowamountdenomvalue,
                    lowamountdenomprefix = excipient.lowamountdenomprefix,
                    lowamountdenomunit = excipient.lowamountdenomunit,
                    highamountnumervalue = excipient.highamountnumervalue,
                    highamountnumerprefix = excipient.highamountnumerprefix,
                    highamountnumerunit = excipient.higamountnumerunit,
                    highamountdenomvalue = excipient.highamountdenomvalue,
                    highamountdenomprefix = excipient.highamountdenomprefix,
                    highamountdenomunit = excipient.highamountdenomunit,
                    concise = excipient.concise,
                    user_FK = excipient.userID,
                    sessionid = PPSessionId,
                    modifieddate = DateTime.Now
                });
            }

            _pharmaceuticalProductSubstanceOperations.SaveCollection(ppSubstanceList);

            BindExcipientsGrid();
        }

        private void BindAdjuvants(int pharmaceuticalProductPk)
        {
            var adjuvantList = _adjuvantOperations.GetAdjuvantsByPPPK(pharmaceuticalProductPk);
            _pharmaceuticalProductSubstanceOperations.DeleteByTypeAndSessionId(PharmaceuticalProductSubstance.SubstanceType.Adjuvant.ToString(), PPSessionId);
            var ppSubstanceList = new List<PharmaceuticalProductSubstance>(adjuvantList.Count);
            foreach (var adjuvant in adjuvantList)
            {
                ppSubstanceList.Add(new PharmaceuticalProductSubstance()
                {
                    substancetype = PharmaceuticalProductSubstance.SubstanceType.Adjuvant.ToString(),
                    ppsubstance_FK = adjuvant.adjuvant_PK,
                    pp_FK = adjuvant.pp_FK,
                    substancecode_FK = adjuvant.substancecode_FK,
                    concentrationtypecode = adjuvant.concentrationtypecode,
                    expressedby_FK = adjuvant.ExpressedBy_FK,
                    lowamountnumervalue = adjuvant.lowamountnumervalue,
                    lowamountnumerprefix = adjuvant.lowamountnumerprefix,
                    lowamountnumerunit = adjuvant.lowamountnumerunit,
                    lowamountdenomvalue = adjuvant.lowamountdenomvalue,
                    lowamountdenomprefix = adjuvant.lowamountdenomprefix,
                    lowamountdenomunit = adjuvant.lowamountdenomunit,
                    highamountnumervalue = adjuvant.highamountnumervalue,
                    highamountnumerprefix = adjuvant.highamountnumerprefix,
                    highamountnumerunit = adjuvant.higamountnumerunit,
                    highamountdenomvalue = adjuvant.highamountdenomvalue,
                    highamountdenomprefix = adjuvant.highamountdenomprefix,
                    highamountdenomunit = adjuvant.highamountdenomunit,
                    concise = adjuvant.concise,
                    user_FK = adjuvant.userID,
                    sessionid = PPSessionId,
                    modifieddate = DateTime.Now
                });
            }

            _pharmaceuticalProductSubstanceOperations.SaveCollection(ppSubstanceList);

            BindAdjuvantsGrid();
        }

        private void BindActiveIngredientsGrid()
        {
            var filters = ActiveIngredientsGrid.GetFilters();

            filters.Add("SubstanceType", PharmaceuticalProductSubstance.SubstanceType.ActiveIngredient.ToString());
            filters.Add("SessionId", PPSessionId);

            var gobList = new List<GEMOrderBy>();
            if (ActiveIngredientsGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(ActiveIngredientsGrid.SecondSortingColumn, ActiveIngredientsGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (ActiveIngredientsGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(ActiveIngredientsGrid.MainSortingColumn, ActiveIngredientsGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("ppsubstance_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            var ds = _pharmaceuticalProductSubstanceOperations.GetListFormDataSet(filters, ActiveIngredientsGrid.CurrentPage, ActiveIngredientsGrid.PageSize, gobList, out itemCount);

            ActiveIngredientsGrid.TotalRecords = itemCount;

            if (ActiveIngredientsGrid.CurrentPage > ActiveIngredientsGrid.TotalPages || (ActiveIngredientsGrid.CurrentPage == 0 && ActiveIngredientsGrid.TotalPages > 0))
            {
                if (ActiveIngredientsGrid.CurrentPage > ActiveIngredientsGrid.TotalPages) ActiveIngredientsGrid.CurrentPage = ActiveIngredientsGrid.TotalPages; else ActiveIngredientsGrid.CurrentPage = 1;

                ds = _pharmaceuticalProductSubstanceOperations.GetListFormDataSet(filters, ActiveIngredientsGrid.CurrentPage, ActiveIngredientsGrid.PageSize, gobList, out itemCount);
            }

            ActiveIngredientsGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            ActiveIngredientsGrid.DataBind();
        }

        private void BindExcipientsGrid()
        {
            var filters = ExcipientsGrid.GetFilters();

            filters.Add("SubstanceType", PharmaceuticalProductSubstance.SubstanceType.Excipient.ToString());
            filters.Add("SessionId", PPSessionId);

            var gobList = new List<GEMOrderBy>();
            if (ExcipientsGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(ExcipientsGrid.SecondSortingColumn, ExcipientsGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (ExcipientsGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(ExcipientsGrid.MainSortingColumn, ExcipientsGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("ppsubstance_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            var ds = _pharmaceuticalProductSubstanceOperations.GetListFormDataSet(filters, ExcipientsGrid.CurrentPage, ExcipientsGrid.PageSize, gobList, out itemCount);

            ExcipientsGrid.TotalRecords = itemCount;

            if (ExcipientsGrid.CurrentPage > ExcipientsGrid.TotalPages || (ExcipientsGrid.CurrentPage == 0 && ExcipientsGrid.TotalPages > 0))
            {
                if (ExcipientsGrid.CurrentPage > ExcipientsGrid.TotalPages) ExcipientsGrid.CurrentPage = ExcipientsGrid.TotalPages; else ExcipientsGrid.CurrentPage = 1;

                ds = _pharmaceuticalProductSubstanceOperations.GetListFormDataSet(filters, ExcipientsGrid.CurrentPage, ExcipientsGrid.PageSize, gobList, out itemCount);
            }

            ExcipientsGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            ExcipientsGrid.DataBind();
        }

        private void BindAdjuvantsGrid()
        {
            var filters = AdjuvantsGrid.GetFilters();

            filters.Add("SubstanceType", PharmaceuticalProductSubstance.SubstanceType.Adjuvant.ToString());
            filters.Add("SessionId", PPSessionId);

            var gobList = new List<GEMOrderBy>();
            if (AdjuvantsGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(AdjuvantsGrid.SecondSortingColumn, AdjuvantsGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (AdjuvantsGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(AdjuvantsGrid.MainSortingColumn, AdjuvantsGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("ppsubstance_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            var ds = _pharmaceuticalProductSubstanceOperations.GetListFormDataSet(filters, AdjuvantsGrid.CurrentPage, AdjuvantsGrid.PageSize, gobList, out itemCount);

            AdjuvantsGrid.TotalRecords = itemCount;

            if (AdjuvantsGrid.CurrentPage > AdjuvantsGrid.TotalPages || (AdjuvantsGrid.CurrentPage == 0 && AdjuvantsGrid.TotalPages > 0))
            {
                if (AdjuvantsGrid.CurrentPage > AdjuvantsGrid.TotalPages) AdjuvantsGrid.CurrentPage = AdjuvantsGrid.TotalPages; else AdjuvantsGrid.CurrentPage = 1;

                ds = _pharmaceuticalProductSubstanceOperations.GetListFormDataSet(filters, AdjuvantsGrid.CurrentPage, AdjuvantsGrid.PageSize, gobList, out itemCount);
            }

            AdjuvantsGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            AdjuvantsGrid.DataBind();
        }

        #endregion

        #region Validate

        public bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (!ddlPharmaceuticalForm.SelectedId.HasValue)
            {
                errorMessage += "Pharmaceutical form can't be empty.<br />";
                ddlPharmaceuticalForm.ValidationError = "Pharmaceutical form can't be empty.";
            }

            var substanceList = _pharmaceuticalProductSubstanceOperations.GetEntitiesByTypeAndSessionId(PharmaceuticalProductSubstance.SubstanceType.ActiveIngredient.ToString(), PPSessionId);
            if (substanceList.Count == 0)
            {
                errorMessage += "Active ingredients can't be empty.<br />";
                lblErrActiveIngredient.Text = "Active ingredients can't be empty.";
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                modalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }

            return true;
        }

        private void ClearValidationErrors()
        {
            ddlPharmaceuticalForm.ValidationError = string.Empty;
            lblErrActiveIngredient.Text = string.Empty;
        }

        private void ValidateFormForXevprm(Pharmaceutical_product_PK entity)
        {
            if (!new[] { "1", "2", "3", "4", "6" }.Contains(Request.QueryString["XevprmValidation"])) return;

            XevprmOperationType operationType;
            Enum.TryParse(Request.QueryString["XevprmValidation"], true, out operationType);

            var validationResult = XevprmXml.ValidatePharmaceuticalProduct(entity, operationType);

            if (validationResult.XevprmValidationExceptions.Count == 0) return;

            var errorControlMaping = new Dictionary<string, Control>()
            {
                {XevprmValidationRules.PP.pharmformcode.Cardinality.RuleId, ddlPharmaceuticalForm},
                {XevprmValidationRules.PP.pharmformcode.DataType.RuleId, ddlPharmaceuticalForm},
                {XevprmValidationRules.PP.AR.Cardinality.RuleId, lbSrAdministrationRoutes},
                {XevprmValidationRules.PP.AR.adminroutecode.DataType.RuleId, lbSrAdministrationRoutes},
                {XevprmValidationRules.PP.AR.BR1.RuleId, lbSrAdministrationRoutes},
                {XevprmValidationRules.PP.MD.medicaldevicecode.DataType.RuleId, lbSrMedicalDevices},
                {XevprmValidationRules.PP.MD.BR1.RuleId, lbSrMedicalDevices},
                {XevprmValidationRules.PP.ACT.Cardinality.RuleId, lblErrActiveIngredient},
                {XevprmValidationRules.PP.ACT.BR1.RuleId, lblErrActiveIngredient},
                {XevprmValidationRules.PP.EXC.BR1.RuleId, lblErrExcipient},
                {XevprmValidationRules.PP.ADJ.BR1.RuleId, lblErrAdjuvant}
            };

            foreach (var error in validationResult.XevprmValidationExceptions)
            {
                if (error.XevprmValidationRuleId == null || !errorControlMaping.Keys.Contains(error.XevprmValidationRuleId)) continue;
                var control = errorControlMaping[error.XevprmValidationRuleId] as Interface.IXevprmValidationError;
                if (control == null) continue;
                control.ValidationError = error.ReadyMessage;
            }

            ActiveIngredientsGrid["Errors"].Visible = true;
            ExcipientsGrid["Errors"].Visible = true;
            AdjuvantsGrid["Errors"].Visible = true;
        }

        private Dictionary<string, string> GetActErrorFieldMaping()
        {
            var errorFieldMaping = new Dictionary<string, string>()
                        {
                {XevprmValidationRules.PP.ACT.BR1.RuleId, "SubstanceName"},
                {XevprmValidationRules.PP.ACT.substancecode.DataType.RuleId, "SubstanceName"},
                {XevprmValidationRules.PP.ACT.substancecode.Cardinality.RuleId, "SubstanceName"},
                {XevprmValidationRules.PP.ACT.concentrationtypecode.BR1.RuleId, "ConcentrationType"},
                {XevprmValidationRules.PP.ACT.concentrationtypecode.Cardinality.RuleId, "ConcentrationType"},

                {XevprmValidationRules.PP.ACT.lowamountnumervalue.Cardinality.RuleId, "LowNumValue"},
                {XevprmValidationRules.PP.ACT.lowamountnumerprefix.BR1.RuleId, "LowNumPrefix"},
                {XevprmValidationRules.PP.ACT.lowamountnumerprefix.Cardinality.RuleId, "LowNumPrefix"},
                {XevprmValidationRules.PP.ACT.lowamountnumerprefix.DataType.RuleId, "LowNumPrefix"},
                {XevprmValidationRules.PP.ACT.lowamountnumerunit.BR1.RuleId, "LowNumUnit"},
                {XevprmValidationRules.PP.ACT.lowamountnumerunit.Cardinality.RuleId, "LowNumUnit"},
                {XevprmValidationRules.PP.ACT.lowamountnumerunit.DataType.RuleId, "LowNumUnit"},
                {XevprmValidationRules.PP.ACT.lowamountdenomvalue.Cardinality.RuleId, "LowDenValue"},
                {XevprmValidationRules.PP.ACT.lowamountdenomprefix.BR1.RuleId, "LowDenPrefix"},
                {XevprmValidationRules.PP.ACT.lowamountdenomprefix.Cardinality.RuleId, "LowDenPrefix"},
                {XevprmValidationRules.PP.ACT.lowamountdenomprefix.DataType.RuleId, "LowDenPrefix"},
                {XevprmValidationRules.PP.ACT.lowamountdenomunit.BR1.RuleId, "LowDenUnit"},
                {XevprmValidationRules.PP.ACT.lowamountdenomunit.Cardinality.RuleId, "LowDenUnit"},
                {XevprmValidationRules.PP.ACT.lowamountdenomunit.DataType.RuleId, "LowDenUnit"},

                {XevprmValidationRules.PP.ACT.highamountnumervalue.BR1.RuleId, "HighNumValue"},
                {XevprmValidationRules.PP.ACT.highamountnumervalue.BR2.RuleId, "HighNumValue"},
                {XevprmValidationRules.PP.ACT.highamountnumerprefix.BR1.RuleId, "HighNumPrefix"},
                {XevprmValidationRules.PP.ACT.highamountnumerprefix.BR2.RuleId, "HighNumPrefix"},
                {XevprmValidationRules.PP.ACT.highamountnumerprefix.BR3.RuleId, "HighNumPrefix"},
                {XevprmValidationRules.PP.ACT.highamountnumerprefix.DataType.RuleId, "HighNumPrefix"},
                {XevprmValidationRules.PP.ACT.highamountnumerunit.BR1.RuleId, "HighNumUnit"},
                {XevprmValidationRules.PP.ACT.highamountnumerunit.BR2.RuleId, "HighNumUnit"},
                {XevprmValidationRules.PP.ACT.highamountnumerunit.BR3.RuleId, "HighNumUnit"},
                {XevprmValidationRules.PP.ACT.highamountnumerunit.BRCustom1.RuleId, "HighNumUnit"},
                {XevprmValidationRules.PP.ACT.highamountnumerunit.DataType.RuleId, "HighNumUnit"},
                {XevprmValidationRules.PP.ACT.highamountdenomvalue.BR1.RuleId, "HighDenValue"},
                {XevprmValidationRules.PP.ACT.highamountdenomvalue.BR2.RuleId, "HighDenValue"},
                {XevprmValidationRules.PP.ACT.highamountdenomvalue.BR3.RuleId, "HighDenValue"},
                {XevprmValidationRules.PP.ACT.highamountdenomprefix.BR1.RuleId, "HighDenPrefix"},
                {XevprmValidationRules.PP.ACT.highamountdenomprefix.BR2.RuleId, "HighDenPrefix"},
                {XevprmValidationRules.PP.ACT.highamountdenomprefix.BR3.RuleId, "HighDenPrefix"},
                {XevprmValidationRules.PP.ACT.highamountdenomprefix.BRCustom1.RuleId, "HighDenPrefix"},
                {XevprmValidationRules.PP.ACT.highamountdenomprefix.DataType.RuleId, "HighDenPrefix"},
                {XevprmValidationRules.PP.ACT.highamountdenomunit.BR1.RuleId, "HighDenUnit"},
                {XevprmValidationRules.PP.ACT.highamountdenomunit.BR2.RuleId, "HighDenUnit"},
                {XevprmValidationRules.PP.ACT.highamountdenomunit.BR3.RuleId, "HighDenUnit"},
                {XevprmValidationRules.PP.ACT.highamountdenomunit.BRCustom1.RuleId, "HighDenUnit"},
                {XevprmValidationRules.PP.ACT.highamountdenomunit.DataType.RuleId, "HighDenUnit"},

            };

            return errorFieldMaping;
        }

        private Dictionary<string, string> GetExcErrorFieldMaping()
        {
            var errorFieldMaping = new Dictionary<string, string>()
                            {
                {XevprmValidationRules.PP.EXC.BR1.RuleId, "SubstanceName"},
                {XevprmValidationRules.PP.EXC.substancecode.DataType.RuleId, "SubstanceName"},
                {XevprmValidationRules.PP.EXC.substancecode.Cardinality.RuleId, "SubstanceName"},
            };

            return errorFieldMaping;
        }

        private Dictionary<string, string> GetAdjErrorFieldMaping()
        {
            var errorFieldMaping = new Dictionary<string, string>()
                        {
                {XevprmValidationRules.PP.ADJ.BR1.RuleId, "SubstanceName"},
                {XevprmValidationRules.PP.ADJ.substancecode.DataType.RuleId, "SubstanceName"},
                {XevprmValidationRules.PP.ADJ.substancecode.Cardinality.RuleId, "SubstanceName"},
                {XevprmValidationRules.PP.ADJ.concentrationtypecode.BR1.RuleId, "ConcentrationType"},
                {XevprmValidationRules.PP.ADJ.concentrationtypecode.Cardinality.RuleId, "ConcentrationType"},

                {XevprmValidationRules.PP.ADJ.lowamountnumervalue.Cardinality.RuleId, "LowNumValue"},
                {XevprmValidationRules.PP.ADJ.lowamountnumerprefix.BR1.RuleId, "LowNumPrefix"},
                {XevprmValidationRules.PP.ADJ.lowamountnumerprefix.Cardinality.RuleId, "LowNumPrefix"},
                {XevprmValidationRules.PP.ADJ.lowamountnumerprefix.DataType.RuleId, "LowNumPrefix"},
                {XevprmValidationRules.PP.ADJ.lowamountnumerunit.BR1.RuleId, "LowNumUnit"},
                {XevprmValidationRules.PP.ADJ.lowamountnumerunit.Cardinality.RuleId, "LowNumUnit"},
                {XevprmValidationRules.PP.ADJ.lowamountnumerunit.DataType.RuleId, "LowNumUnit"},
                {XevprmValidationRules.PP.ADJ.lowamountdenomvalue.Cardinality.RuleId, "LowDenValue"},
                {XevprmValidationRules.PP.ADJ.lowamountdenomprefix.BR1.RuleId, "LowDenPrefix"},
                {XevprmValidationRules.PP.ADJ.lowamountdenomprefix.Cardinality.RuleId, "LowDenPrefix"},
                {XevprmValidationRules.PP.ADJ.lowamountdenomprefix.DataType.RuleId, "LowDenPrefix"},
                {XevprmValidationRules.PP.ADJ.lowamountdenomunit.BR1.RuleId, "LowDenUnit"},
                {XevprmValidationRules.PP.ADJ.lowamountdenomunit.Cardinality.RuleId, "LowDenUnit"},
                {XevprmValidationRules.PP.ADJ.lowamountdenomunit.DataType.RuleId, "LowDenUnit"},

                {XevprmValidationRules.PP.ADJ.highamountnumervalue.BR1.RuleId, "HighNumValue"},
                {XevprmValidationRules.PP.ADJ.highamountnumervalue.BR2.RuleId, "HighNumValue"},
                {XevprmValidationRules.PP.ADJ.highamountnumerprefix.BR1.RuleId, "HighNumPrefix"},
                {XevprmValidationRules.PP.ADJ.highamountnumerprefix.BR2.RuleId, "HighNumPrefix"},
                {XevprmValidationRules.PP.ADJ.highamountnumerprefix.BR3.RuleId, "HighNumPrefix"},
                {XevprmValidationRules.PP.ADJ.highamountnumerprefix.DataType.RuleId, "HighNumPrefix"},
                {XevprmValidationRules.PP.ADJ.highamountnumerunit.BR1.RuleId, "HighNumUnit"},
                {XevprmValidationRules.PP.ADJ.highamountnumerunit.BR2.RuleId, "HighNumUnit"},
                {XevprmValidationRules.PP.ADJ.highamountnumerunit.BR3.RuleId, "HighNumUnit"},
                {XevprmValidationRules.PP.ADJ.highamountnumerunit.BRCustom1.RuleId, "HighNumUnit"},
                {XevprmValidationRules.PP.ADJ.highamountnumerunit.DataType.RuleId, "HighNumUnit"},
                {XevprmValidationRules.PP.ADJ.highamountdenomvalue.BR1.RuleId, "HighDenValue"},
                {XevprmValidationRules.PP.ADJ.highamountdenomvalue.BR2.RuleId, "HighDenValue"},
                {XevprmValidationRules.PP.ADJ.highamountdenomvalue.BR3.RuleId, "HighDenValue"},
                {XevprmValidationRules.PP.ADJ.highamountdenomprefix.BR1.RuleId, "HighDenPrefix"},
                {XevprmValidationRules.PP.ADJ.highamountdenomprefix.BR2.RuleId, "HighDenPrefix"},
                {XevprmValidationRules.PP.ADJ.highamountdenomprefix.BR3.RuleId, "HighDenPrefix"},
                {XevprmValidationRules.PP.ADJ.highamountdenomprefix.BRCustom1.RuleId, "HighDenPrefix"},
                {XevprmValidationRules.PP.ADJ.highamountdenomprefix.DataType.RuleId, "HighDenPrefix"},
                {XevprmValidationRules.PP.ADJ.highamountdenomunit.BR1.RuleId, "HighDenUnit"},
                {XevprmValidationRules.PP.ADJ.highamountdenomunit.BR2.RuleId, "HighDenUnit"},
                {XevprmValidationRules.PP.ADJ.highamountdenomunit.BR3.RuleId, "HighDenUnit"},
                {XevprmValidationRules.PP.ADJ.highamountdenomunit.BRCustom1.RuleId, "HighDenUnit"},
                {XevprmValidationRules.PP.ADJ.highamountdenomunit.DataType.RuleId, "HighDenUnit"},

            };

            return errorFieldMaping;
        }

        #endregion

        #region Save

        public object SaveForm(object arg)
        {
            var pharmaceuticalProduct = new Pharmaceutical_product_PK();

            if (FormType == FormType.Edit && _idPharmProd.HasValue)
            {
                pharmaceuticalProduct = _pharmaceuticalProductOperations.GetEntity(_idPharmProd);
            }

            if (pharmaceuticalProduct == null) return null;

            // Pharmaceutical product name
            pharmaceuticalProduct.name = ConstructPharmaceuticalProductName();

            // Responsible user
            pharmaceuticalProduct.responsible_user_FK = ddlResponsibleUser.SelectedId;

            // Product description
            pharmaceuticalProduct.description = txtDescription.Text;

            // ID
            pharmaceuticalProduct.ID = txtId.Text;

            // Booked slot(s)
            pharmaceuticalProduct.booked_slots = txtBookedSlots.Text;

            // Pharmaceutical form
            pharmaceuticalProduct.Pharmform_FK = ddlPharmaceuticalForm.SelectedId;

            //Comment
            pharmaceuticalProduct.comments = txtComment.Text;

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                pharmaceuticalProduct = _pharmaceuticalProductOperations.Save(pharmaceuticalProduct);

                if (!pharmaceuticalProduct.pharmaceutical_product_PK.HasValue) return null;

                SaveProducts(pharmaceuticalProduct.pharmaceutical_product_PK.Value, auditTrailSessionToken);
                SaveAdministrationRoutes(pharmaceuticalProduct.pharmaceutical_product_PK.Value, auditTrailSessionToken);
                SaveMedicalDevices(pharmaceuticalProduct.pharmaceutical_product_PK.Value, auditTrailSessionToken);
                SaveActiveIngredients(pharmaceuticalProduct.pharmaceutical_product_PK.Value, auditTrailSessionToken);
                SaveExcipients(pharmaceuticalProduct.pharmaceutical_product_PK.Value, auditTrailSessionToken);
                SaveAdjuvants(pharmaceuticalProduct.pharmaceutical_product_PK.Value, auditTrailSessionToken);

                _pharmaceuticalProductOperations.UpdateCalculatedColumn(pharmaceuticalProduct.pharmaceutical_product_PK.Value, Pharmaceutical_product_PK.CalculatedColumn.All);
                _productOperations.UpdateCalculatedColumnByPharmaceuticalProduct(pharmaceuticalProduct.pharmaceutical_product_PK.Value, Product_PK.CalculatedColumn.ActiveSubstances);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, pharmaceuticalProduct.pharmaceutical_product_PK, "PHARMACEUTICAL_PRODUCT", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, pharmaceuticalProduct.pharmaceutical_product_PK, "PHARMACEUTICAL_PRODUCT", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            PPSessionId = null;

            return pharmaceuticalProduct;
        }

        private void SaveProducts(int pharmaceuticalProductPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var productList = _productOperations.GetEntitiesByPharmaceuticalProduct(pharmaceuticalProductPk);
            productList.Sort((p1, p2) => p1.name.CompareTo(p2.name));

            foreach (var product in productList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += product.name;
            }

            _productPharmaceuticalProductMnOperations.DeleteByPharmaceuticalProduct(pharmaceuticalProductPk);

            var productPharmaceuticalProductMnList = new List<Product_mn_PK>(lbSrProducts.LbInput.Items.Count);

            foreach (ListItem listItem in lbSrProducts.LbInput.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value) || (listItem.Value == "-1" && listItem.Text == "<NEW>")) continue;

                productPharmaceuticalProductMnList.Add(new Product_mn_PK(null, int.Parse(listItem.Value), pharmaceuticalProductPk));

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            if (productPharmaceuticalProductMnList.Count > 0)
            {
                _productPharmaceuticalProductMnOperations.SaveCollection(productPharmaceuticalProductMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, pharmaceuticalProductPk.ToString(), "PP_PRODUCTS");
        }

        private void SaveAdministrationRoutes(int pharmaceuticalProductPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var administrationRouteList = _administrationRouteOperations.GetEntitiesByPharmaceuticalProduct(pharmaceuticalProductPk);

            foreach (var administrationRoute in administrationRouteList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += administrationRoute.adminroutecode;
            }

            _pharmaceuticalProductAdministrationRouteMnOperations.DeleteByPharmaceuticalProduct(pharmaceuticalProductPk);

            var pharmaceuticalProductAdministrationRouteMnList = new List<Pp_ar_mn_PK>(lbSrAdministrationRoutes.LbInput.Items.Count);

            foreach (ListItem listItem in lbSrAdministrationRoutes.LbInput.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                pharmaceuticalProductAdministrationRouteMnList.Add(new Pp_ar_mn_PK(null, int.Parse(listItem.Value), pharmaceuticalProductPk));

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            if (pharmaceuticalProductAdministrationRouteMnList.Count > 0)
            {
                _pharmaceuticalProductAdministrationRouteMnOperations.SaveCollection(pharmaceuticalProductAdministrationRouteMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, pharmaceuticalProductPk.ToString(), "PP_ADMINISTRATION_ROUTES");
        }

        private void SaveMedicalDevices(int pharmaceuticalProductPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var medicalDeviceList = _medicalDeviceOperations.GetEntitiesByPharmaceuticalProduct(pharmaceuticalProductPk);

            foreach (var medicalDevice in medicalDeviceList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += medicalDevice.medicaldevicecode;
            }

            _pharmaceuticalProductMedicalDeviceMnOperations.DeleteByPharmaceuticalProduct(pharmaceuticalProductPk);

            var pharmaceuticalProductMedicalDeviceMnList = new List<Pp_md_mn_PK>(lbSrMedicalDevices.LbInput.Items.Count);

            foreach (ListItem listItem in lbSrMedicalDevices.LbInput.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                pharmaceuticalProductMedicalDeviceMnList.Add(new Pp_md_mn_PK(null, int.Parse(listItem.Value), pharmaceuticalProductPk));

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            if (pharmaceuticalProductMedicalDeviceMnList.Count > 0)
            {
                _pharmaceuticalProductMedicalDeviceMnOperations.SaveCollection(pharmaceuticalProductMedicalDeviceMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, pharmaceuticalProductPk.ToString(), "PP_MEDICAL_DEVICES");
        }

        private void SaveActiveIngredients(int pharmaceuticalProductPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var activeIngredientList = _activeIngredientOperations.GetIngredientsByPPPK(pharmaceuticalProductPk);

            foreach (var activeIngredient in activeIngredientList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += activeIngredient.concise;
            }

            var substanceList = _pharmaceuticalProductSubstanceOperations.GetEntitiesByTypeAndSessionId(PharmaceuticalProductSubstance.SubstanceType.ActiveIngredient.ToString(), PPSessionId);

            var activeIngredientPkListToDelete = new List<int>();
            activeIngredientList.ForEach(a => { if (!substanceList.Exists(s => s.ppsubstance_FK == a.activeingredient_PK)) activeIngredientPkListToDelete.Add(a.activeingredient_PK.Value); });
            _activeIngredientOperations.DeleteCollection(activeIngredientPkListToDelete);

            activeIngredientList = new List<Activeingredient_PK>(substanceList.Count);
            activeIngredientList.AddRange(substanceList.Select(substance => new Activeingredient_PK()
            {
                activeingredient_PK =  FormType == FormType.Edit ? substance.ppsubstance_FK : null,
                pp_FK = pharmaceuticalProductPk,
                substancecode_FK = substance.substancecode_FK,
                concentrationtypecode = substance.concentrationtypecode,
                ExpressedBy_FK = substance.expressedby_FK,
                lowamountnumervalue = substance.lowamountnumervalue,
                lowamountnumerprefix = substance.lowamountnumerprefix,
                lowamountnumerunit = substance.lowamountnumerunit,
                lowamountdenomvalue = substance.lowamountdenomvalue,
                lowamountdenomprefix = substance.lowamountdenomprefix,
                lowamountdenomunit = substance.lowamountdenomunit,
                highamountnumervalue = substance.highamountnumervalue,
                highamountnumerprefix = substance.highamountnumerprefix,
                highamountnumerunit = substance.highamountnumerunit,
                highamountdenomvalue = substance.highamountdenomvalue,
                highamountdenomprefix = substance.highamountdenomprefix,
                highamountdenomunit = substance.highamountdenomunit,
                concise = substance.concise,
                userID = SessionManager.Instance.CurrentUser.UserID
            }));

            foreach (var activeIngredient in activeIngredientList)
            {
                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += activeIngredient.concise;
            }

            if (activeIngredientList.Count > 0)
            {
                _activeIngredientOperations.SaveCollection(activeIngredientList);
            }

            if (complexAuditOldValue != complexAuditNewValue) txtActiveIngredientsHidden.IsModified = true;

            _pharmaceuticalProductSubstanceOperations.DeleteByTypeAndSessionId(PharmaceuticalProductSubstance.SubstanceType.ActiveIngredient.ToString(), PPSessionId);

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, pharmaceuticalProductPk.ToString(), "PP_ACTIVE_INGREDIENTS");
        }

        private void SaveExcipients(int pharmaceuticalProductPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var excipientList = _excipientOperations.GetExcipientsByPPPK(pharmaceuticalProductPk);

            foreach (var excipient in excipientList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += excipient.concise;
            }

            var substanceList = _pharmaceuticalProductSubstanceOperations.GetEntitiesByTypeAndSessionId(PharmaceuticalProductSubstance.SubstanceType.Excipient.ToString(), PPSessionId);

            var excipientPkListToDelete = new List<int>();
            excipientList.ForEach(e => { if (!substanceList.Exists(s => s.ppsubstance_FK == e.excipient_PK)) excipientPkListToDelete.Add(e.excipient_PK.Value); });
            _excipientOperations.DeleteCollection(excipientPkListToDelete);

            excipientList = new List<Excipient_PK>(substanceList.Count);
            excipientList.AddRange(substanceList.Select(substance => new Excipient_PK()
            {
                excipient_PK = FormType == FormType.Edit ? substance.ppsubstance_FK : null,
                pp_FK = pharmaceuticalProductPk,
                substancecode_FK = substance.substancecode_FK,
                concentrationtypecode = substance.concentrationtypecode,
                ExpressedBy_FK = substance.expressedby_FK,
                lowamountnumervalue = substance.lowamountnumervalue,
                lowamountnumerprefix = substance.lowamountnumerprefix,
                lowamountnumerunit = substance.lowamountnumerunit,
                lowamountdenomvalue = substance.lowamountdenomvalue,
                lowamountdenomprefix = substance.lowamountdenomprefix,
                lowamountdenomunit = substance.lowamountdenomunit,
                highamountnumervalue = substance.highamountnumervalue,
                highamountnumerprefix = substance.highamountnumerprefix,
                higamountnumerunit = substance.highamountnumerunit,
                highamountdenomvalue = substance.highamountdenomvalue,
                highamountdenomprefix = substance.highamountdenomprefix,
                highamountdenomunit = substance.highamountdenomunit,
                concise = substance.concise,
                userID = SessionManager.Instance.CurrentUser.UserID
            }));

            foreach (var excipient in excipientList)
            {
                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += excipient.concise;
            }

            if (excipientList.Count > 0)
            {
                _excipientOperations.SaveCollection(excipientList);
            }

            if (complexAuditOldValue != complexAuditNewValue) txtExcipientsHidden.IsModified = true;

            _pharmaceuticalProductSubstanceOperations.DeleteByTypeAndSessionId(PharmaceuticalProductSubstance.SubstanceType.Excipient.ToString(), PPSessionId);

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, pharmaceuticalProductPk.ToString(), "PP_EXCIPIENTS");
        }

        private void SaveAdjuvants(int pharmaceuticalProductPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var adjuvantList = _adjuvantOperations.GetAdjuvantsByPPPK(pharmaceuticalProductPk);

            foreach (var adjuvant in adjuvantList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += adjuvant.concise;
            }

            var substanceList = _pharmaceuticalProductSubstanceOperations.GetEntitiesByTypeAndSessionId(PharmaceuticalProductSubstance.SubstanceType.Adjuvant.ToString(), PPSessionId);

            var adjuvantPkListToDelete = new List<int>();
            adjuvantList.ForEach(a => { if (!substanceList.Exists(s => s.ppsubstance_FK == a.adjuvant_PK)) adjuvantPkListToDelete.Add(a.adjuvant_PK.Value); });
            _adjuvantOperations.DeleteCollection(adjuvantPkListToDelete);

            adjuvantList = new List<Adjuvant_PK>(substanceList.Count);
            adjuvantList.AddRange(substanceList.Select(substance => new Adjuvant_PK()
            {
                adjuvant_PK = FormType == FormType.Edit ? substance.ppsubstance_FK : null,
                pp_FK = pharmaceuticalProductPk,
                substancecode_FK = substance.substancecode_FK,
                concentrationtypecode = substance.concentrationtypecode,
                ExpressedBy_FK = substance.expressedby_FK,
                lowamountnumervalue = substance.lowamountnumervalue,
                lowamountnumerprefix = substance.lowamountnumerprefix,
                lowamountnumerunit = substance.lowamountnumerunit,
                lowamountdenomvalue = substance.lowamountdenomvalue,
                lowamountdenomprefix = substance.lowamountdenomprefix,
                lowamountdenomunit = substance.lowamountdenomunit,
                highamountnumervalue = substance.highamountnumervalue,
                highamountnumerprefix = substance.highamountnumerprefix,
                higamountnumerunit = substance.highamountnumerunit,
                highamountdenomvalue = substance.highamountdenomvalue,
                highamountdenomprefix = substance.highamountdenomprefix,
                highamountdenomunit = substance.highamountdenomunit,
                concise = substance.concise,
                userID = SessionManager.Instance.CurrentUser.UserID
            }));

            foreach (var adjuvant in adjuvantList)
            {
                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += adjuvant.concise;
            }

            if (adjuvantList.Count > 0)
            {
                _adjuvantOperations.SaveCollection(adjuvantList);
            }

            _pharmaceuticalProductSubstanceOperations.DeleteByTypeAndSessionId(PharmaceuticalProductSubstance.SubstanceType.Adjuvant.ToString(), PPSessionId);

            if (complexAuditOldValue != complexAuditNewValue) txtAdjuvantsHidden.IsModified = true;

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, pharmaceuticalProductPk.ToString(), "PP_ADJUVANTS");
        }

        #endregion

        #region Delete

        void DeleteEntity(object arg)
        {

        }

        #endregion

        #endregion

        #region Event handlers

        #region Products

        private void LbSrProducts_OnOkButtonClick(object sender, FormEventArgs<List<int>> e)
        {
            foreach (var selectedId in lbSrProducts.Searcher.SelectedItems)
            {
                if (lbSrProducts.LbInput.Items.FindByValue(selectedId.ToString()) != null) continue;

                var product = _productOperations.GetEntity(selectedId);

                if (product != null)
                {
                    var text = !string.IsNullOrWhiteSpace(product.name) ? product.name : Constant.UnknownValue;

                    lbSrProducts.LbInput.Items.Add(new ListItem(text, selectedId.ToString()));
                }
            }
        }

        #endregion

        #region Administration routes

        private void LbSrAdministrationRoutes_OnOkButtonClick(object sender, FormEventArgs<List<int>> e)
        {
            foreach (var selectedId in lbSrAdministrationRoutes.Searcher.SelectedItems)
            {
                if (lbSrAdministrationRoutes.LbInput.Items.FindByValue(selectedId.ToString()) != null) continue;

                var administrationRoute = _administrationRouteOperations.GetEntity(selectedId);

                if (administrationRoute != null)
                {
                    var text = !string.IsNullOrWhiteSpace(administrationRoute.adminroutecode) ? administrationRoute.adminroutecode : Constant.UnknownValue;

                    lbSrAdministrationRoutes.LbInput.Items.Add(new ListItem(text, selectedId.ToString()));
                }
            }
        }

        #endregion

        #region Medical devices

        private void LbSrMedicalDevices_OnOkButtonClick(object sender, FormEventArgs<List<int>> e)
        {
            foreach (var selectedId in lbSrMedicalDevices.Searcher.SelectedItems)
            {
                if (lbSrMedicalDevices.LbInput.Items.FindByValue(selectedId.ToString()) != null) continue;

                var medicalDevice = _medicalDeviceOperations.GetEntity(selectedId);

                if (medicalDevice != null)
                {
                    var text = !string.IsNullOrWhiteSpace(medicalDevice.medicaldevicecode) ? medicalDevice.medicaldevicecode : Constant.UnknownValue;

                    lbSrMedicalDevices.LbInput.Items.Add(new ListItem(text, selectedId.ToString()));
                }
            }
        }

        #endregion

        #region Substances

        public void btnAddActiveIngredient_OnClick(object sender, EventArgs e)
        {
            var substance = new PharmaceuticalProductSubstance()
            {
                sessionid = PPSessionId,
                substancetype = PharmaceuticalProductSubstance.SubstanceType.ActiveIngredient.ToString()
            };

            PPSubstancePopup.ShowModalForm(substance, PharmaceuticalProductSubstance.SubstanceType.ActiveIngredient);
        }

        public void btnAddExcipient_OnClick(object sender, EventArgs e)
        {
            var substance = new PharmaceuticalProductSubstance()
            {
                sessionid = PPSessionId,
                substancetype = PharmaceuticalProductSubstance.SubstanceType.Excipient.ToString()
            };

            PPSubstancePopup.ShowModalForm(substance, PharmaceuticalProductSubstance.SubstanceType.Excipient);
        }

        public void btnAddAdjuvant_OnClick(object sender, EventArgs e)
        {
            var substance = new PharmaceuticalProductSubstance()
            {
                sessionid = PPSessionId,
                substancetype = PharmaceuticalProductSubstance.SubstanceType.Adjuvant.ToString()
            };

            PPSubstancePopup.ShowModalForm(substance, PharmaceuticalProductSubstance.SubstanceType.Adjuvant);
        }

        public void btnEditSubstance_OnClick(object sender, EventArgs e)
        {
            var button = (sender as LinkButton);

            if (button == null) return;

            var substance = _pharmaceuticalProductSubstanceOperations.GetEntity(button.CommandArgument);

            if (substance == null) return;

            PharmaceuticalProductSubstance.SubstanceType substanceType;
            if (Enum.TryParse(substance.substancetype, true, out substanceType))
            {
                PPSubstancePopup.ShowModalForm(substance, substanceType);
            }
        }

        public void PPSubstancePopup_OnOkButtonClick(object sender, FormEventArgs<PharmaceuticalProductSubstance> e)
        {
            var substance = e.Data;

            substance.sessionid = PPSessionId;
            substance.modifieddate = DateTime.Now;

            _pharmaceuticalProductSubstanceOperations.Save(substance);

            BindGrid();
        }

        public void btnDeleteSubstance_OnClick(object sender, EventArgs e)
        {
            var button = (sender as ImageButton);

            if (button == null) return;

            int ppSubstancePkToDelete;
            if (int.TryParse(button.CommandArgument, out ppSubstancePkToDelete))
            {
                _ppSubstancePkToDelete = ppSubstancePkToDelete;
                mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
            }
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            _pharmaceuticalProductSubstanceOperations.Delete(_ppSubstancePkToDelete);
            BindGrid();
        }

        protected void lbtnActiveIngredientErrors_OnClick(object sender, EventArgs e)
        {
            var button = (sender as LinkButton);

            if (button == null) return;

            if (Request.QueryString["XevprmValidation"].NotIn("1", "2", "3", "4", "6")) return;

            int activeingredientPk;

            if (!int.TryParse(button.CommandArgument, out activeingredientPk)) return;

            var activeIngredient = _activeIngredientOperations.GetEntity(activeingredientPk);

            if (activeIngredient == null) return;

            XevprmOperationType operationType;
            Enum.TryParse(Request.QueryString["XevprmValidation"], true, out operationType);

            var validationResult = XevprmXml.ValidateActiveIngredient(activeIngredient, operationType);

            XevprmValidationErrorPopup.ShowModalForm(validationResult, false);
        }

        protected void lbtnExcipientErrors_OnClick(object sender, EventArgs e)
        {
            var button = (sender as LinkButton);

            if (button == null) return;

            if (Request.QueryString["XevprmValidation"].NotIn("1", "2", "3", "4", "6")) return;

            int excipientPk;

            if (!int.TryParse(button.CommandArgument, out excipientPk)) return;

            var excipient = _excipientOperations.GetEntity(excipientPk);

            if (excipient == null) return;

            XevprmOperationType operationType;
            Enum.TryParse(Request.QueryString["XevprmValidation"], true, out operationType);

            var validationResult = XevprmXml.ValidateExcipient(excipient, operationType);

            XevprmValidationErrorPopup.ShowModalForm(validationResult, false);
        }

        protected void lbtnAdjuvantErrors_OnClick(object sender, EventArgs e)
        {
            var button = (sender as LinkButton);

            if (button == null) return;

            if (Request.QueryString["XevprmValidation"].NotIn("1", "2", "3", "4", "6")) return;

            int adjuvantPk;

            if (!int.TryParse(button.CommandArgument, out adjuvantPk)) return;

            var adjuvant = _adjuvantOperations.GetEntity(adjuvantPk);

            if (adjuvant == null) return;

            XevprmOperationType operationType;
            Enum.TryParse(Request.QueryString["XevprmValidation"], true, out operationType);

            var validationResult = XevprmXml.ValidateAdjuvant(adjuvant, operationType);

            XevprmValidationErrorPopup.ShowModalForm(validationResult, false);
        }

        #endregion

        #region Grid

        void ActiveIngredientsGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            if (Request.QueryString["XevprmValidation"].NotIn("1", "2", "3", "4", "6")) return;

            int activeingredientPk;

            if (!int.TryParse(Convert.ToString(e.GetValue("ppsubstance_FK")), out activeingredientPk)) return;

            if (activeingredientPk <= 0) return;

            var activeIngredient = _activeIngredientOperations.GetEntity(activeingredientPk);

            if (activeIngredient == null) return;

            XevprmOperationType operationType;
            Enum.TryParse(Request.QueryString["XevprmValidation"], true, out operationType);

            var validationResult = XevprmXml.ValidateActiveIngredient(activeIngredient, operationType);

            if (validationResult.XevprmValidationExceptions.Count == 0) return;

            var errorFieldMaping = GetActErrorFieldMaping();

            var buttonErrors = e.Row.FindControl("lbtnActiveIngredientErrors") as LinkButton;

            if (buttonErrors != null)
            {
                buttonErrors.Visible = true;
            }

            foreach (DataControlFieldCell cell in e.Row.Cells)
            {
                if (!cell.ContainingField.Visible) continue;

                string fieldName = string.Empty;

                if (cell.ContainingField as PossGrid.PossBoundField != null)
                {
                    fieldName = (cell.ContainingField as PossGrid.PossBoundField).FieldName;
                }

                if (cell.ContainingField as PossGrid.PossTemplateField != null)
                {
                    fieldName = (cell.ContainingField as PossGrid.PossTemplateField).FieldName;
                }

                if (string.IsNullOrWhiteSpace(fieldName)) continue;

                var validationErrors = validationResult.XevprmValidationExceptions.Where(x => errorFieldMaping.ContainsKey(x.XevprmValidationRuleId) && errorFieldMaping[x.XevprmValidationRuleId] == fieldName).Select(x => x.ReadyMessage).ToList();

                if (validationErrors.Count > 0)
                {
                    cell.ForeColor = Color.Red;

                    foreach (Control control in cell.Controls)
                    {
                        if (control is WebControl)
                            (control as WebControl).ForeColor = Color.Red;
                    }

                    if (string.IsNullOrWhiteSpace(cell.Text) || cell.Text == "&nbsp;")
                    {
                        if (cell.Controls.Count == 0)
                        {
                            cell.Text = Constant.UnknownValue;
                        }
                    }
                    cell.ToolTip = String.Join("<br/>", validationErrors);
                }
            }
        }

        void ActiveIngredientsGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void ActiveIngredientsGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!ActiveIngredientsGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = ActiveIngredientsGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (ActiveIngredientsGrid.SortCount > 1 && e.FieldName == ActiveIngredientsGrid.MainSortingColumn)
                return;

            if (e.FieldName != "LowNumValue" && e.FieldName != "LowDenValue" && e.FieldName != "HighNumValue" && e.FieldName != "HighDenValue")
            {
                e.Cell.CssClass = _flip ? "sorted_column_even" : "sorted_column_odd";
            }
            else
            {
                e.Cell.CssClass = _flip ? "sorted_column_even_right" : "sorted_column_odd_right";
            }
        }

        void ExcipientsGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            if (!new[] { "1", "2", "3", "4", "6" }.Contains(Request.QueryString["XevprmValidation"])) return;

            int excipientPk;

            if (!int.TryParse(Convert.ToString(e.GetValue("ppsubstance_FK")), out excipientPk)) return;

            if (excipientPk <= 0) return;

            var excipient = _excipientOperations.GetEntity(excipientPk);

            if (excipient == null) return;

            XevprmOperationType operationType;
            Enum.TryParse(Request.QueryString["XevprmValidation"], true, out operationType);

            var validationResult = XevprmXml.ValidateExcipient(excipient, operationType);

            if (validationResult.XevprmValidationExceptions.Count == 0) return;

            var errorFieldMaping = GetExcErrorFieldMaping();

            var buttonErrors = e.Row.FindControl("lbtnExcipientErrors") as LinkButton;

            if (buttonErrors != null)
            {
                buttonErrors.Visible = true;
            }

            foreach (DataControlFieldCell cell in e.Row.Cells)
            {
                if (!cell.ContainingField.Visible) continue;

                string fieldName = string.Empty;

                if (cell.ContainingField as PossGrid.PossBoundField != null)
                {
                    fieldName = (cell.ContainingField as PossGrid.PossBoundField).FieldName;
                }

                if (cell.ContainingField as PossGrid.PossTemplateField != null)
                {
                    fieldName = (cell.ContainingField as PossGrid.PossTemplateField).FieldName;
                }

                if (string.IsNullOrWhiteSpace(fieldName)) continue;

                var validationErrors = validationResult.XevprmValidationExceptions.Where(x => errorFieldMaping.ContainsKey(x.XevprmValidationRuleId) && errorFieldMaping[x.XevprmValidationRuleId] == fieldName).Select(x => x.ReadyMessage).ToList();

                if (validationErrors.Count > 0)
                {
                    cell.ForeColor = Color.Red;

                    foreach (Control control in cell.Controls)
                    {
                        if (control is WebControl)
                            (control as WebControl).ForeColor = Color.Red;
                    }

                    if (string.IsNullOrWhiteSpace(cell.Text) || cell.Text == "&nbsp;")
                    {
                        if (cell.Controls.Count == 0)
                        {
                            cell.Text = Constant.UnknownValue;
                        }
                    }
                    cell.ToolTip = String.Join("<br/>", validationErrors);
                }
            }
        }

        void ExcipientsGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void ExcipientsGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!ExcipientsGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = ExcipientsGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (ExcipientsGrid.SortCount > 1 && e.FieldName == ExcipientsGrid.MainSortingColumn)
                return;

            if (e.FieldName == "SubstanceName")
            {
                e.Cell.CssClass = _flip ? "sorted_column_even" : "sorted_column_odd";
            }
            else
            {
                e.Cell.CssClass = _flip ? "sorted_column_even_right" : "sorted_column_odd_right";
            }
        }

        void AdjuvantsGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            if (!new[] { "1", "2", "3", "4", "6" }.Contains(Request.QueryString["XevprmValidation"])) return;

            int adjuvantPk;

            if (!int.TryParse(Convert.ToString(e.GetValue("ppsubstance_FK")), out adjuvantPk)) return;

            if (adjuvantPk <= 0) return;

            var adjuvant = _adjuvantOperations.GetEntity(adjuvantPk);

            if (adjuvant == null) return;

            XevprmOperationType operationType;
            Enum.TryParse(Request.QueryString["XevprmValidation"], true, out operationType);

            var validationResult = XevprmXml.ValidateAdjuvant(adjuvant, operationType);

            if (validationResult.XevprmValidationExceptions.Count == 0) return;

            var errorFieldMaping = GetAdjErrorFieldMaping();

            var buttonErrors = e.Row.FindControl("lbtnAdjuvantErrors") as LinkButton;

            if (buttonErrors != null)
            {
                buttonErrors.Visible = true;
            }

            foreach (DataControlFieldCell cell in e.Row.Cells)
            {
                if (!cell.ContainingField.Visible) continue;

                string fieldName = string.Empty;

                if (cell.ContainingField as PossGrid.PossBoundField != null)
                {
                    fieldName = (cell.ContainingField as PossGrid.PossBoundField).FieldName;
                }

                if (cell.ContainingField as PossGrid.PossTemplateField != null)
                {
                    fieldName = (cell.ContainingField as PossGrid.PossTemplateField).FieldName;
                }

                if (string.IsNullOrWhiteSpace(fieldName)) continue;

                var validationErrors = validationResult.XevprmValidationExceptions.Where(x => errorFieldMaping.ContainsKey(x.XevprmValidationRuleId) && errorFieldMaping[x.XevprmValidationRuleId] == fieldName).Select(x => x.ReadyMessage).ToList();

                if (validationErrors.Count > 0)
                {
                    cell.ForeColor = Color.Red;

                    foreach (Control control in cell.Controls)
                    {
                        if (control is WebControl)
                            (control as WebControl).ForeColor = Color.Red;
                    }

                    if (string.IsNullOrWhiteSpace(cell.Text) || cell.Text == "&nbsp;")
                    {
                        if (cell.Controls.Count == 0)
                        {
                            cell.Text = Constant.UnknownValue;
                        }
                    }
                    cell.ToolTip = String.Join("<br/>", validationErrors);
                }
            }
        }

        void AdjuvantsGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void AdjuvantsGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!AdjuvantsGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = AdjuvantsGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (AdjuvantsGrid.SortCount > 1 && e.FieldName == AdjuvantsGrid.MainSortingColumn)
                return;

            if (e.FieldName != "LowNumValue" && e.FieldName != "LowDenValue" && e.FieldName != "HighNumValue" && e.FieldName != "HighDenValue")
            {
                e.Cell.CssClass = _flip ? "sorted_column_even" : "sorted_column_odd";
            }
            else
            {
                e.Cell.CssClass = _flip ? "sorted_column_even_right" : "sorted_column_odd_right";
            }
        }

        #endregion

        #endregion

        #region Support methods

        private string ConstructPharmaceuticalProductName()
        {
            if (!string.IsNullOrWhiteSpace(txtPharmaceuticalProductName.Text)) return txtPharmaceuticalProductName.Text;

            var pharmaceuticalProductName = string.Empty;

            var activeIngredientConciseList = new List<string>();
            var substanceList = _pharmaceuticalProductSubstanceOperations.GetEntitiesByTypeAndSessionId(PharmaceuticalProductSubstance.SubstanceType.ActiveIngredient.ToString(), PPSessionId);

            if (substanceList.Any()) substanceList.ForEach(s => activeIngredientConciseList.Add(s.concise));

            if (activeIngredientConciseList.Count > 0)
            {
                pharmaceuticalProductName = String.Join("+", activeIngredientConciseList);
            }

            pharmaceuticalProductName += string.Format(" ({0})", ddlPharmaceuticalForm.SelectedId.HasValue ? ddlPharmaceuticalForm.Text : "N/A");

            return pharmaceuticalProductName;
        }

        private void StylizeArticle57RelevantControls(bool? isArticle57Relevant)
        {
            ddlPharmaceuticalForm.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, Convert.ToString(ddlPharmaceuticalForm.SelectedValue), isArticle57Relevant));

            var numberOfAdministrationRoutes = lbSrAdministrationRoutes.LbInput.Items.Count > 0 ? lbSrAdministrationRoutes.LbInput.Items.Count.ToString() : null;
            lbSrAdministrationRoutes.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, numberOfAdministrationRoutes, isArticle57Relevant));

            var numberOfMedicalDevices = lbSrMedicalDevices.LbInput.Items.Count > 0 ? lbSrMedicalDevices.LbInput.Items.Count.ToString() : null;
            lbSrMedicalDevices.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, numberOfMedicalDevices, isArticle57Relevant));
        }

        public string HandleMissing(object value)
        {
            if (value != null && value.ToString().Trim() != "")
            {
                return value.ToString();
            }

            return "Missing";
        }

        #endregion

        #region Security

        public void SecurityPageSpecific()
        {
            if (IsPostBack) return;

            if (_containerPage != null)
            {
                _containerPage.SecurityPageSpecific();
                _containerPage.SecurityPageSpecificMy(_isResponsibleUser);
            }
        }

        #endregion
    }
}