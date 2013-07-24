
-- GetEntities
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[official_name_PK], [on_type_FK], [on_status_FK], [on_status_changedate], [on_jurisdiction_FK], [on_domain_FK]
	FROM [dbo].[OFFICIAL_NAME]
END
