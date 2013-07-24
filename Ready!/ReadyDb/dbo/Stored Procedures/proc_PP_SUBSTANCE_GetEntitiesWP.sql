
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_PP_SUBSTANCE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ppsubstance_PK]) AS RowNum,
		[ppsubstance_PK], [ppsubstance_FK], [substancecode_FK], [concentrationtypecode], [lowamountnumervalue], [lowamountnumerprefix], [lowamountnumerunit], [lowamountdenomvalue], [lowamountdenomprefix], [lowamountdenomunit], [highamountnumervalue], [highamountnumerprefix], [highamountnumerunit], [highamountdenomvalue], [highamountdenomprefix], [highamountdenomunit], [pp_FK], [expressedby_FK], [concise], [substancetype], [user_FK], [sessionid], [modifieddate]
		FROM [dbo].[PP_SUBSTANCE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PP_SUBSTANCE]
END