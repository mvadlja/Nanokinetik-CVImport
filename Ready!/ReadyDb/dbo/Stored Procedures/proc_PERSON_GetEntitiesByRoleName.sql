-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PERSON_GetEntitiesByRoleName]
@RoleName NVARCHAR(25) = NULL
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[person_PK], [country_FK], [name], [familyname], [ip], [phone], [address], [city], [email], [status], [local_number], [ev_code], [givenname], [title], [company], [department], [building], [street], [state], [postcode], [countrycode], [tel_countrycode], [telnumber], [telextn], [cell_countrycode], [cellnumber], [fax_countrycode], [faxnumber], [faxextn], [telnum24h], [FullName]
	FROM [dbo].[PERSON]
	left join  [dbo].PERSON_IN_ROLE on [dbo].[PERSON_IN_ROLE].person_FK = [dbo].[PERSON].person_PK
	left join  [dbo].PERSON_ROLE on [dbo].[PERSON_ROLE].person_role_PK = [dbo].[PERSON_IN_ROLE].person_role_FK
	where [dbo].[PERSON_ROLE].person_name = @RoleName AND @RoleName IS NOT NULL
END