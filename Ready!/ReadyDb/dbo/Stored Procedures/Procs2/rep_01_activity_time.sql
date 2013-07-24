CREATE PROCEDURE  rep_01_activity_time 
	@client int,
	@person INT,
	@date_from date,
	@date_to date
	
as

DECLARE @AppURL NVARCHAR(50) = 'http://5.79.32.172:8084';

select 
      o.name_org as Client
    , a.name as Activity
    ,'<a href="javascript:void(window.open(''' + @AppURL + '/Views/Business/APropertiesView.aspx?f=l&idAct='+ CAST(a.activity_PK as varchar)+''',''_blank''))">'+ a.name +'</a>' as ActivityLink
    , a.procedure_number as ActivityNumber 
    , pr.name as Product
    ,'<a href="javascript:void(window.open(''' + @AppURL + '/Views/Business/ProductPKPPropertiesView.aspx?f=l&id='+ CAST(pr.product_PK as varchar)+''',''_blank''))">'+ pr.name +'</a>' as ProductLink
    , tum.time_unit_name as TimeUnit
    ,'<a href="javascript:void(window.open(''' + @AppURL + '/Views/Business/ATimeUnitView.aspx?f=p&idAct='+ CAST(a.activity_PK as varchar)+'&idtu='+ CAST(tu.time_unit_PK as varchar)+''',''_blank''))">'+ tum.time_unit_name +'</a>' as TimeUnitLink
    , tu.[description] 
    , tu.actual_date as [Date]
    , tu.time_hours * 60 + tu.time_minutes AS time_calc
    , cast (tu.time_hours as varchar(5))+ ':' + cast (tu.time_minutes as varchar (25)) as time_label
    , p.name + ' ' + p.familyname as ResponsibleUser
    , dbo.ReturnVarcharFromTime( SUM(tu.time_hours * 60 + tu.time_minutes) OVER ()) AS total_time
	, dbo.ReturnVarcharFromTime( SUM(tu.time_hours * 60 + tu.time_minutes) OVER ( PARTITION BY o.name_org)) AS total_client_time
	, dbo.ReturnVarcharFromTime( SUM(tu.time_hours * 60 + tu.time_minutes) OVER ( PARTITION BY o.name_org, a.procedure_number, pr.name)) AS total_client_act_time
	, a.activity_PK
	, a.name
	, pr.product_PK
	, pr.name as product_name
	, tu.time_unit_PK
	, time_unit_name
from time_unit tu
inner join TIME_UNIT_NAME tum
on tu.time_unit_name_FK = tum.time_unit_name_PK
left join person p 
on tu.user_FK = p.person_PK
inner join activity a 
on tu.activity_FK = a.activity_PK
inner join ACTIVITY_PRODUCT_MN apmn
on a.activity_PK = apmn.activity_FK
inner join product pr 
on pr.product_PK = apmn.product_FK
inner join organization o 
on o.organization_PK = pr.client_organization_FK
WHERE
	tu.actual_date between @date_from AND @date_to
	
	AND (  o.organization_PK = @client OR @client = -1)
	AND ( p.person_PK = @person OR @person = -1)
	
order by Activity, Product
