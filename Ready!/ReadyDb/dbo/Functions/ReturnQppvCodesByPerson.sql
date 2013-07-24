CREATE FUNCTION [dbo].[ReturnQppvCodesByPerson]
(
	@person_PK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @qppCodes nvarchar(max)

	SELECT @qppCodes = ISNULL(NULLIF(COALESCE(@qppCodes + ', ', ''), ', '), 'N/A, ') + ISNULL(NULLIF(LTRIM(RTRIM(qppv.qppv_code)), ''), 'N/A')
	FROM dbo.PERSON person
	LEFT JOIN QPPV_CODE AS qppv ON qppv.Person_FK = person.person_PK
	WHERE person.person_PK = @person_PK

    RETURN @qppCodes
    
  END