using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Runtime.Serialization;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class AuditingMaster
	{
		private Int32? _iDAuditingMaster;
		private String _username;
		private String _dBName;
		private String _tableName;
		private DateTime? _date;
		private String _operation;
		private String _serverName;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? IDAuditingMaster
		{
			get { return _iDAuditingMaster; }
			set
			{
				_iDAuditingMaster = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String Username
		{
			get { return _username; }
			set
			{
				_username = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String DBName
		{
			get { return _dBName; }
			set
			{
				_dBName = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String TableName
		{
			get { return _tableName; }
			set
			{
				_tableName = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? Date
		{
			get { return _date; }
			set
			{
				_date = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String Operation
		{
			get { return _operation; }
			set
			{
				_operation = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String ServerName
		{
			get { return _serverName; }
			set
			{
				_serverName = value;
			}
		}

		#endregion

		public AuditingMaster() { }
		public AuditingMaster(Int32? iDAuditingMaster, String username, String dBName, String tableName, DateTime? date, String operation, String serverName)
		{
			this.IDAuditingMaster = iDAuditingMaster;
			this.Username = username;
			this.DBName = dBName;
			this.TableName = tableName;
			this.Date = date;
			this.Operation = operation;
			this.ServerName = serverName;
		}
	}

	public interface IAuditingMasterOperations : ICRUDOperations<AuditingMaster>
	{
        List<AuditingMaster> SearchAuditingMaster(String dBName, String operation, String serverName, string dateFrom, string dateTill, String tableName, String username, int pageNumber, int pageSize, out int totalRecordsCount);

        DataSet GetDistinctTableName(String dBName);

        DataSet GetDistinctServerName();

        DataSet GetDistinctUserNames();

        DataSet GetDistinctDBNames();

        Int32? GetAuditMasterIDBySessionToken(String session_token);
        DataSet GetRecordVersions(int PKValue, string table_name);
        DataSet GetRecordVersionsAP(int PKValue);

	    DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
	}
}
