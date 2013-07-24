create PROCEDURE rep_list_xevprm_mess_status
AS

	SELECT * FROM 
	(
		SELECT -1 AS xevprm_message_status_PK, '-Choose-' AS name
		UNION ALL
		SELECT 
			xevprm_message_status_PK, 
		   (
		       isnull (xevprm_grid_gateway_status_name, 'N/A' )
		       + 
		       ' (' 
		       + 
         				case 
         				  when 
         					xevprm_grid_status_name = name 
         				  then 
         				   enum_name 
         				  else 
         					name
         				end 
			   + ')' 
		    ) 
			as 
				xevprm_grid_gateway_status_name
		FROM 
			dbo.XEVPRM_MESSAGE_STATUS
	) result
	ORDER BY CASE WHEN xevprm_message_status_PK = -1 THEN 'AAA' ELSE name 

END