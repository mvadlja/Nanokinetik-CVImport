using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace AspNetUIFramework
{
    /// <summary>
    /// Summary description for RegularExp
    /// </summary>
    public static class ValidationHelper
    {
        private static string RE_JMBG = "^(\\d{13})$";
        private static string RE_OIB = "^(\\d{11})$";
        private static string RE_JMBAG = "^(\\d{10})$";
        private static string RE_Email = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        private static string RE_WebAdress = "(http(s)?://)?([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?";
        private static string RE_Number = "\\d+";
        private static string RE_ZiroRacun = "^(\\d{7})+(\\-)+(\\d{10})$";
        private static string RE_Godina = "^(\\d{4})$";
        private static string RE_GUID = @"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$";

        public static bool IsValidDateTime(string s)
        {
            bool isValid = false;
            DateTime temp = DateTime.MinValue;

            if (!String.IsNullOrEmpty(s))
            {
                if (DateTime.TryParse(s, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out temp)) isValid = true;
            }

            return isValid;
        }

        public static bool IsValidDateTime(string s, CultureInfo ci)
        {
            bool isValid = false;
            DateTime temp = DateTime.MinValue;

            if (!String.IsNullOrEmpty(s))
            {
                if (DateTime.TryParse(s, ci.DateTimeFormat, DateTimeStyles.None, out temp)) isValid = true;
            }

            return isValid;
        }

        public static bool IsValidInt(string s)
        {
            bool isValid = false;
            int temp = 0;

            if (!String.IsNullOrEmpty(s))
            {
                if (int.TryParse(s, out temp)) isValid = true;
            }

            return isValid;
        }

        public static bool IsValidDouble(string s)
        {
            bool isValid = false;
            double temp = 0;

            if (!String.IsNullOrEmpty(s))
            {
                if (double.TryParse(s, out temp)) isValid = true;
            }

            return isValid;
        }

        public static bool IsValidDecimal(string s)
        {
            bool isValid = false;
            decimal temp = 0;

            if (!String.IsNullOrEmpty(s))
            {
                if (decimal.TryParse(s, out temp)) isValid = true;
            }

            return isValid;
        }

        public static bool IsValidBool(string s)
        {
            bool isValid = false;
            bool temp = false;

            if (!String.IsNullOrEmpty(s))
            {
                if (bool.TryParse(s, out temp)) isValid = true;
            }

            return isValid;
        }


        public static bool IsValidJMBG(string s)
        {
            Regex r = new Regex(RE_JMBG);
            return r.IsMatch(s);
        }

        public static bool IsValidOIB(string s)
        {
            bool firstCheckValid = false;
            bool validOIB = false;

            Regex r = new Regex(RE_OIB);
            firstCheckValid = r.IsMatch(s);

            if (!firstCheckValid) return false;

            int incrementStep = 10;
            int tempValue = 0;

            // ISO 7064 (MOD 11,10) Check
            for (int i = 0; i < (s.Length - 1); i++)
            {
                tempValue = (Convert.ToInt32(s[i].ToString()) + incrementStep);
                tempValue %= 10;
                if (tempValue == 0) tempValue = 10;
                tempValue *= 2;
                tempValue %= 11;
                incrementStep = tempValue;
            }

            tempValue = 11 - tempValue;
            if (tempValue == 10)
                tempValue = 0;

            if (tempValue.ToString() == s[s.Length - 1].ToString()) validOIB = true;
            else validOIB = false;

            return validOIB;
        }

        public static bool IsValidJMBAG(string s)
        {
            Regex r = new Regex(RE_JMBAG);
            return r.IsMatch(s);
        }

        public static bool IsValidEmail(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;
            Regex r = new Regex(RE_Email);
            return r.IsMatch(s);
        }

        public static bool IsValidWebAdress(string s)
        {
            Regex r = new Regex(RE_WebAdress);
            return r.IsMatch(s);
        }

        public static bool IsValidNumber(string s)
        {
            string regEx = RE_Number;

            Regex r = new Regex(RE_Number);
            return r.IsMatch(s);
        }

        public static bool IsValidZiroRacun(string s)
        {
            Regex r = new Regex(RE_ZiroRacun);
            return r.IsMatch(s);
        }

        public static bool IsValidGUID(string s)
        {
            Regex r = new Regex(RE_GUID, RegexOptions.Compiled);
            return r.IsMatch(s);
        }
    }
}