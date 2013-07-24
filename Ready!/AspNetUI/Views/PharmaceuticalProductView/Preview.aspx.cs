using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using AspNetUIFramework;
using GEM2Common;
using Ready.Model;

namespace AspNetUI.Views.PharmaceuticalProductView
{
    public partial class Preview : PreviewPage
    {
        #region Declarations

        private int? _idPharmProd;
        private bool? _isResponsibleUser;

        private int _sortCount;
        private bool _flip = true;

        private IProduct_PKOperations _productOperations;
        private IPharmaceutical_product_PKOperations _pharmaceuticalProductOperations;
        private IPharmaceuticalProductSubstanceOperations _pharmaceuticalProductSubstanceOperations;
        private IPharmaceutical_form_PKOperations _pharmaceuticalFormOperations;
        private IMedicaldevice_PKOperations _medicalDeviceOperations;
        private IAdminroute_PKOperations _administrationRouteOperations;
        private IActiveingredient_PKOperations _activeIngredientOperations;
        private IExcipient_PKOperations _excipientOperations;
        private IAdjuvant_PKOperations _adjuvantOperations;
        private IPerson_PKOperations _personOperations;
        private ILast_change_PKOperations _lastChangeOperations;
        private IUSEROperations _userOperations;

        #endregion

        #region Properties

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LoadFormVariables();
            BindEventHandlers();
            GenerateTabMenuItems();
            GenerateContexMenuItems();
            AssociatePanels(pnlProperties, pnlFooter);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsPostBack)
            {
                BindDynamicControls(null);
                return;
            }

            InitForm(null);
            BindForm(null);
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

            _idPharmProd = ValidationHelper.IsValidInt(Request.QueryString["idPharmProd"]) ? int.Parse(Request.QueryString["idPharmProd"]) : (int?)null;

