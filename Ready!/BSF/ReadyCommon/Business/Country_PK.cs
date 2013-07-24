// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	17.10.2011. 12:48:01
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.COUNTRY
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "COUNTRY")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Country_PK
	{
		private Int32? _country_PK;
		private String _name;
		private String _abbreviation;
		private String _region;
		private String _code;
        private Int32? _custom_sort_ID;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? country_PK
		{
			get { return _country_PK; }
			set { _country_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name
		{
			get { return _name; }
			set { _name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String abbreviation
		{
			get { return _abbreviation; }
			set { _abbreviation = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String region
		{
			get { return _region; }
			set { _region = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String code
		{
			get { return _code; }
			set { _code = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? custom_sort_ID
        {
            get { return _custom_sort_ID; }
            set { _custom_sort_ID = value; }
        }

		#endregion

		public Country_PK() { }
        public Country_PK(Int32? country_PK, String name, String abbreviation, String region, String code, Int32? custom_sort_ID)
		{
			this.country_PK = country_PK;
			this.name = name;
			this.abbreviation = abbreviation;
			this.region = region;
			this.code = code;
            this.custom_sort_ID = custom_sort_ID;
		}

        public static List<Country_PK> GetNullCheckedCountryList(List<Country_PK> oldItems)
        {
            string countryName = "";
            List<Country_PK> items = new List<Country_PK>();
            foreach (Country_PK item in oldItems)
            {
                countryName = "";
                if (String.IsNullOrWhiteSpace(item.abbreviation) && String.IsNullOrWhiteSpace(item.name)) continue;

                if (!String.IsNullOrWhiteSpace(item.abbreviation))
                {
                    countryName = item.abbreviation;
                }

                if (!String.IsNullOrWhiteSpace(item.name))
                    if (countryName != "") countryName += " - " + item.name;
                    else countryName = item.name;

                if (countryName != "")
                {
                    items.Add(item);
                    item.abbreviation = countryName;
                }
            }

            return items;
        }

	}

	public interface ICountry_PKOperations : ICRUDOperations<Country_PK>
	{
        Country_PK GetEntityByCountryName(String name);
        Country_PK GetEntityByAbbrevation(String name);

        List<Country_PK> GetAssignedEntitiesByProduct(int productPk);
        List<Country_PK> GetAvailableEntitiesByProduct(int productPk);
        List<Country_PK> GetAssignedEntitiesByTask(int taskPk);
        List<Country_PK> GetAvailableEntitiesByTask(int taskPk);
        List<Country_PK> GetAssignedEntitiesByActivity(int activityPk);
        List<Country_PK> GetAvailableEntitiesByActivity(int activityPk);
        List<Country_PK> GetAssignedEntitiesByProject(int projectPk);
        List<Country_PK> GetAvailableEntitiesByProject(int projectPk);

        List<Country_PK> GetSelectedEntitiesForActivityPK_MN(Int32 activity_PK);
        List<Country_PK> GetAvailableEntitiesForActivityPK_MN(Int32 product_PK);
        List<Country_PK> GetCountryCodeByProduct(Int32? product_PK);
        List<Country_PK> GetCountriesByTask(Int32? task_PK);
        List<Country_PK> GetSelectedEntitiesForProjectPK_MN(Int32 project_PK);
        List<Country_PK> GetAvailableEntitiesForProjectPK_MN(Int32 project_PK);
        List<Country_PK> GetSelectedEntitiesForTaskPK_MN(Int32 task_PK);
        List<Country_PK> GetAvailableEntitiesForTaskPK_MN(Int32 task_PK);

        List<Country_PK> GetEntitiesCustomSort();
	}
}
