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
    /// Enables auditing on entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class GEMAuditingAttribute : Attribute
    {
        private string _dataSourceId;
        private string _database;
        private string _table;
        private bool _active;

        #region Properties

        /// <summary>
        /// Used to identify data source when multiple attributes are applied to entity (must be in sync with GEMDataSourceBindingAttribute.DataSourceId)
        /// </summary>
        public string DataSourceId
        {
            get { return _dataSourceId; }
            set { _dataSourceId = value; }
        }

        public string Database
        {
            get { return _database; }
            set { _database = value; }
        }

        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        #endregion

        public GEMAuditingAttribute() { }
    }
}
