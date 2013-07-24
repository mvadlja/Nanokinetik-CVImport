-- GetEntitiesByUserID
CREATE PROCEDURE  [dbo].[proc_REMINDER_GetEntitiesByResponsibleUser]
	@responsible_user_FK int = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = '[status], [reminder_date] desc'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @QueryCount nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	set @OrderByQuery = '[status] desc';
	SET DATEFORMAT dmy;
	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		DistinctReminders.* FROM
		(
		
		SELECT DISTINCT
			tmpTable.reminder_PK,
			tmpTable.responsible_user_FK,
			tmpTable.related_attribute_name,    
			--tmpTable.related_attribute_value,
			tmpTable.reminder_type,
			tmpTable.reminder_name,
			tmpTable.navigate_url,
			tmpTable.time_before_activation,
			tmpTable.additional_emails,
			tmpTable.description,
			tmpTable.is_automatic,
			tmpTable.ResponsibleUser,
			tmpTable.OverDue,
			tmpTable.related_entity_FK,
			case 
			when ISDATE(related_attribute_value) = 1 then convert(datetime, related_attribute_value, 104)
			else null
			end as related_attribute_value,
			 ISNULL(stuff ((
					 SELECT cast('', '' as varchar(max)) + mainTableEmails.printName
					from (
						SELECT DISTINCT
						[dbo].[REMINDER_EMAIL_RECIPIENT].reminder_FK,
						[dbo].[PERSON].[email] as printName
						FROM [dbo].[REMINDER_EMAIL_RECIPIENT]
						LEFT JOIN dbo.[PERSON] on dbo.[REMINDER_EMAIL_RECIPIENT].person_FK = dbo.[PERSON].person_PK
					) as mainTableEmails
					WHERE mainTableEmails.reminder_FK = tmpTable.reminder_PK
					for xml path('''')
					), 1, 1, ''''),'''') + 
					case 
					when tmpTable.additional_emails is not null and tmpTable.additional_emails != '''' then ('', '' + tmpTable.additional_emails)
					else '''' end AS EmailAdresses
			FROM
			(
			SELECT DISTINCT
			case
				when reminder_date <= GETDATE() then ''Yes''--DATEADD(day,- isnull(time_before_activation,0),reminder_date) <= GETDATE() then ''Yes''
				else ''No''
			end as OverDue,
			[dbo].[REMINDER].[reminder_PK],  
			[dbo].[REMINDER].[responsible_user_FK], 
			[dbo].[REMINDER].[related_attribute_name], 
			[dbo].[REMINDER].[related_attribute_value], 
			[dbo].[REMINDER].[reminder_type],
			[dbo].[REMINDER].[reminder_name],
			[dbo].[REMINDER].[navigate_url], 
			[dbo].[REMINDER].[reminder_date], 
			[dbo].[REMINDER].[time_before_activation],  
			[dbo].[REMINDER].[additional_emails], 
			[dbo].[REMINDER].[description], 
			[dbo].[REMINDER].[status],
			[dbo].[REMINDER].[is_automatic],
			[dbo].[REMINDER].[related_entity_FK],
			dbo.[PERSON].name +'' ''+dbo.[PERSON].familyname as ResponsibleUser
			FROM [dbo].[REMINDER]
			left join dbo.[PERSON] on dbo.[PERSON].[person_PK] = [dbo].[REMINDER].[responsible_user_FK]

			'
			SET @TempWhereQuery = ' where ([dbo].[REMINDER].is_automatic != 1 or [dbo].[REMINDER].status != ''Dismissed'')  ';

			-- Check nullability for every parameter
			-- @responsible_user_FK
			IF @responsible_user_FK IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '[dbo].[REMINDER].[responsible_user_FK] = ' + CONVERT (nvarchar(MAX), @responsible_user_FK)
			END

			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + ') as tmpTable 
		) DistinctReminders
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SET @QueryCount = '
	SELECT COUNT(DISTINCT [dbo].[REMINDER].[reminder_PK]) FROM [dbo].[REMINDER]
	' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
END
