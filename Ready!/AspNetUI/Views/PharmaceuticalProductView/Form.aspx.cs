using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using AspNetUIFramework;
using System;
using Ready.Model;

namespace AspNetUI.Views.PharmaceuticalProductView
{
    public partial class Form : FormPage
    {
        #region Declarations

        public int? _idPharmProd;
        public int? _idProd;
        public int? _idTimeUnit;
        private bool? _isResponsibleUser;

        private IProduct_PKOperations _productOperations;
        private IPharmaceutical_product_PKOperations _pharmaceuticalProductOperations;

        public virtual event EventHandler<EventArgs> OnTopMenuChange;

        #endregion

        #region Properties

        public LabelPreview LblPrvParentEntity
        {
            get { return lblPrvParentEntity; }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LoadFormVariables();
            BindEventHandlers();
            GenerateContextMenuItems();
            AssociatePanels(pnlForm, pnlFooter);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsPostBack) return;

            InitForm(null);
            SetFormControlsDefaults(null);
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

            _idPharmProd = ValidationHelper.IsValidInt(Request.QueryString["idPharmProd"]) ? int.Parse(Request.QueryString["idPharmProd"]) : (int?)null;
            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;
            _idTimeUnit = ValidationHelper.IsValidInt(Request.QueryString["idTimeUnit"]) ? int.Parse(Request.QueryString["idTimeUnit"]) : (int?)null;

            _productOperations = new Product_PKDAL();
            _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
        }

