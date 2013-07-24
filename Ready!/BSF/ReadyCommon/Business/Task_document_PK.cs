// ======================================================================================================================
// Author:		Acer\Kiki
// Create date:	30.11.2011. 14:03:31
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.TASK_DOCUMENT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "TASK_DOCUMENT_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Task_document_PK
	{
		private Int32? _task_document_PK;
		private Int32? _task_FK;
		private Int32? _document_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? task_document_PK
		{
			get { return _task_document_PK; }
			set { _task_document_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? task_FK
		{
			get { return _task_FK; }
			set { _task_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? document_FK
		{
			get { return _document_FK; }
			set { _document_FK = value; }
		}

		#endregion

		public Task_document_PK() { }
		public Task_document_PK(Int32? task_document_PK, Int32? task_FK, Int32? document_FK)
		{
			this.task_document_PK = task_document_PK;
			this.task_FK = task_FK;
			this.document_FK = document_FK;
		}
	}

	public interface ITask_document_PKOperations : ICRUDOperations<Task_document_PK>
	{
        //DataSet GetDocumentsByTask(Int32? task_FK);
        DataSet GetTasksByDocument(Int32? document_FK);
        List<Task_document_PK> GetTasksMNByDocument(Int32? document_FK);
        bool AbleToDeleteEntity(int documentPk);
	}
}
