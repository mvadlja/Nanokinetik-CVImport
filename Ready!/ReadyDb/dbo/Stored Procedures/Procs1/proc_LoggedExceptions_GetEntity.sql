-- GetEntity
CREATE PROCEDURE  [dbo].[proc_LoggedExceptions_GetEntity]
	@IDLoggedException int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[IDLoggedException], [Username], [ExceptionType], [ExceptionMessage], [TargetSite], [StackTrace], [Source], [Date], [ServerName], [UniqueKey]
	FROM [dbo].[LoggedExceptions]
	WHERE [IDLoggedException] = @IDLoggedException
END
