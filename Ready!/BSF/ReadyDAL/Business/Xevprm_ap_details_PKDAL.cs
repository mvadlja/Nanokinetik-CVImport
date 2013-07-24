// ======================================================================================================================
// Author:		Kiki-PC\Kiki
// Create date:	8/21/2012 2:25:00 PM
// Description:	GEM2 Generated class for table ready_billev_production.dbo.XEVPRM_AP_DETAILS
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Xevprm_ap_details_PKDAL : GEMDataAccess<Xevprm_ap_details_PK>, IXevprm_ap_details_PKOperations
	{
		public Xevprm_ap_details_PKDAL() : base() { }
		public Xevprm_ap_details_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IXevprm_ap_details_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_AP_DETAILS_GetEntitiesByXevprm", OperationType = GEMOperationType.Select)]
        public List<Xevprm_ap_details_PK> GetEntitiesByXevprm(int? xevprm_message_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Xevprm_ap_details_PK> result = new List<Xevprm_ap_details_PK>();

            try
            {
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (xevprm_message_PK != null) parameters.Add(new GEMDbParameter("xevprm_message_PK", xevprm_message_PK, DbType.Int32, ParameterDirection.Input));

                result = base.ExecuteProcedureReturnEntities(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return result;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_AP_DETAILS_GetEntityForXevprm", OperationType = GEMOperationType.Select)]
        public Xevprm_ap_details_PK GetEntityForXevprm(int? xevprm_message_PK)
        {
            DateTime methodStart = DateTime.Now;
            Xevprm_ap_details_PK result = null;

            try
            {
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (xevprm_message_PK != null) parameters.Add(new GEMDbParameter("xevprm_message_PK", xevprm_message_PK, DbType.Int32, ParameterDirection.Input));
                
                result = base.ExecuteProcedureReturnEntity(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return result;
        }

		#endregion

		#region ICRUDOperations<Xevprm_ap_details_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_AP_DETAILS_GetEntity", OperationType = GEMOperationType.Select)]
		public override Xevprm_ap_details_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_AP_DETAILS_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Xevprm_ap_details_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_AP_DETAILS_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Xevprm_ap_details_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_AP_DETAILS_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Xevprm_ap_details_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_AP_DETAILS_Save", OperationType = GEMOperationType.Save)]
		public override Xevprm_ap_details_PK Save(Xevprm_ap_details_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_AP_DETAILS_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Xevprm_ap_details_PK> SaveCollection(List<Xevprm_ap_details_PK> entities)
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
