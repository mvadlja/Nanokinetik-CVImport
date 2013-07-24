// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	11.11.2011. 10:14:41
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ACTIVITY_ORGANIZATION_APP_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ACTIVITY_ORGANIZATION_APP_MN", Active=false)]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Activity_organization_applicant_PK
	{
		private Int32? _activity_organization_applicant_PK;
		private Int32? _activity_FK;
		private Int32? _organization_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? activity_organization_applicant_PK
		{
			get { return _activity_organization_applicant_PK; }
			set { _activity_organization_applicant_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? activity_FK
		{
			get { return _activity_FK; }
			set { _activity_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? organization_FK
		{
			get { return _organization_FK; }
			set { _organization_FK = value; }
		}

		#endregion

		public Activity_organization_applicant_PK() { }
		public Activity_organization_applicant_PK(Int32? activity_organization_applicant_PK, Int32? activity_FK, Int32? organization_FK)
		{
			this.activity_organization_applicant_PK = activity_organization_applicant_PK;
			this.activity_FK = activity_FK;
			this.organization_FK = organization_FK;
		}
	}

	public interface IActivity_organization_applicant_PKOperations : ICRUDOperations<Activity_organization_applicant_PK>
	{
        void DeleteByActivityPK(Int32? activity_PK);
        DataSet GetApplicantsByActivity(Int32? activity_FK);
	}
}
