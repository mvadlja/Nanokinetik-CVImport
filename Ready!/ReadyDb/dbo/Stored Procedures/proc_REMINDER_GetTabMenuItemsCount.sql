-- GetEntities
CREATE PROCEDURE  [dbo].[proc_REMINDER_GetTabMenuItemsCount]
	@ReminderPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @AuditRecordsCount NVARCHAR(MAX) = NULL;

	select @AuditRecordsCount = count (distinct SessionToken)
	from AuditingMaster am
	left join [AuditingDetails] ad on am.IDAuditingMaster = ad.MasterID		
	where am.TableName = 'REMINDER' and ad.PKValue = CAST (@ReminderPk AS NVARCHAR(MAX));	
	
	SELECT 
			(select @AuditRecordsCount ) as 'AlertAuditTrailList'
			
END