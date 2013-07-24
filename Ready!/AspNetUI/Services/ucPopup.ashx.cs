using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AspNetUIFramework;
using Ready.Model;
using System.Text;
using GEM2Common;
using System.Data;
using System.Threading;

namespace AspNetUI.Services
{
    /// <summary>
    /// Class handles async requesst for ucPopupForm.
    /// Response is serialized JSON object.
    /// </summary>
    public class ucPopup1 : IHttpHandler
    {

        ISubstance_PKOperations _substance_PKOperations;
        ISsi__cont_voc_PKOperations _ssi_cont_voc_PKOperations;

        string returnstring;
        Dictionary<String, String> asyncReturnValues;

        /// <summary>
        /// Method accept async request, recognize action from "action" param, and 
        /// forwards handlig to appropriate method.
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            asyncReturnValues = new Dictionary<string, string>();
            UpdateResponse("action", "none");

            if (!context.Request.Params.AllKeys.Contains("action"))
            {
                returnstring = "";
                return;
            }
            try
            {
                switch (context.Request.Params["action"])
                {
                    //Exact values handlers
                    case "ExactNumeratorValueChanged":
                        ExactNumVal_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["concentrationTypeCode"]);
                        break;
                    case "ExactDenominatorValueChanged":
                        ExactDenVal_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["expressedAs"], context.Request.Params["concentrationTypeCode"]);
                        break;
                    case "ExactNumPrefixChanged":
                        ExactNumPrefix_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["concentrationTypeCode"]);
                        break;
                    case "ExactDenPrefixChanged":
                        ExactDenPrefix_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["concentrationTypeCode"]);
                        break;
                    case "ExactNumUnitChanged":
                        ExactNumUnit_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["concentrationTypeCode"]);
                        break;
                    case "ExactDenUnitChanged":
                        ExactDenUnit_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["concentrationTypeCode"], context.Request.Params["expressedAs"]);
                        break;

                    //Low limit handlers
                    case "LowLimitNumValueChanged":
                        LowLimitNumVal_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["concentrationTypeCode"]);
                        break;
                    case "LowLimitDenValueChanged":
                        LowLimitDenVal_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["expressedAs"], context.Request.Params["concentrationTypeCode"]);
                        break;
                    case "LowLimitNumPrefixChanged":

                        LowLimitNumPrefix_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["highLimitPrefix"], context.Request.Params["concentrationTypeCode"]);
                        break;
                    case "LowLimitDenPrefixChanged":

                        LowLimitDenPrefix_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["concentrationTypeCode"]);
                        break;
                    case "LowLimitNumUnitChanged":
                        LowLimitNumUnit_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["concentrationTypeCode"]);
                        break;
                    case "LowLimitDenUnitChanged":
                        LowLimitDenUnit_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["controlTextValue"], context.Request.Params["expressedAs"], context.Request.Params["concentrationTypeCode"]);
                        break;

                    //High limit handlers
                    case "HighLimitNumValueChanged":
                        HighLimitNumVal_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["concentrationTypeCode"]);
                        break;
                    case "HighLimitDenValueChanged":
                        HighLimitDenVal_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["expressedAs"], context.Request.Params["concentrationTypeCode"]);
                        break;
                    case "HighLimitNumPrefixChanged":
                        HighLimitNumPrefix_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["lowLimitPrefix"], context.Request.Params["concentrationTypeCode"]);
                        break;
                    case "HighLimitDenPrefixChanged":
                        HighLimitDenPrefix_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["concentrationTypeCode"]);
                        break;
                    case "HighLimitNumUnitChanged":
                        HighLimitNumUnit_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["concentrationTypeCode"]);
                        break;
                    case "HighLimitDenUnitChanged":
                        HighLimitDenUnit_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["controlTextValue"], context.Request.Params["expressedAs"], context.Request.Params["concentrationTypeCode"]);
                        break;


                    case "SubstanceChanged":
                        Substance_ChangedJS(context.Request.Params["substanceId"]);
                        break;
                    case "ConcentrationTypeChanged":
                        ConcentrationType_ChangedJS(context.Request.Params["controlValue"]);
                        break;
                    case "ExpressedAsChanged":
                        ExpressedAs_ChangedJS(context.Request.Params["controlValue"], context.Request.Params["controlTextValue"]);
                        break;
                    case "RefreshSubstanceList":
                        RefreshSubstanceListJS(context.Request.Params["name"], context.Request.Params["evcode"], context.Request.Params["page"]);
                        break;
                }
            }
            catch (Exception e) {
                UpdateResponse("action", "none");
            }

            returnstring = SerializeResponse(asyncReturnValues);
            context.Response.ContentType = "text/plain";
            context.Response.Write(returnstring);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    
        /// <summary>
        /// Methods serializes return values to text represented JSON object
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        String SerializeResponse(Dictionary<String, String> args)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            bool first = true;
            foreach (KeyValuePair<String, String> kvp in args)
            {
                if (!first) builder.Append(", ");
                first = false;
                String value = kvp.Value;
                if (!kvp.Value.Trim().StartsWith("#array#["))
                {
                    value = "\"" + JSONEscape(kvp.Value.Trim()) + "\"";
                }
                else
                {
                    value = value.Replace("#array#", "");
                }
                builder.Append("\"" + kvp.Key + "\":" + value);
            }
            builder.Append("}");
            return builder.ToString();

        }

        private void UpdateResponse(String key, String value)
        {
            if (!this.asyncReturnValues.ContainsKey(key))
            {
                asyncReturnValues.Add(key, value);
            }
            else
            {
                asyncReturnValues[key] = value;
            }
        }


        #region Exact handlers

        public void ExactNumVal_ChangedJS(String controlValue, String concentrationTypeCode)
        {
            if (concentrationTypeCode != "Range")
            {
                if (controlValue.Contains(".."))
                {
                    UpdateResponse("action", "clear");
                    return;
                }
                if (!ValidationHelper.IsValidDecimal(controlValue))
                {
                    UpdateResponse("action", "clear");
                    return;
                }

                string ctlExactNumValTmp = controlValue;
                if (ctlExactNumValTmp.Contains("."))
                {
                    ctlExactNumValTmp = ctlExactNumValTmp.Replace(".", ",");
                    controlValue = ctlExactNumValTmp;
                }
           
                UpdateResponse("action", "update");
                UpdateResponse("newValue", Convert.ToDecimal(controlValue).ToString("#0.#####"));
            }
            else
            {
                returnstring = "NoAction";
            }
        }

        public void ExactNumUnit_ChangedJS(String controlValue, String concentrationTypeCode)
        {
            UpdateResponse("action", "none");
            if (concentrationTypeCode != "Range")
            {
                if (controlValue != "" && controlValue != "undefined")
                {
                    _ssi_cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();
                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(controlValue));
                    UpdateResponse("action", "update");
                    UpdateResponse("infoConNUnit", ssiTmp != null ? ssiTmp.Field8 != "" ? ssiTmp.Field8 : "" : "");
                    UpdateResponse("infoUnit", "controlValue");
                }
                else
                {
                    UpdateResponse("action", "clear");
                }
            }
        }

        public void ExactDenVal_ChangedJS(String controlValue, String expressedAs, String concentrationTypeCode)
        {
            string ctlExactDenValTmp = controlValue;
            if (controlValue.Contains(".."))
            {
                UpdateResponse("action", "clear"); ;
                return;
            }
            if (!ValidationHelper.IsValidDecimal(controlValue))
            {
                UpdateResponse("action", "clear");
                return;
            }

            if (ctlExactDenValTmp.Contains("."))
            {
                ctlExactDenValTmp = ctlExactDenValTmp.Replace(".", ",");
                controlValue = ctlExactDenValTmp;
            }
      
            UpdateResponse("action", "update");
            UpdateResponse("newValue", Convert.ToDecimal(controlValue).ToString("#0.#####"));

            if (expressedAs.ToLower().Contains("presentation"))
            {
                UpdateResponse("action", "update");
                UpdateResponse("newValue", ((int)Convert.ToDecimal(controlValue)).ToString());
            }
        }

        public void ExactDenUnit_ChangedJS(String controlValue, String concentrationTypeCode, String expressedAs)
        {
            if (concentrationTypeCode != "Range")
            {
                if (ValidationHelper.IsValidInt(controlValue))
                {
                    UpdateResponse("action", "update");
                    _ssi_cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();
                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(controlValue));
                    if (expressedAs == "Units of Measure")
                    {
                        UpdateResponse("infoConDenExp", ssiTmp != null ? ssiTmp.Field8 != "" ? "" + ssiTmp.Field8 : "" : "");
                    }
                    else
                    {
                        UpdateResponse("infoConDenExp", ssiTmp != null ? ssiTmp.Description != "" ? " " + ssiTmp.Description : "" : "");
                    }
                }
                else
                {
                    UpdateResponse("action", "clear");
                }
            }
        }

        public void ExactNumPrefix_ChangedJS(String controlValue, String concentrationTypeCode)
        {
            if (concentrationTypeCode != "Range")
            {
                if (controlValue != "" && controlValue != "undefined")
                {
                    _ssi_cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();
                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(controlValue));
                    int indexOfFirtOccur = ssiTmp.Description.IndexOf('(') != -1 ? ssiTmp.Description.IndexOf('(') : ssiTmp.Description.Length;
                    int end = ssiTmp.Description.Length - indexOfFirtOccur;

                    if (ssiTmp != null && !String.IsNullOrWhiteSpace(ssiTmp.Description) && !ssiTmp.Description.ToLower().Contains("single"))
                    {
                        UpdateResponse("action", "update");
                        UpdateResponse("infoNumMaxPrefix", ssiTmp.Description.Remove(indexOfFirtOccur, end));
                        UpdateResponse("infoConNumMaxPrefix", (!String.IsNullOrWhiteSpace(ssiTmp.Field8) ? " " + ssiTmp.Field8 : ""));

                    }
                    else if (ssiTmp != null && !String.IsNullOrWhiteSpace(ssiTmp.Description) && ssiTmp.Description.ToLower().Contains("single"))
                    {
                        UpdateResponse("action", "update");
                        UpdateResponse("infoNumMaxPrefix", "");
                        UpdateResponse("infoConNumMaxPrefix", "");

                    }
                    UpdateResponse("ctlValue", controlValue);
                }
                else
                {
                    UpdateResponse("action", "clear");
                }
            }
            else
            {
                UpdateResponse("action", "none");
            }
        }

        public void ExactDenPrefix_ChangedJS(String controlValue, String concentrationTypeCode)
        {
            UpdateResponse("action", "none");
            if (concentrationTypeCode != "Range")
            {
                if (ValidationHelper.IsValidInt(controlValue))
                {
                    _ssi_cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();
                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(controlValue));
                    int indexOfFirtOccur = ssiTmp != null && ssiTmp.Description.IndexOf('(') != -1 ? ssiTmp.Description.IndexOf('(') : ssiTmp.Description.Length;
                    int end = ssiTmp.Description.Length - indexOfFirtOccur;

                    if (ssiTmp != null && !String.IsNullOrWhiteSpace(ssiTmp.Description) && !ssiTmp.Description.ToLower().Contains("single"))
                    {
                        UpdateResponse("action", "update");
                        UpdateResponse("infoDenPrefix", ssiTmp.Description.Remove(indexOfFirtOccur, end));
                        UpdateResponse("infoConDenPrefix", !String.IsNullOrWhiteSpace(ssiTmp.Field8) ? " " + ssiTmp.Field8 : "");

                    }
                    else if (ssiTmp != null && !String.IsNullOrWhiteSpace(ssiTmp.Description) && ssiTmp.Description.ToLower().Contains("single"))
                    {
                        UpdateResponse("action", "update");
                        UpdateResponse("infoDenPrefix", "");
                        UpdateResponse("infoConDenPrefix", "");
                    }
                    else
                    {
                        UpdateResponse("action", "clear");
                    }

                    UpdateResponse("processEnd", "true");
                   
                }
                else
                {
                    UpdateResponse("action", "clear");
                }
            }
        }
        #endregion

        #region Low limit handlers
        public void LowLimitNumVal_ChangedJS(String controlValue, String concentrationTypeCode)
        {
            if (controlValue.Contains(".."))
            {
                UpdateResponse("action", "clear");
                return;
            }
            if (!ValidationHelper.IsValidDecimal(controlValue))
            {
                UpdateResponse("action", "clear");
                return;
            }

            string ctlLowLimitNumValTmp = controlValue;
            if (ctlLowLimitNumValTmp.Contains("."))
            {
                ctlLowLimitNumValTmp = ctlLowLimitNumValTmp.Replace(".", ",");
                controlValue = ctlLowLimitNumValTmp;
            }
            UpdateResponse("action", "update");
            UpdateResponse("newValue", Convert.ToDecimal(controlValue).ToString("#0.#####"));
        }

        public void LowLimitNumPrefix_ChangedJS(String controlValue, String ctlHighLimitPrefix, String concentrationTypeCode)
        {
            if (concentrationTypeCode.ToLower().Contains("range"))
            {
                string prefixNumMin = "";
                string prefixConNumMin = "";
                string prefixNumMax = "";
                string prefixConNumMax = "";

                Ssi__cont_voc_PK prefix = null;
                _ssi_cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();
                if (ValidationHelper.IsValidInt(controlValue))
                    prefix = _ssi_cont_voc_PKOperations.GetEntity(int.Parse(controlValue));

                if (prefix != null && !prefix.Description.ToLower().Contains("single"))
                {
                    prefixNumMin = prefix.Description.Contains("(") ? " " + prefix.Description.Remove(prefix.Description.IndexOf("(")).Trim() : " " + prefix.Description.Trim();
                    prefixConNumMin = !String.IsNullOrWhiteSpace(prefix.Field8) ? " " + prefix.Field8.Trim() : "";
                }

                prefix = null;
                if (ValidationHelper.IsValidInt(ctlHighLimitPrefix))
                    prefix = _ssi_cont_voc_PKOperations.GetEntity(int.Parse(ctlHighLimitPrefix));

                if (prefix != null && !prefix.Description.ToLower().Contains("single"))
                {
                    prefixNumMax = prefix.Description.Contains("(") ? " " + prefix.Description.Remove(prefix.Description.IndexOf("(")).Trim() : " " + prefix.Description.Trim();
                    prefixConNumMax = !String.IsNullOrWhiteSpace(prefix.Field8) ? " " + prefix.Field8.Trim() : "";
                }

                UpdateResponse("action", "update");
                UpdateResponse("prefixNumMin", prefixNumMin);
                UpdateResponse("prefixConNumMin", prefixConNumMin);
                UpdateResponse("prefixNumMax", prefixNumMax);
                UpdateResponse("prefixConNumMax", prefixConNumMax);
            }
        }

        public void LowLimitNumUnit_ChangedJS(String controlValue, String concentrationTypeCode)
        {
            if (concentrationTypeCode.ToLower().Contains("range"))
            {
                if (controlValue != "" && controlValue != "undefined")
                {
                    UpdateResponse("action", "update");
                    _ssi_cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();
                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(controlValue));
                    UpdateResponse("infoConNUnit", ssiTmp != null ? ssiTmp.Field8 != "" ? ssiTmp.Field8.Trim() : "" : "");
                }
                else
                {
                    UpdateResponse("action", "clear");
                }
            }
        }

        public void LowLimitDenVal_ChangedJS(String controlValue, String expressedAs, String concentrationTypeCode)
        {

            if (controlValue.Contains(".."))
            {
                UpdateResponse("action", "clear");
                return;
            }
            if (!ValidationHelper.IsValidDecimal(controlValue))
            {
                UpdateResponse("action", "clear");
                return;
            }
            UpdateResponse("action", "update");
            string ctlLowLimitDenValTmp = controlValue;
            if (ctlLowLimitDenValTmp.Contains("."))
            {
                ctlLowLimitDenValTmp = ctlLowLimitDenValTmp.Replace(".", ",");
                controlValue = ctlLowLimitDenValTmp;
            }

            controlValue = Convert.ToDecimal(controlValue).ToString("#0.#####");
            if (expressedAs.ToLower().Contains("presentation")) controlValue = ((int)Convert.ToDecimal(controlValue)).ToString();
            UpdateResponse("newValue", controlValue);
        }

        public void LowLimitDenPrefix_ChangedJS(String controlValue, String concentrationTypeCode)
        {

            if (controlValue != "" && controlValue != "undefined")
            {
                _ssi_cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();
                Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(controlValue));
                int indexOfFirtOccur = ssiTmp.Description.IndexOf('(') != -1 ? ssiTmp.Description.IndexOf('(') : ssiTmp.Description.Length;
                int end = ssiTmp.Description.Length - indexOfFirtOccur;
                UpdateResponse("action", "update");
                if (ssiTmp != null && !String.IsNullOrWhiteSpace(ssiTmp.Description) && !ssiTmp.Description.ToLower().Contains("single"))
                {
                    UpdateResponse("infoDenPrefix", " " + ssiTmp.Description.Remove(indexOfFirtOccur, end));
                    UpdateResponse("infoConDenPrefix", !String.IsNullOrWhiteSpace(ssiTmp.Field8) ? " " + ssiTmp.Field8 : "");
                }
            }
        }

        public void LowLimitDenUnit_ChangedJS(String controlValue, String controlTextValue, String expressedAs, String concentrationTypeCode)
        {
            if (concentrationTypeCode == "Range")
            {
                if (ValidationHelper.IsValidInt(controlValue))
                {
                    UpdateResponse("action", "update");
                    _ssi_cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();
                    Ssi__cont_voc_PK ssiTmp = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(controlValue));
                    if (expressedAs.ToLower().Trim() == "units of measure")
                    {
                        UpdateResponse("infoDenExp", controlTextValue);
                        UpdateResponse("infoConDenExp", ssiTmp != null ? ssiTmp.Field8 != "" ? "" + ssiTmp.Field8 : "" : "");
                    }
                    else
                    {
                        UpdateResponse("infoDenExp", controlTextValue);
                        UpdateResponse("infoConDenExp", ssiTmp != null ? ssiTmp.Description != "" ? " " + ssiTmp.Description : "" : "");
                    }

                    if (controlTextValue.ToLower().Contains("each"))
                    {
                        UpdateResponse("infoDenExp", "");
                        UpdateResponse("infoConDenExp", "");
                    }
                }
                else
                {
                    UpdateResponse("action", "clear");
                }
            }
        }

        #endregion

        #region High limit handlers
        public void HighLimitNumVal_ChangedJS(String controlValue, String concentrationTypeCode)
        {
            if (controlValue.Contains(".."))
            {
                UpdateResponse("action", "clear");
                return;
            }
            if (!ValidationHelper.IsValidDecimal(controlValue))
            {
                UpdateResponse("action", "clear");
                return;
            }

            UpdateResponse("action", "update");
            string ctlHighLimitNumValTmp = controlValue;
            if (ctlHighLimitNumValTmp.Contains("."))
            {
                ctlHighLimitNumValTmp = ctlHighLimitNumValTmp.Replace(".", ",");
                controlValue = ctlHighLimitNumValTmp;
            }

            controlValue = Convert.ToDecimal(controlValue).ToString("#0.#####");
            UpdateResponse("newValue", controlValue);

        }

        public void HighLimitNumPrefix_ChangedJS(String controlValue, String lowLimitPrefix, String concentrationTypeCode)
        {
            if (concentrationTypeCode.ToLower().Contains("range"))
            {
                string prefixNumMin = "";
                string prefixConNumMin = "";
                string prefixNumMax = "";
                string prefixConNumMax = " ";

                Ssi__cont_voc_PK prefix = null;
                _ssi_cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();
                if (ValidationHelper.IsValidInt(lowLimitPrefix.ToString()))
                    prefix = _ssi_cont_voc_PKOperations.GetEntity(int.Parse(lowLimitPrefix.ToString()));

                if (prefix != null && !prefix.Description.ToLower().Contains("single"))
                {
                    prefixNumMin = prefix.Description.Contains("(") ? " " + prefix.Description.Remove(prefix.Description.IndexOf("(")).Trim() : " " + prefix.Description.Trim();
                    prefixConNumMin = !String.IsNullOrWhiteSpace(prefix.Field8) ? " " + prefix.Field8.Trim() : "";
                }

                prefix = null;
                if (ValidationHelper.IsValidInt(controlValue.ToString()))
                    prefix = _ssi_cont_voc_PKOperations.GetEntity(int.Parse(controlValue.ToString()));

                if (prefix != null && !prefix.Description.ToLower().Contains("single"))
                {
                    prefixNumMax = prefix.Description.Contains("(") ? " " + prefix.Description.Remove(prefix.Description.IndexOf("(")).Trim() : " " + prefix.Description.Trim();
                     prefixConNumMax = !String.IsNullOrWhiteSpace(prefix.Field8) ? " " + prefix.Field8.Trim() : " ";
                }

                UpdateResponse("action", "update");
                UpdateResponse("prefixNumMin", prefixNumMin);
                UpdateResponse("prefixConNumMin", prefixConNumMin);
                UpdateResponse("prefixNumMax", prefixNumMax);
                UpdateResponse("prefixConNumMax", prefixConNumMax);
            }
        }

        public void HighLimitNumUnit_ChangedJS(String controlValue, String concentrationTypeCode)
        {
        }

        public void HighLimitDenVal_ChangedJS(String controlValue, String expressedAs, String concentrationTypeCode)
        {
            string ctlHighLimitDenValTmp = controlValue;
            if (controlValue.Contains(".."))
            {
                UpdateResponse("action", "clear");
                return;
            }
            if (!ValidationHelper.IsValidDecimal(controlValue))
            {
                UpdateResponse("action", "clear");
                return;
            }

            if (controlValue.Contains("."))
            {
                ctlHighLimitDenValTmp = ctlHighLimitDenValTmp.Replace(".", ",");
                controlValue = ctlHighLimitDenValTmp;
            }

            controlValue = Convert.ToDecimal(controlValue).ToString("#0.#####");
            if (expressedAs.ToLower().Contains("presentation")) controlValue = ((int)Convert.ToDecimal(controlValue)).ToString();
            UpdateResponse("newValue", controlValue);
        }

        public void HighLimitDenPrefix_ChangedJS(String controlValue, String concentrationTypeCode)
        {
        }

        public void HighLimitDenUnit_ChangedJS(String controlValue, String controlTextValue, String expressedAs, String concentrationTypeCode)
        {
        }

        #endregion

        void Substance_ChangedJS(String substancePK)
        {
            if (!ValidationHelper.IsValidInt(substancePK))
            {
                UpdateResponse("action", "clear");
                return;
            }

            _substance_PKOperations = new Substance_PKDAL();
            Substance_PK sub = _substance_PKOperations.GetEntity(substancePK);

            if (sub != null && sub.substance_name != null)
            {
                UpdateResponse("action", "update");
                UpdateResponse("substanceName", sub.substance_name);
            }
            else
            {
                UpdateResponse("action", "clear");
            }
        }

        public void ConcentrationType_ChangedJS(String controlValue)
        {
            string concType = "";

            if (ValidationHelper.IsValidInt(controlValue))
            {
                _ssi_cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();
                Ssi__cont_voc_PK ssi = _ssi_cont_voc_PKOperations.GetEntity(controlValue);
                concType = ssi != null ? ssi.term_name_english : "";
                UpdateResponse("newState", concType);
            }
            else
            {
                UpdateResponse("newState", "none");
            }
        }

        public void ExpressedAs_ChangedJS(String controlValue, String controlTextValue)
        {
            if (String.IsNullOrWhiteSpace(controlValue))
            {
                UpdateResponse("action", "clear");
            }
            else
            {
                UpdateResponse("action", "update");
                if (controlTextValue.ToLower().Trim() == "units of measure")
                {
                    _ssi_cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();
                    UpdateResponse("label", "of Measure");
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

                    String jsonArray = "#array#[";
                    foreach (Ssi__cont_voc_PK item in items)
                    {
                        jsonArray += ("{\"value\":\"" + item.ssi__cont_voc_PK + "\",\"text\":\"" + JSONEscape(item.Description) + "\"},");
                    }
                    jsonArray = jsonArray.Substring(0, jsonArray.Length - 1) + "]";
                    UpdateResponse("options", jsonArray);
                }
                else
                {
                    UpdateResponse("label", "of Presentation");
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

                    String jsonArray = "#array#[";
                    foreach (Ssi__cont_voc_PK item in items)
                    {
                        jsonArray += ("{\"value\":\"" + item.ssi__cont_voc_PK + "\",\"text\":\"" + JSONEscape(item.Description) + "\"},");
                    }
                    jsonArray = jsonArray.Substring(0, jsonArray.Length - 1) + "]";
                    UpdateResponse("options", jsonArray);
                }
            }
        }

        public void RefreshSubstanceListJS(String name, String evcode, String page)
        {

            _substance_PKOperations = new Substance_PKDAL();

            int currentPage = ValidationHelper.IsValidInt(page) ? Convert.ToInt32(page) : 1;
            int recordsPerPage = 50;
            int tempCount = 0;

            List<GEMOrderBy> orderBy = new List<GEMOrderBy>();
            orderBy.Add(new GEMOrderBy("Name", GEMOrderByType.ASC));
            DataSet entities = _substance_PKOperations.GetSubstancesByNameSearcher(name.Replace("'", "''"), evcode.Replace("'", "''"), currentPage, recordsPerPage, orderBy, out tempCount);

            String jsonArray = "#array#[";
            foreach (DataRow row in entities.Tables[0].Rows)
            {
                jsonArray += ("{\"value\":\"" + row["ID"] + "\",\"text\":\"" + JSONEscape(Convert.ToString(row["name"]).Replace("\r\n", "").Replace("\r", "").Replace("\n", "")) + " (" + JSONEscape(row["EVCODE"].ToString()) + ")" + "\"},");
            }
            if (jsonArray.EndsWith(","))
            {
                jsonArray = jsonArray.Substring(0, jsonArray.Length - 1);
            }
            jsonArray += "]";
            UpdateResponse("substances", jsonArray);
            UpdateResponse("page", currentPage.ToString());
            UpdateResponse("name", name);
            UpdateResponse("evcode", evcode);
            UpdateResponse("totalPages", (tempCount / recordsPerPage + (tempCount % recordsPerPage != 0 ? 1 : 0)).ToString());
        }

        private String JSONEscape(String str)
        {
            return str.Replace("\"", "\\\"");
        }

    }
}