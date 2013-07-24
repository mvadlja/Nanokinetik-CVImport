-- Delete
create PROCEDURE  [dbo].[proc_PRODUCT_PP_MN_DeleteByPharmaceuticalProduct]
	@PharmaceuticalProductPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_PP_MN] WHERE [pp_FK] = @PharmaceuticalProductPk AND @PharmaceuticalProductPk IS NOT NULL
END