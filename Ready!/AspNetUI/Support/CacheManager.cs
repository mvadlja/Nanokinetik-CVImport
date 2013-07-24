using System.Collections.Generic;
using System.Web;
using Ready.Model;

namespace AspNetUI.Support
{
    public class CacheManager
    {
        public static readonly CacheManager Instance = new CacheManager();
        private CacheManager() { }

        public List<Location_PK> AppLocations
        {
            get { return HttpRuntime.Cache["AppLocations"] != null ? (List<Location_PK>)HttpRuntime.Cache["AppLocations"] : null; }
            set { HttpRuntime.Cache["AppLocations"] = value; }
        }
    }
}