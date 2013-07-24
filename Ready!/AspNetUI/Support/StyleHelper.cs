using System.Linq;
using System.Web.UI.WebControls;

namespace AspNetUI.Support
{
    public static class StyleHelper
    {
        public static bool DisableButtonsWithCssClass(Panel panel, string cssClass)
        {
            var buttonsToDisable = panel.Controls.OfType<Button>().Where(control => control.CssClass.Contains(cssClass));
            foreach (var buttonToDisable in buttonsToDisable)
            {
                if (buttonToDisable == null) return false;

                buttonToDisable.Disable();
            }

            return true;
        }

        public static bool DisableLinkButtonsWithCssClass(Panel panel, string cssClass)
        {
            var buttonsToDisable = panel.Controls.OfType<LinkButton>().Where(control => control.CssClass.Contains(cssClass));
            foreach (var buttonToDisable in buttonsToDisable)
            {
                if (buttonToDisable == null) return false;

                buttonToDisable.Disable();
            }

            return true;
        }

        public static bool EnableButtonsWithCssClass(Panel panel, string cssClass)
        {
            var buttonsToEnable = panel.Controls.OfType<Button>().Where(control => control.CssClass.Contains(cssClass));
            foreach (var buttonToEnable in buttonsToEnable)
            {
                if (buttonToEnable == null) return false;

                buttonToEnable.Enable();
            }

            return true;
        }

        public static bool EnableLinkButtonsWithCssClass(Panel panel, string cssClass)
        {
            var buttonsToEnable = panel.Controls.OfType<LinkButton>().Where(control => control.CssClass.Contains(cssClass));
            foreach (var buttonToEnable in buttonsToEnable)
            {
                if (buttonToEnable == null) return false;

                buttonToEnable.Enable();
            }

            return true;
        }

        public static void Disable(this Button buttonToDisable)
        {
            DisableControl(buttonToDisable);
        }

        public static void Enable(this Button buttonToEnable)
        {
            EnableControl(buttonToEnable);
        }

        public static void Enable(this LinkButton linkButton)
        {
            EnableControl(linkButton);
        }

        public static void Disable(this LinkButton linkButton)
        {
            DisableControl(linkButton);
        }

        private static void DisableControl(WebControl webControl)
        {
            webControl.Enabled = false;
            webControl.Attributes.CssStyle.Add("cursor", "default");
            webControl.Attributes.CssStyle.Add("color", "gray !important");
            if (webControl is HyperLink) webControl.Attributes.CssStyle.Add("border", "0px");
        }

        private static void EnableControl(WebControl webControl)
        {
            webControl.Enabled = true;
            webControl.Attributes.CssStyle.Add("cursor", "pointer");
            webControl.Attributes.CssStyle.Add("color", "black !important");
        }
    }
}