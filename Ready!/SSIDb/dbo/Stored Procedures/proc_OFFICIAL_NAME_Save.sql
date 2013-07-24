
-- Save
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_Save]
	@official_name_PK int = NULL,
	@on_type_FK int = NULL,
	@on_status_FK int = NULL,
	@on_status_changedate varchar(10) = NULL,
	@on_jurisdiction_FK int = NULL,
	@on_domain_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[OFFICIAL_NAME]
	SET
	[on_type_FK] = @on_type_FK,
	[on_status_FK] = @on_status_FK,
	[on_status_changedate] = @on_status_changedate,
	[on_jurisdiction_FK] = @on_jurisdiction_FK,
	[on_domain_FK] = @on_domain_FK
	WHERE [official_name_PK] = @official_name_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[OFFICIAL_NAME]
		([on_type_FK], [on_status_FK], [on_status_changedate], [on_jurisdiction_FK], [on_domain_FK])
		VALUES
		(@on_type_FK, @on_status_FK, @on_status_changedate, @on_jurisdiction_FK, @on_domain_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
