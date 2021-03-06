﻿-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_AP_SAVED_SEARCH_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'AP_SAVED_SEARCH_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[AP_SAVED_SEARCH_PK], [product_FK], [authorisationcountrycode_FK], [productshortname], 
		[responsible_user_person_FK], [packagedesc], [authorisationstatus_FK], [legalstatus], [marketed],
		 [organizationmahcode_FK], [authorisationdateFrom], [authorisationdateTo], [authorisationexpdateFrom],
		  [authorisationexpdateTo], [authorisationnumber],[displayName],[user_FK],[gridLayout],
		  [ispublic],[article57_reporting],[client_org_FK],[sunsetclauseFrom],[sunsetclauseTo], [MEDDRA_FK],[substance_translations],
		  ev_code,[qppv_person_FK], [local_representative_FK], [indications], [local_qppv_person_FK], [mflcode_FK]
		FROM [dbo].[AP_SAVED_SEARCH]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[AP_SAVED_SEARCH]
END
