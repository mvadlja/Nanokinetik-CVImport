
-- GetEntities
CREATE PROCEDURE [dbo].[proc_STRUCT_REPRES_ATTACHMENT_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[struct_repres_attach_PK], [Id], [disk_file], [attachmentname], [filetype], [userID]
	FROM [dbo].[STRUCT_REPRES_ATTACHMENT]
END
