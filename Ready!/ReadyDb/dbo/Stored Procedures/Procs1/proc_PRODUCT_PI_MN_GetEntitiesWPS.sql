﻿-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PI_MN_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'product_pi_mn_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[product_pi_mn_PK], [product_indications_FK], [product_FK]
		FROM [dbo].[PRODUCT_PI_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PRODUCT_PI_MN]
END
