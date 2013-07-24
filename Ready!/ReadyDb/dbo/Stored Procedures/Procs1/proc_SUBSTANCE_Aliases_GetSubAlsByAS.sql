-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_Aliases_GetSubAlsByAS]
@AS_PK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT subAls.*
	FROM [dbo].APPROVED_SUBST_SUBST_ALIAS_MN as alsMN
	left join [dbo].SUBSTANCE_ALIAS subAls on subAls.substance_alias_PK= alsMN.substance_alias_FK
	where alsMN.approved_substance_FK=@AS_PK

END
