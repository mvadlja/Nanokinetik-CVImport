
-- GetEntitiesByUserID
CREATE PROCEDURE  [dbo].[proc_REMINDER_GetListFormDataSet]
	@ResponsibleUserFk nvarchar(250) = NULL,
	@ReminderUserStatus nvarchar(250) = NULL,
	@OverDue nvarchar(250) = NULL,
	@reminder_dates nvarchar(250) = NULL,
	@time_before_activation nvarchar(250) = NULL,
	@related_attribute_value nvarchar(250) = NULL,
	@related_attribute_valueFrom nvarchar(250) = NULL,
	@related_attribute_valueTo nvarchar(250) = NULL,
	@related_attribute_name nvarchar(250) = NULL,
	@reminder_type nvarchar(250) = NULL,
	@reminder_name nvarchar(250) = NULL,
	@description nvarchar(250) = NULL,
	@EmailAdresses nvarchar(250) = NULL,
	@ResponsibleUser nvarchar(250) = NULL,
	@RelatedProducts nvarchar(250) = NULL,

	@EntityContext nvarchar(250) = NULL,
	@EntityContextContains nvarchar(250) = NULL,
	@EntityPk int = NULL,
    @IsPrivate bit = NULL,

	@PageNum int = 1,
	@PageSize int = 1000,
	@OrderByQuery nvarchar(1000) = 'reminder_PK'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @ExecuteQuery nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);
