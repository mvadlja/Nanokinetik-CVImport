using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using AspNetUIFramework;
using CommonComponents;
using EVMessage.Xevprm;
using GEM2Common;
using PossGrid;
using Ready.Model;
using eudravigilance.ema.europa.eu.schema.emaxevmpd;

namespace AspNetUI.Views.ProductXevprmView
{
    public partial class List : ListPage
    {
        #region Declarations
        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;
        private int? _idProd;
        private string _gridId;

        private IProduct_PKOperations _productOperations;
        private IAuthorisedProductOperations _authorisedProductOperations;
        private IXevprm_message_PKOperations _xevprmMessageOperations;
        private IXevprm_entity_details_mn_PKOperations _xevprmEntityDetailsMnOperations;
        private IXevprm_ap_details_PKOperations _xevprmApDetailsOperations;
        private IXevprm_log_PKOperations _logOperations;
        private IRecieved_message_PKOperations _receivedMessageOperations;

        private IUser_grid_settings_PKOperations _userGridSettingsOperations;
        private IUSEROperations _userOperations;

        #endregion

        #region Properties

        private int? _xevprmPkToDelete
        {
            get { return (int?)ViewState["_xevprmPkToDelete"]; }
            set { ViewState["_xevprmPkToDelete"] = value; }
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

            BindGridDynamicControls();

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

            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;

            _productOperations = new Product_PKDAL();
            _authorisedProductOperations = new AuthorisedProductDAL();
            _xevprmMessageOperations = new Xevprm_message_PKDAL();
            _xevprmEntityDetailsMnOperations = new Xevprm_entity_details_mn_PKDAL();
            _xevprmApDetailsOperations = new Xevprm_ap_details_PKDAL();
            _logOperations = new Xevprm_log_PKDAL();
            _userGridSettingsOperations = new User_grid_settings_PKDAL();
            _userOperations = new USERDAL();
            _receivedMessageOperations = new Recieved_message_PKDAL();

            if (ListType == ListType.Search)
            {
                ProductXevprmGrid.GridVersion = ProductXevprmGrid.GridVersion + ListType.ToString();
            }

            _gridId = ProductXevprmGrid.GridId + "_" + ProductXevprmGrid.GridVersion;
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
                    btnSaveLayout.Click += btnSaveLayout_Click;
                    btnClearLayout.Click += btnClearLayout_Click;
                    btnExport.Click += btnExport_Click;
                    btnColumns.Click += btnColumns_OnClick;
                    ColumnsPopup.OnOkButtonClick += ColumnsPopup_OnOkButtonClick;
                    btnRefresh.Click += btnRefresh_Click;
                    btnReset.Click += btnReset_Click;
                    break;
            }

            ProductXevprmGrid.OnRebindRequired += ProductXevprmGridOnRebindRequired;
            ProductXevprmGrid.OnHtmlRowPrepared += ProductXevprmGridOnHtmlRowPrepared;
            ProductXevprmGrid.OnHtmlCellPrepared += ProductXevprmGridOnHtmlCellPrepared;
            ProductXevprmGrid.OnExcelCellPrepared += ProductXevprmGridOnExcelCellPrepared;
            ProductXevprmGrid.OnLoadClientLayout += ProductXevprmGridOnLoadClientLayout;

            mpDelete.OnYesButtonClick += mpDelete_OnYesButtonClick;
            XevprmValidationErrorPopup.OnValidationSuccessful += XevprmValidationErrorPopup_OnValidationSuccessful;
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

                    lblPrvParentEntity.Visible = true;
                    lblPrvParentEntity.Label = "Product:";

                    var product = _productOperations.GetEntity(_idProd);

                    lblPrvParentEntity.Text = product != null && !string.IsNullOrWhiteSpace(product.name) ? product.name : Constant.ControlDefault.LbPrvText;
                    break;
            }

