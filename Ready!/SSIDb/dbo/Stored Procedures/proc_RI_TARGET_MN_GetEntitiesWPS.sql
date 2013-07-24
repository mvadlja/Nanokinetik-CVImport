﻿
-- GetEntitiesWPS
CREATE PROCEDURE [dbo].[proc_RI_TARGET_MN_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'ri_target_mn_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[ri_target_mn_PK], [ri_FK], [target_FK]
		FROM [dbo].[RI_TARGET_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[RI_TARGET_MN]
END