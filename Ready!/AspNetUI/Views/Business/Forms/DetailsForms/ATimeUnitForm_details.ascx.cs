using System;
using System.Collections.Generic;
using AspNetUIFramework;
using Ready.Model;
using System.Data;
using System.Threading;
using System.Globalization;
using AspNetUI.Support;


namespace AspNetUI.Views
{
    public partial class ATimeUnitForm_details : DetailsForm
    {
        string s_apid = "";
        bool newEntry = false;
        CultureInfo cultureInfo = null;

        static List<Time_unit_name_PK> allTimeUnitName;
        static List<Person_PK> responsibleUser;

        // Model data managers
        ITime_unit_PKOperations _time_unit_PKOperations;
        ITime_unit_name_PKOperations _time_unit_name_PKOperations;
        IPerson_PKOperations _person_PKOperations;
        IActivity_product_PKOperations _activityProductMNOperations;
        IActivity_PKOperations _activityOperations;
        IUSEROperations _userOperations;
        ILast_change_PKOperations _last_change_PKOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnSaveButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;

        // Form initialization
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            s_apid = Request.QueryString["idtu"] != null ? Request.QueryString["idtu"].ToString() : "";

            // Retreiving model data managers from central configuration
            _time_unit_PKOperations = new Time_unit_PKDAL();
            _time_unit_name_PKOperations = new Time_unit_name_PKDAL();
            _person_PKOperations = new Person_PKDAL();
            _userOperations = new USERDAL();
            _activityProductMNOperations = new Activity_product_PKDAL();
            _activityOperations = new Activity_PKDAL();
            _last_change_PKOperations = new Last_change_PKDAL(); 
            
            ActivitySearcher.OnListItemSelected += new EventHandler<FormListEventArgs>(ActivitySearcher_OnListItemSelected);
            ActivitySearcherDisplay.OnSearchClick += new EventHandler<EventArgs>(ActivitySearcher_OnSearchClick);
            ActivitySearcherDisplay.OnRemoveClick += new EventHandler<EventArgs>(ActivitySearcherDisplay_OnRemoveClick);

            cultureInfo = new System.Globalization.CultureInfo("hr-HR");

            if (!IsPostBack)
            {
                CBRefresh();
            }

            ConfirmAction.OnConfirmInputButtonYes_Click += new EventHandler<FormPopupEventArgs>(ConfirmActionHandler);
        }

        private void CBRefresh()
        {
            responsibleUser = CBLoader.LoadResponsibleUsers();
            allTimeUnitName = CBLoader.LoadTimeUnitName();
        }
        public void btnSaveOnClick(object sender, EventArgs e)
        {
            if (OnSaveButtonClick != null)
            OnSaveButtonClick(null, new FormDetailsEventArgs(null));
        }

        public void btnCancelOnClick(object sender, EventArgs e)
        {
            if (OnCancelButtonClick != null)
                OnCancelButtonClick(null, new FormDetailsEventArgs(null));
        }

        #region FormOverrides

