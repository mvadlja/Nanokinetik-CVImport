﻿-- GetEntities
CREATE PROCEDURE  [dbo].[proc_USER_IN_ROLE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[user_in_role_PK], [user_FK], [user_role_FK], [row_version]
	FROM [dbo].[USER_IN_ROLE]
END
