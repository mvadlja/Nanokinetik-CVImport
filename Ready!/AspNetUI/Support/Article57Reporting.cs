using System;
using System.Web.UI;
using AspNetUI.Views;
using AspNetUI.Views.Shared.Interface;
using System.Linq;

namespace AspNetUI.Support
{
    public static class Article57Reporting
    {
        /// <summary>
        /// Gets a proper css class for a Article 57 reporting relevant control
        /// </summary>
        /// <param name="isArticle57Conditional">Italics label</param>
        /// <param name="isArticle57Mandatory">Red/Green(true) or Black/Green (false) coloring option</param>
        /// <param name="isArticle57Empty">Indicates is control text is empty</param>
        /// <param name="isArticle57Relevant">Underlines label</param>
        /// <returns>css class</returns>
        public static string GetCssClass(bool isArticle57Conditional, bool isArticle57Mandatory, bool isArticle57Empty = false, bool? isArticle57Relevant = true)
        {
            var cssClass = string.Empty;
            if (isArticle57Relevant != null && (bool)isArticle57Relevant)
            {
                if (isArticle57Conditional && isArticle57Mandatory) cssClass = isArticle57Empty ? "article57RelevantConditionalMandatoryEmpty" : "article57RelevantConditionalMandatoryNonEmpty";
                else if (isArticle57Conditional) cssClass = isArticle57Empty ? "article57RelevantConditionalNonMandatoryEmpty" : "article57RelevantConditionalNonMandatoryNonEmpty";
                else if (isArticle57Mandatory) cssClass = isArticle57Empty ? "article57RelevantNonConditionalMandatoryEmpty" : "article57RelevantNonConditionalMandatoryNonEmpty";
                else cssClass = isArticle57Empty ? "article57RelevantNonConditionalNonMandatoryEmpty" : "article57RelevantNonConditionalNonMandatoryNonEmpty";
            }
            else
            {
                if (isArticle57Conditional && isArticle57Mandatory) cssClass = isArticle57Empty ? "article57NonRelevantConditionalMandatoryEmpty" : "article57NonRelevantConditionalMandatoryNonEmpty";
                else if (isArticle57Conditional) cssClass = isArticle57Empty ? "article57NonRelevantConditionalNonMandatoryEmpty" : "article57NonRelevantConditionalNonMandatoryNonEmpty";
                else if (isArticle57Mandatory) cssClass = isArticle57Empty ? "article57NonRelevantNonConditionalMandatoryEmpty" : "article57NonRelevantNonConditionalMandatoryNonEmpty";
                else cssClass = isArticle57Empty ? "article57NonRelevantNonConditionalNonMandatoryEmpty" : "article57NonRelevantNonConditionalNonMandatoryNonEmpty";
            }

            return cssClass;
        }

        /// <summary>
        /// Gets a proper css class for a Article 57 reporting relevant control
        /// </summary>
        /// <param name="isArticle57Conditional">Italics label</param>
        /// <param name="isArticle57Mandatory">Red/Green(true) or Black/Green (false) coloring option</param>
        /// <param name="controlTextValue">Control text value </param>
        /// <param name="isArticle57Relevant">Underlines label</param>
        /// <returns>css class</returns>
        public static string GetCssClass(bool isArticle57Conditional, bool isArticle57Mandatory, string controlTextValue = "", bool? isArticle57Relevant = true)
        {
            var isArticle57Empty = string.IsNullOrWhiteSpace(controlTextValue) || controlTextValue == Constant.ControlDefault.LbPrvText;

            return GetCssClass(isArticle57Conditional, isArticle57Mandatory, isArticle57Empty, isArticle57Relevant);
        }


        /// <summary>
        /// Removes all "article57*" named css classes from control label
        /// </summary>
        /// <param name="control">Control container from which css classes will be removed</param>
        public static void RemoveAllArticle57CssClasses(Control control)
        {
            var article57RelevantControls = control.Controls.OfType<IArticle57Relevant>();

            if (article57RelevantControls.Any())
            {
                foreach (var article57RelevantControl in article57RelevantControls)
                {
                    article57RelevantControl.LblName.RemoveCssClassContains("article57");
                }
            }
        }
    }
}