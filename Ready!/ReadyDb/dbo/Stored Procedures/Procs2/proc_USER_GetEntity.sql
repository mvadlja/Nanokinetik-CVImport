-- GetEntity
CREATE PROCEDURE  [dbo].[proc_USER_GetEntity]
	@user_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[user_PK], [person_FK], [username], [password], [user_start_date], [user_end_date], [country_FK], [description], [email], [active],[USER].isAdUser, [USER].adDomain
	FROM [dbo].[USER]
	WHERE [user_PK] = @user_PK
END
