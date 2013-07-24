using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Template;
using Microsoft.Reporting.WebForms;
using AspNetUIFramework;
using Ready.Model;

namespace AspNetUI.Views.Business
{
    public partial class PharmacovigilanceQualityReport : DefaultPage
    { 
        #region Declarations

        private ISubstance_PKOperations _substanceOperations;
        private IProduct_PKOperations _productOperations;
        private IActivity_PKOperations _activityOperations;
        private ITask_PKOperations _taskOperations;
        private ITask_name_PKOperations _taskNameOperations;
        private IPerson_PKOperations _personOperations;
        private IType_PKOperations _typeOperations;

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

            if (IsPostBack)
            {
                BindDynamicControls(null);
                return;
            }

            InitForm(null);

            SetFormControlsDefaults(null);
            SecurityPageSpecific();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            MasterPage.UpTopMenu.Update();
        }

        #endregion

        #region Form methods

        #region Initialize

        public override void LoadFormVariables()
        {
            base.LoadFormVariables();

            _substanceOperations = new Substance_PKDAL();
            _productOperations = new Product_PKDAL();
            _activityOperations = new Activity_PKDAL();
            _taskOperations = new Task_PKDAL();
            _taskNameOperations = new Task_name_PKDAL();
            _personOperations = new Person_PKDAL();
            _typeOperations = new Type_PKDAL();
        }

        private void BindEventHandlers()
        {
            btnCreateReport.Click += BtnCreateReportClick;
            btnCreateReportTest.Click += BtnCreateReportTestClick;
            txtSrActiveIngredient.Searcher.OnListItemSelected += ActiveIngredientSearcher_OnListItemSelected;
            txtSrProduct.Searcher.OnListItemSelected += ProductSearcher_OnListItemSelected;
            txtSrActivity.Searcher.OnListItemSelected += ActivitySearcher_OnListItemSelected;
            txtSrTask.Searcher.OnListItemSelected += TaskSearcher_OnListItemSelected;

            if (!IsPostBack)
            {
                rvMain.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerURL"]);
                rvMain.ProcessingMode = ProcessingMode.Remote;
                rvMain.ServerReport.ReportServerCredentials = new CustomReportServerCredentials();
            }

            rvMain.AsyncRendering = false;
            rvMain.SizeToReportContent = true;

            var currentTime = DateTime.Now;
            rvMain.ServerReport.DisplayName = (Uri.EscapeDataString(String.Format("{0}{1}{2}PharmacovigilanceQualityReport", currentTime.Year % 100, currentTime.Month, currentTime.Day)));
        }

