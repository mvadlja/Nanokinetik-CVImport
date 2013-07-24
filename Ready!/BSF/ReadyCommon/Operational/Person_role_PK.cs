// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	17.10.2011. 15:38:28
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PERSON_ROLE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PERSON_ROLE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Person_role_PK
	{
		private Int32? _person_role_PK;
		private String _person_name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? person_role_PK
		{
			get { return _person_role_PK; }
			set { _person_role_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String person_name
		{
			get { return _person_name; }
			set { _person_name = value; }
		}

		#endregion

		public Person_role_PK() { }
		public Person_role_PK(Int32? person_role_PK, String person_name)
		{
			this.person_role_PK = person_role_PK;
			this.person_name = person_name;
		}
	}

	public interface IPerson_role_PKOperations : ICRUDOperations<Person_role_PK>
	{
        List<Person_role_PK> GetAssignedEntitiesByPerson(int? personPk);
        List<Person_role_PK> GetAvailableEntitiesByPerson(int? personPk);
	}
}
