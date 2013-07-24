using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using System.Drawing;


namespace AspNetUI.Support
{
    public partial class AutoCompleteBox_CT : System.Web.UI.UserControl, IControlCommon
    {
        private string _errorMessage = String.Empty;
        private string _emptyErrorMessage = String.Empty;
        private string _controlValueFormat = String.Empty;
        private ControlState _controlState = ControlState.ReadyForAction;
        private string _bindingPath = String.Empty;

        /// <summary>
        /// The web service method to be called. The signature of this method must match the following: 
        /// [System.Web.Services.WebMethod]
        /// [System.Web.Script.Services.ScriptMethod]
        /// public string[] GetCompletionList(string prefixText, int count, string contextKey);
        /// </summary>
        public string ServiceMethod
        {
            get { return ctlInput_AutoCompleteExtender.ServiceMethod; }
            set { ctlInput_AutoCompleteExtender.ServiceMethod = value; }
        }

        /// <summary>
        /// The path to the web service that the extender will pull the word\sentence completions from. 
        /// If this is not provided, the service method should be a page method.
        /// </summary>
        public string ServicePath
        {
            get { return ctlInput_AutoCompleteExtender.ServicePath; }
            set { ctlInput_AutoCompleteExtender.ServicePath = value; }
        }

        /// <summary>
        /// Minimum number of characters that must be entered before getting suggestions from the web service.
        /// </summary>
        public int MinimumPrefixLength
        {
            get { return ctlInput_AutoCompleteExtender.MinimumPrefixLength; }
            set { ctlInput_AutoCompleteExtender.MinimumPrefixLength = value; }
        }

        /// <summary>
        /// Number of suggestions to be retrieved from the web service.
        /// </summary>
        public int CompletionSetCount
        {
            get { return ctlInput_AutoCompleteExtender.CompletionSetCount; }
            set { ctlInput_AutoCompleteExtender.CompletionSetCount = value; }
        }

        /// <summary>
        /// Determines if the first option in the AutoComplete list will be selected by default.
        /// </summary>
        public bool FirstRowSelected
        {
            get { return ctlInput_AutoCompleteExtender.FirstRowSelected; }
            set { ctlInput_AutoCompleteExtender.FirstRowSelected = value; }
        }

        /// <summary>
        /// If true and DelimiterCharacters are specified, then the AutoComplete list items display suggestions 
        /// for the current word to be completed and do not display the rest of the tokens.
        /// </summary>
        public bool ShowOnlyCurrentWordInCompletionListItem
        {
            get { return ctlInput_AutoCompleteExtender.ShowOnlyCurrentWordInCompletionListItem; }
            set { ctlInput_AutoCompleteExtender.ShowOnlyCurrentWordInCompletionListItem = value; }
        }


        #region IControlStates

        public ControlState CurrentControlState
        {
            get { return _controlState; }
            set
            {
                _controlState = value;
                SetDefaultControlState();

                switch (_controlState)
                {
                    case ControlState.IAmInvalid:
                        ctlAlert.Visible = true;
                        ctlLabel.Style["color"] = "#ff0000";
                        ctlInput.Style["border"] = "1px solid #ff0000";
                        ctlInput.Style["padding"] = "2px";
                        ctlInput.ToolTip = ControlErrorMessage;
                        ctlAlert.ToolTip = ControlErrorMessage;
                        break;
                    case ControlState.YouCantChangeMe:
                        ctlInput.Enabled = false;
                        break;
                    case ControlState.YouCantSeeMe:
                        this.Visible = false;
                        break;
                    case ControlState.YouCanOnlyReadMe:
                        ctlInput.ReadOnly = true;
                        break;
                    case ControlState.ReadyForAction:
                    case ControlState.DontKnowActually:
                    default:
                        // Who cares?
                        break;
                }
            }
        }

        public bool ControlSupportsState(ControlState state)
        {
            if (state == ControlState.DontKnowActually
                || state == ControlState.IAmInvalid
                || state == ControlState.ReadyForAction
                || state == ControlState.YouCantChangeMe
                || state == ControlState.YouCanOnlyReadMe
                || state == ControlState.YouCantSeeMe)
                return true;
            else return false;
        }

        #endregion

        #region IControlDesign

        public string TotalControlWidth
        {
            get { return tableCanvas.Width; }
            set { tableCanvas.Width = value; }
        }

