using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GEM2Common;
using AspNetUIFramework;
using System.Web.UI;

namespace AspNetUI.Support
{
    public static class BindingHelper
    {
        public static void UpdateLinkedEntityByListControl<TLinkedEntityType, TKey>(List<string> controlValues, TKey mainEntityID, ICRUDOperations<TLinkedEntityType> linkedEntityDAL, Func<TKey, List<TLinkedEntityType>> getLinkedEntitiesByMainEntityKey, string linkedEntityRelatedDefinitionPropertyName, string linkedEntityPrimaryKeyPropertyName, Func<List<TKey>, TKey, List<TLinkedEntityType>> constructConcreteCollectionFromPKs) 
            where TLinkedEntityType : class
        {
            List<TLinkedEntityType> existingSavedLinkedEntities = getLinkedEntitiesByMainEntityKey.Invoke(mainEntityID);

            List<TKey> linkedEntityTypeCollectionIDsForSave = ControlBindingHelpers.MultiValueList_RetreiveCollectionOfDefinitionEntitiesForSave<TKey, TLinkedEntityType>(controlValues, existingSavedLinkedEntities, linkedEntityRelatedDefinitionPropertyName);
            List<TKey> linkedEntityTypeCollectionIDsForDelete = ControlBindingHelpers.MultiValueList_RetreiveCollectionOfLinkedEntitiesForDelete<TKey, TLinkedEntityType>(controlValues, existingSavedLinkedEntities, linkedEntityRelatedDefinitionPropertyName, linkedEntityPrimaryKeyPropertyName);

            // Extracting concrete collections from ids
            List<TLinkedEntityType> linkedEntityTypeCollectionForSave = constructConcreteCollectionFromPKs.Invoke(linkedEntityTypeCollectionIDsForSave, mainEntityID);

            // Saving
            linkedEntityDAL.SaveCollection(linkedEntityTypeCollectionForSave);

            //// Deleting
            linkedEntityDAL.DeleteCollection(linkedEntityTypeCollectionIDsForDelete);
        }
    }
}