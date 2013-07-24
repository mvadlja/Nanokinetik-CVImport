
-- Save
CREATE PROCEDURE [dbo].[proc_GENE_Save]
	@gene_PK int = NULL,
	@gene_sequence_origin varchar(4000) = NULL,
	@gene_id varchar(12) = NULL,
	@gene_name varchar(4000) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[GENE]
	SET
	[gene_sequence_origin] = @gene_sequence_origin,
	[gene_id] = @gene_id,
	[gene_name] = @gene_name
	WHERE [gene_PK] = @gene_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[GENE]
		([gene_sequence_origin], [gene_id], [gene_name])
		VALUES
		(@gene_sequence_origin, @gene_id, @gene_name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
