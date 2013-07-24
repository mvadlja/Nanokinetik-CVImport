using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetUIFramework
{
    public interface IXmlNavigation
    {
        List<XmlLocation> GetLocationsFromStore(XmlStoreInfo si);
        List<XmlLocation> GetParentHierarchyChain(XmlLocation current, List<XmlLocation> source);
    }
}
