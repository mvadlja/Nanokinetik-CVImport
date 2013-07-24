CREATE TABLE [dbo].[SN_ON_MN] (
    [official_name_FK]  INT NOT NULL,
    [substance_name_FK] INT NULL,
    [sn_on_mn_PK]       INT IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_SN_ON_MN] PRIMARY KEY CLUSTERED ([sn_on_mn_PK] ASC),
    CONSTRAINT [FK_SN_ON_MN_OFFICIAL_NAME] FOREIGN KEY ([official_name_FK]) REFERENCES [dbo].[OFFICIAL_NAME] ([official_name_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_SN_ON_MN_SUBSTANCE_NAME] FOREIGN KEY ([substance_name_FK]) REFERENCES [dbo].[SUBSTANCE_NAME] ([substance_name_PK]) ON DELETE CASCADE
);

