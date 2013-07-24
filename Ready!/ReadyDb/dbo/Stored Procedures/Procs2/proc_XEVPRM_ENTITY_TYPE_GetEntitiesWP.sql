-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_XEVPRM_ENTITY_TYPE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [xevprm_entity_type_PK]) AS RowNum,
		[xevprm_entity_type_PK], [name], [table_name]
		FROM [dbo].[XEVPRM_ENTITY_TYPE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[XEVPRM_ENTITY_TYPE]
END
