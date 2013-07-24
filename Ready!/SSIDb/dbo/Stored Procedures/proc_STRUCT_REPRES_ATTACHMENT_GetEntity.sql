
-- GetEntity
CREATE PROCEDURE [dbo].[proc_STRUCT_REPRES_ATTACHMENT_GetEntity]
	@struct_repres_attach_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[struct_repres_attach_PK], [Id], [disk_file], [attachmentname], [filetype], [userID]
	FROM [dbo].[STRUCT_REPRES_ATTACHMENT]
	WHERE [struct_repres_attach_PK] = @struct_repres_attach_PK
END
