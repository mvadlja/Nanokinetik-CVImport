-- GetEntity
CREATE PROCEDURE  [dbo].[proc_SUBSTANCESSI_GetEntity]
	@substancessis_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substancessis_PK], [valid_according_ssi], [ssi_FK]
	FROM [dbo].[SUBSTANCESSI]
	WHERE [substancessis_PK] = @substancessis_PK
END
