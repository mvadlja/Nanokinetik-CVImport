// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	19.10.2011. 16:52:47
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.AP_DOCUMENT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "AP_DOCUMENT_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Ap_document_mn_PK
	{
		private Int32? _ap_document_mn_PK;
		private Int32? _document_FK;
		private Int32? _ap_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? ap_document_mn_PK
		{
			get { return _ap_document_mn_PK; }
			set { _ap_document_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? document_FK
		{
			get { return _document_FK; }
			set { _document_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? ap_FK
		{
			get { return _ap_FK; }
			set { _ap_FK = value; }
		}

		#endregion

		public Ap_document_mn_PK() { }
		public Ap_document_mn_PK(Int32? ap_document_mn_PK, Int32? document_FK, Int32? ap_FK)
		{
			this.ap_document_mn_PK = ap_document_mn_PK;
			this.document_FK = document_FK;
			this.ap_FK = ap_FK;
		}
	}

	public interface IAp_document_mn_PKOperations : ICRUDOperations<Ap_document_mn_PK>
	{
        DataSet GetAttachmentsByAP(Int32? ap_FK);
        DataSet GetDocumentsByAP(Int32? ap_FK);
        DataSet GetDocumentsByAPWP(Int32 ap_FK, int pageNumber, int pageSize, out int totalRecordsCount);
        List<Ap_document_mn_PK> GetAuthorizedProductsByDocumentFK(Int32? document_FK);
        bool AbleToDeleteEntity(int documentPk);
        void DeleteByAuthorisedProduct(Int32? authorisedProductPk);
	}
}
