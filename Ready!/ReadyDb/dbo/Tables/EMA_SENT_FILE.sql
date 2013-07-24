CREATE TABLE [dbo].[EMA_SENT_FILE] (
    [ema_sent_file_PK] INT             IDENTITY (1, 1) NOT NULL,
    [file_name]        NVARCHAR (500)  NULL,
    [file_type]        NVARCHAR (50)   NULL,
    [file_data]        VARBINARY (MAX) NULL,
    [status]           INT             NULL,
    [sent_time]        DATETIME        NULL,
    [as_to]            NVARCHAR (50)   NULL,
    [as2_from]         NVARCHAR (50)   NULL,
    [as2_header]       NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_EMA_SENT_FILE] PRIMARY KEY CLUSTERED ([ema_sent_file_PK] ASC)
);

