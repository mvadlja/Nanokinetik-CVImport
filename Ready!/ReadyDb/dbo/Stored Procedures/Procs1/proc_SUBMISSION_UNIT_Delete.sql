-- Delete
CREATE PROCEDURE  [dbo].[proc_SUBMISSION_UNIT_Delete]
	@subbmission_unit_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBMISSION_UNIT] WHERE [subbmission_unit_PK] = @subbmission_unit_PK
END
