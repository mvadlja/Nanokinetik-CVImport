using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormAMOUNT : DetailsForm
    {
        #region Declarations
        IAmount_PKOperations _amountPKOperations;
        ISubstance_code_PKOperations _substance_code_PKOperations;
        ISsi__cont_voc_PKOperations _ssi_cont_voc_Operations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;

        private enum PopupFormMode { New, Edit };
        private const string entityType = "Amount";

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
            get { return (int)Session["AMOUNT_id"]; }
            set { Session["AMOUNT_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["AMOUNT_entityOC"]; }
            set { Session["AMOUNT_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["AMOUNT_entityParentOC"]; }
            set { Session["AMOUNT_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["AMOUNT_popupFormMode"]; }
            set { Session["AMOUNT_popupFormMode"] = value; }
        }
        private Amount_PK entity
        {
            get { return (Amount_PK)Session["AMOUNT_entity"]; }
            set { Session["AMOUNT_entity"] = value; }
        }

        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Amount_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";
            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Amount_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.amount_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.amount_PK != null)
                    _id = (int)inEntity.amount_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new Amount_PK();
                SaveForm(_id, null);
                entity.amount_PK = _id;

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

            _substance_code_PKOperations = new Substance_code_PKDAL();
            _ssi_cont_voc_Operations = new Ssi__cont_voc_PKDAL();
            _amountPKOperations = new Amount_PKDAL();
            base.OnInit(e);

        }

        public override object SaveForm(object id, string arg)
        {
            if ((ctlQuantityOper != null)&&(ctlQuantityOper.ControlValue.ToString()!=""))
            {
                entity.quantity = ctlQuantityOper.ControlValue.ToString();
                entity.nonnumericvalue = ctlnonNumValue.ControlValue.ToString();
                if (ctlQuantityOper.ControlValue.ToString() == "107") //"107" = "Range"
                {
                    entity.lownumvalue = ctlLowLimitNumValue.ControlValue.ToString();
                    entity.lownumunit = ctlLowLimitNumUnit.ControlValue.ToString();
                    entity.lownumprefix = ctlLowLimitNumPrefix.ControlValue.ToString();
                    entity.lowdenomvalue = ctlLowLimitDenomValue.ControlValue.ToString();
                    entity.lowdenomunit = ctlLowLimitDenomUnit.ControlValue.ToString();
                    entity.lowdenomprefix = ctlLowLimitDenomPrefix.ControlValue.ToString();
                    entity.highnumvalue = ctlHighLimitNumValue.ControlValue.ToString();
                    entity.highnumunit = ctlHighLimitNumUnit.ControlValue.ToString();
                    entity.highnumprefix = ctlHighLimitNumPrefix.ControlValue.ToString();
                    entity.highdenomvalue = ctlHighLimitDenomValue.ControlValue.ToString();
                    entity.highdenomunit = ctlHighLimitDenomUnit.ControlValue.ToString();
                    entity.highdenomprefix = ctlHighLimitDenomPrefix.ControlValue.ToString();
                }
                else
                {
                    entity.lownumvalue = ctlExactNumValue.ControlValue.ToString();
                    entity.lownumunit = ctlExactNumUnit.ControlValue.ToString();
                    entity.lownumprefix = ctlExactNumPrefix.ControlValue.ToString();
                    entity.lowdenomvalue = ctlExactDenomValue.ControlValue.ToString();
                    entity.lowdenomunit = ctlExactDenomUnit.ControlValue.ToString();
                    entity.lowdenomprefix = ctlExactDenomPrefix.ControlValue.ToString();
                }
            }
            return entity;
        }

        public override void ClearForm(string arg)
        {
            ctlQuantityOper.ControlValue = String.Empty;
            ctlLowLimitNumValue.ControlValue = String.Empty;
            ctlLowLimitNumUnit.ControlValue = String.Empty;
            ctlLowLimitNumPrefix.ControlValue = String.Empty;
            ctlLowLimitDenomValue.ControlValue = String.Empty;
            ctlLowLimitDenomUnit.ControlValue = String.Empty;
            ctlLowLimitDenomPrefix.ControlValue = String.Empty;
            ctlHighLimitDenomPrefix.ControlValue = String.Empty;
            ctlHighLimitDenomUnit.ControlValue = String.Empty;
            ctlHighLimitDenomValue.ControlValue = String.Empty;
            ctlHighLimitNumPrefix.ControlValue = String.Empty;
            ctlHighLimitNumUnit.ControlValue = String.Empty;
            ctlHighLimitNumValue.ControlValue = String.Empty;
            ctlnonNumValue.ControlValue = String.Empty;
            ctlExactDenomPrefix.ControlValue = String.Empty;
            ctlExactDenomUnit.ControlValue = String.Empty;
            ctlExactDenomValue.ControlValue = String.Empty;
            ctlExactNumPrefix.ControlValue = String.Empty;
            ctlExactNumUnit.ControlValue = String.Empty;
            ctlExactNumValue.ControlValue = String.Empty;
            pnlHighLimit.Visible = false;
            pnlLowLimit.Visible = false;
            pnlExactLimit.Visible = false;
        }

        public override void FillDataDefinitions(string arg)
        {
            BindDDLQuantityOper();
            BindDDLLowLimitNumPrefix();
            BindDDLLowLimitNumUnit();
            BindDDLLowLimitDenomUnit();
            BindDDLLowLimitDenomPrefix();
            BindDDLHighLimitNumUnit();
            BindDDLHighLimitNumPrefix();
            BindDDLHighLimitDenomPrefix();
            BindDDLHighLimitDenomUnit();
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;
            if ((((ctlQuantityOper == null) || (ctlQuantityOper.ControlValue.ToString()) == "")) && (String.IsNullOrEmpty(ctlnonNumValue.ControlValue.ToString()))) errorMessage += "Quantity operator or non numeric value must be defined." + "<br />";
            if ((ctlQuantityOper != null) && (ctlQuantityOper.ControlValue.ToString() != ""))
            {
                if (ctlQuantityOper.ControlValue.ToString() == "107") //107 = "Range"
                {
                    if ((!String.IsNullOrEmpty(ctlLowLimitNumValue.ControlValue.ToString())) && (!ValidationHelper.IsValidInt(ctlLowLimitNumValue.ControlValue.ToString()))) errorMessage += ctlLowLimitNumValue.ControlEmptyErrorMessage + "<br />";
                    if ((!String.IsNullOrEmpty(ctlLowLimitDenomValue.ControlValue.ToString())) && (!ValidationHelper.IsValidInt(ctlLowLimitDenomValue.ControlValue.ToString()))) errorMessage += ctlLowLimitDenomValue.ControlEmptyErrorMessage + "<br />";
                    if ((!String.IsNullOrEmpty(ctlHighLimitNumValue.ControlValue.ToString())) && (!ValidationHelper.IsValidInt(ctlHighLimitNumValue.ControlValue.ToString()))) errorMessage += ctlHighLimitNumValue.ControlEmptyErrorMessage + "<br />";
                    if ((!String.IsNullOrEmpty(ctlHighLimitDenomValue.ControlValue.ToString())) && (!ValidationHelper.IsValidInt(ctlHighLimitDenomValue.ControlValue.ToString()))) errorMessage += ctlHighLimitDenomValue.ControlEmptyErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlLowLimitNumValue.ControlValue.ToString())) errorMessage += ctlLowLimitNumValue.ControlErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlLowLimitDenomValue.ControlValue.ToString())) errorMessage += ctlLowLimitDenomValue.ControlErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlHighLimitNumValue.ControlValue.ToString())) errorMessage += ctlHighLimitNumValue.ControlErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlHighLimitDenomValue.ControlValue.ToString())) errorMessage += ctlHighLimitDenomValue.ControlErrorMessage + "<br />";

                    if (ctlLowLimitNumUnit.ControlValue.ToString() == "") errorMessage += ctlLowLimitNumUnit.ControlEmptyErrorMessage + "<br />";
                    if (ctlLowLimitNumPrefix.ControlValue.ToString() == "") errorMessage += ctlLowLimitNumPrefix.ControlEmptyErrorMessage + "<br />";
                    if (ctlHighLimitNumUnit.ControlValue.ToString() == "") errorMessage += ctlHighLimitNumUnit.ControlEmptyErrorMessage + "<br />";
                    if (ctlHighLimitNumPrefix.ControlValue.ToString() == "") errorMessage += ctlHighLimitNumPrefix.ControlEmptyErrorMessage + "<br />";
                    //if (ctlLowLimitDenomUnit.ControlValue.ToString() == "") errorMessage += ctlLowLimitDenomUnit.ControlEmptyErrorMessage + "<br />";
                    //if (ctlLowLimitDenomPrefix.ControlValue.ToString() == "") errorMessage += ctlLowLimitDenomPrefix.ControlEmptyErrorMessage + "<br />";
                    //if (ctlHighLimitDenomUnit.ControlValue.ToString() == "") errorMessage += ctlHighLimitDenomUnit.ControlEmptyErrorMessage + "<br />";
                    //if (ctlHighLimitDenomPrefix.ControlValue.ToString() == "") errorMessage += ctlHighLimitDenomPrefix.ControlEmptyErrorMessage + "<br />";
                }
                else
                {
                    if ((!String.IsNullOrEmpty(ctlExactNumValue.ControlValue.ToString())) && (!ValidationHelper.IsValidInt(ctlExactNumValue.ControlValue.ToString()))) errorMessage += ctlExactNumValue.ControlErrorMessage + "<br />";
                    if ((!String.IsNullOrEmpty(ctlExactDenomValue.ControlValue.ToString())) && (!ValidationHelper.IsValidInt(ctlExactDenomValue.ControlValue.ToString()))) errorMessage += ctlExactDenomValue.ControlErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlExactNumValue.ControlValue.ToString())) errorMessage += ctlExactNumValue.ControlEmptyErrorMessage + "<br />";
                    //if (String.IsNullOrEmpty(ctlExactDenomValue.ControlValue.ToString())) errorMessage += ctlExactDenomValue.ControlEmptyErrorMessage + "<br />";
                    
                    //if (ctlExactDenomUnit.ControlValue.ToString() == "") errorMessage += ctlExactDenomUnit.ControlEmptyErrorMessage + "<br />";
                    //if (ctlExactDenomPrefix.ControlValue.ToString() == "") errorMessage += ctlExactDenomPrefix.ControlEmptyErrorMessage + "<br />";
                    if (ctlExactNumUnit.ControlValue.ToString() == "") errorMessage += ctlExactNumUnit.ControlEmptyErrorMessage + "<br />";
                    if (ctlExactNumPrefix.ControlValue.ToString() == "") errorMessage += ctlExactNumPrefix.ControlEmptyErrorMessage + "<br />";
                }
            }
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            return true;
        }

        private void BindDDLQuantityOper()
        {
            ctlQuantityOper.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_Operations.GetEntitiesByListName("Quantity Operator");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlQuantityOper.SourceValueProperty = "ssi__cont_voc_PK";
            ctlQuantityOper.SourceTextExpression = "term_name_english";
            ctlQuantityOper.FillControl<Ssi__cont_voc_PK>(items);
            pnlHighLimit.Visible = (ctlQuantityOper.ControlValue.ToString() == "107") ? true : false;
        }
        private void BindDDLLowLimitNumPrefix()
        {
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_Operations.GetEntitiesByListName("Unit Prefix");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });
            ctlLowLimitNumPrefix.ControlBoundItems.Clear();
            ctlExactNumPrefix.ControlBoundItems.Clear();
            ctlExactNumPrefix.SourceValueProperty = ctlLowLimitNumPrefix.SourceValueProperty = "term_name_english";
            ctlExactNumPrefix.SourceTextExpression = ctlLowLimitNumPrefix.SourceTextExpression = "Description";
            ctlLowLimitNumPrefix.FillControl<Ssi__cont_voc_PK>(items);
            ctlExactNumPrefix.FillControl<Ssi__cont_voc_PK>(items);
        }
        private void BindDDLLowLimitNumUnit()
        {
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_Operations.GetEntitiesByListName("Units of Measure");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });
            ctlExactNumUnit.ControlBoundItems.Clear();
            ctlLowLimitNumUnit.ControlBoundItems.Clear();
            ctlExactNumUnit.SourceValueProperty = ctlLowLimitNumUnit.SourceValueProperty = "term_name_english";
            ctlExactNumUnit.SourceTextExpression = ctlLowLimitNumUnit.SourceTextExpression = "Description";
            ctlLowLimitNumUnit.FillControl<Ssi__cont_voc_PK>(items);
            ctlExactNumUnit.FillControl<Ssi__cont_voc_PK>(items);
        }   
        private void BindDDLLowLimitDenomPrefix()
        {
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_Operations.GetEntitiesByListName("Unit Prefix");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });
            ctlExactDenomPrefix.ControlBoundItems.Clear();
            ctlLowLimitDenomPrefix.ControlBoundItems.Clear();
            ctlExactDenomPrefix.SourceValueProperty = ctlLowLimitDenomPrefix.SourceValueProperty = "term_name_english";
            ctlExactDenomPrefix.SourceTextExpression = ctlLowLimitDenomPrefix.SourceTextExpression = "Description";
            ctlLowLimitDenomPrefix.FillControl<Ssi__cont_voc_PK>(items);
            ctlExactDenomPrefix.FillControl<Ssi__cont_voc_PK>(items);
        }
        private void BindDDLLowLimitDenomUnit()
        {
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_Operations.GetEntitiesByListName("Units of Measure");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });
            ctlExactDenomUnit.ControlBoundItems.Clear(); 
            ctlLowLimitDenomUnit.ControlBoundItems.Clear();
            ctlExactDenomUnit.SourceValueProperty = ctlLowLimitDenomUnit.SourceValueProperty = "term_name_english";
            ctlExactDenomUnit.SourceTextExpression = ctlLowLimitDenomUnit.SourceTextExpression = "Description";
            ctlLowLimitDenomUnit.FillControl<Ssi__cont_voc_PK>(items);
            ctlExactDenomUnit.FillControl<Ssi__cont_voc_PK>(items);
        }
        private void BindDDLHighLimitNumPrefix()
        {
            ctlHighLimitNumPrefix.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_Operations.GetEntitiesByListName("Unit Prefix");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlHighLimitNumPrefix.SourceValueProperty = "term_name_english";
            ctlHighLimitNumPrefix.SourceTextExpression = "Description";
            ctlHighLimitNumPrefix.FillControl<Ssi__cont_voc_PK>(items);
        }
        private void BindDDLHighLimitNumUnit()
        {
            ctlHighLimitNumUnit.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_Operations.GetEntitiesByListName("Units of Measure");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlHighLimitNumUnit.SourceValueProperty = "term_name_english";
            ctlHighLimitNumUnit.SourceTextExpression = "Description";
            ctlHighLimitNumUnit.FillControl<Ssi__cont_voc_PK>(items);
        }
        private void BindDDLHighLimitDenomPrefix()
        {
            ctlHighLimitDenomPrefix.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_Operations.GetEntitiesByListName("Unit Prefix");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlHighLimitDenomPrefix.SourceValueProperty = "term_name_english";
            ctlHighLimitDenomPrefix.SourceTextExpression = "Description";
            ctlHighLimitDenomPrefix.FillControl<Ssi__cont_voc_PK>(items);
        }
        private void BindDDLHighLimitDenomUnit()
        {
            ctlHighLimitDenomUnit.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_Operations.GetEntitiesByListName("Units of Measure");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlHighLimitDenomUnit.SourceValueProperty = "term_name_english";
            ctlHighLimitDenomUnit.SourceTextExpression = "Description";
            ctlHighLimitDenomUnit.FillControl<Ssi__cont_voc_PK>(items);
        }

        public override void BindForm(object id, string arg)
        {
            if (popupFormMode == PopupFormMode.Edit)
            {
                ctlQuantityOper.ControlValue = entity.quantity;
                ctlnonNumValue.ControlValue = entity.nonnumericvalue;
                if (entity.quantity == "107") //"Range"
                {
                    ctlLowLimitNumValue.ControlValue = entity.lownumvalue;
                    ctlLowLimitNumUnit.ControlValue = entity.lownumunit;
                    ctlLowLimitNumPrefix.ControlValue = entity.lownumprefix;
                    ctlLowLimitDenomValue.ControlValue = entity.lowdenomvalue;
                    ctlLowLimitDenomUnit.ControlValue = entity.lowdenomunit;
                    ctlLowLimitDenomPrefix.ControlValue = entity.lowdenomprefix;
                    ctlHighLimitDenomPrefix.ControlValue = entity.highdenomprefix;
                    ctlHighLimitDenomUnit.ControlValue = entity.highdenomunit;
                    ctlHighLimitDenomValue.ControlValue = entity.highdenomvalue;
                    ctlHighLimitNumPrefix.ControlValue = entity.highnumprefix;
                    ctlHighLimitNumUnit.ControlValue = entity.highnumunit;
                    ctlHighLimitNumValue.ControlValue = entity.highnumvalue;
                    pnlLowLimit.Visible = true;
                    pnlHighLimit.Visible = true;
                }
                else
                {
                    ctlExactNumValue.ControlValue = entity.lownumvalue;
                    ctlExactNumUnit.ControlValue = entity.lownumunit;
                    ctlExactNumPrefix.ControlValue = entity.lownumprefix;
                    ctlExactDenomValue.ControlValue = entity.lowdenomvalue;
                    ctlExactDenomUnit.ControlValue = entity.lowdenomunit;
                    ctlExactDenomPrefix.ControlValue = entity.lowdenomprefix;
                    pnlExactLimit.Visible = true;
                }
            }
        }


        #endregion

        #region Form methods

        public void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateForm(null))
            {
                SaveForm(_id, "");
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

        #region PopupForms

        public void ctlQuantityOnInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            ListItemCollection lic = ctlQuantityOper.ControlBoundItems;
            // TODO: postaviti svim panelima Visible = false;
            pnlHighLimit.Visible = false;
            string selectedItem = "";
            foreach (ListItem item in lic)
            {
                if (item.Selected) selectedItem = item.Value;
            }

            if (selectedItem == "107")
            {
                //infoConcTypeText1.Visible = true;
                //infoConcTypeText2.Visible = false;
                pnlLowLimit.Visible = true;
                pnlHighLimit.Visible = true;
                pnlExactLimit.Visible = false;
                //infoConcType.InnerText = " " + concType + " ";

            }
            else if (selectedItem == "")
            {
                pnlLowLimit.Visible = false;
                pnlHighLimit.Visible = false;
                pnlExactLimit.Visible = false;
            }
            else
            {
                pnlExactLimit.Visible = true;
                pnlLowLimit.Visible = false;
                pnlHighLimit.Visible = false;
                //infoConcType.InnerText = " " + concType + " ";
            }

            //ClearFullDescription();
        }
        #endregion

        #region Exact handlers
        public void ExactDenUnit_Changed(object sender, ValueChangedEventArgs e)
        {
            //if (ctlConTypeCode.ControlTextValue != "Range")
            //{
            //    infoUnitText.Visible = true;
            //    infoMeasure.InnerHtml = ctlExactDenUnit != null ? ctlExactDenUnit.ControlTextValue : "";
            //    if (ctlExactDenUnit.ControlValue.ToString() != "")
            //    {
            //        Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlExactDenUnit.ControlValue));
            //        if (ctlExpressedAs.ControlTextValue == "Units of Measure")
            //        {
            //            infoDenExp2.InnerText = ssiTmp != null ? ssiTmp.term_name_english != "" ? "/" + ssiTmp.term_name_english : "" : "";
            //        }
            //        else
            //        {
            //            infoDenExp2.InnerText = ssiTmp != null ? ssiTmp.Description != "" ? "/" + ssiTmp.Description : "" : "";
            //        }
            //    }
            //}
        }

        public void ExactDenPrefix_Changed(object sender, ValueChangedEventArgs e)
        {
        }

        public void ExactDenVal_Changed(object sender, ValueChangedEventArgs e)
        {
        }

        public void ExactNumUnit_Changed(object sender, ValueChangedEventArgs e)
        {
            //if (ctlConTypeCode.ControlTextValue != "Range")
            //{
            //    infoUnit.InnerText = ctlExactNumUnit.ControlTextValue;
            //    if (ctlExactNumUnit.ControlValue.ToString() != "")
            //    {
            //        Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlExactNumUnit.ControlValue));
            //        infoUnit2.InnerText = ssiTmp != null ? ssiTmp.term_name_english != "" ? ssiTmp.term_name_english : "" : "";
            //    }
            //    //infoUnit2.InnerText = " " + ctlExactNumUnit.ControlTextValue;
            //}
        }

        public void ExactNumPrefix_Changed(object sender, ValueChangedEventArgs e)
        {
            //if (ctlConTypeCode.ControlTextValue != "Range")
            //{
            //    if (ctlExactNumPrefix.ControlValue.ToString() != "")
            //    {
            //        Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlExactNumPrefix.ControlValue));
            //        int indexOfFirtOccur = ssiTmp.Description.IndexOf('(') != -1 ? ssiTmp.Description.IndexOf('(') : ssiTmp.Description.Length;
            //        int end = ssiTmp.Description.Length - indexOfFirtOccur;
                    //infoNumPrefix.InnerText = ssiTmp != null ? ssiTmp.Description != "" ? " " + ssiTmp.Description.Remove(indexOfFirtOccur, end) : "" : "";
                    //infoNumPrefix2.InnerText = ssiTmp != null ? ssiTmp.term_name_english != "" ? " " + ssiTmp.term_name_english : "" : "";
                //}
                //infoNumPrefix.InnerHtml = " " + ctlExactNumPrefix.ControlTextValue + " ";
            //}
        }

        public void ExactNumVal_Changed(object sender, ValueChangedEventArgs e)
        {
            //pnlInfo.Controls.Add(new LiteralControl('<a href'));  
            //if (ctlConTypeCode.ControlTextValue != "Range")
            //{
            //    infoConcTypeText2.Visible = true;
            //    infoConcTypeText1.Visible = false;
            //    infoRangeMin.InnerHtml = " " + ctlExactNumVal.ControlTextValue + " ";
            //    infoRangeMin2.InnerHtml = " " + ctlExactNumVal.ControlTextValue + " ";
            //}
        }


        #endregion 

        #region High limit handlers
        public void HighLimitNumVal_Changed(object sender, ValueChangedEventArgs e)
        {
            //pnlInfo.Controls.Add(new LiteralControl('<a href'));  
            //info.InnerHtml = ctlLowLimitNumVal.ControlValue.ToString();
            //infoRangeText.Visible = true;
            //infoRangeMax.InnerHtml = ctlHighLimitNumVal.ControlValue.ToString();
            //infoRangeMax2.InnerHtml = " - " + ctlHighLimitNumVal.ControlValue.ToString();
        }

        public void HighLimitNumPrefix_Changed(object sender, ValueChangedEventArgs e)
        {
            //pnlInfo.Controls.Add(new LiteralControl('<a href'));  
            //info.InnerHtml = ctlLowLimitNumVal.ControlValue.ToString();

            //if (ctlConTypeCode.ControlTextValue == "Range")
            //{
            //    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlHighLimitNumPrefix.ControlValue));
            //    int indexOfFirtOccur = ssiTmp.Description.IndexOf('(') != -1 ? ssiTmp.Description.IndexOf('(') : ssiTmp.Description.Length;
            //    int end = ssiTmp.Description.Length - indexOfFirtOccur;
            //    infoNumPrefix.InnerText = ssiTmp != null ? ssiTmp.Description != "" ? " " + ssiTmp.Description.Remove(indexOfFirtOccur, end) : "" : "";
            //    infoNumPrefix2.InnerText = ssiTmp != null ? ssiTmp.Description != "" ? " " + ssiTmp.Description.Remove(indexOfFirtOccur, end) : "" : "";
            //    //infoNumPrefix.InnerHtml = " " + ctlExactNumPrefix.ControlTextValue + " ";
            //}
        }

        public void HighLimitNumUnit_Changed(object sender, ValueChangedEventArgs e)
        {
            //pnlInfo.Controls.Add(new LiteralControl('<a href'));  
            //info.InnerHtml = ctlLowLimitNumVal.ControlValue.ToString();
        }

        public void HighLimitDenVal_Changed(object sender, ValueChangedEventArgs e)
        {
            //pnlInfo.Controls.Add(new LiteralControl('<a href'));  
            //info.InnerHtml = ctlLowLimitNumVal.ControlValue.ToString();
        }

        public void HighLimitDenPrefix_Changed(object sender, ValueChangedEventArgs e)
        {
            //pnlInfo.Controls.Add(new LiteralControl('<a href'));  
            //info.InnerHtml = ctlLowLimitNumVal.ControlValue.ToString();
        }

        public void HighLimitDenUnit_Changed(object sender, ValueChangedEventArgs e)
        {
            //pnlInfo.Controls.Add(new LiteralControl('<a href'));  
            //info.InnerHtml = ctlLowLimitNumVal.ControlValue.ToString();
        }

        #endregion

        #region Low limit handlers
        public void LowLimitNumVal_Changed(object sender, ValueChangedEventArgs e)
        {
            //pnlInfo.Controls.Add(new LiteralControl('<a href'));  
            //infoRangeMin.InnerHtml = ctlLowLimitNumVal.ControlValue.ToString();
            //infoRangeMin2.InnerHtml = " " + ctlLowLimitNumVal.ControlValue.ToString();
            //infoRangeMinText.Visible = true;
        }

        public void LowLimitNumPrefix_Changed(object sender, ValueChangedEventArgs e)
        {

            //if (ctlConTypeCode.ControlTextValue == "Range")
            //{
            //    if (ctlLowLimitNumPrefix.ControlValue.ToString() != "")
            //    {
            //        Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlLowLimitNumPrefix.ControlValue));
            //        int indexOfFirtOccur = ssiTmp.Description.IndexOf('(') != -1 ? ssiTmp.Description.IndexOf('(') : ssiTmp.Description.Length;
            //        int end = ssiTmp.Description.Length - indexOfFirtOccur;
            //        infoNumPrefix.InnerText = ssiTmp != null ? ssiTmp.Description != "" ? " " + ssiTmp.Description.Remove(indexOfFirtOccur, end) : "" : "";
            //        infoNumPrefix2.InnerText = ssiTmp != null ? ssiTmp.term_name_english != "" ? " " + ssiTmp.term_name_english : "" : "";
            //        //infoNumPrefix.InnerHtml = " " + ctlExactNumPrefix.ControlTextValue + " ";
            //    }
            //}
            //pnlInfo.Controls.Add(new LiteralControl('<a href'));  
            //info.InnerHtml = ctlLowLimitNumVal.ControlValue.ToString();
        }

        public void LowLimitNumUnit_Changed(object sender, ValueChangedEventArgs e)
        {
            //pnlInfo.Controls.Add(new LiteralControl('<a href'));  
            //info.InnerHtml = ctlLowLimitNumVal.ControlValue.ToString();
            //if (ctlConTypeCode.ControlTextValue == "Range")
            //{
            //    if (ctlLowLimitNumUnit.ControlValue.ToString() != "")
            //    {
            //        infoUnit.InnerText = ctlLowLimitNumUnit.ControlTextValue;
            //        Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlLowLimitNumUnit.ControlValue));
            //        infoUnit2.InnerText = ssiTmp != null ? ssiTmp.term_name_english != "" ? ssiTmp.term_name_english : "" : "";
            //        //infoUnit2.InnerText = " " + ctlLowLimitNumUnit.ControlTextValue;
            //    }
            //}
        }

        public void LowLimitDenVal_Changed(object sender, ValueChangedEventArgs e)
        {
            //pnlInfo.Controls.Add(new LiteralControl('<a href'));  
            //info.InnerHtml = ctlLowLimitNumVal.ControlValue.ToString();
            //infoUnitText.Visible = true;
            //infoDenValue.InnerText = " " + ctlLowLimitDenVal.ControlTextValue + " ";
            //infoDenValue2.InnerText = "/" + ctlLowLimitDenVal.ControlTextValue + " ";
        }

        public void LowLimitDenPrefix_Changed(object sender, ValueChangedEventArgs e)
        {
            //pnlInfo.Controls.Add(new LiteralControl('<a href'));  
            //info.InnerHtml = ctlLowLimitNumVal.ControlValue.ToString();
            //if (ctlLowLimitDenPrefix.ControlValue.ToString() != "")
            //{
            //    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlLowLimitDenPrefix.ControlValue));
            //    int indexOfFirtOccur = ssiTmp.Description.IndexOf('(') != -1 ? ssiTmp.Description.IndexOf('(') : ssiTmp.Description.Length;
            //    int end = ssiTmp.Description.Length - indexOfFirtOccur;
            //    infoDenPrefix.InnerText = ssiTmp != null ? ssiTmp.Description != "" ? " " + ssiTmp.Description.Remove(indexOfFirtOccur, end) : "" : "";
            //    infoDenPrefix2.InnerText = ssiTmp != null ? ssiTmp.term_name_english != "" ? ssiTmp.term_name_english : "" : "";
            //}
        }

        public void LowLimitDenUnit_Changed(object sender, ValueChangedEventArgs e)
        {
            //pnlInfo.Controls.Add(new LiteralControl('<a href'));  
            //info.InnerHtml = ctlLowLimitNumVal.ControlValue.ToString();
            //if (ctlConTypeCode.ControlTextValue == "Range")
            //{
            //    if (ctlLowLimitDenUnit.ControlValue.ToString() != "")
            //    {
            //        infoDenExp.InnerHtml = " " + ctlLowLimitDenUnit.ControlTextValue;
            //        Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlLowLimitDenUnit.ControlValue));
            //        infoDenExp2.InnerText = ssiTmp != null ? ssiTmp.term_name_english != "" ? ssiTmp.term_name_english : "" : "";
            //        //infoDenExp2.InnerText = " " + ctlLowLimitDenUnit.ControlTextValue;
            //    }
            //}
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