-- GetEntitiesWP
create PROCEDURE [dbo].[proc_MA_MA_ENTITY_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ma_ma_entity_mn_PK]) AS RowNum,
		[ma_ma_entity_mn_PK], [ma_FK], [ma_entity_FK], [ma_entity_type_FK]
		FROM [dbo].[MA_MA_ENTITY_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[MA_MA_ENTITY_MN]
END
