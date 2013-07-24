CREATE TABLE [dbo].[APPROVED_SUBST_INTER_CODE_MN] (
    [approved_subst_inter_code_PK] INT IDENTITY (1, 1) NOT NULL,
    [approved_substance_FK]        INT NULL,
    [international_code_FK]        INT NULL,
    CONSTRAINT [PK_APPROVED_SUBST_INTER_CODE_MN] PRIMARY KEY CLUSTERED ([approved_subst_inter_code_PK] ASC),
    CONSTRAINT [FK_APPROVED_SUBST_INTER_CODE_MN_APPROVED_SUBST_1] FOREIGN KEY ([approved_substance_FK]) REFERENCES [dbo].[APPROVED_SUBSTANCE] ([approved_substance_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_APPROVED_SUBST_INTER_CODE_MN_INTER_CODE_2] FOREIGN KEY ([international_code_FK]) REFERENCES [dbo].[INTERNATIONAL_CODE] ([international_code_PK]) ON DELETE CASCADE
);

