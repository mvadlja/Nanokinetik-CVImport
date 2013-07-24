
-- Save
CREATE PROCEDURE [dbo].[proc_RI_GENE_MN_Save]
	@ri_gene_mn_PK int = NULL,
	@ri_FK int = NULL,
	@gene_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[RI_GENE_MN]
	SET
	[ri_FK] = @ri_FK,
	[gene_FK] = @gene_FK
	WHERE [ri_gene_mn_PK] = @ri_gene_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[RI_GENE_MN]
		([ri_FK], [gene_FK])
		VALUES
		(@ri_FK, @gene_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
