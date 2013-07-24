// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	17.10.2011. 16:25:20
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ORGANIZATION_IN_ROLE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Organization_in_role_DAL : GEMDataAccess<Organization_in_role_>, IOrganization_in_role_Operations
	{
		public Organization_in_role_DAL() : base() { }
		public Organization_in_role_DAL(string dataSourceId) : base(dataSourceId) { }

		#region IOrganization_in_role_Operations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_IN_ROLE_GetOrganisationInRolesByOrganisationPK", OperationType = GEMOperationType.Select)]
        public List<Organization_in_role_> GetOrganisationInRolesByOrganisationPK(Int32? organization_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Organization_in_role_> entities = new List<Organization_in_role_>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (organization_FK != null) parameters.Add(new GEMDbParameter("organization_FK", organization_FK, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_IN_ROLE_GetOrganizationsByRole", OperationType = GEMOperationType.Select)]
        public DataSet GetOrganizationsByRole(String role_name)
        {
            DateTime methodStart = DateTime.Now;
            DataSet entities = new DataSet();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (role_name != null) parameters.Add(new GEMDbParameter("role_name", role_name, DbType.String, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnDataSet(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_IN_ROLE_GetOrganizationsByRoleSearcher", OperationType = GEMOperationType.Select)]
        public DataSet GetOrganizationsByRoleSearcher(String role_name, String name_org, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;
            totalRecordsCount = 0;
            string orderBy = null;

            try
            {
                // Generating order by clause
                orderBy = base.CreateOrderByClause(orderByConditions);

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();
               
                if (role_name != null && role_name.ToString() != String.Empty) parameters.Add(new GEMDbParameter("role_name", role_name, DbType.String, ParameterDirection.Input));
                if (name_org != null && name_org.ToString() != String.Empty) parameters.Add(new GEMDbParameter("name_org", name_org, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("OrderByQuery", orderBy, DbType.String, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);
                totalRecordsCount = (int)outputValues["totalRecordsCount"];

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_IN_ROLE_DeleteByOrganization", OperationType = GEMOperationType.Select)]
        public void DeleteByOrganization(int organizationPk)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("OrganizationPk", organizationPk, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

		#endregion

		#region ICRUDOperations<Organization_in_role_> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_IN_ROLE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Organization_in_role_ GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_IN_ROLE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Organization_in_role_> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_IN_ROLE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Organization_in_role_> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_IN_ROLE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Organization_in_role_> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_IN_ROLE_Save", OperationType = GEMOperationType.Save)]
		public override Organization_in_role_ Save(Organization_in_role_ entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_IN_ROLE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Organization_in_role_> SaveCollection(List<Organization_in_role_> entities)
		{
			return base.SaveCollection(entities);
		}

		public override void DeleteCollection<PKType>(List<PKType> entityPKs)
		{
			base.DeleteCollection<PKType>(entityPKs);
		}

		#endregion
	}
}
