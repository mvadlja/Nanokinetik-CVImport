// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:19:15
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PRODUCT_PI_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PRODUCT_PI_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Product_pi_mn_PK
	{
		private Int32? _product_pi_mn_PK;
		private Int32? _product_indications_FK;
		private Int32? _product_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? product_pi_mn_PK
		{
			get { return _product_pi_mn_PK; }
			set { _product_pi_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? product_indications_FK
		{
			get { return _product_indications_FK; }
			set { _product_indications_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? product_FK
		{
			get { return _product_FK; }
			set { _product_FK = value; }
		}

		#endregion

		public Product_pi_mn_PK() { }
		public Product_pi_mn_PK(Int32? product_pi_mn_PK, Int32? product_indications_FK, Int32? product_FK)
		{
			this.product_pi_mn_PK = product_pi_mn_PK;
			this.product_indications_FK = product_indications_FK;
			this.product_FK = product_FK;
		}
	}

	public interface IProduct_pi_mn_PKOperations : ICRUDOperations<Product_pi_mn_PK>
	{
        DataSet GetPIByProduct(Int32? Product_FK);
        DataSet GetPISearcher(String name, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        void DeleteByProductPK(Int32? Product_FK);
    }
}
