// ======================================================================================================================
// Author:		ANTEC\Kiki
// Create date:	4.12.2011. 21:19:46
// Description:	GEM2 Generated class for table SSI.dbo.SUBSTANCE_NAME
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Substance_name_PKDAL : GEMDataAccess<Substance_name_PK>, ISubstance_name_PKOperations
	{
		public Substance_name_PKDAL() : base() { }
		public Substance_name_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ISubstance_name_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_SUBSTANCE_NAME_MN_GetSNBySubstancePK", OperationType = GEMOperationType.Select)]
        public List<Substance_name_PK> GetSNBySubstancePK(Int32? SubstancePK)
        {
            DateTime methodStart = DateTime.Now;
            List<Substance_name_PK> entities = new List<Substance_name_PK>();

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

		#region ICRUDOperations<Substance_name_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_NAME_GetEntity", OperationType = GEMOperationType.Select)]
		public override Substance_name_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_NAME_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Substance_name_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_NAME_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Substance_name_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_NAME_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Substance_name_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_NAME_Save", OperationType = GEMOperationType.Save)]
		public override Substance_name_PK Save(Substance_name_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_NAME_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Substance_name_PK> SaveCollection(List<Substance_name_PK> entities)
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
