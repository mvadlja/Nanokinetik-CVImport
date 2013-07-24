// ======================================================================================================================
// Author:		space-monkey\dpetek
// Create date:	4.9.2012. 16:05:09
// Description:	GEM2 Generated class for table ready_dev.dbo.ENTITY_LAST_CHANGE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "ready_dev", Table = "ENTITY_LAST_CHANGE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "ready_poss_wc")]
	public class Last_change_PK
	{
		private Int32? _last_change_PK;
		private String _change_table;
		private DateTime? _change_date;
		private Int32? _user_FK;
		private Int32? _entity_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? last_change_PK
		{
			get { return _last_change_PK; }
			set { _last_change_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String change_table
		{
			get { return _change_table; }
			set { _change_table = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? change_date
		{
			get { return _change_date; }
			set { _change_date = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? user_FK
		{
			get { return _user_FK; }
			set { _user_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? entity_FK
		{
			get { return _entity_FK; }
			set { _entity_FK = value; }
		}

		#endregion

		public Last_change_PK() { }
		public Last_change_PK(Int32? last_change_PK, String change_table, DateTime? change_date, Int32? user_FK, Int32? entity_FK)
		{
			this.last_change_PK = last_change_PK;
			this.change_table = change_table;
			this.change_date = change_date;
			this.user_FK = user_FK;
			this.entity_FK = entity_FK;
		}
	}

	public interface ILast_change_PKOperations : ICRUDOperations<Last_change_PK>
	{
        Last_change_PK GetEntityLastChange(string table_name, int? entity_PK);
	}
}
