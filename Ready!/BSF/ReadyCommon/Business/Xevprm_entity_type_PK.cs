// ======================================================================================================================
// Author:		Kiki-PC\Kiki
// Create date:	8/21/2012 2:15:17 PM
// Description:	GEM2 Generated class for table ready_billev_production.dbo.XEVPRM_ENTITY_TYPE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "XEVPRM_ENTITY_TYPE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Xevprm_entity_type_PK
	{
		private Int32? _xevprm_entity_type_PK;
		private String _name;
		private String _table_name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? xevprm_entity_type_PK
		{
			get { return _xevprm_entity_type_PK; }
			set { _xevprm_entity_type_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name
		{
			get { return _name; }
			set { _name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String table_name
		{
			get { return _table_name; }
			set { _table_name = value; }
		}

		#endregion

		public Xevprm_entity_type_PK() { }
		public Xevprm_entity_type_PK(Int32? xevprm_entity_type_PK, String name, String table_name)
		{
			this.xevprm_entity_type_PK = xevprm_entity_type_PK;
			this.name = name;
			this.table_name = table_name;
		}
	}

	public interface IXevprm_entity_type_PKOperations : ICRUDOperations<Xevprm_entity_type_PK>
	{

	}
}
