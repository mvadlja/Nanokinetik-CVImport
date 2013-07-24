// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	3.11.2011. 13:14:35
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_EXCIPIENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Excipient_PKDAL : GEMDataAccess<Excipient_PK>, IExcipient_PKOperations
	{
		public Excipient_PKDAL() : base() { }
		public Excipient_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IExcipient_PKOperations Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_EXCIPIENT_GetExcipientsByPPPK", OperationType = GEMOperationType.Select)]
        public List<Excipient_PK> GetExcipientsByPPPK(Int32? pharmaceutical_product_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Excipient_PK> entities = new List<Excipient_PK>();

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_EXCIPIENT_DeleteNULLByUserID", OperationType = GEMOperationType.Delete)]
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

		#region ICRUDOperations<Excipient_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_EXCIPIENT_GetEntity", OperationType = GEMOperationType.Select)]
		public override Excipient_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_EXCIPIENT_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Excipient_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_EXCIPIENT_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Excipient_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_EXCIPIENT_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Excipient_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_EXCIPIENT_Save", OperationType = GEMOperationType.Save)]
		public override Excipient_PK Save(Excipient_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_EXCIPIENT_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Excipient_PK> SaveCollection(List<Excipient_PK> entities)
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
