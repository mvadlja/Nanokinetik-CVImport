-----------------------------------------------
CREATE PROCEDURE  [dbo].[proc_AUDIT_GetRecordVersions]
	@PKValue int = 8250, --2147483647,		
	@table_name nvarchar(100) = 'AUTHORISED_PRODUCT'
	
AS
BEGIN
	SET NOCOUNT ON;
 
	with audit_records as
	(
		select
			MIN(am.IDAuditingMaster) IDAuditingMaster,
			MIN(am.Date) as ChangeDate,
			MIN(am.Username) as Username,
			SessionToken as SessionToken
		from AuditingMaster am
			inner join [AuditingDetails] ad on am.IDAuditingMaster = ad.MasterID		
		where am.TableName = @table_name 
		      and ad.PKValue = @PKValue 	
		      and ((ad.NewValue IS NOT NULL AND ad.NewValue != '') OR (ad.OldValue IS NOT NULL AND ad.OldValue != ''))
			  and ColumnName not like '%_PK'
			  --and ad.ColumnName not like '%_FK'
			  --and ad.ColumnName not like '%_PK'
		group by SessionToken
	)
	select ROW_NUMBER() over (order by ChangeDate) as [Version], ChangeDate, Username, SessionToken, IDAuditingMaster
	from audit_records
    

	
	
END