        // Saves form
        public override object SaveForm(object id, string arg)
        {
            Time_unit_PK entity = null;

            if (Request.QueryString["f"] != null && Request.QueryString["f"].ToString() == "sa" && Request.QueryString["sa"] == "1") entity = new Time_unit_PK();
            else
            {
                if (id != null) entity = _time_unit_PKOperations.GetEntity(id);

                if (entity == null)
                {
                    // new Time_unit_PK
                    entity = new Time_unit_PK();
                }
            }

            if (Permission != DetailsPermissionType.READ_WRITE)
            {
                return entity;
            }

            if (ctlresponsible_user.ControlValue != null && ctlresponsible_user.ControlValue.ToString() != "")
            {
                entity.user_FK = (int?)Convert.ToInt32(ctlresponsible_user.ControlValue);
            }
            else
            {
                entity.user_FK = null;
            }

            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            entity.inserted_by = user != null ? user.Person_FK : null;

            entity.time_unit_name_FK = ValidationHelper.IsValidInt(ctltime_unit_name_FK.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctltime_unit_name_FK.ControlValue) : null;
            entity.description = ctlDescription.ControlValue.ToString();
            entity.comment = ctlcomment.ControlValue.ToString();
            entity.actual_date = ValidationHelper.IsValidDateTime(ctlactual_date.ControlValue.ToString(), new System.Globalization.CultureInfo("hr-HR")) ? (DateTime?)Convert.ToDateTime(ctlactual_date.ControlValue).Date : null;
            
            if (ctltime_hours.ControlValue.ToString().Trim() == String.Empty) entity.time_hours = 0;
            else entity.time_hours = ValidationHelper.IsValidInt(ctltime_hours.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctltime_hours.ControlValue) : 0;

            if (ctltime_minutes.ControlValue.ToString().Trim() == String.Empty) entity.time_minutes = 0;
            else entity.time_minutes = ValidationHelper.IsValidInt(ctltime_minutes.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctltime_minutes.ControlValue) : 0;
            
            //entity.activity_FK = ValidationHelper.IsValidInt(Request.QueryString["idAct"]) ? (int?)Convert.ToInt32(Request.QueryString["idAct"]) : entity.activity_FK;
            entity.activity_FK = ActivitySearcherDisplay.SelectedObject != null && ValidationHelper.IsValidInt(ActivitySearcherDisplay.SelectedObject.ToString()) ? (int?)Convert.ToInt32(ActivitySearcherDisplay.SelectedObject.ToString()) : null;
            entity = _time_unit_PKOperations.Save(entity);

            if (!ListOperations.ListsEquals<int>(InitialFormHash, AuditTrailHelper.GetPanelHashValue(pnlDataDetails)))
            {
                AuditTrailHelper.UpdateLastChange(entity.time_unit_PK, "TIME", _last_change_PKOperations, _userOperations);
            }

            FirstBind.Text = "0";
            return entity;
        }

        // Clears form
        public override void ClearForm(string arg)
        {
            if (FirstBind.Text == "0")
            {
                ctlID.ControlLabel = String.Empty;
                ctltime_unit_name_FK.ControlValue = String.Empty;
                ctlDescription.ControlValue = String.Empty;
                ctlcomment.ControlValue = String.Empty;
                ctlactual_date.ControlValue = String.Empty;
                ctltime_hours.ControlValue = String.Empty;
                ctltime_minutes.ControlValue = String.Empty;
            }

        }

        // Fills all form controls with data
        public override void FillDataDefinitions(string arg)
        {
            // 
            BindDDLTimeUnitName();
            BindDDLResponsibleUser();

            //if (newEntry || Request.QueryString["sa"] == "1") {
            //    USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            //    ctlresponsible_user.ControlValue = user.Person_FK;
            //}

            if (newEntry || Request.QueryString["sa"] == "1")
            {
                ctlactual_date.ControlValue = DateTime.Now.Date;

                USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
                ctlresponsible_user.ControlValue = user.Person_FK;

                if (Request.QueryString["idtu"] != null)
                {
                    Time_unit_PK entity = _time_unit_PKOperations.GetEntity(Request.QueryString["idtu"].ToString());

                    if (entity != null)
                    {
                        if(Request.QueryString["f"] != "dn") ctltime_unit_name_FK.ControlValue = !entity.time_unit_name_FK.HasValue ? String.Empty : entity.time_unit_name_FK.ToString();
                        if (Request.QueryString["sa"] != "1")
                            ctlresponsible_user.ControlValue = entity.user_FK.HasValue ? entity.user_FK : null;
                    }
                }

            }
        }

