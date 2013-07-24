// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:10:03
// Description:	GEM2 Generated class for table SSI.dbo.SUBSTANCE_RELATIONSHIP
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Substance_relationship_PKDAL : GEMDataAccess<Substance_relationship_PK>, ISubstance_relationship_PKOperations
	{
		public Substance_relationship_PKDAL() : base() { }
		public Substance_relationship_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ISubstance_relationship_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_RELATIONSHIP_GetRELByRIPK", OperationType = GEMOperationType.Select)]
        public List<Substance_relationship_PK> GetRELByRIPK(Int32? RIPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Substance_relationship_PK> entities = new List<Substance_relationship_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (RIPK != null) parameters.Add(new GEMDbParameter("RIPK", RIPK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Substance_relationship_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_RELATIONSHIP_GetEntity", OperationType = GEMOperationType.Select)]
		public override Substance_relationship_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_RELATIONSHIP_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Substance_relationship_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_RELATIONSHIP_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Substance_relationship_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_RELATIONSHIP_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Substance_relationship_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_RELATIONSHIP_Save", OperationType = GEMOperationType.Save)]
		public override Substance_relationship_PK Save(Substance_relationship_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_RELATIONSHIP_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Substance_relationship_PK> SaveCollection(List<Substance_relationship_PK> entities)
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
