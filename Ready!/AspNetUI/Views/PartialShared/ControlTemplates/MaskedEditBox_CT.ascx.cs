using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using AjaxControlToolkit;
using System.Drawing;

namespace AspNetUI.Support
{
    public partial class MaskedEditBox_CT : System.Web.UI.UserControl, IControlCommon
    {
        private string _errorMessage = String.Empty;
        private string _emptyErrorMessage = String.Empty;
        private string _controlValueFormat = String.Empty;
        private ControlState _controlState = ControlState.ReadyForAction;
        private string _bindingPath = String.Empty;

        /// <summary>
        /// 9 - Only a numeric character
        /// L - Only a letter
        /// $ - Only a letter or a space
        /// C - Only a custom character (case sensitive)
        /// A - Only a letter or a custom character
        /// N - Only a numeric or custom character
        /// ? - Any character
        /// / - Date separator
        /// : - Time separator
        /// . - Decimal separator
        /// , - Thousand separator
        /// \ - Escape character
        /// { - Initial delimiter for repetition of masks
        /// } - Final delimiter for repetition of masks
        /// Examples:
        /// 9999999 - Seven numeric characters
        /// 99\/99 - Four numeric characters separated in the middle by a "/"
        /// </summary>
        public string ControlMask
        {
            get { return ctlInput_MaskedEditExtender.Mask; }
            set { ctlInput_MaskedEditExtender.Mask = value; }
        }

        /// <summary>
        /// None - No validation
        /// Number - Number validation
        /// Date - Date validation
        /// Time - Time validation
        /// DateTime - Date and time validation
        /// </summary>
        public MaskedEditType ControlMaskType
        {
            get { return ctlInput_MaskedEditExtender.MaskType; }
            set { ctlInput_MaskedEditExtender.MaskType = value; }
        }

        /// <summary>
        /// AM/PM time
        /// </summary>
        public bool ControlAcceptAMPM
        {
            get { return ctlInput_MaskedEditExtender.AcceptAMPM; }
            set { ctlInput_MaskedEditExtender.AcceptAMPM = value; }
        }

        /// <summary>
        /// Negative number supported
        /// </summary>
        public bool ControlAcceptsNegative
        {
            get { return ctlInput_MaskedEditExtender.AcceptNegative != MaskedEditShowSymbol.None ? true : false; }
            set { ctlInput_MaskedEditExtender.AcceptNegative = value == true ? MaskedEditShowSymbol.Left : MaskedEditShowSymbol.None; }
        }

        /// <summary>
        /// Supports autocomplete
        /// </summary>
        public bool ControlAutoComplete
        {
            get { return ctlInput_MaskedEditExtender.AutoComplete; }
            set { ctlInput_MaskedEditExtender.AutoComplete = value; }
        }

        /// <summary>
        /// Default character used for autocomplete value
        /// </summary>
        public string AutoCompleteValue
        {
            get { return ctlInput_MaskedEditExtender.AutoCompleteValue; }
            set { ctlInput_MaskedEditExtender.AutoCompleteValue = value; }
        }

        /// <summary>
        /// Default century used when a date mask only has two digits for the year
        /// </summary>
        public int ControlCentury
        {
            get { return ctlInput_MaskedEditExtender.Century; }
            set { ctlInput_MaskedEditExtender.Century = value; }
        }

        public bool ControlClearMaskOnLostFocus
        {
            get { return ctlInput_MaskedEditExtender.ClearMaskOnLostFocus; }
            set { ctlInput_MaskedEditExtender.ClearMaskOnLostFocus = value; }
        }

        public bool ControlClearTextOnInvalid
        {
            get { return ctlInput_MaskedEditExtender.ClearTextOnInvalid; }
            set { ctlInput_MaskedEditExtender.ClearTextOnInvalid = value; }
        }

        /// <summary>
        /// Specifies how the currency symbol is displayed
        /// None - Do not show the currency symbol
        /// Left - Show the currency symbol on the left of the mask
        /// Right - Show the currency symbol on the right of the mask
        /// </summary>
        public MaskedEditShowSymbol ControlDisplayMoney
        {
            get { return ctlInput_MaskedEditExtender.DisplayMoney; }
            set { ctlInput_MaskedEditExtender.DisplayMoney = value; }
        }

        /// <summary>
        /// Valid characters for mask type "C" (case-sensitive)
        /// </summary>
        public string ControlFilter
        {
            get { return ctlInput_MaskedEditExtender.Filtered; }
            set { ctlInput_MaskedEditExtender.Filtered = value; }
        }

        /// <summary>
        /// Prompt character for unspecified mask characters
        /// </summary>
        public string ControlPromptChararacter
        {
            get { return ctlInput_MaskedEditExtender.PromptCharacter; }
            set { ctlInput_MaskedEditExtender.PromptCharacter = value; }
        }

        /// <summary>
        /// Custom date format
        /// </summary>
        public MaskedEditUserDateFormat ControlUserDateFormat
        {
            get { return ctlInput_MaskedEditExtender.UserDateFormat; }
            set { ctlInput_MaskedEditExtender.UserDateFormat = value; }
        }

        /// <summary>
        /// Custom time format
        /// </summary>
        public MaskedEditUserTimeFormat ControlUserTimeFormat
        {
            get { return ctlInput_MaskedEditExtender.UserTimeFormat; }
            set { ctlInput_MaskedEditExtender.UserTimeFormat = value; }
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