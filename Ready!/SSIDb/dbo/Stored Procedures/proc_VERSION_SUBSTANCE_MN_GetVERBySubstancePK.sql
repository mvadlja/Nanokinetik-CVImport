
-- GetEntities
create PROCEDURE [dbo].[proc_VERSION_SUBSTANCE_MN_GetVERBySubstancePK]
@SubstancePK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ver.*
	FROM [dbo].[VERSION_SUBSTANCE_MN]
	left join [dbo].[VERSION] ver on ver.version_PK = [dbo].[VERSION_SUBSTANCE_MN].version_FK
	where [dbo].[VERSION_SUBSTANCE_MN].substance_FK = @SubstancePK

END
