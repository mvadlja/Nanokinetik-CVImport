-- GetEntities
CREATE PROCEDURE  [dbo].[proc_LoggedExceptions_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[IDLoggedException], [Username], [ExceptionType], [ExceptionMessage], [TargetSite], [StackTrace], [Source], [Date], [ServerName], [UniqueKey]
	FROM [dbo].[LoggedExceptions]
END
