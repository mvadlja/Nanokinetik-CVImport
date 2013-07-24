//===========================================================================================================
// GEM2 - Generic entity model 2
//===========================================================================================================
// Copyright © Bruno Klarin (brunedito@yahoo.com).  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED.
//===========================================================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace GEM2Common
{
    /// <summary>
    /// Binds method to stored procedure
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class GEMOperationBindingAttribute : Attribute
    {
        private string _dataSourceId;
        private string _procedureName;
        private GEMOperationType _operationType;

        #region Properties

        /// <summary>
        /// Used to identify data source when multiple attributes are applied to method (must be in sync with GEMDataSourceBindingAttribute.DataSourceId)
        /// </summary>
        public string DataSourceId
        {
            get { return _dataSourceId; }
            set { _dataSourceId = value; }
        }

        public string ProcedureName
        {
            get { return _procedureName; }
            set { _procedureName = value; }
        }

        public GEMOperationType OperationType
        {
            get { return _operationType; }
            set { _operationType = value; }
        }

        #endregion

        public GEMOperationBindingAttribute() { }
    }
}
