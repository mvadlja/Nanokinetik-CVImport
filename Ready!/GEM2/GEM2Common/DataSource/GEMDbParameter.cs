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
using System.Data.Common;

namespace GEM2Common
{
    /// <summary>
    /// Represents lightweight generic parameter for data source.
    /// </summary>
    public class GEMDbParameter
    {
        private string _parameterName;
        private object _value;
        private DbType? _dbType;
        private ParameterDirection? _direction;

        #region Properties

        public string ParameterName
        {
            get { return _parameterName; }
            set { _parameterName = value; }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public DbType? DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }

        public ParameterDirection? Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        #endregion

        public GEMDbParameter() { }
        public GEMDbParameter(string name, object value, DbType? type, ParameterDirection? direction)
        {
            this.ParameterName = name;
            this.Value = value;
            this.DbType = type;
            this.Direction = direction;
        }
    }
}
