CREATE TABLE [dbo].[SUBSTANCE_SUBSTANCE_NAME_MN] (
    [substance_substance_name_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [substance_FK]                   INT NOT NULL,
    [substance_name_FK]              INT NULL,
    CONSTRAINT [PK_SUBSTANCE_SUBSTANCE_NAME_MN] PRIMARY KEY CLUSTERED ([substance_substance_name_mn_PK] ASC),
    CONSTRAINT [FK_SUBSTANCE_SUBSTANCE_NAME_MN_SUBSTANCE] FOREIGN KEY ([substance_FK]) REFERENCES [dbo].[SUBSTANCE] ([substance_s_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_SUBSTANCE_SUBSTANCE_NAME_MN_SUBSTANCE_NAME] FOREIGN KEY ([substance_name_FK]) REFERENCES [dbo].[SUBSTANCE_NAME] ([substance_name_PK]) ON DELETE CASCADE
);

