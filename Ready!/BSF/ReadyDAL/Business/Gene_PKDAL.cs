// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:46:04
// Description:	GEM2 Generated class for table SSI.dbo.GENE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Gene_PKDAL : GEMDataAccess<Gene_PK>, IGene_PKOperations
	{
		public Gene_PKDAL() : base() { }
		public Gene_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IGene_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_GENE_GetGeneByRIPK", OperationType = GEMOperationType.Select)]
        public List<Gene_PK> GetGeneByRIPK(Int32? RIPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Gene_PK> entities = new List<Gene_PK>();

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

		#region ICRUDOperations<Gene_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_GENE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Gene_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_GENE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Gene_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_GENE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Gene_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_GENE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Gene_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_GENE_Save", OperationType = GEMOperationType.Save)]
		public override Gene_PK Save(Gene_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_GENE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Gene_PK> SaveCollection(List<Gene_PK> entities)
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
