// ======================================================================================================================
// Author:		Mateo-HP\Mateo
// Create date:	6.12.2011. 13:47:41
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ACTIVITY_PROJECT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class Activity_project_PKDAL : GEMDataAccess<Activity_project_PK>, IActivity_project_PKOperations
    {
        IXevprm_message_PKOperations _xevprm_message_PKOperations = new Xevprm_message_PKDAL();
        IAuditingDetailOperations _auditing_detal_PKOperations = new AuditingDetailDAL();
        IActivity_PKOperations _activity_PKOperatoins = new Activity_PKDAL();
        IProject_PKOperations _project_PKOperations = new Project_PKDAL();

        private List<Activity_project_PK> oldValues;
        private List<Activity_project_PK> newValues;

        public Activity_project_PKDAL() : base() { }
        public Activity_project_PKDAL(string dataSourceId) : base(dataSourceId) { }

        #region IActivity_project_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_PROJECT_MN_DeleteByActivity", OperationType = GEMOperationType.Select)]
        public void DeleteByActivity(int activityPk)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ActivityPk", activityPk, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

        public void StartSessionActivity(int? activity_PK)
        {
            if (oldValues == null) oldValues = new List<Activity_project_PK>();
            if (newValues == null) newValues = new List<Activity_project_PK>();
            oldValues.Clear();
            newValues.Clear();

            oldValues = GetEntities().FindAll(item => item.activity_FK == activity_PK);
            newValues = GetEntities().FindAll(item => item.activity_FK == activity_PK);
        }

        public void EndSessionActivity(int? activity_PK)
        {
            oldValues.Sort(delegate(Activity_project_PK ap1, Activity_project_PK ap2)
            {
                if (ap1.project_FK < ap2.project_FK) return -1;
                if (ap1.project_FK > ap2.project_FK) return 1;
                return 0;
            });
            newValues.Sort(delegate(Activity_project_PK ap1, Activity_project_PK ap2)
            {
                if (ap1.project_FK < ap2.project_FK) return -1;
                if (ap1.project_FK > ap2.project_FK) return 1;
                return 0;
            });

            if (oldValues != newValues)
            {
                string complexOldValue = "";
                string complexNewValue = "";

                foreach (Activity_project_PK oldValue in oldValues)
                {
                    if (complexOldValue != "") complexOldValue += "|||";
                    complexOldValue += _project_PKOperations.GetEntity(oldValue.project_FK).name;
                }

                foreach (Activity_project_PK newValue in newValues)
                {
                    if (complexNewValue != "") complexNewValue += "|||";
                    complexNewValue += _project_PKOperations.GetEntity(newValue.project_FK).name;
                }
                //if (complexOldValue != complexNewValue)
                //{

                //    Activity_PK entity = _activity_PKOperatoins.GetEntity(activity_PK);
                //    Int32 master_PK = _xevprm_message_PKOperations.GetAuditMasterIDBySessionToken((string)System.Web.HttpContext.Current.Session["AUDIT_TRAIL_TOKEN"]);
                //    AuditingDetail _auditDetail = new AuditingDetail();
                //    _auditDetail.ColumnName = "ACTIVITY_PROJECT_MN";
                //    _auditDetail.MasterID = master_PK;
                //    _auditDetail.NewValue = complexNewValue;
                //    _auditDetail.OldValue = complexOldValue;
                //    _auditDetail.PKValue = entity.activity_PK.ToString();
                //    _auditing_detal_PKOperations.Save(_auditDetail);
                //}
            }

        }



        #endregion

        #region ICRUDOperations<Activity_project_PK> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_PROJECT_MN_GetEntity", OperationType = GEMOperationType.Select)]
        public override Activity_project_PK GetEntity<PKType>(PKType entityId)
        {
            return base.GetEntity<PKType>(entityId);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_PROJECT_MN_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<Activity_project_PK> GetEntities()
        {
            return base.GetEntities();
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_PROJECT_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<Activity_project_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_PROJECT_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<Activity_project_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_PROJECT_MN_Save", OperationType = GEMOperationType.Save)]
        public override Activity_project_PK Save(Activity_project_PK entity)
        {

            Activity_project_PK apTmp = base.Save(entity);
            if (newValues != null)
            {
                if (newValues.Find(item => item.activity_project_PK == apTmp.activity_project_PK) == null)
                {
                    newValues.Add(apTmp);
                }
            }
            return apTmp;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_PROJECT_MN_Delete", OperationType = GEMOperationType.Delete)]
        public override void Delete<PKType>(PKType entityId)
        {
            if (newValues != null)
            {
                newValues.RemoveAll(item => item.activity_project_PK == Convert.ToInt32(entityId));
            }
            base.Delete<PKType>(entityId);
        }

        public override List<Activity_project_PK> SaveCollection(List<Activity_project_PK> entities)
        {
            List<Activity_project_PK> savedCollection = base.SaveCollection(entities);
            if (newValues != null)
            {
                foreach (Activity_project_PK apTmp in savedCollection)
                {
                    if (newValues.Find(item => item.activity_project_PK == apTmp.activity_project_PK) == null)
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
                    newValues.RemoveAll(item => item.activity_project_PK == Convert.ToInt32(entityId));
                }
            }
            base.DeleteCollection<PKType>(entityPKs);
        }

        #endregion
    }
}