        private void BindDDLTimeUnitName()
        {
            ctltime_unit_name_FK.ControlBoundItems.Clear();

            List<Time_unit_name_PK> items = allTimeUnitName; //_time_unit_name_PKOperations.GetEntities();

            //items.Sort(delegate(Time_unit_name_PK item1, Time_unit_name_PK item2)
            //{
            //    return item1.time_unit_name.CompareTo(item2.time_unit_name);
            //});

            ctltime_unit_name_FK.SourceValueProperty = "time_unit_name_PK";
            ctltime_unit_name_FK.SourceTextExpression = "time_unit_name";
            ctltime_unit_name_FK.FillControl<Time_unit_name_PK>(items);
        }

        void BindDDLResponsibleUser()
        {
            List<Person_PK> items = responsibleUser;//_person_PKOperations.GetEntities(); // TODO: .GetPersonsByRole("Responsible user");

            //items.Sort(delegate(Person_PK c1, Person_PK c2)
            //{
            //    return c1.name.CompareTo(c2.name);
            //});

            //foreach (Person_PK item in items)
            //{
            //    item.name += " " + item.familyname;
            //}

            ctlresponsible_user.SourceValueProperty = "person_PK";
            ctlresponsible_user.SourceTextExpression = "name";
            ctlresponsible_user.FillControl<Person_PK>(items);

        }

