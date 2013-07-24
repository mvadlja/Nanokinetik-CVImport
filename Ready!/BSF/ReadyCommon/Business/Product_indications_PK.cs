// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:39:21
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PRODUCT_INDICATION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PRODUCT_INDICATION")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Product_indications_PK
	{
		private Int32? _product_indications_PK;
		private Decimal? _meddraversion;
		private String _meddralevel;
		private Int32? _meddracode;
        private String _name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? product_indications_PK
		{
			get { return _product_indications_PK; }
			set { _product_indications_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Decimal)]
		public Decimal? meddraversion
		{
			get { return _meddraversion; }
			set { _meddraversion = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String meddralevel
		{
			get { return _meddralevel; }
			set { _meddralevel = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? meddracode
		{
			get { return _meddracode; }
			set { _meddracode = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }
		#endregion

		public Product_indications_PK() { }
		public Product_indications_PK(Int32? product_indications_PK, Decimal? meddraversion, String meddralevel, Int32? meddracode, String name)
		{
			this.product_indications_PK = product_indications_PK;
			this.meddraversion = meddraversion;
			this.meddralevel = meddralevel;
			this.meddracode = meddracode;
            this.name = name;
		}
	}

	public interface IProduct_indications_PKOperations : ICRUDOperations<Product_indications_PK>
	{

	}
}
