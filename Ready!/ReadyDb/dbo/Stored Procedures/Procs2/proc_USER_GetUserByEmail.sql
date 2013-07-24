-- GetUserByEmail
CREATE PROCEDURE  [dbo].[proc_USER_GetUserByEmail]
	@Email nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT [dbo].[USER].[user_PK], [dbo].[USER].[username], [dbo].[USER].[password], [dbo].[USER].[country_FK], [dbo].[USER].[description],[dbo].[USER].[email], [dbo].[USER].[active],[USER].isAdUser, [USER].adDomain
	FROM [dbo].[USER]
	WHERE [dbo].[USER].[Email] = @Email

END