        // Binds form
        public override void BindForm(object id, string arg)
        {
            Time_unit_PK entity = null;

            if (arg == "sa")
            {
                pnlTimeUnitProperties.Visible = false;
               
                if (FirstBind.Text == "0")
                {
                    {
                        pnlDataDetails.Visible = true;
                        pnlFooter.Visible = true;

                        entity = _time_unit_PKOperations.GetEntity(Request.QueryString["idtu"].ToString());

                        if (entity != null)
                        {
                            FillDataDefinitions("");
                            ctltime_unit_name_FK.ControlValue = !entity.time_unit_name_FK.HasValue ? String.Empty : entity.time_unit_name_FK.ToString();
                            
                            ctlDescription.ControlValue = entity.description == null ? String.Empty : entity.description.ToString();
                            ctlactual_date.ControlValue = !entity.actual_date.HasValue ? String.Empty : ((DateTime)entity.actual_date).ToString("dd.MM.yyyy");
                            ctltime_hours.ControlValue = !entity.time_hours.HasValue ? String.Empty : entity.time_hours.ToString();
                            ctltime_minutes.ControlValue = !entity.time_minutes.HasValue ? String.Empty : entity.time_minutes.ToString();
                            ctlcomment.ControlValue = entity.comment == null ? String.Empty : entity.comment.ToString();

                            if (Request.QueryString["f"] == "sa")
                            {
                                USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
                                ctlresponsible_user.ControlValue = user != null ? user.Person_FK : null;
                            }
                            else
                            {
                                ctlresponsible_user.ControlValue = !entity.user_FK.HasValue ? null : entity.user_FK;
                            }

                            Activity_PK act = _activityOperations.GetEntity(entity.activity_FK);
                            if (act != null) ActivitySearcherDisplay.SetSelectedObject(act.activity_PK, act.name);

                            FirstBind.Text = "1";

                            if (!IsInitialHashWritten)
                            {
                                if (Request.QueryString["f"] != "sa" && Request.QueryString["sa"] != "1")
                                {
                                    InitialFormHash = AuditTrailHelper.GetPanelHashValue(pnlDataDetails);
                                }
                                IsInitialHashWritten = true;
                            }
                        }
                    }
                }

            }
            else
            {
                if (id != null)
                {
                    if (((string)id).StartsWith("p"))
                    {
                        pnlTimeUnitProperties.Visible = true;
                        pnlDataDetails.Visible = false;
                        pnlFooter.Visible = false;
                        FirstBind.Text = "0";

                        entity = _time_unit_PKOperations.GetEntity(Request.QueryString["idtu"].ToString());

                        if (entity != null)
                        {
                            Time_unit_name_PK timeUnitName = null;
                            if (entity.time_unit_name_FK != null)
                                timeUnitName = allTimeUnitName.Find(item => item.time_unit_name_PK == entity.time_unit_name_FK);// _time_unit_name_PKOperations.GetEntity(entity.time_unit_name_FK);
                            Person_PK resUser = null;
                            if (entity.user_FK != null)
                                resUser = responsibleUser.Find(item => item.person_PK == entity.user_FK);

                            Person_PK insertedBy = null;
                            if (entity.inserted_by != null)
                                insertedBy = responsibleUser.Find(item => item.person_PK == entity.inserted_by);

                            lblTaskName.Text = timeUnitName != null ? timeUnitName.time_unit_name ?? "-" : "-";
                            //lblResponsibleUser.Text = entity.user_FK == null ? "-" : String.Format("{0} {1}", resUser.name, resUser.familyname);
                            lblResponsibleUser.Text = resUser != null ? resUser.name : "-";
                            //lblInsertedBy.Text = entity.inserted_by == null ? "-" : String.Format("{0} {1}", insertedBy.name, insertedBy.familyname);
                            lblInsertedBy.Text = insertedBy != null ? insertedBy.name : "-";
                            lblDescription.Text = !String.IsNullOrWhiteSpace(entity.description) ? StringOperations.TrimAfter(entity.description) : "-";
                            lblActualDate.Text = !entity.actual_date.HasValue ? "-" : ((DateTime)entity.actual_date).ToString("dd.MM.yyyy");

                            string time = "";
                            if (entity.time_hours != null && entity.time_hours.ToString() != "")
                            {
                                //time = String.Format("{00:##}", entity.time_hours.ToString());
                                if (entity.time_hours.ToString().Length < 2) time += "0";
                                time = entity.time_hours.ToString();
                            }
                            else
                            {
                                time = "00";
                            }
                            if (time.Length < 2) time = "0" + time;
                            if (entity.time_minutes != null && entity.time_minutes.ToString() != "")
                            {
                                //time = String.Format("{00:##}", entity.time_minutes.ToString());
                                time += ":";
                                if (entity.time_minutes.ToString().Length < 2) time += "0";
                                time += entity.time_minutes.ToString();
                            }
                            else
                            {
                                time += ":00";
                            }
                            lblTime.Text = time;


                            lblComment.Text = !String.IsNullOrWhiteSpace(entity.comment) ? StringOperations.TrimAfter(entity.comment) : "-";

                            lblLastChange.Text = AuditTrailHelper.GetLastChangeFormattedString(entity.time_unit_PK, "TIME", _last_change_PKOperations, _person_PKOperations);

                            if (entity.time_unit_PK.HasValue)
                            {
                                BindProducts(entity.time_unit_PK);
                            }

                            divActivityLink.InnerHtml = "-";
                            if (entity.activity_FK.HasValue)
                            {
                                Activity_PK act = _activityOperations.GetEntity(entity.activity_FK);
                                if (act != null)
                                {
                                    string activityName;
                                    if (act.name != null && act.name != "") activityName = act.name;
                                    else activityName = "Nameless activity";
                                    divActivityLink.InnerHtml = "<a href=APropertiesView.aspx?f=l&idAct=" + act.activity_PK + ">" + activityName + "</a><br />";
                                }
                            }
                        }

                    }
                    else if (Request.QueryString["f"] != null && Request.QueryString["f"].ToString() != "dn")
                    {
                        pnlDataDetails.Visible = true;
                        pnlFooter.Visible = true;
                        pnlTimeUnitProperties.Visible = false;

                        entity = _time_unit_PKOperations.GetEntity(Request.QueryString["idtu"].ToString());

                        if (entity != null)
                        {
                            ctltime_unit_name_FK.ControlValue = !entity.time_unit_name_FK.HasValue ? String.Empty : entity.time_unit_name_FK.ToString();
                            ctlresponsible_user.ControlValue = !entity.user_FK.HasValue ? null : _person_PKOperations.GetEntity(entity.user_FK).person_PK;
                            ctlDescription.ControlValue = entity.description == null ? String.Empty : entity.description.ToString();
                            ctlactual_date.ControlValue = !entity.actual_date.HasValue ? String.Empty : ((DateTime)entity.actual_date).ToString("dd.MM.yyyy");
                            ctltime_hours.ControlValue = !entity.time_hours.HasValue ? String.Empty : entity.time_hours.ToString();
                            ctltime_minutes.ControlValue = !entity.time_minutes.HasValue ? String.Empty : entity.time_minutes.ToString();
                            ctlcomment.ControlValue = entity.comment == null ? String.Empty : entity.comment.ToString();

                            Activity_PK act = _activityOperations.GetEntity(entity.activity_FK);
                            if (act != null) ActivitySearcherDisplay.SetSelectedObject(act.activity_PK, act.name);

                            if (!IsInitialHashWritten)
                            {
                                if (Request.QueryString["f"] != "sa" && Request.QueryString["sa"] != "1")
                                {
                                    InitialFormHash = AuditTrailHelper.GetPanelHashValue(pnlDataDetails);
                                }
                                IsInitialHashWritten = true;
                            }

                        }
                    }
                    else {
                        pnlDataDetails.Visible = true;
                        pnlFooter.Visible = true;
                        pnlTimeUnitProperties.Visible = false;

                        USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
                        ctlresponsible_user.ControlValue = user.Person_FK;

                        ctlactual_date.ControlValue = DateTime.Now.Date;

                        if (Request.QueryString["idAct"] != null && Request.QueryString["idAct"].ToString().Trim() != "")
                        {
                            string idActS = Request.QueryString["idAct"].ToString().Trim();
                            int idAct = ValidationHelper.IsValidInt(idActS) ? Convert.ToInt32(idActS) : 0;
                            Activity_PK act = idAct != 0 ? _activityOperations.GetEntity(idAct) : null;
                            if (act != null) ActivitySearcherDisplay.SetSelectedObject(act.activity_PK, act.name);
                        }
                    }
                }
                else
                {
                    pnlTimeUnitProperties.Visible = false;

                    USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
                    ctlresponsible_user.ControlValue = user.Person_FK;

                    ctlactual_date.ControlValue = DateTime.Now.Date;

                    if (Request.QueryString["idAct"] != null && Request.QueryString["idAct"].ToString().Trim() != "")
                    {
                        string idActS = Request.QueryString["idAct"].ToString().Trim();
                        int idAct = ValidationHelper.IsValidInt(idActS) ? Convert.ToInt32(idActS) : 0;
                        Activity_PK act = idAct != 0 ? _activityOperations.GetEntity(idAct) : null;
                        if (act != null) ActivitySearcherDisplay.SetSelectedObject(act.activity_PK, act.name);
                    }

                    newEntry = true;
                }
            }
        }

