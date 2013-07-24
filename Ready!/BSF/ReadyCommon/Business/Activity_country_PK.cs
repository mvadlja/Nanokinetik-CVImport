// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	11.11.2011. 10:14:17
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ACTIVITY_COUNTRY_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ACTIVITY_COUNTRY_MN", Active=false)]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Activity_country_PK
	{
		private Int32? _activity_country_PK;
		private Int32? _activity_FK;
		private Int32? _country_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? activity_country_PK
		{
			get { return _activity_country_PK; }
			set { _activity_country_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? activity_FK
		{
			get { return _activity_FK; }
			set { _activity_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? country_FK
		{
			get { return _country_FK; }
			set { _country_FK = value; }
		}

		#endregion

		public Activity_country_PK() { }
		public Activity_country_PK(Int32? activity_country_PK, Int32? activity_FK, Int32? country_FK)
		{
			this.activity_country_PK = activity_country_PK;
			this.activity_FK = activity_FK;
			this.country_FK = country_FK;
		}
	}

	public interface IActivity_country_PKOperations : ICRUDOperations<Activity_country_PK>
	{
        void DeleteByActivityPK(Int32? activity_PK);
        DataSet GetCountriesByActivity(Int32? activity_FK);
        List<Activity_country_PK> GetCountriesListByActivity(Int32? activity_PK);
	}
}
