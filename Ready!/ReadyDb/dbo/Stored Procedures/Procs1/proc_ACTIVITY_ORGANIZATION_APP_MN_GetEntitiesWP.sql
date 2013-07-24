﻿-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_ORGANIZATION_APP_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [activity_organization_applicant_PK]) AS RowNum,
		[activity_organization_applicant_PK], [activity_FK], [organization_FK]
		FROM [dbo].[ACTIVITY_ORGANIZATION_APP_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ACTIVITY_ORGANIZATION_APP_MN]
END
