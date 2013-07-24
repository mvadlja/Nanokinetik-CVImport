// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:47:05
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PRODUCT_PP_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PRODUCT_PP_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Product_mn_PK
	{
		private Int32? _product_mn_PK;
		private Int32? _product_FK;
		private Int32? _pp_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? product_mn_PK
		{
			get { return _product_mn_PK; }
			set { _product_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? product_FK
		{
			get { return _product_FK; }
			set { _product_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? pp_FK
		{
			get { return _pp_FK; }
			set { _pp_FK = value; }
		}

		#endregion

		public Product_mn_PK() { }
		public Product_mn_PK(Int32? product_mn_PK, Int32? product_FK, Int32? pp_FK)
		{
			this.product_mn_PK = product_mn_PK;
			this.product_FK = product_FK;
			this.pp_FK = pp_FK;
		}
	}

	public interface IProduct_mn_PKOperations : ICRUDOperations<Product_mn_PK>
	{
        DataSet GetPPByProduct(Int32? Product_FK);
        void DeleteByProductPK(Int32? Product_FK);

        void DeleteByPharmaceuticalProduct(int pharmaceuticalProductPk);
        
        List<Product_mn_PK> GetProductsByPPFK(Int32? pp_FK);

        DataSet GetProductsByPP(Int32? pp_FK);
    }
}
