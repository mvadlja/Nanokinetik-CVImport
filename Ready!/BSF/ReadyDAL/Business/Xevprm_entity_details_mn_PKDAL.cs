// ======================================================================================================================
// Author:		Kiki-PC\Kiki
// Create date:	8/21/2012 2:16:17 PM
// Description:	GEM2 Generated class for table ready_billev_production.dbo.XEVPRM_ENTITY_DETAILS_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Xevprm_entity_details_mn_PKDAL : GEMDataAccess<Xevprm_entity_details_mn_PK>, IXevprm_entity_details_mn_PKOperations
	{
		public Xevprm_entity_details_mn_PKDAL() : base() { }
		public Xevprm_entity_details_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IXevprm_entity_details_mn_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_ENTITY_DETAILS_MN_GetEntitiesByXevprm", OperationType = GEMOperationType.Select)]
        public List<Xevprm_entity_details_mn_PK> GetEntitiesByXevprm(int? xevprm_message_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Xevprm_entity_details_mn_PK> entites = new List<Xevprm_entity_details_mn_PK>();

            try
            {
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (xevprm_message_PK.HasValue) parameters.Add(new GEMDbParameter("xevprm_message_PK", xevprm_message_PK, DbType.Int32, ParameterDirection.Input));
                
                entites = base.ExecuteProcedureReturnEntities(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entites;
        }

		#endregion

		#region ICRUDOperations<Xevprm_entity_details_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_ENTITY_DETAILS_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Xevprm_entity_details_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_ENTITY_DETAILS_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Xevprm_entity_details_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_ENTITY_DETAILS_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Xevprm_entity_details_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_ENTITY_DETAILS_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Xevprm_entity_details_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_ENTITY_DETAILS_MN_Save", OperationType = GEMOperationType.Save)]
		public override Xevprm_entity_details_mn_PK Save(Xevprm_entity_details_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_ENTITY_DETAILS_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Xevprm_entity_details_mn_PK> SaveCollection(List<Xevprm_entity_details_mn_PK> entities)
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
