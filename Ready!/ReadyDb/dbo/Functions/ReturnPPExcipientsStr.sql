CREATE FUNCTION [dbo].[ReturnPPExcipientsStr]

(
	@pp_PK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @ExcipientsStr nvarchar(max)

	SELECT @ExcipientsStr = COALESCE(@ExcipientsStr + ', ', '') +
			isnull(rtrim(ltrim(
		CASE
			WHEN ppexc.concise IS NULL OR ppexc.concise = '' THEN ss.substance_name
		--	ELSE ss.substance_name +' ' +ppexc.concise
			ELSE ppexc.concise
		END
     )), '')     
			from dbo.PP_EXCIPIENT ppexc
			left join dbo.SUBSTANCES ss on ppexc.substancecode_FK=ss.substance_PK
			LEFT JOIN [dbo].[PHARMACEUTICAL_PRODUCT] pp ON pp.pharmaceutical_product_PK = ppexc.pp_FK
			where pp.pharmaceutical_product_PK = @pp_PK
	
    RETURN @ExcipientsStr
    
  END
