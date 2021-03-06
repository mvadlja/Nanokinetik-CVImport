﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using AspNetUI.Support;

namespace AspNetUI.Model.XlsImport
{
    public class XlsPackagingMaterial
    {

        // Reference to product
        public string ProductNumber { get; set; }

        public string Name { get; set; }


        public string Serizalize()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Item ");
            XMLSerializationHelper.AppendIfNotNull(sb, "ProductNumber", this.ProductNumber == null ? null : ProductNumber.Trim());

            XMLSerializationHelper.AppendIfNotNull(sb, "Name", this.Name == null ? null : Name.Trim());

            sb.Append("/>");
            return sb.ToString().Replace("\n", " ") + "\n";
        }
    }
}