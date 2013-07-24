// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	4.1.2012. 12:02:23
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.APPROVED_SUBST_SUBST_ATT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "APPROVED_SUBST_SUBST_ATT_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Approved_subst_subst_att_PK
	{
		private Int32? _approved_subst_subst_att_PK;
		private Int32? _approved_substance_FK;
		private Int32? _substance_attachment_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? approved_subst_subst_att_PK
		{
			get { return _approved_subst_subst_att_PK; }
			set { _approved_subst_subst_att_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? approved_substance_FK
		{
			get { return _approved_substance_FK; }
			set { _approved_substance_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? substance_attachment_FK
		{
			get { return _substance_attachment_FK; }
			set { _substance_attachment_FK = value; }
		}

		#endregion

		public Approved_subst_subst_att_PK() { }
		public Approved_subst_subst_att_PK(Int32? approved_subst_subst_att_PK, Int32? approved_substance_FK, Int32? substance_attachment_FK)
		{
			this.approved_subst_subst_att_PK = approved_subst_subst_att_PK;
			this.approved_substance_FK = approved_substance_FK;
			this.substance_attachment_FK = substance_attachment_FK;
		}
	}

	public interface IApproved_subst_subst_att_PKOperations : ICRUDOperations<Approved_subst_subst_att_PK>
	{

	}
}
