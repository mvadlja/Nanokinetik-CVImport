using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ready.Model;
using System.Globalization;

namespace AspNetUI.Support
{
    public class CBLoader
    {
        static ICountry_PKOperations _countryOperations;
        static IType_PKOperations _typeOperations;
        static IPerson_PKOperations _personOperations;
        static IOrganization_PKOperations _organizationOperations;
        static IDomain_PKOperations _domainOperations;
        static IActivity_PKOperations _activity_PKOperations;
        static ITime_unit_name_PKOperations _time_unit_name_PKOperations;
        static IPharmaceutical_form_PKOperations _pharmaceutical_form_PKOperations;
        static ILanguagecode_PKOperations _languageCodeOperations;
        static ITask_PKOperations _taskOperations;
        static ITask_name_PKOperations _taskNameOperations;
        static IAdminroute_PKOperations _adminroute_PKOperations = new Adminroute_PKDAL();
        static IXml_report_mapping_PKOperations _xmlReportMappingOperations = new Xml_report_mapping_PKDAL();

        public static List<Country_PK> LoadCountries()
        {
            List<Country_PK> countries = new List<Country_PK>();
            _countryOperations = new Country_PKDAL();

            countries = Country_PK.GetNullCheckedCountryList(_countryOperations.GetEntitiesCustomSort());
            return countries;
        }

        public static List<Country_PK> LoadCountriesRaw()
        {
            List<Country_PK> countries = new List<Country_PK>();
            _countryOperations = new Country_PKDAL();
            countries = _countryOperations.GetEntitiesCustomSort();
            return countries;
        }

        public static List<Type_PK> LoadTypes()
        {
            List<Type_PK> types = new List<Type_PK>();
            List<Type_PK> typesWithoutNull = new List<Type_PK>();

            _typeOperations = new Type_PKDAL();
            types = _typeOperations.GetEntities();

            foreach (var item in types)
            {
                string name = "";
                if (String.IsNullOrWhiteSpace(item.name)) continue;

                if (!String.IsNullOrWhiteSpace(item.name))
                    name = item.name;

                if (name != "")
                    typesWithoutNull.Add(item);
            }

            typesWithoutNull.Sort(delegate(Type_PK c1, Type_PK c2)
            {
                return c1.name.CompareTo(c2.name);
            });
            return typesWithoutNull;
        }

        public static List<Person_PK> LoadResponsibleUsers()
        {
            _personOperations = new Person_PKDAL();
            List<Person_PK> items = _personOperations.GetEntities();

            items.Sort(delegate(Person_PK c1, Person_PK c2)
            {
                return c1.name.CompareTo(c2.name);
            });

            foreach (Person_PK item in items)
                item.name += " " + item.familyname;

            return items;
        }

        public static List<Organization_PK> LoadAgency()
        {
            return LoadOrganizationByRole("Agency");
        }

        public static List<Domain_PK> LoadDomains()
        {
            _domainOperations = new Domain_PKDAL();
            List<Domain_PK> domains = new List<Domain_PK>();
            List<Domain_PK> domainsWithoutNull = new List<Domain_PK>();
            domains = _domainOperations.GetEntities();

            foreach (var item in domains)
            {
                string name = "";
                if (String.IsNullOrWhiteSpace(item.name)) continue;

                if (!String.IsNullOrWhiteSpace(item.name))
                    name = item.name;

                if (name != "")
                    domainsWithoutNull.Add(item);
            }
            domainsWithoutNull.Sort(delegate(Domain_PK c1, Domain_PK c2)
            {
                return c1.name.CompareTo(c2.name);
            });
            return domainsWithoutNull;
        }

        public static List<Activity_PK> LoadActivities()
        {
            _activity_PKOperations = new Activity_PKDAL();
            List<Activity_PK> activities = new List<Activity_PK>();
            List<Activity_PK> activitiesWithoutNull = new List<Activity_PK>();
            activities = _activity_PKOperations.GetEntities();

            foreach (var item in activities)
            {
                if (String.IsNullOrWhiteSpace(item.name)) item.name = "Missing";

                activitiesWithoutNull.Add(item);
            }

            activitiesWithoutNull.Sort(delegate(Activity_PK c1, Activity_PK c2)
            {
                return c1.name.CompareTo(c2.name);
            });

            return activitiesWithoutNull;
        }

        public static List<Time_unit_name_PK> LoadTimeUnitName()
        {
            _time_unit_name_PKOperations = new Time_unit_name_PKDAL();
            List<Time_unit_name_PK> timeUnitName = new List<Time_unit_name_PK>();
            List<Time_unit_name_PK> timeUnitNameWithoutNull = new List<Time_unit_name_PK>();

            timeUnitName = _time_unit_name_PKOperations.GetEntities();

            foreach (var item in timeUnitName)
            {
                string name = "";
                if (String.IsNullOrWhiteSpace(item.time_unit_name)) continue;

                if (!String.IsNullOrWhiteSpace(item.time_unit_name))
                    name = item.time_unit_name;

                if (name != "")
                    timeUnitNameWithoutNull.Add(item);
            }

            timeUnitNameWithoutNull.Sort(delegate(Time_unit_name_PK item1, Time_unit_name_PK item2)
            {
                return item1.time_unit_name.CompareTo(item2.time_unit_name);
            });

            return timeUnitNameWithoutNull;
        }

        public static List<Organization_PK> LoadDistributor()
        {
            return LoadOrganizationByRole("Distributor");
        }

