-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_ATC_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'atc_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[atc_PK], [operationtype], [type_term], [atccode], [newownerid], [atccode_desc], [versiondateformat], [versiondate], [comments], [pom_code], [pom_subcode], [pom_ddd], [pom_u], [pom_ar], [pom_note], [name], [name_archive], [search_by], [is_group], [evpmd_code], [value], [is_maunal_entry]
		FROM [dbo].[ATC]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ATC]
END
