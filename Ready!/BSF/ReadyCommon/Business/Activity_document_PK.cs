// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	11.11.2011. 10:14:29
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ACTIVITY_DOCUMENT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ACTIVITY_DOCUMENT_MN", Active=false)]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Activity_document_PK
	{
		private Int32? _activity_document_PK;
		private Int32? _activity_FK;
		private Int32? _document_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? activity_document_PK
		{
			get { return _activity_document_PK; }
			set { _activity_document_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? activity_FK
		{
			get { return _activity_FK; }
			set { _activity_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? document_FK
		{
			get { return _document_FK; }
			set { _document_FK = value; }
		}

		#endregion

		public Activity_document_PK() { }
		public Activity_document_PK(Int32? activity_document_PK, Int32? activity_FK, Int32? document_FK)
		{
			this.activity_document_PK = activity_document_PK;
			this.activity_FK = activity_FK;
			this.document_FK = document_FK;
		}
	}

	public interface IActivity_document_PKOperations : ICRUDOperations<Activity_document_PK>
	{
        DataSet GetDocumentsByActivity(Int32? activity_FK);
        DataSet GetActivitiesByDocument(Int32? document_FK);
        List<Activity_document_PK> GetActivitiesMNByDocument(Int32? document_FK);
        bool AbleToDeleteEntity(int documentPk);
	}
}
