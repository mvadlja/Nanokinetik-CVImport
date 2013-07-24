// ======================================================================================================================
// Author:		Mateo-HP\Mateo
// Create date:	6.12.2011. 16:53:06
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PROJECT_DOCUMENT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PROJECT_DOCUMENT_MN")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Project_document_PK
    {
        private Int32? _project_document_PK;
        private Int32? _project_FK;
        private Int32? _document_FK;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? project_document_PK
        {
            get { return _project_document_PK; }
            set { _project_document_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? project_FK
        {
            get { return _project_FK; }
            set { _project_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? document_FK
        {
            get { return _document_FK; }
            set { _document_FK = value; }
        }

        #endregion

        public Project_document_PK() { }
        public Project_document_PK(Int32? project_document_PK, Int32? project_FK, Int32? document_FK)
        {
            this.project_document_PK = project_document_PK;
            this.project_FK = project_FK;
            this.document_FK = document_FK;
        }
    }

    public interface IProject_document_PKOperations : ICRUDOperations<Project_document_PK>
    {
        void DeleteByDocumentID(int documentID);
        List<Project_document_PK> GetProjectMNByDocumentFK(Int32? document_FK);
        bool AbleToDeleteEntity(int documentPk);
    }
}
