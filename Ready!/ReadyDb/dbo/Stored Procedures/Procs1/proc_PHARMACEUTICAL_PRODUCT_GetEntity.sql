-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_GetEntity]
	@pharmaceutical_product_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[pharmaceutical_product_PK], [name], [ID], [responsible_user_FK], [description], [comments], [Pharmform_FK],booked_slots
	FROM [dbo].[PHARMACEUTICAL_PRODUCT]
	WHERE [pharmaceutical_product_PK] = @pharmaceutical_product_PK
END
