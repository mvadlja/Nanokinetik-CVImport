-- Delete
CREATE PROCEDURE  [dbo].[proc_PP_ACTIVE_INGREDIENT_Delete]
	@activeingredient_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PP_ACTIVE_INGREDIENT] WHERE [activeingredient_PK] = @activeingredient_PK
END
