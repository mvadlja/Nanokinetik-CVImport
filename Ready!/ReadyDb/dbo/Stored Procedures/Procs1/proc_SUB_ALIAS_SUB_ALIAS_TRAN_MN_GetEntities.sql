-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SUB_ALIAS_SUB_ALIAS_TRAN_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[sub_alias_sub_alias_tran_PK], [sub_alias_FK], [sub_alias_tran_FK]
	FROM [dbo].[SUB_ALIAS_SUB_ALIAS_TRAN_MN]
END
