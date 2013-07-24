// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	4.1.2012. 12:02:23
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.APPROVED_SUBST_SUBST_ALIAS_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "APPROVED_SUBST_SUBST_ALIAS_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Approved_substance_subst_alias_PK
	{
		private Int32? _approved_substance_subst_alias_PK;
		private Int32? _approved_substance_FK;
		private Int32? _substance_alias_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? approved_substance_subst_alias_PK
		{
			get { return _approved_substance_subst_alias_PK; }
			set { _approved_substance_subst_alias_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? approved_substance_FK
		{
			get { return _approved_substance_FK; }
			set { _approved_substance_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? substance_alias_FK
		{
			get { return _substance_alias_FK; }
			set { _substance_alias_FK = value; }
		}

		#endregion

		public Approved_substance_subst_alias_PK() { }
		public Approved_substance_subst_alias_PK(Int32? approved_substance_subst_alias_PK, Int32? approved_substance_FK, Int32? substance_alias_FK)
		{
			this.approved_substance_subst_alias_PK = approved_substance_subst_alias_PK;
			this.approved_substance_FK = approved_substance_FK;
			this.substance_alias_FK = substance_alias_FK;
		}
	}

	public interface IApproved_substance_subst_alias_PKOperations : ICRUDOperations<Approved_substance_subst_alias_PK>
	{

	}
}
