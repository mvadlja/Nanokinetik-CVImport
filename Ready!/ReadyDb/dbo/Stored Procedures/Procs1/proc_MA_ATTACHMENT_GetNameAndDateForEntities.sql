-- GetEntities
create PROCEDURE [dbo].[proc_MA_ATTACHMENT_GetNameAndDateForEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	 [file_name], [last_change]
	FROM [dbo].[MA_ATTACHMENT]
END
