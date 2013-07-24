-----------------------------------------------
CREATE PROCEDURE  [dbo].[proc_AUDIT_GetFK]
	@PKValue int = 8252, --2147483647,	
	@date datetime = '2099-01-01 12:30:50',
	@table_name nvarchar(100) = 'AUTHORISED_PRODUCT',
	@column_name nvarchar(100) = 'product_FK'
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @retvalue int 
	
	select top 1 @retvalue = newvalue
	from dbo.AuditingDetails ad
	inner join dbo.AuditingMaster am on ad.MasterID = am.IDAuditingMaster
	where am.TableName=@table_name
		  and ColumnName = @column_name
		  and PKValue = @PKValue
	order by IDAuditingMaster desc
	
	RETURN @retvalue
	
END
