
-- GetEntities
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_s_PK], [language], [substance_id], [substance_class], [ref_info_FK], [sing_FK], [responsible_user_FK], [description], [comments], [name]
	FROM [dbo].[SUBSTANCE]
END
