using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;
using System.Configuration;

namespace AspNetUI.Views.Shared.UserControl.Popup
{

    public partial class PPSubstancePopup : System.Web.UI.UserControl
    {
        ISubstance_PKOperations _substance_PKOperations;
        ISsi__cont_voc_PKOperations _ssi_cont_voc_PKOperations;

        public virtual event EventHandler<FormEventArgs<PharmaceuticalProductSubstance>> OnOkButtonClick;
        public virtual event EventHandler<EventArgs> OnCloseButtonClick;

        public string callbackInvocation = null;

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

        private PharmaceuticalProductSubstance.SubstanceType _substanceType
        {
            get { return ViewState["PPSubstancePopupSubstanceType"] != null ? (PharmaceuticalProductSubstance.SubstanceType)ViewState["PPSubstancePopupSubstanceType"] : PharmaceuticalProductSubstance.SubstanceType.ActiveIngredient; }
            set { ViewState["PPSubstancePopupSubstanceType"] = value; }
        }

        private PharmaceuticalProductSubstance _substance
        {
            get { return (PharmaceuticalProductSubstance)ViewState["PPSubstancePopupSubstance"]; }
            set { ViewState["PPSubstancePopupSubstance"] = value; }
        }

        #endregion

        #region Operations

        public void ShowModalForm(PharmaceuticalProductSubstance substance, PharmaceuticalProductSubstance.SubstanceType substanceType)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            _substanceType = substanceType;

            substance = substance ?? new PharmaceuticalProductSubstance();
            substance.substancetype = substanceType.ToString();
            _substance = substance;

            switch (substanceType)
            {
                case PharmaceuticalProductSubstance.SubstanceType.Adjuvant:
                    infoType.InnerText = "Adjuvant";
                    divHeader.InnerText = "Adjuvant";
                    break;

                case PharmaceuticalProductSubstance.SubstanceType.Excipient:
                    infoType.InnerText = "Excipient";
                    divHeader.InnerText = "Excipient";
                    break;

                case PharmaceuticalProductSubstance.SubstanceType.ActiveIngredient:
                    infoType.InnerText = "Active Ingredient";
                    divHeader.InnerText = "Active Ingredient";
                    break;
            }
            ctlUcPopupVisible.Value = "true";

            ClearForm(null);
            FillDataDefinitions(null);
            BindForm(null);
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

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

            if (!mgr.IsClientScriptIncludeRegistered(typeof(Template.Default), "associatedJS"))
            {
                String url = mgr.GetWebResourceUrl(typeof(Template.Default), "AspNetUI.Scripts.ucPopup.js");
                ScriptManager.RegisterClientScriptInclude(this, typeof(Template.Default), "associatedJS", url);
            }

            if (!mgr.IsClientScriptBlockRegistered(cstype, "callbackInovcation"))
            {
                ScriptManager.RegisterClientScriptBlock(this, cstype, "callbackInovcation", "var ucPopupInvokeCallback=\"" + callbackInvocation + "\";", true);
            }

        }

        public object SaveForm(object args)
        {
            var substance = _substance;

            substance.substancecode_FK = ValidationHelper.IsValidInt(ctlSubstance_PK.Value) ? (int?)Convert.ToInt32(ctlSubstance_PK.Value) : null;
            substance.concentrationtypecode = ValidationHelper.IsValidInt(ctlConTypeCode.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlConTypeCode.ControlValue) : null;
            substance.expressedby_FK = ValidationHelper.IsValidInt(ctlExpressedAs.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlExpressedAs.ControlValue) : null;

