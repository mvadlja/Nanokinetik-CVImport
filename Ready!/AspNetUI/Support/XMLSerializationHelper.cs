using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetUI.Support
{
    /// <summary>
    /// A class with which the serialization to a DB XML is enabled
    /// </summary>
    public class XMLSerializationHelper
    {
        private static bool IsCellValueNull(object value)
        {
            return (value == DBNull.Value || value == null);
        }

        /// <summary>
        /// Converts the date to a serializable XML parameter
        /// </summary>
        /// <param name="date">Date to be converted</param>
        /// <param name="dateTimeStringFormat">String format of date conversion</param>
        /// <returns>String suitable for DB XML reader to read</returns>
        private static String ConvertDateToSerializableString(object date, string dateTimeStringFormat)
        {
            string result;
            try
            {
                result = Convert.ToDateTime(date).ToString(dateTimeStringFormat);
            }
            catch (Exception)
            {
                throw new Exception("Conversion to DateTime failed.");
            }
            return result;
        }

        /// <summary>
        /// Appends the attribute if it is not DBNull od null
        /// </summary>
        /// <param name="sb">Appending StringBuilder</param>
        /// <param name="attributeName">Name of the XML attribute</param>
        /// <param name="cellValue">Attribute value</param>
        public static void AppendIfNotNull(StringBuilder sb, string attributeName, object cellValue)
        {
            if (!IsCellValueNull(cellValue))
            {
                cellValue = FormatStringForXML(cellValue.ToString());
                sb.AppendFormat("{0}=\"{1}\" ", attributeName, cellValue);
            }
        }

        /// <summary>
        /// Appends the date in a yyyy-MM-dd format attribute if it is not DBNull od null
        /// </summary>
        /// <param name="sb">Appending StringBuilder</param>
        /// <param name="dateAttributeName">Name of the XML attribute</param>
        /// <param name="dateValue">Attribute value</param>
        public static void AppendDateIfNotNull(StringBuilder sb, string dateAttributeName, object dateValue)
        {
            AppendDateIfNotNull(sb, dateAttributeName, dateValue, "yyyy-MM-dd");
        }

        /// <summary>
        /// Appends the date in dateTimeStringFormat format attribute if it is not DBNull od null
        /// </summary>
        /// <param name="sb">Appending StringBuilder</param>
        /// <param name="dateAttributeName">Name of the XML attribute</param>
        /// <param name="dateValue">Attribute value</param>
        /// <param name="dateTimeStringFormat">DateTime string format for serialization</param>
        public static void AppendDateIfNotNull(StringBuilder sb, string dateAttributeName, object dateValue, string dateTimeStringFormat)
        {
            if (!IsCellValueNull(dateValue))
            {
                sb.AppendFormat("{0}=\"{1}\" ", dateAttributeName, ConvertDateToSerializableString(dateValue, dateTimeStringFormat));
            }
        }

        /// <summary>
        /// Appends a decimal number with a decimal point separator - for XML parsing
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="attributeName"></param>
        /// <param name="cellValue"></param>
        public static void AppendDecimalIfNotNull(StringBuilder sb, string attributeName, object cellValue)
        {
            AppendIfNotNull(sb, attributeName, cellValue.ToString().Replace(",", "."));
        }

        public static String FormatStringForXML(String cellValue)
        {
            //cellValue = cellValue.Replace(" ", "&#160;");
            cellValue = cellValue.Replace("&", "&amp;");
            cellValue = cellValue.Replace("\"", "&quot;");

            return cellValue;
        }

        /// <summary>
        /// Dodaje " ?xml version="1.0" encoding="windows-1252" ? " u header XML-a.
        /// Potrebno dodati ako se salju stringovi koji mogu sadrzavati hrv. znakove
        /// </summary>
        /// <param name="sb"></param>
        public static void AppendCustomEncodingHeader(StringBuilder sb)
        {
            sb.AppendFormat("<{0}>", "?xml version=\"1.0\" encoding=\"windows-1252\" ?");
        }

        //public static void AppendTimeIfDateTimeNotNull (StringBuilder sb, string attributeName, object date )
        //{
        //    if ( !IsCellValueNull( date ) )
        //    {
        //        string result = string.Empty;
        //        try
        //        {
        //            result = Convert.ToDateTime( date ).ToString( "HH:mm" );
        //        }
        //        catch ( Exception )
        //        {
        //            throw new Exception( "Conversion to DateTime failed." );
        //        }
        //        sb.AppendFormat( "{0}=\"{1}\" ", attributeName, result );
        //    }
        //}
    }
}