CREATE FUNCTION [dbo].[ReturnAttachmentForSU]

(
	@sUnitDoc_FK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @attachment nvarchar(max)

	SELECT @attachment = COALESCE(@attachment + ', ', '') +
			isnull(rtrim(ltrim(atch.attachmentname)) , '')
			from dbo.ATTACHMENT as atch
			JOIN DOCUMENT as doc on doc.document_PK=atch.document_FK
			where doc.document_PK=@sUnitDoc_FK
	ORDER BY atch.attachmentname
	
    RETURN @attachment
    
  END
