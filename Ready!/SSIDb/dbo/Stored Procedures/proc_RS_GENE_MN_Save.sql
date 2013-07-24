
-- Save
CREATE PROCEDURE [dbo].[proc_RS_GENE_MN_Save]
	@rs_gene_mn_PK int = NULL,
	@rs_FK int = NULL,
	@gene_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[RS_GENE_MN]
	SET
	[rs_FK] = @rs_FK,
	[gene_FK] = @gene_FK
	WHERE [rs_gene_mn_PK] = @rs_gene_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[RS_GENE_MN]
		([rs_FK], [gene_FK])
		VALUES
		(@rs_FK, @gene_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