        private void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
        }

        #endregion

        #region Fill

        private void ClearForm(object arg)
        {
            txtSrActiveIngredient.Text = String.Empty;
            txtSrProduct.Text = String.Empty;
            txtSrActivity.Text = String.Empty;
            txtSrTask.Text = String.Empty;

            txtTaskId.Text = string.Empty;

            ddlTaskResponsibleUser.SelectedValue = string.Empty;
            ddlTaskPerformanceIndicator.SelectedValue = string.Empty;

            dtRngTaskStartDate.TextFrom = string.Empty;
            dtRngTaskStartDate.TextTo = string.Empty;
            dtRngTaskExpectedFinishedDate.TextFrom = string.Empty;
            dtRngTaskExpectedFinishedDate.TextTo = string.Empty;
            dtRngTaskActualFinishedDate.TextFrom = string.Empty;
            dtRngTaskActualFinishedDate.TextTo = string.Empty;
        }

        private void FillFormControls(object arg)
        {
            FillDdlTaskResponsibleUsers(null);
            FillDdlPerformanceIndicator(null);
        }

        private void SetFormControlsDefaults(object arg)
        {
            BindDynamicControls(null);
        }

        /// <summary>
        /// Binds task responsible users drop down list
        /// </summary>
        /// <param name="args"></param>
        private void FillDdlTaskResponsibleUsers(object args)
        {
            var responsibleUsers = _personOperations.GetPersonsByRole(Constant.PersonRoleName.ResponsibleUser);
            ddlTaskResponsibleUser.Fill(responsibleUsers, "FullName", "person_PK");
            ddlTaskResponsibleUser.SortItemsByText();
        } 
        
        /// <summary>
        /// Binds performance indicator drop down list
        /// </summary>
        /// <param name="args"></param>
        private void FillDdlPerformanceIndicator(object args)
        {
            var taskPerformanceIndicators = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.PVQPerformanceIndicators);

            ddlTaskPerformanceIndicator.Fill(taskPerformanceIndicators, "name", "type_PK");
            ddlTaskPerformanceIndicator.SortItemsByText();
        }

        #endregion

        #region Bind

        private void BindDynamicControls(object arg)
        {

        }

        #endregion

        #endregion

        #region Event handlers

        public void BtnCreateReportClick(object sender, EventArgs e)
        {
            if (!SecurityHelper.IsPermitted(Permission.CreatePharmacovigilanceQualityReport)) return;

            int activeIngredientPk = txtSrActiveIngredient.SelectedEntityId ?? -1;
            int productPk = txtSrProduct.SelectedEntityId ?? -1;
            int activityPk = txtSrActivity.SelectedEntityId ?? -1;
            int taskPk = txtSrTask.SelectedEntityId ?? -1;
            int taskResponsibleUserPk = ddlTaskResponsibleUser.SelectedId ?? -1;
            string taskPerformanceIndicator = ddlTaskPerformanceIndicator.SelectedId.HasValue ? Convert.ToString(ddlTaskPerformanceIndicator.SelectedText) : null;
            string taskId = txtTaskId.Text;
            var taskStartDateFrom = ValidationHelper.IsValidDateTime(dtRngTaskStartDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngTaskStartDate.TextFrom, CultureInfoHr) : null;
            var taskStartDateTo = ValidationHelper.IsValidDateTime(dtRngTaskStartDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngTaskStartDate.TextTo, CultureInfoHr) : null;
            var taskExpectedFinishedDateFrom = ValidationHelper.IsValidDateTime(dtRngTaskExpectedFinishedDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngTaskExpectedFinishedDate.TextFrom, CultureInfoHr) : null;
            var taskExpectedFinishedDateTo = ValidationHelper.IsValidDateTime(dtRngTaskExpectedFinishedDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngTaskExpectedFinishedDate.TextTo, CultureInfoHr) : null;
            var taskActualFinishedDateFrom = ValidationHelper.IsValidDateTime(dtRngTaskActualFinishedDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngTaskActualFinishedDate.TextFrom, CultureInfoHr) : null;
            var taskActualFinishedDateTo = ValidationHelper.IsValidDateTime(dtRngTaskActualFinishedDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngTaskActualFinishedDate.TextTo, CultureInfoHr) : null;

            rvMain.ServerReport.ReportPath = ConfigurationManager.AppSettings["PharmacovigilanceQualityReportName"];
            var setParameters = new List<ReportParameter>();

            setParameters.Add(new ReportParameter("ActiveIngredientPk", activeIngredientPk.ToString()));
            setParameters.Add(new ReportParameter("ProductPk", productPk.ToString()));
            setParameters.Add(new ReportParameter("ActivityPk", activityPk.ToString()));
            setParameters.Add(new ReportParameter("TaskPk", taskPk.ToString()));
            setParameters.Add(new ReportParameter("TaskResponsibleUserPk", taskResponsibleUserPk.ToString()));
            setParameters.Add(new ReportParameter("TaskPerformanceIndicator", !string.IsNullOrWhiteSpace(taskPerformanceIndicator) ? taskPerformanceIndicator : null));
            setParameters.Add(new ReportParameter("TaskId", !string.IsNullOrWhiteSpace(taskId) ? taskId : null));
            setParameters.Add(new ReportParameter("TaskStartDateFrom", taskStartDateFrom.HasValue ? ((DateTime)taskStartDateFrom).ToString("dd.MM.yyyy") : null));
            setParameters.Add(new ReportParameter("TaskStartDateTo", taskStartDateTo.HasValue ? ((DateTime)taskStartDateTo).ToString("dd.MM.yyyy") : null));
            setParameters.Add(new ReportParameter("TaskExpectedFinishedDateFrom", taskExpectedFinishedDateFrom.HasValue ? ((DateTime)taskExpectedFinishedDateFrom).ToString("dd.MM.yyyy") : null));
            setParameters.Add(new ReportParameter("TaskExpectedFinishedDateTo", taskExpectedFinishedDateTo.HasValue ? ((DateTime)taskExpectedFinishedDateTo).ToString("dd.MM.yyyy") : null));
            setParameters.Add(new ReportParameter("TaskActualFinishedDateFrom", taskActualFinishedDateFrom.HasValue ? ((DateTime)taskActualFinishedDateFrom).ToString("dd.MM.yyyy") : null));
            setParameters.Add(new ReportParameter("TaskActualFinishedDateTo", taskActualFinishedDateTo.HasValue ? ((DateTime)taskActualFinishedDateTo).ToString("dd.MM.yyyy") : null));

            rvMain.ShowParameterPrompts = false;
            rvMain.ShowToolBar = true;
            rvMain.ShowPrintButton = false;
            rvMain.ShowZoomControl = false;
            rvMain.ShowExportControls = true;
            rvMain.ShowPrintButton = false;

            rvMain.ServerReport.SetParameters(setParameters);
            rvMain.ServerReport.Refresh();
        }

        public void BtnCreateReportTestClick(object sender, EventArgs e)
        {
            rvMain.ServerReport.ReportPath = ConfigurationManager.AppSettings["PharmacovigilanceQualityReportName"];
            var setParameters = new List<ReportParameter>();
            var expectedFinishedDateFrom = ValidationHelper.IsValidDateTime(dtRngTaskExpectedFinishedDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngTaskExpectedFinishedDate.TextFrom, CultureInfoHr) : null;
            
            setParameters.Add(new ReportParameter("ActiveIngredientPk", "-1"));
            setParameters.Add(new ReportParameter("ProductPk", "-1"));
            setParameters.Add(new ReportParameter("ActivityPk", "-1"));
            setParameters.Add(new ReportParameter("TaskPk", "-1"));
            setParameters.Add(new ReportParameter("TaskResponsibleUserPk", "-1"));
            setParameters.Add(new ReportParameter("TaskPerformanceIndicator", "comply"));
            setParameters.Add(new ReportParameter("TaskId", "-1"));
            setParameters.Add(new ReportParameter("TaskStartDateFrom", "01.01.2010"));
            setParameters.Add(new ReportParameter("TaskStartDateTo", "01.01.2015"));
            setParameters.Add(new ReportParameter("TaskExpectedFinishedDateFrom", expectedFinishedDateFrom.HasValue ? ((DateTime)expectedFinishedDateFrom).ToString("dd.MM.yyyy") : null));
            setParameters.Add(new ReportParameter("TaskExpectedFinishedDateTo", "01.01.2015"));
            setParameters.Add(new ReportParameter("TaskActualFinishedDateFrom", "01.01.2010"));
            setParameters.Add(new ReportParameter("TaskActualFinishedDateTo", "01.01.2015"));

            rvMain.ShowParameterPrompts = false;
            rvMain.ShowToolBar = true;
            rvMain.ShowPrintButton = false;
            rvMain.ShowZoomControl = false;
            rvMain.ShowExportControls = true;
            rvMain.ShowPrintButton = false;

            rvMain.ServerReport.SetParameters(setParameters);
            rvMain.ServerReport.Refresh();
        }

        #region Active ingredient searcher

        /// <summary>
        /// Handles active ingredient list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ActiveIngredientSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var activeIngredient = _substanceOperations.GetEntity(e.Data);

            if (activeIngredient == null || activeIngredient.substance_PK == null) return;

            txtSrActiveIngredient.Text = activeIngredient.substance_name;
            txtSrActiveIngredient.SelectedEntityId = activeIngredient.substance_PK;
        }

        #endregion

        #region Product searcher

        /// <summary>
        /// Handles product list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ProductSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var product = _productOperations.GetEntity(e.Data);

            if (product == null || product.product_PK == null) return;

            txtSrProduct.Text = product.name;
            txtSrProduct.SelectedEntityId = product.product_PK;
        }

        #endregion

        #region Activity searcher

        /// <summary>
        /// Handles activity list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ActivitySearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var actvivity = _activityOperations.GetEntity(e.Data);

            if (actvivity == null || actvivity.activity_PK == null) return;

            txtSrActivity.Text = actvivity.name;
            txtSrActivity.SelectedEntityId = actvivity.activity_PK;
        }

        #endregion

        #region Task searcher

        /// <summary>
        /// Handles task list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TaskSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var task = _taskOperations.GetEntity(e.Data);

            if (task == null || task.task_PK == null || task.task_name_FK == null) return;

            var taskName = _taskNameOperations.GetEntity(task.task_name_FK);

            if (!string.IsNullOrWhiteSpace(taskName.task_name)) txtSrTask.Text = taskName.task_name;
            else txtSrTask.Text = "Missing";
            txtSrTask.SelectedEntityId = task.task_PK;
        }

        #endregion

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            if (!SecurityHelper.IsPermitted(Permission.CreatePharmacovigilanceQualityReport)) btnCreateReport.Disable();

            return true;
        }

        #endregion
    }
}