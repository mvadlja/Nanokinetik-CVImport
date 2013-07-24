// ======================================================================================================================
// Author:		KRISTIJAN-HPDV7\Kristijan
// Create date:	9.7.2013. 13:46:28
// Description:	GEM2 Generated class for table ReadyDev.dbo.PRODUCT_PACKAGING_MATERIAL_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PRODUCT_PACKAGING_MATERIAL_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Product_packaging_material_mn_PK
	{
		private Int32? _product_packaging_material_mn_PK;
		private Int32? _product_FK;
		private Int32? _type_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? product_packaging_material_mn_PK
		{
			get { return _product_packaging_material_mn_PK; }
			set { _product_packaging_material_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? product_FK
		{
			get { return _product_FK; }
			set { _product_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? type_FK
		{
			get { return _type_FK; }
			set { _type_FK = value; }
		}

		#endregion

		public Product_packaging_material_mn_PK() { }
		public Product_packaging_material_mn_PK(Int32? product_packaging_material_mn_PK, Int32? product_FK, Int32? type_FK)
		{
			this.product_packaging_material_mn_PK = product_packaging_material_mn_PK;
			this.product_FK = product_FK;
			this.type_FK = type_FK;
		}
	}

	public interface IProduct_packaging_material_mn_PKOperations : ICRUDOperations<Product_packaging_material_mn_PK>
	{
        void DeleteByProduct(Int32? productPk);
	}
}
