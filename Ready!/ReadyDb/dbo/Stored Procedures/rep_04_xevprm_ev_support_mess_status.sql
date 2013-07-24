
CREATE  PROCEDURE [dbo].[rep_04_xevprm_ev_support_mess_status] 

	@message_status_fk int,
	@deleted int

as
    --TEST params
	--declare @senderorgID nvarchar(25) = 'BPET';
	--declare @receiverorgID nvarchar(25) = 'EVMPDHVAL';
	--declare @Environment nvarchar(25) = 'TEST';

	--Produkcija
	declare @senderorgID nvarchar(25) = 'BPEP';
	declare @receiverorgID nvarchar(25) = 'EVMPDH';
	declare @Environment nvarchar(25) = 'PRODUCTION';	
	
select 
		@senderorgID as senderorgID,
		@receiverorgID as receiverorgID,
		@Environment as Environment,
		--'BPEP' as senderorgID,
		--'EVMPDHVAL' as receiverorgID,
		--'TEST' as Environment,
		generated_file_name + '.xml' as OrigFileName,
		generated_file_name + '.zip' as OrigZipName,
		message_number as MessageNumb,
		gateway_submission_date as SendTimeDate,
		deleted

from 
	XEVPRM_MESSAGE
where 
	message_status_FK = @message_status_fk --or message_status_FK = -1 --ACK_pending
and
	deleted = @deleted --or deleted = -1
order by 
	message_creation_date desc, 
	generated_file_name, 
	message_number