
-- GetEntities
create PROCEDURE [dbo].[proc_TARGET_GetTargetByRIPK]
@RIPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT tar.*
	FROM [dbo].[RI_TARGET_MN]
	left join [dbo].[TARGET] tar on tar.target_PK = [dbo].[RI_TARGET_MN].target_FK
	where [dbo].[RI_TARGET_MN].ri_FK = @RIPK
	
END
