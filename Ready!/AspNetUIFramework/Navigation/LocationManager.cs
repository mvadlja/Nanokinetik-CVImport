using System;
using System.Collections.Generic;
using System.Linq;
using CommonTypes;
using System.Configuration;
using System.IO;
using System.Web;

namespace AspNetUIFramework
{
    public class LocationManager
    {
        public static readonly LocationManager Instance = new LocationManager();
        private LocationManager() { }

        // Updating application locations cache if necessary (null or changed)
        public void RetreiveApplicationLocations()
        {
            if (CacheManager.Instance.AppLocations == null || CacheManager.Instance.LocationsVersion != File.GetLastWriteTime(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["Navi_XmlLocations"])).ToString())
            {
                CacheManager.Instance.AppLocations = XmlNavigationManager.Instance.GetLocationsFromStore(new XmlStoreInfo(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["Navi_XsdLocation"]), ConfigurationManager.AppSettings["Navi_XsdNamespace"], HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["Navi_XmlLocations"])));
                CacheManager.Instance.LocationsVersion = File.GetLastWriteTime(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["Navi_XmlLocations"])).ToString();
            }
        }

        public void RetreiveApplicationLocationsFromDb()
        {
            if (CacheManager.Instance.AppLocations == null)
            {
                CacheManager.Instance.AppLocations = XmlNavigationManager.Instance.GetLocationsFromStore(new XmlStoreInfo(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["Navi_XsdLocation"]), ConfigurationManager.AppSettings["Navi_XsdNamespace"], HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["Navi_XmlLocations"])));
            }
        }

        // Input must be Request.Url.LocalPath => example: "/Pages/Prava/Users.aspx"
        public XmlLocation ParseLocationFromUrl(string requestUrl, List<XmlLocation> locations)
        {
            // Mapping application location to request path
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AppVirtualPath"]))
            {
                requestUrl = requestUrl.Replace(ConfigurationManager.AppSettings["AppVirtualPath"].ToLower(), "");
            }

            // formatting request url to look like: ~/Pages/Prava/Users.aspx
            string formatedRequestURL = "~" + requestUrl;

            return locations.Find((XmlLocation loc) => loc.LocationUrl.ToLower() == formatedRequestURL.ToLower());
        }

        // Returns parent location by LogicalUniqueName
        public XmlLocation GetLocationByName(string logicalUniqueName, List<XmlLocation> locations)
        {
            XmlLocation loc = null;

            if (!String.IsNullOrEmpty(logicalUniqueName) && locations != null)
            {
                loc = locations.Find((XmlLocation tempLoc) => tempLoc.LogicalUniqueName == logicalUniqueName);

                //if (loc != null)
                //{
                //    if (AuthorizeLocation(loc, locations) != RightTypes.Delete) loc = null;
                //}
            }

            return loc;
        }

        // Returns parent location by partOfLogicalUniqueName
        public XmlLocation GetLocationByNameContains(string partOfLogicalUniqueName, List<XmlLocation> locations)
        {
            XmlLocation loc = null;

            if (!String.IsNullOrEmpty(partOfLogicalUniqueName) && locations != null)
                {
                loc = locations.Find((XmlLocation tempLoc) => tempLoc.LogicalUniqueName.Contains(partOfLogicalUniqueName));

                //if (loc != null)
                //{
                //    if (AuthorizeLocation(loc, locations) != RightTypes.Delete) loc = null;
                //}
                }

            return loc;
        }

        // Returns parent locations by partOfLogicalUniqueName
        public List<XmlLocation> GetLocationsByNameContains(string partOfLogicalUniqueName, List<XmlLocation> locations)
        {
            List<XmlLocation> _locations = null;

            if (!String.IsNullOrEmpty(partOfLogicalUniqueName) && locations != null)
            {
                _locations = locations.FindAll((XmlLocation tempLoc) => tempLoc.LogicalUniqueName.Contains(partOfLogicalUniqueName));

                //if (loc != null)
                //{
                //    if (AuthorizeLocation(loc, locations) != RightTypes.Delete) loc = null;
                //}
            }

            return _locations;
        }

        // Returns child locations of parent
        public List<XmlLocation> GetChildLocations(string logicalUniqueName, List<XmlLocation> locations)
        {
            var children = new List<XmlLocation>();

            if (!String.IsNullOrEmpty(logicalUniqueName) && locations != null)
            {
                List<XmlLocation> tempChildren = locations.FindAll(tempLocation => tempLocation.ParentLocationID == logicalUniqueName);

                //children.AddRange(tempChildren.Where(loc => AuthorizeLocation(loc, locations) == RightTypes.Delete));
                children.AddRange(tempChildren);
            }

            return children;
        }

        // Locations with "." are file locations
        public bool IsLocationFolder(string logicalUniqueName, List<XmlLocation> locations)
        {
            bool isFolder = true;

            XmlLocation loc = GetLocationByName(logicalUniqueName, locations);

            if (loc != null)
            {
                isFolder = loc.LocationUrl.IndexOf(".") < 0;
            }

            return isFolder;
        }

        // Recursion!
        public XmlLocation FindFirstAuthorizedActiveChildFileLocationInHierarchy(List<XmlLocation> locationsLevel, List<XmlLocation> locations)
        {
            XmlLocation result = null;

            foreach (XmlLocation loc in locationsLevel)
            {
                if (!IsLocationFolder(loc.LogicalUniqueName, locations) && loc.Active == true)// && AuthorizeLocation(loc, locations) == RightTypes.Delete)
                {
                    result = loc;
                    break;
                }
                else
                {
                    List<XmlLocation> locChildren = GetChildLocations(loc.LogicalUniqueName, locations);
                    result = FindFirstAuthorizedActiveChildFileLocationInHierarchy(locChildren, locations);
                }
            }

            return result;
        }

        // Authorizes location based on user in session and its roles, and roles configured in locations xml
        public RightTypes AuthorizeLocation(XmlLocation location, List<XmlLocation> locations)
        {
            RightTypes rightType = RightTypes.Restricted;

            if (!String.IsNullOrEmpty(location.Roles))
            {
                string[] availableRoles = location.Roles.Split(new string[] { ", " }, StringSplitOptions.None);

                foreach (var userRole in SessionManager.Instance.CurrentUser.Roles.Where(userRole => availableRoles.Any(availableRole => userRole == availableRole)))
                {
                    rightType = RightTypes.Delete;
                }
            }
            else rightType = RightTypes.Delete;

            return rightType;
        }
    }
}
