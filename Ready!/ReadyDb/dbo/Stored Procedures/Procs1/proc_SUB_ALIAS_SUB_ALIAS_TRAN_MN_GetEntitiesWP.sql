-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_SUB_ALIAS_SUB_ALIAS_TRAN_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [sub_alias_sub_alias_tran_PK]) AS RowNum,
		[sub_alias_sub_alias_tran_PK], [sub_alias_FK], [sub_alias_tran_FK]
		FROM [dbo].[SUB_ALIAS_SUB_ALIAS_TRAN_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUB_ALIAS_SUB_ALIAS_TRAN_MN]
END
