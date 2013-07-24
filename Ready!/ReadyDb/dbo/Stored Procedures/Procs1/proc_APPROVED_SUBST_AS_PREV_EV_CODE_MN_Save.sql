﻿-- Save
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_AS_PREV_EV_CODE_MN_Save]
	@approved_subst_prev_ev_code_PK int = NULL,
	@approved_substance_FK int = NULL,
	@as_previous_ev_code_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[APPROVED_SUBST_AS_PREV_EV_CODE_MN]
	SET
	[approved_substance_FK] = @approved_substance_FK,
	[as_previous_ev_code_FK] = @as_previous_ev_code_FK
	WHERE [approved_subst_prev_ev_code_PK] = @approved_subst_prev_ev_code_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[APPROVED_SUBST_AS_PREV_EV_CODE_MN]
		([approved_substance_FK], [as_previous_ev_code_FK])
		VALUES
		(@approved_substance_FK, @as_previous_ev_code_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
