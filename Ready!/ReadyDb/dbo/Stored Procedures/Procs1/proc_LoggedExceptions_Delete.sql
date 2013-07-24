-- Delete
CREATE PROCEDURE  [dbo].[proc_LoggedExceptions_Delete]
	@IDLoggedException int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[LoggedExceptions] WHERE [IDLoggedException] = @IDLoggedException
END
