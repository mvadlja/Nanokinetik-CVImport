﻿-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_PP_ACTIVE_INGREDIENT_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'activeingredient_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[activeingredient_PK], [substancecode_FK], [resolutionmode], [concentrationtypecode], [lowamountnumervalue], [lowamountnumerprefix], [lowamountnumerunit], [lowamountdenomvalue], [lowamountdenomprefix], [lowamountdenomunit], [highamountnumervalue], [highamountnumerprefix], [highamountnumerunit], [highamountdenomvalue], [highamountdenomprefix], [highamountdenomunit], [pp_FK], [userID], [strength_value], [strength_unit], [ExpressedBy_FK], [concise]
		FROM [dbo].[PP_ACTIVE_INGREDIENT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PP_ACTIVE_INGREDIENT]
END
