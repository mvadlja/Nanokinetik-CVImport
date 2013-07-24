using System;
using AspNetUI.Views;
using Ready.Model;

namespace AspNetUI.Support
{
    public static class PersonHelper
    {
        /// <summary>
        /// Gets QPPV person formatted display text as <person.FullName> + " (" <qppvCode.qppv_code> ")"
        /// </summary>
        /// <param name="qppvCodeFk"></param>
        /// <param name="defaultEmptyValue"> </param>
        /// <returns></returns>
        public static string GetQppvNameFormatted(int? qppvCodeFk, string defaultEmptyValue = Constant.DefaultEmptyValue)
        {
            var personOperations = new Person_PKDAL();
            var qppvCodeOperations = new Qppv_code_PKDAL();

            var qppvCode = qppvCodeFk.HasValue ? qppvCodeOperations.GetEntity(qppvCodeFk) : null;
            if (qppvCode == null) return defaultEmptyValue;

            var qppvText = String.Empty;
            var qppvPerson = personOperations.GetEntity(qppvCode.person_FK);

            if (qppvPerson != null && qppvCode.person_FK != null && !string.IsNullOrWhiteSpace(qppvCode.qppv_code))
            {
                qppvText = qppvPerson.FullName + " (" + qppvCode.qppv_code + ")";
            }
            else if (qppvPerson != null && !string.IsNullOrWhiteSpace(qppvPerson.FullName))
            {
                qppvText = qppvPerson.FullName;
            }
            else if (!string.IsNullOrWhiteSpace(qppvCode.qppv_code))
            {
                qppvText = " (" + qppvCode + ")";
            }

            return qppvText.Trim();
        }
    }
}