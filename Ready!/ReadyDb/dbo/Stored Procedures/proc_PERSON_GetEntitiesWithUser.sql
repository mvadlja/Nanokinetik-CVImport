﻿-- GetEntities
CREATE PROCEDURE [dbo].[proc_PERSON_GetEntitiesWithUser]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[person_PK], [country_FK], [name], [familyname], [ip], [phone], [address], [city], [email], [status], [local_number], [ev_code], [givenname], [title], [company], [department], [building], [street], [state], [postcode], [countrycode], [tel_countrycode], [telnumber], [telextn], [cell_countrycode], [cellnumber], [fax_countrycode], [faxnumber], [faxextn], [telnum24h], [FullName]
	FROM [dbo].[PERSON] p
	WHERE p.person_PK in (SELECT person_FK FROM dbo.[USER])
END