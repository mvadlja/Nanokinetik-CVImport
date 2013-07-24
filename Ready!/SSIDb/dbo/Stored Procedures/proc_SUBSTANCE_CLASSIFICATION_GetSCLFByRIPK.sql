
-- GetEntities
create PROCEDURE [dbo].[proc_SUBSTANCE_CLASSIFICATION_GetSCLFByRIPK]
@RIPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT sclf.*
	FROM [dbo].[RI_SCLF_MN]
	left join [dbo].[SUBSTANCE_CLASSIFICATION] sclf on sclf.subst_clf_PK = [dbo].[RI_SCLF_MN].sclf_FK
	where [dbo].[RI_SCLF_MN].ref_info_FK = @RIPK
	
END
