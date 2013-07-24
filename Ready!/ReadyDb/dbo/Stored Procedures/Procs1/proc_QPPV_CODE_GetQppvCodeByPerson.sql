-- GetTypesByActivity
CREATE PROCEDURE  [dbo].[proc_QPPV_CODE_GetQppvCodeByPerson]
	@person_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT qppv.qppv_code_PK, qppv.person_FK, qppv.qppv_code
	FROM [dbo].[QPPV_CODE] qppv
	 JOIN [dbo].[PERSON] person ON person.person_PK = qppv.person_FK
	 WHERE qppv.person_FK = @person_FK;

END
