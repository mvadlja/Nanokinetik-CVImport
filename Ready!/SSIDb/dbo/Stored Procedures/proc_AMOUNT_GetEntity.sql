
-- GetEntity
CREATE PROCEDURE [dbo].[proc_AMOUNT_GetEntity]
	@amount_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[amount_PK], [quantity], [lownumvalue], [lownumunit], [lownumprefix], [lowdenomvalue], [lowdenomunit], [lowdenomprefix], [highnumvalue], [highnumunit], [highnumprefix], [highdenomvalue], [highdenomunit], [highdenomprefix], [average], [prefix], [unit], [nonnumericvalue]
	FROM [dbo].[AMOUNT]
	WHERE [amount_PK] = @amount_PK
END
