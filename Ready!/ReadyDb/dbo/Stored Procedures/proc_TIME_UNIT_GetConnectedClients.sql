CREATE PROCEDURE proc_TIME_UNIT_GetConnectedClients
	@project_PK INT = NULL
AS
BEGIN

	--SET @project_PK = 16; -- UNCOMMENT FOR TEST

	WITH Clients AS 
	(
		SELECT 
		      ROW_NUMBER() OVER (ORDER BY product.client_organization_FK) as ClientGroup
		     , product.client_organization_FK
		FROM ACTIVITY_PROJECT_MN activityProject_mn
		LEFT JOIN ACTIVITY activity ON activity.activity_PK = activityProject_mn.activity_FK
		LEFT JOIN ACTIVITY_PRODUCT_MN activityProd_mn on activityProd_mn.activity_FK = activity.activity_PK
		LEFT JOIN PRODUCT product ON product.product_PK = activityProd_mn.product_FK 
		WHERE 
			(activityProject_mn.project_FK = @project_PK OR @project_PK IS NULL)
			AND product.client_organization_FK IS NOT NULL
		GROUP BY 
			  product.client_organization_FK
	)
	
	SELECT * FROM Clients;
END