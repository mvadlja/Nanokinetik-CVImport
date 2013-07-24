// ======================================================================================================================
// Author:		TomoZ560\Tomo
// Create date:	21.10.2011. 0:03:20
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ATTACHMENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Attachment_PKDAL : GEMDataAccess<Attachment_PK>, IAttachment_PKOperations
	{
		public Attachment_PKDAL() : base() { }
		public Attachment_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IAttachment_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_GetCheckedInAttachment", OperationType = GEMOperationType.Select)]
        public Attachment_PK GetCheckedInAttachment(int? attachmentPk, Guid sessionId)
        {
            DateTime methodStart = DateTime.Now;
            Attachment_PK entity = new Attachment_PK();

            try
            {
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("attachmentPk", attachmentPk, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("sessionId", sessionId, DbType.Guid, ParameterDirection.Input));

                entity = base.ExecuteProcedureReturnEntity(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entity;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_GetAttachmentsBySessionId", OperationType = GEMOperationType.Select)]
        public List<Attachment_PK> GetAttachmentsBySessionId(Guid sessionId)
        {
            var methodStart = DateTime.Now;
            var entities = new List<Attachment_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("sessionId", sessionId, DbType.Guid, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_GetEntityByEVCode", OperationType = GEMOperationType.Select)]
        public Attachment_PK GetEntityByEVCode(string ev_code)
        {
            DateTime methodStart = DateTime.Now;
            Attachment_PK entity = new Attachment_PK();

            try
            {
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ev_code", ev_code, DbType.String, ParameterDirection.Input));

                entity = base.ExecuteProcedureReturnEntity(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entity;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_GetEntitiesWithoutDiskFile", OperationType = GEMOperationType.Select)]
        public List<Attachment_PK> GetEntitiesWithoutDiskFile()
        {
            DateTime methodStart = DateTime.Now;
            List<Attachment_PK> entities = new List<Attachment_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_GetAttachmentsForDocumentWP", OperationType = GEMOperationType.Select)]
        public List<Attachment_PK> GetAttachmentsForDocumentWP(Int32 document_FK, int pageNumber, int pageSize, out int totalRecordsCount)
        {
            DateTime methodStart = DateTime.Now;
            List<Attachment_PK> entities = new List<Attachment_PK>();
            totalRecordsCount = 0;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("document_FK", document_FK, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);
                totalRecordsCount = (int)outputValues["totalRecordsCount"];

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_GetAttachmentsForDocumentWithDiskFile", OperationType = GEMOperationType.Select)]
        public List<Attachment_PK> GetAttachmentsForDocumentWithDiskFile(Int32 document_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Attachment_PK> entities = new List<Attachment_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("document_FK", document_FK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_GetAttachmentsForDocument", OperationType = GEMOperationType.Select)]
        public List<Attachment_PK> GetAttachmentsForDocument(Int32 document_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Attachment_PK> entities = new List<Attachment_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("document_FK", document_FK, DbType.Int32, ParameterDirection.Input));
            
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_SaveWithoutDiskFile", OperationType = GEMOperationType.Select)]
        public Attachment_PK SaveWithoutDiskFile(Int32? attachment_PK, Int32? document_FK, String attachmentname, String filetype,String typeForFts, Int32? userID)
        {
            DateTime methodStart = DateTime.Now;
            Attachment_PK entity = new Attachment_PK();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (attachment_PK == null) return null;

                parameters.Add(new GEMDbParameter("attachment_PK", attachment_PK, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("document_FK", document_FK, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("attachmentname", attachmentname, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("filetype", filetype, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("type_for_fts", typeForFts, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("userID", userID, DbType.Int32, ParameterDirection.Input));

                entity = base.ExecuteProcedureReturnEntity(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entity;
        }


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_SaveLinkToDocument", OperationType = GEMOperationType.Select)]
        public Attachment_PK SaveLinkToDocument(Int32? attachment_PK, Int32? document_FK)
        {
            DateTime methodStart = DateTime.Now;
            Attachment_PK entity = new Attachment_PK();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (attachment_PK == null || document_FK == null) return null;

                parameters.Add(new GEMDbParameter("attachment_PK", attachment_PK, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("document_FK", document_FK, DbType.Int32, ParameterDirection.Input));

                entity = base.ExecuteProcedureReturnEntity(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entity;
        }


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_GetEntityWithoutDiskFile", OperationType = GEMOperationType.Select)]
        public Attachment_PK GetEntityWithoutDiskFile(Int32? attachment_PK)
        {
            DateTime methodStart = DateTime.Now;
            Attachment_PK entity = new Attachment_PK();

            try
            {
                if (attachment_PK == null) return null;

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("attachment_PK", attachment_PK, DbType.Int32, ParameterDirection.Input));

                entity = base.ExecuteProcedureReturnEntity(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entity;
        }



        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_DeleteNULLByUserID", OperationType = GEMOperationType.Delete)]
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

		#region ICRUDOperations<Attachment_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_GetEntity", OperationType = GEMOperationType.Select)]
		public override Attachment_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Attachment_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Attachment_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Attachment_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_Save", OperationType = GEMOperationType.Save)]
		public override Attachment_PK Save(Attachment_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ATTACHMENT_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Attachment_PK> SaveCollection(List<Attachment_PK> entities)
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
