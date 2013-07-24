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
    /// CRUD operations on data source.
    /// </summary>
    /// <typeparam name="T">Any class type (entity)</typeparam>
    public interface ICRUDOperations<T> where T : class
    {
        /// <summary>
        /// Gets entity from data source by it's primary key
        /// </summary>
        /// <typeparam name="PKType"></typeparam>
        /// <param name="entityId"></param>
        /// <returns></returns>
        T GetEntity<PKType>(PKType entityId);

        /// <summary>
        /// Gets all entities from data source
        /// </summary>
        /// <returns></returns>
        List<T> GetEntities();

        /// <summary>
        /// Gets entities from data source with paging support
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecordsCount"></param>
        /// <returns></returns>
        List<T> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount);

        /// <summary>
        /// Gets entities from data source with paging and sorting support
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByConditions"></param>
        /// <param name="totalRecordsCount"></param>
        /// <returns></returns>
        List<T> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);

        /// <summary>
        /// Saves entity to data source
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Save(T entity);

        /// <summary>
        /// Deletes entity from data source
        /// </summary>
        /// <typeparam name="PKType"></typeparam>
        /// <param name="entityId"></param>
        void Delete<PKType>(PKType entityId);

        /// <summary>
        /// Saves every entity in collection to data source
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        List<T> SaveCollection(List<T> entities);

        /// <summary>
        /// Deletes every entity by its primary key in collection from data source
        /// </summary>
        /// <typeparam name="PKType"></typeparam>
        /// <param name="entityPKs"></param>
        void DeleteCollection<PKType>(List<PKType> entityPKs);
    }
}
