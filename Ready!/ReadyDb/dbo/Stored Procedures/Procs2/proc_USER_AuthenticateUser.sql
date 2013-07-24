-- AuthenticateUser
CREATE PROCEDURE  [dbo].[proc_USER_AuthenticateUser]
	@UserName nvarchar(50) = NULL,
	@Password nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT [dbo].[USER].[user_PK], [dbo].[USER].[username], [dbo].[USER].[password], [dbo].[USER].[country_FK], [dbo].[USER].[description],[dbo].[USER].[email], [dbo].[USER].[active],[dbo].[USER].isAdUser,[dbo].[USER].adDomain
	FROM [dbo].[USER]
	WHERE [dbo].[USER].[username] = @UserName
	AND [dbo].[USER].[password] = @Password

END
