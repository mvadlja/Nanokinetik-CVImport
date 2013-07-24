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
    /// Binds data source to entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class GEMDataSourceBindingAttribute : Attribute
    {
        private string _dataSourceId;
        private string _connectionStringName;

        #region Properties

        /// <summary>
        /// Used to identify data source when multiple attributes are applied to entity
        /// </summary>
        public string DataSourceId
        {
            get { return _dataSourceId; }
            set { _dataSourceId = value; }
        }

        public string ConnectionStringName
        {
            get { return _connectionStringName; }
            set { _connectionStringName = value; }
        }

        #endregion

        public GEMDataSourceBindingAttribute() { }
    }
}
