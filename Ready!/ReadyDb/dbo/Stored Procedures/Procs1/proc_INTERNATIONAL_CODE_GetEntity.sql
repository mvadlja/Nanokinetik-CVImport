-- GetEntity
CREATE PROCEDURE  [dbo].[proc_INTERNATIONAL_CODE_GetEntity]
	@international_code_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[international_code_PK], [sourcecode], [resolutionmode_sources], [referencetext], [resolutionmode_substance]
	FROM [dbo].[INTERNATIONAL_CODE]
	WHERE [international_code_PK] = @international_code_PK
END
