
-- GetEntities
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_RELATIONSHIP_GetRELByRIPK]
@RIPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT rel.*
	FROM [dbo].[SR_RI_MN]
	left join [dbo].[SUBSTANCE_RELATIONSHIP] rel on rel.substance_relationship_PK = [dbo].[SR_RI_MN].sr_FK
	where [dbo].[SR_RI_MN].ri_FK = @RIPK
	
END