        public string LabelColumnWidth
        {
            get { return tdLabel.Width; }
            set { tdLabel.Width = value; }
        }

        public string ControlInputWidth
        {
            get { return ctlInput.Width.ToString(); }
            set { ctlInput.Width = Unit.Parse(value); }
        }

        public string ControlLabelAlign
        {
            get { return tdLabel.Style["text-align"]; }
            set { tdLabel.Style["text-align"] = value; }
        }

        public bool FontItalic
        {
            get { return ctlLabel.Font.Italic; }
            set { ctlLabel.Font.Italic = value; }
        }

        public bool FontBold
        {
            get { return ctlLabel.Font.Bold; }
            set { ctlLabel.Font.Bold = value; }
        }

        public bool FontValueBold
        {
            get { return ctlInput.Font.Bold; }
            set { ctlInput.Font.Bold = value; }
        }

        public bool FontUnderline
        {
            get { return ctlLabel.Font.Underline; }
            set { ctlLabel.Font.Underline = value; }
        }

        public Color FontColor
        {
            get { return ctlLabel.ForeColor; }
            set { ctlLabel.ForeColor = value; }
        }

        #endregion

        #region IControlBinder

        public string BindingPath
        {
            get { return _bindingPath; }
            set { _bindingPath = value; }
        }

        public string SerializeValue(object input)
        {
            string value = String.Empty;

            if (input != null)
            {
                if (String.IsNullOrEmpty(ControlValueFormat)) value = input.ToString();
                else
                {
                    value = String.Format(ControlValueFormat, input);
                }
            }

            return value;
        }

        public object DeserializeValue(string input)
        {
            return input;
        }

        #endregion

        #region IControlData

        public object ControlValue
        {
            get { return DeserializeValue(ctlInput.Text); }
            set { ctlInput.Text = SerializeValue(value); }
        }

        public string ControlValueFormat
        {
            get { return _controlValueFormat; }
            set { _controlValueFormat = value; }
        }

        public string ControlLabel
        {
            get { return ctlLabel.Text; }
            set
            {
                ctlLabel.Text = value;

                if (String.IsNullOrEmpty(value)) ctlLabel.Visible = false;
                else ctlLabel.Visible = true;
            }
        }

        public string ControlInputUnitsLabel
        {
            get { return ctlInputUnitsLabel.Text; }
            set
            {
                ctlInputUnitsLabel.Text = value;

                if (String.IsNullOrEmpty(value)) tdInputUnitsLabel.Visible = false;
                else tdInputUnitsLabel.Visible = true;
            }
        }

        public string ControlDescriptor
        {
            get { return ctlDescription.ToolTip; }
            set
            {
                ctlDescription.ToolTip = value;

                if (String.IsNullOrEmpty(value)) ctlDescription.Visible = false;
                else ctlDescription.Visible = true;
            }
        }

        public string ControlErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        public string ControlEmptyErrorMessage
        {
            get { return _emptyErrorMessage; }
            set { _emptyErrorMessage = value; }
        }

        public bool IsMandatory
        {
            get { return ctlMark.Visible; }
            set { ctlMark.Visible = value; }
        }

        public int MaxLength
        {
            get { return ctlInput.MaxLength; }
            set { ctlInput.MaxLength = value; }
        }

        public string ControlTextValue
        {
            get { return ControlValue.ToString(); }
        }
        #endregion

        #region IControlBehavior

        public bool AutoPostback
        {
            get { return ctlInput.AutoPostBack; }
            set { ctlInput.AutoPostBack = value; }
        }

        public event EventHandler<ValueChangedEventArgs> InputValueChanged;

        #endregion

        private void SetDefaultControlState()
        {
            this.Visible = true;
            ctlInput.Enabled = true;
            ctlInput.ReadOnly = false;
            ctlAlert.Visible = false;
            ctlLabel.Style["color"] = "";
            ctlInput.Style["border"] = "";
            ctlInput.Style["padding"] = "";

            ctlInput.ToolTip = String.Empty;
            ctlAlert.ToolTip = String.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ctlInput_TextChanged(object sender, EventArgs e)
        {
            if (InputValueChanged != null) InputValueChanged(this, new ValueChangedEventArgs(ctlInput.Text));
        }
    }
}