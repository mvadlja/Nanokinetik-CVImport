using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Views;
using AspNetUI.Views.Shared.Interface;
using AspNetUI.Views.Shared.UserControl;
using Ready.Model;

namespace AspNetUI.Support
{
    public class LastChange
    {
        /// <summary>
        /// Gets last change formatted display string
        /// </summary>
        /// <param name="entityPk"></param>
        /// <param name="tableName"></param>
        /// <param name="lastChangeOperations"></param>
        /// <param name="personOperations"></param>
        /// <returns></returns>
        public static string GetFormattedString(int? entityPk, string tableName, ILast_change_PKOperations lastChangeOperations, IPerson_PKOperations personOperations)
        {
            var lastChange = lastChangeOperations.GetEntityLastChange(tableName, entityPk);

            if (lastChange == null) return Constant.DefaultEmptyValue;

            var lastChangePerson = personOperations.GetPersonByUserID(lastChange.user_FK);
            string personFullName = Constant.DefaultEmptyValue;

            if (lastChangePerson != null && lastChangePerson.person_PK != null && !string.IsNullOrWhiteSpace(lastChangePerson.FullName))
            {
                personFullName = lastChangePerson.FullName;
            }
            else
            {
                if (Thread.CurrentPrincipal.Identity.Name != null) personFullName = Thread.CurrentPrincipal.Identity.Name;
            }

            personFullName = personFullName.Trim();

            if (lastChange.change_date != null)
            {
                var date = ((DateTime) lastChange.change_date).ToShortDateString().TrimEnd(new[] {'.'});
                var time = ((DateTime)lastChange.change_date).ToShortTimeString();

                return string.Format("{0}, {1} by {2}", date, time, personFullName);
            }
            return Constant.DefaultEmptyValue;
        }

        /// <summary>
        /// Handles last change mechanism. 
        /// Checks if any control has been modified and executes proper action
        /// </summary>
        /// <param name="controlContainer"></param>
        /// <param name="entityPk"></param>
        /// <param name="tableName"></param>
        /// <param name="lastChangeOperations"></param>
        /// <param name="userOperations"></param>
        /// <param name="forceLastChange">If this parameter if true, method will forcefully register last change</param>
        public static void HandleLastChange(Control controlContainer, int? entityPk, string tableName, ILast_change_PKOperations lastChangeOperations, IUSEROperations userOperations, bool forceLastChange = false)
        {
            bool formControlsContentModified = false;
            if(!forceLastChange) formControlsContentModified = controlContainer.Controls.Cast<Control>().Any(c => (c is ILastChange) && ((ILastChange)c).IsModified);

            if (formControlsContentModified || forceLastChange)
            {
                UpdateLastChange(entityPk, tableName, lastChangeOperations, userOperations);
            }
        }

        /// <summary>
        /// Logs last change into database
        /// </summary>
        /// <param name="entityPk"></param>
        /// <param name="tableName"></param>
        /// <param name="lastChangeOperations"></param>
        /// <param name="userOperations"></param>
        public static void UpdateLastChange(int? entityPk, string tableName, ILast_change_PKOperations lastChangeOperations, IUSEROperations userOperations)
        {
            var lastChange = lastChangeOperations.GetEntityLastChange(tableName, entityPk) ?? new Last_change_PK();
            lastChange.change_date = DateTime.Now;
            lastChange.change_table = tableName;
            lastChange.entity_FK = entityPk;

            var user = userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null && user.User_PK != null)
            {
                lastChange.user_FK = user.User_PK;
            }

            lastChangeOperations.Save(lastChange);
        }
    }
}