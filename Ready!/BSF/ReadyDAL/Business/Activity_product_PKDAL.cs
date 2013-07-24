// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	11.11.2011. 10:14:51
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ACTIVITY_PRODUCT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Activity_product_PKDAL : GEMDataAccess<Activity_product_PK>, IActivity_product_PKOperations
	{
        private List<Activity_product_PK> oldValues;
        private List<Activity_product_PK> newValues;

        IXevprm_message_PKOperations _xevprm_message_PKOperations = new Xevprm_message_PKDAL();
        IAuditingDetailOperations _auditing_detal_PKOperations = new AuditingDetailDAL();
        IActivity_PKOperations _activity_PKOperatoins = new Activity_PKDAL();
        IProduct_PKOperations _product_PKOperations = new Product_PKDAL();

		public Activity_product_PKDAL() : base() { }
		public Activity_product_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IActivity_product_PKOperations Members

        public void StartSessionActivity(int? activity_PK) {
            if (oldValues == null) oldValues = new List<Activity_product_PK>();
            if (newValues == null) newValues = new List<Activity_product_PK>();
            oldValues.Clear();
            newValues.Clear();

            oldValues = GetEntities().FindAll(item => item.activity_FK == activity_PK);
            newValues = GetEntities().FindAll(item => item.activity_FK == activity_PK);
        }

        public void EndSessionActivity(int? activity_PK) {
            oldValues.Sort(delegate(Activity_product_PK ap1, Activity_product_PK ap2)
            {
                if (ap1.product_FK < ap2.product_FK) return -1;
                if (ap1.product_FK > ap2.product_FK) return 1;
                return 0;
            });
            newValues.Sort(delegate(Activity_product_PK ap1, Activity_product_PK ap2)
            {
                if (ap1.product_FK < ap2.product_FK) return -1;
                if (ap1.product_FK > ap2.product_FK) return 1;
                return 0;
            });

            if (oldValues != newValues) {
                string complexOldValue = "";
                string complexNewValue = "";

                foreach (Activity_product_PK oldValue in oldValues) {
                    if (complexOldValue != "") complexOldValue += "|||";
                    complexOldValue += _product_PKOperations.GetEntity(oldValue.product_FK).name;
                }

                foreach (Activity_product_PK newValue in newValues) {
                    if (complexNewValue != "") complexNewValue += "|||";
                    complexNewValue += _product_PKOperations.GetEntity(newValue.product_FK).name;
                }
                //if (complexOldValue != complexNewValue) {

                //    Activity_PK entity = _activity_PKOperatoins.GetEntity(activity_PK);
                //    Int32 master_PK = _xevprm_message_PKOperations.GetAuditMasterIDBySessionToken((string)System.Web.HttpContext.Current.Session["AUDIT_TRAIL_TOKEN"]);
                //    AuditingDetail _auditDetail = new AuditingDetail();
                //    _auditDetail.ColumnName = "ACTIVITY_PRODUCT_MN";
                //    _auditDetail.MasterID = master_PK;
                //    _auditDetail.NewValue = complexNewValue;
                //    _auditDetail.OldValue = complexOldValue;
                //    _auditDetail.PKValue = entity.activity_PK.ToString();
                //    _auditing_detal_PKOperations.Save(_auditDetail);
                //}
            }

        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_PRODUCT_MN_GetProductsByActivity", OperationType = GEMOperationType.Select)]
        public DataSet GetProductsByActivity(Int32? activity_PK)
        {
            DateTime methodStart = DateTime.Now;
            DataSet entities = new DataSet();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (activity_PK != null) parameters.Add(new GEMDbParameter("activity_PK", activity_PK, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnDataSet(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_PRODUCT_MN_GetProductsByActivityList", OperationType = GEMOperationType.Select)]
        public List<Activity_product_PK> GetProductsByActivityList(Int32? activity_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Activity_product_PK> entities = new List<Activity_product_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (activity_PK != null) parameters.Add(new GEMDbParameter("activity_PK", activity_PK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_PRODUCT_MN_DeleteByActivityPK", OperationType = GEMOperationType.Select)]
        public void DeleteByActivityPK(Int32? activity_PK)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("activity_PK", activity_PK, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                if (activity_PK != null) {
                    List<Activity_product_PK> toDelete =  GetProductsByActivityList(activity_PK);
                    foreach (Activity_product_PK act_prod in toDelete) {
                        if (newValues != null) {
                            newValues.Remove(act_prod);
                        }
                    }
                }

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

		#endregion

		#region ICRUDOperations<Activity_product_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_PRODUCT_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Activity_product_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_PRODUCT_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Activity_product_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_PRODUCT_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Activity_product_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_PRODUCT_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Activity_product_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_PRODUCT_MN_Save", OperationType = GEMOperationType.Save)]
		public override Activity_product_PK Save(Activity_product_PK entity)
		{
            Activity_product_PK apTmp = base.Save(entity);
            if (newValues != null)
            {
                if (newValues.Find(item => item.activity_product_PK == apTmp.activity_product_PK) == null)
                {
                    newValues.Add(apTmp);
                }
            }
			return apTmp;
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_PRODUCT_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
            if (newValues != null)
            {
                newValues.RemoveAll(item => item.activity_product_PK == Convert.ToInt32(entityId));
            }
			base.Delete<PKType>(entityId);
		}

		public override List<Activity_product_PK> SaveCollection(List<Activity_product_PK> entities)
		{
            List<Activity_product_PK> savedCollection = base.SaveCollection(entities);
            if (newValues != null)
            {
                foreach (Activity_product_PK apTmp in savedCollection)
                {
                    if (newValues.Find(item => item.activity_product_PK == apTmp.activity_product_PK) == null)
                    {
                        newValues.Add(apTmp);
                    }
                }
            }

            return savedCollection;
		}

		public override void DeleteCollection<PKType>(List<PKType> entityPKs)
		{
            if (newValues != null)
            {
                foreach (PKType entityId in entityPKs)
                {
                    newValues.RemoveAll(item => item.activity_product_PK == Convert.ToInt32(entityId));
                }
            }

			base.DeleteCollection<PKType>(entityPKs);
		}

		#endregion
	}
}
