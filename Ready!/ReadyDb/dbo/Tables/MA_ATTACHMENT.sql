CREATE TABLE [dbo].[MA_ATTACHMENT] (
    [ma_attachment_PK] INT                        IDENTITY (1, 1) NOT NULL,
    [file_name]        NVARCHAR (200)             NULL,
    [file_path]        NVARCHAR (500)             NULL,
    [last_change]      DATETIME                   NULL,
    [deleted]          BIT                        NULL,
    [file_id]          UNIQUEIDENTIFIER           DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [file_data]        VARBINARY (MAX) FILESTREAM NULL,
    CONSTRAINT [PK_MA_ATTACHMENT] PRIMARY KEY CLUSTERED ([ma_attachment_PK] ASC),
    UNIQUE NONCLUSTERED ([file_id] ASC)
) FILESTREAM_ON [ReadyFileGroup];

