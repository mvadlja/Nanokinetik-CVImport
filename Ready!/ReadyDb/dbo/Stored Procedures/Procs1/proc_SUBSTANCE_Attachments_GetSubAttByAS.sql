-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_Attachments_GetSubAttByAS]
@AS_PK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT subAtt.*
	FROM [dbo].[APPROVED_SUBST_SUBST_ATT_MN] as attMN
	left join [dbo].SUBSTANCE_ATTACHMENT subAtt on subAtt.substance_attachment_PK= attMN.substance_attachment_FK
	where attMN.approved_substance_FK=@AS_PK

END
