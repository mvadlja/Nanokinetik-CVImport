// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	17.10.2011. 16:25:20
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ORG_IN_TYPE_FOR_PARTNER
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Org_in_type_for_partner_DAL : GEMDataAccess<Org_in_type_for_partner>, IOrg_in_type_for_partnerOperations
	{
		public Org_in_type_for_partner_DAL() : base() { }
		public Org_in_type_for_partner_DAL(string dataSourceId) : base(dataSourceId) { }

		#region IOrg_in_type_for_partner_Operations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_PARTNER_DeleteByProduct", OperationType = GEMOperationType.Select)]
        public void DeleteByProduct(int productPk)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ProductPk", productPk, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_PARTNER_GetEntitiesByProduct", OperationType = GEMOperationType.Select)]
        public List<Org_in_type_for_partner> GetEntitiesByProduct(int productPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Org_in_type_for_partner>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ProductPk", productPk, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_PARTNER_GetOrganisationPartnerTypesByOrganisationPK", OperationType = GEMOperationType.Select)]
        public List<Org_in_type_for_partner> GetOrganisationPartnerTypesByOrganisationPK(Int32? organization_FK, Int32? product_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Org_in_type_for_partner> entities = new List<Org_in_type_for_partner>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (organization_FK != null) parameters.Add(new GEMDbParameter("organization_FK", organization_FK, DbType.Int32, ParameterDirection.Input));
                if (product_FK != null) parameters.Add(new GEMDbParameter("product_FK", product_FK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_PARTNER_GetOrganizationsByPartnerRoleSearcher", OperationType = GEMOperationType.Select)]
        public DataSet GetOrganizationsByPartnerRoleSearcher(String role_name, String name_org, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
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


		#endregion

		#region ICRUDOperations<Org_in_type_for_partner_> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_PARTNER_GetEntity", OperationType = GEMOperationType.Select)]
		public override Org_in_type_for_partner GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_PARTNER_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Org_in_type_for_partner> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_PARTNER_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Org_in_type_for_partner> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_PARTNER_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Org_in_type_for_partner> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_PARTNER_Save", OperationType = GEMOperationType.Save)]
		public override Org_in_type_for_partner Save(Org_in_type_for_partner entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_PARTNER_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Org_in_type_for_partner> SaveCollection(List<Org_in_type_for_partner> entities)
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
