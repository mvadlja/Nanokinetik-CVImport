

CREATE PROCEDURE [dbo].[rep_list_xevprm_mess_delete_status]
AS

	SELECT * FROM 
	(
		SELECT -1 AS deleted, '-Choose-' AS name
		UNION ALL
		select distinct deleted, name 
		from
			(SELECT 
				deleted, 
				case 
				  when deleted = 1 then 'True'
				  when deleted = 0 then 'False' end as name
			FROM 

			dbo.XEVPRM_MESSAGE ) low
	) result
	ORDER BY CASE WHEN deleted = -1 THEN 'AAA' ELSE name 
	END