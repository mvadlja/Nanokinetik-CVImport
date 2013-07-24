-- GetUserByUsername
CREATE PROCEDURE  [dbo].[proc_USER_GetUserByUsername]
	@UserName nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT [dbo].[USER].[user_PK], [dbo].[USER].[person_FK], [dbo].[USER].[username], [dbo].[USER].[password], [dbo].[USER].[user_start_date], [dbo].[USER].[user_end_date], [dbo].[USER].[country_FK], [dbo].[USER].[description], [dbo].[USER].[email], [dbo].[USER].[active],[USER].isAdUser, [USER].adDomain
	FROM [dbo].[USER]
	WHERE [dbo].[USER].[username] = @UserName

END
