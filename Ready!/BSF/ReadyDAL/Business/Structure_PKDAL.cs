// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:09:06
// Description:	GEM2 Generated class for table SSI.dbo.STRUCTURE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Structure_PKDAL : GEMDataAccess<Structure_PK>, IStructure_PKOperations
	{
		public Structure_PKDAL() : base() { }
		public Structure_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IStructure_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SING_STRUCTURE_MN_GetStructBySingPK", OperationType = GEMOperationType.Select)]
        public List<Structure_PK> GetStructBySingPK(Int32? SingPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Structure_PK> entities = new List<Structure_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (SingPK != null) parameters.Add(new GEMDbParameter("SingPK", SingPK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Structure_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCTURE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Structure_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCTURE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Structure_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCTURE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Structure_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCTURE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Structure_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCTURE_Save", OperationType = GEMOperationType.Save)]
		public override Structure_PK Save(Structure_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCTURE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Structure_PK> SaveCollection(List<Structure_PK> entities)
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
