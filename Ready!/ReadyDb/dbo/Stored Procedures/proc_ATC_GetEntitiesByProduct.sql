-- GetEntities
CREATE PROCEDURE  proc_ATC_GetEntitiesByProduct
	@ProductPk INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[atc_PK], [operationtype], [type_term], [atccode], [newownerid], [atccode_desc], [versiondateformat], [versiondate], [comments], [pom_code], [pom_subcode], [pom_ddd], [pom_u], [pom_ar], [pom_note], [name], [name_archive], [search_by], [is_group], [evpmd_code], [value], [is_maunal_entry]
	FROM [dbo].[ATC]
	JOIN [dbo].[PRODUCT_ATC_MN] ON [dbo].[PRODUCT_ATC_MN].atc_FK = [dbo].[ATC].atc_PK
	WHERE [dbo].[PRODUCT_ATC_MN].product_FK = @ProductPk AND @ProductPk IS NOT NULL

END