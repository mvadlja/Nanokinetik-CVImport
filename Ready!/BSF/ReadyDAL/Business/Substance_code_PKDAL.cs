// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	6.11.2011. 1:00:07
// Description:	GEM2 Generated class for table SSI.dbo.SUBSTANCE_CODE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Substance_code_PKDAL : GEMDataAccess<Substance_code_PK>, ISubstance_code_PKOperations
	{
		public Substance_code_PKDAL() : base() { }
		public Substance_code_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ISubstance_code_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_SUBSTANCE_CODE_MN_GetSCBySubstancePK", OperationType = GEMOperationType.Select)]
        public List<Substance_code_PK> GetSCBySubstancePK(Int32? SubstancePK)
        {
            DateTime methodStart = DateTime.Now;
            List<Substance_code_PK> entities = new List<Substance_code_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (SubstancePK != null) parameters.Add(new GEMDbParameter("SubstancePK", SubstancePK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Substance_code_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_CODE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Substance_code_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_CODE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Substance_code_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_CODE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Substance_code_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_CODE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Substance_code_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_CODE_Save", OperationType = GEMOperationType.Save)]
		public override Substance_code_PK Save(Substance_code_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_CODE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Substance_code_PK> SaveCollection(List<Substance_code_PK> entities)
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
