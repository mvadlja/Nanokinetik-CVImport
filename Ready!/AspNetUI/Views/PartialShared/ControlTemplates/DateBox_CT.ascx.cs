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
    public partial class DateBox_CT : System.Web.UI.UserControl, IControlCommon, IControlSpecialDisable
    {
        public event EventHandler<EventArgs> ReminderOnClick;

        private string _errorMessage = String.Empty;
        private string _emptyErrorMessage = String.Empty;
        private ControlState _controlState = ControlState.ReadyForAction;
        private string _bindingPath = String.Empty;
        private bool _showReminder;
        private bool _alarmExists;


        #region Custom Properites
        public bool ShowReminder {
            get { return this._showReminder; }
            set { 
                this._showReminder = value;
                lnkSetReminder.Visible = this._showReminder;
                reminderHolder.Visible = this._showReminder;
            }
        }

        public bool AlarmExists
        {
            get { return (bool)ViewState["alarmExists"]; }
            set
            {
                ViewState["alarmExists"] = value;

                if (value)
                {
                    imgSetReminder.ImageUrl = "~/Images/alarm_exists.png";
                }
                else
                {
                    imgSetReminder.ImageUrl = "~/Images/alarm.png";
                }
            }
        }
        #endregion

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
                        //ctlAlert.Visible = true;
                        ctlLabel.Style["color"] = "#ff0000";
                        ctlInput.Style["border"] = "1px solid #ff0000";
                        ctlInput.Style["padding"] = "2px";
                        ctlInput.ToolTip = ControlErrorMessage;
                        ctlAlert.ToolTip = ControlErrorMessage;
                        break;
                    case ControlState.YouCantChangeMe:
                        ctlInput.Enabled = false;
                        ctlInputImg.Visible = false;
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

                if (_showReminder)
                {
                    lnkSetReminder.Visible = _showReminder;
                    reminderHolder.Visible = _showReminder;
                }

                if (AlarmExists)
                {
                    imgSetReminder.ImageUrl = "~/Images/alarm_exists.png";
                }
                else
                {
                    imgSetReminder.ImageUrl = "~/Images/alarm.png";
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
                    value = String.Format("{0:" + ControlValueFormat + "}", input);
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
            get { return ctlInput_CalendarExtender.Format; }
            set
            {
                ctlInput_CalendarExtender.Format = value;
            }
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
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ControlDescriptor
        {
            get { return ctlDescription.ToolTip; }
            set
            {
                ctlDescription.ToolTip = value;

                if (String.IsNullOrEmpty(value)) { ctlDescription.Visible = false; descriptionHolder.Visible = false; }
                else { ctlDescription.Visible = true; descriptionHolder.Visible = true; }
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
            //ctlAlert.Visible = false;
            alertHolder.Visible = false;
            descriptionHolder.Visible = false;
            reminderHolder.Visible = false;
            ctlLabel.Style["color"] = "";
            ctlInput.Style["border"] = "";
            ctlInput.Style["padding"] = "";
            ctlInputImg.Visible = true;

            ctlInput.ToolTip = String.Empty;
            ctlAlert.ToolTip = String.Empty;
        }
        protected override void OnInit(EventArgs e)
        {
            if (ViewState["alarmExists"] == null) ViewState["alarmExists"] = false;
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ctlInput_TextChanged(object sender, EventArgs e)
        {
            if (InputValueChanged != null) InputValueChanged(this, new ValueChangedEventArgs(ctlInput.Text));
        }

        public void lnkSetReminderClick(object sender, EventArgs e) {
            if (ReminderOnClick != null) {
                ReminderOnClick(sender, e);
            }
        }

        public void DisableControl() {
            CurrentControlState = ControlState.YouCantChangeMe;
        }

        public void EnableControl() {
            CurrentControlState = ControlState.ReadyForAction;
        }
    }
}