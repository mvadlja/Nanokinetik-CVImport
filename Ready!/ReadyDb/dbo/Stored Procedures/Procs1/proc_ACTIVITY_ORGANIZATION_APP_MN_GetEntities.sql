﻿-- GetEntities
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_ORGANIZATION_APP_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[activity_organization_applicant_PK], [activity_FK], [organization_FK]
	FROM [dbo].[ACTIVITY_ORGANIZATION_APP_MN]
END
