﻿-- Delete
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_ORGANIZATION_APP_MN_DeleteByActivityPK]
	@activity_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM dbo.ACTIVITY_ORGANIZATION_APP_MN WHERE activity_FK = @activity_PK;
END
