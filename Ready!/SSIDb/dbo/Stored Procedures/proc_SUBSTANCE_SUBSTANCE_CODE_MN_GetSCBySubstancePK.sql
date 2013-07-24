
-- GetEntities
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_SUBSTANCE_CODE_MN_GetSCBySubstancePK]
@SubstancePK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT sn.*
	FROM [dbo].[SUBSTANCE_SUBSTANCE_CODE_MN]
	left join [dbo].[SUBSTANCE_CODE] sn on sn.substance_code_PK = [dbo].[SUBSTANCE_SUBSTANCE_CODE_MN].substance_code_FK
	where [dbo].[SUBSTANCE_SUBSTANCE_CODE_MN].substance_FK = @SubstancePK

END
