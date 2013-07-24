// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:35:03
// Description:	GEM2 Generated class for table SSI.dbo.NS_MOIETY_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "NS_MOIETY_MN")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Ns_moiety_mn_PK
	{
		private Int32? _ns_moiety_mn_PK;
		private Int32? _moiety_FK;
		private Int32? _ns_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? ns_moiety_mn_PK
		{
			get { return _ns_moiety_mn_PK; }
			set { _ns_moiety_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? moiety_FK
		{
			get { return _moiety_FK; }
			set { _moiety_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? ns_FK
		{
			get { return _ns_FK; }
			set { _ns_FK = value; }
		}

		#endregion

		public Ns_moiety_mn_PK() { }
		public Ns_moiety_mn_PK(Int32? ns_moiety_mn_PK, Int32? moiety_FK, Int32? ns_FK)
		{
			this.ns_moiety_mn_PK = ns_moiety_mn_PK;
			this.moiety_FK = moiety_FK;
			this.ns_FK = ns_FK;
		}
	}

	public interface INs_moiety_mn_PKOperations : ICRUDOperations<Ns_moiety_mn_PK>
	{

	}
}
