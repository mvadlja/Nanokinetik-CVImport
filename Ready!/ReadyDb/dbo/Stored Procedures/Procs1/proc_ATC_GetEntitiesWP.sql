-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_ATC_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [atc_PK]) AS RowNum,
		[atc_PK], [operationtype], [type_term], [atccode], [newownerid], [atccode_desc], [versiondateformat], [versiondate], [comments], [pom_code], [pom_subcode], [pom_ddd], [pom_u], [pom_ar], [pom_note], [name], [name_archive], [search_by], [is_group], [evpmd_code], [value], [is_maunal_entry]
		FROM [dbo].[ATC]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ATC]
END
