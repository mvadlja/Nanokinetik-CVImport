
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [official_name_PK]) AS RowNum,
		[official_name_PK], [on_type_FK], [on_status_FK], [on_status_changedate], [on_jurisdiction_FK], [on_domain_FK]
		FROM [dbo].[OFFICIAL_NAME]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[OFFICIAL_NAME]
END
