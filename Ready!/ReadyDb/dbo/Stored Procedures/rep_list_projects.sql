create PROCEDURE [dbo].[rep_list_projects]
AS

	SELECT * FROM 
	(
		SELECT -1 AS project_PK, '-Choose-' AS name
		UNION ALL
		SELECT 
			project_PK, name
		FROM 
			dbo.PROJECT
	) result
	ORDER BY CASE WHEN project_PK = -1 THEN 'AAA' ELSE name 

END
