
-- Save
CREATE PROCEDURE [dbo].[proc_ISOTOPE_Save]
	@isotope_PK int = NULL,
	@nuclide_id varchar(12) = NULL,
	@nuclide_name varchar(4000) = NULL,
	@substitution_type varchar(250) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ISOTOPE]
	SET
	[nuclide_id] = @nuclide_id,
	[nuclide_name] = @nuclide_name,
	[substitution_type] = @substitution_type
	WHERE [isotope_PK] = @isotope_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ISOTOPE]
		([nuclide_id], [nuclide_name], [substitution_type])
		VALUES
		(@nuclide_id, @nuclide_name, @substitution_type)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
