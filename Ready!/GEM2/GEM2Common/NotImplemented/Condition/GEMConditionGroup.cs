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
    /// Groups condition collection to group that can be joined with other groups.
    /// </summary>
    internal class GEMConditionGroup
    {
        private GEMConditionJoiner _conditionGroupJoiner;
        private List<GEMCondition> _conditionsCollection;

        #region Properties

        public GEMConditionJoiner ConditionGroupJoiner
        {
            get { return _conditionGroupJoiner; }
            set { _conditionGroupJoiner = value; }
        }
        
        public List<GEMCondition> ConditionsCollection
        {
            get { return _conditionsCollection; }
            set { _conditionsCollection = value; }
        }

        #endregion

        public GEMConditionGroup() { }
        public GEMConditionGroup(GEMConditionJoiner conditionGroupJoiner, List<GEMCondition> conditionsCollection)
        {
            this.ConditionGroupJoiner = conditionGroupJoiner;
            this.ConditionsCollection = conditionsCollection;
        }
    }
}
