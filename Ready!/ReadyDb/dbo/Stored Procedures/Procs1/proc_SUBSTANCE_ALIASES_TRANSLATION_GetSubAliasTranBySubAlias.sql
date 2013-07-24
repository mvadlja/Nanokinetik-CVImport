-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_ALIASES_TRANSLATION_GetSubAliasTranBySubAlias]
@alias_PK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT subAliasesTranslation.*
	FROM [dbo].[SUB_ALIAS_SUB_ALIAS_TRAN_MN] as tranMN
	left join [dbo].SUBSTANCE_ALIAS_TRANSLATION subAliasesTranslation on subAliasesTranslation.substance_alias_translation_PK = tranMN.sub_alias_tran_FK
	where tranMN.sub_alias_FK = @alias_PK

END
