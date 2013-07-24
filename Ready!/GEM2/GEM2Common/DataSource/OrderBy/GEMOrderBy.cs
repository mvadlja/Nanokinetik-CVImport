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
    /// Definition of order-by conditions.
    /// </summary>
    public class GEMOrderBy
    {
        private string _orderByElement;
        private GEMOrderByType _orderByType;

        #region Properties

        public string OrderByElement
        {
            get { return _orderByElement; }
            set { _orderByElement = value; }
        }

        public GEMOrderByType OrderByType
        {
            get { return _orderByType; }
            set { _orderByType = value; }
        }

        #endregion

        public GEMOrderBy() { }
        public GEMOrderBy(string orderByElement, GEMOrderByType orderByType)
        {
            this.OrderByElement = orderByElement;
            this.OrderByType = orderByType;
        }
    }
}
