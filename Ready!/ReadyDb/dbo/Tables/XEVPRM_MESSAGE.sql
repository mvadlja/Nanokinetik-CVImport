CREATE TABLE [dbo].[XEVPRM_MESSAGE] (
    [xevprm_message_PK]       INT            IDENTITY (1, 1) NOT NULL,
    [message_number]          NVARCHAR (100) NULL,
    [message_status_FK]       INT            NOT NULL,
    [message_creation_date]   DATETIME       NOT NULL,
    [user_FK]                 INT            NOT NULL,
    [xml]                     NVARCHAR (MAX) NULL,
    [xml_hash]                NVARCHAR (50)  CONSTRAINT [DF_xEVMPD_message_isLocked] DEFAULT ((0)) NULL,
    [sender_ID]               NVARCHAR (60)  NULL,
    [ack]                     NVARCHAR (MAX) NULL,
    [ack_type]                INT            NULL,
    [gateway_submission_date] DATETIME       NULL,
    [gateway_ack_date]        DATETIME       NULL,
    [submitted_FK]            INT            NULL,
    [generated_file_name]     NVARCHAR (MAX) NULL,
    [deleted]                 BIT            CONSTRAINT [DF_XEVPRM_MESSAGE_deleted] DEFAULT ((0)) NULL,
    [received_message_FK]     INT            NULL,
    CONSTRAINT [PK_xEVMPD_message] PRIMARY KEY CLUSTERED ([xevprm_message_PK] ASC),
    CONSTRAINT [CK_xEVMPD_messageee] CHECK ((1)=(1)),
    CONSTRAINT [FK_XEVPRM_MESSAGE_RECIEVED_MESSAGE] FOREIGN KEY ([received_message_FK]) REFERENCES [dbo].[RECIEVED_MESSAGE] ([recieved_message_PK]),
    CONSTRAINT [UQ__XEVPRM_M__0AA22DA96F8A7843] UNIQUE NONCLUSTERED ([message_number] ASC)
);





