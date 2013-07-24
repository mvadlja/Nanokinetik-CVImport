
-- GetEntities
create PROCEDURE [dbo].[proc_NS_PROPERTY_MN_GetPropertyByNonStoPK]
@NonStoPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT property.*
	FROM [dbo].[NS_PROPERTY_MN]
	left join [dbo].[PROPERTY] property on property.property_PK = [dbo].[NS_PROPERTY_MN].property_FK
	where [dbo].[NS_PROPERTY_MN].ns_FK = @NonStoPK
END
