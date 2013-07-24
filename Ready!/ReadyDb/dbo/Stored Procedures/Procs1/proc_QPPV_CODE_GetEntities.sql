-- GetEntities
create PROCEDURE [dbo].[proc_QPPV_CODE_GetEntities]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[qppv_code_PK], [person_FK], [qppv_code]
	FROM [dbo].[QPPV_CODE]
END
