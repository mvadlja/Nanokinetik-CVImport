// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	4.11.2011. 13:24:50
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SSI_CONTROLED_VOCABULARY
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Ssi__cont_voc_PKDAL : GEMDataAccess<Ssi__cont_voc_PK>, ISsi__cont_voc_PKOperations
	{
		public Ssi__cont_voc_PKDAL() : base() { }
		public Ssi__cont_voc_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ISsi__cont_voc_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ON_DOMAIN_ON_MN_GetDomainByONPK", OperationType = GEMOperationType.Select)]
        public List<Ssi__cont_voc_PK> GetDomainByONPK(Int32? ONPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Ssi__cont_voc_PK> entities = new List<Ssi__cont_voc_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (ONPK != null) parameters.Add(new GEMDbParameter("ONPK", ONPK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ON_ONJ_MN_GetJurByONPK", OperationType = GEMOperationType.Select)]
        public List<Ssi__cont_voc_PK> GetJurByONPK(Int32? ONPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Ssi__cont_voc_PK> entities = new List<Ssi__cont_voc_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (ONPK != null) parameters.Add(new GEMDbParameter("ONPK", ONPK, DbType.Int32, ParameterDirection.Input));

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


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SSI_CONTROLED_VOCABULARY_GetConcentrationTypes", OperationType = GEMOperationType.Select)]
        public List<Ssi__cont_voc_PK> GetConcentrationTypes()
        {
            DateTime methodStart = DateTime.Now;
            List<Ssi__cont_voc_PK> entities = new List<Ssi__cont_voc_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>(); 

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);

                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SSI_CONTROLED_VOCABULARY_GetPrefixes", OperationType = GEMOperationType.Select)]
        public List<Ssi__cont_voc_PK> GetPrefixes()
        {
            DateTime methodStart = DateTime.Now;
            List<Ssi__cont_voc_PK> entities = new List<Ssi__cont_voc_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);

                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SSI_CONTROLED_VOCABULARY_GetPrefixesSubstanceClass", OperationType = GEMOperationType.Select)]
        public List<Ssi__cont_voc_PK> GetPrefixesSubstanceClass()
        {
            DateTime methodStart = DateTime.Now;
            List<Ssi__cont_voc_PK> entities = new List<Ssi__cont_voc_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);

                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SSI_CONTROLED_VOCABULARY_GetEntitiesByListName", OperationType = GEMOperationType.Select)]
        public List<Ssi__cont_voc_PK> GetEntitiesByListName(String list_name)
        {
            DateTime methodStart = DateTime.Now;
            List<Ssi__cont_voc_PK> entities = new List<Ssi__cont_voc_PK>();

            try
            {
                // Input parameters validation
                if (list_name == null) return entities;

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("list_name", list_name, DbType.String, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);

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

		#region ICRUDOperations<Ssi__cont_voc_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SSI_CONTROLED_VOCABULARY_GetEntity", OperationType = GEMOperationType.Select)]
		public override Ssi__cont_voc_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SSI_CONTROLED_VOCABULARY_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Ssi__cont_voc_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SSI_CONTROLED_VOCABULARY_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Ssi__cont_voc_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SSI_CONTROLED_VOCABULARY_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Ssi__cont_voc_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SSI_CONTROLED_VOCABULARY_Save", OperationType = GEMOperationType.Save)]
		public override Ssi__cont_voc_PK Save(Ssi__cont_voc_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SSI_CONTROLED_VOCABULARY_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Ssi__cont_voc_PK> SaveCollection(List<Ssi__cont_voc_PK> entities)
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
