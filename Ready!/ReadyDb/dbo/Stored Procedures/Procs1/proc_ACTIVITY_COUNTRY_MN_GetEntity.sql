-- GetEntity
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_COUNTRY_MN_GetEntity]
	@activity_country_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[activity_country_PK], [activity_FK], [country_FK]
	FROM [dbo].[ACTIVITY_COUNTRY_MN]
	WHERE [activity_country_PK] = @activity_country_PK
END
