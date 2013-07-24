-- Delete
create PROCEDURE  [dbo].[proc_PP_MD_MN_DeleteByPharmaceuticalProduct]
	@PharmaceuticalProductPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PP_MD_MN] WHERE pharmaceutical_product_FK = @PharmaceuticalProductPk AND @PharmaceuticalProductPk IS NOT NULL
END