-- GetEntities
CREATE PROCEDURE  [dbo].[proc_Pharmaceutical_Product_GetTabMenuItemsCount]
	@pp_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @AuditRecordsCount NVARCHAR(MAX) = NULL;

	select @AuditRecordsCount = count (distinct SessionToken)
	from AuditingMaster am
	left join [AuditingDetails] ad on am.IDAuditingMaster = ad.MasterID		
	where am.TableName = 'PHARMACEUTICAL_PRODUCT' and ad.PKValue = CAST (@pp_PK AS NVARCHAR(MAX));	
	
	SELECT 
			(select count(DISTINCT pd.doc_FK) from dbo.[PP_DOCUMENT_MN] pd where pd.pp_FK = @pp_PK) as 'PharmProdDocList',
			(select @AuditRecordsCount ) as 'PharmProdAuditTrailList'
			
END
