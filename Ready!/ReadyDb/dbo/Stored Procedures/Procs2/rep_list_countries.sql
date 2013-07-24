CREATE PROCEDURE  [dbo].[rep_list_countries]

AS

SELECT * FROM (
SELECT 'All' AS abbrevation, 'All' as name UNION ALL
SELECT 
	abbreviation, name
from
	dbo.COUNTRY
	
)pr
ORDER BY CASE WHEN abbrevation = 'All' THEN 'AAA' ELSE name end
