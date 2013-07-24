
-- Save
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_Save]
	@substance_s_PK int = NULL,
	@language int = NULL,
	@substance_id int = NULL,
	@substance_class int = NULL,
	@ref_info_FK int = NULL,
	@sing_FK int = NULL,
	@responsible_user_FK int = NULL,
	@description varchar(2000) = NULL,
	@comments varchar(250) = NULL,
	@name varchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBSTANCE]
	SET
	[language] = @language,
	[substance_id] = @substance_id,
	[substance_class] = @substance_class,
	[ref_info_FK] = @ref_info_FK,
	[sing_FK] = @sing_FK,
	[responsible_user_FK] = @responsible_user_FK,
	[description] = @description,
	[comments] = @comments,
	[name] = @name
	WHERE [substance_s_PK] = @substance_s_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBSTANCE]
		([language], [substance_id], [substance_class], [ref_info_FK], [sing_FK], [responsible_user_FK], [description], [comments], [name])
		VALUES
		(@language, @substance_id, @substance_class, @ref_info_FK, @sing_FK, @responsible_user_FK, @description, @comments, @name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
