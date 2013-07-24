using System;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormVER : DetailsForm
    {
        #region Declarations

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "Version";

        #endregion

        #region Properties

        public string ModalPopupContainerWidth
        {
            get { return PopupControls_Entity_Container.Style["Width"]; }
            set { PopupControls_Entity_Container.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return PopupControls_Entity_Container.Style["Height"]; }
            set { PopupControls_Entity_Container.Style["Height"] = value; }
        }

        public string ModalPopupContainerBodyPadding
        {
            get { return modalPopupContainerBody.Style["padding"]; }
            set { modalPopupContainerBody.Style["padding"] = value; }
        }

        private SSIRepository SSIRep
        {
            get { return (SSIRepository)Session["SSIRepository"]; }
            set { Session["SSIRepository"] = value; }
        }
        private int _id
        {
            get { return (int)Session["VER_id"]; }
            set { Session["VER_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["VER_entityOC"]; }
            set { Session["VER_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["VER_entityParentOC"]; }
            set { Session["VER_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["VER_popupFormMode"]; }
            set { Session["VER_popupFormMode"] = value; }
        }
        private Version_PK entity
        {
            get { return (Version_PK)Session["VER_entity"]; }
            set { Session["VER_entity"] = value; }
        }

        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Version_PK inEntity, ObjectContainer inParentOC)
        {

            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Version_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.version_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.version_PK != null)
                    _id = (int)inEntity.version_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new Version_PK();
                SaveForm(_id, null);
                entity.version_PK = _id;

                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.EditedObjectContainer = inEntityOC;
                entityOC.AssignedObjects = inEntityOC.AssignedObjects;

                if (inEntityOC.ActionOld == ActionType.New)
                    entityOC.SetState(ActionType.New, StatusType.Temp);
                else
                    entityOC.SetState(ActionType.Edit, StatusType.Temp);
            }
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            base.OnInit(e);
        }

        public override object SaveForm(object id, string arg)
        {
            entity.version_number = Int32.Parse(txtVersionNumber.ControlValue.ToString());
            entity.effectve_date = dbxEffectiveDate.ControlValue.ToString();
            if (popupFormMode == PopupFormMode.Edit)
            {
                entity.change_made = txtChangeMade.ControlValue.ToString();
            }
            return entity;
        }

        public override void ClearForm(string arg)
        {
            txtVersionNumber.ControlValue = "";
            dbxEffectiveDate.ControlValue = "";
            txtChangeMade.ControlValue = "";
        }

        public override void FillDataDefinitions(string arg)
        {

        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;
            int dummyData;
            if (String.IsNullOrEmpty(txtVersionNumber.ControlValue.ToString())) errorMessage += txtVersionNumber.ControlEmptyErrorMessage + "<br />";
            if (!Int32.TryParse(txtVersionNumber.ControlValue.ToString(), out dummyData)) errorMessage += txtVersionNumber.ControlErrorMessage + "<br />";
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("hr-HR");
            if (dbxEffectiveDate.ControlValue.ToString() != "" && !ValidationHelper.IsValidDateTime(dbxEffectiveDate.ControlValue.ToString(), cultureInfo)) errorMessage += dbxEffectiveDate.ControlErrorMessage + "<br />";
            
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }

            return true;
        }


        public override void BindForm(object id, string arg)
        {
            if (popupFormMode == PopupFormMode.Edit)
            {
                txtChangeMade.ControlValue = entity.change_made;
                txtVersionNumber.ControlValue = entity.version_number;
                dbxEffectiveDate.ControlValue = entity.effectve_date;
            }
        }

        #endregion

        #region Form methods

        public void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateForm(null))
            {
                SaveForm(_id, null);
                SSIRep.SaveState(entityOC, entityType, entityParentOC);

                PopupControls_Entity_Container.Style["display"] = "none";
                OnOkButtonClick(sender, new FormDetailsEventArgs(entity));
                ClearForm(null);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            ClearForm(null);

            SSIRep.DiscardObjectAndRestore(entityOC, entityType, entityParentOC);
            OnCancelButtonClick(sender, new FormDetailsEventArgs(null));
        }

        protected void btnClosePopupForm_Click(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            ClearForm(null);

            SSIRep.DiscardObjectAndRestore(entityOC, entityType, entityParentOC);
            OnCancelButtonClick(sender, new FormDetailsEventArgs(null));
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
                return DetailsPermissionType.READ;
            }

            return DetailsPermissionType.READ;
        }

        #endregion
    }
}