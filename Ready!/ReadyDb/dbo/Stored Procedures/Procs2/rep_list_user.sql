CREATE PROCEDURE  [dbo].[rep_list_user]
AS
SELECT * FROM (
SELECT -1 AS person_pk, '-- Choose --' AS familyname UNION ALL
SELECT
	person_pk,
	name + ' ' + familyname AS familyname
from
PERSON
)x
ORDER BY CASE WHEN person_pk = -1 THEN 'AAA' ELSE familyname end
