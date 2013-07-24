-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_FORM_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[pharmaceutical_form_PK], [name], [ev_code]
	FROM [dbo].[PHARMACEUTICAL_FORM]
	WHERE deleted ='False'
END
