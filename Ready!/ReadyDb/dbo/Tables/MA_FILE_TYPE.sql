CREATE TABLE [dbo].[MA_FILE_TYPE] (
    [ma_file_type_PK] INT            IDENTITY (1, 1) NOT NULL,
    [name]            NVARCHAR (100) NULL,
    [enum_name]       NVARCHAR (50)  NULL,
    CONSTRAINT [PK_MA_FILE_TYPES] PRIMARY KEY CLUSTERED ([ma_file_type_PK] ASC)
);

