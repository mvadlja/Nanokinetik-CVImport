CREATE TABLE [dbo].[SENT_MESSAGE] (
    [sent_message_PK] INT                        IDENTITY (1, 1) NOT NULL,
    [sent_time]       DATETIME                   NULL,
    [msg_type]        INT                        NULL,
    [xevmpd_FK]       INT                        NULL,
    [msg_id]          UNIQUEIDENTIFIER           DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [msg_data]        VARBINARY (MAX) FILESTREAM NULL,
    CONSTRAINT [PK_SENT_MESSAGE_PK] PRIMARY KEY CLUSTERED ([sent_message_PK] ASC),
    UNIQUE NONCLUSTERED ([msg_id] ASC)
) FILESTREAM_ON [ReadyFileGroup];

