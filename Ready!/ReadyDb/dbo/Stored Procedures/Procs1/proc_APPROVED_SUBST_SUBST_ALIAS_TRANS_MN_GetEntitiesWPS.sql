﻿-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBST_ALIAS_TRANS_MN_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'approved_subst_sub_alias_trans_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[approved_subst_sub_alias_trans_PK], [approved_substance_FK], [substance_alias_translation_FK]
		FROM [dbo].[APPROVED_SUBST_SUBST_ALIAS_TRANS_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[APPROVED_SUBST_SUBST_ALIAS_TRANS_MN]
END
