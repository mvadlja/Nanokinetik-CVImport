-- Delete
CREATE PROCEDURE  [dbo].[proc_SU_AGENCY_MN_Delete]
	@su_agency_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SU_AGENCY_MN] WHERE [su_agency_mn_PK] = @su_agency_mn_PK
END
