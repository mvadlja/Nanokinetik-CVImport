CREATE TABLE [dbo].[MA_STATUS] (
    [ma_stutus_PK] INT            IDENTITY (1, 1) NOT NULL,
    [name]         NVARCHAR (100) NULL,
    [enum_name]    NVARCHAR (50)  NULL,
    CONSTRAINT [PK_MA_STATUS] PRIMARY KEY CLUSTERED ([ma_stutus_PK] ASC)
);

