// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	14.9.2012. 10:33:59
// Description:	GEM2 Generated class for table ready_dev.dbo.EMA_SENT_FILE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "ready_dev", Table = "EMA_SENT_FILE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Ema_sent_file_PK
	{
		private Int32? _ema_sent_file_PK;
		private String _file_name;
		private String _file_type;
		private Byte[] _file_data;
		private Int32? _status;
		private DateTime? _sent_time;
		private String _as_to;
		private String _as2_from;
		private String _as2_header;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? ema_sent_file_PK
		{
			get { return _ema_sent_file_PK; }
			set { _ema_sent_file_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String file_name
		{
			get { return _file_name; }
			set { _file_name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String file_type
		{
			get { return _file_type; }
			set { _file_type = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Binary)]
		public Byte[] file_data
		{
			get { return _file_data; }
			set { _file_data = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? status
		{
			get { return _status; }
			set { _status = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? sent_time
		{
			get { return _sent_time; }
			set { _sent_time = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String as_to
		{
			get { return _as_to; }
			set { _as_to = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String as2_from
		{
			get { return _as2_from; }
			set { _as2_from = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String as2_header
		{
			get { return _as2_header; }
			set { _as2_header = value; }
		}

		#endregion

		public Ema_sent_file_PK() { }
		public Ema_sent_file_PK(Int32? ema_sent_file_PK, String file_name, String file_type, Byte[] file_data, Int32? status, DateTime? sent_time, String as_to, String as2_from, String as2_header)
		{
			this.ema_sent_file_PK = ema_sent_file_PK;
			this.file_name = file_name;
			this.file_type = file_type;
			this.file_data = file_data;
			this.status = status;
			this.sent_time = sent_time;
			this.as_to = as_to;
			this.as2_from = as2_from;
			this.as2_header = as2_header;
		}
	}

	public interface IEma_sent_file_PKOperations : ICRUDOperations<Ema_sent_file_PK>
	{

	}
}
