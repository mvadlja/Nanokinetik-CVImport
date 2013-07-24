using System;
using System.Linq;
using AspNetUIFramework;
using Ready.Model;
using System.Threading;

namespace AspNetUI.Support
{
    public class SecurityOperations
    {
        public enum SecuredEntity
        { 
            AUTHORISED_PRODUCTS,
            PRODUCTS,
            ACTIVITES,
            PHARMACEUTICAL_PRODUCTS,
            SUBMISSION_UNITS,
            PROJECTS,
            TASKS,
            TIME,
            DOCUMENTS
        }

        public enum PermissionType { 
            NONE,
            READ,
            READ_WRITE,
        }

        public static bool CheckUserRole(string role)
        {
            if (SessionManager.Instance.CurrentUser == null) return false;
            string[] roles = SessionManager.Instance.CurrentUser.Roles;
            if (roles == null) return false;
            return roles.Contains(role);
        }

        public static ListForm.ListPermissionType CheckResponsibleUserAccessForPreview(SecuredEntity entityName, String entityPK) {
            DetailsForm.DetailsPermissionType detailPermission = CheckResponsibleUserAccessForDetails(entityName, entityPK);
            switch (detailPermission) { 
                case DetailsForm.DetailsPermissionType.NONE:
                    return ListForm.ListPermissionType.NONE;

                case DetailsForm.DetailsPermissionType.READ:
                    return ListForm.ListPermissionType.READ;

                case DetailsForm.DetailsPermissionType.READ_WRITE:
                    return ListForm.ListPermissionType.READ_WRITE;

                default:
                    return ListForm.ListPermissionType.READ;
            }

        }

        public static DetailsForm.DetailsPermissionType CheckResponsibleUserAccessForDetails(SecuredEntity entityName, String entityPK)
        {
            Int32? responsibleUserPK = null;
            object entity = GetEntityByName(entityName, entityPK);

            if (entity == null) return DetailsForm.DetailsPermissionType.READ_WRITE;

            switch (entityName)
            {
                case SecuredEntity.AUTHORISED_PRODUCTS:
                    responsibleUserPK = (entity as AuthorisedProduct).responsible_user_person_FK;
                    break;
                case SecuredEntity.PRODUCTS:
                    responsibleUserPK = (entity as Product_PK).responsible_user_person_FK;
                    break;
                case SecuredEntity.PHARMACEUTICAL_PRODUCTS:
                    responsibleUserPK = (entity as Pharmaceutical_product_PK).responsible_user_FK;
                    break;
                case SecuredEntity.SUBMISSION_UNITS:
                    responsibleUserPK = (entity as Subbmission_unit_PK).person_FK;
                    break;
                case SecuredEntity.PROJECTS:
                    responsibleUserPK = (entity as Project_PK).user_FK;
                    break;
                case SecuredEntity.ACTIVITES:
                    responsibleUserPK = (entity as Activity_PK).user_FK;
                    break;
                case SecuredEntity.TASKS:
                    responsibleUserPK = (entity as Task_PK).user_FK;
                    break;
                case SecuredEntity.TIME:
                    responsibleUserPK = (entity as Time_unit_PK).user_FK;
                    break;
                case SecuredEntity.DOCUMENTS:
                    responsibleUserPK = (entity as Document_PK).person_FK;
                    break;
                default:
                    responsibleUserPK = null;
                    break;
            }

            if (responsibleUserPK == null) return DetailsForm.DetailsPermissionType.READ;

            IUSEROperations _userOperations = new USERDAL();
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);

            if (user.Person_FK != responsibleUserPK) return DetailsForm.DetailsPermissionType.READ;

            return DetailsForm.DetailsPermissionType.READ_WRITE;
        }

        private static object GetEntityByName(SecuredEntity entityName, String entityPK) {
            object entity = null;
            if (!String.IsNullOrEmpty(entityPK)) {
                Int32? pk = Convert.ToInt32(entityPK);

                switch (entityName) { 
                    case SecuredEntity.AUTHORISED_PRODUCTS:
                        return (new AuthorisedProductDAL()).GetEntity(pk);

                    case SecuredEntity.PRODUCTS:
                        return (new Product_PKDAL()).GetEntity(pk);

                    case SecuredEntity.PHARMACEUTICAL_PRODUCTS:
                        return (new Pharmaceutical_product_PKDAL()).GetEntity(pk);

                    case SecuredEntity.SUBMISSION_UNITS:
                        return (new Subbmission_unit_PKDAL()).GetEntity(pk);

                    case SecuredEntity.PROJECTS:
                        return (new Project_PKDAL()).GetEntity(pk);

                    case SecuredEntity.ACTIVITES:
                        return (new Activity_PKDAL()).GetEntity(pk);

                    case SecuredEntity.TASKS:
                        return (new Task_PKDAL()).GetEntity(pk);

                    case SecuredEntity.TIME:
                        return (new Time_unit_PKDAL()).GetEntity(pk);

                    case SecuredEntity.DOCUMENTS:
                        return (new Document_PKDAL()).GetEntity(pk);

                    default:
                        return null;
                }

            }
            return entity;
        }
        
    }
}