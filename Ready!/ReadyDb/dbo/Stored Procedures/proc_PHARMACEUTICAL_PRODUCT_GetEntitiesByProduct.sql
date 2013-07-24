-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_GetEntitiesByProduct]
	@ProductPk INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[pharmaceutical_product_PK], [name], [ID], [responsible_user_FK], [description], [comments], [Pharmform_FK],[booked_slots]
	FROM [dbo].[PHARMACEUTICAL_PRODUCT]
	JOIN [dbo].[PRODUCT_PP_MN] ON [dbo].[PRODUCT_PP_MN].pp_FK = [dbo].[PHARMACEUTICAL_PRODUCT].pharmaceutical_product_PK
	WHERE [dbo].[PRODUCT_PP_MN].product_FK = @ProductPk AND @ProductPk IS NOT NULL
END