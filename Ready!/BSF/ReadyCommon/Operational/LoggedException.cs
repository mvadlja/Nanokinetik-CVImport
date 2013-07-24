using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Runtime.Serialization;
using GEM2Common;

namespace Kmis.Model
{
	[Serializable()]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class LoggedException
	{
		private Int32? _iDLoggedException;
		private String _username;
		private String _exceptionType;
		private String _exceptionMessage;
		private String _targetSite;
		private String _stackTrace;
		private String _source;
		private DateTime? _date;
		private String _serverName;
		private Guid? _uniqueKey;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? IDLoggedException
		{
			get { return _iDLoggedException; }
			set
			{
				_iDLoggedException = value;
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
		public String ExceptionType
		{
			get { return _exceptionType; }
			set
			{
				_exceptionType = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String ExceptionMessage
		{
			get { return _exceptionMessage; }
			set
			{
				_exceptionMessage = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String TargetSite
		{
			get { return _targetSite; }
			set
			{
				_targetSite = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String StackTrace
		{
			get { return _stackTrace; }
			set
			{
				_stackTrace = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String Source
		{
			get { return _source; }
			set
			{
				_source = value;
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
		public String ServerName
		{
			get { return _serverName; }
			set
			{
				_serverName = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Guid)]
		public Guid? UniqueKey
		{
			get { return _uniqueKey; }
			set
			{
				_uniqueKey = value;
			}
		}

		#endregion

		public LoggedException() { }
		public LoggedException(Int32? iDLoggedException, String username, String exceptionType, String exceptionMessage, String targetSite, String stackTrace, String source, DateTime? date, String serverName, Guid? uniqueKey)
		{
			this.IDLoggedException = iDLoggedException;
			this.Username = username;
			this.ExceptionType = exceptionType;
			this.ExceptionMessage = exceptionMessage;
			this.TargetSite = targetSite;
			this.StackTrace = stackTrace;
			this.Source = source;
			this.Date = date;
			this.ServerName = serverName;
			this.UniqueKey = uniqueKey;
		}
	}

	public interface ILoggedExceptionOperations : ICRUDOperations<LoggedException>
	{
        DataSet SearchEntries(String username, String exceptionType, String dateFrom, String dateTill, String serverName, String uniqueKey, int pageNumber, int pageSize, out int totalRecordsCount);

        DataSet GetDistinctExceptionTypes();
	}
}
