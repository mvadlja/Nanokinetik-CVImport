-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
create FUNCTION [dbo].[ParseDelimitedString](@Delimeter varchar(1), @InputString varchar(MAX))
RETURNS @tblTemp TABLE(Name varchar(255))
AS
BEGIN

	DECLARE @StartPos int, @Length int;
	DECLARE @Name varchar(255);
	
	WHILE LEN(@InputString) > 0
	BEGIN
		SET @StartPos = CHARINDEX(@Delimeter, @InputString)
		IF @StartPos < 0 SET @StartPos = 0
		SET @Length = LEN(@InputString) - @StartPos - 1
		IF @Length < 0 SET @Length = 0
		IF @StartPos > 0
		  BEGIN
			SET @Name = SUBSTRING(@InputString, 1, @StartPos - 1)
			SET @InputString = SUBSTRING(@InputString, @StartPos + 1, LEN(@InputString) - @StartPos)
		  END
		ELSE
		  BEGIN
			SET @Name = @InputString
			SET @InputString = ''
		  END
		INSERT @tblTemp (Name) VALUES(@Name)
	END
	RETURN 
END
