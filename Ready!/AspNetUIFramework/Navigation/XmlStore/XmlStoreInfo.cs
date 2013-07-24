using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetUIFramework
{
    [Serializable()]
    public class XmlStoreInfo
    {
        private string _schemaFilePath;
        private string _schemaNamespace;
        private string _xmlFilePath;

        #region Properties

        public string XmlFilePath
        {
            get { return _xmlFilePath; }
            set { _xmlFilePath = value; }
        }
        public string SchemaNamespace
        {
            get { return _schemaNamespace; }
            set { _schemaNamespace = value; }
        }
        public string SchemaFilePath
        {
            get { return _schemaFilePath; }
            set { _schemaFilePath = value; }
        }

        #endregion

        public XmlStoreInfo() { }
        public XmlStoreInfo(string schemaFilePath, string schemaNamespace, string xmlFilePath)
        {
            this.SchemaFilePath = schemaFilePath;
            this.SchemaNamespace = schemaNamespace;
            this.XmlFilePath = xmlFilePath;
        }
    }
}
