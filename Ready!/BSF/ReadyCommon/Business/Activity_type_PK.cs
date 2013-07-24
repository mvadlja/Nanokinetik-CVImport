// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	11.11.2011. 10:15:01
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ACTIVITY_TYPE_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ACTIVITY_TYPE_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Activity_type_PK
	{
		private Int32? _activity_type_PK;
		private Int32? _activity_FK;
		private Int32? _type_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? activity_type_PK
		{
			get { return _activity_type_PK; }
			set { _activity_type_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? activity_FK
		{
			get { return _activity_FK; }
			set { _activity_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? type_FK
		{
			get { return _type_FK; }
			set { _type_FK = value; }
		}

		#endregion

		public Activity_type_PK() { }
		public Activity_type_PK(Int32? activity_type_PK, Int32? activity_FK, Int32? type_FK)
		{
			this.activity_type_PK = activity_type_PK;
			this.activity_FK = activity_FK;
			this.type_FK = type_FK;
		}
	}

	public interface IActivity_type_PKOperations : ICRUDOperations<Activity_type_PK>
	{
        void DeleteByActivityPK(Int32? activity_PK);
        DataSet GetTypesByActivity(Int32? activity_FK);
	}
}
