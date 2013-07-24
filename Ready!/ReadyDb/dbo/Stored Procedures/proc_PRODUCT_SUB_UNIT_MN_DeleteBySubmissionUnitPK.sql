
create PROCEDURE  [dbo].[proc_PRODUCT_SUB_UNIT_MN_DeleteBySubmissionUnitPK]
	@submissionUnitPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_SUB_UNIT_MN] WHERE submission_unit_FK = @submissionUnitPk;
END