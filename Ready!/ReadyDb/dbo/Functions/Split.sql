create FUNCTION [dbo].[Split]
(
  @delimited nvarchar(max),
  @delimiter nvarchar(100)
) RETURNS @t TABLE
(
-- Id column can be commented out, not required for sql splitting string
--  id int identity(1,1), -- I use this column for numbering splitted parts
  val nvarchar(max)
)
AS
BEGIN
  declare @xml xml
  set @xml = N'<root><r>' + replace(@delimited,@delimiter,'</r><r>') + '</r></root>'

  insert into @t(val)
  select
    r.value('.','varchar(5)') as item
  from @xml.nodes('//root/r') as records(r)

  RETURN
END
