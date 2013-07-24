// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	11.11.2011. 10:14:51
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ACTIVITY_PRODUCT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ACTIVITY_PRODUCT_MN", Active=false)]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Activity_product_PK
	{
		private Int32? _activity_product_PK;
		private Int32? _activity_FK;
		private Int32? _product_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? activity_product_PK
		{
			get { return _activity_product_PK; }
			set { _activity_product_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? activity_FK
		{
			get { return _activity_FK; }
			set { _activity_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? product_FK
		{
			get { return _product_FK; }
			set { _product_FK = value; }
		}

		#endregion

		public Activity_product_PK() { }
		public Activity_product_PK(Int32? activity_product_PK, Int32? activity_FK, Int32? product_FK)
		{
			this.activity_product_PK = activity_product_PK;
			this.activity_FK = activity_FK;
			this.product_FK = product_FK;
		}
	}

	public interface IActivity_product_PKOperations : ICRUDOperations<Activity_product_PK>
	{
        DataSet GetProductsByActivity(Int32? activity_PK);
        void DeleteByActivityPK(Int32? activity_PK);
        List<Activity_product_PK> GetProductsByActivityList(Int32? activity_PK);
        void StartSessionActivity(int? activity_PK);
        void EndSessionActivity(int? activity_PK);
	}
}
