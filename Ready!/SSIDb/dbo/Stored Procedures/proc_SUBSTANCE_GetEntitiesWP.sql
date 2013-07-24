
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [substance_s_PK]) AS RowNum,
		[substance_s_PK], [language], [substance_id], [substance_class], [ref_info_FK], [sing_FK], [responsible_user_FK], [description], [comments], [name]
		FROM [dbo].[SUBSTANCE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUBSTANCE]
END
