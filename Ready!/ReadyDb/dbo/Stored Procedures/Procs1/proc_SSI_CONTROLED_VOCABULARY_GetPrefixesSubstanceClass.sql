﻿-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SSI_CONTROLED_VOCABULARY_GetPrefixesSubstanceClass]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[SSI_CONTROLED_VOCABULARY].*
	FROM [dbo].[SSI_CONTROLED_VOCABULARY]
	WHERE [dbo].[SSI_CONTROLED_VOCABULARY].list_name = 'Substance Class' ORDER BY [dbo].[SSI_CONTROLED_VOCABULARY].term_name_english
END
