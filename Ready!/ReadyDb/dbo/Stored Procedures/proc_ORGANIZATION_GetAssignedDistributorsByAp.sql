
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_GetAssignedDistributorsByAp]
	@authorisedProductFk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT organization_PK, type_org_EMEA, type_org_FK, name_org, localnumber, ev_code, organizationsenderid_EMEA, address, city, state, postcode, countrycode_FK, tel_number, tel_extension, tel_countrycode, fax_number, fax_extenstion, fax_countrycode, email, comment, mfl_evcode, mflcompany, mfldepartment, mflbuilding, pom_agency, pom_client, pom_manu, pom_applicant, pom_license_holder_, pom_dist
	FROM  AP_ORGANIZATION_DIST_MN authorisedProductDistributorMn
	LEFT JOIN ORGANIZATION organisation ON organisation.organization_PK = authorisedProductDistributorMn.organization_FK
	WHERE (authorisedProductDistributorMn.ap_FK = @authorisedProductfk OR @authorisedProductfk = NULL)

END