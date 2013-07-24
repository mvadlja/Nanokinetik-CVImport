-- Save
CREATE PROCEDURE  [dbo].[proc_USER_Save]
	@user_PK int = NULL,
	@person_FK int = NULL,
	@username nvarchar(20) = NULL,
	@password nvarchar(100) = NULL,
	@user_start_date date = NULL,
	@user_end_date date = NULL,
	@country_FK int = NULL,
	@description nvarchar(MAX) = NULL,
	@email nvarchar(50) = NULL,
	@active bit = NULL,
	@isAdUser bit = NULL,
	@adDomain varchar(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[USER]
	SET
	[person_FK] = @person_FK,
	[username] = @username,
	[password] = @password,
	[user_start_date] = @user_start_date,
	[user_end_date] = @user_end_date,
	[country_FK] = @country_FK,
	[description] = @description,
	[email] = @email,
	[active] = @active,
	[isAdUser] = @isAdUser,
	[adDomain] = @adDomain 
	WHERE [user_PK] = @user_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[USER]
		([person_FK], [username], [password], [user_start_date], [user_end_date], [country_FK], [description], [email], [active],[isAdUser],[adDomain])
		VALUES
		(@person_FK, @username, @password, @user_start_date, @user_end_date, @country_FK, @description, @email, @active, @isAdUser,@adDomain )

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
