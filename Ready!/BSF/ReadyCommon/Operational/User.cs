// ======================================================================================================================
// Author:		Home-Laptop\Admin
// Create date:	27.2.2011. 22:41:02
// Description:	GEM2 Generated class for table Kmis.dbo.Users
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    // TODO: Srediti kada se prebaci na sve na Ready_poss_wc [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "USER")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class USER
    {
        private Int32? _user_PK;
        private Int32? _person_FK;
        private String _username;
        private String _password;
        private DateTime? _user_start_date;
        private DateTime? _user_end_date;
        private Int32? _country_FK;
        private String _description;
        private String _email;
        private Boolean? _active;
        private Boolean? _is_ad_user;
        private Int32? _ad_domain;



        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = false, ParameterType = DbType.Int32)]
        public Int32? Person_FK
        {
            get { return _person_FK; }
            set { _person_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? User_PK
        {
            get { return _user_PK; }
            set { _user_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String Username
        {
            get { return _username; }
            set { _username = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? User_start_date
        {
            get { return _user_start_date; }
            set { _user_start_date = value; }

        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? User_end_date
        {
            get { return _user_end_date; }
            set { _user_end_date = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? Country_FK
        {
            get { return _country_FK; }
            set { _country_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String Email
        {
            get { return _email; }
            set { _email = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? Active
        {
            get { return _active; }
            set { _active = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? IsAdUser
        {
            get { return _is_ad_user; }
            set { _is_ad_user = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public Int32? AdDomain
        {
            get { return _ad_domain; }
            set { _ad_domain = value; }
        }

        #endregion

        public USER() { }
        public USER(Int32? user_PK, Int32? person_FK, String username, String password, DateTime? user_start_date, DateTime? user_end_date, Int32? country_FK, String description, String email, Boolean? active, Boolean? is_ad, Int32? ad_domain)
        {
            this.User_PK = user_PK;
            this.Person_FK = person_FK;
            this.Username = username;
            this.Password = password;
            this.User_start_date = user_start_date;
            this.User_end_date = user_end_date;
            this.Country_FK = country_FK;
            this.Description = description;
            this.Email = email;
            this.Active = active;
            this.IsAdUser = is_ad;
            this.AdDomain = ad_domain;
        }
    }

    public interface IUSEROperations : ICRUDOperations<USER>
    {
        USER AuthenticateUser(String userName, String password);
        USER GetUserByUsername(String userName);
        List<USER> SearchUsers(String userName, Int32? countryID, Int32? roleID, int pageNumber, int pageSize, out int totalRecordsCount);
        USER GetUserByEmail(String email);
        USER GetUserByPersonID(int? person_PK);
        DataSet GetUsersDataSet(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetUserPermissions(string username);
    }
}
