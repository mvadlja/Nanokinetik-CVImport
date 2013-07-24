-- GetEntities
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_AS_PREV_EV_CODE_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[approved_subst_prev_ev_code_PK], [approved_substance_FK], [as_previous_ev_code_FK]
	FROM [dbo].[APPROVED_SUBST_AS_PREV_EV_CODE_MN]
END
