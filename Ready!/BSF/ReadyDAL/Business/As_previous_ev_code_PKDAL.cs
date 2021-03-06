// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	4.1.2012. 12:02:23
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.AS_PREVIOUS_EV_CODE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class As_previous_ev_code_PKDAL : GEMDataAccess<As_previous_ev_code_PK>, IAs_previous_ev_code_PKOperations
	{
		public As_previous_ev_code_PKDAL() : base() { }
		public As_previous_ev_code_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IAs_previous_ev_code_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_TRANSLATION_GetPrevCodeByAS", OperationType = GEMOperationType.Select)]
        public List<As_previous_ev_code_PK> GetPrevEvcodeByAs(int? as_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<As_previous_ev_code_PK> entities = new List<As_previous_ev_code_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (as_PK != null) parameters.Add(new GEMDbParameter("AS_PK", as_PK, DbType.Int32, ParameterDirection.Input));

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
		#endregion

		#region ICRUDOperations<As_previous_ev_code_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AS_PREVIOUS_EV_CODE_GetEntity", OperationType = GEMOperationType.Select)]
		public override As_previous_ev_code_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AS_PREVIOUS_EV_CODE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<As_previous_ev_code_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AS_PREVIOUS_EV_CODE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<As_previous_ev_code_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AS_PREVIOUS_EV_CODE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<As_previous_ev_code_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AS_PREVIOUS_EV_CODE_Save", OperationType = GEMOperationType.Save)]
		public override As_previous_ev_code_PK Save(As_previous_ev_code_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AS_PREVIOUS_EV_CODE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<As_previous_ev_code_PK> SaveCollection(List<As_previous_ev_code_PK> entities)
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
