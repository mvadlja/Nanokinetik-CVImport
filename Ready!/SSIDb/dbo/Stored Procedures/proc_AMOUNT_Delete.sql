
-- Delete
CREATE PROCEDURE [dbo].[proc_AMOUNT_Delete]
	@amount_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[AMOUNT] WHERE [amount_PK] = @amount_PK
END
