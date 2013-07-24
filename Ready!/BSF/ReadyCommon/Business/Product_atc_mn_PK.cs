// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:45:12
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PRODUCT_ATC_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PRODUCT_ATC_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Product_atc_mn_PK
	{
		private Int32? _product_atc_mn_PK;
		private Int32? _product_FK;
		private Int32? _atc_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? product_atc_mn_PK
		{
			get { return _product_atc_mn_PK; }
			set { _product_atc_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? product_FK
		{
			get { return _product_FK; }
			set { _product_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? atc_FK
		{
			get { return _atc_FK; }
			set { _atc_FK = value; }
		}

		#endregion

		public Product_atc_mn_PK() { }
		public Product_atc_mn_PK(Int32? product_atc_mn_PK, Int32? product_FK, Int32? atc_FK)
		{
			this.product_atc_mn_PK = product_atc_mn_PK;
			this.product_FK = product_FK;
			this.atc_FK = atc_FK;
		}
	}

	public interface IProduct_atc_mn_PKOperations : ICRUDOperations<Product_atc_mn_PK>
	{
        DataSet GetATCByProduct(Int32? Product_FK);
        void DeleteByProductPK(Int32? Product_FK);
	}
}
