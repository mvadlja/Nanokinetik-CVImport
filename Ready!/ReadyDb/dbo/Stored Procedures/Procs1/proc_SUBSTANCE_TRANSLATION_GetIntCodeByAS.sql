-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_TRANSLATION_GetIntCodeByAS]
@AS_PK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT intCode.*
	FROM [dbo].[APPROVED_SUBST_INTER_CODE_MN]
	left join [dbo].[INTERNATIONAL_CODE] intCode on intCode.international_code_PK= [dbo].[APPROVED_SUBST_INTER_CODE_MN].international_code_FK
	where [dbo].[APPROVED_SUBST_INTER_CODE_MN].approved_substance_FK=@AS_PK

END
