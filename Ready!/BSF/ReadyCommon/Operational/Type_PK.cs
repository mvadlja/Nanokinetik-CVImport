// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	17.10.2011. 14:25:55
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.TYPE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "TYPE")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Type_PK
    {
        private Int32? _type_PK;
        private String _name;
        private String _group;
        private String _entity_related;
        private String _form_related;
        private String _type;
        private String _description;
        private String _group_description;
		private Int32? _custom_sort;        
        private String _ev_code;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? type_PK
        {
            get { return _type_PK; }
            set { _type_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String group
        {
            get { return _group; }
            set { _group = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String entity_related
        {
            get { return _entity_related; }
            set { _entity_related = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String form_related
        {
            get { return _form_related; }
            set { _form_related = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String type
        {
            get { return _type; }
            set { _type = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String group_description
        {
            get { return _group_description; }
            set { _group_description = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String ev_code
        {
            get { return _ev_code; }
            set { _ev_code = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? custom_sort
        {
            get { return _custom_sort; }
            set { _custom_sort = value; }
        }

        #endregion

        public Type_PK() { }
        public Type_PK(Int32? type_PK, String name, String group, String entity_related, String form_related, String type, String description, String group_description, String ev_code)
        {
            this.type_PK = type_PK;
            this.name = name;
            this.group = group;
            this.entity_related = entity_related;
            this.form_related = form_related;
            this.type = type;
            this.description = description;
            this.group_description = group_description;
            this.ev_code = ev_code;
        }
    }

	public interface IType_PKOperations : ICRUDOperations<Type_PK>
	{
        List<Type_PK> GetTypesForDDL(String group);
        Type_PK GetEntityByGroup(String group, String name);
        List<Type_PK> GetAvailableTypesForActivity(Int32? activityPk);
        List<Type_PK> GetAssignedTypesForActivity(Int32? activityPk);
        List<Type_PK> GetAvailablePackagingMaterialsForProduct(Int32? productPk);
        List<Type_PK> GetAssignedPackagingMaterialsForProduct(Int32? productPk);

        DataSet GetGroups();

        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
	}
}
