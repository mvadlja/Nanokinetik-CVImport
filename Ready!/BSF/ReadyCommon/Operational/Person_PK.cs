// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	17.10.2011. 10:57:22
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PERSON
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PERSON")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Person_PK
	{
		private Int32? _person_PK;
		private Int32? _country_FK;
		private String _name;
		private String _familyname;
		private String _ip;
		private String _phone;
		private String _address;
		private String _city;
		private String _email;
		private Int32? _status;
		private String _local_number;
		private String _ev_code;
		private String _givenname;
		private String _title;
		private String _company;
		private String _department;
		private String _building;
		private String _street;
		private String _state;
		private String _postcode;
		private String _countrycode;
		private String _tel_countrycode;
		private String _telnumber;
		private String _telextn;
		private String _cell_countrycode;
		private String _cellnumber;
		private String _fax_countrycode;
		private String _faxnumber;
		private String _faxextn;
        private String _telnum24h;
        private String _fullName;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? person_PK
		{
			get { return _person_PK; }
			set { _person_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? country_FK
		{
			get { return _country_FK; }
			set { _country_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name
		{
			get { return _name; }
			set { _name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String familyname
		{
			get { return _familyname; }
			set { _familyname = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String ip
		{
			get { return _ip; }
			set { _ip = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String phone
		{
			get { return _phone; }
			set { _phone = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String address
		{
			get { return _address; }
			set { _address = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String city
		{
			get { return _city; }
			set { _city = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String email
		{
			get { return _email; }
			set { _email = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? status
		{
			get { return _status; }
			set { _status = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String local_number
		{
			get { return _local_number; }
			set { _local_number = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String ev_code
		{
			get { return _ev_code; }
			set { _ev_code = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String givenname
		{
			get { return _givenname; }
			set { _givenname = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String title
		{
			get { return _title; }
			set { _title = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String company
		{
			get { return _company; }
			set { _company = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String department
		{
			get { return _department; }
			set { _department = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String building
		{
			get { return _building; }
			set { _building = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String street
		{
			get { return _street; }
			set { _street = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String state
		{
			get { return _state; }
			set { _state = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String postcode
		{
			get { return _postcode; }
			set { _postcode = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String countrycode
		{
			get { return _countrycode; }
			set { _countrycode = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String tel_countrycode
		{
			get { return _tel_countrycode; }
			set { _tel_countrycode = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String telnumber
		{
			get { return _telnumber; }
			set { _telnumber = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String telextn
		{
			get { return _telextn; }
			set { _telextn = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String cell_countrycode
		{
			get { return _cell_countrycode; }
			set { _cell_countrycode = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String cellnumber
		{
			get { return _cellnumber; }
			set { _cellnumber = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String fax_countrycode
		{
			get { return _fax_countrycode; }
			set { _fax_countrycode = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String faxnumber
		{
			get { return _faxnumber; }
			set { _faxnumber = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String faxextn
		{
			get { return _faxextn; }
			set { _faxextn = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String telnum24h
        {
            get { return _telnum24h; }
            set { _telnum24h = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
	    public string FullName
	    {
	        get { return _fullName; }
	        set { _fullName = value; }
	    }

	    #endregion

		public Person_PK() { }
		public Person_PK(Int32? person_PK, Int32? country_FK, String name, String familyname, String ip, String phone, String address, String city, String email, Int32? status, String local_number, String ev_code, String givenname, String title, String company, String department, String building, String street, String state, String postcode, String countrycode, String tel_countrycode, String telnumber, String telextn, String cell_countrycode, String cellnumber, String fax_countrycode, String faxnumber, String faxextn, String telnum24h, String fullName)
		{
			this.person_PK = person_PK;
			this.country_FK = country_FK;
			this.name = name;
			this.familyname = familyname;
			this.ip = ip;
			this.phone = phone;
			this.address = address;
			this.city = city;
			this.email = email;
			this.status = status;
			this.local_number = local_number;
			this.ev_code = ev_code;
			this.givenname = givenname;
			this.title = title;
			this.company = company;
			this.department = department;
			this.building = building;
			this.street = street;
			this.state = state;
			this.postcode = postcode;
			this.countrycode = countrycode;
			this.tel_countrycode = tel_countrycode;
			this.telnumber = telnumber;
			this.telextn = telextn;
			this.cell_countrycode = cell_countrycode;
			this.cellnumber = cellnumber;
			this.fax_countrycode = fax_countrycode;
			this.faxnumber = faxnumber;
			this.faxextn = faxextn;
            this.telnum24h = telnum24h;
            this.FullName = fullName;
		}
	}

	public interface IPerson_PKOperations : ICRUDOperations<Person_PK>
	{
        List<Person_PK> GetPersonsByRole(string roleName);
        DataSet GetPersonDataSet(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        Person_PK GetPersonByUserID(int? user_PK);
        DataSet GetPersonsSearcher(string name, string email, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        List<Person_PK> GetAllEntities();
        List<Person_PK> GetEntitiesByRoleName(string roleName);
        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        List<Person_PK> GetEntitiesWithUser();
        List<Person_PK> GetAvailableEntitiesByUserRole(int userRolePk);
        List<Person_PK> GetAssignedEntitiesByUserRole(int userRolePk);
	}
}
