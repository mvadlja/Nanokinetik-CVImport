CREATE TABLE [dbo].[SUBSTANCESSI] (
    [substancessis_PK]    INT IDENTITY (1, 1) NOT NULL,
    [valid_according_ssi] BIT NULL,
    [ssi_FK]              INT NULL,
    CONSTRAINT [PK_SUBSTANCESSI] PRIMARY KEY CLUSTERED ([substancessis_PK] ASC)
);

