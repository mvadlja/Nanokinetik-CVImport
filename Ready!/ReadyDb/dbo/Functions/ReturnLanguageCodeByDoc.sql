create FUNCTION [dbo].[ReturnLanguageCodeByDoc]
(
	@document_FK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @attachment nvarchar(max)

	SELECT @attachment = COALESCE(@attachment + ', ', '') +
			isnull(rtrim(ltrim(languageCode.code)) , '')
				FROM [dbo].[LANGUAGE_CODE] languageCode
				LEFT JOIN [dbo].[DOCUMENT_LANGUAGE_MN] ON languageCode.languagecode_PK = DOCUMENT_LANGUAGE_MN.language_FK 
				WHERE ([dbo].[DOCUMENT_LANGUAGE_MN].[document_FK]  = @document_FK OR @document_FK IS NULL)
				
    RETURN @attachment
    
  END
