// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:28:28
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PRODUCT_ORGANIZATION_MANU_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PRODUCT_ORGANIZATION_MANU_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Product_organization_manu_mn_PK
	{
		private Int32? _product_organization_manu_mn_PK;
		private Int32? _organization_FK;
		private Int32? _product_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? product_organization_manu_mn_PK
		{
			get { return _product_organization_manu_mn_PK; }
			set { _product_organization_manu_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? organization_FK
		{
			get { return _organization_FK; }
			set { _organization_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? product_FK
		{
			get { return _product_FK; }
			set { _product_FK = value; }
		}

		#endregion

		public Product_organization_manu_mn_PK() { }
		public Product_organization_manu_mn_PK(Int32? product_organization_manu_mn_PK, Int32? organization_FK, Int32? product_FK)
		{
			this.product_organization_manu_mn_PK = product_organization_manu_mn_PK;
			this.organization_FK = organization_FK;
			this.product_FK = product_FK;
		}
	}

	public interface IProduct_organization_manu_mn_PKOperations : ICRUDOperations<Product_organization_manu_mn_PK>
	{
        DataSet GetOrgByProduct(Int32? Product_FK);
        void DeleteByProductPK(Int32? Product_FK);
	}
}
