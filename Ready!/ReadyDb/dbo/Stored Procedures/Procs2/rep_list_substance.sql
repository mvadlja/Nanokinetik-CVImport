CREATE PROCEDURE  [dbo].[rep_list_substance]

AS

SELECT * FROM (
SELECT -1 AS substance_pk, 'All' as name UNION ALL
SELECT
			   substance_pk, LEFT(substance_name,100) AS name 
			FROM
				dbo.SUBSTANCES
	
)pr
ORDER BY CASE WHEN substance_pk = -1 THEN 'AAA' ELSE name end
