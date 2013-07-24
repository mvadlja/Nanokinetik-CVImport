CREATE TABLE [dbo].[MA_MA_ENTITY_MN] (
    [ma_ma_entity_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [ma_FK]              INT NULL,
    [ma_entity_FK]       INT NULL,
    [ma_entity_type_FK]  INT NULL,
    CONSTRAINT [PK_MA_MA_ENTITIES_MN] PRIMARY KEY CLUSTERED ([ma_ma_entity_mn_PK] ASC)
);

