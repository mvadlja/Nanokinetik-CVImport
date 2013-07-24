
-- GetEntity
CREATE PROCEDURE [dbo].[proc_PROPERTY_GetEntity]
	@property_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[property_PK], [property_type], [property_name], [substance_id], [substance_name], [amount_type], [amount_FK]
	FROM [dbo].[PROPERTY]
	WHERE [property_PK] = @property_PK
END
