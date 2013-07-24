﻿-- GetEntity
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBST_ALIAS_TRANS_MN_GetEntity]
	@approved_subst_sub_alias_trans_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[approved_subst_sub_alias_trans_PK], [approved_substance_FK], [substance_alias_translation_FK]
	FROM [dbo].[APPROVED_SUBST_SUBST_ALIAS_TRANS_MN]
	WHERE [approved_subst_sub_alias_trans_PK] = @approved_subst_sub_alias_trans_PK
END
