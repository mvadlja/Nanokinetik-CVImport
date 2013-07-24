-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_TRANSLATION_GetPrevCodeByAS]
@AS_PK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT prevCode.*
	FROM [dbo].[APPROVED_SUBST_AS_PREV_EV_CODE_MN] as prevMN
	left join [dbo].AS_PREVIOUS_EV_CODE prevCode on prevCode.as_previous_ev_code_PK= prevMN.as_previous_ev_code_FK
	where prevMN.approved_substance_FK=@AS_PK

END
