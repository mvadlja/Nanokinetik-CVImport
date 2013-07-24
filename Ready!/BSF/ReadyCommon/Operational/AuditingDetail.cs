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
	public class AuditingDetail
	{
		private Int32? _iDAuditingDetail;
		private Int32? _masterID;
		private String _columnName;
		private String _oldValue;
		private String _newValue;
		private String _pKValue;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? IDAuditingDetail
		{
			get { return _iDAuditingDetail; }
			set
			{
				_iDAuditingDetail = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? MasterID
		{
			get { return _masterID; }
			set
			{
				_masterID = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String ColumnName
		{
			get { return _columnName; }
			set
			{
				_columnName = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String OldValue
		{
			get { return _oldValue; }
			set
			{
				_oldValue = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String NewValue
		{
			get { return _newValue; }
			set
			{
				_newValue = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String PKValue
		{
			get { return _pKValue; }
			set
			{
				_pKValue = value;
			}
		}

		#endregion

		public AuditingDetail() { }
		public AuditingDetail(Int32? iDAuditingDetail, Int32? masterID, String columnName, String oldValue, String newValue, String pKValue)
		{
			this.IDAuditingDetail = iDAuditingDetail;
			this.MasterID = masterID;
			this.ColumnName = columnName;
			this.OldValue = oldValue;
			this.NewValue = newValue;
			this.PKValue = pKValue;
		}
	}

	public interface IAuditingDetailOperations : ICRUDOperations<AuditingDetail>
	{
        DataSet GetDistinctColumns(String dBName, String tableName);
        List<AuditingDetail> GetEntriesForMasterAudit(Int32 masterID);
        AuditingDetail GetColumnValue(Int32? detailId);

	    DataSet GetColumnValues(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
	}
}
