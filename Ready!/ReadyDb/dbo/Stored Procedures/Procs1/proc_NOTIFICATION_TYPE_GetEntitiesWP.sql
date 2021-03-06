﻿-- GetEntitiesWP
create PROCEDURE [dbo].[proc_NOTIFICATION_TYPE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [notification_type_PK]) AS RowNum,
		[notification_type_PK], [name]
		FROM [dbo].[NOTIFICATION_TYPE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[NOTIFICATION_TYPE]
END
