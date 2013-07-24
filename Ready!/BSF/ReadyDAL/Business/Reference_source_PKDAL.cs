// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	8.11.2011. 21:17:21
// Description:	GEM2 Generated class for table SSI.dbo.REFERENCE_SOURCE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Reference_source_PKDAL : GEMDataAccess<Reference_source_PK>, IReference_source_PKOperations
	{
		public Reference_source_PKDAL() : base() { }
		public Reference_source_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IReference_source_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RS_SN_MN_GetRSBySNPK", OperationType = GEMOperationType.Select)]
        public List<Reference_source_PK> GetRSBySNPK(Int32? SNPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Reference_source_PK> entities = new List<Reference_source_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (SNPK != null) parameters.Add(new GEMDbParameter("SNPK", SNPK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RS_SC_MN_GetRSBySCPK", OperationType = GEMOperationType.Select)]
        public List<Reference_source_PK> GetRSBySCPK(Int32? SCPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Reference_source_PK> entities = new List<Reference_source_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (SCPK != null) parameters.Add(new GEMDbParameter("SCPK", SCPK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RS_TRG_MN_GetRSByTRG", OperationType = GEMOperationType.Select)]
        public List<Reference_source_PK> GetRSByTRG(Int32? trg)
        {
            DateTime methodStart = DateTime.Now;
            List<Reference_source_PK> entities = new List<Reference_source_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (trg != null) parameters.Add(new GEMDbParameter("trg", trg, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RS_GE_MN_GetRSBySNPK", OperationType = GEMOperationType.Select)]
        public List<Reference_source_PK> GetRSByGEPK(Int32? GEPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Reference_source_PK> entities = new List<Reference_source_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (GEPK != null) parameters.Add(new GEMDbParameter("GEPK", GEPK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RS_SCLF_MN_GetRSBySCLFPK", OperationType = GEMOperationType.Select)]
        public List<Reference_source_PK> GetRSBySCLFPK(Int32? SCLFPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Reference_source_PK> entities = new List<Reference_source_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (SCLFPK != null) parameters.Add(new GEMDbParameter("SCLFPK", SCLFPK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RS_GENE_MN_GetRSByGenePK", OperationType = GEMOperationType.Select)]
        public List<Reference_source_PK> GetRSByGenePK(Int32? GenePK)
        {
            DateTime methodStart = DateTime.Now;
            List<Reference_source_PK> entities = new List<Reference_source_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (GenePK != null) parameters.Add(new GEMDbParameter("GenePK", GenePK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RS_SR_MN_GetRSByRELPK", OperationType = GEMOperationType.Select)]
        public List<Reference_source_PK> GetRSByRELPK(Int32? RELPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Reference_source_PK> entities = new List<Reference_source_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (RELPK != null) parameters.Add(new GEMDbParameter("RELPK", RELPK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Reference_source_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REFERENCE_SOURCE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Reference_source_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REFERENCE_SOURCE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Reference_source_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REFERENCE_SOURCE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Reference_source_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REFERENCE_SOURCE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Reference_source_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REFERENCE_SOURCE_Save", OperationType = GEMOperationType.Save)]
		public override Reference_source_PK Save(Reference_source_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REFERENCE_SOURCE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Reference_source_PK> SaveCollection(List<Reference_source_PK> entities)
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
