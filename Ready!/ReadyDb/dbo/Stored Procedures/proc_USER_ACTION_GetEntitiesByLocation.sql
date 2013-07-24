-- GetEntities
CREATE PROCEDURE [dbo].[proc_USER_ACTION_GetEntitiesByLocation]
	@LocationPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	ua.[user_action_PK], ua.[unique_name], ua.[display_name], ua.[description], ua.[active]
	FROM [dbo].[USER_ACTION] ua
	JOIN dbo.LOCATION_USER_ACTION_MN luaMn ON luaMn.user_action_FK = ua.user_action_PK
	WHERE  luaMn.location_FK = @LocationPk AND @LocationPk IS NOT NULL AND ua.active = '1'

END