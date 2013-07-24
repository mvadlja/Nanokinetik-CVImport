
create PROCEDURE  [dbo].[proc_AP_ORGANIZATION_DIST_MN_GetAssignedDistributorsByAp]
	@authorisedProductFk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT authorisedProductDistributorMn.ap_organizatation_dist_mn_PK, authorisedProductDistributorMn.ap_FK, authorisedProductDistributorMn.organization_FK
	FROM  AP_ORGANIZATION_DIST_MN authorisedProductDistributorMn
	LEFT JOIN ORGANIZATION organisation ON organisation.organization_PK = authorisedProductDistributorMn.organization_FK
	WHERE (authorisedProductDistributorMn.ap_FK = @authorisedProductfk OR @authorisedProductfk = NULL)

END