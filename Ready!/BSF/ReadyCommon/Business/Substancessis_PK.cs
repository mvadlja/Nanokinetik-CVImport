// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	4.1.2012. 12:02:23
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SUBSTANCESSI
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "SUBSTANCESSI")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Substancessis_PK
	{
		private Int32? _substancessis_PK;
		private Boolean? _valid_according_ssi;
		private Int32? _ssi_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? substancessis_PK
		{
			get { return _substancessis_PK; }
			set { _substancessis_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
		public Boolean? valid_according_ssi
		{
			get { return _valid_according_ssi; }
			set { _valid_according_ssi = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? ssi_FK
		{
			get { return _ssi_FK; }
			set { _ssi_FK = value; }
		}

		#endregion

		public Substancessis_PK() { }
		public Substancessis_PK(Int32? substancessis_PK, Boolean? valid_according_ssi, Int32? ssi_FK)
		{
			this.substancessis_PK = substancessis_PK;
			this.valid_according_ssi = valid_according_ssi;
			this.ssi_FK = ssi_FK;
		}
	}

	public interface ISubstancessis_PKOperations : ICRUDOperations<Substancessis_PK>
	{

	}
}
