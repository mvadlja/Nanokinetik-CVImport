
-- GetEntity
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_GetEntity]
	@official_name_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[official_name_PK], [on_type_FK], [on_status_FK], [on_status_changedate], [on_jurisdiction_FK], [on_domain_FK]
	FROM [dbo].[OFFICIAL_NAME]
	WHERE [official_name_PK] = @official_name_PK
END