        private void BindProducts(int? timeUnit_PK)
        {
            Time_unit_PK tu = _time_unit_PKOperations.GetEntity(timeUnit_PK);

            if (tu == null || tu.activity_FK == null)
            {
                divProductsLinks.InnerHtml = "-";
                return;
            }

            DataSet ds = _activityProductMNOperations.GetProductsByActivity(tu.activity_FK);
            DataTable dt = null;
            if (ds != null)
            {
                if (ds.Tables.Count > 0) dt = ds.Tables[0];
            }

            int rowNum = dt.Rows.Count;
            divProductsLinks.InnerHtml = "";

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    divProductsLinks.InnerHtml += (--rowNum) != 0 ?
                      "<a href=ProductPKPPropertiesView.aspx?f=l&id=" + dr["product_PK"].ToString() + ">" + dr["name"].ToString() + "</a>,<br />" :
                      "<a href=ProductPKPPropertiesView.aspx?f=l&id=" + dr["product_PK"].ToString() + ">" + dr["name"].ToString() + "</a><br />";
                }
                if (dt.Rows.Count == 0) divProductsLinks.InnerHtml = "-";
            }
        }

        // Validates form
        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrWhiteSpace(ctltime_unit_name_FK.ControlValue.ToString())) errorMessage += ctltime_unit_name_FK.ControlEmptyErrorMessage + "<br />";
            if (!string.IsNullOrWhiteSpace(ctlactual_date.ControlValue.ToString()) && !ValidationHelper.IsValidDateTime(ctlactual_date.ControlValue.ToString(), cultureInfo)) errorMessage += ctlactual_date.ControlErrorMessage + "<br />";
            if (!String.IsNullOrWhiteSpace(ctltime_hours.ControlValue.ToString()))
                if (!ValidationHelper.IsValidInt(ctltime_hours.ControlValue.ToString())) errorMessage += ctltime_hours.ControlErrorMessage + "<br />";
            if (!String.IsNullOrWhiteSpace(ctltime_minutes.ControlValue.ToString()))
                if (!ValidationHelper.IsValidInt(ctltime_minutes.ControlValue.ToString())) errorMessage += ctltime_minutes.ControlErrorMessage + "<br />";

            // If errors were found, showing them in modal popup
            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }

        public void btnDeleteOnClick(object sender, EventArgs e)
        {
            ConfirmAction.ShowModalPopup("Delete", null, "Are you sure that you want to delete this record?", "Warning!");
        }

        public void ConfirmActionHandler(object sender, FormPopupEventArgs e)
        {
            
            string action = e.Action;
            object parameter = e.Data;

            if (action == "Delete")
            {
                try
                {
                    // Deletes record from model
                    if (ValidationHelper.IsValidInt(Request.QueryString["idtu"]))
                        {
                            _time_unit_PKOperations.Delete(Convert.ToInt32(Request.QueryString["idtu"].ToString()));

                            bool allTimeUnits = Session["AllTimeUnits"] != null ? (bool)Session["AllTimeUnits"] : true; // show All time units in case of error

                            if (allTimeUnits) Response.Redirect("~/Views/Business/AllTimeUnitView.aspx?f=l");
                            else Response.Redirect("~/Views/Business/TimeUnitView.aspx?f=l");

                            if (Request.QueryString["idAct"] != null)
                            {
                                Response.Redirect("~\\Views\\Business\\ATimeUnitView.aspx?f=l&idAct=" + Request.QueryString["idAct"].ToString());
                            }
                            else
                            {
                                Response.Redirect("~\\Views\\Business\\TimeUnitView.aspx?f=l");
                            }
                        }
                }
                catch (Exception ex)
                {
                    FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Message", "<br /><center>Can't be deleted. Item is in use.</center><br />");
                }
            }

        }

        #endregion

        #region activityDisplay

        // ActivitySearcher
        void ActivitySearcher_OnListItemSelected(object sender, FormListEventArgs e)
        {
            Activity_PK activity = _activityOperations.GetEntity(e.DataItemID);

            if (activity != null && activity.name != null)
                ActivitySearcherDisplay.SetSelectedObject(activity.activity_PK, activity.name);

        }
        void ActivitySearcher_OnSearchClick(object sender, EventArgs e)
        {
            ActivitySearcher.ShowModalSearcher("Activities");
        }

        void ActivitySearcherDisplay_OnRemoveClick(object sender, EventArgs e)
        {
            ActivitySearcherDisplay.EnableSearcher(true);
        }

        #endregion

        #region Security

        public override DetailsPermissionType CheckAccess()
        {
            if (SecurityOperations.CheckUserRole("Office"))
            {
                return DetailsPermissionType.READ_WRITE;
            }

            if (SecurityOperations.CheckUserRole("User"))
            {
                if (Request.QueryString["f"] == "sa" || Request.QueryString["sa"] == "1") return DetailsPermissionType.READ_WRITE;
                return SecurityOperations.CheckResponsibleUserAccessForDetails(SecurityOperations.SecuredEntity.TIME, Request.QueryString["idtu"]);
            }

            return DetailsPermissionType.READ;
        }

        #endregion
    }
}