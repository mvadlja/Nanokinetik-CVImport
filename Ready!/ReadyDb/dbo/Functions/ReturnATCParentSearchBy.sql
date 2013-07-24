CREATE FUNCTION [dbo].[ReturnATCParentSearchBy]

(
	@atccode nvarchar(max)
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @n int;
	declare @tmp nvarchar(max);
	
	SET @n = LEN(@atccode) - 1;
    
    while (@n >= 1)
		BEGIN
		SET @tmp = (SELECT TOP 1 ATC.search_by from ATC WHERE atccode=SUBSTRING(@atccode,1,@n));
		IF @tmp != ''
			BEGIN
				RETURN @tmp
			END
		SET @n = @n - 1;
		END
    
    return '';
  END
