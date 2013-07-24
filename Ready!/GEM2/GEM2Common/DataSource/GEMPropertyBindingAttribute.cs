//===========================================================================================================
// GEM2 - Generic entity model 2
//===========================================================================================================
// Copyright © Bruno Klarin (brunedito@yahoo.com).  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED.
//===========================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace GEM2Common
{
    /// <summary>
    /// Binds data source parameter to entity property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class GEMPropertyBindingAttribute : Attribute
    {
        private string _dataSourceId;
        private bool _isPrimaryKey;
        private bool _isForeignKey;
        private string _parameterName;
        private DbType _parameterType;
        private bool _isConfidential;
        private bool _isRowVersion;

        #region Properties

        /// <summary>
        /// Used to identify data source parameter when multiple attributes are applied to property (must be in sync with GEMDataSourceBindingAttribute.DataSourceId)
        /// </summary>
        public string DataSourceId
        {
            get { return _dataSourceId; }
            set { _dataSourceId = value; }
        }

        public bool IsPrimaryKey
        {
            get { return _isPrimaryKey; }
            set { _isPrimaryKey = value; }
        }

        public bool IsForeignKey
        {
            get { return _isForeignKey; }
            set { _isForeignKey = value; }
        }

        public string ParameterName
        {
            get { return _parameterName; }
            set { _parameterName = value; }
        }

        public DbType ParameterType
        {
            get { return _parameterType; }
            set { _parameterType = value; }
        }

        /// <summary>
        /// If confidential then it should never be logged or audited
        /// </summary>
        public bool IsConfidential
        {
            get { return _isConfidential; }
            set { _isConfidential = value; }
        }

        /// <summary>
        /// If row version then property can be used in optimistic concurrency check
        /// </summary>
        public bool IsRowVersion
        {
            get { return _isRowVersion; }
            set { _isRowVersion = value; }
        }

        #endregion

        public GEMPropertyBindingAttribute() { }
    }
}
