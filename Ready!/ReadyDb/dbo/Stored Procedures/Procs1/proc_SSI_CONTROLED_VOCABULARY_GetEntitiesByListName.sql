﻿-- GetEntitiesByListName
CREATE PROCEDURE  [dbo].[proc_SSI_CONTROLED_VOCABULARY_GetEntitiesByListName]
	@list_name nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ssi__cont_voc_PK], [list_name], [term_id], [term_name_english], [latin_name_latin], [synonim1], [synonim2], [Description], [Field8], [Field9], [Field10], [Field11], [Field12], [Field13], [Field14], [evcode]
	FROM [dbo].[SSI_CONTROLED_VOCABULARY]
	WHERE list_name = @list_name
END
