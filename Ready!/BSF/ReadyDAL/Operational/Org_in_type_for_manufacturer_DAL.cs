// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	19.10.2011. 12:00:31
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ORG_IN_TYPE_FOR_MANUFACTURER
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Org_in_type_for_manufacturer_DAL : GEMDataAccess<Org_in_type_for_manufacturer>, IOrg_in_type_for_manufacturerOperations
	{
		public Org_in_type_for_manufacturer_DAL() : base() { }
		public Org_in_type_for_manufacturer_DAL(string dataSourceId) : base(dataSourceId) { }

		#region IOrg_in_type_for_manufacturer_Operations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_MANUFACTURER_DeleteByProduct", OperationType = GEMOperationType.Select)]
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_MANUFACTURER_GetEntitiesByProduct", OperationType = GEMOperationType.Select)]
        public List<Org_in_type_for_manufacturer> GetEntitiesByProduct(int productPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Org_in_type_for_manufacturer>();

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_MANUFACTURER_GetOrganisationManufacturerTypesByOrganisationPK", OperationType = GEMOperationType.Select)]
        public List<Org_in_type_for_manufacturer> GetOrganisationManufacturerTypesByOrganisationPK(Int32? organization_FK, Int32? product_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Org_in_type_for_manufacturer> entities = new List<Org_in_type_for_manufacturer>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("organization_FK", organization_FK, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("product_FK", product_FK, DbType.Int32, ParameterDirection.Input));

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

		#endregion

		#region ICRUDOperations<Org_in_type_for_manufacturer_> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_MANUFACTURER_GetEntity", OperationType = GEMOperationType.Select)]
		public override Org_in_type_for_manufacturer GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_MANUFACTURER_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Org_in_type_for_manufacturer> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_MANUFACTURER_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Org_in_type_for_manufacturer> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_MANUFACTURER_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Org_in_type_for_manufacturer> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_MANUFACTURER_Save", OperationType = GEMOperationType.Save)]
		public override Org_in_type_for_manufacturer Save(Org_in_type_for_manufacturer entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORG_IN_TYPE_FOR_MANUFACTURER_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Org_in_type_for_manufacturer> SaveCollection(List<Org_in_type_for_manufacturer> entities)
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
