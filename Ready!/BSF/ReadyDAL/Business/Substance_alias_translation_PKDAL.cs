// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	4.1.2012. 12:02:23
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SUBSTANCE_ALIAS_TRANSLATION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Substance_alias_translation_PKDAL : GEMDataAccess<Substance_alias_translation_PK>, ISubstance_alias_translation_PKOperations
	{
		public Substance_alias_translation_PKDAL() : base() { }
		public Substance_alias_translation_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ISubstance_alias_translation_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_ALIASES_TRANSLATION_GetSubAliasTranBySubAlias", OperationType = GEMOperationType.Select)]
        public List<Substance_alias_translation_PK> GetSubAliasTranBySubAlias(int? subAlias_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Substance_alias_translation_PK> entities = new List<Substance_alias_translation_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (subAlias_PK != null) parameters.Add(new GEMDbParameter("alias_PK", subAlias_PK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Substance_alias_translation_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_ALIAS_TRANSLATION_GetEntity", OperationType = GEMOperationType.Select)]
		public override Substance_alias_translation_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_ALIAS_TRANSLATION_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Substance_alias_translation_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_ALIAS_TRANSLATION_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Substance_alias_translation_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_ALIAS_TRANSLATION_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Substance_alias_translation_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_ALIAS_TRANSLATION_Save", OperationType = GEMOperationType.Save)]
		public override Substance_alias_translation_PK Save(Substance_alias_translation_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCE_ALIAS_TRANSLATION_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Substance_alias_translation_PK> SaveCollection(List<Substance_alias_translation_PK> entities)
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
