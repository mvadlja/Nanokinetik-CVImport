
create PROCEDURE  [dbo].[proc_SU_AGENCY_MN_DeleteBySubmissionUnitPK]
	@submissionUnitPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].SU_AGENCY_MN WHERE submission_unit_FK = @submissionUnitPk;
END