-- GetOrganisationInRolesByOrganisationPK
CREATE PROCEDURE  [dbo].[proc_PP_EXCIPIENT_GetExcipientsByPPPK]
	@pharmaceutical_product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[PP_EXCIPIENT].*
	FROM [dbo].[PP_EXCIPIENT]
	WHERE ([dbo].[PP_EXCIPIENT].[pp_FK] = @pharmaceutical_product_FK OR @pharmaceutical_product_FK IS NULL)

END
