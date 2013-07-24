-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_SAVED_SEARCH_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[pharmaceutical_products_PK],
	 [name], [responsible_user_FK],
	  [description], [product_FK],
	   [Pharmform_FK], [comments],
	    [displayName], [user_FK], 
	    [gridLayout], [isPublic],
	    [administrationRoutes],
	    [activeIngridients],[excipients]
	    ,[adjuvants],[medical_devices],pp_FK
	FROM [dbo].[PHARMACEUTICAL_PRODUCT_SAVED_SEARCH]
END
