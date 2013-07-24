CREATE FUNCTION [dbo].[ReturnMEDDRAForAP]

(
	@ap_FK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @meddra nvarchar(max)

	SELECT @meddra = COALESCE(@meddra + ', ', '') +
			isnull(rtrim(ltrim( '<' + 
								CONVERT(VARCHAR(20), (SELECT name FROM [dbo].[TYPE] WHERE type_PK=m.version_type_FK)) + 
								'>, ' + 
								CONVERT(VARCHAR(20), (SELECT NAME FROM [dbo].[TYPE] WHERE type_PK=m.level_type_FK)) + 
								', ' + 
								CONVERT(VARCHAR(20),m.code) + 
								', ' + 
								CONVERT(VARCHAR(max),m.term )
								)), '')
			from dbo.MEDDRA as m
			JOIN dbo.MEDDRA_AP_MN ON (dbo.MEDDRA_AP_MN.ap_FK=@ap_FK AND dbo.MEDDRA_AP_MN.meddra_FK=m.meddra_pk)
	
    RETURN @meddra
    
  END
