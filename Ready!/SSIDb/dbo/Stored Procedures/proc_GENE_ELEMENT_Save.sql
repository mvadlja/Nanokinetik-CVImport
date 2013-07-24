
-- Save
CREATE PROCEDURE [dbo].[proc_GENE_ELEMENT_Save]
	@gene_element_PK int = NULL,
	@ge_type varchar(250) = NULL,
	@ge_id varchar(12) = NULL,
	@ge_name varchar(250) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[GENE_ELEMENT]
	SET
	[ge_type] = @ge_type,
	[ge_id] = @ge_id,
	[ge_name] = @ge_name
	WHERE [gene_element_PK] = @gene_element_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[GENE_ELEMENT]
		([ge_type], [ge_id], [ge_name])
		VALUES
		(@ge_type, @ge_id, @ge_name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
