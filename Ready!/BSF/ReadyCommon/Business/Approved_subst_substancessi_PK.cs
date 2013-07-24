// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	4.1.2012. 12:02:23
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.APPROVED_SUBST_SUBSTANCESSI_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "APPROVED_SUBST_SUBSTANCESSI_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Approved_subst_substancessi_PK
	{
		private Int32? _approved_subst_substancessi_PK;
		private Int32? _approved_substance_FK;
		private Int32? _substancessi_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? approved_subst_substancessi_PK
		{
			get { return _approved_subst_substancessi_PK; }
			set { _approved_subst_substancessi_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? approved_substance_FK
		{
			get { return _approved_substance_FK; }
			set { _approved_substance_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? substancessi_FK
		{
			get { return _substancessi_FK; }
			set { _substancessi_FK = value; }
		}

		#endregion

		public Approved_subst_substancessi_PK() { }
		public Approved_subst_substancessi_PK(Int32? approved_subst_substancessi_PK, Int32? approved_substance_FK, Int32? substancessi_FK)
		{
			this.approved_subst_substancessi_PK = approved_subst_substancessi_PK;
			this.approved_substance_FK = approved_substance_FK;
			this.substancessi_FK = substancessi_FK;
		}
	}

	public interface IApproved_subst_substancessi_PKOperations : ICRUDOperations<Approved_subst_substancessi_PK>
	{

	}
}
