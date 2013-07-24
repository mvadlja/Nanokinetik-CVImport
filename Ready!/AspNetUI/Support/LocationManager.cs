using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using AspNetUI.Views;
using Ready.Model;

namespace AspNetUI.Support
{
    public class LocationManager
    {
        public static readonly LocationManager Instance = new LocationManager();
        private LocationManager() { }

        public void RetreiveApplicationLocationsFromDb()
        {
            if (CacheManager.Instance.AppLocations == null)
            {
                ILocation_PKOperations locationOperations = new Location_PKDAL();
                CacheManager.Instance.AppLocations = locationOperations.GetEntities();
            }
        }

        public Location_PK ParseLocationFromUrl(string requestUrl, List<Location_PK> locations)
        {
            if (string.IsNullOrWhiteSpace(requestUrl)) return null;

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AppVirtualPath"]))
            {
                requestUrl = requestUrl.Replace(ConfigurationManager.AppSettings["AppVirtualPath"].ToLower(), "");
            }

            string formatedRequestUrl = "~" + requestUrl;

            return locations.Find(loc => loc.location_url.ToLower() == formatedRequestUrl.ToLower());
        }

        public Location_PK GetLocationByName(string logicalUniqueName, List<Location_PK> locations)
        {
            Location_PK loc = null;

            if (!String.IsNullOrEmpty(logicalUniqueName) && locations != null)
            {
                loc = locations.Find(tempLoc => tempLoc.unique_name == logicalUniqueName);
            }

            return loc;
        }

        public Location_PK GetLocationByNameContains(string partOfLogicalUniqueName, List<Location_PK> locations)
        {
            Location_PK resulLocation = null;

            if (!String.IsNullOrEmpty(partOfLogicalUniqueName) && locations != null)
            {
                resulLocation = locations.Find(tempLoc => tempLoc.unique_name.Contains(partOfLogicalUniqueName));
            }

            return resulLocation;
        }

        public List<Location_PK> GetLocationsByNameContains(string partOfLogicalUniqueName, List<Location_PK> locations)
        {
            List<Location_PK> resultLocations = null;

            if (!String.IsNullOrEmpty(partOfLogicalUniqueName) && locations != null)
            {
                resultLocations = locations.FindAll(tempLoc => tempLoc.unique_name.Contains(partOfLogicalUniqueName));
            }

            return resultLocations;
        }

        public List<Location_PK> GetChildLocations(string logicalUniqueName, List<Location_PK> locations)
        {
            var childrenLocations = new List<Location_PK>();

            if (!String.IsNullOrEmpty(logicalUniqueName) && locations != null)
            {
                List<Location_PK> tempChildren = locations.FindAll(tempLocation => tempLocation.parent_unique_name == logicalUniqueName);
                childrenLocations.AddRange(tempChildren);
            }

            return childrenLocations;
        }

        // Locations with "." are file locations
        public bool IsLocationFolder(string logicalUniqueName, List<Location_PK> locations)
        {
            bool isFolder = true;

            Location_PK resultLocation = GetLocationByName(logicalUniqueName, locations);

            if (resultLocation != null)
            {
                if (resultLocation.location_url != null) isFolder = resultLocation.location_url.IndexOf(".") < 0;
            }

            return isFolder;
        }

        public Location_PK FindFirstAuthorizedActiveChildFileLocationInHierarchy(List<Location_PK> locationsLevel, List<Location_PK> locations)
        {
            Location_PK result = null;

            foreach (Location_PK loc in locationsLevel)
            {
                if (!IsLocationFolder(loc.unique_name, locations) && loc.active == true)
                {
                    if (SecurityHelper.IsPermitted(Permission.View, loc))
                    {
                        result = loc;
                        break;
                    }
                }
                else
                {
                    List<Location_PK> locChildren = GetChildLocations(loc.unique_name, locations);
                    result = FindFirstAuthorizedActiveChildFileLocationInHierarchy(locChildren, locations);
                }
            }

            return result;
        }

        public static Location_PK GetRefererLocation()
        {
            var locationNamesToIgnoreRefererLocation = new List<string> { "ReminderList", "UserAccount" };
            var refererLocation = Instance.ParseLocationFromUrl(HttpContext.Current.Request.ExtractRefererQuery(new List<string> { "EntityContext", "Action" }), CacheManager.Instance.AppLocations);
            var currentLocation = Instance.ParseLocationFromUrl(HttpContext.Current.Request.ExtractCurrentQuery(new List<string> { "EntityContext", "Action" }), CacheManager.Instance.AppLocations);

            if (currentLocation == null) return null;
            return locationNamesToIgnoreRefererLocation.Contains(currentLocation.unique_name) ? null : refererLocation;
        }

