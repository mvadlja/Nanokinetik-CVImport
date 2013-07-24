
CREATE PROCEDURE [dbo].[proc_ORGANIZATION_GetListFormDataSet]
	@OrganizationName nvarchar(250) = NULL,
	@EvCode nvarchar(250) = NULL,
	@CountryCode nvarchar(250) = NULL,
	@Address nvarchar(250) = NULL,
	@FaxNumber nvarchar(250) = NULL,
	@City nvarchar(250) = NULL,
	@Email nvarchar(250) = NULL,
	@State nvarchar(250) = NULL,
	@PhoneNumber nvarchar(250) = NULL,
	@PhoneExtension nvarchar(250) = NULL,
	@RoleAssignments nvarchar(250) = NULL,

	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'organization_PK'
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
		ORGANIZATION.* FROM
		(
			SELECT DISTINCT
			organization.organization_PK,
			organization.name_org AS OrganizationName,
			organization.ev_code AS EvCode,
			country.abbreviation AS CountryCode,
			organization.address AS Address,
			organization.fax_number AS FaxNumber,
			organization.city AS City,
			organization.email AS Email,
			organization.state AS State,
			organization.tel_number AS PhoneNumber,
			organization.tel_extension AS PhoneExtension,
			(SELECT Stuff((SELECT '', '' + role_name FROM [dbo].[ORGANIZATION_IN_ROLE] orMn
				JOIN dbo.ORGANIZATION_ROLE r ON r.role_org_PK = orMn.role_org_FK
				WHERE orMn.organization_FK = organization.organization_PK
				FOR XML PATH('''')),1,2,'''')) AS RoleAssignments,			
			'''' as [Delete]

			FROM dbo.ORGANIZATION organization
			LEFT JOIN Country AS country ON country.country_PK = organization.countrycode_FK
			'
		SET @Query = @Query + 
		') AS organization
		'
		SET @TempWhereQuery = '';

		-- @OrganizationName
		IF @OrganizationName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'organization.OrganizationName LIKE N''%' + REPLACE(REPLACE(@OrganizationName,'[','[[]'),'''','''''') + '%'''
		END

		-- @EvCode
		IF @EvCode IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'organization.EvCode LIKE N''%' + REPLACE(REPLACE(@EvCode,'[','[[]'),'''','''''') + '%'''
		END

		-- @CountryCode
		IF @CountryCode IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'organization.CountryCode LIKE N''%' + REPLACE(REPLACE(@CountryCode,'[','[[]'),'''','''''') + '%'''
		END

		-- @Address
		IF @Address IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'organization.Address LIKE N''%' + REPLACE(REPLACE(@Address,'[','[[]'),'''','''''') + '%'''
		END

		-- @FaxNumber
		IF @FaxNumber IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'organization.FaxNumber LIKE N''%' + REPLACE(REPLACE(@FaxNumber,'[','[[]'),'''','''''') + '%'''
		END

		-- @City
		IF @City IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'organization.City LIKE N''%' + REPLACE(REPLACE(@City,'[','[[]'),'''','''''') + '%'''
		END

		-- @Email
		IF @Email IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'organization.Email LIKE N''%' + REPLACE(REPLACE(@Email,'[','[[]'),'''','''''') + '%'''
		END

		-- @State
		IF @State IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'organization.State LIKE N''%' + REPLACE(REPLACE(@State,'[','[[]'),'''','''''') + '%'''
		END

		-- @PhoneNumber
		IF @PhoneNumber IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'organization.PhoneNumber LIKE N''%' + REPLACE(REPLACE(@PhoneNumber,'[','[[]'),'''','''''') + '%'''
		END

		-- @PhoneExtension
		IF @PhoneExtension IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'organization.PhoneExtension LIKE N''%' + REPLACE(REPLACE(@PhoneExtension,'[','[[]'),'''','''''') + '%'''
		END

		-- @RoleAssignments
		IF @RoleAssignments IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'organization.RoleAssignments LIKE N''%' + REPLACE(REPLACE(@RoleAssignments,'[','[[]'),'''','''''') + '%'''
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