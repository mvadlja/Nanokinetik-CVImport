-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PP_MD_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [pp_md_mn_PK]) AS RowNum,
		[pp_md_mn_PK], [pp_medical_device_FK], [pharmaceutical_product_FK]
		FROM [dbo].[PP_MD_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PP_MD_MN]
END
