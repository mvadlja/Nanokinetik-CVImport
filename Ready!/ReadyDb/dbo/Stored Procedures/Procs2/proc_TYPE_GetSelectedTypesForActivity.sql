-- dasdas
CREATE PROCEDURE  [dbo].[proc_TYPE_GetSelectedTypesForActivity]
	@activity_pk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[TYPE].*
	FROM [dbo].[TYPE]
	WHERE [dbo].[TYPE].[group] = 'A'
	and [dbo].[TYPE].[type_PK] IN
	(
		select type_FK from dbo.ACTIVITY_TYPE_MN
		where activity_FK = @activity_pk
	)

END
