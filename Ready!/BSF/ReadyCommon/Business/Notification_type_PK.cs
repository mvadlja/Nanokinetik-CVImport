// ======================================================================================================================
// Author:		Kiki-PC\Kiki
// Create date:	8/29/2012 11:17:56 AM
// Description:	GEM2 Generated class for table ready_dev.dbo.NOTIFICATION_TYPE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "NOTIFICATION_TYPE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Notification_type_PK
	{
		private Int32? _notification_type_PK;
		private String _name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? notification_type_PK
		{
			get { return _notification_type_PK; }
			set { _notification_type_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name
		{
			get { return _name; }
			set { _name = value; }
		}

		#endregion

		public Notification_type_PK() { }
		public Notification_type_PK(Int32? notification_type_PK, String name)
		{
			this.notification_type_PK = notification_type_PK;
			this.name = name;
		}
	}

	public interface INotification_type_PKOperations : ICRUDOperations<Notification_type_PK>
	{

	}

    public enum NotificationType
    {
        NULL,
        AS2ServiceStartStop
    }
}
