-- GetEntity
CREATE PROCEDURE  [dbo].[proc_ATC_GetEntity]
	@atc_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[atc_PK], [operationtype], [type_term], [atccode], [newownerid], [atccode_desc], [versiondateformat], [versiondate], [comments], [pom_code], [pom_subcode], [pom_ddd], [pom_u], [pom_ar], [pom_note], [name], [name_archive], [search_by], [is_group], [evpmd_code], [value], [is_maunal_entry]
	FROM [dbo].[ATC]
	WHERE [atc_PK] = @atc_PK
END
