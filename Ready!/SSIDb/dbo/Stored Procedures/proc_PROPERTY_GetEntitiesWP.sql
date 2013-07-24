
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_PROPERTY_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [property_PK]) AS RowNum,
		[property_PK], [property_type], [property_name], [substance_id], [substance_name], [amount_type], [amount_FK]
		FROM [dbo].[PROPERTY]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PROPERTY]
END
