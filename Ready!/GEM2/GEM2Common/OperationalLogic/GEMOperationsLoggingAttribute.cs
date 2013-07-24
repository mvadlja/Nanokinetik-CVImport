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
    /// Enables operations logging for entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GEMOperationsLoggingAttribute : Attribute
    {
        private string _dataSourceId;
        private bool _active = true;

        #region Properties

        /// <summary>
        /// Used to identify data source when multiple attributes are applied to entity (must be in sync with GEMDataSourceBindingAttribute.DataSourceId)
        /// </summary>
        public string DataSourceId
        {
            get { return _dataSourceId; }
            set { _dataSourceId = value; }
        }

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        #endregion

        public GEMOperationsLoggingAttribute() { }
    }
}
