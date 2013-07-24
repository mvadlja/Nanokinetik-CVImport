using System.Web.UI;

namespace AspNetUIFramework.ExcelExport
{
    /// <summary>
    /// Summary description for ExcelRepository
    /// </summary>
    public static class ExcelRepository
    {
        private static Control _Control;
        private static string _HTML;

        public static Control Control
        {
            get { return _Control; }
            set { _Control = value; }
        }

        public static string HTML
        {
            get { return _HTML; }
            set { _HTML = value; }
        }
    }
}
