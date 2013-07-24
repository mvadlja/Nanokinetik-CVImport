// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	17.10.2011. 16:09:48
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PERSON_IN_ROLE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PERSON_IN_ROLE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Person_in_role_PK
	{
		private Int32? _person_in_role_PK;
		private Int32? _person_FK;
		private Int32? _person_role_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? person_in_role_PK
		{
			get { return _person_in_role_PK; }
			set { _person_in_role_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? person_FK
		{
			get { return _person_FK; }
			set { _person_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? person_role_FK
		{
			get { return _person_role_FK; }
			set { _person_role_FK = value; }
		}

		#endregion

		public Person_in_role_PK() { }
		public Person_in_role_PK(Int32? person_in_role_PK, Int32? person_FK, Int32? person_role_FK)
		{
			this.person_in_role_PK = person_in_role_PK;
			this.person_FK = person_FK;
			this.person_role_FK = person_role_FK;
		}
	}

	public interface IPerson_in_role_PKOperations : ICRUDOperations<Person_in_role_PK>
	{
        List<Person_in_role_PK> GetPersonRolePKsByPersonPK(Int32? person_FK);
        DataSet GetPersonsByRoleSearcher(String person_role, String name, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
	    void DeleteByPerson(int? personPk);
	}
}
