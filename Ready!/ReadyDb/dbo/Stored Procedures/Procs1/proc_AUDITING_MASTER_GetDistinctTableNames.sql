-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_AUDITING_MASTER_GetDistinctTableNames]
	@db_name [nvarchar](200) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT [table_name] FROM AUDITING_MASTER
	WHERE [db_name] = @db_name
END
