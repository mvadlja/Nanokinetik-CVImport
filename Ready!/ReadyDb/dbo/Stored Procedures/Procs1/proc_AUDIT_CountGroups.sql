-----------------------------------------------
CREATE PROCEDURE  [dbo].[proc_AUDIT_CountGroups]
	@PKValue int = 8250, --2147483647,		
	@table_name nvarchar(100) = 'AUTHORISED_PRODUCT'
	
AS
BEGIN
	SET NOCOUNT ON;
	
	--with audit_records as
	--(
	--select		
	--	am.Date as ChangeDate		
	--from AuditingMaster am
	--	inner join [AuditingDetails] ad on am.IDAuditingMaster = ad.MasterID
	
	--where am.TableName = @table_name 
	--and ad.PKValue = @PKValue 
	--and ad.NewValue <> ''  -- ignore empty values 
	--and ad.ColumnName not like '%_FK' -- ignore (for now) link tables /values
	--and ad.ColumnName not like '%_PK'
	--group by am.Date	
	--)
	--select COUNT(*), ChangeDate 
	--from audit_records
 --   group by changeDate
 
 with audit_records as
	(
	select		
		am.Date as ChangeDate,
		MIN(am.Username) as Username		
	from AuditingMaster am
		inner join [AuditingDetails] ad on am.IDAuditingMaster = ad.MasterID
	
	where am.TableName = @table_name 
	and ad.PKValue = @PKValue 
	and ad.NewValue <> ''  -- ignore empty values 
	and ad.ColumnName not like '%_FK' -- ignore (for now) link tables /values
	and ad.ColumnName not like '%_PK'
	group by am.Date	
	)
	select ROW_NUMBER() over (order by ChangeDate), ChangeDate, Username
	from audit_records
    

	
	
END
