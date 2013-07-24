CREATE TABLE [dbo].[MA_ENTITY_TYPE] (
    [ma_entity_type_PK] INT            IDENTITY (1, 1) NOT NULL,
    [name]              NVARCHAR (100) NULL,
    [enum_name]         NVARCHAR (50)  NULL,
    CONSTRAINT [PK_MA_ENTITY_TYPE] PRIMARY KEY CLUSTERED ([ma_entity_type_PK] ASC)
);

