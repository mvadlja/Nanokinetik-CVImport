-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_SU_AGENCY_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [su_agency_mn_PK]) AS RowNum,
		[su_agency_mn_PK], [agency_FK], [submission_unit_FK]
		FROM [dbo].[SU_AGENCY_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SU_AGENCY_MN]
END
