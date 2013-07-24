-- GetEntities
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_COUNTRY_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[activity_country_PK], [activity_FK], [country_FK]
	FROM [dbo].[ACTIVITY_COUNTRY_MN]
END
