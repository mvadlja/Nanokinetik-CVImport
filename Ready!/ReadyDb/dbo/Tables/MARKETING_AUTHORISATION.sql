CREATE TABLE [dbo].[MARKETING_AUTHORISATION] (
    [marketing_authorisation_PK] INT            IDENTITY (1, 1) NOT NULL,
    [ready_id]                   NVARCHAR (32)  NOT NULL,
    [ma_status_FK]               INT            NULL,
    [message_folder]             NVARCHAR (500) NULL,
    [creation_time]              DATETIME       NULL,
    CONSTRAINT [PK_MA_INBOUND_FILE] PRIMARY KEY CLUSTERED ([marketing_authorisation_PK] ASC),
    CONSTRAINT [FK_MARKETING_AUTHORISATION_MA_STATUS] FOREIGN KEY ([ma_status_FK]) REFERENCES [dbo].[MA_STATUS] ([ma_stutus_PK])
);

