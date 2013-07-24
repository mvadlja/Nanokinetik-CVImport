// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:46:58
// Description:	GEM2 Generated class for table SSI.dbo.MOIETY
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Moiety_PKDAL : GEMDataAccess<Moiety_PK>, IMoiety_PKOperations
	{
		public Moiety_PKDAL() : base() { }
		public Moiety_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IMoiety_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_NS_MOIETY_MN_GetMoietyByNonStoPK", OperationType = GEMOperationType.Select)]
        public List<Moiety_PK> GetMoietyByNonStoPK(Int32? NonStoPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Moiety_PK> entities = new List<Moiety_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (NonStoPK != null) parameters.Add(new GEMDbParameter("NonStoPK", NonStoPK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Moiety_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MOIETY_GetEntity", OperationType = GEMOperationType.Select)]
		public override Moiety_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MOIETY_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Moiety_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MOIETY_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Moiety_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MOIETY_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Moiety_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MOIETY_Save", OperationType = GEMOperationType.Save)]
		public override Moiety_PK Save(Moiety_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MOIETY_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Moiety_PK> SaveCollection(List<Moiety_PK> entities)
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
