// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	11.1.2012. 11:24:28
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SUB_ALIAS_SUB_ALIAS_TRAN_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "SUB_ALIAS_SUB_ALIAS_TRAN_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Sub_alias_sub_alias_tran_PK
	{
		private Int32? _sub_alias_sub_alias_tran_PK;
		private Int32? _sub_alias_FK;
		private Int32? _sub_alias_tran_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? sub_alias_sub_alias_tran_PK
		{
			get { return _sub_alias_sub_alias_tran_PK; }
			set { _sub_alias_sub_alias_tran_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? sub_alias_FK
		{
			get { return _sub_alias_FK; }
			set { _sub_alias_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? sub_alias_tran_FK
		{
			get { return _sub_alias_tran_FK; }
			set { _sub_alias_tran_FK = value; }
		}

		#endregion

		public Sub_alias_sub_alias_tran_PK() { }
		public Sub_alias_sub_alias_tran_PK(Int32? sub_alias_sub_alias_tran_PK, Int32? sub_alias_FK, Int32? sub_alias_tran_FK)
		{
			this.sub_alias_sub_alias_tran_PK = sub_alias_sub_alias_tran_PK;
			this.sub_alias_FK = sub_alias_FK;
			this.sub_alias_tran_FK = sub_alias_tran_FK;
		}
	}

	public interface ISub_alias_sub_alias_tran_PKOperations : ICRUDOperations<Sub_alias_sub_alias_tran_PK>
	{

	}
}
