-----------------------------------------------
CREATE PROCEDURE  [dbo].[proc_AUDIT_GetEntity]
	@PKValue int = 55, --2147483647,
	@table_name nvarchar(100) = 'AUTHORISED_PRODUCT',
	@date datetime = '2099-01-01'
AS
BEGIN
	SET NOCOUNT ON;
	Declare @query nvarchar(MAX);
	
	if @date = '2099-01-01'
	begin
		Declare @colName nvarchar(100);
		SELECT @colName = COLUMN_NAME
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE COLUMN_NAME like '%PK' 
		and TABLE_NAME = @table_name
		
		set @Query = 'Select * from '+@table_name+' where '+@colName+' = '+CAST(@pkvalue as nvarchar)
		print @query
		EXECUTE sp_executesql @Query;
	end
	else
	begin
		---
		--- first, extract column names
		---	
		Declare @ColNames nvarchar(max)
		Declare @maxColNames nvarchar(max)
		SELECT 
			@ColNames = COALESCE(@ColNames + ', ', '') + Column_name,
			--@maxColNames = COALESCE(@maxColNames + ', ', '') +'case max('+ Column_name+') when '''' then null else max('+ Column_name +') end as '+Column_name
			@maxColNames = COALESCE(@maxColNames + ', ', '') +'case max('+ Column_name+') when '''' then null else cast( max('+ Column_name+') as '+data_type+') end as '+Column_name
		FROM information_schema.columns
		where table_name = @table_name
		order by ordinal_position

		--- now, construct query 
		set @Query = '
				select ' + @maxColNames + '
				from 
					(select ad.* from
						(select ColumnName, MAX(IDAuditingDetail) as IDAuditingDetail
							from dbo.AuditingDetails 
							left join AuditingMaster m on m.IDAuditingMaster = masterID
							where MasterID in ( select IDAuditingMaster from [dbo].[AuditingMaster]
												where TableName = '''+@table_name+''')
								  and PKValue = '+CAST(@PKValue as nvarchar)+'
							group by  ColumnName--, masterID, IDAuditingDetail, m.[date]
							--having MasterID <= 1805
							having MAX(m.[Date]) <= '''+CONVERT(nvarchar,@date,120)+'''
						) filter
					left join dbo.AuditingDetails ad
					   on filter.IDAuditingDetail = ad.IDAuditingDetail) data
				pivot (
					max(newValue)
					for columnName in (  ' + @ColNames + ')
				) pvt
				group by pkvalue'

		print @query 
		EXECUTE sp_executesql @Query;
	end
END






--exec [proc_AUDIT_GetEntity] 59, 'PRODUCT'
