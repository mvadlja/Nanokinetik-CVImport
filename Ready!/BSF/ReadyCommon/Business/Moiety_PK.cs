// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:46:58
// Description:	GEM2 Generated class for table SSI.dbo.MOIETY
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "MOIETY")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Moiety_PK
	{
		private Int32? _moiety_PK;
		private String _moiety_role;
		private String _moiety_name;
		private String _moiety_id;
		private String _amount_type;
		private Int32? _amount_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? moiety_PK
		{
			get { return _moiety_PK; }
			set { _moiety_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String moiety_role
		{
			get { return _moiety_role; }
			set { _moiety_role = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String moiety_name
		{
			get { return _moiety_name; }
			set { _moiety_name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String moiety_id
		{
			get { return _moiety_id; }
			set { _moiety_id = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String amount_type
		{
			get { return _amount_type; }
			set { _amount_type = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? amount_FK
		{
			get { return _amount_FK; }
			set { _amount_FK = value; }
		}

		#endregion

		public Moiety_PK() { }
		public Moiety_PK(Int32? moiety_PK, String moiety_role, String moiety_name, String moiety_id, String amount_type, Int32? amount_FK)
		{
			this.moiety_PK = moiety_PK;
			this.moiety_role = moiety_role;
			this.moiety_name = moiety_name;
			this.moiety_id = moiety_id;
			this.amount_type = amount_type;
			this.amount_FK = amount_FK;
		}
	}

	public interface IMoiety_PKOperations : ICRUDOperations<Moiety_PK>
	{
        List<Moiety_PK> GetMoietyByNonStoPK(Int32? NonStoPK);

	}
}
