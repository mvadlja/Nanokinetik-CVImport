
-- GetEntities
create PROCEDURE [dbo].[proc_SN_ON_MN_GetONBySNPK]
@SNPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ofn.*
	FROM [dbo].[SN_ON_MN]
	left join [dbo].[OFFICIAL_NAME] ofn on ofn.official_name_PK = [dbo].[SN_ON_MN].official_name_FK
	where [dbo].[SN_ON_MN].substance_name_FK = @SNPK
END
