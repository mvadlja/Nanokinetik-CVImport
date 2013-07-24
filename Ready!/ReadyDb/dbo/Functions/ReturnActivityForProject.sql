CREATE FUNCTION [dbo].[ReturnActivityForProject]

(
	@project int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @activity nvarchar(max)

	SELECT @activity = COALESCE(@activity + ', ', '') +
			isnull(rtrim(ltrim(act.name)) , '')
			from dbo.ACTIVITY as act
			join dbo.ACTIVITY_PROJECT_MN on act.activity_PK = activity_FK
			where project_FK=@project
	ORDER BY act.name
	
	
    RETURN @activity
    
  END
