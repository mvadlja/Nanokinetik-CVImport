CREATE TABLE [dbo].[SUBSTANCE_SUBSTANCE_CODE_MN] (
    [substance_substance_code_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [substance_FK]                   INT NOT NULL,
    [substance_code_FK]              INT NULL,
    CONSTRAINT [PK_SUBSTANCE_SUBSTANCE_CODE_MN] PRIMARY KEY CLUSTERED ([substance_substance_code_mn_PK] ASC),
    CONSTRAINT [FK_SUBSTANCE_SUBSTANCE_CODE_MN_SUBSTANCE] FOREIGN KEY ([substance_FK]) REFERENCES [dbo].[SUBSTANCE] ([substance_s_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_SUBSTANCE_SUBSTANCE_CODE_MN_SUBSTANCE_CODE] FOREIGN KEY ([substance_code_FK]) REFERENCES [dbo].[SUBSTANCE_CODE] ([substance_code_PK]) ON DELETE CASCADE
);

