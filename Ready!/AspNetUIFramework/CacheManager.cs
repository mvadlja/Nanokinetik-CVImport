using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

namespace AspNetUIFramework
{
    public class CacheManager
    {
        public static readonly CacheManager Instance = new CacheManager();
        private CacheManager() { }

        public List<XmlLocation> AppLocations
        {
            get { return HttpContext.Current.Cache["AppLocations"] == null ? null : (List<XmlLocation>)HttpContext.Current.Cache["AppLocations"]; }
            set { HttpContext.Current.Cache["AppLocations"] = value; }
        }

        // version of Locations.xml - file change dependancy
        public string LocationsVersion
        {
            get { return HttpContext.Current.Cache["LocationsVersion"] == null ? String.Empty : (string)HttpContext.Current.Cache["LocationsVersion"]; }
            set { HttpContext.Current.Cache["LocationsVersion"] = value; }
        }
    }
}
