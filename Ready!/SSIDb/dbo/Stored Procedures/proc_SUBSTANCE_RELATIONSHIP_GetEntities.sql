
-- GetEntities
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_RELATIONSHIP_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_relationship_PK], [relationship], [interaction_type], [substance_id], [substance_name], [amount_type], [amount_FK]
	FROM [dbo].[SUBSTANCE_RELATIONSHIP]
END
