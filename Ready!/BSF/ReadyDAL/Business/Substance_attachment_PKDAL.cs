// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	4.1.2012. 12:02:23
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SUBSTANCE_ATTACHMENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Substance_attachment_PKDAL : GEMDataAccess<Substance_attachment_PK>, ISubstance_attachment_PKOperations
	{
		public Substance_attachment_PKDAL() : base() { }
		public Substance_attachment_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ISubstance_attachment_PKOperations Members
        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_Attachments_GetSubAttByAS", OperationType = GEMOperationType.Select)]
        public List<Substance_attachment_PK> GetSubAttByAs(int? as_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Substance_attachment_PK> entities = new List<Substance_attachment_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (as_PK != null) parameters.Add(new GEMDbParameter("AS_PK", as_PK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Substance_attachment_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_ATTACHMENT_GetEntity", OperationType = GEMOperationType.Select)]
		public override Substance_attachment_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_ATTACHMENT_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Substance_attachment_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_ATTACHMENT_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Substance_attachment_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_ATTACHMENT_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Substance_attachment_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_ATTACHMENT_Save", OperationType = GEMOperationType.Save)]
		public override Substance_attachment_PK Save(Substance_attachment_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_ATTACHMENT_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Substance_attachment_PK> SaveCollection(List<Substance_attachment_PK> entities)
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
