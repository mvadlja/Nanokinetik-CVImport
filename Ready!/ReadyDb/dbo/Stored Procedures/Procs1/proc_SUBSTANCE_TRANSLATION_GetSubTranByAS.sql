-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_TRANSLATION_GetSubTranByAS]
@AS_PK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT subtran.*
	FROM [dbo].[APPROVED_SUBST_SUBST_TRANS_MN]
	left join [dbo].[SUBSTANCE_TRANSLATION] subtran on subtran.substance_translations_PK= [dbo].[APPROVED_SUBST_SUBST_TRANS_MN].substance_translations_FK
	where [dbo].[APPROVED_SUBST_SUBST_TRANS_MN].approved_substance_FK=@AS_PK

END
