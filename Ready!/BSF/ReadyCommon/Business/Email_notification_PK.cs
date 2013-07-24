// ======================================================================================================================
// Author:		Kiki-PC\Kiki
// Create date:	8/29/2012 11:16:13 AM
// Description:	GEM2 Generated class for table ready_dev.dbo.EMAIL_NOTIFICATION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "EMAIL_NOTIFICATION")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Email_notification_PK
	{
		private Int32? _email_notification_PK;
		private Int32? _notification_type_FK;
		private String _email;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? email_notification_PK
		{
			get { return _email_notification_PK; }
			set { _email_notification_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? notification_type_FK
		{
			get { return _notification_type_FK; }
			set { _notification_type_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String email
		{
			get { return _email; }
			set { _email = value; }
		}

		#endregion

		public Email_notification_PK() { }
		public Email_notification_PK(Int32? email_notification_PK, Int32? notification_type_FK, String email)
		{
			this.email_notification_PK = email_notification_PK;
			this.notification_type_FK = notification_type_FK;
			this.email = email;
		}
	}

	public interface IEmail_notification_PKOperations : ICRUDOperations<Email_notification_PK>
	{
        List<Email_notification_PK> GetEntitiesByNotificationType(NotificationType notificationType);
	}
}
