// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	3.11.2011. 12:24:55
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_ADJUVANT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Adjuvant_PKDAL : GEMDataAccess<Adjuvant_PK>, IAdjuvant_PKOperations
	{
		public Adjuvant_PKDAL() : base() { }
		public Adjuvant_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IAdjuvant_PKOperations Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADJUVANT_GetAdjuvantsByPPPK", OperationType = GEMOperationType.Select)]
        public List<Adjuvant_PK> GetAdjuvantsByPPPK(Int32? pharmaceutical_product_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Adjuvant_PK> entities = new List<Adjuvant_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (pharmaceutical_product_FK != null) parameters.Add(new GEMDbParameter("pharmaceutical_product_FK", pharmaceutical_product_FK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADJUVANT_DeleteNULLByUserID", OperationType = GEMOperationType.Delete)]
        public void DeleteNULLByUserID(Int32? userID)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (userID != null && userID.ToString() != String.Empty) parameters.Add(new GEMDbParameter("userID", userID, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

		#endregion

		#region ICRUDOperations<Adjuvant_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADJUVANT_GetEntity", OperationType = GEMOperationType.Select)]
		public override Adjuvant_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADJUVANT_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Adjuvant_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADJUVANT_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Adjuvant_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADJUVANT_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Adjuvant_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADJUVANT_Save", OperationType = GEMOperationType.Save)]
		public override Adjuvant_PK Save(Adjuvant_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADJUVANT_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Adjuvant_PK> SaveCollection(List<Adjuvant_PK> entities)
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
