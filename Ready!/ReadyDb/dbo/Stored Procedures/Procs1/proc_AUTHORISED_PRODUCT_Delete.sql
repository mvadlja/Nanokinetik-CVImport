﻿-- Delete
CREATE PROCEDURE  [dbo].[proc_AUTHORISED_PRODUCT_Delete]
	@ap_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[AUTHORISED_PRODUCT] WHERE [ap_PK] = @ap_PK
END
