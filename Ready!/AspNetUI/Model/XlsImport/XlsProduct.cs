using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using AspNetUI.Support;

namespace AspNetUI.Model.XlsImport
{
    public class XlsProduct
    {
        public string ProductNumber { get; set; }

        public string Name { get; set; }
        public string ProductName
        {
            get
            {
                return String.Format("{0}({1})", this.Name, this.ProductNumber);
            }
        }

        public string Region { get; set; }
        public string CustomerGroupReservedTo { get; set; }
        public decimal BatchSize { get; set; }
        public string PackSize { get; set; }
        public string StorageCondition { get; set; }

        public XlsProduct(string name, string productNumber, string region, string customerGroupReservedTo,
                          decimal batchSize, string packSize, string storageConditions)
        {
            this.Name = name;
            this.ProductNumber = productNumber;
            this.Region = region;
            this.CustomerGroupReservedTo = customerGroupReservedTo;
            this.BatchSize = batchSize;
            this.PackSize = packSize;
            this.StorageCondition = storageConditions;
        }

        public string Serizalize()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Item ");
            XMLSerializationHelper.AppendIfNotNull(sb, "ProductNumber", this.ProductNumber == null ? null : ProductNumber.Trim());

            XMLSerializationHelper.AppendIfNotNull(sb, "Name", this.Name == null ? null : Name.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "ProductName", this.ProductName == null ? null : ProductName.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "Region", this.Region == null ? null : Region.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "CustomerGroupReservedTo", this.CustomerGroupReservedTo == null ? null : CustomerGroupReservedTo.Trim());
            XMLSerializationHelper.AppendDecimalIfNotNull(sb, "BatchSize", this.BatchSize);
            XMLSerializationHelper.AppendIfNotNull(sb, "PackSize", this.PackSize == null ? null : PackSize.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "StorageCondition", this.StorageCondition == null ? null : StorageCondition.Trim());

            sb.Append("/>");
            return sb.ToString().Replace("\n", " ") + "\n";
        }
    }
}