
-- Delete
CREATE PROCEDURE [dbo].[proc_GENE_ELEMENT_Delete]
	@gene_element_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[GENE_ELEMENT] WHERE [gene_element_PK] = @gene_element_PK
END