DECLARE @TempJoinQuery nvarchar(MAX);
BEGIN
	SET NOCOUNT ON;
	SET DATEFORMAT dmy;
	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		Reminder.* FROM
		(
			SELECT DISTINCT
			rmd.[reminder_PK],  
			rmd.[responsible_user_FK], 
			rmd.[related_attribute_name], 
			rmd.[reminder_type],
			rmd.[reminder_name],
			rmd.[navigate_url], 
			rmd.[time_before_activation],  
			rmd.[additional_emails], 
			rmd.[description], 
			rmd.[is_automatic],
			rmd.entity_FK,
			rus.name AS ReminderUserStatus,
			dbo.[ReturnDatesByReminder](rmd.[reminder_PK]) AS reminder_dates, 
			reminderStatus.name AS status, 
			reminderRepeatingMode.name AS repeating_mode,
			rmd.related_entity_FK,
			p.name + '' '' + p.familyname as ResponsibleUser,
			''RelatedProducts'' = 
			CASE
				WHEN reminder_type = ''Authorised product'' THEN (SELECT p.ProductName FROM dbo.PRODUCT p WHERE p.product_PK = rmd.entity_FK)
				WHEN reminder_type = ''Activity'' THEN dbo.ReturnProductsByActivity(rmd.entity_FK)	
				WHEN reminder_type = ''Task'' THEN dbo.ReturnProductsByTask(rmd.entity_FK) 	
				WHEN reminder_type = ''Project'' THEN dbo.ReturnProductsByProject(rmd.entity_FK)
				WHEN reminder_type = ''P Document'' THEN dbo.ReturnProductsByPDocument(rmd.entity_FK, rmd.related_entity_FK) 	
				WHEN reminder_type = ''AP Document'' THEN dbo.ReturnProductsByAPDocument(rmd.entity_FK, rmd.related_entity_FK)
				WHEN reminder_type = ''A Document'' THEN dbo.ReturnProductsByADocument(rmd.entity_FK, rmd.related_entity_FK) 		
				ELSE ''No''
			END,
			''EntityContext'' = 
			CASE
				WHEN reminder_type = ''Authorised product'' OR reminder_type = ''AP Document'' THEN ''AuthorisedProduct''
				WHEN reminder_type = ''Product'' OR reminder_type = ''P Document'' THEN ''Product''
				WHEN reminder_type = ''Activity'' OR reminder_type = ''A Document'' THEN ''Activity''
				WHEN reminder_type = ''Task'' OR reminder_type = ''Task Document'' THEN ''Task''
				WHEN reminder_type = ''Project'' OR reminder_type = ''Project Document'' THEN ''Project''
				ELSE ''Unknown''
			END,
			[dbo].[ReturnIsOverDue](rmd.[reminder_PK]) AS OverDue,
			CASE 
				WHEN ISDATE(related_attribute_value) = 1 then convert(datetime, related_attribute_value, 104)
				ELSE null
			END AS related_attribute_value,
			ISNULL(NULLIF(
			ISNULL(
				stuff ((
					SELECT cast('', '' AS NVARCHAR(MAX)) + EmailTable.Email
					FROM (
						SELECT DISTINCT
						[dbo].[PERSON].[email] as Email
						FROM [dbo].[REMINDER_EMAIL_RECIPIENT] rmdERcv
						LEFT JOIN dbo.[PERSON] on rmdERcv.person_FK = dbo.[PERSON].person_PK
						WHERE rmdERcv.reminder_FK = rmd.reminder_PK
					) as EmailTable
					for xml path('''') 
				), 1, 2, ''''),'''') + 
			CASE 
				WHEN rmd.additional_emails is not null and rmd.additional_emails != '''' then '', ''
				ELSE '''' 
			END, '', ''),'''')
			+
			CASE 
				WHEN rmd.additional_emails is not null and rmd.additional_emails != '''' then rmd.additional_emails
				ELSE '''' 
			END AS EmailAdresses,
			'''' as [Action]

			FROM [dbo].[REMINDER] rmd
			LEFT JOIN dbo.[PERSON] p on p.[person_PK] = rmd.[responsible_user_FK]
			LEFT JOIN REMINDER_DATES reminderDates ON reminderDates.reminder_FK = rmd.reminder_PK
			LEFT JOIN REMINDER_STATUS reminderStatus ON reminderStatus.reminder_status_PK = reminderDates.reminder_status_FK
			LEFT JOIN REMINDER_REPEATING_MODES reminderRepeatingMode ON reminderRepeatingMode.reminder_repeating_mode_PK = reminderDates.reminder_repeating_mode_FK
			LEFT JOIN REMINDER_USER_STATUS rus ON rus.reminder_user_status_PK = rmd.reminder_user_status_FK		
			WHERE (rmd.is_automatic != 1 or reminderStatus.name != ''Dismissed'' )
		) AS Reminder
		'
		SET @TempWhereQuery = '';

		-- @ResponsibleUserFk
		IF @ResponsibleUserFk IS NOT NULL AND @IsPrivate = '0'
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.responsible_user_FK = ' + REPLACE(REPLACE(@ResponsibleUserFk,'[','[[]'),'''','''''')
		END

		-- @ReminderUserStatus
		IF @ReminderUserStatus IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.ReminderUserStatus LIKE N''%' + REPLACE(REPLACE(@ReminderUserStatus,'[','[[]'),'''','''''') + '%'''
		END

		-- @OverDue
		IF @OverDue IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.OverDue LIKE N''%' + REPLACE(REPLACE(@OverDue,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @RelatedProducts
		IF @RelatedProducts IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.RelatedProducts LIKE N''%' + REPLACE(REPLACE(@RelatedProducts,'[','[[]'),'''','''''') + '%'''
		END

		-- @reminder_dates
		IF @reminder_dates IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.reminder_dates LIKE N''%' + REPLACE(REPLACE(@reminder_dates,'[','[[]'),'''','''''') + '%'''
		END	

		-- @time_before_activation
		IF @time_before_activation IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.time_before_activation LIKE N''%' + REPLACE(REPLACE(@time_before_activation,'[','[[]'),'''','''''') + '%'''
		END

		-- @related_attribute_value
		IF @related_attribute_value IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30),Reminder.[related_attribute_value],104) LIKE ''%' + REPLACE(REPLACE(@related_attribute_value,'[','[[]'),'''','''''') + '%'''
		END	
		
		-- @related_attribute_valueFrom
		IF @related_attribute_valueFrom IS NOT NULL AND ISDATE(@related_attribute_valueFrom) = 1
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.related_attribute_value >= convert(datetime, ''' + REPLACE(REPLACE(@related_attribute_valueFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @related_attribute_valueTo
		IF @related_attribute_valueTo IS NOT NULL AND ISDATE(@related_attribute_valueTo) = 1
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.related_attribute_value <= convert(datetime, ''' + REPLACE(REPLACE(@related_attribute_valueTo,'[','[[]'),'''','''''') + ''', 104) '
		END

		-- @related_attribute_name
		IF @related_attribute_name IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.related_attribute_name LIKE N''%' + REPLACE(REPLACE(@related_attribute_name,'[','[[]'),'''','''''') + '%'''
		END

		-- @reminder_type
		IF @reminder_type IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.reminder_type LIKE N''%' + REPLACE(REPLACE(@reminder_type,'[','[[]'),'''','''''') + '%'''
		END

		-- @reminder_name
		IF @reminder_name IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.reminder_name LIKE N''%' + REPLACE(REPLACE(@reminder_name,'[','[[]'),'''','''''') + '%'''
		END

		-- @description
		IF @description IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.description LIKE N''%' + REPLACE(REPLACE(@description,'[','[[]'),'''','''''') + '%'''
		END

		-- @EmailAdresses
		IF @EmailAdresses IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.EmailAdresses LIKE N''%' + REPLACE(REPLACE(@EmailAdresses,'[','[[]'),'''','''''') + '%'''
		END

		-- @ResponsibleUser
		IF @ResponsibleUser IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.ResponsibleUser LIKE N''%' + REPLACE(REPLACE(@ResponsibleUser,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @EntityContext
		IF @EntityContext IS NOT NULL AND @EntityPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.EntityContext = N''' + REPLACE(REPLACE(@EntityContext,'[','[[]'),'''','''''') + ''''
		END
		
		-- @EntityContextContains
		IF @EntityContextContains IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.entity_FK = ' + REPLACE(REPLACE(@EntityPk,'[','[[]'),'''','''''') + ' AND Reminder.reminder_type LIKE N''%' + REPLACE(REPLACE(@EntityContextContains,'[','[[]'),'''','''''') + '%'''
		END
  
        -- @EntityPk
		IF @EntityPk IS NOT NULL AND @EntityContextContains IS NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Reminder.related_entity_FK = ' + REPLACE(REPLACE(@EntityPk,'[','[[]'),'''','''''')
		END

		-- @IsPrivate
		IF @IsPrivate IS NOT NULL AND @IsPrivate = '1'
		BEGIN
		    IF @ResponsibleUserFk IS NOT NULL BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + '(Reminder.responsible_user_FK = ' + REPLACE(REPLACE(@ResponsibleUserFk,'[','[[]'),'''','''''')
				
				DECLARE @PersonEmail NVARCHAR(500) = '';
				SET @PersonEmail = (SELECT email FROM PERSON WHERE person_PK = CONVERT(INT, @ResponsibleUserFk));
			
				IF NULLIF(@PersonEmail, '') IS NOT NULL BEGIN 
					IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
					ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' OR '; END
					SET @TempWhereQuery = @TempWhereQuery + 'Reminder.EmailAdresses LIKE N''%' + REPLACE(REPLACE(@PersonEmail,'[','[[]'),'''','''''') + '%''' + ')'
				END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ')' END
		    END
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