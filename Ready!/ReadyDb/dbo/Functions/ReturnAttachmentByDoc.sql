CREATE FUNCTION [dbo].[ReturnAttachmentByDoc]
(
	@document_FK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @attachment nvarchar(max)

	SELECT @attachment = COALESCE(@attachment + ', ', '') +
			isnull(rtrim(ltrim(att.attachmentname)) , '')
			FROM [dbo].[ATTACHMENT] att
			WHERE (att.[document_FK] = @document_FK)
			
	ORDER BY att.attachmentname
	
    RETURN isnull(@attachment,'')
    
  END
