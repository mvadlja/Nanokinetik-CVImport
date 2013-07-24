-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[pharmaceutical_product_PK], [name], [ID], [responsible_user_FK], [description], [comments], [Pharmform_FK],[booked_slots]
	FROM [dbo].[PHARMACEUTICAL_PRODUCT]
END
