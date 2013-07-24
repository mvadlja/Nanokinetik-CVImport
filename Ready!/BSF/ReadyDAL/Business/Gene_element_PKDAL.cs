// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:46:20
// Description:	GEM2 Generated class for table SSI.dbo.GENE_ELEMENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Gene_element_PKDAL : GEMDataAccess<Gene_element_PK>, IGene_element_PKOperations
	{
		public Gene_element_PKDAL() : base() { }
		public Gene_element_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IGene_element_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_GENE_ELEMENT_GetGEByRIPK", OperationType = GEMOperationType.Select)]
        public List<Gene_element_PK> GetGEByRIPK(Int32? RIPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Gene_element_PK> entities = new List<Gene_element_PK>();

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

		#region ICRUDOperations<Gene_element_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_GENE_ELEMENT_GetEntity", OperationType = GEMOperationType.Select)]
		public override Gene_element_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_GENE_ELEMENT_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Gene_element_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_GENE_ELEMENT_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Gene_element_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_GENE_ELEMENT_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Gene_element_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_GENE_ELEMENT_Save", OperationType = GEMOperationType.Save)]
		public override Gene_element_PK Save(Gene_element_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_GENE_ELEMENT_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Gene_element_PK> SaveCollection(List<Gene_element_PK> entities)
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
