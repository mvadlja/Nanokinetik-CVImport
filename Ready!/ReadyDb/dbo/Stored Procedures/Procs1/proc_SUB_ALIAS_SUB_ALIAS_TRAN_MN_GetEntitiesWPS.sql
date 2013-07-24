-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_SUB_ALIAS_SUB_ALIAS_TRAN_MN_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'sub_alias_sub_alias_tran_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[sub_alias_sub_alias_tran_PK], [sub_alias_FK], [sub_alias_tran_FK]
		FROM [dbo].[SUB_ALIAS_SUB_ALIAS_TRAN_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUB_ALIAS_SUB_ALIAS_TRAN_MN]
END
