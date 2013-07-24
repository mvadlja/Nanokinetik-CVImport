
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_CODE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [substance_code_PK]) AS RowNum,
		[substance_code_PK], [code], [code_system_FK], [code_system_id_FK], [code_system_status_FK], [code_system_changedate], [comment]
		FROM [dbo].[SUBSTANCE_CODE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUBSTANCE_CODE]
END
