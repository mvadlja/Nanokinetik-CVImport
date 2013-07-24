// ======================================================================================================================
// Author:		KIKI-PC\Alan
// Create date:	27.6.2012. 12:03:00
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.REMINDER_EMAIL_RECIPIENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "REMINDER_EMAIL_RECIPIENT")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Reminder_email_recipient_PK
	{
		private Int32? _reminder_email_recipient_PK;
		private Int32? _reminder_FK;
		private Int32? _person_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? reminder_email_recipient_PK
		{
			get { return _reminder_email_recipient_PK; }
			set { _reminder_email_recipient_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? reminder_FK
		{
			get { return _reminder_FK; }
			set { _reminder_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? person_FK
		{
			get { return _person_FK; }
			set { _person_FK = value; }
		}

		#endregion

		public Reminder_email_recipient_PK() { }
		public Reminder_email_recipient_PK(Int32? reminder_email_recipient_PK, Int32? reminder_FK, Int32? person_FK)
		{
			this.reminder_email_recipient_PK = reminder_email_recipient_PK;
			this.reminder_FK = reminder_FK;
			this.person_FK = person_FK;
		}
	}

	public interface IReminder_email_recipient_PKOperations : ICRUDOperations<Reminder_email_recipient_PK>
	{
	    List<Reminder_email_recipient_PK> GetEntitiesByReminder(Int32? reminder_FK);
	}
}
