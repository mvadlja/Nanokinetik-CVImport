-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PP_AR_MN_GetEntity]
	@pp_ar_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[pp_ar_mn_PK], [admin_route_FK], [pharmaceutical_product_FK]
	FROM [dbo].[PP_AR_MN]
	WHERE [pp_ar_mn_PK] = @pp_ar_mn_PK
END
