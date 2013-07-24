-- GetEntities
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetSelectedEntitiesForActivityPK_MN]
	@activity_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[country_PK], [name], [abbreviation], [region], [code], [custom_sort_ID]
	FROM [dbo].[COUNTRY]
	WHERE
	country_PK
	IN
	(
		select country_FK from dbo.ACTIVITY_COUNTRY_MN where activity_FK = @activity_PK
	)
	ORDER BY [custom_sort_ID] ASC
END
