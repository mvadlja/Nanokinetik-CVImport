// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:37:19
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PRODUCT_COUNTRY_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PRODUCT_COUNTRY_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Product_country_mn_PK
	{
		private Int32? _product_country_mn_PK;
		private Int32? _country_FK;
		private Int32? _product_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? product_country_mn_PK
		{
			get { return _product_country_mn_PK; }
			set { _product_country_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? country_FK
		{
			get { return _country_FK; }
			set { _country_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? product_FK
		{
			get { return _product_FK; }
			set { _product_FK = value; }
		}

		#endregion

		public Product_country_mn_PK() { }
		public Product_country_mn_PK(Int32? product_country_mn_PK, Int32? country_FK, Int32? product_FK)
		{
			this.product_country_mn_PK = product_country_mn_PK;
			this.country_FK = country_FK;
			this.product_FK = product_FK;
		}
	}

	public interface IProduct_country_mn_PKOperations : ICRUDOperations<Product_country_mn_PK>
	{
        DataSet GetCountriesByProduct(Int32? Product_FK);
        List<Product_country_mn_PK> GetCountriesListByProduct(Int32? Product_FK);
        void DeleteByProductPK(Int32? Product_FK);
	}
}
