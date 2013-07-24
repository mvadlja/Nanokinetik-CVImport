-- Save
CREATE PROCEDURE  [dbo].[proc_SU_AGENCY_MN_Save]
	@su_agency_mn_PK int = NULL,
	@agency_FK int = NULL,
	@submission_unit_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SU_AGENCY_MN]
	SET
	[agency_FK] = @agency_FK,
	[submission_unit_FK] = @submission_unit_FK
	WHERE [su_agency_mn_PK] = @su_agency_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SU_AGENCY_MN]
		([agency_FK], [submission_unit_FK])
		VALUES
		(@agency_FK, @submission_unit_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
