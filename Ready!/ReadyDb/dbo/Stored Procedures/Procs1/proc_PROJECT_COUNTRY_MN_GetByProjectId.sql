﻿-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PROJECT_COUNTRY_MN_GetByProjectId]
	@ProjecId int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	COUNTRY.*
	FROM COUNTRY JOIN [dbo].[PROJECT_COUNTRY_MN] on COUNTRY.country_PK = PROJECT_COUNTRY_MN.country_FK
	WHERE PROJECT_COUNTRY_MN.project_FK=@ProjecId;
END
