﻿
-- GetEntity
CREATE PROCEDURE [dbo].[proc_PP_SUBSTANCE_GetEntity]
	@ppsubstance_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ppsubstance_PK], [ppsubstance_FK], [substancecode_FK], [concentrationtypecode], [lowamountnumervalue], [lowamountnumerprefix], [lowamountnumerunit], [lowamountdenomvalue], [lowamountdenomprefix], [lowamountdenomunit], [highamountnumervalue], [highamountnumerprefix], [highamountnumerunit], [highamountdenomvalue], [highamountdenomprefix], [highamountdenomunit], [pp_FK], [expressedby_FK], [concise], [substancetype], [user_FK], [sessionid], [modifieddate]
	FROM [dbo].[PP_SUBSTANCE]
	WHERE [ppsubstance_PK] = @ppsubstance_PK
END