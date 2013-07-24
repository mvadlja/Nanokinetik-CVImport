using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using AspNetUIFramework;
using Ready.Model;

namespace AspNetUI.Views.TimeUnitView
{
    public partial class Preview : PreviewPage
    {
        #region Declarations

        private int? _idTimeUnit;
        private bool? _isResponsibleUser;

        private ITime_unit_PKOperations _timeUnitOperations;
        private ITime_unit_name_PKOperations _timeUnitNameOperations;
        private IActivity_PKOperations _activityOperations;
        private IProduct_PKOperations _productOperations;
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

            _idTimeUnit = ValidationHelper.IsValidInt(Request.QueryString["idTimeUnit"]) ? int.Parse(Request.QueryString["idTimeUnit"]) : (int?)null;

            _timeUnitOperations = new Time_unit_PKDAL();
            _timeUnitNameOperations = new Time_unit_name_PKDAL();
            _activityOperations = new Activity_PKDAL();
            _productOperations = new Product_PKDAL();
            _personOperations = new Person_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _userOperations = new USERDAL();
        }

        private void BindEventHandlers()
        {
            mpDelete.OnYesButtonClick += mpDelete_OnYesButtonClick;
            btnDelete.Click += btnDelete_Click;

            if (MasterPage != null && MasterPage.ContextMenu != null)
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

        }

        void FillFormControls(object arg)
        {

        }

        void SetFormControlsDefaults(object arg)
        {
            BindDynamicControls(null);
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            if (!_idTimeUnit.HasValue) return;

            var timeUnit = _timeUnitOperations.GetEntity(_idTimeUnit);
            if (timeUnit == null) return;

            var timeUnitName = timeUnit.time_unit_name_FK.HasValue ? _timeUnitNameOperations.GetEntity(timeUnit.time_unit_name_FK) : null;

            // Entity
            lblPrvTimeUnit.Text = timeUnitName != null ? timeUnitName.time_unit_name : Constant.ControlDefault.LbPrvText;

            // Name
            lblPrvName.Text = timeUnitName != null ? timeUnitName.time_unit_name : Constant.ControlDefault.LbPrvText;

            // Responsible user
            var responsibleUser = timeUnit.user_FK.HasValue ? _personOperations.GetEntity(timeUnit.user_FK) : null;
            lblPrvResponsibleUser.Text = responsibleUser != null ? responsibleUser.FullName : Constant.ControlDefault.LbPrvText;

            // Inserted by
            var insertedBy = timeUnit.inserted_by.HasValue ? _personOperations.GetEntity(timeUnit.inserted_by) : null;
            lblPrvInsertedBy.Text = insertedBy != null ? insertedBy.FullName : Constant.ControlDefault.LbPrvText;

            // Description
            lblPrvDescription.Text = !string.IsNullOrWhiteSpace(timeUnit.description) ? timeUnit.description : Constant.ControlDefault.LbPrvText;

            // Actual date
            lblPrvActualDate.Text = timeUnit.actual_date.HasValue ? timeUnit.actual_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Time
            var hours = timeUnit.time_hours.HasValue ? string.Format("{0:00}", timeUnit.time_hours) : "00";
            var minutes = timeUnit.time_minutes.HasValue ? string.Format("{0:00}", timeUnit.time_minutes) : "00";
            lblPrvTime.Text = string.Format("{0}:{1}", hours, minutes);

            // Comment
            lblPrvComment.Text = !string.IsNullOrWhiteSpace(timeUnit.comment) ? timeUnit.comment : Constant.ControlDefault.LbPrvText;

            // Last change
            lblPrvLastChange.Text = LastChange.GetFormattedString(timeUnit.time_unit_PK, "TIME_UNIT", _lastChangeOperations, _personOperations);

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if (user != null) _isResponsibleUser = timeUnit.user_FK == user.Person_FK;
        }

        private void BindDynamicControls(object arg)
        {
            if (!_idTimeUnit.HasValue) return;

            var timeUnit = _timeUnitOperations.GetEntity(_idTimeUnit.Value);
            int? activityPk = timeUnit != null ? timeUnit.activity_FK : null;

            // Products
            BindProducts(activityPk);

            // Activity
            BindActivity(activityPk);
        }

