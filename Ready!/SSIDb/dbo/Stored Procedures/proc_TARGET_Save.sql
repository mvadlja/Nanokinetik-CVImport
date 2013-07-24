
-- Save
CREATE PROCEDURE [dbo].[proc_TARGET_Save]
	@target_PK int = NULL,
	@target_gene_id varchar(12) = NULL,
	@target_gene_name varchar(4000) = NULL,
	@interaction_type varchar(250) = NULL,
	@target_organism_type varchar(250) = NULL,
	@target_type varchar(250) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[TARGET]
	SET
	[target_gene_id] = @target_gene_id,
	[target_gene_name] = @target_gene_name,
	[interaction_type] = @interaction_type,
	[target_organism_type] = @target_organism_type,
	[target_type] = @target_type
	WHERE [target_PK] = @target_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[TARGET]
		([target_gene_id], [target_gene_name], [interaction_type], [target_organism_type], [target_type])
		VALUES
		(@target_gene_id, @target_gene_name, @interaction_type, @target_organism_type, @target_type)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
