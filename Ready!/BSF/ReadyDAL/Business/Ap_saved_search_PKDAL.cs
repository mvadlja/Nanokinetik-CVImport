// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	21.2.2012. 14:11:59
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.AP_SAVED_SEARCH
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Ap_saved_search_PKDAL : GEMDataAccess<Ap_saved_search_PK>, IAp_saved_search_PKOperations
	{
		public Ap_saved_search_PKDAL() : base() { }
		public Ap_saved_search_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IAp_saved_search_PKOperations Members
        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_SAVED_SEARCH_GetEntitiesByUserOrPublic", OperationType = GEMOperationType.Select)]
        public List<Ap_saved_search_PK> GetEntitiesByUserOrPublic(Int32? user_fk)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                List<Ap_saved_search_PK> entities = new List<Ap_saved_search_PK>();
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
                return new List<Ap_saved_search_PK>();
            }

        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_SAVED_SEARCH_GetEntitiesByUser", OperationType = GEMOperationType.Select)]
        public List<Ap_saved_search_PK> GetEntitiesByUser(Int32? user_fk)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                List<Ap_saved_search_PK> entities = new List<Ap_saved_search_PK>();
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("@user_fk", user_fk, DbType.Int32, ParameterDirection.Input));

                entities=base.ExecuteProcedureReturnEntities(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);

                return entities;
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                return new List<Ap_saved_search_PK>();
            }

        }


		#endregion

		#region ICRUDOperations<Ap_saved_search_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_SAVED_SEARCH_GetEntity", OperationType = GEMOperationType.Select)]
		public override Ap_saved_search_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_SAVED_SEARCH_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Ap_saved_search_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_SAVED_SEARCH_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Ap_saved_search_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_SAVED_SEARCH_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Ap_saved_search_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_SAVED_SEARCH_Save", OperationType = GEMOperationType.Save)]
		public override Ap_saved_search_PK Save(Ap_saved_search_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_SAVED_SEARCH_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Ap_saved_search_PK> SaveCollection(List<Ap_saved_search_PK> entities)
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
