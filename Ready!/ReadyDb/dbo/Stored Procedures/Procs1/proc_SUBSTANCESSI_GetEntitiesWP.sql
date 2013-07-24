-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_SUBSTANCESSI_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [substancessis_PK]) AS RowNum,
		[substancessis_PK], [valid_according_ssi], [ssi_FK]
		FROM [dbo].[SUBSTANCESSI]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUBSTANCESSI]
END
