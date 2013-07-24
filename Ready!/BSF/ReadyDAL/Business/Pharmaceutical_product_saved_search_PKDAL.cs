// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	24.2.2012. 13:16:59
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PHARMACEUTICAL_PRODUCT_SAVED_SEARCH
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Pharmaceutical_product_saved_search_PKDAL : GEMDataAccess<Pharmaceutical_product_saved_search_PK>, IPharmaceutical_product_saved_search_PKOperations
	{
		public Pharmaceutical_product_saved_search_PKDAL() : base() { }
		public Pharmaceutical_product_saved_search_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IPharmaceutical_products_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PHARMACEUTICAL_PRODUCT_SAVED_SEARCH_GetEntitiesByUserOrPublic", OperationType = GEMOperationType.Select)]
        public List<Pharmaceutical_product_saved_search_PK> GetEntitiesByUserOrPublic(Int32? user_fk)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                List<Pharmaceutical_product_saved_search_PK> entities = new List<Pharmaceutical_product_saved_search_PK>();
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("@user_fk", user_fk, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);

                return entities;
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                return new List<Pharmaceutical_product_saved_search_PK>();
            }

        }

		#endregion

		#region ICRUDOperations<Pharmaceutical_products_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PHARMACEUTICAL_PRODUCT_SAVED_SEARCH_GetEntity", OperationType = GEMOperationType.Select)]
		public override Pharmaceutical_product_saved_search_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PHARMACEUTICAL_PRODUCT_SAVED_SEARCH_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Pharmaceutical_product_saved_search_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PHARMACEUTICAL_PRODUCT_SAVED_SEARCH_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Pharmaceutical_product_saved_search_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PHARMACEUTICAL_PRODUCT_SAVED_SEARCH_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Pharmaceutical_product_saved_search_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PHARMACEUTICAL_PRODUCT_SAVED_SEARCH_Save", OperationType = GEMOperationType.Save)]
		public override Pharmaceutical_product_saved_search_PK Save(Pharmaceutical_product_saved_search_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PHARMACEUTICAL_PRODUCT_SAVED_SEARCH_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Pharmaceutical_product_saved_search_PK> SaveCollection(List<Pharmaceutical_product_saved_search_PK> entities)
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
