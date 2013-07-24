-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [pharmaceutical_product_PK]) AS RowNum,
		[pharmaceutical_product_PK], [name], [ID], [responsible_user_FK], [description], [comments], [Pharmform_FK],booked_slots
		FROM [dbo].[PHARMACEUTICAL_PRODUCT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PHARMACEUTICAL_PRODUCT]
END
