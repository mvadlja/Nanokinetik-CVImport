-----------------------------------------------
CREATE PROCEDURE  [dbo].[proc_AUDIT_GetRecordVersionsAP]
	@ap_PK int = 8250 --2147483647,		
AS
BEGIN
	SET NOCOUNT ON;
 
	with audit_records as
	(
		select		
			MIN(am.Date) as ChangeDate,
			am.Username as Username,
			am.SessionToken as SessionToken
		from AuditingMaster am
			right join [COMPLEX_AUDIT_DATA] cad on cad.session_token=am.SessionToken
		group by am.SessionToken, am.Username
				
	)
	select ROW_NUMBER() over (order by MIN(ChangeDate)) as [Version], MIN(ChangeDate) AS ChangeDate, MIN(Username) AS Username,SessionToken
	from audit_records
	group by SessionToken
    

	
	
END