            if (substance.concentrationtypecode.HasValue)
            {
                string concise = infoConSubName.InnerText + infoConRangeNMin.InnerText + infoConNumMinPrefix.InnerText + infoConRangeNMax.InnerText + infoConNumMaxPrefix.InnerText + infoConNUnit.InnerText +
                                 infoMeasure2.InnerText + infoConRangeDMin.InnerText + infoConRangeDMax.InnerText + infoConDenPrefix.InnerText + infoConDenExp.InnerText.Trim();
                substance.concise = String.IsNullOrWhiteSpace(concise) ? null : concise.Trim();

                substance.lowamountnumervalue = ValidationHelper.IsValidDecimal(ctlExactNumVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlExactNumVal.ControlTextValue) : null;
                substance.lowamountdenomvalue = ValidationHelper.IsValidDecimal(ctlExactDenVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlExactDenVal.ControlTextValue) : null;
                substance.lowamountnumerunit = ctlExactNumUnit.ControlValue.ToString();
                substance.lowamountdenomunit = ctlExactDenUnit.ControlValue.ToString();
                substance.lowamountnumerprefix = ctlExactNumPrefix.ControlValue.ToString();
                substance.lowamountdenomprefix = ctlExactDenPrefix.ControlValue.ToString();

                substance.highamountnumervalue = null;
                substance.highamountdenomvalue = null;
                substance.highamountnumerprefix = null;
                substance.highamountdenomprefix = null;
                substance.highamountnumerunit = null;
                substance.highamountdenomunit = null;

                if (ctlConTypeCode.ControlTextValue == "Range")
                {
                    substance.lowamountnumervalue = ValidationHelper.IsValidDecimal(ctlLowLimitNumVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlLowLimitNumVal.ControlTextValue) : null;
                    substance.lowamountdenomvalue = ValidationHelper.IsValidDecimal(ctlLowLimitDenVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlLowLimitDenVal.ControlTextValue) : null;
                    substance.lowamountnumerprefix = ctlLowLimitNumPrefix.ControlValue.ToString();
                    substance.lowamountdenomprefix = ctlLowLimitDenPrefix.ControlValue.ToString();
                    substance.lowamountnumerunit = ctlLowLimitNumUnit.ControlValue.ToString();
                    substance.lowamountdenomunit = ctlLowLimitDenUnit.ControlValue.ToString();

                    substance.highamountnumervalue = ValidationHelper.IsValidDecimal(ctlHighLimitNumVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlHighLimitNumVal.ControlTextValue) : null;
                    substance.highamountdenomvalue = ValidationHelper.IsValidDecimal(ctlHighLimitDenVal.ControlTextValue) ? (decimal?)Convert.ToDecimal(ctlHighLimitDenVal.ControlTextValue) : null;
                    substance.highamountnumerunit = ctlHighLimitNumUnit.ControlValue.ToString();
                    substance.highamountdenomunit = ctlHighLimitDenUnit.ControlValue.ToString();
                    substance.highamountnumerprefix = ctlHighLimitNumPrefix.ControlValue.ToString();
                    substance.highamountdenomprefix = ctlHighLimitDenPrefix.ControlValue.ToString();
                }
            }
            else
            {
                substance.concise = null;

                substance.lowamountnumervalue = null;
                substance.lowamountdenomvalue = null;
                substance.lowamountnumerunit = null;
                substance.lowamountdenomunit = null;
                substance.lowamountnumerprefix = null;
                substance.lowamountdenomprefix = null;

                substance.highamountnumervalue = null;
                substance.highamountdenomvalue = null;
                substance.highamountnumerprefix = null;
                substance.highamountdenomprefix = null;
                substance.highamountnumerunit = null;
                substance.highamountdenomunit = null;
            }

            substance.user_FK = SessionManager.Instance.CurrentUser.UserID;

            _substance = substance;

            return substance;
        }

        public void ClearForm(string arg)
        {
            ctlSubstance_PK.Value = "";
            substanceSearch.ControlValue = String.Empty;
            ctlConTypeCode.ControlValue = String.Empty;
            ctlExpressedAs.ControlValue = String.Empty;

            ctlHighLimitNumUnit.ControlValue = String.Empty;
            ctlHighLimitDenUnit.ControlValue = String.Empty;

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

            ctlHighLimitDenVal.CurrentControlState = ControlState.YouCanOnlyReadMe;

            ClearFullDescription();
        }

        public void FillDataDefinitions(string arg)
        {
            BindDDLPrefix();
            BindDDLConType();
            BindDDLUnits();
            BindDDLExpressedBy();
        }

