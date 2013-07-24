
-- GetEntities
CREATE PROCEDURE [dbo].[proc_MOIETY_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[moiety_PK], [moiety_role], [moiety_name], [moiety_id], [amount_type], [amount_FK]
	FROM [dbo].[MOIETY]
END
