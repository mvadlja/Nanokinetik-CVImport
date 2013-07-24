-- GetEntity
CREATE PROCEDURE  [dbo].[proc_AS_PREVIOUS_EV_CODE_GetEntity]
	@as_previous_ev_code_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[as_previous_ev_code_PK], [devevcode]
	FROM [dbo].[AS_PREVIOUS_EV_CODE]
	WHERE [as_previous_ev_code_PK] = @as_previous_ev_code_PK
END
