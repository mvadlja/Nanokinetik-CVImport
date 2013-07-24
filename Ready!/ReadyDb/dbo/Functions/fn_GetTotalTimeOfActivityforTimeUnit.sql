create FUNCTION [dbo].[fn_GetTotalTimeOfActivityforTimeUnit] 	(@activity_PK int)
RETURNS decimal (4,2)
AS
begin
  declare @Satiminuta decimal (4,2)
select   @Satiminuta = (((sum(    cast(tu.time_hours as decimal(4,2))   )*60) + (sum(  cast(tu.time_minutes as decimal(4,2)) )))/60) from activity a 
inner join dbo.TIME_UNIT tu
on a.activity_PK = tu.activity_FK
where a.activity_PK = @activity_PK
  return @Satiminuta 
end
