// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	19.12.2011. 14:44:41
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PRODUCT_SUB_UNIT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PRODUCT_SUB_UNIT_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Product_submission_unit_PK
	{
		private Int32? _product_submission_unit_PK;
		private Int32? _product_FK;
		private Int32? _submission_unit_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? product_submission_unit_PK
		{
			get { return _product_submission_unit_PK; }
			set { _product_submission_unit_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? product_FK
		{
			get { return _product_FK; }
			set { _product_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? submission_unit_FK
		{
			get { return _submission_unit_FK; }
			set { _submission_unit_FK = value; }
		}

		#endregion

		public Product_submission_unit_PK() { }
		public Product_submission_unit_PK(Int32? product_submission_unit_PK, Int32? product_FK, Int32? submission_unit_FK)
		{
			this.product_submission_unit_PK = product_submission_unit_PK;
			this.product_FK = product_FK;
			this.submission_unit_FK = submission_unit_FK;
		}
	}

	public interface IProduct_submission_unit_PKOperations : ICRUDOperations<Product_submission_unit_PK>
	{
        DataSet GetSUByProduct(Int32? product_FK);
        List<Product_submission_unit_PK> GetProductsBySU(Int32? submission_unit_FK);
        void DeleteBySubmissionUnitPK(Int32? submissionUnitPk);
	}
}
