CREATE TABLE [dbo].[MA_FILE] (
    [ma_file_PK]   INT             IDENTITY (1, 1) NOT NULL,
    [file_type_FK] INT             NULL,
    [file_name]    NVARCHAR (200)  NULL,
    [file_path]    NVARCHAR (500)  NULL,
    [file_data]    VARBINARY (MAX) NULL,
    [ready_id_FK]  NVARCHAR (32)   NULL,
    CONSTRAINT [PK_MA_FILE] PRIMARY KEY CLUSTERED ([ma_file_PK] ASC),
    CONSTRAINT [FK_MA_FILE_MA_FILE_TYPES] FOREIGN KEY ([file_type_FK]) REFERENCES [dbo].[MA_FILE_TYPE] ([ma_file_type_PK])
);

