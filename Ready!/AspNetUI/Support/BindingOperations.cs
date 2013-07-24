using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Drawing;
using AspNetUIFramework;
using System.Web.UI;

namespace AspNetUI.Support
{
    public enum ColoringType{
        BLACK,
        BLACK_GREEN,
        RED_GREEN
    }

    public class BindingOperations
    {

        #region Public methods
        public static void AssignValueProperties(object control, object value) {
            if (control is ITextControl) {
                assignValueTextControl((ITextControl)control, value);
            }
            if (control is IControlData) {
                assignValueCustomControl((IControlData)control, value);
            }
        }

        public static void AssignColorByValue(object control, object value, ColoringType coloringType){
            if (control is WebControl) {
                assignColorWebControl((WebControl)control, value, coloringType);
            }
            if (control is IControlDesign) {
                assignColorCustomControl((IControlDesign)control, value, coloringType);
            }
        }

        public static void AssignColor(object control, bool activeColor, ColoringType coloringType) {
            if (control is WebControl) {
                if ( activeColor )
                    assignColorWebControl((WebControl)control, "active", coloringType);
                else
                    assignColorWebControl((WebControl)control, "", coloringType);
            }
            if (control is IControlDesign) {
                if ( activeColor )
                    assignColorCustomControl((IControlDesign)control, "active", coloringType);
                else
                    assignColorCustomControl((IControlDesign)control, "", coloringType);
            }

        }
        #endregion

        #region Private methods

        private static void assignValueTextControl(ITextControl control, object value) {
            if (value == null || value.ToString().Trim() == "")
            {
                control.Text = "-";
            }
            else {
                control.Text = value.ToString().Trim();
            }
        }
        private static void assignValueCustomControl(IControlData control, object value) {
            if (value == null || value.ToString().Trim() == "")
            {
                control.ControlValue = "-";
            }
            else {
                control.ControlValue = value.ToString().Trim();
            }
        }
        private static void assignColorWebControl(WebControl control, object value, ColoringType coloringType) {
            bool activeColor = (value == null || value.ToString().Trim() == "") ? false : true;

            switch (coloringType) { 
                case ColoringType.BLACK:
                    control.ForeColor = Color.Black;
                    break;
                case ColoringType.BLACK_GREEN:
                    if (activeColor) control.ForeColor = Color.Green;
                    else control.ForeColor = Color.Black;
                    break;
                case ColoringType.RED_GREEN:
                    if (activeColor) control.ForeColor = Color.Green;
                    else control.ForeColor = Color.Red;
                    break;
                default:
                    control.ForeColor = Color.Black;
                    break;
            }

        }
        private static void assignColorCustomControl(IControlDesign control, object value, ColoringType coloringType) {
            bool activeColor = (value == null || value.ToString().Trim() == "") ? false : true;
            switch (coloringType)
            {
                case ColoringType.BLACK:
                    control.FontColor = Color.Black;
                    break;
                case ColoringType.BLACK_GREEN:
                    if (activeColor) control.FontColor = Color.Green;
                    else control.FontColor = Color.Black;
                    break;
                case ColoringType.RED_GREEN:
                    if (activeColor) control.FontColor = Color.Green;
                    else control.FontColor = Color.Red;
                    break;
                default:
                    control.FontColor = Color.Black;
                    break;
            }
        }
        #endregion
    }
}