            BindDynamicControls(null);
        }

        void FillXevprmStatus(object arg)
        {
            // Fill xevprm status combo box
            var xevprmStatusList = new List<string>()
            {
                "", "Pending", "Validation failed", "Validation OK", "In progress", "Submission failed", 
                "Submission aborted", "Successful", "Errors", "Failed"
            };
            var xevprmStatusListItems = new List<ListItem>(xevprmStatusList.Count);
            xevprmStatusList.ForEach(item => xevprmStatusListItems.Add(new ListItem(item, item)));
            ProductXevprmGrid.SetComboList("XevprmStatus", xevprmStatusListItems);
        }

        void FillOperation(object arg)
        {
            // Fill operation combo box
            var operationList = new List<string>() { "", "Insert", "Update", "Variation", "Nullify", "Withdraw" };
            var operationListItems = new List<ListItem>(operationList.Count);
            operationList.ForEach(item => operationListItems.Add(new ListItem(item, item)));
            ProductXevprmGrid.SetComboList("Operation", operationListItems);
        }

        void FillSubmissionStatus(object arg)
        {
            // Fill submission status combo box
            var submissionStatusList = new List<string>()
            {
                "", "Pending", "In progress", "Submission error", "Submission aborted", "MDN pending", "MDN error", 
                "ACK pending", "ACK01 received", "ACK02 received", "ACK03 received", "ACK01 delivery failed", 
                "ACK02 delivery failed", "ACK03 delivery failed", "ACK01 delivered", "ACK02 delivered", "ACK03 delivered"
            };
            var submissionStatusListItems = new List<ListItem>(submissionStatusList.Count);
            submissionStatusList.ForEach(item => submissionStatusListItems.Add(new ListItem(item, item)));
            ProductXevprmGrid.SetComboList("GatewaySubmissionStatus", submissionStatusListItems);
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            var userGridSettings = _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            if (userGridSettings != null && !String.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                ProductXevprmGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
            }
        }

        private void BindXevprmOperationTypeButtons()
        {
            var authProdWithoutXevprmPkList = _authorisedProductOperations.GetA57RelEntityIDsWithoutXevprmByProduct(_idProd);

            btnXevprmInsert.EmbedPermissions(Permission.XevprmInsert);

            btnXevprmInsert.Enabled = btnXevprmInsert.Enabled && authProdWithoutXevprmPkList.Count > 0;

            const string enabledCssClass = "xevprm-operationtype-button-enabled";
            const string disabledCssClass = "xevprm-operationtype-button-disabled";

            btnXevprmInsert.CssClass = authProdWithoutXevprmPkList.Count > 0 ? enabledCssClass : disabledCssClass;
        }

        private void BindGrid()
        {
            BindXevprmOperationTypeButtons();

            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (ProductXevprmGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(ProductXevprmGrid.SecondSortingColumn, ProductXevprmGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (ProductXevprmGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(ProductXevprmGrid.MainSortingColumn, ProductXevprmGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("xevprm_message_PK", GEMOrderByType.DESC));

            var itemCount = 0;

            var ds = _xevprmMessageOperations.GetListFormDataSet(filters, ProductXevprmGrid.CurrentPage, ProductXevprmGrid.PageSize, gobList, out itemCount);

            ProductXevprmGrid.TotalRecords = itemCount;

            if (ProductXevprmGrid.CurrentPage > ProductXevprmGrid.TotalPages || (ProductXevprmGrid.CurrentPage == 0 && ProductXevprmGrid.TotalPages > 0))
            {
                if (ProductXevprmGrid.CurrentPage > ProductXevprmGrid.TotalPages) ProductXevprmGrid.CurrentPage = ProductXevprmGrid.TotalPages; else ProductXevprmGrid.CurrentPage = 1;

                ds = _xevprmMessageOperations.GetListFormDataSet(filters, ProductXevprmGrid.CurrentPage, ProductXevprmGrid.PageSize, gobList, out itemCount);
            }

            ProductXevprmGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            ProductXevprmGrid.DataBind();
        }


        private void BindGridDynamicControls()
        {
            FillXevprmStatus(null);
            FillOperation(null);
            FillSubmissionStatus(null);
        }

        private void BindDynamicControls(object args)
        {

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
                var xevprm = _xevprmMessageOperations.GetEntity(entityPk);
                xevprm.deleted = true;
                _xevprmMessageOperations.Save(xevprm);

                Log.Xevprm.LogEvent("Deleted.", xevprm.xevprm_message_PK, xevprm.XevprmStatus, Thread.CurrentPrincipal.Identity.Name);

                Response.Redirect(string.Format("~/Views/ProductXevprmView/List.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, _idProd));
            }

            MasterPage.ModalPopup.ShowModalPopup("Error!", "Could not delete entity! Contact your system administrator.<br/><br/>");
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
                    Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext.Product));
                    break;
            }
        }

        #endregion

        #region Delete

        public void btnDeleteEntity_OnClick(object sender, EventArgs e)
        {
            var deleteButton = sender as ImageButton;
            if (deleteButton == null) return;

            var commandNameString = Convert.ToString(deleteButton.CommandName);
            var commandArgumentString = Convert.ToString(deleteButton.CommandArgument);

            if (commandNameString == Constant.CommandArgument.Delete)
            {
                _xevprmPkToDelete = ValidationHelper.IsValidInt(commandArgumentString) ? int.Parse(commandArgumentString) : (int?)null;
                if (_xevprmPkToDelete.HasValue) mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
            }
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_xevprmPkToDelete);
        }

        #endregion

        #region Grid

        void ProductXevprmGridOnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {
            string applicationURL = ConfigurationManager.AppSettings["ApplicationURL"];
            string applicationURLSecure = ConfigurationManager.AppSettings["ApplicationURLSecure"];
            string appVirtualPath = ConfigurationManager.AppSettings["AppVirtualPath"];

            string URL = Request.Url.Authority; // default value if none of config section is present
            string scheme = Request.Url.Scheme;

            if (scheme == "http") URL = applicationURL;
            if (scheme == "https") URL = applicationURLSecure;

            string applicationURLWithScheme = scheme + "://" + URL + appVirtualPath;

            if (args.RowType == DataControlRowType.DataRow)
            {
                bool applyStyle = true;
                switch (args.FieldName)
                {
                    case "AuthorisedProduct":
                        args.Cell.Url = string.Format("{0}/Views/AuthorisedProductView/Preview.aspx?EntityContext={1}&idAuthProd={2}", applicationURLWithScheme, EntityContext.AuthorisedProduct, Convert.ToString(args.Row["ap_FK"]));
                        break;
                    case "Product":
                        args.Cell.Url = string.Format("{0}/Views/ProductView/Preview.aspx?EntityContext={1}&idProd={2}", applicationURLWithScheme, EntityContext.Product, Convert.ToString(args.Row["product_FK"]));
                        break;
                    case "XevprmXml":
                        if (args.Cell.Text.Contains("XML"))
                        {
                            args.Cell.Url = applicationURLWithScheme + "/GetXML.aspx?id=" + Convert.ToString(args.Row["xevprm_message_PK"]);
                            args.Cell.Text = "XML";
                        }
                        break;
                    case "XevprmXmlPdf":
                        if (args.Cell.Text.Contains("PDF"))
                        {
                            args.Cell.Url = applicationURLWithScheme + "/GetPDF.aspx?id=" + Convert.ToString(args.Row["xevprm_message_PK"]);
                        }
                        break;
                    case "XevprmXmlRtf":
                        if (args.Cell.Text.Contains("RTF"))
                        {
                            args.Cell.Url = applicationURLWithScheme + "/GetRTF.aspx?id=" + Convert.ToString(args.Row["xevprm_message_PK"]);
                        }
                        break;
                    case "AckXml":
                        if (args.Cell.Text.Contains("XML"))
                        {
                            args.Cell.Url = applicationURLWithScheme + "/GetACK.aspx?id=" + Convert.ToString(args.Row["xevprm_message_PK"]);
                            args.Cell.Text = args.Cell.Text.Contains("XML") ? "0" + Convert.ToString(args.Row["ack_type"]) + " XML" : args.Cell.Text;
                        }
                        break;
                    case "AckXmlPdf":
                        if (args.Cell.Text.Contains("PDF"))
                        {
                            args.Cell.Url = applicationURLWithScheme + "/GetPDFACK.aspx?id=" + Convert.ToString(args.Row["xevprm_message_PK"]);
                        }
                        break;
                    case "AckXmlRtf":
                        if (args.Cell.Text.Contains("RTF"))
                        {
                            args.Cell.Url = applicationURLWithScheme + "/GetRTFACK.aspx?id=" + Convert.ToString(args.Row["xevprm_message_PK"]);
                        }
                        break;
                    default:
                        applyStyle = false;
                        break;
                }

                if (applyStyle)
                {
                    args.Cell.FontColor = Color.Blue;
                    args.Cell.FontFamily = "Times New Roman";
                    args.Cell.FontSize = 9;
                    args.Cell.FontUnderline = true;
                }
            }
            else if (args.RowType == DataControlRowType.Header && args.FieldValue != null)
            {
                switch (args.FieldName)
                {
                    case "XevprmXml":
                        args.Cell.Text = "MSG XML";
                        break;
                    case "XevprmXmlPdf":
                        args.Cell.Text = "MSG PDF";
                        args.Column.Width = "5%";
                        break;
                    case "XevprmXmlRtf":
                        args.Cell.Text = "MSG RTF";
                        args.Column.Width = "5%";
                        break;
                    case "AckXml":
                        args.Cell.Text = "ACK XML";
                        break;
                    case "AckXmlPdf":
                        args.Cell.Text = "ACK PDF";
                        args.Column.Width = "5%";
                        break;
                    case "AckXmlRtf":
                        args.Cell.Text = "ACK RTF";
                        args.Column.Width = "5%";
                        break;
                }
            }
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in ProductXevprmGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("xevprm_message_PK", "XevprmXmlPdf", "XevprmXmlRtf", "AckXmlPdf", "AckXmlRtf", "SubmittedBy"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (ProductXevprmGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
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
            ProductXevprmGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();

            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            ProductXevprmGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);

            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = ProductXevprmGrid.GetClientLayoutString(),
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

            filters.Add("Export", "true");

            var gobList = new List<GEMOrderBy>();
            if (ProductXevprmGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(ProductXevprmGrid.SecondSortingColumn, ProductXevprmGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (ProductXevprmGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(ProductXevprmGrid.MainSortingColumn, ProductXevprmGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("xevprm_message_PK", GEMOrderByType.DESC));

            int itemCount = 0;
            var ds = _xevprmMessageOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);

            ProductXevprmGrid["XevprmXmlPdf"].Visible = true;
            ProductXevprmGrid["XevprmXmlRtf"].Visible = true;
            ProductXevprmGrid["AckXmlPdf"].Visible = true;
            ProductXevprmGrid["AckXmlRtf"].Visible = true;
            ProductXevprmGrid["SubmittedBy"].Visible = true;
            ProductXevprmGrid["Delete"].Visible = false;

            if (ds != null) ProductXevprmGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));

            ProductXevprmGrid["XevprmXmlPdf"].Visible = false;
            ProductXevprmGrid["XevprmXmlRtf"].Visible = false;
            ProductXevprmGrid["AckXmlPdf"].Visible = false;
            ProductXevprmGrid["AckXmlRtf"].Visible = false;
            ProductXevprmGrid["SubmittedBy"].Visible = false;
            ProductXevprmGrid["Delete"].Visible = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            ProductXevprmGrid.ResetVisibleColumns();
            ProductXevprmGrid.SecondSortingColumn = null;

            BindGrid();
        }

        void ProductXevprmGridOnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            int xevprmMessagePk = Convert.ToInt32(e.GetValue("xevprm_message_PK"));
            var xevprmStatus = (XevprmStatus)e.GetValue("message_status_FK");

            //Xevprm XML links
            if (xevprmStatus.NotIn(XevprmStatus.Created, XevprmStatus.ValidationFailed) &&
                !string.IsNullOrWhiteSpace(Convert.ToString(e.GetValue("XevprmXml"))))
            {
                var hlXml = e.FindControl("hlXML") as HyperLink;
                if (hlXml != null)
                {
                    hlXml.Visible = true;
                    hlXml.NavigateUrl = "~/GetXML.aspx?id=" + xevprmMessagePk;
                    hlXml.Target = "_blank";
                    hlXml.EmbedPermissions(Permission.XevprmShowXmlFile);
                }

                var hlPdf = e.FindControl("hlPDF") as HyperLink;
                if (hlPdf != null)
                {
                    hlPdf.Visible = true;
                    hlPdf.NavigateUrl = "~/GetPDF.aspx?id=" + xevprmMessagePk;
                    hlPdf.Target = "_blank";
                    hlPdf.EmbedPermissions(Permission.XevprmShowPdfFile);
                }

                var hlRtf = e.FindControl("hlRTF") as HyperLink;
                if (hlRtf != null)
                {
                    hlRtf.Visible = true;
                    hlRtf.NavigateUrl = "~/GetRTF.aspx?id=" + xevprmMessagePk;
                    hlRtf.Target = "_blank";
                    hlRtf.EmbedPermissions(Permission.XevprmShowRtfFile);
                }
            }

            string ackType = Convert.ToString(e.GetValue("ack_type"));

            //Xevprm ACK links
            if (xevprmStatus.In(XevprmStatus.ACKReceived, XevprmStatus.SubmittingMDN, XevprmStatus.ACKDeliveryFailed, XevprmStatus.ACKDelivered) &&
                !string.IsNullOrWhiteSpace(Convert.ToString(e.GetValue("AckXml"))) && !string.IsNullOrWhiteSpace(ackType))
            {
                var hlAck = e.FindControl("hlACK") as HyperLink;
                if (hlAck != null)
                {
                    hlAck.Visible = true;
                    hlAck.Text = string.Format("0{0} XML", ackType);
                    hlAck.NavigateUrl = "~/GetACK.aspx?id=" + xevprmMessagePk;
                    hlAck.Target = "_blank";
                    hlAck.EmbedPermissions(Permission.XevprmShowXmlFile);
                }

                var hlAckPdf = e.FindControl("hlACK_PDF") as HyperLink;
                if (hlAckPdf != null)
                {
                    hlAckPdf.Visible = true;
                    hlAckPdf.Text = string.Format("0{0} PDF", ackType);
                    hlAckPdf.NavigateUrl = "~/GetPDFACK.aspx?id=" + xevprmMessagePk;
                    hlAckPdf.Target = "_blank";
                    hlAckPdf.EmbedPermissions(Permission.XevprmShowPdfFile);
                }

                var hlAckRtf = e.FindControl("hlACK_RTF") as HyperLink;
                if (hlAckRtf != null)
                {
                    hlAckRtf.Visible = true;
                    hlAckRtf.Text = string.Format("0{0} RTF", ackType);
                    hlAckRtf.NavigateUrl = "~/GetRTFACK.aspx?id=" + xevprmMessagePk;
                    hlAckRtf.Target = "_blank";
                    hlAckRtf.EmbedPermissions(Permission.XevprmShowRtfFile);
                }
            }

            //Xevprm available actions and coloring
            var btnStatus = e.FindControl("btnStatus") as LinkButton;
            var lblStatus = e.FindControl("lblStatus") as Label;
            var btnGatewaySubmissionError = e.FindControl("btnGatewaySubmissionError") as LinkButton;
            var lblGatewaySubmissionStatus = e.FindControl("lblGatewaySubmissionStatus") as Label;
            var lblOperation = e.FindControl("lblOperation") as Label;
            var btnAction = e.FindControl("btnAction") as LinkButton;

            if (btnStatus == null || lblStatus == null || btnGatewaySubmissionError == null || lblGatewaySubmissionStatus == null || lblOperation == null || btnAction == null) return;

            string availableAction = XevprmHelper.AvailableGridActionForXevprmStatus(xevprmStatus);

            if (Convert.ToString(e.GetValue("XevprmNum")) == "1" && !string.IsNullOrWhiteSpace(availableAction) && availableAction != "-")
            {
                btnAction.Text = availableAction;
                btnAction.Visible = true;
            }

            switch (xevprmStatus)
            {
                case XevprmStatus.Created:
                    lblGatewaySubmissionStatus.Visible = false;
                    break;
                case XevprmStatus.ValidationFailed:
                    lblStatus.Visible = false;
                    btnStatus.Visible = true;
                    lblGatewaySubmissionStatus.Visible = false;
                    btnStatus.ForeColor = Color.Red;
                    lblOperation.ForeColor = Color.Red;
                    break;
                case XevprmStatus.ValidationSuccessful:
                    lblStatus.ForeColor = Color.Blue;
                    lblOperation.ForeColor = Color.Blue;
                    btnAction.EmbedPermissions(Permission.XevprmSubmit);
                    break;
                case XevprmStatus.ReadyToSubmit:
                    lblStatus.ForeColor = Color.DarkBlue;
                    lblOperation.ForeColor = Color.DarkBlue;
                    lblGatewaySubmissionStatus.ForeColor = Color.DarkBlue;
                    btnAction.EmbedPermissions(Permission.XevprmAbort);
                    break;
                case XevprmStatus.SubmittingMessage:
                    lblStatus.ForeColor = Color.DarkBlue;
                    lblOperation.ForeColor = Color.DarkBlue;
                    lblGatewaySubmissionStatus.ForeColor = Color.DarkBlue;
                    break;
                case XevprmStatus.SubmissionFailed:
                    lblGatewaySubmissionStatus.Visible = false;
                    btnGatewaySubmissionError.Visible = true;
                    lblStatus.ForeColor = Color.Red;
                    lblOperation.ForeColor = Color.Red;
                    btnGatewaySubmissionError.ForeColor = Color.Red;
                    btnAction.EmbedPermissions(Permission.XevprmResubmit);
                    break;
                case XevprmStatus.SubmissionAborted:
                    lblStatus.ForeColor = Color.Red;
                    lblOperation.ForeColor = Color.Red;
                    lblGatewaySubmissionStatus.ForeColor = Color.Red;
                    btnAction.EmbedPermissions(Permission.XevprmResubmit);
                    break;
                case XevprmStatus.MDNPending:
                    lblStatus.ForeColor = Color.DarkBlue;
                    lblOperation.ForeColor = Color.DarkBlue;
                    lblGatewaySubmissionStatus.ForeColor = Color.DarkBlue;
                    btnAction.EmbedPermissions(Permission.XevprmAbort);
                    break;
                case XevprmStatus.MDNReceivedError:
                    lblGatewaySubmissionStatus.Visible = false;
                    btnGatewaySubmissionError.Visible = true;
                    lblStatus.ForeColor = Color.Red;
                    lblOperation.ForeColor = Color.Red;
                    btnGatewaySubmissionError.ForeColor = Color.Red;
                    btnAction.EmbedPermissions(Permission.XevprmResubmit);
                    break;
                case XevprmStatus.MDNReceivedSuccessful:
                    lblStatus.ForeColor = Color.DarkBlue;
                    lblOperation.ForeColor = Color.DarkBlue;
                    lblGatewaySubmissionStatus.ForeColor = Color.DarkBlue;
                    btnAction.EmbedPermissions(Permission.XevprmAbort);
                    break;
                case XevprmStatus.ACKReceived:
                    lblStatus.ForeColor = Color.DarkBlue;
                    lblOperation.ForeColor = Color.DarkBlue;
                    lblGatewaySubmissionStatus.ForeColor = Color.DarkBlue;
                    break;
                case XevprmStatus.SubmittingMDN:
                    lblStatus.ForeColor = Color.DarkBlue;
                    lblOperation.ForeColor = Color.DarkBlue;
                    lblGatewaySubmissionStatus.ForeColor = Color.DarkBlue;
                    break;
                case XevprmStatus.ACKDeliveryFailed:
                    lblGatewaySubmissionStatus.Visible = false;
                    btnGatewaySubmissionError.Visible = true;
                    lblStatus.ForeColor = Color.Red;
                    lblOperation.ForeColor = Color.Red;
                    btnGatewaySubmissionError.ForeColor = Color.Red;
                    btnAction.EmbedPermissions(Permission.XevprmResubmit);
                    break;
                case XevprmStatus.ACKDelivered:
                    var color = ackType == "1" ? Color.Green : ackType == "2" ? Color.Purple : Color.Red;
                    lblStatus.ForeColor = color;
                    lblOperation.ForeColor = color;
                    lblGatewaySubmissionStatus.ForeColor = color;
                    break;
            }

            DataControlFieldCell deleteTemplate = e.Row.Controls.OfType<DataControlFieldCell>().FirstOrDefault(x => x.ContainingField.SortExpression == "Delete");

            if (deleteTemplate != null)
            {
                var btnDelete = deleteTemplate.Controls.OfType<ImageButton>().FirstOrDefault(x => x.CommandName == "Delete");

                if (btnDelete != null)
                {
                    //Delete xevprm
                    if (SecurityHelper.IsPermitted(Permission.XevprmDelete))
                    {
                        if (xevprmStatus.NotIn(XevprmStatus.ReadyToSubmit, XevprmStatus.SubmittingMessage, XevprmStatus.MDNPending, XevprmStatus.MDNReceivedSuccessful,
                                               XevprmStatus.ACKReceived, XevprmStatus.SubmittingMDN, XevprmStatus.ACKDeliveryFailed) && xevprmStatus == XevprmStatus.ACKDelivered && ackType != "1")
                            btnDelete.Visible = true;
                    }

                    if (SecurityHelper.IsPermitted(Permission.XevprmAdminDelete))
                    {
                        if (xevprmStatus.NotIn(XevprmStatus.ReadyToSubmit, XevprmStatus.SubmittingMessage, XevprmStatus.SubmittingMDN))
                            btnDelete.Visible = true;
                    }
                }
            }

            //color coding 
            string colorStatus = e.GetValue("AuthorisationStatus").ToString();
            var pnlStatusColor = e.FindControl("pnlStatusColor") as HtmlGenericControl;

            if (pnlStatusColor != null)
            {
                if (colorStatus == "Valid" || colorStatus == "Valid (after renewal)")
                {
                    // green
                    pnlStatusColor.Attributes.Add("class", "statusGreen");
                }
                else if (colorStatus == "Pending" || colorStatus == "Planned")
                {
                    // yellow
                    pnlStatusColor.Attributes.Add("class", "statusYellow");
                }
                else
                {
                    // black
                    pnlStatusColor.Attributes.Add("class", "statusBlack");
                }
            }
        }

        void ProductXevprmGridOnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void ProductXevprmGridOnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!ProductXevprmGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = ProductXevprmGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (ProductXevprmGrid.SortCount > 1 && e.FieldName == ProductXevprmGrid.MainSortingColumn)
                return;

            e.Cell.CssClass = _flip ? "sorted_column_even" : "sorted_column_odd";
        }

        void ProductXevprmGridOnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
        {
            var userGridSettings = MasterPage != null ? _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, MasterPage.CurrentLocation.display_name) : null;
            if (userGridSettings != null && !string.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                args.ClientLayoutString = userGridSettings.grid_layout;
            }
        }

        #endregion

        #region Xevprm operation type

        protected void btnXevprmInsert_OnClick(object sender, EventArgs e)
        {
            ConstructXevprm(XevprmOperationType.Insert);
        }

        #endregion

        void XevprmValidationErrorPopup_OnValidationSuccessful(object sender, FormEventArgs<int> e)
        {
            int xevprmMessagePk = e.Data;

            var xevprmMessage = _xevprmMessageOperations.GetEntity(xevprmMessagePk);

            var operationResult = XevprmXml.ConstructXevprmXml(xevprmMessagePk);

            if (operationResult.IsSuccess)
            {
                xevprmMessage.XevprmStatus = XevprmStatus.ValidationSuccessful;
                xevprmMessage.xml = operationResult.Result;
                xevprmMessage.xml_hash = XevprmHelper.ComputeHash(xevprmMessage.xml);
                xevprmMessage = _xevprmMessageOperations.Save(xevprmMessage);

                var updateResult = Xevprm.UpdateXevprmEntityDetailsTables(xevprmMessage);

                if (updateResult.IsSuccess)
                {
                    BindGrid();
                }
                else
                {
                    HandleXevprmMessageOperationResultError(updateResult, xevprmMessagePk);
                }
            }
            else
            {
                HandleXevprmMessageOperationResultError(operationResult, xevprmMessagePk);
            }
        }

        void btnRefresh_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        public void btnStatus_OnClick(object sender, EventArgs e)
        {
            int xevprmMessagePk;

            if (sender is LinkButton && int.TryParse((sender as LinkButton).CommandArgument, out xevprmMessagePk))
            {
                XevprmValidationErrorPopup.ShowModalForm(xevprmMessagePk);
            }
        }

        public void btnStatusError_OnClick(object sender, EventArgs e)
        {
            int xevprmMessagePk;

            if (sender is LinkButton && int.TryParse((sender as LinkButton).CommandArgument, out xevprmMessagePk))
            {
                var xevprmMessage = _xevprmMessageOperations.GetEntity(xevprmMessagePk);

                if (xevprmMessage == null) return;

                string submissionError = null;

                if (xevprmMessage.XevprmStatus.In(XevprmStatus.SubmissionFailed, XevprmStatus.ACKDeliveryFailed))
                {
                    submissionError = _logOperations.GetMessageSubmissionError(xevprmMessagePk, xevprmMessage.XevprmStatus == XevprmStatus.SubmissionFailed ? "Xevprm" : "MDN");
                }
                else if (xevprmMessage.XevprmStatus.In(XevprmStatus.MDNReceivedError))
                {
                    var receivedMessage = _receivedMessageOperations.GetEntity(xevprmMessage.received_message_FK);

                    if (receivedMessage != null)
                    {
                        submissionError = receivedMessage.processing_error;
                    }
                }

                if (string.IsNullOrWhiteSpace(submissionError))
                {
                    submissionError = "Unknown error.<br/>Please contact your system administrator.";
                }

                var description = string.Format("<div style='text-align:center'>{0}<br/><br/></div>", submissionError);

                MasterPage.ModalPopup.ShowModalPopup("Submission error!", description);
            }
        }

        protected void btnAction_OnClick(object sender, EventArgs e)
        {
            int xevprmMessagePk;

            if (!(sender is LinkButton && int.TryParse((sender as LinkButton).CommandArgument, out xevprmMessagePk))) return;

            var xevprmMessage = _xevprmMessageOperations.GetEntity(xevprmMessagePk);

            if (xevprmMessage == null) return;

            //Action check
            if ((sender as LinkButton).CommandName != Convert.ToString(xevprmMessage.message_status_FK))
            {
                MasterPage.ModalPopup.ShowModalPopup("Warning!", "<div style='text-align:center'>xEVPRM status has changed since page was loaded!<br/>Press OK to refresh page.<br/><br/></div>");
                return;
            }

            switch (xevprmMessage.XevprmStatus)
            {
                //Validate
                case XevprmStatus.Created:
                    {
                        var operationResult = XevprmXml.ConstructXevprmXml(xevprmMessagePk);

                        if (operationResult.IsSuccess)
                        {
                            xevprmMessage.XevprmStatus = XevprmStatus.ValidationSuccessful;
                            xevprmMessage.xml = operationResult.Result;
                            xevprmMessage.xml_hash = XevprmHelper.ComputeHash(xevprmMessage.xml);
                            xevprmMessage = _xevprmMessageOperations.Save(xevprmMessage);

                            Xevprm.UpdateXevprmEntityDetailsTables(xevprmMessage);

                            Log.Xevprm.LogEvent("Validated successfully.", xevprmMessagePk, xevprmMessage.XevprmStatus);

                            BindGrid();
                        }
                        else if (operationResult.Exception == null)
                        {
                            xevprmMessage.XevprmStatus = XevprmStatus.ValidationFailed;
                            xevprmMessage = _xevprmMessageOperations.Save(xevprmMessage);

                            Log.Xevprm.LogEvent("Validation failed.", xevprmMessagePk, xevprmMessage.XevprmStatus);

                            BindGrid();

                            XevprmValidationErrorPopup.ShowModalForm(xevprmMessagePk);
                        }
                        else
                        {
                            HandleXevprmMessageOperationResultError(operationResult, xevprmMessagePk, xevprmMessage.XevprmStatus);
                        }
                    }
                    break;

                //Validate
                case XevprmStatus.ValidationFailed:
                    {
                        var operationResult = XevprmXml.ConstructXevprmXml(xevprmMessagePk);

                        if (operationResult.IsSuccess)
                        {
                            xevprmMessage.XevprmStatus = XevprmStatus.ValidationSuccessful;
                            xevprmMessage.xml = operationResult.Result;
                            xevprmMessage.xml_hash = XevprmHelper.ComputeHash(xevprmMessage.xml);
                            xevprmMessage = _xevprmMessageOperations.Save(xevprmMessage);

                            Xevprm.UpdateXevprmEntityDetailsTables(xevprmMessage);

                            Log.Xevprm.LogEvent("Validated successfully.", xevprmMessagePk, xevprmMessage.XevprmStatus);

                            BindGrid();
                        }
                        else if (operationResult.Exception == null)
                        {
                            XevprmValidationErrorPopup.ShowModalForm(xevprmMessagePk);
                        }
                        else
                        {
                            HandleXevprmMessageOperationResultError(operationResult, xevprmMessagePk, xevprmMessage.XevprmStatus);
                        }
                    }
                    break;

                //Submit
                case XevprmStatus.ValidationSuccessful:
                    {
                        var operationResult = XevprmXml.ConstructXevprmXml(xevprmMessagePk);

                        if (operationResult.IsSuccess && xevprmMessage.xml_hash == XevprmHelper.ComputeHash(operationResult.Result))
                        {
                            xevprmMessage.XevprmStatus = XevprmStatus.ReadyToSubmit;
                            xevprmMessage.gateway_submission_date = DateTime.Now;

                            var user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
                            xevprmMessage.submitted_FK = user != null ? user.User_PK : null;

                            xevprmMessage = _xevprmMessageOperations.Save(xevprmMessage);

                            Log.Xevprm.LogEvent("Ready for submission.", xevprmMessagePk, xevprmMessage.XevprmStatus);

                            BindGrid();
                        }
                        else if (operationResult.Exception == null)
                        {
                            xevprmMessage.XevprmStatus = XevprmStatus.Created;
                            xevprmMessage = _xevprmMessageOperations.Save(xevprmMessage);

                            Log.Xevprm.LogEvent("Couldn't be submitted due to product information changes since last validation.", xevprmMessagePk, xevprmMessage.XevprmStatus);

                            BindGrid();

                            MasterPage.ModalPopup.ShowModalPopup("Error", "<div style='text-align:center'>Changes to product information have been made since last validation!<br/>Please validate the message again.<br/><br/></div>");
                        }
                        else
                        {
                            HandleXevprmMessageOperationResultError(operationResult, xevprmMessagePk, xevprmMessage.XevprmStatus);
                        }
                    }
                    break;

                //Abort
                case XevprmStatus.ReadyToSubmit:
                case XevprmStatus.MDNPending:
                case XevprmStatus.MDNReceivedSuccessful:
                    {
                        xevprmMessage.XevprmStatus = XevprmStatus.SubmissionAborted;
                        xevprmMessage = _xevprmMessageOperations.Save(xevprmMessage);

                        Log.Xevprm.LogEvent("Aborted.", xevprmMessagePk, xevprmMessage.XevprmStatus);

                        BindGrid();
                    }
                    break;

                //Resubmit
                case XevprmStatus.SubmissionAborted:
                case XevprmStatus.SubmissionFailed:
                case XevprmStatus.MDNReceivedError:
                    {
                        try
                        {
                            var xevprmEntityDetailsMnList = _xevprmEntityDetailsMnOperations.GetEntitiesByXevprm(xevprmMessagePk);
                            int? idAuthProd = xevprmEntityDetailsMnList.Where(x => x.XevprmEntityType == XevprmEntityType.AuthorisedProduct).Select(x => x.xevprm_entity_FK).FirstOrDefault();

                            string messageNumber = idAuthProd.HasValue ? _xevprmMessageOperations.GetLatestMessageNumberByXevprmEntity(idAuthProd.Value, (int)XevprmEntityType.AuthorisedProduct) : null;
                            messageNumber = messageNumber != null ? Xevprm.GenerateMessageNumber(messageNumber.Split('_')[0], 7) : null;

                            if (string.IsNullOrWhiteSpace(messageNumber)) return;

                            xevprmMessage.message_number = messageNumber;

                            var xevprmXml = new XevprmXml();
                            var xRootNamespace = XRootNamespace.Parse(xevprmMessage.xml);
                            xevprmXml.Evprm = xRootNamespace.evprm;
                            xevprmXml.Evprm.ichicsrmessageheader.messagenumb = messageNumber;

                            xevprmMessage.xml = xevprmXml.Xml;

                            xevprmMessage.XevprmStatus = XevprmStatus.ReadyToSubmit;

                            xevprmMessage = _xevprmMessageOperations.Save(xevprmMessage);

                            Log.Xevprm.LogEvent("Ready for re-submission.", xevprmMessagePk, xevprmMessage.XevprmStatus);

                            BindGrid();
                        }
                        catch (Exception ex)
                        {
                            HandleXevprmMessageOperationResultError(new OperationResult<object>(ex, "Unexpected error at resubmiting xevprm message."), xevprmMessagePk, xevprmMessage.XevprmStatus);
                        }
                    }
                    break;

                //Resubmit MDN
                case XevprmStatus.ACKDeliveryFailed:
                    {
                        xevprmMessage.XevprmStatus = XevprmStatus.ACKReceived;
                        xevprmMessage = _xevprmMessageOperations.Save(xevprmMessage);

                        Log.Xevprm.LogEvent("Ready for MDN re-submission.", xevprmMessagePk, xevprmMessage.XevprmStatus);

                        BindGrid();
                    }
                    break;
            }
        }

        #endregion

        #region Support methods

        private void HandleXevprmMessageOperationResultError<T>(OperationResult<T> operationResult, int? xevprmMessagePk = null, XevprmStatus xevprmStatus = XevprmStatus.NULL)
        {
            string errorId = System.Guid.NewGuid().ToString();
            string message = string.Format("<div style='text-align:center'>Unexpected error occured!<br/>Please contact your system administrator.<br/><br/>Error ID: {0}</div>", errorId);
            MasterPage.ModalPopup.ShowModalPopup("Error!", message);
            Log.Xevprm.LogError(operationResult.Exception, string.Format("Error ({0}): {1}", errorId, operationResult.Description), xevprmMessagePk, xevprmStatus, Thread.CurrentPrincipal.Identity.Name);
        }

        private Dictionary<string, string> GetFilters()
        {
            var filters = ProductXevprmGrid.GetFilters();

            filters.Add("QueryBy", EntityContext.ToString());
            filters.Add("EntityPk", Convert.ToString(_idProd));

            return filters;
        }

        private void GenerateContextMenuItems()
        {
            if (ListType == ListType.List)
            {
                MasterPage.ContextMenu.SetContextMenuItemsVisible(new[] { new ContextMenuItem(ContextMenuEventTypes.Back, "Back") });
            }
        }

        private void GenerateTabMenuItems()
        {
            var location = Support.LocationManager.Instance.GetLocationByName("ProdXevprmList", Support.CacheManager.Instance.AppLocations);
            MasterPage.TabMenu.TabControls.Clear();
            tabMenu.Visible = true;
            if (location != null)
            {
                tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            var location = Support.LocationManager.Instance.GetLocationByName("Prod", Support.CacheManager.Instance.AppLocations);
            if (location != null)
            {
                var topLevelParent = MasterPage.FindTopLevelParent(location);

                MasterPage.CurrentLocation = location;
                MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
            }
        }

        private DataTable PrepareDataForExport(DataTable projectDataTable)
        {
            if (projectDataTable == null || projectDataTable.Rows.Count == 0) return projectDataTable;

            return projectDataTable;
        }

        public string HandleMissing(object value)
        {
            if (value != null && value.ToString().Trim() != "")
            {
                return value.ToString();
            }

            return "Missing";
        }

        private void ConstructXevprm(XevprmOperationType xevprmOperationType)
        {
            var authProdWithoutXevprmPkList = _authorisedProductOperations.GetA57RelEntityIDsWithoutXevprmByProduct(_idProd);

            using (var transactionScope = new TransactionScope())
            {
                foreach (int authorisedProductPk in authProdWithoutXevprmPkList)
                {
                    var user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);

                    if (user == null || !user.User_PK.HasValue) return;

                    string messageNumber = _xevprmMessageOperations.GetLatestMessageNumberByXevprmEntity(authorisedProductPk, (int)XevprmEntityType.AuthorisedProduct);
                    messageNumber = messageNumber != null ? Xevprm.GenerateMessageNumber(messageNumber.Split('_')[0], 7) : null;

                    var operationResult = Xevprm.CreateNewMessage(authorisedProductPk, XevprmEntityType.AuthorisedProduct, xevprmOperationType, user.User_PK.Value, messageNumber);

                    if (operationResult.IsSuccess && operationResult.Result != null)
                    {
                        Log.Xevprm.LogEvent("Successfully created.", operationResult.Result.xevprm_message_PK, XevprmStatus.Created);
                    }
                    else
                    {
                        HandleXevprmMessageOperationResultError(operationResult, null);
                        return;
                    }
                }

                transactionScope.Complete();
            }

            BindGrid();
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