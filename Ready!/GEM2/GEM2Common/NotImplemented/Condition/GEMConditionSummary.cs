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
    /// Complete condition container. Can contain conditions or conditions groups.
    /// </summary>
    internal class GEMConditionSummary
    {
        private List<GEMCondition> _conditions;
        private List<GEMConditionGroup> _conditionsGroups;

        #region Properties

        public List<GEMCondition> Conditions
        {
            get { return _conditions; }
            set { _conditions = value; }
        }
        
        public List<GEMConditionGroup> ConditionsGroups
        {
            get { return _conditionsGroups; }
            set { _conditionsGroups = value; }
        }

        #endregion

        public GEMConditionSummary() {}
        public GEMConditionSummary(List<GEMCondition> conditions)
        {
            this.Conditions = conditions;
            this.ConditionsGroups = null;
        }
        public GEMConditionSummary(List<GEMConditionGroup> conditionsGroups)
        {
            this.Conditions = null;
            this.ConditionsGroups = conditionsGroups;
        }
    }
}
