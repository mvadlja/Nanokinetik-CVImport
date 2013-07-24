CREATE FUNCTION [dbo].[ReturnPPIDocumentForAP]

(
	@ap_PK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @docName nvarchar(max)

	SELECT @docName = COALESCE(@docName + ', ', '') +
			isnull(rtrim(ltrim(d.name)), '')
			
	FROM[dbo].[AP_DOCUMENT_MN]
		LEFT JOIN [dbo].[DOCUMENT] d ON d.document_PK = [dbo].[AP_DOCUMENT_MN].[document_FK]
		WHERE ([dbo].[AP_DOCUMENT_MN].[ap_FK] = @ap_PK) AND ((select name from [TYPE] where type_PK=d.type_FK) IN ('ppi'))
  
    RETURN @docName
    
  END
