
-- GetEntities
CREATE PROCEDURE [dbo].[proc_PROPERTY_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[property_PK], [property_type], [property_name], [substance_id], [substance_name], [amount_type], [amount_FK]
	FROM [dbo].[PROPERTY]
END
