CREATE TABLE [dbo].[MA_MESSAGE_HEADER] (
    [ma_message_header_PK] INT             IDENTITY (1, 1) NOT NULL,
    [messageformatversion] NVARCHAR (10)   NULL,
    [messageformatrelease] NVARCHAR (10)   NULL,
    [registrationnumber]   NVARCHAR (30)   NULL,
    [registrationid]       BIGINT          NULL,
    [readymessageid]       NVARCHAR (32)   NULL,
    [messagedateformat]    NVARCHAR (10)   NULL,
    [messagedate]          DATETIME        NULL,
    [ready_id_FK]          NVARCHAR (32)   NULL,
    [message_file_name]    NVARCHAR (1000) NULL,
    CONSTRAINT [PK_MA_MESSAGE_HEADER] PRIMARY KEY CLUSTERED ([ma_message_header_PK] ASC),
    CONSTRAINT [FK_MA_MESSAGE_HEADER_MA_MESSAGE_HEADER] FOREIGN KEY ([ma_message_header_PK]) REFERENCES [dbo].[MA_MESSAGE_HEADER] ([ma_message_header_PK])
);

