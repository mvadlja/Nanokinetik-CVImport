-- GetEntities
CREATE PROCEDURE  [dbo].[proc_AS_PREVIOUS_EV_CODE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[as_previous_ev_code_PK], [devevcode]
	FROM [dbo].[AS_PREVIOUS_EV_CODE]
END
