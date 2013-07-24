
-- GetEntities
CREATE PROCEDURE [dbo].[proc_AMOUNT_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[amount_PK], [quantity], [lownumvalue], [lownumunit], [lownumprefix], [lowdenomvalue], [lowdenomunit], [lowdenomprefix], [highnumvalue], [highnumunit], [highnumprefix], [highdenomvalue], [highdenomunit], [highdenomprefix], [average], [prefix], [unit], [nonnumericvalue]
	FROM [dbo].[AMOUNT]
END
