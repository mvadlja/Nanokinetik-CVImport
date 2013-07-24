-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SUBSTANCESSI_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substancessis_PK], [valid_according_ssi], [ssi_FK]
	FROM [dbo].[SUBSTANCESSI]
END
