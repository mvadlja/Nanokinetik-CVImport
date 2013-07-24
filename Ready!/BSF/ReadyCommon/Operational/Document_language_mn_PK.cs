// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	28.10.2011. 9:44:53
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.DOCUMENT_LANGUAGE_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "DOCUMENT_LANGUAGE_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Document_language_mn_PK
	{
		private Int32? _document_language_mn_PK;
		private Int32? _document_FK;
		private Int32? _language_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? document_language_mn_PK
		{
			get { return _document_language_mn_PK; }
			set { _document_language_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? document_FK
		{
			get { return _document_FK; }
			set { _document_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? language_FK
		{
			get { return _language_FK; }
			set { _language_FK = value; }
		}

		#endregion

		public Document_language_mn_PK() { }
		public Document_language_mn_PK(Int32? document_language_mn_PK, Int32? document_FK, Int32? language_FK)
		{
			this.document_language_mn_PK = document_language_mn_PK;
			this.document_FK = document_FK;
			this.language_FK = language_FK;
		}
	}

	public interface IDocument_language_mn_PKOperations : ICRUDOperations<Document_language_mn_PK>
	{
        List<Document_language_mn_PK> GetLanguagesByDocument(Int32? document_FK);

	}
}