        public override void LoadActionQuery()
        {
            base.LoadActionQuery();

            if (_idProd.HasValue) EntityContext = EntityContext.Product;
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }
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
            lblPrvParentEntity.Text = Constant.ControlDefault.LbPrvText;
        }

        void FillFormControls(object arg)
        {

        }

        void SetFormControlsDefaults(object arg)
        {
            lblPrvParentEntity.Visible = FormType != FormType.New;
        }

        #endregion


        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);

            return null;
        }

        #endregion

        #endregion

        #region Event handlers

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                case ContextMenuEventTypes.Cancel:
                    if (EntityContext == EntityContext.Product)
                    {
                        if (FormType == FormType.New)
                        {
                            if (From == "Prod") Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext));
                            else if (From == "ProdSearch") Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                            else if (From == "ProdPreview" && _idProd.HasValue) Response.Redirect(string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext, _idProd));
                            else if (From == "TimeUnitProdList" && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}&idTimeUnit={1}", EntityContext.TimeUnit, _idTimeUnit));
                            else if (From == "TimeUnitMyProdList" && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}&idTimeUnit={1}", EntityContext.TimeUnitMy, _idTimeUnit));
                            else if (From == "ProdPharmProdList" && _idProd.HasValue) Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}&idProd={1}", EntityContext, _idProd));
                        }
                    }
                    else
                    {
                        if (FormType == FormType.New)
                        {
                            if (EntityContext == EntityContext.Default)
                            {
                                if (!string.IsNullOrWhiteSpace(From))
                                {
                                    if (From == "PharmProd" && _idProd.HasValue) Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}&idProd={1}", EntityContext.PharmaceuticalProduct, _idProd));
                                }
                            }
                        }
                        else if (FormType == FormType.Edit || FormType == FormType.SaveAs)
                        {
                            if (_idPharmProd.HasValue) Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/Preview.aspx?EntityContext={0}&idPharmProd={1}", EntityContext.PharmaceuticalProduct, _idPharmProd));
                        }
                    }

                    Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}", EntityContext.PharmaceuticalProduct));
                    break;

                case ContextMenuEventTypes.Save:

                    if (PharmaceuticalProductForm.ValidateForm(null))
                    {
                        SaveForm(null);

                        var savedPharmaceuticalProduct = PharmaceuticalProductForm.SaveForm(null);

                        if (savedPharmaceuticalProduct is Pharmaceutical_product_PK)
                        {
                            var pharmaceuticalProduct = savedPharmaceuticalProduct as Pharmaceutical_product_PK;
                            if (pharmaceuticalProduct.pharmaceutical_product_PK.HasValue)
                            {
                                MasterPage.OneTimePermissionToken = Permission.View;
                                Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/Preview.aspx?EntityContext={0}&idPharmProd={1}", EntityContext.PharmaceuticalProduct, pharmaceuticalProduct.pharmaceutical_product_PK));
                            }
                        }

                        Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}", EntityContext.PharmaceuticalProduct));
                    }
                    break;
            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            OnContextMenuItemClick(null, new ContextMenuEventArgs(ContextMenuEventTypes.Save));
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            OnContextMenuItemClick(null, new ContextMenuEventArgs(ContextMenuEventTypes.Cancel));
        }

        #endregion

        #endregion

        #region Support methods

        private void GenerateContextMenuItems()
        {
            var contexMenu = new[]
            {
                new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), 
                new ContextMenuItem(ContextMenuEventTypes.Save, "Save"), 
            };

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contexMenu);
        }

        private void GenerateTabMenuItems()
        {
            Location_PK location = null;

            if (FormType == FormType.New)
            {
                if (EntityContext == EntityContext.Product) location = Support.LocationManager.Instance.GetLocationByName("ProdPharmProdList", Support.CacheManager.Instance.AppLocations);
                else
                {
                    location = Support.LocationManager.Instance.ParseLocationFromUrl(Request.ExtractCurrentQuery(new List<string> { "EntityContext", "Action" }), Support.CacheManager.Instance.AppLocations);
                    tabMenu.Visible = false;
                    if (location != null)
                    {
                        MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                        MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                    }
                    return;
                }

                MasterPage.TabMenu.TabControls.Clear();
                tabMenu.Visible = true;
                if (location != null)
                {
                    tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                    tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                }
            }
            else
            {
                location = Support.LocationManager.Instance.GetLocationByName("PharmProdPreview", Support.CacheManager.Instance.AppLocations);
                MasterPage.TabMenu.TabControls.Clear();
                tabMenu.Visible = true;
                if (location != null)
                {
                    tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                    tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                }
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location;

            if (EntityContext == EntityContext.Product)
            {
                location = Support.LocationManager.Instance.GetLocationByName("ProdPharmProdList", Support.CacheManager.Instance.AppLocations);
            }
            else
            {
                location = Support.LocationManager.Instance.ParseLocationFromUrl(Request.ExtractCurrentQuery(new List<string> { "EntityContext", "Action" }), Support.CacheManager.Instance.AppLocations);
            }

            if (location != null)
            {
                var topLevelParent = MasterPage.FindTopLevelParent(location);

                MasterPage.CurrentLocation = location;
                MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
            }
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            return true;
        }

        public override void SecurityPageSpecificMy(bool? isResponsibleUser)
        {
            var isPermittedInsertPharmaceuticalProduct = false;
            if (EntityContext == EntityContext.Default) isPermittedInsertPharmaceuticalProduct = SecurityHelper.IsPermitted(Permission.InsertPharmaceuticalProduct);

            if (isPermittedInsertPharmaceuticalProduct)
            {
                SecurityHelper.SetControlsForReadWrite(
                               MasterPage.ContextMenu,
                               new[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save") },
                               new List<Panel> { PnlForm },
                               new Dictionary<Panel, List<string>> { { PnlFooter, new List<string> { "Save" } } }
                           );
            }
            else
            {
                SecurityHelper.SetControlsForRead(
                                  MasterPage.ContextMenu,
                                  new[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save") },
                                  new List<Panel> { PnlForm },
                                  new Dictionary<Panel, List<string>> { { PnlFooter, new List<string> { "Save" } } }
                              );
            }

            if (MasterPage.RefererLocation != null)
            {
                isPermittedInsertPharmaceuticalProduct = SecurityHelper.IsPermittedAny(new List<Permission> { Permission.InsertPharmaceuticalProduct, Permission.SaveAsPharmaceuticalProduct, Permission.EditPharmaceuticalProduct }, MasterPage.RefererLocation);
                if (isPermittedInsertPharmaceuticalProduct)
                {
                    SecurityHelper.SetControlsForReadWrite(
                                   MasterPage.ContextMenu,
                                   new[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save") },
                                   new List<Panel> { PnlForm },
                                   new Dictionary<Panel, List<string>> { { PnlFooter, new List<string> { "Save" } } }
                               );
                }
            }

            base.SecurityPageSpecificMy(_isResponsibleUser);
        }

        #endregion
    }
}