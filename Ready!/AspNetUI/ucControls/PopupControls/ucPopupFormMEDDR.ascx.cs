using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormMEDDR : DetailsForm
    {
        #region Declarations
        
        
        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;

        IMeddra_pkOperations _meddra_PKOperations;
        IType_PKOperations _type_PKOperations;
        IAuthorisedProductOperations _authorisedProduct_PKOperations;

        private static int? objectID = null;
        public static int idGenerator = 1;

        #endregion

        #region Properties

        public string ModalPopupContainerWidth
        {
            get { return PopupControls_Struct_Container.Style["Width"]; }
            set { PopupControls_Struct_Container.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return PopupControls_Struct_Container.Style["Height"]; }
            set { PopupControls_Struct_Container.Style["Height"] = value; }
        }

        public string ModalPopupContainerBodyPadding
        {
            get { return modalPopupContainerBody.Style["padding"]; }
            set { modalPopupContainerBody.Style["padding"] = value; }
        }

        public int IdGenerator {
            get { return idGenerator; }
            set { idGenerator = value; }
        }

        #endregion

        #region Operations

        public void ShowModalForm(int? _id, string header, int? id)
        {
            if (_id != null)
            {
                objectID = _id;
                BindForm(_id, null);
            }
            else {
                objectID = null;
            }

            PopupControls_Struct_Container.Style["display"] = "inline";
            divHeader.InnerHtml = header;
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            _meddra_PKOperations = new Meddra_pkDAL();
            _type_PKOperations = new Type_PKDAL();
            _authorisedProduct_PKOperations = new AuthorisedProductDAL();
            PopupControls_Struct_Container.Style["display"] = "none";

            if (Session["MEDDRAAddedItems"] == null ) Session["MEDDRAAddedItems"] = new List<Meddra_pk>();
            if (Session["MEDDRANewItemsBuffer"] == null) Session["MEDDRANewItemsBuffer"] = new List<Int32>();

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Form.Enctype = "multipart/form-data";

            //TODO: potrebno je registrirati na full post back save iz context menia
            //ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnSubmit);

            //if (Session["UploadedAttachs"] == null) _attachementOperations.DeleteNULLByUserID(SessionManager.Instance.CurrentUser.UserID);

            //BindGridAttachments();
            if ( !IsPostBack) BindForm(null, null);
        }

        public override object SaveForm(object id, string arg)
        {
            int? version = null, level = null;

            if ( ValidationHelper.IsValidInt(ctlmeddraversion.ControlValue.ToString()))
                            version = Int32.Parse(ctlmeddraversion.ControlValue.ToString());
            if (ValidationHelper.IsValidInt(ctlmeddralevel.ControlValue.ToString()))
                            level = Int32.Parse(ctlmeddralevel.ControlValue.ToString());

            string code = ctlmeddracode.ControlValue.ToString();
            string term = ctlmeddraterm.ControlValue.ToString();

            Meddra_pk entity = null;
            bool newItem = false;
            if (id == null)
            {
                entity = new Meddra_pk();
                entity.meddra_pk = -(++idGenerator);
                newItem = true;
            }
            else
            {
                //entity = _meddra_PKOperations.GetEntity(id);
                entity = ((List<Meddra_pk>)Session["MEDDRAAddedItems"]).Find(item => item.meddra_pk == Convert.ToInt32(id));
                if (entity != null)
                {
                    ((List<Meddra_pk>)Session["MEDDRAAddedItems"]).RemoveAll(item => item.meddra_pk == entity.meddra_pk);
                }
                else
                {
                    entity = new Meddra_pk();
                    entity.meddra_pk = -(++idGenerator);
                    newItem = true;
                }
            }

            entity.version_type_FK = version;
            entity.level_type_FK = level;
            entity.code = code;
            entity.term = term;

            //entity = _meddra_PKOperations.Save(entity);
            if (Session["MEDDRAAddedItems"] == null) Session["MEDDRAAddedItems"] = new List<Meddra_pk>();
            ((List<Meddra_pk>)Session["MEDDRAAddedItems"]).Add(entity);
            //if (newItem) {
            //    ((List<Int32>)Session["MEDDRANewItemsBuffer"]).Add((Int32)entity.meddra_pk);
            //}

            return entity;
        }

        public override void ClearForm(string arg)
        {
            ctlmeddracode.ControlValue = "";
            ctlmeddraterm.ControlValue = "";
            BindMEDDRAVersionDDL(null);
            BindMEDDRLevelDDL(null);
        }

        public override void FillDataDefinitions(string arg)
        {
        }

        public override void BindForm(object id, string arg)
        {

            Meddra_pk entity;
            if (id != null)
            {
                entity = ((List<Meddra_pk>)Session["MEDDRAAddedItems"]).Find(item => item.meddra_pk == Convert.ToInt32(id));

                //entity = _meddra_PKOperations.GetEntity(id);
            }
            else {
                entity = new Meddra_pk();
            }

            if (entity.term != null) ctlmeddraterm.ControlValue = entity.term;
            if (entity.code != null) ctlmeddracode.ControlValue = entity.code;
            
            BindMEDDRAVersionDDL(entity.version_type_FK);
            BindMEDDRLevelDDL(entity.level_type_FK);
            
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (string.IsNullOrEmpty(ctlmeddraversion.ControlValue.ToString()) || ctlmeddraversion.ControlValue.ToString() == "") {
                errorMessage += "MEDDRA Version can't be empty.<br />";
            }

            if (string.IsNullOrEmpty(ctlmeddralevel.ControlValue.ToString()) || ctlmeddralevel.ControlValue.ToString() == "")
            {
                errorMessage += "MEDDRA Level can't be empty.<br />";
            }

            if (string.IsNullOrEmpty(ctlmeddracode.ControlValue.ToString()) || ctlmeddracode.ControlValue.ToString() == "")
            {
                errorMessage += "MEDDRA Code can't be empty. <br />";
            }



            // If errors were found, showing them in modal popup
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }

        #endregion

        #region Form methods

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateForm(null))
            {
                PopupControls_Struct_Container.Style["display"] = "none";
                Meddra_pk entity= (Meddra_pk)SaveForm(objectID, null);
                
                OnOkButtonClick(sender, new FormDetailsEventArgs(entity));
                ClearForm(null);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PopupControls_Struct_Container.Style["display"] = "none";
            ClearForm(null);

            OnCancelButtonClick(sender, new FormDetailsEventArgs(null));
        }

        protected void btnClosePopupForm_Click(object sender, EventArgs e)
        {
            PopupControls_Struct_Container.Style["display"] = "none";
            ClearForm(null);

            OnCancelButtonClick(sender, new FormDetailsEventArgs(null));
        }

        #endregion

        #region Binds
        private void BindMEDDRAVersionDDL(int? selectedId) {
            List<Type_PK> versions = _type_PKOperations.GetTypesForDDL("MV");

            ctlmeddraversion.SourceValueProperty = "type_PK";
            ctlmeddraversion.SourceTextExpression = "name";
            //ctlmeddraversion.FillControl<Type_PK>(versions);

            ctlmeddraversion.ControlBoundItems.Clear();
            foreach (Type_PK v in versions) { 
                ctlmeddraversion.ControlBoundItems.Add(new ListItem(v.name.ToString(), v.type_PK.ToString()));
            }

            Type_PK defaultV = versions.Find(item => item.name == "15.0");

            if (defaultV != null) ctlmeddraversion.ControlValue = defaultV.type_PK;
            if (selectedId != null) ctlmeddraversion.ControlValue = selectedId;
        }

        private void BindMEDDRLevelDDL(int? selectedId) {
            List<Type_PK> levels = _type_PKOperations.GetTypesForDDL("ML");

            levels.Sort(
                    delegate(Type_PK t1, Type_PK t2)
                    {
                        return t1.custom_sort.GetValueOrDefault().CompareTo(t2.custom_sort.GetValueOrDefault());
                    });

            ctlmeddralevel.SourceValueProperty = "type_PK";
            ctlmeddralevel.SourceTextExpression = "name";
            //ctlmeddralevel.FillControl<Type_PK>(levels);

            ctlmeddralevel.ControlBoundItems.Clear();

            foreach (Type_PK lv in levels) {
                ctlmeddralevel.ControlBoundItems.Add(new ListItem(lv.name.ToString(), lv.type_PK.ToString()));
            }

            if (selectedId != null) ctlmeddralevel.ControlValue = selectedId;
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