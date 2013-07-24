
-- Delete
CREATE PROCEDURE [dbo].[proc_STRUCT_REPRES_ATTACHMENT_Delete]
	@struct_repres_attach_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[STRUCT_REPRES_ATTACHMENT] WHERE [struct_repres_attach_PK] = @struct_repres_attach_PK
END
