
CREATE PROCEDURE [dbo].[proc_AuditingDetails_GetColumnValues]
	@ColumnName nvarchar(250) = NULL,
	@OldValue nvarchar(250) = NULL,
	@NewValue nvarchar(250) = NULL,
	
	@SessionToken nvarchar(50) = null,
	@QueryBy nvarchar(100) = NULL,
	@EntityPk nvarchar(25) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'ColumnName'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @ExecuteQuery nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		Audit.* FROM
		(
			SELECT 
			ad.IDAuditingDetail,
			am.Date as ChangeDate, 
			am.Username,
			dbo.[ReturnAuditColumnNameForColumn](am.TableName, ad.ColumnName) AS ColumnName,
			case when 
			(ad.ColumnName LIKE ''%_FK''
				OR ad.ColumnName=''authorisation_procedure''
				OR ad.ColumnName=''legalstatus''
				OR ad.ColumnName=''regulatory_status''
				OR ad.ColumnName=''version_number''
				OR ad.ColumnName=''version_label'') 
			  then dbo.[ReturnAUDITNameForID](am.TableName, ad.ColumnName, ad.NewValue) ELSE ISNULL(ad.NewValue,'''') END as NewValue,
			case when 
				(ad.ColumnName LIKE ''%_FK''
				OR ad.ColumnName=''authorisation_procedure''
				OR ad.ColumnName=''legalstatus''
				OR ad.ColumnName=''regulatory_status''
				OR ad.ColumnName=''version_number''
				OR ad.ColumnName=''version_label'') then dbo.[ReturnAUDITNameForID](am.TableName, ad.ColumnName, ad.OldValue)	ELSE ISNULL(ad.OldValue,'''') END as OldValue
			FROM AuditingMaster am
		    INNER JOIN [AuditingDetails] ad ON am.IDAuditingMaster = ad.MasterID
			WHERE am.TableName =  ''' + CONVERT (nvarchar(MAX), @QueryBy) + '''
		    AND ad.PKValue = ''' + CONVERT (nvarchar(10), @EntityPk) + '''
			AND am.SessionToken = '''+ CONVERT (nvarchar(MAX), @SessionToken) + '''
			AND ((ad.NewValue IS NOT NULL AND ad.NewValue != '''') OR (ad.OldValue IS NOT NULL AND ad.OldValue != ''''))
		    AND (ad.ColumnName not like ''%_PK'' AND ad.ColumnName != ''prevent_start_date_alert'' AND ColumnName != ''prevent_exp_finish_date_alert'')
			'
			IF (@QueryBy = 'REMINDER')
				SET @Query = @Query + '
				AND ad.ColumnName NOT IN (''user_FK'', ''table_name'', ''entity_FK'', ''navigate_url'', ''is_automatic'', ''related_entity_FK'', ''reminder_date_PK'', ''repeating_mode'', ''status'', ''reminder_date'')
				'
			ELSE IF (@QueryBy = 'AUTHORISED_PRODUCT')
				SET @Query = @Query + '
				AND ad.ColumnName NOT IN (''Indications'')
				'


		SET @Query = @Query + 
		') AS Audit 
		'
		SET @TempWhereQuery = '';

		-- @ColumnName
		IF @ColumnName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Audit.ColumnName LIKE N''%' + REPLACE(REPLACE(@ColumnName,'[','[[]'),'''','''''') + '%'''
		END

		-- @OldValue
		IF @OldValue IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Audit.OldValue LIKE N''%' + REPLACE(REPLACE(@OldValue,'[','[[]'),'''','''''') + '%'''
		END

		-- @NewValue
		IF @NewValue IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Audit.NewValue LIKE N''%' + REPLACE(REPLACE(@NewValue,'[','[[]'),'''','''''') + '%'''
		END
		
		IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
		SET @Query = @Query + 
	')
	
	'
	SET @ExecuteQuery = @Query +
	'
	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @ExecuteQuery;

	SET @ExecuteQuery = @Query +
	'
	SELECT COUNT (*)
	FROM PagingResult
	'
	EXECUTE sp_executesql @ExecuteQuery;

END