// ======================================================================================================================
// Author:		space-monkey\dpetek
// Create date:	14.3.2012. 9:34:32
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.MEDDRA
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Meddra_pkDAL : GEMDataAccess<Meddra_pk>, IMeddra_pkOperations
	{
		public Meddra_pkDAL() : base() { }
		public Meddra_pkDAL(string dataSourceId) : base(dataSourceId) { }

		#region IMeddra_pkOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MEDDRA_GetMeddraByAp", OperationType = GEMOperationType.Select)]
        public List<Meddra_pk> GetMeddraByAp(int? authorisedProductFk)
        {
            var methodStart = DateTime.Now;
            var entities = new List<Meddra_pk>();

            try
            {
                var parameters = new List<GEMDbParameter>();
                if (authorisedProductFk.HasValue) parameters.Add(new GEMDbParameter("authorisedProductFk", authorisedProductFk, DbType.Int32, ParameterDirection.Input));

                entities = ExecuteProcedureReturnEntities(parameters);

                // Logging operation if applicable
                LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return entities;
        }

		#endregion

		#region ICRUDOperations<Meddra_pk> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MEDDRA_GetEntity", OperationType = GEMOperationType.Select)]
		public override Meddra_pk GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MEDDRA_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Meddra_pk> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MEDDRA_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Meddra_pk> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MEDDRA_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Meddra_pk> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MEDDRA_Save", OperationType = GEMOperationType.Save)]
		public override Meddra_pk Save(Meddra_pk entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MEDDRA_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Meddra_pk> SaveCollection(List<Meddra_pk> entities)
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
