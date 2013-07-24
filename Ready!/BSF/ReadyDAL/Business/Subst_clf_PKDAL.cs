// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:09:27
// Description:	GEM2 Generated class for table SSI.dbo.SUBSTANCE_CLASSIFICATION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Subst_clf_PKDAL : GEMDataAccess<Subst_clf_PK>, ISubst_clf_PKOperations
	{
		public Subst_clf_PKDAL() : base() { }
		public Subst_clf_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ISubst_clf_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_CLASSIFICATION_GetSCLFByRIPK", OperationType = GEMOperationType.Select)]
        public List<Subst_clf_PK> GetSCLFByRIPK(Int32? RIPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Subst_clf_PK> entities = new List<Subst_clf_PK>();

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


		#region ICRUDOperations<Subst_clf_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_CLASSIFICATION_GetEntity", OperationType = GEMOperationType.Select)]
		public override Subst_clf_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_CLASSIFICATION_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Subst_clf_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_CLASSIFICATION_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Subst_clf_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_CLASSIFICATION_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Subst_clf_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_CLASSIFICATION_Save", OperationType = GEMOperationType.Save)]
		public override Subst_clf_PK Save(Subst_clf_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_CLASSIFICATION_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Subst_clf_PK> SaveCollection(List<Subst_clf_PK> entities)
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
