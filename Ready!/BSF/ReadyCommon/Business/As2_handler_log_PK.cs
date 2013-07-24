// ======================================================================================================================
// Author:		Kiki-PC\Kiki
// Create date:	8/7/2012 9:46:44 AM
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.AS2_HANDLER_LOG
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "AS2_HANDLER_LOG")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class As2_handler_log_PK
	{
		private Int32? _as2_handler_log_PK;
		private DateTime? _log_time;
		private String _event_type;
		private String _description;
		private DateTime? _received_time;
		private String _message_number;
		private String _as2_to;
		private String _as2_from;
		private String _message_ID;
		private String _filename;
		private Int32? _received_message_FK;
		private String _connection;
		private String _date;
		private String _content_length;
		private String _content_type;
		private String _from;
		private String _host;
		private String _user_agent;
		private String _mime_version;
		private String _content_transfer_encoding;
		private String _content_disposition;
		private String _disposition_notification_to;
		private String _disposition_notification_options;
		private String _receipt_delivery_option;
		private String _ediint_features;
		private String _as2_version;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? as2_handler_log_PK
		{
			get { return _as2_handler_log_PK; }
			set { _as2_handler_log_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? log_time
		{
			get { return _log_time; }
			set { _log_time = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String event_type
		{
			get { return _event_type; }
			set { _event_type = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String description
		{
			get { return _description; }
			set { _description = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? received_time
		{
			get { return _received_time; }
			set { _received_time = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String message_number
		{
			get { return _message_number; }
			set { _message_number = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String as2_to
		{
			get { return _as2_to; }
			set { _as2_to = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String as2_from
		{
			get { return _as2_from; }
			set { _as2_from = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String message_ID
		{
			get { return _message_ID; }
			set { _message_ID = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String filename
		{
			get { return _filename; }
			set { _filename = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? received_message_FK
		{
			get { return _received_message_FK; }
			set { _received_message_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String connection
		{
			get { return _connection; }
			set { _connection = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String date
		{
			get { return _date; }
			set { _date = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String content_length
		{
			get { return _content_length; }
			set { _content_length = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String content_type
		{
			get { return _content_type; }
			set { _content_type = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String from
		{
			get { return _from; }
			set { _from = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String host
		{
			get { return _host; }
			set { _host = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String user_agent
		{
			get { return _user_agent; }
			set { _user_agent = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String mime_version
		{
			get { return _mime_version; }
			set { _mime_version = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String content_transfer_encoding
		{
			get { return _content_transfer_encoding; }
			set { _content_transfer_encoding = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String content_disposition
		{
			get { return _content_disposition; }
			set { _content_disposition = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String disposition_notification_to
		{
			get { return _disposition_notification_to; }
			set { _disposition_notification_to = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String disposition_notification_options
		{
			get { return _disposition_notification_options; }
			set { _disposition_notification_options = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String receipt_delivery_option
		{
			get { return _receipt_delivery_option; }
			set { _receipt_delivery_option = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String ediint_features
		{
			get { return _ediint_features; }
			set { _ediint_features = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String as2_version
		{
			get { return _as2_version; }
			set { _as2_version = value; }
		}

		#endregion

		public As2_handler_log_PK() { }
		public As2_handler_log_PK(Int32? as2_handler_log_PK, DateTime? log_time, String event_type, String description, DateTime? received_time, String message_number, String as2_to, String as2_from, String message_ID, String filename, Int32? received_message_FK, String connection, String date, String content_length, String content_type, String from, String host, String user_agent, String mime_version, String content_transfer_encoding, String content_disposition, String disposition_notification_to, String disposition_notification_options, String receipt_delivery_option, String ediint_features, String as2_version)
		{
			this.as2_handler_log_PK = as2_handler_log_PK;
			this.log_time = log_time;
			this.event_type = event_type;
			this.description = description;
			this.received_time = received_time;
			this.message_number = message_number;
			this.as2_to = as2_to;
			this.as2_from = as2_from;
			this.message_ID = message_ID;
			this.filename = filename;
			this.received_message_FK = received_message_FK;
			this.connection = connection;
			this.date = date;
			this.content_length = content_length;
			this.content_type = content_type;
			this.from = from;
			this.host = host;
			this.user_agent = user_agent;
			this.mime_version = mime_version;
			this.content_transfer_encoding = content_transfer_encoding;
			this.content_disposition = content_disposition;
			this.disposition_notification_to = disposition_notification_to;
			this.disposition_notification_options = disposition_notification_options;
			this.receipt_delivery_option = receipt_delivery_option;
			this.ediint_features = ediint_features;
			this.as2_version = as2_version;
		}
	}

	public interface IAs2_handler_log_PKOperations : ICRUDOperations<As2_handler_log_PK>
	{
        DataSet GetReportDataSet(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
	}
}
