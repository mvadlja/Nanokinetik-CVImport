// ======================================================================================================================
// Author:		POSSIMUSIT-MATE\Mateo
// Create date:	23.3.2012. 8:41:10
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_DOCUMENT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PP_DOCUMENT_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Pp_document_PK
	{
		private Int32? _pp_document_PK;
		private Int32? _pp_FK;
		private Int32? _doc_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? pp_document_PK
		{
			get { return _pp_document_PK; }
			set { _pp_document_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? pp_FK
		{
			get { return _pp_FK; }
			set { _pp_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? doc_FK
		{
			get { return _doc_FK; }
			set { _doc_FK = value; }
		}

		#endregion

		public Pp_document_PK() { }
		public Pp_document_PK(Int32? pp_document_PK, Int32? pp_FK, Int32? doc_FK)
		{
			this.pp_document_PK = pp_document_PK;
			this.pp_FK = pp_FK;
			this.doc_FK = doc_FK;
		}
	}

	public interface IPp_document_PKOperations : ICRUDOperations<Pp_document_PK>
	{
        List<Pp_document_PK> GetPProductsByDocumentFK(Int32? document_FK);
        bool AbleToDeleteEntity(int documentPk);
    }
}
