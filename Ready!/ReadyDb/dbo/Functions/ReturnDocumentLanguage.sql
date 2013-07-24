CREATE FUNCTION [dbo].[ReturnDocumentLanguage]

(
	@document_PK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @language_code nvarchar(max)

	SELECT @language_code = COALESCE(@language_code + ', ', '') +
			isnull(rtrim(ltrim(l.code)), '')
			
	FROM dbo.LANGUAGE_CODE l

	LEFT JOIN dbo.DOCUMENT_LANGUAGE_MN dl
	ON dl.language_FK = l.languagecode_PK
	where dl.document_FK = @document_PK

	ORDER BY l.name
  
    RETURN @language_code
    
  END
