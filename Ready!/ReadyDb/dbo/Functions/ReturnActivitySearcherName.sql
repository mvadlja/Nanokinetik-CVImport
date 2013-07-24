CREATE FUNCTION [dbo].[ReturnActivitySearcherName]

(
	@activity_PK int=null
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
	declare @full_name nvarchar(max);
	with name_parts as
		(
			select [name], [procedure_number], stuff (
					(
						SELECT cast(', ' as varchar(max)) + mainTableAct.printName
						from (
							SELECT DISTINCT
							amn.activity_FK,
							c.abbreviation as printName
							FROM dbo.COUNTRY c
							LEFT JOIN dbo.ACTIVITY_COUNTRY_MN amn ON c.country_pk = amn.country_FK
						) as mainTableAct 
						WHERE mainTableAct.activity_FK = act.activity_PK
						for xml path('')
						), 1, 1, '') as countries
			from [ACTIVITY] as act
			where act.activity_PK = @activity_PK
		)
	
	
	
	select @full_name = (select 
							CASE
								WHEN name is null then 'Missing'
								WHEN (procedure_number is null OR procedure_number='') and (countries is null OR countries='') 
													then name
													
								WHEN (procedure_number is null OR procedure_number='') and (countries is not null AND countries!='') 
													then name + ' (' + countries + ')'
													
								WHEN (procedure_number is not null AND procedure_number!='') and (countries is null OR countries='') 
													then name + ' (' + procedure_number + ')'
													
								WHEN (procedure_number is not null AND procedure_number!='') and (countries is not null AND countries!='') 
													then name + ' (' + countries + ', ' + procedure_number + ')'			
							END
						from name_parts);
	return @full_name;
  END
