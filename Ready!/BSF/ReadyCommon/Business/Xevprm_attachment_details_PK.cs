// ======================================================================================================================
// Author:		Kiki-PC\Kiki
// Create date:	8/21/2012 2:17:18 PM
// Description:	GEM2 Generated class for table ready_billev_production.dbo.XEVPRM_ATTACHMENT_DETAILS
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "XEVPRM_ATTACHMENT_DETAILS")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Xevprm_attachment_details_PK
	{
		private Int32? _xevprm_attachment_details_PK;
		private Int32? _attachment_FK;
		private String _file_name;
		private String _file_type;
		private String _attachment_name;
		private String _attachment_type;
		private String _language_code;
		private String _attachment_version;
		private DateTime? _attachment_version_date;
		private Int32? _operation_type;
		private String _ev_code;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? xevprm_attachment_details_PK
		{
			get { return _xevprm_attachment_details_PK; }
			set { _xevprm_attachment_details_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? attachment_FK
		{
			get { return _attachment_FK; }
			set { _attachment_FK = value; }
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

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String attachment_name
		{
			get { return _attachment_name; }
			set { _attachment_name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String attachment_type
		{
			get { return _attachment_type; }
			set { _attachment_type = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String language_code
		{
			get { return _language_code; }
			set { _language_code = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String attachment_version
		{
			get { return _attachment_version; }
			set { _attachment_version = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? attachment_version_date
		{
			get { return _attachment_version_date; }
			set { _attachment_version_date = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? operation_type
		{
			get { return _operation_type; }
			set { _operation_type = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String ev_code
		{
			get { return _ev_code; }
			set { _ev_code = value; }
		}

        public XevprmOperationType OperationType
        {
            get { return operation_type != null && Enum.IsDefined(typeof(XevprmOperationType), operation_type) ? (XevprmOperationType)operation_type : XevprmOperationType.NULL; }
            set { operation_type = (int)value == 0 ? (int?)null : (int)value; }
        }
		#endregion

		public Xevprm_attachment_details_PK() { }
		public Xevprm_attachment_details_PK(Int32? xevprm_attachment_details_PK, Int32? attachment_FK, String file_name, String file_type, String attachment_name, String attachment_type, String language_code, String attachment_version, DateTime? attachment_version_date, Int32? operation_type, String ev_code)
		{
			this.xevprm_attachment_details_PK = xevprm_attachment_details_PK;
			this.attachment_FK = attachment_FK;
			this.file_name = file_name;
			this.file_type = file_type;
			this.attachment_name = attachment_name;
			this.attachment_type = attachment_type;
			this.language_code = language_code;
			this.attachment_version = attachment_version;
			this.attachment_version_date = attachment_version_date;
			this.operation_type = operation_type;
			this.ev_code = ev_code;
		}
	}

	public interface IXevprm_attachment_details_PKOperations : ICRUDOperations<Xevprm_attachment_details_PK>
	{
        Xevprm_attachment_details_PK GetEntityForXevprm(int? xevprm_message_PK);
	}
}