        public static EntityContext ResolveEntityContext(Location_PK location = null)
        {
            var resultEntityContext = EntityContext.Unknown;

            var queryString = location != null ?
                HttpUtility.ParseQueryString(location.location_url.Substring(location.location_url.IndexOf("?", StringComparison.Ordinal), location.location_url.Length - location.location_url.IndexOf("?", StringComparison.Ordinal))) :
                HttpContext.Current.Request.QueryString;

            var entityContextString = queryString["EntityContext"];
            Enum.TryParse(entityContextString, true, out resultEntityContext);

            return resultEntityContext;
        }

        public static  Location_PK GetFirstAuthorisedLocation()
        {
            Location_PK redirectLocation = null;

            var defaultRedirectLocation = Instance.GetLocationByName("Act", CacheManager.Instance.AppLocations);
            if (defaultRedirectLocation != null && SecurityHelper.IsPermitted(Permission.View, defaultRedirectLocation))
            {
                redirectLocation = defaultRedirectLocation;
            }
            else
            {
                redirectLocation = Instance.GetLocationByName("Ready", CacheManager.Instance.AppLocations);
                Location_PK firstAuthorisedLocation = Instance.FindFirstAuthorizedActiveChildFileLocationInHierarchy(new List<Location_PK> { redirectLocation }, CacheManager.Instance.AppLocations);
                if (firstAuthorisedLocation != null)
                {
                    redirectLocation = firstAuthorisedLocation;
                }
                else
                {
                    var topMenuLocations = CacheManager.Instance.AppLocations.FindAll(l => l.parent_unique_name == null && l.old_location == false && l.navigation_level != 0);
                    foreach (var location in topMenuLocations)
                    {
                        firstAuthorisedLocation = Instance.FindFirstAuthorizedActiveChildFileLocationInHierarchy(new List<Location_PK> { location }, CacheManager.Instance.AppLocations);
                        if (firstAuthorisedLocation != null)
                        {
                            redirectLocation = firstAuthorisedLocation;
                            break;
                        }
                    }
                    if (redirectLocation == null) redirectLocation = Instance.GetLocationByName("Error", CacheManager.Instance.AppLocations);
                }
            }

            return redirectLocation;
        }

        public static Location_PK RetrieveMasterLocation(EntityContext entityContext = EntityContext.Unknown)
        {
            if (entityContext == EntityContext.Unknown) entityContext = ResolveEntityContext();
            Location_PK location = null;
            if (entityContext == EntityContext.Unknown)
            {
            }
            else if (entityContext == EntityContext.Default)
            {
            }
            else if (entityContext == EntityContext.Product)
            {
            }
            else if (entityContext == EntityContext.AuthorisedProduct)
            {
            }
            else if (entityContext == EntityContext.PharmaceuticalProduct)
            {
            }
            else if (entityContext == EntityContext.SubmissionUnit)
            {
            }
            else if (entityContext == EntityContext.Project)
            {
            }
            else if (entityContext == EntityContext.ActivityMy)
            {
            }
            else if (entityContext == EntityContext.Activity)
            {
            }
            else if (entityContext == EntityContext.Task)
            {
            }
            else if (entityContext == EntityContext.TimeUnitMy)
            {
            }
            else if (entityContext == EntityContext.TimeUnit)
            {
            }
            else if (entityContext == EntityContext.Document)
            {
            }
            else if (entityContext == EntityContext.Person) location = Instance.GetLocationByName("Person", CacheManager.Instance.AppLocations);
            else if (entityContext == EntityContext.Organisation) location = Instance.GetLocationByName("Org", CacheManager.Instance.AppLocations);
            else if (entityContext == EntityContext.Type) location = Instance.GetLocationByName("Type", CacheManager.Instance.AppLocations);
            else if (entityContext == EntityContext.TimeUnitName) location = Instance.GetLocationByName("TimeUnitName", CacheManager.Instance.AppLocations);
            else if (entityContext == EntityContext.TaskName) location = Instance.GetLocationByName("TaskName", CacheManager.Instance.AppLocations);
            else if (entityContext == EntityContext.ApprovedSubstance) location = Instance.GetLocationByName("ApprovedSubstance", CacheManager.Instance.AppLocations);
            else if (entityContext == EntityContext.Substance) location = Instance.GetLocationByName("Substance", CacheManager.Instance.AppLocations);
            else if (entityContext == EntityContext.Atc) location = Instance.GetLocationByName("Atc", CacheManager.Instance.AppLocations);
            else if (entityContext == EntityContext.Alerter) location = Instance.GetLocationByName("Alerter", CacheManager.Instance.AppLocations);
            else if (entityContext == EntityContext.UserRole) location = Instance.GetLocationByName("UserRole", CacheManager.Instance.AppLocations);
            else if (entityContext == EntityContext.UserAction) location = Instance.GetLocationByName("UserAction", CacheManager.Instance.AppLocations);
            else if (entityContext == EntityContext.UserSecurity) location = Instance.GetLocationByName("Person", CacheManager.Instance.AppLocations);

            return location;
        }
    }
}