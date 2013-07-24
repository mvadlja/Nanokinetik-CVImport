-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_ATC_RefreshSearchBy]

AS
BEGIN
	SET NOCOUNT ON;
	declare @it int;
	declare @tmp int;
	declare @buffer nvarchar(max);
	declare @delimiter nvarchar(2);
	set @it = 1;
	
	while (@it < 12)
		BEGIN
			-- getting parent object
			
			UPDATE ATC SET search_by=
			(
				--[dbo].[ReturnATCParentSearchBy](ATC.atccode) + ' ' + name
				CASE 
					WHEN ([dbo].[ReturnATCParentSearchBy](ATC.atccode) IS NULL OR [dbo].[ReturnATCParentSearchBy](ATC.atccode)='') THEN name
					when ([dbo].[ReturnATCParentSearchBy](ATC.atccode) IS NOT NULL AND [dbo].[ReturnATCParentSearchBy](ATC.atccode) != '') THEN [dbo].[ReturnATCParentSearchBy](ATC.atccode) + ' ' + name
				END
			) 
			WHERE LEN(atccode) = @it;
			
			SET @it = @it + 1;
		END
END
