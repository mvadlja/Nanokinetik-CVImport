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
    /// Defines condition.
    /// </summary>
    internal class GEMCondition
    {
        private GEMConditionJoiner _conditionJoiner;
        private string _propertyName;
        private GEMConditionOperator _conditionOperator;
        private object _conditionValue;

        #region Properties

        public GEMConditionJoiner ConditionJoiner
        {
            get { return _conditionJoiner; }
            set { _conditionJoiner = value; }
        }
        
        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

        public GEMConditionOperator ConditionOperator
        {
            get { return _conditionOperator; }
            set { _conditionOperator = value; }
        }

        public object ConditionValue
        {
            get { return _conditionValue; }
            set { _conditionValue = value; }
        }

        #endregion

        public GEMCondition() { }
        public GEMCondition(GEMConditionJoiner conditionJoiner, string propertyName, GEMConditionOperator conditionOperator, object conditionValue)
        {
            this.ConditionJoiner = conditionJoiner;
            this.PropertyName = propertyName;
            this.ConditionOperator = conditionOperator;
            this.ConditionValue = conditionValue;
        }
    }
}