        private void BindProducts(int? activityPk)
        {
            var productList = activityPk.HasValue ? _productOperations.GetEntitiesByActivity(activityPk.Value) : new List<Product_PK>();

            if (productList.Count > 0)
            {
                lblPrvProducts.ShowLinks = true;

                foreach (var product in productList)
                {
                    lblPrvProducts.PnlLinks.Controls.Add(new HyperLink
                    {
                        ID = string.Format("Product_{0}", product.product_PK),
                        NavigateUrl = string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, product.product_PK),
                        Text = StringOperations.ReplaceNullOrWhiteSpace(product.name, Constant.UnknownValue)
                    });
                    lblPrvProducts.PnlLinks.Controls.Add(new LiteralControl("<br/>"));
                }
            }
            else
            {
                lblPrvProducts.Text = Constant.ControlDefault.LbPrvText;
            }

        }
        private void BindActivity(int? activityPk)
        {
            var activity = activityPk.HasValue ? _activityOperations.GetEntity(activityPk.Value) : null;
            if (activity != null && activity.activity_PK.HasValue)
            {
                lblPrvActivity.ShowLinks = true;

                var hlActivity = new HyperLink
                {
                    ID = string.Format("Activity_{0}", activityPk),
                    NavigateUrl = string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idAct={1}", EntityContext.Activity, activityPk),
                    Text = StringOperations.ReplaceNullOrWhiteSpace(activity.name, Constant.UnknownValue)
                };

                lblPrvActivity.PnlLinks.Controls.Add(hlActivity);
            }
            else
            {
                lblPrvActivity.Text = Constant.ControlDefault.LbPrvText;
            }
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

        void DeleteEntity(int? entityPk)
        {
            if (entityPk.HasValue)
            {
                try
                {
                    _timeUnitOperations.Delete(entityPk);
                    Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}", EntityContext));
                }
                catch
                {
                }
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

                        if (EntityContext == EntityContext.TimeUnit) Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}{1}", EntityContext.TimeUnit, query));
                        else if (EntityContext == EntityContext.TimeUnitMy) Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}{1}", EntityContext.TimeUnitMy, query));
                        Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}", EntityContext.TimeUnit));
                    }
                    break;

                case ContextMenuEventTypes.Edit:
                    {
                        if (EntityContext == EntityContext.TimeUnit && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/TimeUnitView/Form.aspx?EntityContext={0}&Action=Edit&idTimeUnit={1}&From=TimeUnitPreview", EntityContext, _idTimeUnit));
                        else if (EntityContext == EntityContext.TimeUnitMy && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/TimeUnitView/Form.aspx?EntityContext={0}&Action=Edit&idTimeUnit={1}&From=TimeUnitMyPreview", EntityContext, _idTimeUnit));
                        Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}", EntityContext.TimeUnit));
                    }
                    break;

                case ContextMenuEventTypes.SaveAs:
                    {
                        if (EntityContext == EntityContext.TimeUnit && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/TimeUnitView/Form.aspx?EntityContext={0}&Action=SaveAs&idTimeUnit={1}&From=TimeUnitPreview", EntityContext, _idTimeUnit));
                        else if (EntityContext == EntityContext.TimeUnitMy && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/TimeUnitView/Form.aspx?EntityContext={0}&Action=SaveAs&idTimeUnit={1}&From=TimeUnitMyPreview", EntityContext, _idTimeUnit));
                        Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}", EntityContext.TimeUnit));
                    }
                    break;
            }
        }

        #endregion

        #region Delete

        private void btnDelete_Click(object sender, EventArgs eventArgs)
        {
            mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_idTimeUnit);
        }

        #endregion

        #endregion

        #region Support methods

        private void GenerateContexMenuItems()
        {
            var contextMenu = new[]
            {
                new ContextMenuItem(ContextMenuEventTypes.Back, "Back"), 
                new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit"), 
                new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As")
            };

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contextMenu);

        }

        private void GenerateTabMenuItems()
        {
            tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, MasterPage.CurrentLocation);
            tabMenu.SelectItem(MasterPage.CurrentLocation, Support.CacheManager.Instance.AppLocations);
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            if (!base.SecurityPageSpecific())
            {
                if (SecurityHelper.IsPermitted(Permission.SaveAsTimeUnit)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });

                if (SecurityHelper.IsPermitted(Permission.EditTimeUnit)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit") });
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit") });

                if (SecurityHelper.IsPermitted(Permission.DeleteTimeUnit)) StyleHelper.EnableLinkButtonsWithCssClass(PnlFooter, "Delete");
                else StyleHelper.DisableLinkButtonsWithCssClass(PnlFooter, "Delete");

                SecurityPageSpecificMy(_isResponsibleUser);
            }

            return true;
        }

        #endregion
    }
}