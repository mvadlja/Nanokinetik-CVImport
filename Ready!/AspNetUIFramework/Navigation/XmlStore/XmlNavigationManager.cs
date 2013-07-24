using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;
using System.Xml;
using System.Configuration;
using System.IO;

namespace AspNetUIFramework
{
    public class XmlNavigationManager : IXmlNavigation
    {
        public static readonly XmlNavigationManager Instance = new XmlNavigationManager();
        private XmlNavigationManager() { }

        #region IXmlNavigation Members

        public List<XmlLocation> GetLocationsFromStore(XmlStoreInfo si)
        {
            object _lock = new object();
            List<XmlLocation> locC = new List<XmlLocation>();

            XmlDocument xDoc = null;
            XmlNodeList nodeList = null;

            // null refference check
            if (si == null)
            {
                throw new ArgumentException();
            }

            // urls-s check
            if (!File.Exists(si.SchemaFilePath) || !File.Exists(si.XmlFilePath))
            {
                throw new FileNotFoundException();
            }

            try
            {
                // synchronization
                lock (_lock)
                {
                    // loading xml, validating shema
                    xDoc = new XmlDocument();
                    xDoc.Load(si.XmlFilePath);
                    xDoc.Schemas.Add(si.SchemaNamespace, si.SchemaFilePath);
                    xDoc.Validate(new ValidationEventHandler(ValidationEventHandler));
                    nodeList = xDoc.DocumentElement.ChildNodes;

                    ExtractXmlNodes(ref locC, nodeList, String.Empty);

                    return locC;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<XmlLocation> GetParentHierarchyChain(XmlLocation current, List<XmlLocation> source)
        {
            // Bad imput parameters
            if (current == null || source == null || source.Count < 1)
                return new List<XmlLocation>();

            List<XmlLocation> result = new List<XmlLocation>();

            // Recursivly getting parent locations
            GetParentLocation(current, source, ref result);
            result.Reverse();

            return result;
        }

        #endregion

        #region Helpers

        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    throw e.Exception;
            }
        }

        private void ExtractXmlNodes(ref List<XmlLocation> locC, XmlNodeList nodeList, string parentLocationID)
        {
            XmlLocation loc = null;

            // getting all nodes on this level
            foreach (XmlNode xNode in nodeList)
            {
                // Skipping comments
                if (xNode.NodeType == XmlNodeType.Comment) continue;

                loc = new XmlLocation();
                loc.LogicalUniqueName = xNode.Attributes["LogicalUniqueName"].Value;
                loc.RowId = String.IsNullOrEmpty(xNode.Attributes["RowId"].Value) ? null : (int?)Convert.ToInt32(xNode.Attributes["RowId"].Value);
                loc.Active = String.IsNullOrEmpty(xNode.Attributes["Active"].Value) ? null : (bool?)Convert.ToBoolean(xNode.Attributes["Active"].Value);
                loc.DisplayName = xNode.Attributes["DisplayName"].Value;
                loc.NameShort = xNode.Attributes["NameShort"].Value;
                loc.LocationUrl = xNode.Attributes["LocationUrl"].Value;
                loc.LocationTarget = String.IsNullOrEmpty(xNode.Attributes["LocationTarget"].Value) ? LocationTarget._self : (LocationTarget)Enum.Parse(typeof(LocationTarget), xNode.Attributes["LocationTarget"].Value);
                loc.Description = xNode.Attributes["Description"].Value;
                loc.Roles = xNode.Attributes["Roles"].Value;
                loc.Actions = xNode.Attributes["Actions"].Value;
                // if this location has it's parent (else null)
                loc.ParentLocationID = parentLocationID;
                loc.GenerateInTopMenu = String.IsNullOrEmpty(xNode.Attributes["GenerateInTopMenu"].Value) ? null : (bool?)Convert.ToBoolean(xNode.Attributes["GenerateInTopMenu"].Value);
                loc.GenerateInTabMenu = xNode.Attributes["GenerateInTabMenu"] != null && !string.IsNullOrWhiteSpace(xNode.Attributes["GenerateInTabMenu"].Value) ? (bool?)Convert.ToBoolean(xNode.Attributes["GenerateInTabMenu"].Value) : null;
                loc.OldLocation = xNode.Attributes["OldLocation"] != null && !string.IsNullOrWhiteSpace(xNode.Attributes["OldLocation"].Value) ? (bool?)Convert.ToBoolean(xNode.Attributes["OldLocation"].Value) : null;

                locC.Add(loc);

                // check for all childs
                ExtractXmlNodes(ref locC, xNode.ChildNodes, loc.LogicalUniqueName);
            }
        }

        private void GetParentLocation(XmlLocation loc, List<XmlLocation> source, ref List<XmlLocation> result)
        {
            result.Add(loc);

            if (!String.IsNullOrEmpty(loc.ParentLocationID))
            {
                GetParentLocation(source.Find(delegate(XmlLocation tempLocation)
                {
                    if (tempLocation.LogicalUniqueName == loc.ParentLocationID) return true;
                    else return false;
                }), source, ref result);
            }
        }

        #endregion
    }
}