        public static List<Organization_PK> LoadOrganizationByRole(string role)
        {
            _organizationOperations = new Organization_PKDAL();

            List<Organization_PK> organizations = _organizationOperations.GetOrganizationsByRole(role);
            List<Organization_PK> organizationWithoutNull = new List<Organization_PK>();

            foreach (var item in organizations)
            {
                string name = "";
                if (String.IsNullOrWhiteSpace(item.name_org)) continue;

                if (!String.IsNullOrWhiteSpace(item.name_org))
                    name = item.name_org;

                if (name != "")
                    organizationWithoutNull.Add(item);
            }

            organizationWithoutNull.Sort(delegate(Organization_PK c1, Organization_PK c2)
            {
                return c1.name_org.CompareTo(c2.name_org);
            });

            return organizationWithoutNull;
        }

        public static List<Pharmaceutical_form_PK> LoadPharmaceuticalForm()
        {
            _pharmaceutical_form_PKOperations = new Pharmaceutical_form_PKDAL();
            List<Pharmaceutical_form_PK> items = _pharmaceutical_form_PKOperations.GetEntities();
            List<Pharmaceutical_form_PK> sortedItems = new List<Pharmaceutical_form_PK>();

            foreach (var item in items)
            {
                if (!String.IsNullOrWhiteSpace(item.name))
                {
                    item.name = item.name.Trim();
                    sortedItems.Add(item);
                }
            }

            sortedItems.Sort(delegate(Pharmaceutical_form_PK c1, Pharmaceutical_form_PK c2)
            {
                return c1.name.CompareTo(c2.name);
            });

            return sortedItems;
        }

        public static List<Languagecode_PK> LoadLanguageCode()
        {
            _languageCodeOperations = _languageCodeOperations = new Languagecode_PKDAL();
            List<Languagecode_PK> items = _languageCodeOperations.GetEntities();
            List<Languagecode_PK> languageCodeWithoutNull = new List<Languagecode_PK>();

            foreach (var item in items)
            {
                if (!string.IsNullOrWhiteSpace(item.code))
                    languageCodeWithoutNull.Add(item);
            }

            languageCodeWithoutNull.Sort(delegate(Languagecode_PK p1, Languagecode_PK p2)
            {
                return p1.code.CompareTo(p2.code);
            });
            return languageCodeWithoutNull;
        }

        public static List<Task_PK> LoadTasks()
        {
            _taskOperations = new Task_PKDAL();
            List<Task_PK> tasks = _taskOperations.GetEntities();

            return tasks;
        }

        public static List<Task_name_PK> LoadTaskName()
        {
            _taskNameOperations= new Task_name_PKDAL();
            List<Task_name_PK> taskName = _taskNameOperations.GetEntities();
            List<Task_name_PK> taskNameWithoutNull = new List<Task_name_PK>();

            foreach (var item in taskName)
            {
                if (!string.IsNullOrWhiteSpace(item.task_name))
                    taskNameWithoutNull.Add(item);
            }

            taskNameWithoutNull.Sort(delegate(Task_name_PK t1, Task_name_PK t2)
            {
                return t1.task_name.CompareTo(t2.task_name);
            });

            return taskNameWithoutNull;
        }

        public static List<Adminroute_PK> LoadAdminRoutes()
        {
            List<Adminroute_PK> items = _adminroute_PKOperations.GetEntities();
            List<Adminroute_PK> validItems = new List<Adminroute_PK>();

            foreach (var item in items)
            {
                if (!string.IsNullOrWhiteSpace(item.adminroutecode))
                    validItems.Add(item);
            }

            validItems.Sort(delegate(Adminroute_PK item1, Adminroute_PK item2)
            {
                return item1.adminroutecode.CompareTo(item2.adminroutecode);
            });

            return validItems;
        }

        public static List<Type_PK> LoadDocumentVersionNumbers() {
            List<Type_PK> allTypes = CBLoader.LoadTypes();
            List<Type_PK> vnumbers = allTypes.FindAll(delegate(Type_PK item){
                if ( item.group != "VD" ) return false;
                double d = 0.0;
                if (!double.TryParse(item.name, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out d)) d = 0.0;

                if (d < 10.0) return true;
                return false;
            });

            vnumbers.Sort(delegate(Type_PK t1, Type_PK t2){
                double d1 = 0.0, d2 = 0.0;
                if (!double.TryParse(t1.name, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out d1)) d1 = 0.0;
                if (!double.TryParse(t2.name, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out d2)) d2 = 0.0;
                return d1.CompareTo(d2);
            });

            return vnumbers;
        }

        static Dictionary<string, string> xmlMap = new Dictionary<string, string>();
        public static string MapXmlTag(string xmlTag)
        {
            if (xmlMap.Count == 0)
            {
                FillMapDictionary();
            }

            if (xmlMap.ContainsKey(xmlTag))
                return xmlMap[xmlTag];
            else
                return xmlTag;
        }

        private static void FillMapDictionary()
        {
            List<Xml_report_mapping_PK> xmlReportMap = _xmlReportMappingOperations.GetEntities();
            xmlMap.Clear();

            foreach (var item in xmlReportMap)
            {
                xmlMap.Add(item.xml_tag, item.display_tag);
            }
        }

        public static List<Person_PK> LoadQPPVs()
        {
            _personOperations = new Person_PKDAL();
            List<Person_PK> items = _personOperations.GetPersonsByRole("QPPV");

            items.Sort(delegate(Person_PK c1, Person_PK c2)
            {
                return c1.name.CompareTo(c2.name);
            });

            foreach (Person_PK item in items)
                item.name += " " + item.familyname;

            return items;
        }
    }
}