        public void valueChanged(object sender, ValueChangedEventArgs e)
        {
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
            if (ctlConTypeCode.ControlTextValue != "Range")
            {

                infoConcTypeText2.Visible = true;
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

                ctlExactNumVal.ControlValue = Convert.ToDecimal(ctlExactNumVal.ControlTextValue).ToString("#0.#####");
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
                    prefixConNumMin = !String.IsNullOrWhiteSpace(prefix.Field8) ? " " + prefix.Field8.Trim() : "";
                }

                prefix = null;
                if (ValidationHelper.IsValidInt(ctlHighLimitNumPrefix.ControlValue.ToString()))
                    prefix = _ssi_cont_voc_PKOperations.GetEntity(int.Parse(ctlHighLimitNumPrefix.ControlValue.ToString()));

                if (prefix != null && !prefix.Description.ToLower().Contains("single"))
                {
                    prefixNumMax = prefix.Description.Contains("(") ? " " + prefix.Description.Remove(prefix.Description.IndexOf("(")).Trim() : " " + prefix.Description.Trim();
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
        }

        public void HighLimitDenUnit_Changed(object sender, ValueChangedEventArgs e)
        {
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
                    prefixConNumMin = !String.IsNullOrWhiteSpace(prefix.Field8) ? " " + prefix.Field8.Trim() : "";
                }

                prefix = null;
                if (ValidationHelper.IsValidInt(ctlHighLimitNumPrefix.ControlValue.ToString()))
                    prefix = _ssi_cont_voc_PKOperations.GetEntity(int.Parse(ctlHighLimitNumPrefix.ControlValue.ToString()));

                if (prefix != null && !prefix.Description.ToLower().Contains("single"))
                {
                    prefixNumMax = prefix.Description.Contains("(") ? " " + prefix.Description.Remove(prefix.Description.IndexOf("(")).Trim() : " " + prefix.Description.Trim();
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

                    infoRangeMinText.Visible = true;
                    infoConcType.InnerText = " " + concType + " ";

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
                    infoConcTypeText2.Visible = true;

                    ctlHighLimitNumPrefix.CurrentControlState = ControlState.ReadyForAction;
                    ctlHighLimitNumUnit.CurrentControlState = ControlState.ReadyForAction;
                    ctlHighLimitDenPrefix.CurrentControlState = ControlState.ReadyForAction;
                    ctlHighLimitDenUnit.CurrentControlState = ControlState.ReadyForAction;

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
                ClearFullDescription();
                pnlLowLimitVisible.Value = "false";
                pnlHighLimitVisible.Value = "false";
                pnlExactLimitVisible.Value = "false";
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
                HighLimitDenVal_Changed(null, null);
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

        public bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;
            if (!ValidationHelper.IsValidInt(ctlSubstance_PK.Value.ToString())) errorMessage += "Substance name can't be empty.<br />";

            if (_substanceType != PharmaceuticalProductSubstance.SubstanceType.Excipient)
            {
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
                ((Shared.Template.Default)this.Page.Master).ModalPopup.ShowModalPopup("Error!", errorMessage);
                //FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
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
                    return s1.Description.CompareTo(s2.Description);
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

            ctlExactDenPrefix.SourceValueProperty = "ssi__cont_voc_PK";
            ctlExactDenPrefix.SourceTextExpression = "Description";
            ctlExactDenPrefix.FillControl<Ssi__cont_voc_PK>(items);

            ctlHighLimitDenPrefix.SourceValueProperty = "ssi__cont_voc_PK";
            ctlHighLimitDenPrefix.SourceTextExpression = "Description";
            ctlHighLimitDenPrefix.FillControl<Ssi__cont_voc_PK>(items);

            ctlLowLimitDenPrefix.SourceValueProperty = "ssi__cont_voc_PK";
            ctlLowLimitDenPrefix.SourceTextExpression = "Description";
            ctlLowLimitDenPrefix.FillControl<Ssi__cont_voc_PK>(items);
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

        public void BindForm(object args)
        {
            var substance = _substance;

            ctlHighLimitNumUnit.CurrentControlState = ControlState.YouCantChangeMe;
            ctlHighLimitDenPrefix.CurrentControlState = ControlState.YouCantChangeMe;
            ctlHighLimitDenUnit.CurrentControlState = ControlState.YouCantChangeMe;
            ctlFormType.Value = _substanceType.ToString();

            string concType = "";

            infoSubName.InnerText = "";
            infoConSubName.InnerText = "";

            pnlLowLimitVisible.Value = "false";
            pnlHighLimitVisible.Value = "false";
            pnlExactLimitVisible.Value = "false";

            if (_substanceType == PharmaceuticalProductSubstance.SubstanceType.ActiveIngredient || _substanceType == PharmaceuticalProductSubstance.SubstanceType.Adjuvant)
            {
                ctlConTypeCode.Visible = true;
                ctlExpressedAs.Visible = true;
            }
            else if (_substanceType == PharmaceuticalProductSubstance.SubstanceType.Excipient)
            {
                ctlConTypeCode.Visible = false;
                ctlExpressedAs.Visible = false;
            }

            if (substance == null) return;

            Substance_PK sub = substance.substancecode_FK.HasValue ? _substance_PKOperations.GetEntity(substance.substancecode_FK) : null;
            if (sub != null)
            {
                ctlSubstance_PK.Value = sub.substance_PK.ToString();
                substanceSearch.ControlValue = sub.substance_name;
                infoSubName.InnerText = sub.substance_name;
                infoConSubName.InnerText = sub.substance_name;

                infoSubNameText.Visible = true;
                infoSubNameText2.Visible = true;
            }

            if (substance.concentrationtypecode.HasValue)
            {
                Ssi__cont_voc_PK ssiConType = _ssi_cont_voc_PKOperations.GetEntity(substance.concentrationtypecode);
                concType = ssiConType != null ? ssiConType.term_name_english : "";

                ctlConTypeCode.ControlValue = substance.concentrationtypecode.Value.ToString();

                if (concType == "Range")
                {
                    pnlLowLimitVisible.Value = "true";
                    pnlHighLimitVisible.Value = "true";
                    pnlExactLimitVisible.Value = "false";

                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(substance.expressedby_FK);
                    ctlExpressedAs.ControlValue = ssiTmp != null ? ssiTmp.ssi__cont_voc_PK.ToString() : "";
                    if (ssiTmp != null && ssiTmp.term_name_english != "")
                    {
                        if (ssiTmp.term_name_english == "Units of Measure")
                            BindDDLMeasures();
                        else
                            BindDDLPresentation();
                    }

                    ctlHighLimitNumUnit.ControlValue = substance.highamountnumerunit ?? String.Empty;
                    ctlHighLimitDenUnit.ControlValue = substance.highamountdenomunit ?? String.Empty;
                    ctlHighLimitNumPrefix.ControlValue = substance.highamountnumerprefix ?? String.Empty;
                    ctlHighLimitDenPrefix.ControlValue = substance.highamountdenomprefix ?? String.Empty;

                    ctlLowLimitNumUnit.ControlValue = substance.lowamountnumerunit ?? String.Empty;
                    ctlLowLimitDenUnit.ControlValue = substance.lowamountdenomunit ?? String.Empty;
                    ctlLowLimitNumPrefix.ControlValue = substance.lowamountnumerprefix ?? String.Empty;
                    ctlLowLimitDenPrefix.ControlValue = substance.lowamountdenomprefix ?? String.Empty;

                    ctlHighLimitNumVal.ControlValue = substance.highamountnumervalue == null ? String.Empty : substance.highamountnumervalue.ToString();
                    ctlHighLimitDenVal.ControlValue = substance.highamountdenomvalue == null ? String.Empty : substance.highamountdenomvalue.ToString();
                    ctlLowLimitNumVal.ControlValue = substance.lowamountnumervalue == null ? String.Empty : substance.lowamountnumervalue.ToString();
                    ctlLowLimitDenVal.ControlValue = substance.lowamountdenomvalue == null ? String.Empty : substance.lowamountdenomvalue.ToString();

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
                    pnlLowLimitVisible.Value = "false";
                    pnlHighLimitVisible.Value = "false";
                    pnlExactLimitVisible.Value = "true";

                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(substance.expressedby_FK);
                    ctlExpressedAs.ControlValue = ssiTmp != null ? ssiTmp.ssi__cont_voc_PK : null;
                    if (ssiTmp != null && ssiTmp.term_name_english != "")
                    {
                        if (ssiTmp.term_name_english == "Units of Measure")
                            BindDDLMeasures();
                        else
                            BindDDLPresentation();
                    }

                    ctlExactNumUnit.ControlValue = substance.lowamountnumerunit == null ? String.Empty : substance.lowamountnumerunit.ToString();
                    ctlExactDenUnit.ControlValue = substance.lowamountdenomunit == null ? String.Empty : substance.lowamountdenomunit.ToString();
                    ctlExactNumPrefix.ControlValue = substance.lowamountnumerprefix == null ? String.Empty : substance.lowamountnumerprefix.ToString();
                    ctlExactDenPrefix.ControlValue = substance.lowamountdenomprefix == null ? String.Empty : substance.lowamountdenomprefix.ToString();

                    ctlExactNumVal.ControlValue = substance.lowamountnumervalue == null ? String.Empty : substance.lowamountnumervalue.ToString();
                    ctlExactDenVal.ControlValue = substance.lowamountdenomvalue == null ? String.Empty : substance.lowamountdenomvalue.ToString();

                    ExactDenUnit_Changed(null, null);
                    ExactNumUnit_Changed(null, null);
                    ExactDenPrefix_Changed(null, null);
                    ExactDenVal_Changed(null, null);
                    ExactNumPrefix_Changed(null, null);
                    ExactNumVal_Changed(null, null);
                }

                infoRangeMinText.Visible = true;
                infoRangeNText.Visible = true;
                infoRangeDText.Visible = true;
                infoUnitText.Visible = true;
                infoSubNameText.Visible = true;
                infoSubNameText2.Visible = true;
                infoConcTypeText1.Visible = true;
                infoConcTypeText2.Visible = true;
            }

            Substance_Changed();
        }

        #endregion

        #region Form methods

        public void btnOk_Click(object sender, EventArgs e)
        {
            Substance_Changed();
            if (ValidateForm(null))
            {
                SaveForm(null);
                PopupControls_Entity_Container.Style["display"] = "none";

                ctlUcPopupVisible.Value = "false";
                if (OnOkButtonClick != null)
                    OnOkButtonClick(sender, new FormEventArgs<PharmaceuticalProductSubstance>(_substance));
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

        #endregion

    }
}