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
using System.Text;
using System.Configuration;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupForm : DetailsForm
    {
        IAdjuvant_PKOperations _adjuvant_PKOperations;
        IExcipient_PKOperations _excipient_PKOPerations;
        IActiveingredient_PKOperations _activeIngredieent_PKOperations;
        ISubstance_PKOperations _substance_PKOperations;
        ISsi__cont_voc_PKOperations _ssi_cont_voc_PKOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCloseButtonClick;

        public string callbackInvocation = null;

        public enum ucPopupFormType
        {
            ActiveIngredient,
            Excipient,
            Adjuvant
        };

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


        private ucPopupFormType FormType
        {

            get { return ViewState["FormType"] != null ? (ucPopupFormType)ViewState["FormType"] : ucPopupFormType.ActiveIngredient; }
            set { ViewState["FormType"] = value; }
        }

        private int? _pp_PK
        {
            get { return (int?)ViewState["ucPopupFormCP_contractID"]; }
            set { ViewState["ucPopupFormCP_contractID"] = value; }
        }
        private int? _entity_PK
        {
            get { return (int?)ViewState["ucPopupFormCP_contractPayoutID"]; }
            set { ViewState["ucPopupFormCP_contractPayoutID"] = value; }
        }

        private object _entityData
        {
            get { return ViewState["ucPopupForm_entityData"] != null ? (object)ViewState["ucPopupForm_entityData"] : null; }
            set { ViewState["ucPopupForm_entityData"] = value; }
        }

        #endregion

        #region Operations

        public void ShowModalForm(int? entity_PK, int? pp_PK, object data, ucPopupFormType formType)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";


            _entity_PK = entity_PK;
            _pp_PK = pp_PK;


            FormType = formType;


            switch (formType)
            {

                case ucPopupFormType.Adjuvant:
                    infoType.InnerText = "Adjuvant";
                    divHeader.InnerText = "Adjuvant";
                    pnlAdjuvants.Visible = true;
                    _entityData = data ?? new List<Adjuvant_PK>();
                    break;

                case ucPopupFormType.Excipient:
                    infoType.InnerText = "Excipient";
                    divHeader.InnerText = "Excipient";
                    pnlAdjuvants.Visible = true;
                    _entityData = data ?? new List<Excipient_PK>();
                    ctlConTypeCode.Visible = false;
                    ctlExpressedAs.Visible = false;

                    ctlConTypeCode.IsMandatory = false;
                    ctlHighLimitNumUnit.IsMandatory = false;
                    ctlHighLimitDenUnit.IsMandatory = false;
                    ctlHighLimitNumPrefix.IsMandatory = false;
                    ctlHighLimitDenPrefix.IsMandatory = false;
                    ctlLowLimitNumUnit.IsMandatory = false;
                    ctlLowLimitDenUnit.IsMandatory = false;
                    ctlLowLimitNumPrefix.IsMandatory = false;
                    ctlLowLimitDenPrefix.IsMandatory = false;
                    ctlHighLimitNumVal.IsMandatory = false;
                    ctlHighLimitDenVal.IsMandatory = false;
                    ctlLowLimitNumVal.IsMandatory = false;
                    ctlLowLimitDenVal.IsMandatory = false;
                    // infoRangeNText.Visible = false;
                    break;

                case ucPopupFormType.ActiveIngredient:
                    infoType.InnerText = "Active Ingredient";
                    divHeader.InnerText = "Active Ingredient";
                    pnlAdjuvants.Visible = true;
                    _entityData = data ?? new List<Activeingredient_PK>();
                    break;
            }
            ctlUcPopupVisible.Value = "true";
            BindForm(_entity_PK, null);
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            _activeIngredieent_PKOperations = new Activeingredient_PKDAL();
            _adjuvant_PKOperations = new Adjuvant_PKDAL();
            _excipient_PKOPerations = new Excipient_PKDAL();
            _substance_PKOperations = new Substance_PKDAL();
            _ssi_cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();


            base.OnInit(e);

            string applicationURL = ConfigurationManager.AppSettings["ApplicationURL"];
            string applicationURLSecure = ConfigurationManager.AppSettings["ApplicationURLSecure"];
            string appVirtualPath = ConfigurationManager.AppSettings["AppVirtualPath"];

            string URL = Request.Url.Authority; // default value if none of config section is present
            string scheme = Request.Url.Scheme;

            if (scheme == "http") URL = applicationURL;
            if (scheme == "https") URL = applicationURLSecure;

            string applicationURLWithScheme = scheme + "://" + URL + appVirtualPath;
            callbackInvocation = applicationURLWithScheme + @"/Services/ucPopup.ashx";

            // bind JS file
            ClientScriptManager mgr = Page.ClientScript;
            Type cstype = this.GetType();
            if (!mgr.IsStartupScriptRegistered(cstype, "ucPopupInitScript"))
            {
                ScriptManager.RegisterStartupScript(this.Page, cstype, "ucPopupInitScript", "ucPopupInitialize(null,null);", true);
            }

            if (!mgr.IsClientScriptIncludeRegistered(typeof(MasterMain),"associatedJS"))
            {
                String url = mgr.GetWebResourceUrl(typeof(MasterMain), "AspNetUI.Scripts.ucPopup.js");
                ScriptManager.RegisterClientScriptInclude(this,typeof(MasterMain), "associatedJS", url);
            }
     
            if (!mgr.IsClientScriptBlockRegistered(cstype, "callbackInovcation"))
            {
                ScriptManager.RegisterClientScriptBlock(this, cstype, "callbackInovcation", "var ucPopupInvokeCallback=\"" + callbackInvocation + "\";", true);
            }

        }

        public override object SaveForm(object id, string arg)
        {

            string concType = ctlConTypeCode.ControlValue.ToString() != "" ? _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlConTypeCode.ControlValue)).term_name_english : "";

            switch (FormType)
            {

                case ucPopupFormType.Adjuvant:


                    if (id != null && id is int)
                    {

                        ((List<Adjuvant_PK>)_entityData).RemoveAll(item => item != null && item.adjuvant_PK == (int)id);
                    }

                    Adjuvant_PK adjuvant = new Adjuvant_PK();

                    int? minAdjPK = ((List<Adjuvant_PK>)_entityData).Min(item => item.adjuvant_PK);
                    minAdjPK = minAdjPK != null && minAdjPK < 0 ? --minAdjPK : -1;

                    if (concType == "Range")
                    {
                        adjuvant.adjuvant_PK = minAdjPK;
                        adjuvant.pp_FK = null;

                        adjuvant.substancecode_FK = ValidationHelper.IsValidInt(ctlSubstance_PK.Value.ToString()) ? (int?)Convert.ToInt32(ctlSubstance_PK.Value.ToString()) : null;
                        adjuvant.concentrationtypecode = ValidationHelper.IsValidInt(ctlConTypeCode.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlConTypeCode.ControlValue) : null;
                        adjuvant.ExpressedBy_FK = ValidationHelper.IsValidInt(ctlExpressedAs.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlExpressedAs.ControlValue) : null;

                        string conciseAdj = infoConSubName.InnerText + infoConRangeNMin.InnerText + infoConNumMinPrefix.InnerText + infoConRangeNMax.InnerText + infoConNumMaxPrefix.InnerText + infoConNUnit.InnerText +
                                         infoMeasure2.InnerText + infoConRangeDMin.InnerText + infoConRangeDMax.InnerText + infoConDenPrefix.InnerText + infoConDenExp.InnerText.Trim();
                        adjuvant.concise = String.IsNullOrWhiteSpace(conciseAdj) ? null : conciseAdj.Trim();

                        adjuvant.higamountnumerunit = ctlHighLimitNumUnit.ControlValue.ToString();
                        adjuvant.highamountdenomunit = ctlHighLimitDenUnit.ControlValue.ToString();
                        adjuvant.highamountnumerprefix = ctlHighLimitNumPrefix.ControlValue.ToString();
                        adjuvant.highamountdenomprefix = ctlHighLimitDenPrefix.ControlValue.ToString();

                        adjuvant.lowamountnumerunit = ctlLowLimitNumUnit.ControlValue.ToString();
                        adjuvant.lowamountdenomunit = ctlLowLimitDenUnit.ControlValue.ToString();
                        adjuvant.lowamountnumerprefix = ctlLowLimitNumPrefix.ControlValue.ToString();
                        adjuvant.lowamountdenomprefix = ctlLowLimitDenPrefix.ControlValue.ToString();

                        adjuvant.highamountnumervalue = ValidationHelper.IsValidDecimal(ctlHighLimitNumVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlHighLimitNumVal.ControlTextValue) : null;
                        adjuvant.highamountdenomvalue = ValidationHelper.IsValidDecimal(ctlHighLimitDenVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlHighLimitDenVal.ControlTextValue) : null;
                        adjuvant.lowamountnumervalue = ValidationHelper.IsValidDecimal(ctlLowLimitNumVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlLowLimitNumVal.ControlTextValue) : null;
                        adjuvant.lowamountdenomvalue = ValidationHelper.IsValidDecimal(ctlLowLimitDenVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlLowLimitDenVal.ControlTextValue) : null;

                        adjuvant.userID = SessionManager.Instance.CurrentUser.UserID;

                        ((List<Adjuvant_PK>)_entityData).Add(adjuvant);
                    }
                    else
                    {
                        adjuvant.adjuvant_PK = minAdjPK;
                        adjuvant.pp_FK = null;
                        adjuvant.substancecode_FK = ValidationHelper.IsValidInt(ctlSubstance_PK.Value.ToString()) ? (int?)Convert.ToInt32(ctlSubstance_PK.Value.ToString()) : null;
                        adjuvant.concentrationtypecode = ValidationHelper.IsValidInt(ctlConTypeCode.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlConTypeCode.ControlValue) : null;
                        adjuvant.ExpressedBy_FK = ValidationHelper.IsValidInt(ctlExpressedAs.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlExpressedAs.ControlValue) : null;

                        string conciseAdj = infoConSubName.InnerText + infoConRangeNMin.InnerText + infoConRangeNMax.InnerText + infoConNumMaxPrefix.InnerText + infoConNUnit.InnerText +
                                         infoMeasure2.InnerText + infoConRangeDMin.InnerText + infoConRangeDMax.InnerText + infoConDenPrefix.InnerText + infoConDenExp.InnerText.Trim();
                        adjuvant.concise = String.IsNullOrWhiteSpace(conciseAdj) ? null : conciseAdj.Trim();

                        adjuvant.lowamountnumerunit = ctlExactNumUnit.ControlValue.ToString();
                        adjuvant.lowamountdenomunit = ctlExactDenUnit.ControlValue.ToString();
                        adjuvant.lowamountnumerprefix = ctlExactNumPrefix.ControlValue.ToString();
                        adjuvant.lowamountdenomprefix = ctlExactDenPrefix.ControlValue.ToString();

                        adjuvant.lowamountnumervalue = ValidationHelper.IsValidDecimal(ctlExactNumVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlExactNumVal.ControlTextValue) : null;
                        adjuvant.lowamountdenomvalue = ValidationHelper.IsValidDecimal(ctlExactDenVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlExactDenVal.ControlTextValue) : null;

                        adjuvant.userID = SessionManager.Instance.CurrentUser.UserID;
                        ((List<Adjuvant_PK>)_entityData).Add(adjuvant);
                    }


                    return adjuvant;
                case ucPopupFormType.Excipient:


                    if (id != null && id is int)
                    {

                        ((List<Excipient_PK>)_entityData).RemoveAll(item => item != null && item.excipient_PK == (int)id);
                    }

                    Excipient_PK excipient = new Excipient_PK();

                    int? minExcPK = ((List<Excipient_PK>)_entityData).Min(item => item.excipient_PK);
                    minExcPK = minExcPK != null && minExcPK < 0 ? --minExcPK : -1;
                    /*
                    if (concType == "Range")
                    {
                        excipient.pp_FK = null;
                        excipient.substancecode_FK = ValidationHelper.IsValidInt(SubNameSearcherDisplay.SelectedObject.ToString()) ? (int?)SubNameSearcherDisplay.SelectedObject : null;
                        excipient.concentrationtypecode = ValidationHelper.IsValidInt(ctlConTypeCode.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlConTypeCode.ControlValue) : null;
                        excipient.ExpressedBy_FK = ValidationHelper.IsValidInt(ctlExpressedAs.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlExpressedAs.ControlValue) : null;

                        string conciseExc = infoConSubName.InnerText + infoConRangeNMin.InnerText + infoConRangeNMax.InnerText + infoConNPrefix.InnerText + infoConNUnit.InnerText +
                                         infoMeasure2.InnerText + infoConRangeDMin.InnerText + infoConRangeDMax.InnerText + infoConDenPrefix.InnerText + infoConDenExp.InnerText;
                        excipient.concise = String.IsNullOrWhiteSpace(conciseExc) ? null : conciseExc.Trim();

                        excipient.higamountnumerunit = ctlHighLimitNumUnit.ControlValue.ToString();
                        excipient.highamountdenomunit = ctlHighLimitDenUnit.ControlValue.ToString();
                        excipient.highamountnumerprefix = ctlHighLimitNumPrefix.ControlValue.ToString();
                        excipient.highamountdenomprefix = ctlHighLimitDenPrefix.ControlValue.ToString();

                        excipient.lowamountnumerunit = ctlLowLimitNumUnit.ControlValue.ToString();
                        excipient.lowamountdenomunit = ctlLowLimitDenUnit.ControlValue.ToString();
                        excipient.lowamountnumerprefix = ctlLowLimitNumPrefix.ControlValue.ToString();
                        excipient.lowamountdenomprefix = ctlLowLimitDenPrefix.ControlValue.ToString();

                        excipient.highamountnumervalue = ValidationHelper.IsValidDecimal(ctlHighLimitNumVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlHighLimitNumVal.ControlTextValue) : null;
                        excipient.highamountdenomvalue = ValidationHelper.IsValidDecimal(ctlHighLimitDenVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlHighLimitDenVal.ControlTextValue) : null;
                        excipient.lowamountnumervalue = ValidationHelper.IsValidDecimal(ctlLowLimitNumVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlLowLimitNumVal.ControlTextValue) : null;
                        excipient.lowamountdenomvalue = ValidationHelper.IsValidDecimal(ctlLowLimitDenVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlLowLimitDenVal.ControlTextValue) : null;
                        excipient.userID = SessionManager.Instance.CurrentUser.UserID;

                        excipient = _excipient_PKOPerations.Save(excipient);

                        ppExcipients.Add((int)excipient.excipient_PK);
                    }
                    else
                    {*/
                    excipient.excipient_PK = minExcPK;
                    excipient.pp_FK = null;
                    excipient.substancecode_FK = ValidationHelper.IsValidInt(ctlSubstance_PK.Value.ToString()) ? (int?)Convert.ToInt32(ctlSubstance_PK.Value.ToString()) : null;
                    /*excipient.concentrationtypecode = ValidationHelper.IsValidInt(ctlConTypeCode.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlConTypeCode.ControlValue) : null;
                    excipient.ExpressedBy_FK = ValidationHelper.IsValidInt(ctlExpressedAs.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlExpressedAs.ControlValue) : null;
                    */
                    string conciseExc = infoConSubName.InnerText + infoConRangeNMin.InnerText + infoConRangeNMax.InnerText + infoConNumMaxPrefix.InnerText + infoConNUnit.InnerText +
                                     infoMeasure2.InnerText + infoConRangeDMin.InnerText + infoConRangeDMax.InnerText + infoConDenPrefix.InnerText + infoConDenExp.InnerText;
                    excipient.concise = String.IsNullOrWhiteSpace(conciseExc) ? null : conciseExc.Trim();
                    /*
                    excipient.lowamountnumerunit = ctlExactNumUnit.ControlValue.ToString();
                    excipient.lowamountdenomunit = ctlExactDenUnit.ControlValue.ToString();
                    excipient.lowamountnumerprefix = ctlExactNumPrefix.ControlValue.ToString();
                    excipient.lowamountdenomprefix = ctlExactDenPrefix.ControlValue.ToString();

                    excipient.lowamountnumervalue = ValidationHelper.IsValidDecimal(ctlExactNumVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlExactNumVal.ControlTextValue) : null;
                    excipient.lowamountdenomvalue = ValidationHelper.IsValidDecimal(ctlExactDenVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlExactDenVal.ControlTextValue) : null;
                    */
                    excipient.userID = SessionManager.Instance.CurrentUser.UserID;

                    ((List<Excipient_PK>)_entityData).Add(excipient);
                    //}


                    return excipient;

                case ucPopupFormType.ActiveIngredient:


                    if (id != null && id is int)
                    {

                        ((List<Activeingredient_PK>)_entityData).RemoveAll(item => item != null && item.activeingredient_PK == (int)id);
                    }

                    Activeingredient_PK ingredient = new Activeingredient_PK();

                    int? minIngPK = ((List<Activeingredient_PK>)_entityData).Min(item => item.activeingredient_PK);
                    minIngPK = minIngPK != null && minIngPK < 0 ? --minIngPK : -1;

                    if (concType == "Range")
                    {
                        ingredient.activeingredient_PK = minIngPK;
                        ingredient.pp_FK = null;
                        ingredient.substancecode_FK = ValidationHelper.IsValidInt(ctlSubstance_PK.Value.ToString()) ? (int?)Convert.ToInt32(ctlSubstance_PK.Value.ToString()) : null;
                        ingredient.concentrationtypecode = ValidationHelper.IsValidInt(ctlConTypeCode.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlConTypeCode.ControlValue) : null;
                        ingredient.ExpressedBy_FK = ValidationHelper.IsValidInt(ctlExpressedAs.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlExpressedAs.ControlValue) : null;

                        //ingredient.strength_value = ValidationHelper.IsValidInt(ctlStrengthValue.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlStrengthValue.ControlValue) : null;
                        //ingredient.strength_unit = ctlStrengthUnit.ControlValue.ToString();

                        string conciseIng = infoConSubName.InnerText + infoConRangeNMin.InnerText + infoConNumMinPrefix.InnerText + infoConRangeNMax.InnerText + infoConNumMaxPrefix.InnerText + infoConNUnit.InnerText +
                                         infoMeasure2.InnerText + infoConRangeDMin.InnerText + infoConRangeDMax.InnerText + infoConDenPrefix.InnerText + infoConDenExp.InnerText.Trim();
                        ingredient.concise = String.IsNullOrWhiteSpace(conciseIng) ? null : conciseIng.Trim();

                        ingredient.highamountnumerunit = ctlHighLimitNumUnit.ControlValue.ToString();
                        ingredient.highamountdenomunit = ctlHighLimitDenUnit.ControlValue.ToString();
                        ingredient.highamountnumerprefix = ctlHighLimitNumPrefix.ControlValue.ToString();
                        ingredient.highamountdenomprefix = ctlHighLimitDenPrefix.ControlValue.ToString();

                        ingredient.lowamountnumerunit = ctlLowLimitNumUnit.ControlValue.ToString();
                        ingredient.lowamountdenomunit = ctlLowLimitDenUnit.ControlValue.ToString();
                        ingredient.lowamountnumerprefix = ctlLowLimitNumPrefix.ControlValue.ToString();
                        ingredient.lowamountdenomprefix = ctlLowLimitDenPrefix.ControlValue.ToString();

                        ingredient.highamountnumervalue = ValidationHelper.IsValidDecimal(ctlHighLimitNumVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlHighLimitNumVal.ControlTextValue) : null;
                        ingredient.highamountdenomvalue = ValidationHelper.IsValidDecimal(ctlHighLimitDenVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlHighLimitDenVal.ControlTextValue) : null;
                        ingredient.lowamountnumervalue = ValidationHelper.IsValidDecimal(ctlLowLimitNumVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlLowLimitNumVal.ControlTextValue) : null;
                        ingredient.lowamountdenomvalue = ValidationHelper.IsValidDecimal(ctlLowLimitDenVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlLowLimitDenVal.ControlTextValue) : null;
                        ingredient.userID = SessionManager.Instance.CurrentUser.UserID;
                        ingredient = _activeIngredieent_PKOperations.Save(ingredient);

                        ((List<Activeingredient_PK>)_entityData).Add(ingredient);
                    }
                    else
                    {
                        ingredient.activeingredient_PK = minIngPK;
                        ingredient.pp_FK = null;
                        ingredient.substancecode_FK = ValidationHelper.IsValidInt(ctlSubstance_PK.Value.ToString()) ? (int?)Convert.ToInt32(ctlSubstance_PK.Value.ToString()) : null;
                        ingredient.concentrationtypecode = ValidationHelper.IsValidInt(ctlConTypeCode.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlConTypeCode.ControlValue) : null;
                        ingredient.ExpressedBy_FK = ValidationHelper.IsValidInt(ctlExpressedAs.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlExpressedAs.ControlValue) : null;

                        //ingredient.strength_value = ValidationHelper.IsValidInt(ctlStrengthValue.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlStrengthValue.ControlValue) : null;
                        //ingredient.strength_unit = ctlStrengthUnit.ControlValue.ToString();
                        string conciseIng = infoConSubName.InnerText + infoConRangeNMin.InnerText + infoConRangeNMax.InnerText + infoConNumMaxPrefix.InnerText + infoConNUnit.InnerText +
                                         infoMeasure2.InnerText + infoConRangeDMin.InnerText + infoConRangeDMax.InnerText + infoConDenPrefix.InnerText + infoConDenExp.InnerText.Trim();
                        ingredient.concise = String.IsNullOrWhiteSpace(conciseIng) ? null : conciseIng.Trim();

                        ingredient.lowamountnumerunit = ctlExactNumUnit.ControlValue.ToString();
                        ingredient.lowamountdenomunit = ctlExactDenUnit.ControlValue.ToString();
                        ingredient.lowamountnumerprefix = ctlExactNumPrefix.ControlValue.ToString();
                        ingredient.lowamountdenomprefix = ctlExactDenPrefix.ControlValue.ToString();

                        ingredient.lowamountnumervalue = ValidationHelper.IsValidDecimal(ctlExactNumVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlExactNumVal.ControlTextValue) : null;
                        ingredient.lowamountdenomvalue = ValidationHelper.IsValidDecimal(ctlExactDenVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlExactDenVal.ControlTextValue) : null;
                        ingredient.userID = SessionManager.Instance.CurrentUser.UserID;
                        ingredient = _activeIngredieent_PKOperations.Save(ingredient);

                        ((List<Activeingredient_PK>)_entityData).Add(ingredient);
                    }

                    return ingredient;
            }
            return null;
        }

        public override void ClearForm(string arg)
        {

            ctlSubstance_PK.Value = "";
            substanceSearch.ControlValue = String.Empty;
            ctlConTypeCode.ControlValue = String.Empty;
            ctlExpressedAs.ControlValue = String.Empty;

            //ctlStrengthUnit.ControlValue = String.Empty;
            //ctlStrengthValue.ControlValue = String.Empty;

            ctlHighLimitNumUnit.ControlValue = String.Empty;
            ctlHighLimitDenUnit.ControlValue = String.Empty;
            //ctlHighLimitNumPrefix.ControlValue = String.Empty;
            //ctlHighLimitDenPrefix.ControlValue = String.Empty;

            ctlLowLimitNumUnit.ControlValue = String.Empty;
            ctlLowLimitDenUnit.ControlValue = String.Empty;
            ctlLowLimitNumPrefix.ControlValue = String.Empty;
            ctlLowLimitDenPrefix.ControlValue = String.Empty;

            ctlHighLimitNumVal.ControlValue = String.Empty;
            ctlHighLimitDenVal.ControlValue = String.Empty;
            ctlLowLimitNumVal.ControlValue = String.Empty;
            ctlLowLimitDenVal.ControlValue = String.Empty;
            ctlExactNumVal.ControlValue = String.Empty;
            ctlExactDenVal.ControlValue = String.Empty;

            ctlExactNumUnit.ControlValue = String.Empty;
            ctlExactDenUnit.ControlValue = String.Empty;
            ctlExactNumPrefix.ControlValue = String.Empty;
            ctlExactDenPrefix.ControlValue = String.Empty;

            infoConcType.InnerText = "";
            infoSubName.InnerText = "";
            infoConSubName.InnerText = "";
            infoConDenExp.InnerText = "";
            //   infoSubNameText2.Visible = false;
            // infoUnitText.Visible = false;
            //   infoSubNameText.Visible = false;
            //  infoConcTypeText1.Visible = false;
            //   infoConcTypeText2.Visible = false;
            //    infoRangeMinText.Visible = false;

            //infoRangeNText.Visible = false;
            //  infoRangeDText.Visible = false;

            //pnlExactLimit.Visible = false;
            //pnlLowLimit.Visible = false;
            //pnlHighLimit.Visible = false;

            ctlHighLimitDenVal.CurrentControlState = ControlState.YouCanOnlyReadMe;

            ClearFullDescription();
        }

        public override void FillDataDefinitions(string arg)
        {
            BindDDLPrefix();
            BindDDLPrefixSubstanceClass();
            BindDDLConType();
            BindDDLUnits();
            //BindDDLMeasures();
            BindDDLExpressedBy();
        }

        public void valueChanged(object sender, ValueChangedEventArgs e)
        {
            //pnlInfo.Controls.Add(new LiteralControl('<a href'));  
            if (ctlLowLimitNumVal.ControlTextValue.Contains(".."))
            {
                ctlLowLimitNumVal.ControlValue = "";
                return;
            }
            if (!ValidationHelper.IsValidDecimal(ctlLowLimitNumVal.ControlTextValue))
            {
                ctlLowLimitNumVal.ControlValue = "";
                return;
            }
            info.InnerHtml = ctlLowLimitNumVal.ControlTextValue;
        }

        public string retHello()
        {
            return "Hello";
        }


        #region Exact handlers
        public void ExactDenUnit_Changed(object sender, ValueChangedEventArgs e)
        {
            if (ctlConTypeCode.ControlTextValue != "Range")
            {
                infoDenExp.InnerText = "";
                infoConDenExp.InnerText = "";
                if (ValidationHelper.IsValidInt(ctlExactDenUnit.ControlValue.ToString()))
                {
                    infoUnitText.Visible = true;
                    infoDenExp.InnerText = " " + ctlExactDenUnit.ControlTextValue;
                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlExactDenUnit.ControlValue));
                    if (ctlExpressedAs.ControlTextValue == "Units of Measure")
                    {
                        //infoConDenExp.InnerText = ssiTmp != null ? ssiTmp.term_name_english != "" ? "" + ssiTmp.term_name_english : "" : "";
                        infoConDenExp.InnerText = ssiTmp != null ? ssiTmp.Field8 != "" ? "" + ssiTmp.Field8 : "" : "";
                    }
                    else
                    {
                        infoConDenExp.InnerText = ssiTmp != null ? ssiTmp.Description != "" ? " " + ssiTmp.Description : "" : "";
                    }

                    if (ctlExactDenUnit.ControlTextValue.ToLower().Contains("each"))
                    {
                        infoDenExp.InnerText = "";
                        infoConDenExp.InnerText = "";
                    }
                }
            }
        }

        public void ExactDenPrefix_Changed(object sender, ValueChangedEventArgs e)
        {
            if (ctlConTypeCode.ControlTextValue != "Range")
            {
                infoDenPrefix.InnerText = "";
                infoConDenPrefix.InnerText = " ";
                if (ValidationHelper.IsValidInt(ctlExactDenPrefix.ControlValue.ToString()))
                {
                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlExactDenPrefix.ControlValue));
                    int indexOfFirtOccur = ssiTmp != null && ssiTmp.Description.IndexOf('(') != -1 ? ssiTmp.Description.IndexOf('(') : ssiTmp.Description.Length;
                    int end = ssiTmp.Description.Length - indexOfFirtOccur;

                    if (ssiTmp != null && !String.IsNullOrWhiteSpace(ssiTmp.Description) && !ssiTmp.Description.ToLower().Contains("single"))
                    {
                        infoUnitText.Visible = true;
                        infoDenPrefix.InnerText = " " + ssiTmp.Description.Remove(indexOfFirtOccur, end);
                        //infoConDenPrefix.InnerText = !String.IsNullOrWhiteSpace(ssiTmp.term_name_english) ? " " + ssiTmp.term_name_english : "";
                        infoConDenPrefix.InnerText = !String.IsNullOrWhiteSpace(ssiTmp.Field8) ? " " + ssiTmp.Field8 : "";
                    }
                    else
                    {
                        infoDenPrefix.InnerText = "";
                        infoConDenPrefix.InnerText = "";
                    }

                    if (ctlExactDenVal.ControlTextValue == "1")
                    {
                        infoConDenPrefix.InnerText = infoConDenPrefix.InnerText.TrimStart();
                        infoConDenPrefix.Attributes["style"] = "margin-left:-3px;";
                        if (ctlExactDenPrefix.ControlTextValue.ToLower().Contains("single"))
                            infoConDenExp.Attributes["style"] = "margin-left:0px;";
                        else
                            infoConDenExp.Attributes["style"] = "margin-left:-3px;";

                    }
                    else
                    {
                        infoConDenPrefix.InnerText = " " + infoConDenPrefix.InnerText.TrimStart();
                        infoConDenPrefix.Attributes["style"] = "margin-left:0px;";
                        if (ctlExactDenPrefix.ControlTextValue.ToLower().Contains("single"))
                            infoConDenExp.Attributes["style"] = "margin-left:0px;";
                        else
                            infoConDenExp.Attributes["style"] = "margin-left:-3px;";
                    }

                    ctlHighLimitNumPrefix.ControlValue = ctlLowLimitNumPrefix.ControlValue;
                }
            }
        }

        public void ExactDenVal_Changed(object sender, ValueChangedEventArgs e)
        {
            string ctlExactDenValTmp = ctlExactDenVal.ControlTextValue;
            if (ctlExactDenVal.ControlTextValue.Contains(".."))
            {
                ctlExactDenVal.ControlValue = "";
                return;
            }
            if (!ValidationHelper.IsValidDecimal(ctlExactDenVal.ControlTextValue))
            {
                ctlExactDenVal.ControlValue = "";
                return;
            }

            if (ctlExactDenValTmp.Contains("."))
            {
                ctlExactDenValTmp = ctlExactDenValTmp.Replace(".", ",");
                ctlExactDenVal.ControlValue = ctlExactDenValTmp;
            }
            /*
            decimal exactDenVal = Convert.ToDecimal(ctlExactDenVal.ControlTextValue);
            bool showZeroes = true;
            if (exactDenVal == Math.Round(exactDenVal, 0)) showZeroes = false;

            ctlExactDenVal.ControlValue = showZeroes ? ctlExactDenVal.ControlValue : exactDenVal.ToString("#0");

            if (ctlExpressedAs.ControlTextValue.ToLower().Contains("presentation")) ctlExactDenVal.ControlValue = Convert.ToDecimal(ctlExactDenVal.ControlTextValue).ToString("#0");
            */
            ctlExactDenVal.ControlValue = Convert.ToDecimal(ctlExactDenVal.ControlTextValue).ToString("#0.#####");
            if (ctlExpressedAs.ControlTextValue.ToLower().Contains("presentation")) ctlExactDenVal.ControlValue = ((int)Convert.ToDecimal(ctlExactDenVal.ControlTextValue));
            if (ctlExactDenVal.ControlTextValue != "1")
            {
                infoUnitText.Visible = true;
                infoRangeDMin.InnerHtml = " " + ctlExactDenVal.ControlTextValue;
                infoConRangeDMin.InnerHtml = "/" + ctlExactDenVal.ControlTextValue;
                infoConDenPrefix.InnerText = " " + infoConDenPrefix.InnerText.TrimStart();
                infoConDenPrefix.Attributes["style"] = "margin-left:0px;";
            }
            else
            {
                infoRangeDMin.InnerHtml = "";
                infoConRangeDMin.InnerHtml = "/";
                infoConDenPrefix.InnerText = infoConDenPrefix.InnerText.TrimStart();
                infoConDenPrefix.Attributes["style"] = "margin-left:-3px;";
            }
        }

        public void ExactNumUnit_Changed(object sender, ValueChangedEventArgs e)
        {
            if (ctlConTypeCode.ControlTextValue != "Range")
            {
                if (ctlExactNumUnit.ControlValue.ToString() != "")
                {
                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlExactNumUnit.ControlValue));
                    //infoConNUnit.InnerText = ssiTmp != null ? ssiTmp.term_name_english != "" ? ssiTmp.term_name_english : "" : "";
                    infoConNUnit.InnerText = ssiTmp != null ? ssiTmp.Field8 != "" ? ssiTmp.Field8 : "" : "";
                    infoUnit.InnerText = ctlExactNumUnit.ControlTextValue;
                    infoConNUnit.Attributes["style"] = "margin-left:-3px;";
                }
                else
                {
                    infoUnit.InnerText = "";
                    infoConNUnit.InnerText = "";
                    infoConNUnit.Attributes["style"] = "margin-left:0px;";
                }

                if (ctlExactNumUnit.ControlTextValue.ToLower().Contains("count"))
                {
                    infoConNUnit.Attributes["class"] = "red";
                }
                else
                {
                    infoConNUnit.Attributes["class"] = "blue";
                }
            }
        }

        public void ExactNumPrefix_Changed(object sender, ValueChangedEventArgs e)
        {
            if (ctlConTypeCode.ControlTextValue != "Range")
            {
                infoNumMaxPrefix.InnerText = "";
                infoConNumMaxPrefix.InnerText = " ";
                if (ctlExactNumPrefix.ControlValue.ToString() != "")
                {
                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlExactNumPrefix.ControlValue));
                    int indexOfFirtOccur = ssiTmp.Description.IndexOf('(') != -1 ? ssiTmp.Description.IndexOf('(') : ssiTmp.Description.Length;
                    int end = ssiTmp.Description.Length - indexOfFirtOccur;

                    if (ssiTmp != null && !String.IsNullOrWhiteSpace(ssiTmp.Description) && !ssiTmp.Description.ToLower().Contains("single"))
                    {
                        infoNumMaxPrefix.InnerText = " " + ssiTmp.Description.Remove(indexOfFirtOccur, end);
                        //infoConNumMaxPrefix.InnerText = !String.IsNullOrWhiteSpace(ssiTmp.term_name_english) ? " " + ssiTmp.term_name_english : "";
                        infoConNumMaxPrefix.InnerText = !String.IsNullOrWhiteSpace(ssiTmp.Field8) ? " " + ssiTmp.Field8 : "";
                    }

                    if (ctlExactNumPrefix.ControlTextValue.ToLower().Contains("single"))
                        infoConNUnit.Attributes["style"] = "margin-left:0px;";
                    else
                        infoConNUnit.Attributes["style"] = "margin-left:-3px;";
                }
            }
        }

        public void ExactNumVal_Changed(object sender, ValueChangedEventArgs e)
        {
            //pnlInfo.Controls.Add(new LiteralControl('<a href'));  
            if (ctlConTypeCode.ControlTextValue != "Range")
            {

                infoConcTypeText2.Visible = true;
                //  infoConcTypeText1.Visible = false;
                if (ctlExactNumVal.ControlTextValue.Contains(".."))
                {
                    ctlExactNumVal.ControlValue = "";
                    return;
                }
                if (!ValidationHelper.IsValidDecimal(ctlExactNumVal.ControlTextValue))
                {
                    ctlExactNumVal.ControlValue = "";
                    return;
                }

                string ctlExactNumValTmp = ctlExactNumVal.ControlTextValue;
                if (ctlExactNumValTmp.Contains("."))
                {
                    ctlExactNumValTmp = ctlExactNumValTmp.Replace(".", ",");
                    ctlExactNumVal.ControlValue = ctlExactNumValTmp;
                }
                /*
                decimal exactNumVal =  Convert.ToDecimal(ctlExactNumVal.ControlTextValue);
                bool showZeroes = true;
                if (exactNumVal == Math.Round(exactNumVal, 0)) showZeroes = false;

                ctlExactNumVal.ControlValue = showZeroes ? ctlExactNumVal.ControlValue : exactNumVal.ToString("#0");
                infoRangeNMin.InnerHtml = " " + (showZeroes ? Convert.ToDecimal(ctlExactNumVal.ControlTextValue).ToString("#0.00") : Convert.ToDecimal(ctlExactNumVal.ControlTextValue).ToString("#0")) + " ";
                infoConRangeNMin.InnerHtml = " " + (showZeroes ? Convert.ToDecimal(ctlExactNumVal.ControlTextValue).ToString("#0.00") : Convert.ToDecimal(ctlExactNumVal.ControlTextValue).ToString("#0")) + " ";
                */
                ctlExactNumVal.ControlValue = Convert.ToDecimal(ctlExactNumVal.ControlTextValue).ToString("#0.#####");
                //if (ctlExpressedAs.ControlTextValue.ToLower().Contains("presentation")) ctlExactNumVal.ControlValue = ((int)Convert.ToDecimal(ctlExactNumVal.ControlTextValue));
                infoRangeNMin.InnerHtml = " " + ctlExactNumVal.ControlTextValue;
                infoConRangeNMin.InnerHtml = " " + ctlExactNumVal.ControlTextValue;
            }
        }


        #endregion

        #region High limit handlers
        public void HighLimitNumVal_Changed(object sender, ValueChangedEventArgs e)
        {
            infoConRangeNMax.InnerText = "";
            infoRangeNMax.InnerText = "";

            if (ctlHighLimitNumVal.ControlTextValue.Contains(".."))
            {
                ctlHighLimitNumVal.ControlValue = "";
                infoConRangeNMax.InnerHtml = "";
                return;
            }
            if (!ValidationHelper.IsValidDecimal(ctlHighLimitNumVal.ControlTextValue))
            {
                ctlHighLimitNumVal.ControlValue = "";

                return;
            }

            string ctlHighLimitNumValTmp = ctlHighLimitNumVal.ControlTextValue;
            if (ctlHighLimitNumValTmp.Contains("."))
            {
                ctlHighLimitNumValTmp = ctlHighLimitNumValTmp.Replace(".", ",");
                ctlHighLimitNumVal.ControlValue = ctlHighLimitNumValTmp;
            }

            ctlHighLimitNumVal.ControlValue = Convert.ToDecimal(ctlHighLimitNumVal.ControlTextValue).ToString("#0.#####");

            if (ctlHighLimitNumVal.ControlTextValue == ctlLowLimitNumVal.ControlTextValue &&
                ctlHighLimitNumPrefix.ControlTextValue == ctlLowLimitNumPrefix.ControlTextValue)
            {

                // infoRangeNText.Visible = false;
                infoRangeNMax.InnerText = "";
                infoConRangeNMax.InnerText = "";
                infoNumMinPrefix.InnerText = "";
                infoConNumMinPrefix.InnerText = "";
            }
            else
            {
                infoRangeNText.Visible = true;
                infoRangeNMax.InnerHtml = ctlHighLimitNumVal.ControlTextValue;
                infoConRangeNMax.InnerHtml = " - " + ctlHighLimitNumVal.ControlTextValue;
                LowLimitNumPrefix_Changed(null, null);
            }
        }

        public void HighLimitNumPrefix_Changed(object sender, ValueChangedEventArgs e)
        {
            if (ctlConTypeCode.ControlTextValue.ToLower().Contains("range"))
            {
                string prefixNumMin = "";
                string prefixConNumMin = "";
                string prefixNumMax = "";
                string prefixConNumMax = " ";

                Ssi__cont_voc_PK prefix = null;

                if (ValidationHelper.IsValidInt(ctlLowLimitNumPrefix.ControlValue.ToString()))
                    prefix = _ssi_cont_voc_PKOperations.GetEntity(int.Parse(ctlLowLimitNumPrefix.ControlValue.ToString()));

                if (prefix != null && !prefix.Description.ToLower().Contains("single"))
                {
                    prefixNumMin = prefix.Description.Contains("(") ? " " + prefix.Description.Remove(prefix.Description.IndexOf("(")).Trim() : " " + prefix.Description.Trim();
                    //prefixConNumMin = !String.IsNullOrWhiteSpace(prefix.term_name_english) ? " " + prefix.term_name_english.Trim() : "";
                    prefixConNumMin = !String.IsNullOrWhiteSpace(prefix.Field8) ? " " + prefix.Field8.Trim() : "";
                }

                prefix = null;
                if (ValidationHelper.IsValidInt(ctlHighLimitNumPrefix.ControlValue.ToString()))
                    prefix = _ssi_cont_voc_PKOperations.GetEntity(int.Parse(ctlHighLimitNumPrefix.ControlValue.ToString()));

                if (prefix != null && !prefix.Description.ToLower().Contains("single"))
                {
                    prefixNumMax = prefix.Description.Contains("(") ? " " + prefix.Description.Remove(prefix.Description.IndexOf("(")).Trim() : " " + prefix.Description.Trim();
                    //prefixConNumMax = !String.IsNullOrWhiteSpace(prefix.term_name_english) ? " " + prefix.term_name_english.Trim() : "";
                    prefixConNumMax = !String.IsNullOrWhiteSpace(prefix.Field8) ? " " + prefix.Field8.Trim() : " ";
                }

                infoRangeNText.Visible = true;
                infoRangeNMax.InnerHtml = ctlHighLimitNumVal.ControlTextValue;
                infoConRangeNMax.InnerHtml = " - " + ctlHighLimitNumVal.ControlTextValue;

                if (prefixNumMin == prefixNumMax)
                {
                    prefixNumMin = "";
                    prefixConNumMin = "";

                    if (ctlHighLimitNumVal.ControlTextValue == ctlLowLimitNumVal.ControlTextValue)
                    {
                        //   infoRangeNText.Visible = false;
                        infoRangeNMax.InnerHtml = "";
                        infoConRangeNMax.InnerHtml = "";
                    }
                }

                if (ctlHighLimitNumPrefix.ControlTextValue.ToLower().Contains("single"))
                    infoConNUnit.Attributes["style"] = "margin-left:0px;";
                else
                    infoConNUnit.Attributes["style"] = "margin-left:-3px;";

                infoNumMinPrefix.InnerText = prefixNumMin;
                infoConNumMinPrefix.InnerText = prefixConNumMin;
                infoNumMaxPrefix.InnerText = prefixNumMax;
                infoConNumMaxPrefix.InnerText = prefixConNumMax;

                //ctlHighLimitNumPrefix.ControlValue = ctlLowLimitNumPrefix.ControlValue;
            }
        }

        public void HighLimitNumUnit_Changed(object sender, ValueChangedEventArgs e)
        {
        }

        public void HighLimitDenVal_Changed(object sender, ValueChangedEventArgs e)
        {
            infoConRangeDMax.InnerText = "";
            infoRangeDMax.InnerText = "";

            string ctlHighLimitDenValTmp = ctlHighLimitDenVal.ControlTextValue;
            if (ctlHighLimitDenVal.ControlTextValue.Contains(".."))
            {
                ctlHighLimitDenVal.ControlValue = "";
                return;
            }
            if (!ValidationHelper.IsValidDecimal(ctlHighLimitDenVal.ControlTextValue))
            {
                ctlHighLimitDenVal.ControlValue = "";
                return;
            }

            if (ctlHighLimitDenValTmp.Contains("."))
            {
                ctlHighLimitDenValTmp = ctlHighLimitDenValTmp.Replace(".", ",");
                ctlHighLimitDenVal.ControlValue = ctlHighLimitDenValTmp;
            }

            ctlHighLimitDenVal.ControlValue = Convert.ToDecimal(ctlHighLimitDenVal.ControlTextValue).ToString("#0.#####");
            if (ctlExpressedAs.ControlTextValue.ToLower().Contains("presentation")) ctlHighLimitDenVal.ControlValue = ((int)Convert.ToDecimal(ctlHighLimitDenVal.ControlTextValue));


            if (ctlHighLimitDenVal.ControlTextValue != ctlLowLimitDenVal.ControlTextValue)
            {
                infoUnitText.Visible = true;
                infoRangeDText.Visible = true;
                infoRangeDMax.InnerHtml = ctlHighLimitDenVal.ControlTextValue;
                infoConRangeDMax.InnerHtml = " - " + ctlHighLimitDenVal.ControlTextValue;
                infoRangeDMin.InnerHtml = ctlLowLimitDenVal.ControlTextValue;
                infoConRangeDMin.InnerHtml = "/" + ctlLowLimitDenVal.ControlTextValue;
                infoConDenPrefix.Attributes["style"] = "margin-left:0px;";
                infoConDenPrefix.InnerText = " " + infoConDenPrefix.InnerText.TrimStart();
            }
            else
            {
                //     infoRangeDText.Visible = false;
                infoRangeDMax.InnerHtml = "";
                infoConRangeDMax.InnerHtml = "";

                if (ctlLowLimitDenVal.ControlTextValue == "1")
                {
                    infoRangeDMin.InnerHtml = "";
                    infoConRangeDMin.InnerHtml = "/";
                    infoConDenPrefix.InnerText = infoConDenPrefix.InnerText.TrimStart();
                    infoConDenPrefix.Attributes["style"] = "margin-left:-3px;";
                }
                else
                {
                    infoConDenPrefix.InnerText = " " + infoConDenPrefix.InnerText.TrimStart();
                    infoConDenPrefix.Attributes["style"] = "margin-left:0px;";
                }
            }

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
        public void Substance_Changed()
        {

            if (!ValidationHelper.IsValidInt(ctlSubstance_PK.Value))
            {
                infoSubName.InnerText = "";
                infoConSubName.InnerText = "";
                return;
            }
            Substance_PK sub = _substance_PKOperations.GetEntity(ctlSubstance_PK.Value);

            if (sub != null && sub.substance_name != null)
            {
                ctlSubstance_PK.Value = sub.substance_PK.ToString();
                substanceSearch.ControlValue = sub.substance_name;

                infoSubName.InnerText = sub.substance_name;
                infoSubNameText2.Visible = true;
                infoConSubName.InnerText = sub.substance_name;
                infoSubNameText.Visible = true;
            }


        }
        public void LowLimitNumVal_Changed(object sender, ValueChangedEventArgs e)
        {
            infoConRangeNMin.InnerText = "";
            infoRangeNMin.InnerText = "";

            if (ctlLowLimitNumVal.ControlTextValue.Contains(".."))
            {
                ctlLowLimitNumVal.ControlValue = "";
                return;
            }
            if (!ValidationHelper.IsValidDecimal(ctlLowLimitNumVal.ControlTextValue))
            {
                ctlLowLimitNumVal.ControlValue = "";

                return;
            }

            string ctlLowLimitNumValTmp = ctlLowLimitNumVal.ControlTextValue;
            if (ctlLowLimitNumValTmp.Contains("."))
            {
                ctlLowLimitNumValTmp = ctlLowLimitNumValTmp.Replace(".", ",");
                ctlLowLimitNumVal.ControlValue = ctlLowLimitNumValTmp;
            }

            infoRangeMinText.Visible = true;
            ctlLowLimitNumVal.ControlValue = Convert.ToDecimal(ctlLowLimitNumVal.ControlTextValue).ToString("#0.#####");

            infoRangeNMin.InnerHtml = " " + ctlLowLimitNumVal.ControlTextValue;
            infoConRangeNMin.InnerHtml = " " + ctlLowLimitNumVal.ControlTextValue;

            if (ctlHighLimitNumVal.ControlTextValue == ctlLowLimitNumVal.ControlTextValue &&
                ctlHighLimitNumPrefix.ControlTextValue == ctlLowLimitNumPrefix.ControlTextValue)
            {
                //  infoRangeNText.Visible = false;
                infoRangeNMax.InnerText = "";
                infoConRangeNMax.InnerText = "";
                infoNumMinPrefix.InnerText = "";
                infoConNumMinPrefix.InnerText = "";
            }
            else
            {
                infoRangeNText.Visible = true;
                infoRangeNMax.InnerHtml = ctlHighLimitNumVal.ControlTextValue;
                infoConRangeNMax.InnerHtml = " - " + ctlHighLimitNumVal.ControlTextValue;
                LowLimitNumPrefix_Changed(null, null);
            }
        }

        public void LowLimitNumPrefix_Changed(object sender, ValueChangedEventArgs e)
        {
            if (ctlConTypeCode.ControlTextValue.ToLower().Contains("range"))
            {
                string prefixNumMin = "";
                string prefixConNumMin = "";
                string prefixNumMax = "";
                string prefixConNumMax = "";

                Ssi__cont_voc_PK prefix = null;

                if (ValidationHelper.IsValidInt(ctlLowLimitNumPrefix.ControlValue.ToString()))
                    prefix = _ssi_cont_voc_PKOperations.GetEntity(int.Parse(ctlLowLimitNumPrefix.ControlValue.ToString()));

                if (prefix != null && !prefix.Description.ToLower().Contains("single"))
                {
                    prefixNumMin = prefix.Description.Contains("(") ? " " + prefix.Description.Remove(prefix.Description.IndexOf("(")).Trim() : " " + prefix.Description.Trim();
                    //prefixConNumMin = !String.IsNullOrWhiteSpace(prefix.term_name_english) ? " " + prefix.term_name_english.Trim() : "";
                    prefixConNumMin = !String.IsNullOrWhiteSpace(prefix.Field8) ? " " + prefix.Field8.Trim() : "";
                }

                prefix = null;
                if (ValidationHelper.IsValidInt(ctlHighLimitNumPrefix.ControlValue.ToString()))
                    prefix = _ssi_cont_voc_PKOperations.GetEntity(int.Parse(ctlHighLimitNumPrefix.ControlValue.ToString()));

                if (prefix != null && !prefix.Description.ToLower().Contains("single"))
                {
                    prefixNumMax = prefix.Description.Contains("(") ? " " + prefix.Description.Remove(prefix.Description.IndexOf("(")).Trim() : " " + prefix.Description.Trim();
                    //prefixConNumMax = !String.IsNullOrWhiteSpace(prefix.term_name_english) ? " " + prefix.term_name_english.Trim() : "";
                    prefixConNumMax = !String.IsNullOrWhiteSpace(prefix.Field8) ? " " + prefix.Field8.Trim() : "";
                }

                infoRangeNText.Visible = true;
                infoRangeNMax.InnerHtml = ctlHighLimitNumVal.ControlTextValue;
                infoConRangeNMax.InnerHtml = " - " + ctlHighLimitNumVal.ControlTextValue;

                if (prefixNumMin == prefixNumMax)
                {
                    prefixNumMin = "";
                    prefixConNumMin = "";

                    if (ctlHighLimitNumVal.ControlTextValue == ctlLowLimitNumVal.ControlTextValue)
                    {

                        //  infoRangeNText.Visible = false;
                        infoRangeNMax.InnerHtml = "";
                        infoConRangeNMax.InnerHtml = "";
                    }
                }

                if (ctlHighLimitNumPrefix.ControlTextValue.ToLower().Contains("single"))
                    infoConNUnit.Attributes["style"] = "margin-left:0px;";
                else
                    infoConNUnit.Attributes["style"] = "margin-left:-3px;";

                infoNumMinPrefix.InnerText = prefixNumMin;
                infoConNumMinPrefix.InnerText = prefixConNumMin;
                infoNumMaxPrefix.InnerText = prefixNumMax;
                infoConNumMaxPrefix.InnerText = prefixConNumMax;

                //ctlHighLimitNumPrefix.ControlValue = ctlLowLimitNumPrefix.ControlValue;
            }
        }

        public void LowLimitNumUnit_Changed(object sender, ValueChangedEventArgs e)
        {
            if (ctlConTypeCode.ControlTextValue.ToLower().Contains("range"))
            {
                infoUnit.InnerText = "";
                infoConNUnit.InnerText = "";
                if (ctlLowLimitNumUnit.ControlValue.ToString() != "")
                {
                    infoUnit.InnerText = ctlLowLimitNumUnit.ControlTextValue.Trim();

                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlLowLimitNumUnit.ControlValue));
                    //infoConNUnit.InnerText = ssiTmp != null ? ssiTmp.term_name_english != "" ? ssiTmp.term_name_english.Trim() : "" : "";
                    infoConNUnit.InnerText = ssiTmp != null ? ssiTmp.Field8 != "" ? ssiTmp.Field8.Trim() : "" : "";

                    if (ctlLowLimitNumUnit.ControlTextValue.ToLower().Contains("count"))
                    {
                        infoConNUnit.Attributes["class"] = "red";
                    }
                    else
                    {
                        infoConNUnit.Attributes["class"] = "blue";
                    }

                    if (ctlHighLimitNumPrefix.ControlTextValue.ToLower().Contains("single"))
                        infoConNUnit.Attributes["style"] = "margin-left:0px;";
                    else
                        infoConNUnit.Attributes["style"] = "margin-left:-3px;";
                    ctlHighLimitNumUnit.ControlValue = ctlLowLimitNumUnit.ControlValue;
                }
                else
                {
                    infoConNUnit.Attributes["style"] = "margin-left:0px;";
                }
            }
        }

        public void LowLimitDenVal_Changed(object sender, ValueChangedEventArgs e)
        {
            infoConRangeDMin.InnerText = "";
            infoRangeDMin.InnerText = "";

            if (ctlLowLimitDenVal.ControlTextValue.Contains(".."))
            {
                ctlLowLimitDenVal.ControlValue = "";
                ctlHighLimitDenVal.ControlValue = ctlLowLimitDenVal.ControlValue;
                return;
            }
            if (!ValidationHelper.IsValidDecimal(ctlLowLimitDenVal.ControlTextValue.ToString()))
            {
                ctlLowLimitDenVal.ControlValue = "";
                ctlHighLimitDenVal.ControlValue = ctlLowLimitDenVal.ControlValue;
                return;
            }

            string ctlLowLimitDenValTmp = ctlLowLimitDenVal.ControlTextValue;
            if (ctlLowLimitDenValTmp.Contains("."))
            {
                ctlLowLimitDenValTmp = ctlLowLimitDenValTmp.Replace(".", ",");
                ctlLowLimitDenVal.ControlValue = ctlLowLimitDenValTmp;
            }

            infoUnitText.Visible = true;
            ctlLowLimitDenVal.ControlValue = Convert.ToDecimal(ctlLowLimitDenVal.ControlTextValue).ToString("#0.#####");
            if (ctlExpressedAs.ControlTextValue.ToLower().Contains("presentation")) ctlLowLimitDenVal.ControlValue = ((int)Convert.ToDecimal(ctlLowLimitDenVal.ControlTextValue));

            ctlHighLimitDenVal.ControlValue = ctlLowLimitDenVal.ControlValue;

            infoRangeDMin.InnerHtml = ctlLowLimitDenVal.ControlTextValue;
            infoConRangeDMin.InnerHtml = "/" + ctlLowLimitDenVal.ControlTextValue;

            if (ctlHighLimitDenVal.ControlTextValue != ctlLowLimitDenVal.ControlTextValue)
            {
                infoUnitText.Visible = true;
                infoRangeDText.Visible = true;
                infoRangeDMax.InnerHtml = ctlHighLimitDenVal.ControlTextValue;
                infoConRangeDMax.InnerHtml = " - " + ctlHighLimitDenVal.ControlTextValue;
                infoConDenPrefix.Attributes["style"] = "margin-left:0px;";
                infoConDenPrefix.InnerText = " " + infoConDenPrefix.InnerText.TrimStart();
            }
            else
            {
                //   infoRangeDText.Visible = false;
                infoRangeDMax.InnerHtml = "";
                infoConRangeDMax.InnerHtml = "";

                if (ctlLowLimitDenVal.ControlTextValue == "1")
                {
                    infoRangeDMin.InnerHtml = "";
                    infoConRangeDMin.InnerHtml = "/";
                    infoConDenPrefix.InnerText = infoConDenPrefix.InnerText.TrimStart();
                    infoConDenPrefix.Attributes["style"] = "margin-left:-3px;";
                }
                else
                {
                    infoConDenPrefix.InnerText = " " + infoConDenPrefix.InnerText.TrimStart();
                    infoConDenPrefix.Attributes["style"] = "margin-left:0px;";
                }
            }
        }

        public void LowLimitDenPrefix_Changed(object sender, ValueChangedEventArgs e)
        {
            infoDenPrefix.InnerText = "";
            infoConDenPrefix.InnerText = "";

            if (ctlLowLimitDenPrefix.ControlValue.ToString() != "")
            {
                Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlLowLimitDenPrefix.ControlValue));
                int indexOfFirtOccur = ssiTmp.Description.IndexOf('(') != -1 ? ssiTmp.Description.IndexOf('(') : ssiTmp.Description.Length;
                int end = ssiTmp.Description.Length - indexOfFirtOccur;

                if (ssiTmp != null && !String.IsNullOrWhiteSpace(ssiTmp.Description) && !ssiTmp.Description.ToLower().Contains("single"))
                {
                    infoUnitText.Visible = true;
                    infoDenPrefix.InnerText = " " + ssiTmp.Description.Remove(indexOfFirtOccur, end);
                    //infoConDenPrefix.InnerText = !String.IsNullOrWhiteSpace(ssiTmp.term_name_english) ? " " + ssiTmp.term_name_english : "";
                    infoConDenPrefix.InnerText = !String.IsNullOrWhiteSpace(ssiTmp.Field8) ? " " + ssiTmp.Field8 : "";
                }


                if (ctlLowLimitDenVal.ControlTextValue == "1" && ctlHighLimitDenVal.ControlTextValue == "1")
                {
                    infoConDenPrefix.InnerText = infoConDenPrefix.InnerText.TrimStart();
                    infoConDenPrefix.Attributes["style"] = "margin-left:-3px;";
                }
                else
                {
                    infoConDenPrefix.InnerText = " " + infoConDenPrefix.InnerText.TrimStart();
                    infoConDenPrefix.Attributes["style"] = "margin-left:0px;";
                }

                if (ctlLowLimitDenPrefix.ControlTextValue.ToLower().Contains("single"))
                    infoConDenExp.Attributes["style"] = "margin-left:0px;";
                else
                    infoConDenExp.Attributes["style"] = "margin-left:-3px;";

                ctlHighLimitDenPrefix.ControlValue = ctlLowLimitDenPrefix.ControlValue;
            }
        }

        public void LowLimitDenUnit_Changed(object sender, ValueChangedEventArgs e)
        {
            if (ctlConTypeCode.ControlTextValue == "Range")
            {
                infoConDenExp.InnerText = "";
                infoDenExp.InnerText = "";
                if (ValidationHelper.IsValidInt(ctlLowLimitDenUnit.ControlValue.ToString()))
                {
                    infoUnitText.Visible = true;

                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlLowLimitDenUnit.ControlValue));
                    if (ctlExpressedAs.ControlTextValue == "Units of Measure")
                    {
                        infoDenExp.InnerText = "" + ctlLowLimitDenUnit.ControlTextValue;
                        //infoConDenExp.InnerText = ssiTmp != null ? ssiTmp.term_name_english != "" ? "" + ssiTmp.term_name_english : "" : "";
                        infoConDenExp.InnerText = ssiTmp != null ? ssiTmp.Field8 != "" ? "" + ssiTmp.Field8 : "" : "";
                    }
                    else
                    {
                        infoDenExp.InnerText = " " + ctlLowLimitDenUnit.ControlTextValue;
                        infoConDenExp.InnerText = ssiTmp != null ? ssiTmp.Description != "" ? " " + ssiTmp.Description : "" : "";
                    }

                    if (ctlLowLimitDenUnit.ControlTextValue.ToLower().Contains("each"))
                    {
                        infoDenExp.InnerText = "";
                        infoConDenExp.InnerText = "";
                    }

                    ctlHighLimitDenUnit.ControlValue = ctlLowLimitDenUnit.ControlValue;
                }
            }
        }

        #endregion


        public void ConcentrationType_Changed(object sender, ValueChangedEventArgs e)
        {
            string concType = "";

            if (ValidationHelper.IsValidInt(ctlConTypeCode.ControlValue.ToString()))
            {
                Ssi__cont_voc_PK ssi = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlConTypeCode.ControlValue));
                concType = ssi != null ? ssi.term_name_english : "";


                if (concType.ToLower().Contains("range"))
                {
                    infoConcTypeText1.Visible = true;
                    //     infoConcTypeText2.Visible = false;

                    //pnlLowLimit.Visible = true;
                    //pnlHighLimit.Visible = true;
                    //pnlExactLimit.Visible = false;
                    infoRangeMinText.Visible = true;
                    infoConcType.InnerText = " " + concType + " ";

                    //ctlHighLimitNumPrefix.CurrentControlState = ControlState.YouCantChangeMe;
                    ctlHighLimitNumUnit.CurrentControlState = ControlState.YouCantChangeMe;
                    ctlHighLimitDenPrefix.CurrentControlState = ControlState.YouCantChangeMe;
                    ctlHighLimitDenUnit.CurrentControlState = ControlState.YouCantChangeMe;

                    ctlHighLimitNumPrefix.ControlValue = ctlLowLimitNumPrefix.ControlValue;
                    ctlHighLimitNumUnit.ControlValue = ctlLowLimitNumUnit.ControlValue;
                    ctlHighLimitDenPrefix.ControlValue = ctlLowLimitDenPrefix.ControlValue;
                    ctlHighLimitDenUnit.ControlValue = ctlLowLimitDenUnit.ControlValue;

                    BindFullDescription("Range");
                }
                else
                {
                    infoConcType.InnerText = "";
                    //   infoConcTypeText1.Visible = false;
                    infoConcTypeText2.Visible = true;

                    //pnlExactLimit.Visible = true;
                    //pnlLowLimit.Visible = false;
                    //pnlHighLimit.Visible = false;
                    //  infoRangeMinText.Visible = false;

                    ctlHighLimitNumPrefix.CurrentControlState = ControlState.ReadyForAction;
                    ctlHighLimitNumUnit.CurrentControlState = ControlState.ReadyForAction;
                    ctlHighLimitDenPrefix.CurrentControlState = ControlState.ReadyForAction;
                    ctlHighLimitDenUnit.CurrentControlState = ControlState.ReadyForAction;

                    //pnlExactLimit
                    if (concType.ToLower().Contains("approximately"))
                    {
                        pnlExactLimit.GroupingText = "Approximate value";
                    }
                    else if (concType.ToLower().Contains("not less than"))
                    {
                        pnlExactLimit.GroupingText = "Minimum value";
                    }
                    else if (concType.ToLower().Contains("up to"))
                    {
                        pnlExactLimit.GroupingText = "Maximum value";
                    }
                    else if (concType.ToLower().Contains("equal"))
                    {
                        pnlExactLimit.GroupingText = "Exact value";
                    }
                    else if (concType.ToLower().Contains("average"))
                    {
                        pnlExactLimit.GroupingText = "Average value";
                    }

                    BindFullDescription("Exact");
                }
            }
            else
            {

                //pnlExactLimit.Visible = false;
                //pnlLowLimit.Visible = false;
                //pnlHighLimit.Visible = false;
                //    infoConcTypeText1.Visible = false;
                //   infoConcTypeText2.Visible = false;
                ClearFullDescription();
            }
        }

        private void BindFullDescription(String arg)
        {
            ClearFullDescription();
            if (arg == "Range")
            {

                LowLimitNumVal_Changed(null, null);
                LowLimitNumPrefix_Changed(null, null);
                LowLimitNumUnit_Changed(null, null);

                LowLimitDenVal_Changed(null, null);
                LowLimitDenPrefix_Changed(null, null);
                LowLimitDenUnit_Changed(null, null);

                HighLimitNumVal_Changed(null, null);
                HighLimitNumPrefix_Changed(null, null);
                //HighLimitNumUnit_Changed(null, null);

                HighLimitDenVal_Changed(null, null);
                //HighLimitDenPrefix_Changed(null, null);
                //HighLimitDenUnit_Changed(null, null);
            }
            else
            {

                ExactDenVal_Changed(null, null);
                ExactDenPrefix_Changed(null, null);
                ExactDenUnit_Changed(null, null);
                ExactNumVal_Changed(null, null);
                ExactNumPrefix_Changed(null, null);
                ExactNumUnit_Changed(null, null);
            }
            Substance_Changed();

        }

        private void ClearFullDescription()
        {
            infoMeasure.InnerText = "";
            infoMeasure2.InnerText = "";
            infoNumMinPrefix.InnerText = "";
            infoNumMaxPrefix.InnerText = "";
            infoRangeNMax.InnerText = "";
            infoRangeNMin.InnerText = "";
            infoRangeDMin.InnerText = "";
            infoRangeDMax.InnerText = "";
            infoDenPrefix.InnerText = "";
            infoDenExp.InnerText = "";
            infoUnit.InnerText = "";

            infoConRangeNMin.InnerText = "";
            infoConRangeNMax.InnerText = "";
            infoConNumMinPrefix.InnerText = "";
            infoConNumMaxPrefix.InnerText = "";
            infoConNUnit.InnerText = "";
            infoConRangeDMin.InnerText = "";
            infoConRangeDMax.InnerText = "";
            infoConDenPrefix.InnerText = "";
            infoConDenExp.InnerText = "";


            //     infoUnitText.Visible = false;
            //    infoRangeMinText.Visible = false;
            //    infoRangeNText.Visible = false;
            //    infoRangeDText.Visible = false;
        }

        public void ExpressedAs_Changed(object sender, ValueChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace((string)ctlExpressedAs.ControlValue))
            {
                List<Ssi__cont_voc_PK> items = new List<Ssi__cont_voc_PK>();
                ctlExactDenUnit.ControlBoundItems.Clear();
                ctlLowLimitDenUnit.ControlBoundItems.Clear();
                ctlHighLimitDenUnit.ControlBoundItems.Clear();

                ctlExactDenUnit.SourceValueProperty = "ssi__cont_voc_PK";
                ctlExactDenUnit.SourceTextExpression = "Description";
                ctlExactDenUnit.FillControl<Ssi__cont_voc_PK>(items);

                ctlLowLimitDenUnit.SourceValueProperty = "ssi__cont_voc_PK";
                ctlLowLimitDenUnit.SourceTextExpression = "Description";
                ctlLowLimitDenUnit.FillControl<Ssi__cont_voc_PK>(items);

                ctlHighLimitDenUnit.SourceValueProperty = "ssi__cont_voc_PK";
                ctlHighLimitDenUnit.SourceTextExpression = "Description";
                ctlHighLimitDenUnit.FillControl<Ssi__cont_voc_PK>(items);

                HighLimitExp.InnerHtml = "";
                LowLimitExp.InnerHtml = "";
                ExactLimitExp.InnerHtml = "";
            }
            else

                if (ctlConTypeCode.ControlTextValue.ToLower().Contains("range"))
                {

                    if (ctlExpressedAs.ControlTextValue == "Units of Measure")
                    {
                        HighLimitExp.InnerHtml = "of Measure";
                        LowLimitExp.InnerHtml = "of Measure";
                        ExactLimitExp.InnerHtml = "of Measure"; 
                        BindDDLMeasures();
                    }
                    else
                    {
                        HighLimitExp.InnerHtml = "of Presentation";
                        LowLimitExp.InnerHtml = "of Presentation";
                        ExactLimitExp.InnerHtml = "of Presentation";
                        BindDDLPresentation();
                    }
                    BindDDLPrefix_OnExpressedAsChanged();
                    BindFullDescription("Range");
                }
                else if (ctlConTypeCode.ControlValue != null && ValidationHelper.IsValidInt(ctlConTypeCode.ControlValue.ToString()))
                {
                    if (ctlExpressedAs.ControlTextValue == "Units of Measure")
                    {
                        HighLimitExp.InnerHtml = "of Measure";
                        LowLimitExp.InnerHtml = "of Measure";
                        ExactLimitExp.InnerHtml = "of Measure"; 
                        BindDDLMeasures();
                    }
                    else
                    {
                        HighLimitExp.InnerHtml = "of Presentation";
                        LowLimitExp.InnerHtml = "of Presentation";
                        ExactLimitExp.InnerHtml = "of Presentation";
                        BindDDLPresentation();
                    }
                    BindDDLPrefix_OnExpressedAsChanged();
                    BindFullDescription("Exact");
                }
                else
                {
                    if (ctlExpressedAs.ControlTextValue == "Units of Measure")
                    {
                        HighLimitExp.InnerHtml = "of Measure";
                        LowLimitExp.InnerHtml = "of Measure";
                        ExactLimitExp.InnerHtml = "of Measure";
                        BindDDLMeasures();
                    }
                    else
                    {
                        HighLimitExp.InnerHtml = "of Presentation";
                        LowLimitExp.InnerHtml = "of Presentation";
                        ExactLimitExp.InnerHtml = "of Presentation";
                        BindDDLPresentation();
                    }
                }
            Substance_Changed();
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;
            if (!ValidationHelper.IsValidInt(ctlSubstance_PK.Value.ToString())) errorMessage += "Substance name can't be empty.<br />";

            if (FormType != ucPopupFormType.Excipient)
            {
                //if (String.IsNullOrEmpty(ctlConTypeCode.ControlValue.ToString())) errorMessage += ctlConTypeCode.ControlEmptyErrorMessage + "<br />";

                string concType = "";

                if (ctlConTypeCode.ControlValue != null && ValidationHelper.IsValidInt(ctlConTypeCode.ControlValue.ToString()))
                {
                    Ssi__cont_voc_PK ssiConcType = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(ctlConTypeCode.ControlValue));
                    if (ssiConcType != null)
                    {
                        concType = ssiConcType.term_name_english;
                    }
                }

                if (concType.ToLower().Contains("range"))
                {
                    if (String.IsNullOrEmpty(ctlLowLimitNumPrefix.ControlValue.ToString())) errorMessage += ctlLowLimitNumPrefix.ControlEmptyErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlLowLimitDenPrefix.ControlValue.ToString())) errorMessage += ctlLowLimitDenPrefix.ControlEmptyErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlHighLimitNumPrefix.ControlValue.ToString())) errorMessage += ctlHighLimitNumPrefix.ControlEmptyErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlHighLimitDenPrefix.ControlValue.ToString())) errorMessage += ctlHighLimitDenPrefix.ControlEmptyErrorMessage + "<br />";


                    if (String.IsNullOrEmpty(ctlLowLimitNumUnit.ControlValue.ToString())) errorMessage += ctlLowLimitNumUnit.ControlEmptyErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlLowLimitDenUnit.ControlValue.ToString())) errorMessage += ctlLowLimitDenUnit.ControlEmptyErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlHighLimitNumUnit.ControlValue.ToString())) errorMessage += ctlHighLimitNumUnit.ControlEmptyErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlHighLimitDenUnit.ControlValue.ToString())) errorMessage += ctlHighLimitDenUnit.ControlEmptyErrorMessage + "<br />";

                    if (String.IsNullOrEmpty(ctlLowLimitNumVal.ControlValue.ToString())) errorMessage += ctlLowLimitNumVal.ControlEmptyErrorMessage + "<br />";
                    else if (!ValidationHelper.IsValidDecimal(ctlLowLimitNumVal.ControlValue.ToString())) errorMessage += ctlLowLimitNumVal.ControlErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlLowLimitDenVal.ControlValue.ToString())) errorMessage += ctlLowLimitDenVal.ControlEmptyErrorMessage + "<br />";
                    else if (!ValidationHelper.IsValidDecimal(ctlLowLimitDenVal.ControlValue.ToString())) errorMessage += ctlLowLimitDenVal.ControlErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlHighLimitNumVal.ControlValue.ToString())) errorMessage += ctlHighLimitNumVal.ControlEmptyErrorMessage + "<br />";
                    else if (!ValidationHelper.IsValidDecimal(ctlHighLimitNumVal.ControlValue.ToString())) errorMessage += ctlHighLimitNumVal.ControlErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlHighLimitDenVal.ControlValue.ToString())) errorMessage += ctlHighLimitDenVal.ControlEmptyErrorMessage + "<br />";
                    else if (!ValidationHelper.IsValidDecimal(ctlHighLimitDenVal.ControlValue.ToString())) errorMessage += ctlHighLimitDenVal.ControlErrorMessage + "<br />";
                }
                else if (!String.IsNullOrWhiteSpace(concType))
                {
                    if (String.IsNullOrEmpty(ctlExactNumVal.ControlValue.ToString())) errorMessage += ctlExactNumVal.ControlEmptyErrorMessage + "<br />";
                    else if (!ValidationHelper.IsValidDecimal(ctlExactNumVal.ControlValue.ToString())) errorMessage += ctlExactNumVal.ControlErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlExactDenVal.ControlValue.ToString())) errorMessage += ctlExactDenVal.ControlEmptyErrorMessage + "<br />";
                    else if (!ValidationHelper.IsValidDecimal(ctlExactDenVal.ControlValue.ToString())) errorMessage += ctlExactDenVal.ControlErrorMessage + "<br />";

                    if (String.IsNullOrEmpty(ctlExactNumPrefix.ControlValue.ToString())) errorMessage += ctlExactNumPrefix.ControlEmptyErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlExactNumUnit.ControlValue.ToString())) errorMessage += ctlExactNumUnit.ControlEmptyErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlExactDenPrefix.ControlValue.ToString())) errorMessage += ctlExactDenPrefix.ControlEmptyErrorMessage + "<br />";
                    if (String.IsNullOrEmpty(ctlExactDenUnit.ControlValue.ToString())) errorMessage += ctlExactDenUnit.ControlEmptyErrorMessage + "<br />";
                }
            }

            // If errors were found, showing them in modal popup
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }

        private void BindDDLConType()
        {
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_PKOperations.GetConcentrationTypes();

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.ToString().CompareTo(s2.term_name_english.ToString());
            });

            ctlConTypeCode.SourceValueProperty = "ssi__cont_voc_PK";
            ctlConTypeCode.SourceTextExpression = "term_name_english";
            ctlConTypeCode.FillControl<Ssi__cont_voc_PK>(items);
        }

        private void BindDDLExpressedBy()
        {
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_PKOperations.GetEntitiesByListName("ExpressedBy");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.ToString().CompareTo(s2.term_name_english.ToString());
            });

            ctlExpressedAs.SourceValueProperty = "ssi__cont_voc_PK";
            ctlExpressedAs.SourceTextExpression = "term_name_english";
            ctlExpressedAs.FillControl<Ssi__cont_voc_PK>(items);
        }


        private void BindDDLUnits()
        {
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_PKOperations.GetEntitiesByListName("Units of Measure");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                if (s1.custom_sort == null && s2.custom_sort == null)
                    return s1.Description.ToString().CompareTo(s2.Description.ToString());
                if (s1.custom_sort != null && s2.custom_sort != null)
                    return s1.custom_sort.Value.CompareTo(s2.custom_sort.Value);
                if (s1.custom_sort != null) return -1;
                return 1;
            });

            ctlExactNumUnit.SourceValueProperty = "ssi__cont_voc_PK";
            ctlExactNumUnit.SourceTextExpression = "Description";
            ctlExactNumUnit.FillControl<Ssi__cont_voc_PK>(items);

            ctlHighLimitNumUnit.SourceValueProperty = "ssi__cont_voc_PK";
            ctlHighLimitNumUnit.SourceTextExpression = "Description";
            ctlHighLimitNumUnit.FillControl<Ssi__cont_voc_PK>(items);

            ctlLowLimitNumUnit.SourceValueProperty = "ssi__cont_voc_PK";
            ctlLowLimitNumUnit.SourceTextExpression = "Description";
            ctlLowLimitNumUnit.FillControl<Ssi__cont_voc_PK>(items);
        }

        private void BindDDLMeasures()
        {
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_PKOperations.GetEntitiesByListName("Measure_lower");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                if (s1.custom_sort == null && s2.custom_sort == null)
                    return s1.Description.ToString().CompareTo(s2.Description.ToString());
                if (s1.custom_sort != null && s2.custom_sort != null)
                    return s1.custom_sort.Value.CompareTo(s2.custom_sort.Value);
                if (s1.custom_sort != null) return -1;
                return 1;
            });

            ctlExactDenUnit.ControlBoundItems.Clear();
            ctlExactDenUnit.SourceValueProperty = "ssi__cont_voc_PK";
            ctlExactDenUnit.SourceTextExpression = "Description";
            ctlExactDenUnit.FillControl<Ssi__cont_voc_PK>(items);

            ctlLowLimitDenUnit.ControlBoundItems.Clear();
            ctlLowLimitDenUnit.SourceValueProperty = "ssi__cont_voc_PK";
            ctlLowLimitDenUnit.SourceTextExpression = "Description";
            ctlLowLimitDenUnit.FillControl<Ssi__cont_voc_PK>(items);

            ctlHighLimitDenUnit.ControlBoundItems.Clear();
            ctlHighLimitDenUnit.SourceValueProperty = "ssi__cont_voc_PK";
            ctlHighLimitDenUnit.SourceTextExpression = "Description";
            ctlHighLimitDenUnit.FillControl<Ssi__cont_voc_PK>(items);
        }


        private void BindDDLPresentation()
        {
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_PKOperations.GetEntitiesByListName("Units of Presentation");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                if (s1.custom_sort == null && s2.custom_sort == null)
                    return s1.Description.ToString().CompareTo(s2.Description.ToString());
                if (s1.custom_sort != null && s2.custom_sort != null)
                    return s1.custom_sort.Value.CompareTo(s2.custom_sort.Value);
                if (s1.custom_sort != null) return -1;
                return 1;
            });

            ctlExactDenUnit.ControlBoundItems.Clear();
            ctlExactDenUnit.SourceValueProperty = "ssi__cont_voc_PK";
            ctlExactDenUnit.SourceTextExpression = "Description";
            ctlExactDenUnit.FillControl<Ssi__cont_voc_PK>(items);

            ctlLowLimitDenUnit.ControlBoundItems.Clear();
            ctlLowLimitDenUnit.SourceValueProperty = "ssi__cont_voc_PK";
            ctlLowLimitDenUnit.SourceTextExpression = "Description";
            ctlLowLimitDenUnit.FillControl<Ssi__cont_voc_PK>(items);

            ctlHighLimitDenUnit.ControlBoundItems.Clear();
            ctlHighLimitDenUnit.SourceValueProperty = "ssi__cont_voc_PK";
            ctlHighLimitDenUnit.SourceTextExpression = "Description";
            ctlHighLimitDenUnit.FillControl<Ssi__cont_voc_PK>(items);
        }


        private void BindDDLPrefix_OnExpressedAsChanged()
        {
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_PKOperations.GetPrefixes();

            //items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            //{
            //return s1.custom_sort.ToString().CompareTo(s2.custom_sort.ToString());

            //});
            //Ssi__cont_voc_PK single = items.Find(sing => sing.Description.ToLower().Contains("single"));

            //if (ctlExpressedAs.ControlBoundItems.Count > 0 && ctlExpressedAs.ControlTextValue.ToLower().Contains("presentation"))
            //{
            //    /*
            //    ctlExactDenPrefix.ControlBoundItems.Clear();
            //    ctlHighLimitDenPrefix.ControlBoundItems.Clear();
            //    ctlLowLimitDenPrefix.ControlBoundItems.Clear();

            //    Ssi__cont_voc_PK single = items.Find(sing => sing.Description.ToLower().Contains("single"));

            //    if (single != null)
            //    {
            //        ctlExactDenPrefix.ControlBoundItems.Add(new ListItem(single.Description, single.ssi__cont_voc_PK.ToString()));
            //        ctlHighLimitDenPrefix.ControlBoundItems.Add(new ListItem(single.Description, single.ssi__cont_voc_PK.ToString()));
            //        ctlLowLimitDenPrefix.ControlBoundItems.Add(new ListItem(single.Description, single.ssi__cont_voc_PK.ToString()));
            //    }
            //     * */
            //    List<Ssi__cont_voc_PK> singleList = new List<Ssi__cont_voc_PK>();
            //    singleList.Add(single);
            //    items = singleList;

            //}
            /*
            else
            {
             * */
            ctlExactDenPrefix.SourceValueProperty = "ssi__cont_voc_PK";
            ctlExactDenPrefix.SourceTextExpression = "Description";
            ctlExactDenPrefix.FillControl<Ssi__cont_voc_PK>(items);

            ctlHighLimitDenPrefix.SourceValueProperty = "ssi__cont_voc_PK";
            ctlHighLimitDenPrefix.SourceTextExpression = "Description";
            ctlHighLimitDenPrefix.FillControl<Ssi__cont_voc_PK>(items);

            ctlLowLimitDenPrefix.SourceValueProperty = "ssi__cont_voc_PK";
            ctlLowLimitDenPrefix.SourceTextExpression = "Description";
            ctlLowLimitDenPrefix.FillControl<Ssi__cont_voc_PK>(items);

            //if (ctlExpressedAs.ControlBoundItems.Count > 0 && ctlExpressedAs.ControlTextValue.ToLower().Contains("presentation"))
            //{
            //    ctlLowLimitDenPrefix.ControlValue = single.ssi__cont_voc_PK;
            //    ctlExactDenPrefix.ControlValue = single.ssi__cont_voc_PK;
            //}

            //}
        }

        private void BindDDLPrefix()
        {
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_PKOperations.GetPrefixes();

            ctlExactDenPrefix.SourceValueProperty = "ssi__cont_voc_PK";
            ctlExactDenPrefix.SourceTextExpression = "Description";
            ctlExactDenPrefix.FillControl<Ssi__cont_voc_PK>(items);

            ctlHighLimitDenPrefix.SourceValueProperty = "ssi__cont_voc_PK";
            ctlHighLimitDenPrefix.SourceTextExpression = "Description";
            ctlHighLimitDenPrefix.FillControl<Ssi__cont_voc_PK>(items);

            ctlLowLimitDenPrefix.SourceValueProperty = "ssi__cont_voc_PK";
            ctlLowLimitDenPrefix.SourceTextExpression = "Description";
            ctlLowLimitDenPrefix.FillControl<Ssi__cont_voc_PK>(items);


            ctlExactNumPrefix.SourceValueProperty = "ssi__cont_voc_PK";
            ctlExactNumPrefix.SourceTextExpression = "Description";
            ctlExactNumPrefix.FillControl<Ssi__cont_voc_PK>(items);

            ctlHighLimitNumPrefix.SourceValueProperty = "ssi__cont_voc_PK";
            ctlHighLimitNumPrefix.SourceTextExpression = "Description";
            ctlHighLimitNumPrefix.FillControl<Ssi__cont_voc_PK>(items);

            ctlLowLimitNumPrefix.SourceValueProperty = "ssi__cont_voc_PK";
            ctlLowLimitNumPrefix.SourceTextExpression = "Description";
            ctlLowLimitNumPrefix.FillControl<Ssi__cont_voc_PK>(items);
        }

        private void BindDDLPrefixSubstanceClass()
        {
            //List<Ssi__cont_voc_PK> items = _ssi_cont_voc_PKOperations.GetPrefixesSubstanceClass();

            //items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            //{
            //    return s1.term_name_english.ToString().CompareTo(s2.term_name_english.ToString());
            //});

            //ctlExactNumPrefix.SourceValueProperty = "ssi__cont_voc_PK";
            //ctlExactNumPrefix.SourceTextExpression = "term_name_english";
            //ctlExactNumPrefix.FillControl<Ssi__cont_voc_PK>(items);

            //ctlHighLimitNumPrefix.SourceValueProperty = "ssi__cont_voc_PK";
            //ctlHighLimitNumPrefix.SourceTextExpression = "term_name_english";
            //ctlHighLimitNumPrefix.FillControl<Ssi__cont_voc_PK>(items);

            //ctlLowLimitNumPrefix.SourceValueProperty = "ssi__cont_voc_PK";
            //ctlLowLimitNumPrefix.SourceTextExpression = "term_name_english";
            //ctlLowLimitNumPrefix.FillControl<Ssi__cont_voc_PK>(items);
        }

        public override void BindForm(object id, string arg)
        {
            ctlHighLimitNumUnit.CurrentControlState = ControlState.YouCantChangeMe;
            ctlHighLimitDenPrefix.CurrentControlState = ControlState.YouCantChangeMe;
            ctlHighLimitDenUnit.CurrentControlState = ControlState.YouCantChangeMe;
            ctlFormType.Value = FormType.ToString();
            string concType = "";

            if (id != null && id.ToString() != "")
            {
                switch (FormType)
                {

                    case ucPopupFormType.Adjuvant:

                        infoSubName.InnerText = "";
                        infoConSubName.InnerText = "";


                        pnlLowLimitVisible.Value = "false";
                        pnlHighLimitVisible.Value = "false";
                        pnlExactLimitVisible.Value = "false";

                        Adjuvant_PK adjuvant = ((List<Adjuvant_PK>)_entityData).Find(item => item != null && item.adjuvant_PK == (int)id);
                        //pnlLowLimit.Visible = false;
                        //pnlHighLimit.Visible = false;
                        //pnlExactLimit.Visible = false;



                        //Adjuvant_PK adjuvant = _adjuvant_PKOperations.GetEntity(id);

                        if (adjuvant != null)
                        {
                            Substance_PK sub1 = adjuvant.substancecode_FK != null ? _substance_PKOperations.GetEntity(adjuvant.substancecode_FK) : null;
                            if (sub1 != null)
                            {

                                ctlSubstance_PK.Value = sub1.substance_PK.ToString();
                                substanceSearch.ControlValue = sub1.substance_name;
                                infoSubName.InnerText = sub1.substance_name;
                                infoConSubName.InnerText = sub1.substance_name;
                            }

                            if (adjuvant.concentrationtypecode != null)
                            {
                                Ssi__cont_voc_PK ssiConType = _ssi_cont_voc_PKOperations.GetEntity(adjuvant.concentrationtypecode);
                                concType = ssiConType != null ? ssiConType.term_name_english : "";

                                ctlConTypeCode.ControlValue = adjuvant.concentrationtypecode == null ? String.Empty : adjuvant.concentrationtypecode.ToString();

                                if (concType == "Range")
                                {

                                    //pnlLowLimit.Visible = true;
                                    //pnlHighLimit.Visible = true;
                                    //pnlExactLimit.Visible = false;
                                    pnlLowLimitVisible.Value = "true";
                                    pnlHighLimitVisible.Value = "true";
                                    pnlExactLimitVisible.Value = "false";

                                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(adjuvant.ExpressedBy_FK);
                                    ctlExpressedAs.ControlValue = ssiTmp != null ? ssiTmp.ssi__cont_voc_PK.ToString() : "";
                                    if (ssiTmp != null && ssiTmp.term_name_english != "")
                                    {
                                        if (ssiTmp.term_name_english == "Units of Measure")
                                            BindDDLMeasures();
                                        else
                                            BindDDLPresentation();
                                    }

                                    ctlHighLimitNumUnit.ControlValue = adjuvant.higamountnumerunit == null ? String.Empty : adjuvant.higamountnumerunit.ToString();
                                    ctlHighLimitDenUnit.ControlValue = adjuvant.highamountdenomunit == null ? String.Empty : adjuvant.highamountdenomunit.ToString();
                                    ctlHighLimitNumPrefix.ControlValue = adjuvant.highamountnumerprefix == null ? String.Empty : adjuvant.highamountnumerprefix.ToString();
                                    ctlHighLimitDenPrefix.ControlValue = adjuvant.highamountdenomprefix == null ? String.Empty : adjuvant.highamountdenomprefix.ToString();

                                    ctlLowLimitNumUnit.ControlValue = adjuvant.lowamountnumerunit == null ? String.Empty : adjuvant.lowamountnumerunit.ToString();
                                    ctlLowLimitDenUnit.ControlValue = adjuvant.lowamountdenomunit == null ? String.Empty : adjuvant.lowamountdenomunit.ToString();
                                    ctlLowLimitNumPrefix.ControlValue = adjuvant.lowamountnumerprefix == null ? String.Empty : adjuvant.lowamountnumerprefix.ToString();
                                    ctlLowLimitDenPrefix.ControlValue = adjuvant.lowamountdenomprefix == null ? String.Empty : adjuvant.lowamountdenomprefix.ToString();

                                    ctlHighLimitNumVal.ControlValue = adjuvant.highamountnumervalue == null ? String.Empty : adjuvant.highamountnumervalue.ToString();
                                    ctlHighLimitDenVal.ControlValue = adjuvant.highamountdenomvalue == null ? String.Empty : adjuvant.highamountdenomvalue.ToString();
                                    ctlLowLimitNumVal.ControlValue = adjuvant.lowamountnumervalue == null ? String.Empty : adjuvant.lowamountnumervalue.ToString();
                                    ctlLowLimitDenVal.ControlValue = adjuvant.lowamountdenomvalue == null ? String.Empty : adjuvant.lowamountdenomvalue.ToString();

                                    LowLimitDenPrefix_Changed(null, null);
                                    LowLimitDenUnit_Changed(null, null);
                                    LowLimitDenVal_Changed(null, null);
                                    LowLimitNumPrefix_Changed(null, null);
                                    LowLimitNumUnit_Changed(null, null);
                                    LowLimitNumVal_Changed(null, null);
                                    HighLimitDenPrefix_Changed(null, null);
                                    HighLimitDenUnit_Changed(null, null);
                                    HighLimitDenVal_Changed(null, null);
                                    HighLimitNumPrefix_Changed(null, null);
                                    HighLimitNumUnit_Changed(null, null);
                                    HighLimitNumVal_Changed(null, null);
                                }
                                else
                                {

                                    //pnlExactLimit.Visible = true;
                                    //pnlLowLimit.Visible = false;
                                    //pnlHighLimit.Visible = false;
                                    pnlLowLimitVisible.Value = "false";
                                    pnlHighLimitVisible.Value = "false";
                                    pnlExactLimitVisible.Value = "true";

                                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(adjuvant.ExpressedBy_FK);
                                    ctlExpressedAs.ControlValue = ssiTmp != null ? ssiTmp.ssi__cont_voc_PK : null;
                                    if (ssiTmp != null && ssiTmp.term_name_english != "")
                                    {
                                        if (ssiTmp.term_name_english == "Units of Measure")
                                            BindDDLMeasures();
                                        else
                                            BindDDLPresentation();
                                    }

                                    ctlExactNumUnit.ControlValue = adjuvant.lowamountnumerunit == null ? String.Empty : adjuvant.lowamountnumerunit.ToString();
                                    ctlExactDenUnit.ControlValue = adjuvant.lowamountdenomunit == null ? String.Empty : adjuvant.lowamountdenomunit.ToString();
                                    ctlExactNumPrefix.ControlValue = adjuvant.lowamountnumerprefix == null ? String.Empty : adjuvant.lowamountnumerprefix.ToString();
                                    ctlExactDenPrefix.ControlValue = adjuvant.lowamountdenomprefix == null ? String.Empty : adjuvant.lowamountdenomprefix.ToString();

                                    ctlExactNumVal.ControlValue = adjuvant.lowamountnumervalue == null ? String.Empty : adjuvant.lowamountnumervalue.ToString();
                                    ctlExactDenVal.ControlValue = adjuvant.lowamountdenomvalue == null ? String.Empty : adjuvant.lowamountdenomvalue.ToString();

                                    ExactDenUnit_Changed(null, null);
                                    ExactNumUnit_Changed(null, null);
                                    ExactDenPrefix_Changed(null, null);
                                    ExactDenVal_Changed(null, null);
                                    ExactNumPrefix_Changed(null, null);
                                    ExactNumVal_Changed(null, null);
                                }
                            }
                        }
                        break;

                    case ucPopupFormType.Excipient:

                        infoSubName.InnerText = "";
                        infoConSubName.InnerText = "";


                        //pnlExactLimit.Visible = false;
                        //pnlLowLimit.Visible = false;
                        //pnlHighLimit.Visible = false;
                        pnlLowLimitVisible.Value = "false";
                        pnlHighLimitVisible.Value = "false";
                        pnlExactLimitVisible.Value = "false";
                        ctlConTypeCode.Visible = false;
                        ctlExpressedAs.Visible = false;


                        Excipient_PK excipient = ((List<Excipient_PK>)_entityData).Find(item => item != null && item.excipient_PK == (int)id);

                        //Excipient_PK excipient = _excipient_PKOPerations.GetEntity(id);

                        if (excipient != null)
                        {
                            Substance_PK sub2 = excipient.substancecode_FK.HasValue ? _substance_PKOperations.GetEntity(excipient.substancecode_FK) : null;
                            if (sub2 != null)
                            {

                                ctlSubstance_PK.Value = sub2.substance_PK.ToString();
                                substanceSearch.ControlValue = sub2.substance_name;

                                infoSubName.InnerText = sub2.substance_name;
                                infoConSubName.InnerText = sub2.substance_name;
                            }
                        }

                        break;

                    case ucPopupFormType.ActiveIngredient:

                        infoSubName.InnerText = "";
                        infoConSubName.InnerText = "";


                        //pnlLowLimit.Visible = false;
                        //pnlHighLimit.Visible = false;
                        //pnlExactLimit.Visible = false;
                        pnlLowLimitVisible.Value = "false";
                        pnlHighLimitVisible.Value = "false";
                        pnlExactLimitVisible.Value = "false";

                        Activeingredient_PK ingredient = ((List<Activeingredient_PK>)_entityData).Find(item => item != null && item.activeingredient_PK == (int)id);

                        //Activeingredient_PK ingredient = _activeIngredieent_PKOperations.GetEntity(id);

                        if (ingredient != null)
                        {
                            Substance_PK sub3 = ingredient.substancecode_FK != null ? _substance_PKOperations.GetEntity(ingredient.substancecode_FK) : null;
                            if (sub3 != null)
                            {

                                ctlSubstance_PK.Value = sub3.substance_PK.ToString();
                                substanceSearch.ControlValue = sub3.substance_name;
                                infoSubName.InnerText = sub3.substance_name;
                                infoConSubName.InnerText = sub3.substance_name;
                            }

                            if (ingredient.concentrationtypecode != null)
                            {
                                Ssi__cont_voc_PK ssiConType = _ssi_cont_voc_PKOperations.GetEntity(ingredient.concentrationtypecode);
                                concType = ssiConType != null ? ssiConType.term_name_english : "";

                                ctlConTypeCode.ControlValue = ingredient.concentrationtypecode == null ? String.Empty : ingredient.concentrationtypecode.ToString();

                                if (concType == "Range")
                                {

                                    //pnlLowLimit.Visible = true;
                                    //pnlHighLimit.Visible = true;
                                    //pnlExactLimit.Visible = false;
                                    pnlLowLimitVisible.Value = "true";
                                    pnlHighLimitVisible.Value = "true";
                                    pnlExactLimitVisible.Value = "false";

                                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(ingredient.ExpressedBy_FK);
                                    ctlExpressedAs.ControlValue = ssiTmp != null ? ssiTmp.ssi__cont_voc_PK : null;
                                    if (ssiTmp != null && ssiTmp.term_name_english != "")
                                    {
                                        if (ssiTmp.term_name_english == "Units of Measure")
                                            BindDDLMeasures();
                                        else
                                            BindDDLPresentation();
                                    }

                                    ctlHighLimitNumUnit.ControlValue = ingredient.highamountnumerunit == null ? String.Empty : ingredient.highamountnumerunit.ToString();
                                    ctlHighLimitDenUnit.ControlValue = ingredient.highamountdenomunit == null ? String.Empty : ingredient.highamountdenomunit.ToString();
                                    ctlHighLimitNumPrefix.ControlValue = ingredient.highamountnumerprefix == null ? String.Empty : ingredient.highamountnumerprefix.ToString();
                                    ctlHighLimitDenPrefix.ControlValue = ingredient.highamountdenomprefix == null ? String.Empty : ingredient.highamountdenomprefix.ToString();

                                    ctlLowLimitNumUnit.ControlValue = ingredient.lowamountnumerunit == null ? String.Empty : ingredient.lowamountnumerunit.ToString();
                                    ctlLowLimitDenUnit.ControlValue = ingredient.lowamountdenomunit == null ? String.Empty : ingredient.lowamountdenomunit.ToString();
                                    ctlLowLimitNumPrefix.ControlValue = ingredient.lowamountnumerprefix == null ? String.Empty : ingredient.lowamountnumerprefix.ToString();
                                    ctlLowLimitDenPrefix.ControlValue = ingredient.lowamountdenomprefix == null ? String.Empty : ingredient.lowamountdenomprefix.ToString();

                                    ctlHighLimitNumVal.ControlValue = ingredient.highamountnumervalue == null ? String.Empty : ingredient.highamountnumervalue.ToString();
                                    ctlHighLimitDenVal.ControlValue = ingredient.highamountdenomvalue == null ? String.Empty : ingredient.highamountdenomvalue.ToString();
                                    ctlLowLimitNumVal.ControlValue = ingredient.lowamountnumervalue == null ? String.Empty : ingredient.lowamountnumervalue.ToString();
                                    ctlLowLimitDenVal.ControlValue = ingredient.lowamountdenomvalue == null ? String.Empty : ingredient.lowamountdenomvalue.ToString();

                                    LowLimitDenPrefix_Changed(null, null);
                                    LowLimitDenUnit_Changed(null, null);
                                    LowLimitDenVal_Changed(null, null);
                                    LowLimitNumPrefix_Changed(null, null);
                                    LowLimitNumUnit_Changed(null, null);
                                    LowLimitNumVal_Changed(null, null);
                                    HighLimitDenPrefix_Changed(null, null);
                                    HighLimitDenUnit_Changed(null, null);
                                    HighLimitDenVal_Changed(null, null);
                                    HighLimitNumPrefix_Changed(null, null);
                                    HighLimitNumUnit_Changed(null, null);
                                    HighLimitNumVal_Changed(null, null);
                                }
                                else
                                {

                                    //pnlExactLimit.Visible = true;
                                    //pnlLowLimit.Visible = false;
                                    //pnlHighLimit.Visible = false;
                                    pnlLowLimitVisible.Value = "false";
                                    pnlHighLimitVisible.Value = "false";
                                    pnlExactLimitVisible.Value = "true";


                                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(ingredient.ExpressedBy_FK);
                                    ctlExpressedAs.ControlValue = ssiTmp != null ? ssiTmp.ssi__cont_voc_PK : null;
                                    if (ssiTmp != null && ssiTmp.term_name_english != "")
                                    {
                                        if (ssiTmp.term_name_english == "Units of Measure")
                                            BindDDLMeasures();
                                        else
                                            BindDDLPresentation();
                                    }

                                    ctlExactNumUnit.ControlValue = ingredient.lowamountnumerunit == null ? String.Empty : ingredient.lowamountnumerunit.ToString();
                                    ctlExactDenUnit.ControlValue = ingredient.lowamountdenomunit == null ? String.Empty : ingredient.lowamountdenomunit.ToString();
                                    ctlExactNumPrefix.ControlValue = ingredient.lowamountnumerprefix == null ? String.Empty : ingredient.lowamountnumerprefix.ToString();
                                    ctlExactDenPrefix.ControlValue = ingredient.lowamountdenomprefix == null ? String.Empty : ingredient.lowamountdenomprefix.ToString();

                                    ctlExactNumVal.ControlValue = ingredient.lowamountnumervalue == null ? String.Empty : ingredient.lowamountnumervalue.ToString();
                                    ctlExactDenVal.ControlValue = ingredient.lowamountdenomvalue == null ? String.Empty : ingredient.lowamountdenomvalue.ToString();

                                    ExactDenUnit_Changed(null, null);
                                    ExactNumUnit_Changed(null, null);
                                    ExactDenPrefix_Changed(null, null);
                                    ExactDenVal_Changed(null, null);
                                    ExactNumPrefix_Changed(null, null);
                                    ExactNumVal_Changed(null, null);

                                }
                            }
                        }
                        break;
                }

                infoSubNameText.Visible = true;
                infoSubNameText2.Visible = true;
            }
            else
            {

                pnlLowLimitVisible.Value = "false";
                pnlHighLimitVisible.Value = "false";
                pnlExactLimitVisible.Value = "false";
            }
            //pnlLowLimit.Visible = true;
            //pnlHighLimit.Visible = true;
            //pnlExactLimit.Visible = true;
            infoRangeMinText.Visible = true;
            infoRangeNText.Visible = true;
            infoRangeDText.Visible = true;
            infoUnitText.Visible = true;
            infoSubNameText.Visible = true;
            infoSubNameText2.Visible = true;
            infoConcTypeText1.Visible = true;
            infoConcTypeText2.Visible = true;
            Substance_Changed();
        }

        #endregion

        #region Form methods

        public void btnOk_Click(object sender, EventArgs e)
        {
            Substance_Changed();
            if (ValidateForm(null))
            {
                SaveForm(_entity_PK, null);
                PopupControls_Entity_Container.Style["display"] = "none";

                ctlUcPopupVisible.Value = "false";
                if (OnOkButtonClick != null)
                    OnOkButtonClick(sender, new FormDetailsEventArgs(_entityData));
                ClearForm(null);
            }
            else
            {
                String arg = ctlConTypeCode.ControlTextValue.ToLower().Contains("range") ? "Range" : "Exact";
                BindFullDescription(arg);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            ctlUcPopupVisible.Value = "false";
            ClearForm(null);

            if (OnCloseButtonClick != null)
            {
                OnCloseButtonClick(sender, null);
            }
        }

        protected void btnClosePopupForm_Click(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            ctlUcPopupVisible.Value = "false";
            ClearForm(null);

            if (OnCloseButtonClick != null)
            {
                OnCloseButtonClick(sender, null);
            }
        }

        void SubNameSearcher_OnListItemSelected(object sender, FormListEventArgs e)
        {
            Substance_PK sub = _substance_PKOperations.GetEntity(e.DataItemID);

            if (sub != null && sub.substance_name != null)
            {

                ctlSubstance_PK.Value = sub.substance_PK.ToString();
                substanceSearch.ControlValue = sub.substance_name;

                infoSubName.InnerText = sub.substance_name;
                infoSubNameText2.Visible = true;
                infoConSubName.InnerText = sub.substance_name;
                infoSubNameText.Visible = true;
            }
        }

        void SubNameSearcher_OnSearchClick(object sender, EventArgs e)
        {

            //  SubNameSearcher.ShowModalSearcher("SubName");
        }

        void SubNameSearcherDisplay_OnRemoveClick(object sender, EventArgs e)
        {

            // SubNameSearcherDisplay.EnableSearcher(true);
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