﻿-- GetEntity
create PROCEDURE proc_USER_ROLE_ACTION_GetEntity
	@user_role_action_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[user_role_action_PK], [user_role_FK], [location_FK], [user_action_FK]
	FROM [dbo].[USER_ROLE_ACTION]
	WHERE [user_role_action_PK] = @user_role_action_PK
END