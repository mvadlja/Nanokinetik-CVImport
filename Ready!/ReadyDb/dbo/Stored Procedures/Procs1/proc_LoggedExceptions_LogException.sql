-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_LoggedExceptions_LogException] 
	@Username [nvarchar](100) = NULL,
	@ExceptionType [nvarchar](200) = NULL,
	@ExceptionMessage [nvarchar](1000) = NULL,
	@TargetSite [nvarchar](1000) = NULL,
	@StackTrace [nvarchar](2000) = NULL,
	@Source [nvarchar](100) = NULL,
	@Date [datetime] = NULL,
	@ServerName [nvarchar](100) = NULL,
	@UniqueKey [uniqueidentifier] = NULL
AS
BEGIN
	DECLARE @ScopeIdentity int

	INSERT INTO [dbo].[LoggedExceptions]
	([Username], [ExceptionType], [ExceptionMessage], [TargetSite], [StackTrace], [Source], [Date], [ServerName], [UniqueKey])
	VALUES
	(@Username, @ExceptionType, @ExceptionMessage, @TargetSite, @StackTrace, @Source, @Date, @ServerName, @UniqueKey)
	
	SET @ScopeIdentity = SCOPE_IDENTITY()
	SELECT @ScopeIdentity AS ScopeIdentity
END
