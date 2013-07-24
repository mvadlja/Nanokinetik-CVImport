
-- GetEntities
create PROCEDURE [dbo].[proc_NS_MOIETY_MN_GetMoietyByNonStoPK]
@NonStoPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT moiety.*
	FROM [dbo].[NS_MOIETY_MN]
	left join [dbo].[MOIETY] moiety on moiety.moiety_PK = [dbo].[NS_MOIETY_MN].moiety_FK
	where [dbo].[NS_MOIETY_MN].ns_FK = @NonStoPK
END
