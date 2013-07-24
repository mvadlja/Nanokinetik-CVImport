CREATE FUNCTION [dbo].[ReturnVarcharFromTime]

(
	@time_min int
)

RETURNS VARCHAR(50)
AS
  BEGIN
  		 
    RETURN 
    	CASE WHEN LEN(CAST(@time_min / 60 AS VARCHAR(10)) ) = 1 THEN '0' ELSE '' END +
    	
    	CAST(@time_min / 60 AS VARCHAR(10)) 
    	
    	+ ':' + 
    	
    	CASE WHEN LEN(CAST(@time_min % 60 AS VARCHAR(10)) ) = 1 THEN '0' ELSE '' END +
    	CAST(@time_min % 60 AS VARCHAR(10))
    
  END