            _productOperations = new Product_PKDAL();
            _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
            _pharmaceuticalProductSubstanceOperations = new PharmaceuticalProductSubstanceDAL();
            _pharmaceuticalFormOperations = new Pharmaceutical_form_PKDAL();
            _medicalDeviceOperations = new Medicaldevice_PKDAL();
            _administrationRouteOperations = new Adminroute_PKDAL();
            _activeIngredientOperations = new Activeingredient_PKDAL();
            _excipientOperations = new Excipient_PKDAL();
            _adjuvantOperations = new Adjuvant_PKDAL();
            _personOperations = new Person_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _userOperations = new USERDAL();
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }

            ActiveIngredientsGrid.OnRebindRequired += ActiveIngredientsGrid_OnRebindRequired;
            ActiveIngredientsGrid.OnHtmlRowPrepared += ActiveIngredientsGrid_OnHtmlRowPrepared;
            ActiveIngredientsGrid.OnHtmlCellPrepared += ActiveIngredientsGrid_OnHtmlCellPrepared;

            ExcipientsGrid.OnRebindRequired += ExcipientsGrid_OnRebindRequired;
            ExcipientsGrid.OnHtmlRowPrepared += ExcipientsGrid_OnHtmlRowPrepared;
            ExcipientsGrid.OnHtmlCellPrepared += ExcipientsGrid_OnHtmlCellPrepared;

            AdjuvantsGrid.OnRebindRequired += AdjuvantsGrid_OnRebindRequired;
            AdjuvantsGrid.OnHtmlRowPrepared += AdjuvantsGrid_OnHtmlRowPrepared;
            AdjuvantsGrid.OnHtmlCellPrepared += AdjuvantsGrid_OnHtmlCellPrepared;

            btnDelete.Click += btnDelete_OnClick;
            mpDelete.OnYesButtonClick += mpDelete_OnYesButtonClick;
        }

        void InitForm(object arg)
        {

        }

        #endregion

        #region Fill

        void ClearForm(object arg)
        {

        }

        void FillFormControls(object arg)
        {

        }

        void SetFormControlsDefaults(object arg)
        {
            btnDelete.Visible = _idPharmProd.HasValue && _pharmaceuticalProductOperations.AbleToDeleteEntity(_idPharmProd.Value);
            BindDynamicControls(null);
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            if (!_idPharmProd.HasValue) return;

            var pharmaceuticalProduct = _pharmaceuticalProductOperations.GetEntity(_idPharmProd);

            if (pharmaceuticalProduct == null || !pharmaceuticalProduct.pharmaceutical_product_PK.HasValue) return;

            lblPrvPharmaceuticalProduct.Text = !string.IsNullOrEmpty(pharmaceuticalProduct.name) ? pharmaceuticalProduct.name : Constant.ControlDefault.LbPrvText;

            //---------------------------------------------------------------LEFT PANE --------------------------------------------------------------

            // Products
            BindProducts(pharmaceuticalProduct.pharmaceutical_product_PK.Value);

            // Pharmaceutical product name
            lblPrvPharmaceuticalProductName.Text = !string.IsNullOrEmpty(pharmaceuticalProduct.name) ? pharmaceuticalProduct.name : Constant.ControlDefault.LbPrvText;

            // Responsible user
            BindResponsibleUser(pharmaceuticalProduct.responsible_user_FK);

            // Pharmaceutical product description
            lblPrvDescription.Text = !string.IsNullOrEmpty(pharmaceuticalProduct.description) ? pharmaceuticalProduct.description : Constant.ControlDefault.LbPrvText;

            // Pharmaceutical form
            BindPharmaceuticalForm(pharmaceuticalProduct.Pharmform_FK);

            // Administration routes
            BindAdministrationRoutes(pharmaceuticalProduct.pharmaceutical_product_PK.Value);

            // Medical devices
            BindMedicalDevices(pharmaceuticalProduct.pharmaceutical_product_PK.Value);

            // ID
            lblPrvId.Text = !string.IsNullOrEmpty(pharmaceuticalProduct.ID) ? pharmaceuticalProduct.ID : Constant.ControlDefault.LbPrvText;

            // Booked slot(s)
            lblPrvBookedSlots.Text = !string.IsNullOrEmpty(pharmaceuticalProduct.booked_slots) ? pharmaceuticalProduct.booked_slots : Constant.ControlDefault.LbPrvText;

            // Comment
            lblPrvComment.Text = !string.IsNullOrWhiteSpace(pharmaceuticalProduct.comments) ? pharmaceuticalProduct.comments : Constant.ControlDefault.LbPrvText;

            // Last change
            lblPrvLastChange.Text = LastChange.GetFormattedString(pharmaceuticalProduct.pharmaceutical_product_PK, "PHARMACEUTICAL_PRODUCT", _lastChangeOperations, _personOperations);

            //---------------------------------------------------------------FOOTER PANE --------------------------------------------------------------

            // Coloring
            StylizeArticle57RelevantControls(true);

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if (user != null) _isResponsibleUser = pharmaceuticalProduct.responsible_user_FK == user.Person_FK;
        }

        private void BindGrid()
        {
            BindActiveIngredientsGrid();
            BindExcipientsGrid();
            BindAdjuvantsGrid();
        }

        private void BindResponsibleUser(int? responsibleUserFk)
        {
            var responsibleUser = responsibleUserFk != null ? _personOperations.GetEntity(responsibleUserFk) : null;
            lblPrvResponsibleUser.Text = responsibleUser != null && !string.IsNullOrWhiteSpace(responsibleUser.FullName) ? responsibleUser.FullName : Constant.ControlDefault.LbPrvText;
        }

        private void BindPharmaceuticalForm(int? pharmaceuticalFormPk)
        {
            var pharmaceuticalForm = pharmaceuticalFormPk.HasValue ? _pharmaceuticalFormOperations.GetEntity(pharmaceuticalFormPk) : null;
            var pharmaceuticalFormName = pharmaceuticalForm != null ? pharmaceuticalForm.name : null;
            lblPrvPharmaceuticalForm.Text = !string.IsNullOrWhiteSpace(pharmaceuticalFormName) ? pharmaceuticalFormName : Constant.ControlDefault.LbPrvText;
        }

        private void BindAdministrationRoutes(int pharmaceuticalProductPk)
        {
            var administrationRouteList = _administrationRouteOperations.GetEntitiesByPharmaceuticalProduct(pharmaceuticalProductPk);
            var administrationRouteNameList = administrationRouteList.Select(administrationRoute => administrationRoute.adminroutecode).ToList();
            administrationRouteNameList.ForEach(item => StringOperations.ReplaceNullOrWhiteSpace(item, Constant.UnknownValue));

            lblPrvAdministrationRoutes.Text = StringOperations.ReplaceNullOrWhiteSpace(String.Join(", ", administrationRouteNameList), Constant.ControlDefault.LbPrvText);
        }

        private void BindMedicalDevices(int pharmaceuticalProductPk)
        {
            var medicalDeviceList = _medicalDeviceOperations.GetEntitiesByPharmaceuticalProduct(pharmaceuticalProductPk);
            var medicalDeviceNameList = medicalDeviceList.Select(medicalDevice => medicalDevice.medicaldevicecode).ToList();
            medicalDeviceNameList.ForEach(item => StringOperations.ReplaceNullOrWhiteSpace(item, Constant.UnknownValue));

            lblPrvMedicalDevices.Text = StringOperations.ReplaceNullOrWhiteSpace(String.Join(", ", medicalDeviceNameList), Constant.ControlDefault.LbPrvText);
        }

        private void BindProducts(int pharmaceuticalProductPk)
        {
            lblPrvProducts.Text = Constant.ControlDefault.LbPrvText;

            var productList = _productOperations.GetEntitiesByPharmaceuticalProduct(pharmaceuticalProductPk);

            if (productList.Count == 0)
            {
                divLinkedEntities.Visible = false;
                return;
            };

            pnlProperties.CssClass = "properties padding-0";
            lblPrvProducts.ShowLinks = true;
            lblPrvProducts.TextBold = true;
            lblPrvProducts.PnlLinks.Width = Unit.Pixel(800);
            var productNameList = new List<string>();

            foreach (var product in productList)
            {
                lblPrvProducts.PnlLinks.Controls.Add(new HyperLink
                {
                    ID = string.Format("Product_{0}", product.product_PK),
                    NavigateUrl = string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, product.product_PK),
                    Text = StringOperations.ReplaceNullOrWhiteSpace(product.name, Constant.UnknownValue)
                });
                lblPrvProducts.PnlLinks.Controls.Add(new LiteralControl("<br/>"));
                productNameList.Add(StringOperations.ReplaceNullOrWhiteSpace(product.name, Constant.UnknownValue));
            }
            lblPrvProducts.Text = StringOperations.ReplaceNullOrWhiteSpace(String.Join(", ", productNameList), Constant.ControlDefault.LbPrvText);
        }

        private void BindDynamicControls(object arg)
        {
            if (!_idPharmProd.HasValue) return;

            BindProducts(_idPharmProd.Value);
            BindFooterButtons();
        }

        private void BindFooterButtons()
        {
            if (SecurityHelper.IsPermitted(Permission.InsertDocument)) btnAddDocument.PostBackUrl = string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=New&idPharmProd={1}", EntityContext, _idPharmProd);
            else btnAddDocument.Disable();
        }

        private void BindActiveIngredientsGrid()
        {
            if (!_idPharmProd.HasValue) return;

            var filters = ActiveIngredientsGrid.GetFilters();

            filters.Add("SubstanceType", PharmaceuticalProductSubstance.SubstanceType.ActiveIngredient.ToString());
            filters.Add("PharmaceuticalProductPk", _idPharmProd.Value.ToString());

            var gobList = new List<GEMOrderBy>();
            if (ActiveIngredientsGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(ActiveIngredientsGrid.SecondSortingColumn, ActiveIngredientsGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (ActiveIngredientsGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(ActiveIngredientsGrid.MainSortingColumn, ActiveIngredientsGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("ppsubstance_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            var ds = _pharmaceuticalProductSubstanceOperations.GetPreviewFormDataSet(filters, ActiveIngredientsGrid.CurrentPage, ActiveIngredientsGrid.PageSize, gobList, out itemCount);

            ActiveIngredientsGrid.TotalRecords = itemCount;

            if (ActiveIngredientsGrid.CurrentPage > ActiveIngredientsGrid.TotalPages || (ActiveIngredientsGrid.CurrentPage == 0 && ActiveIngredientsGrid.TotalPages > 0))
            {
                if (ActiveIngredientsGrid.CurrentPage > ActiveIngredientsGrid.TotalPages) ActiveIngredientsGrid.CurrentPage = ActiveIngredientsGrid.TotalPages; else ActiveIngredientsGrid.CurrentPage = 1;

                ds = _pharmaceuticalProductSubstanceOperations.GetPreviewFormDataSet(filters, ActiveIngredientsGrid.CurrentPage, ActiveIngredientsGrid.PageSize, gobList, out itemCount);
            }

            ActiveIngredientsGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            ActiveIngredientsGrid.DataBind();
        }

        private void BindExcipientsGrid()
        {
            if (!_idPharmProd.HasValue) return;

            var filters = ExcipientsGrid.GetFilters();

            filters.Add("SubstanceType", PharmaceuticalProductSubstance.SubstanceType.Excipient.ToString());
            filters.Add("PharmaceuticalProductPk", _idPharmProd.Value.ToString());

            var gobList = new List<GEMOrderBy>();
            if (ExcipientsGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(ExcipientsGrid.SecondSortingColumn, ExcipientsGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (ExcipientsGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(ExcipientsGrid.MainSortingColumn, ExcipientsGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("ppsubstance_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            var ds = _pharmaceuticalProductSubstanceOperations.GetPreviewFormDataSet(filters, ExcipientsGrid.CurrentPage, ExcipientsGrid.PageSize, gobList, out itemCount);

            ExcipientsGrid.TotalRecords = itemCount;

            if (ExcipientsGrid.CurrentPage > ExcipientsGrid.TotalPages || (ExcipientsGrid.CurrentPage == 0 && ExcipientsGrid.TotalPages > 0))
            {
                if (ExcipientsGrid.CurrentPage > ExcipientsGrid.TotalPages) ExcipientsGrid.CurrentPage = ExcipientsGrid.TotalPages; else ExcipientsGrid.CurrentPage = 1;

                ds = _pharmaceuticalProductSubstanceOperations.GetPreviewFormDataSet(filters, ExcipientsGrid.CurrentPage, ExcipientsGrid.PageSize, gobList, out itemCount);
            }

            ExcipientsGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            ExcipientsGrid.DataBind();
        }

        private void BindAdjuvantsGrid()
        {
            if (!_idPharmProd.HasValue) return;

            var filters = AdjuvantsGrid.GetFilters();

            filters.Add("SubstanceType", PharmaceuticalProductSubstance.SubstanceType.Adjuvant.ToString());
            filters.Add("PharmaceuticalProductPk", _idPharmProd.Value.ToString());

            var gobList = new List<GEMOrderBy>();
            if (AdjuvantsGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(AdjuvantsGrid.SecondSortingColumn, AdjuvantsGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (AdjuvantsGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(AdjuvantsGrid.MainSortingColumn, AdjuvantsGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("ppsubstance_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            var ds = _pharmaceuticalProductSubstanceOperations.GetPreviewFormDataSet(filters, AdjuvantsGrid.CurrentPage, AdjuvantsGrid.PageSize, gobList, out itemCount);

            AdjuvantsGrid.TotalRecords = itemCount;

            if (AdjuvantsGrid.CurrentPage > AdjuvantsGrid.TotalPages || (AdjuvantsGrid.CurrentPage == 0 && AdjuvantsGrid.TotalPages > 0))
            {
                if (AdjuvantsGrid.CurrentPage > AdjuvantsGrid.TotalPages) AdjuvantsGrid.CurrentPage = AdjuvantsGrid.TotalPages; else AdjuvantsGrid.CurrentPage = 1;

                ds = _pharmaceuticalProductSubstanceOperations.GetPreviewFormDataSet(filters, AdjuvantsGrid.CurrentPage, AdjuvantsGrid.PageSize, gobList, out itemCount);
            }

            AdjuvantsGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            AdjuvantsGrid.DataBind();
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
                _pharmaceuticalProductOperations.Delete(entityPk);
                Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}", EntityContext.PharmaceuticalProduct));
            }

            MasterPage.ModalPopup.ShowModalPopup("Error!", "Could not delete entity! Contact your system administrator.");
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
                        var query = Request.QueryString["idLay"] != null ? string.Format("&idLay={0}", Request.QueryString["idLay"]) : null;

                        if (EntityContext == EntityContext.Product) Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.PharmaceuticalProduct) Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}{1}", EntityContext, query));
                        Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}", EntityContext.Product));
                    }
                    break;

                case ContextMenuEventTypes.Edit:
                    {
                        if (EntityContext == EntityContext.PharmaceuticalProduct && _idPharmProd.HasValue) Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/Form.aspx?EntityContext={0}&Action=Edit&idPharmProd={1}&From=PharmProdPreview", EntityContext, _idPharmProd));
                        Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}", EntityContext.PharmaceuticalProduct));
                    }
                    break;

                case ContextMenuEventTypes.SaveAs:
                    {
                        if (EntityContext == EntityContext.PharmaceuticalProduct && _idPharmProd.HasValue) Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/Form.aspx?EntityContext={0}&Action=SaveAs&idPharmProd={1}&From=PharmProdPreview", EntityContext, _idPharmProd));
                        Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}", EntityContext.PharmaceuticalProduct));
                    }
                    break;

                case ContextMenuEventTypes.PreviousItem:
                    {
                        if (_idPharmProd.HasValue)
                        {
                            Int32? prevPharmaceuticalProductPk = _pharmaceuticalProductOperations.GetPrevAlphabeticalEntity(_idPharmProd);

                            if (prevPharmaceuticalProductPk.HasValue)
                            {
                                Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/Preview.aspx?EntityContext={0}&idPharmProd={1}", EntityContext.PharmaceuticalProduct, prevPharmaceuticalProductPk));
                            }
                        }

                        Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}", EntityContext.PharmaceuticalProduct));
                    }
                    break;

                case ContextMenuEventTypes.NextItem:
                    {
                        if (_idPharmProd.HasValue)
                        {
                            Int32? nextPharmaceuticalProductPk = _pharmaceuticalProductOperations.GetNextAlphabeticalEntity(_idPharmProd);

                            if (nextPharmaceuticalProductPk.HasValue)
                            {
                                Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/Preview.aspx?EntityContext={0}&idPharmProd={1}", EntityContext.PharmaceuticalProduct, nextPharmaceuticalProductPk));
                            }
                        }

                        Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}", EntityContext.PharmaceuticalProduct));
                    }
                    break;
            }
        }

        #endregion

        #region Delete

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_idPharmProd);
        }

        #endregion


        #region Grid

        void ActiveIngredientsGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
        }

        void ActiveIngredientsGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void ActiveIngredientsGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (e.FieldName == "SubstanceName")
            {
                e.Cell.CssClass = "text-bold-pp-grid";
            }

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
                if (e.FieldName == "SubstanceName") e.Cell.CssClass = _flip ? "sorted_column_even text-bold-pp-grid" : " sorted_column_odd text-bold-pp-grid";
                else e.Cell.CssClass = _flip ? "sorted_column_even" : " sorted_column_odd";
            }
            else
            {
                e.Cell.CssClass = _flip ? "sorted_column_even_right" : "sorted_column_odd_right";
            }
        }

        void ExcipientsGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
        }

        void ExcipientsGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void ExcipientsGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (e.FieldName == "SubstanceName")
            {
                e.Cell.CssClass = "text-bold-pp-grid";
            }

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
                e.Cell.CssClass = _flip ? "sorted_column_even text-bold-pp-grid" : "sorted_column_odd text-bold-pp-grid";
            }
            else
            {
                e.Cell.CssClass = _flip ? "sorted_column_even_right" : "sorted_column_odd_right";
            }
        }

        void AdjuvantsGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
        }

        void AdjuvantsGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void AdjuvantsGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (e.FieldName == "SubstanceName")
            {
                e.Cell.CssClass = "text-bold-pp-grid";
            }

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
                if (e.FieldName == "SubstanceName") e.Cell.CssClass = _flip ? "sorted_column_even text-bold-pp-grid" : " sorted_column_odd text-bold-pp-grid";
                else e.Cell.CssClass = _flip ? "sorted_column_even" : "sorted_column_odd";
            }
            else
            {
                e.Cell.CssClass = _flip ? "sorted_column_even_right" : "sorted_column_odd_right";
            }
        }

        #endregion

        #endregion

        #region Support methods

        private void GenerateContexMenuItems()
        {
            var contexMenu = new[]
            {
                new ContextMenuItem(ContextMenuEventTypes.Back, "Back"), 
                new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit"), 
                new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As"),
                new ContextMenuItem(ContextMenuEventTypes.PreviousItem, "Previous"),
                new ContextMenuItem(ContextMenuEventTypes.NextItem, "Next")
            };

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contexMenu);

            if (SecurityHelper.IsPermitted(Permission.View))
            {
                int? nextId = null;
                int? prevId = null;

                if (_idPharmProd.HasValue)
                {
                    nextId = _pharmaceuticalProductOperations.GetNextAlphabeticalEntity(_idPharmProd);
                    prevId = _pharmaceuticalProductOperations.GetPrevAlphabeticalEntity(_idPharmProd);
                }

                if (prevId == null) MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.PreviousItem, "Previous") });
                if (nextId == null) MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.NextItem, "Next") });
            }
            else MasterPage.ContextMenu.SetContextMenuItemsDisabled(
                new[] { 
                        new ContextMenuItem(ContextMenuEventTypes.NextItem, "Next"), 
                        new ContextMenuItem(ContextMenuEventTypes.PreviousItem, "Previous") 
                      });
        }

        private void GenerateTabMenuItems()
        {
            tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, MasterPage.CurrentLocation);
            tabMenu.SelectItem(MasterPage.CurrentLocation, Support.CacheManager.Instance.AppLocations);
        }

        private void StylizeArticle57RelevantControls(bool? isArticle57Relevant)
        {
            lblPrvPharmaceuticalForm.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lblPrvPharmaceuticalForm.Text, isArticle57Relevant));
            lblPrvAdministrationRoutes.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lblPrvAdministrationRoutes.Text, isArticle57Relevant));
            lblPrvMedicalDevices.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvMedicalDevices.Text, isArticle57Relevant));
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

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            if (!base.SecurityPageSpecific())
            {
                if (SecurityHelper.IsPermitted(Permission.SaveAsPharmaceuticalProduct)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });

                if (SecurityHelper.IsPermitted(Permission.EditPharmaceuticalProduct)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit") });
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit") });

                if (SecurityHelper.IsPermitted(Permission.DeletePharmaceuticalProduct)) StyleHelper.EnableLinkButtonsWithCssClass(PnlFooter, "Delete");
                else StyleHelper.DisableLinkButtonsWithCssClass(PnlFooter, "Delete");

                SecurityPageSpecificMy(_isResponsibleUser);
            }

            return true;
        }

        #endregion
    }
}