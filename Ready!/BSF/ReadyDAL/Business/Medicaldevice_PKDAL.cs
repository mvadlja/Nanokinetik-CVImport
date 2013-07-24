// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	28.10.2011. 12:44:12
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_MEDICAL_DEVICE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Medicaldevice_PKDAL : GEMDataAccess<Medicaldevice_PK>, IMedicaldevice_PKOperations
	{
		public Medicaldevice_PKDAL() : base() { }
		public Medicaldevice_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IMedicaldevice_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_PP_MEDICAL_DEVICE_GetMedDevicesByCodeSearcher]", OperationType = GEMOperationType.Select)]
        public DataSet GetMedDevicesByCodeSearcher(String code, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;
            totalRecordsCount = 0;
            string orderBy = null;

            try
            {
                // Generating order by clause
                orderBy = base.CreateOrderByClause(orderByConditions);

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (code != null && code.ToString() != String.Empty) parameters.Add(new GEMDbParameter("code", code, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("OrderByQuery", orderBy, DbType.String, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);
                totalRecordsCount = (int)outputValues["totalRecordsCount"];

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_MEDICAL_DEVICE_GetEntitiesByPharmaceuticalProduct", OperationType = GEMOperationType.Select)]
        public List<Medicaldevice_PK> GetEntitiesByPharmaceuticalProduct(int pharmaceuticalProductPk)
        {
            var methodStart = DateTime.Now;
            var entities = new List<Medicaldevice_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("PharmaceuticalProductPk", pharmaceuticalProductPk, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Medicaldevice_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_MEDICAL_DEVICE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Medicaldevice_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_MEDICAL_DEVICE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Medicaldevice_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_MEDICAL_DEVICE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Medicaldevice_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_MEDICAL_DEVICE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Medicaldevice_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_MEDICAL_DEVICE_Save", OperationType = GEMOperationType.Save)]
		public override Medicaldevice_PK Save(Medicaldevice_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_MEDICAL_DEVICE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Medicaldevice_PK> SaveCollection(List<Medicaldevice_PK> entities)
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
