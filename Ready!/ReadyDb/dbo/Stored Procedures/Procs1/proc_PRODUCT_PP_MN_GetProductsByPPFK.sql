CREATE PROCEDURE  [dbo].[proc_PRODUCT_PP_MN_GetProductsByPPFK]
	@pp_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[PRODUCT_PP_MN].*
	FROM [dbo].[PRODUCT_PP_MN]
	WHERE ([dbo].[PRODUCT_PP_MN].pp_FK = @pp_FK OR @pp_FK IS NULL)

END
