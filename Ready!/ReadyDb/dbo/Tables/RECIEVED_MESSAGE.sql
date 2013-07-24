CREATE TABLE [dbo].[RECIEVED_MESSAGE] (
    [recieved_message_PK]       INT             IDENTITY (1, 1) NOT NULL,
    [msg_data]                  VARBINARY (MAX) NULL,
    [received_time]             DATETIME        NULL,
    [processed_time]            DATETIME        NULL,
    [processed]                 BIT             NULL,
    [is_successfully_processed] BIT             NULL,
    [msg_type]                  INT             NULL,
    [as_header]                 NVARCHAR (MAX)  NULL,
    [processing_error]          NVARCHAR (MAX)  NULL,
    [xevmpd_FK]                 INT             NULL,
    [status]                    INT             NULL,
    CONSTRAINT [PK_RECIEVED_MESSAGE] PRIMARY KEY CLUSTERED ([recieved_message_PK] ASC)
);

