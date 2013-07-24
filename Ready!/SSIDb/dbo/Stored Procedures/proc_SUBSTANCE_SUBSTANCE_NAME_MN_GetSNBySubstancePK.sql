
-- GetEntities
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_SUBSTANCE_NAME_MN_GetSNBySubstancePK]
@SubstancePK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT sn.*
	FROM [dbo].[SUBSTANCE_SUBSTANCE_NAME_MN]
	left join [dbo].[SUBSTANCE_NAME] sn on sn.substance_name_PK = [dbo].[SUBSTANCE_SUBSTANCE_NAME_MN].substance_name_FK
	where [dbo].[SUBSTANCE_SUBSTANCE_NAME_MN].substance_FK = @SubstancePK

END
