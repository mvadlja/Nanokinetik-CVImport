using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using AspNetUI.Support;

namespace AspNetUI.Model.XlsImport
{
    public class XlsMFDocument
    {
        // Reference to AP
        public string RegistrationNumber { get; set; }

        public string Name { get; set; }
        public bool IsEffective { get; set; }
        public string ApiSource { get; set; }


        public string Serizalize()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Item ");
            XMLSerializationHelper.AppendIfNotNull(sb, "RegistrationNumber", this.RegistrationNumber == null ? null : RegistrationNumber.Trim());

            XMLSerializationHelper.AppendIfNotNull(sb, "Name", this.Name == null ? null : Name.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "IsEffective", this.IsEffective);
            XMLSerializationHelper.AppendIfNotNull(sb, "ApiSource", this.ApiSource == null ? null : ApiSource.Trim());

            sb.Append("/>");
            return sb.ToString().Replace("\n", " ") + "\n";
        }
    }
}