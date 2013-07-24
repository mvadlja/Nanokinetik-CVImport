
-- GetEntity
CREATE PROCEDURE [dbo].[proc_MOIETY_GetEntity]
	@moiety_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[moiety_PK], [moiety_role], [moiety_name], [moiety_id], [amount_type], [amount_FK]
	FROM [dbo].[MOIETY]
	WHERE [moiety_PK] = @moiety_PK
END
