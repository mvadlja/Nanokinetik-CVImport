-- Save
CREATE PROCEDURE [dbo].[proc_TYPE_Save]
	@type_PK int = NULL,
	@name nvarchar(100) = NULL,
	@group nvarchar(20) = NULL,
	@entity_related nvarchar(50) = NULL,
	@form_related nvarchar(50) = NULL,
	@type nvarchar(50) = NULL,
	@description nvarchar(MAX) = NULL,
	@group_description nvarchar(50) = NULL,
	@ev_code nvarchar(20) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[TYPE]
	SET
	[name] = @name,
	[group] = @group,
	[entity_related] = @entity_related,
	[form_related] = @form_related,
	[type] = @type,
	[description] = @description,
	[group_description] = @group_description,
	[ev_code] = @ev_code
	WHERE [type_PK] = @type_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[TYPE]
		([name], [group], [entity_related], [form_related], [type], [description],[group_description], [ev_code])
		VALUES
		(@name, @group, @entity_related, @form_related, @type, @description,@group_description, @ev_code)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
