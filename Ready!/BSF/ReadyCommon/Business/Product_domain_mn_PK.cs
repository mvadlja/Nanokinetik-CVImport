// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:41:09
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PRODUCT_DOMAIN_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PRODUCT_DOMAIN_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Product_domain_mn_PK
	{
		private Int32? _product_domain_mn_PK;
		private Int32? _product_FK;
		private Int32? _domain_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? product_domain_mn_PK
		{
			get { return _product_domain_mn_PK; }
			set { _product_domain_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? product_FK
		{
			get { return _product_FK; }
			set { _product_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? domain_FK
		{
			get { return _domain_FK; }
			set { _domain_FK = value; }
		}

		#endregion

		public Product_domain_mn_PK() { }
		public Product_domain_mn_PK(Int32? product_domain_mn_PK, Int32? product_FK, Int32? domain_FK)
		{
			this.product_domain_mn_PK = product_domain_mn_PK;
			this.product_FK = product_FK;
			this.domain_FK = domain_FK;
		}
	}

	public interface IProduct_domain_mn_PKOperations : ICRUDOperations<Product_domain_mn_PK>
	{
        DataSet GetDomainByProduct(Int32? Product_FK);
        void DeleteByProductPK(Int32? Product_FK);
	}
}