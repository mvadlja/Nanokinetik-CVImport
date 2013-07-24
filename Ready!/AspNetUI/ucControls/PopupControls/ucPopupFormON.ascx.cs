using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AspNetUIFramework;
using AspNetUI;
using GEM2Common;
using Kmis.Model;
using Ready.Model;
using AspNetUI.Support;


namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormON : DetailsForm
    {
        
        #region Declarations
        
        IOfficial_name_PKOperations _official_name_PKOperations;
        ISsi__cont_voc_PKOperations _ssi__cont_voc_PKOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "OfficialName";

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
            get { return (int)Session["ON_id"]; }
            set { Session["ON_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["ON_entityOC"]; }
            set { Session["ON_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["ON_entityParentOC"]; }
            set { Session["ON_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["ON_popupFormMode"]; }
            set { Session["ON_popupFormMode"] = value; }
        }
        private Official_name_PK entity
        {
            get { return (Official_name_PK)Session["ON_entity"]; }
            set { Session["ON_entity"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Official_name_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Official_name_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.official_name_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
                ClearForm(null);
            }
            else
            {  
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.official_name_PK != null)
                    _id = (int)inEntity.official_name_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);
                
                BindForm(_id, null);
                entity = new Official_name_PK();
                SaveForm(_id, null);
                entity.official_name_PK = _id;

                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.EditedObjectContainer = inEntityOC;
                entityOC.AssignedObjects =  inEntityOC.AssignedObjects;
                if (inEntityOC.ActionOld == ActionType.New)
                    entityOC.SetState(ActionType.New, StatusType.Temp);
                else
                    entityOC.SetState(ActionType.Edit, StatusType.Temp);
                
                BindAssignedObjects();
            }

            if (Session["SubNameONMandatory"] != null && (bool)Session["SubNameONMandatory"] == true)
            {
                ctlONDomains.IsMandatory = true;
                ctlONJuristictions.IsMandatory = true;
            }
            else
            {
                ctlONDomains.IsMandatory = false;
                ctlONJuristictions.IsMandatory = false;
            }
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            _official_name_PKOperations = new Official_name_PKDAL();
            _ssi__cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();

            base.OnInit(e);
        }

        public override object SaveForm(object id, string arg)
        {
            entity.on_type_FK = ValidationHelper.IsValidInt(ctlONameType.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlONameType.ControlValue) : null;
            entity.on_status_FK = ValidationHelper.IsValidInt(ctlONameStatus.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlONameStatus.ControlValue) : null;
            entity.on_status_changedate = ValidationHelper.IsValidDateTime(ctlONameStatusChangeDate.ControlValue.ToString()) ? ctlONameStatusChangeDate.ControlValue.ToString() : null;

            return entity;
        }

        public override void ClearForm(string arg)
        {
            ctlONameType.ControlValue = String.Empty;
            ctlONameStatus.ControlValue = String.Empty;
            ctlONameStatusChangeDate.ControlValue = String.Empty;
            ListItemCollection lic = ctlONJuristictions.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            lic = ctlONDomains.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                  li.Selected = false;
            }
        }

        public override void FillDataDefinitions(string arg)
        {
            BindDDLONType();
            BindDDLONStatus();
            BindListONJuristicion();
            BindListONDomain();
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrEmpty(ctlONameType.ControlValue.ToString())) errorMessage += ctlONameType.ControlEmptyErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlONameStatus.ControlValue.ToString())) errorMessage += ctlONameStatus.ControlEmptyErrorMessage + "<br />";
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("hr-HR");
            if (ctlONameStatusChangeDate.ControlValue.ToString() != "" && !ValidationHelper.IsValidDateTime(ctlONameStatusChangeDate.ControlValue.ToString(), cultureInfo)) errorMessage += ctlONameStatusChangeDate.ControlErrorMessage + "<br />";
            if (Session["SubNameONMandatory"] != null && (bool)Session["SubNameONMandatory"] == true)
            {
                int numSelected = 0;
                foreach (ListItem item in ctlONDomains.ControlBoundItems)
                {
                    if (item.Selected == true)
                        numSelected++;
                    if (numSelected > 0)
                        break;
                }
                if (numSelected == 0) errorMessage += ctlONDomains.ControlEmptyErrorMessage + "<br />";

                numSelected = 0;
                foreach (ListItem item in ctlONJuristictions.ControlBoundItems)
                {
                    if (item.Selected == true)
                        numSelected++;
                    if (numSelected > 0)
                        break;
                }
                if (numSelected == 0) errorMessage += ctlONJuristictions.ControlEmptyErrorMessage + "<br />";
            }
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }
        private void BindDDLONType()
        {
            ctlONameType.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssi__cont_voc_PKOperations.GetEntitiesByListName("Source");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlONameType.SourceValueProperty = "ssi__cont_voc_PK";
            ctlONameType.SourceTextExpression = "term_name_english";
            ctlONameType.FillControl<Ssi__cont_voc_PK>(items);
        }
        private void BindDDLONStatus()
        {
            ctlONameStatus.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssi__cont_voc_PKOperations.GetEntitiesByListName("Status");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlONameStatus.SourceValueProperty = "ssi__cont_voc_PK";
            ctlONameStatus.SourceTextExpression = "term_name_english";
            ctlONameStatus.FillControl<Ssi__cont_voc_PK>(items);
        }
        void BindListONDomain()
        {
            ctlONDomains.ControlBoundItems.Clear();

            List<Ssi__cont_voc_PK> items = _ssi__cont_voc_PKOperations.GetEntitiesByListName("Domain");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlONDomains.SourceValueProperty = "ssi__cont_voc_PK";
            ctlONDomains.SourceTextExpression = "term_name_english";
            ctlONDomains.FillControl<Ssi__cont_voc_PK>(items);
        }
        void BindListONJuristicion()
        {
            ctlONJuristictions.ControlBoundItems.Clear();

            List<Ssi__cont_voc_PK> items = _ssi__cont_voc_PKOperations.GetEntitiesByListName("Country Code");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.synonim1.CompareTo(s2.synonim1);
            });

            ctlONJuristictions.SourceValueProperty = "ssi__cont_voc_PK";
            ctlONJuristictions.SourceTextExpression = "synonim1";
            ctlONJuristictions.FillControl<Ssi__cont_voc_PK>(items);
        }
        public override void BindForm(object id, string arg)
        {
            if (popupFormMode == PopupFormMode.Edit)
            {
                ctlONameStatus.ControlValue = entity.on_status_FK == null ? String.Empty : entity.on_status_FK.ToString();
                ctlONameType.ControlValue = entity.on_type_FK == null ? String.Empty : entity.on_type_FK.ToString();
                ctlONameStatusChangeDate.ControlValue = entity.on_status_changedate == null ? String.Empty : entity.on_status_changedate.ToString();
            }
        }
        private void BindSelectedDomains()
        {
            List<Ssi__cont_voc_PK> domains = SSIRep.GetObjectsList<Ssi__cont_voc_PK>("Domain", entityOC);
            ListItemCollection lic = ctlONDomains.ControlBoundItems;

            if (domains.Count > 0)
            {
                foreach (ListItem li in lic)
                {
                    if (domains.Find((Ssi__cont_voc_PK r) => r.ssi__cont_voc_PK.ToString() == li.Value) != null) li.Selected = true;
                    else li.Selected = false;
                }
            }
        }
        private void BindSelectedJuristictions()
        {
            List<Ssi__cont_voc_PK> juristiction = SSIRep.GetObjectsList<Ssi__cont_voc_PK>("Juristiction", entityOC);
            ListItemCollection lic = ctlONJuristictions.ControlBoundItems;

            if (juristiction.Count > 0)
            {
                foreach (ListItem li in lic)
                {
                    if (juristiction.Find((Ssi__cont_voc_PK r) => r.ssi__cont_voc_PK.ToString() == li.Value) != null) li.Selected = true;
                    else li.Selected = false;
                }
            }
        }
        private void BindAssignedObjects()
        {
            BindSelectedDomains();
            BindSelectedJuristictions();
        }

        private void SaveSelectedDomains()
        {
            List<int> selectedKeys = new List<int>();
            List<int> newKeys = new List<int>();

            foreach (ListItem item in ctlONDomains.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    selectedKeys.Add(Int32.Parse(item.Value));
                    newKeys.Add(Int32.Parse(item.Value));
                }
            }
            if (entityOC.AssignedObjects.ContainsKey("Domain"))
            foreach (ObjectContainer oc in entityOC.AssignedObjects["Domain"])
            {
                    oc.SetState(ActionType.Delete, StatusType.Temp);
            }

            foreach (int key in selectedKeys)
            {
                ObjectContainer newObject = new ObjectContainer();
                newObject.Object = _ssi__cont_voc_PKOperations.GetEntity(key);
                newObject.ID = key;

                if (!entityOC.AssignedObjects.ContainsKey("Domain"))
                    entityOC.AssignedObjects.Add("Domain", new List<ObjectContainer>());

                if (!entityOC.AssignedObjectsTemp.ContainsKey("Domain"))
                    entityOC.AssignedObjectsTemp.Add("Domain", new List<ObjectContainer>());

                entityOC.AssignedObjects["Domain"].Add(newObject);
                entityOC.AssignedObjectsTemp["Domain"].Add(newObject);

                newObject.SetState(ActionType.New, StatusType.Temp);
            }
        }

        private void SaveSelectedJuristictions()
        {
            List<int> selectedKeys = new List<int>();
            List<int> newKeys = new List<int>();

            foreach (ListItem item in ctlONJuristictions.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    selectedKeys.Add(Int32.Parse(item.Value));
                    newKeys.Add(Int32.Parse(item.Value));
                }
            }
            if (entityOC.AssignedObjects.ContainsKey("Juristiction"))
                foreach (ObjectContainer oc in entityOC.AssignedObjects["Juristiction"])
                {
                        oc.SetState(ActionType.Delete, StatusType.Temp);
                }

            foreach (int key in selectedKeys)
            {
                ObjectContainer newObject = new ObjectContainer();
                newObject.Object = _ssi__cont_voc_PKOperations.GetEntity(key);
                newObject.ID = key;

                if (!entityOC.AssignedObjects.ContainsKey("Juristiction"))
                    entityOC.AssignedObjects.Add("Juristiction", new List<ObjectContainer>());

                if (!entityOC.AssignedObjectsTemp.ContainsKey("Juristiction"))
                    entityOC.AssignedObjectsTemp.Add("Juristiction", new List<ObjectContainer>());

                entityOC.AssignedObjects["Juristiction"].Add(newObject);
                entityOC.AssignedObjectsTemp["Juristiction"].Add(newObject);

                newObject.SetState(ActionType.New, StatusType.Temp);
            }
        }
        #endregion

        #region Form methods

        public void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateForm(null))
            {
                entity = (Official_name_PK)SaveForm(_id, null);
                SaveSelectedDomains();
                SaveSelectedJuristictions();
                SSIRep.SaveState(entityOC, entityType,entityParentOC);

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