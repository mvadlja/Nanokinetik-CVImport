// ======================================================================================================================
// Author:		Koki-PC\Koki
// Create date:	12/23/2011 12:16:39 PM
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SU_AGENCY_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "SU_AGENCY_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Su_agency_mn_PK
	{
		private Int32? _su_agency_mn_PK;
		private Int32? _agency_FK;
		private Int32? _submission_unit_FK;
         
		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? su_agency_mn_PK
		{
			get { return _su_agency_mn_PK; }
			set { _su_agency_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? agency_FK
		{
			get { return _agency_FK; }
			set { _agency_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? submission_unit_FK
		{
			get { return _submission_unit_FK; }
			set { _submission_unit_FK = value; }
		}

		#endregion

		public Su_agency_mn_PK() { }
		public Su_agency_mn_PK(Int32? su_agency_mn_PK, Int32? agency_FK, Int32? submission_unit_FK)
		{
			this.su_agency_mn_PK = su_agency_mn_PK;
			this.agency_FK = agency_FK;
			this.submission_unit_FK = submission_unit_FK;
		}
	}

	public interface ISu_agency_mn_PKOperations : ICRUDOperations<Su_agency_mn_PK>
	{
        DataSet GetAgencyBySU(Int32? su_FK);
        void DeleteBySubmissionUnitPK(Int32? submissionUnitPk);
	}
}
