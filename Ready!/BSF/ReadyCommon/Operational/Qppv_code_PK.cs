// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:40:22
// Description:	GEM2 Generated class for table SSI.dbo.RI_TARGET_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "QPPV_CODE")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Qppv_code_PK
    {
        private Int32? _qppv_code_PK;
        private Int32? _person_FK;
        private String _qppv_code;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? qppv_code_PK
        {
          get { return _qppv_code_PK; }
          set { _qppv_code_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? person_FK
        {
          get { return _person_FK; }
          set { _person_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String qppv_code
        {
          get { return _qppv_code; }
          set { _qppv_code = value; }
        }

        #endregion

        public Qppv_code_PK() { }
        public Qppv_code_PK(Int32? qppv_code_PK, Int32? person_FK, String qppv_code)
        {
            this.qppv_code_PK = qppv_code_PK;
            this.person_FK = person_FK;
            this.qppv_code = qppv_code;
        }

        public override string ToString()
        {
            return this.qppv_code != null ? this.qppv_code : String.Empty;
        }
    }

   
    public interface IQppv_code_PKOperations : ICRUDOperations<Qppv_code_PK>
    {
        List<Qppv_code_PK> GetQppvCodeByPerson(int? person_PK);
        DataSet GetQppvByPersonCodeSearcher(String personName, String qppv_code, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetLocalQppvByPersonRoleSearcher(String personName, String qppv_code, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
    }
}
