-- GetAvailableProductsForActivity
CREATE PROCEDURE  [dbo].[proc_PRODUCT_GetSelectedProductsForActivity]
	@activity_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT [dbo].[PRODUCT].[product_PK], [dbo].[PRODUCT].[operationtype], [dbo].[PRODUCT].[newownerid], [dbo].[PRODUCT].[resolutionmode], [dbo].[PRODUCT].[enquiryemail], [dbo].[PRODUCT].[enquiryphone], [dbo].[PRODUCT].[senderlocalcode], [dbo].[PRODUCT].[infodateformat], [dbo].[PRODUCT].[infodate], [dbo].[PRODUCT].[orphan_drug], [dbo].[PRODUCT].[intensive_monitoring], [dbo].[PRODUCT].[authorisation_procedure], [dbo].[PRODUCT].[comments], [dbo].[PRODUCT].[responsible_user_person_FK], [dbo].[PRODUCT].[procedure_type], [dbo].[PRODUCT].[psur], [dbo].[PRODUCT].[next_dlp], [dbo].[PRODUCT].[name], [dbo].[PRODUCT].[description], [dbo].[PRODUCT].[client_organization_FK], [dbo].[PRODUCT].[type_product_FK], [dbo].[PRODUCT].[product_number], [dbo].[PRODUCT].[product_ID], [dbo].[PRODUCT].[xevprm_product_number_type], [dbo].[PRODUCT].[domain_FK]
	FROM [dbo].[PRODUCT]
	WHERE product_PK IN
	(
		SELECT DISTINCT product_FK FROM [dbo].[ACTIVITY_PRODUCT_MN]
		WHERE activity_FK = @activity_FK
	)
	
END
