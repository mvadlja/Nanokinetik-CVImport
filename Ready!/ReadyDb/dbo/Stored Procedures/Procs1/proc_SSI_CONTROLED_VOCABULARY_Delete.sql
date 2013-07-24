-- Delete
CREATE PROCEDURE  [dbo].[proc_SSI_CONTROLED_VOCABULARY_Delete]
	@ssi__cont_voc_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SSI_CONTROLED_VOCABULARY] WHERE [ssi__cont_voc_PK] = @ssi__cont_voc_PK
END
