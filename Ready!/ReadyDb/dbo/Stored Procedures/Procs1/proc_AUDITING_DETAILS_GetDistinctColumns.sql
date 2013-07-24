-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_AUDITING_DETAILS_GetDistinctColumns]
	@db_name [nvarchar](200) = NULL,
	@table_name [nvarchar](200) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT [column_name] FROM AUDITING_DETAILS
	LEFT JOIN AUDITING_MASTER ON AUDITING_MASTER.auditing_master_PK = AUDITING_DETAILS.master_ID
	WHERE AUDITING_MASTER.[db_name] = @db_name
	AND AUDITING_MASTER.[table_name] = @table_name
END
