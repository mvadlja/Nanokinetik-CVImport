using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AspNetUIFramework
{
    public static class ControlBindingHelpers
    {
        #region MultiValueList helpers

        public static List<TDefinitionEntityKeyType> MultiValueList_RetreiveCollectionOfDefinitionEntitiesForSave<TDefinitionEntityKeyType, TLinkedEntityType>(List<string> controlSelectedValues, List<TLinkedEntityType> existingSavedLinks, string linkedEntityRelatedDefinitionPropertyName)
            where TLinkedEntityType : class
        {
            List<TDefinitionEntityKeyType> definitionEntitiesForSave = new List<TDefinitionEntityKeyType>();
            PropertyInfo linkedEntityRelatedPropertyInfo = typeof(TLinkedEntityType).GetProperty(linkedEntityRelatedDefinitionPropertyName);
            string linkedEntityRelatedPropertyValue = String.Empty;

            // Save collection
            bool isNew = true;
            foreach (string controlSelectedValue in controlSelectedValues)
            {
                isNew = true;

                foreach (TLinkedEntityType existingSavedLink in existingSavedLinks)
                {
                    linkedEntityRelatedPropertyValue = linkedEntityRelatedPropertyInfo.GetValue(existingSavedLink, null).ToString();

                    if (controlSelectedValue == linkedEntityRelatedPropertyValue)
                    {
                        isNew = false;
                        break;
                    }
                }

                if (isNew)
                {
                    definitionEntitiesForSave.Add((TDefinitionEntityKeyType)Convert.ChangeType(controlSelectedValue, typeof(TDefinitionEntityKeyType)));
                }
            }

            return definitionEntitiesForSave;
        }

        public static List<TLinkedEntityKeyType> MultiValueList_RetreiveCollectionOfLinkedEntitiesForDelete<TLinkedEntityKeyType, TLinkedEntityType>(List<string> controlSelectedValues, List<TLinkedEntityType> existingSavedLinks, string linkedEntityRelatedDefinitionPropertyName, string linkedEntityPrimaryKeyPropertyName) 
            where TLinkedEntityType : class 
        {
            List<TLinkedEntityKeyType> linkedEntitiesForDelete = new List<TLinkedEntityKeyType>();
            PropertyInfo linkedEntityRelatedPropertyInfo = typeof(TLinkedEntityType).GetProperty(linkedEntityRelatedDefinitionPropertyName);
            string linkedEntityRelatedPropertyValue = String.Empty;
            PropertyInfo linkedEntityPrimaryKeyPropertyInfo = typeof(TLinkedEntityType).GetProperty(linkedEntityPrimaryKeyPropertyName);
            string linkedEntityPrimaryKeyPropertyValue = String.Empty;

            // Delete collection
            bool stillSelected = false;
            foreach (TLinkedEntityType existingSavedLink in existingSavedLinks)
            {
                stillSelected = false;
                linkedEntityRelatedPropertyValue = linkedEntityRelatedPropertyInfo.GetValue(existingSavedLink, null).ToString();

                foreach (string controlSelectedValue in controlSelectedValues)
                {
                    if (controlSelectedValue == linkedEntityRelatedPropertyValue)
                    {
                        stillSelected = true;
                        break;
                    }
                }

                if (!stillSelected)
                {
                    linkedEntityPrimaryKeyPropertyValue = linkedEntityPrimaryKeyPropertyInfo.GetValue(existingSavedLink, null).ToString();
                    linkedEntitiesForDelete.Add((TLinkedEntityKeyType)Convert.ChangeType(linkedEntityPrimaryKeyPropertyValue, typeof(TLinkedEntityKeyType)));
                }
            }

            return linkedEntitiesForDelete;
        }

        #endregion
    }
}
