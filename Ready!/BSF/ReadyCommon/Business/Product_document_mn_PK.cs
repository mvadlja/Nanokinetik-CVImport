// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:34:57
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PRODUCT_DOCUMENT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PRODUCT_DOCUMENT_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Product_document_mn_PK
	{
		private Int32? _product_document_mn_PK;
		private Int32? _product_FK;
		private Int32? _document_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? product_document_mn_PK
		{
			get { return _product_document_mn_PK; }
			set { _product_document_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? product_FK
		{
			get { return _product_FK; }
			set { _product_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? document_FK
		{
			get { return _document_FK; }
			set { _document_FK = value; }
		}

		#endregion

		public Product_document_mn_PK() { }
		public Product_document_mn_PK(Int32? product_document_mn_PK, Int32? product_FK, Int32? document_FK)
		{
			this.product_document_mn_PK = product_document_mn_PK;
			this.product_FK = product_FK;
			this.document_FK = document_FK;
		}
	}

	public interface IProduct_document_mn_PKOperations : ICRUDOperations<Product_document_mn_PK>
	{
        DataSet GetDocumentsByProduct(Int32? Product_FK);
        List<Product_document_mn_PK> GetProductsByDocumentFK(Int32? document_FK);
        bool AbleToDeleteEntity(int documentPk);
	}
}
