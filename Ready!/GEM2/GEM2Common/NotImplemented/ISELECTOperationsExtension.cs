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
    /// SELECT operations on data source for T entity only.
    /// </summary>
    /// <typeparam name="T">Any class type (entity)</typeparam>
    internal interface ISELECTOperationsExtension<T> where T : class
    {
        /// <summary>
        /// Gets entity from data source by conditions
        /// </summary>
        /// <param name="conditionsGroups"></param>
        /// <returns></returns>
        T GetEntityByConditions(List<GEMConditionGroup> conditionsGroups);

        /// <summary>
        /// Gets entities from data source by conditions
        /// </summary>
        /// <param name="conditionsGroups"></param>
        /// <returns></returns>
        List<T> GetEntitiesByConditions(List<GEMConditionGroup> conditionsGroups);

        /// <summary>
        /// Gets entities from data source with paging support by conditions
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecordsCount"></param>
        /// <param name="conditionsGroups"></param>
        /// <returns></returns>
        List<T> GetEntitiesByConditions(int pageNumber, int pageSize, out int totalRecordsCount, List<GEMConditionGroup> conditionsGroups);

        /// <summary>
        /// Gets entities from data source with paging and sorting support by conditions
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByConditions"></param>
        /// <param name="totalRecordsCount"></param>
        /// <param name="conditionsGroups"></param>
        /// <returns></returns>
        List<T> GetEntitiesByConditions(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount, List<GEMConditionGroup> conditionsGroups);
    }
}
