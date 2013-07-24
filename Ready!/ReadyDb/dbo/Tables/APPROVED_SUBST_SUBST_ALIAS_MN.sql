CREATE TABLE [dbo].[APPROVED_SUBST_SUBST_ALIAS_MN] (
    [approved_substance_subst_alias_PK] INT IDENTITY (1, 1) NOT NULL,
    [approved_substance_FK]             INT NULL,
    [substance_alias_FK]                INT NULL,
    CONSTRAINT [PK_APPROVED_SUBST_SUBST_ALIAS_MN] PRIMARY KEY CLUSTERED ([approved_substance_subst_alias_PK] ASC),
    CONSTRAINT [FK_APPROVED_SUBST_SUBST_ALIAS_MN_APPROVED_SUBST_1] FOREIGN KEY ([approved_substance_FK]) REFERENCES [dbo].[APPROVED_SUBSTANCE] ([approved_substance_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_APPROVED_SUBST_SUBST_ALIAS_MN_SUBST_ALIAS_2] FOREIGN KEY ([substance_alias_FK]) REFERENCES [dbo].[SUBSTANCE_ALIAS] ([substance_alias_PK]) ON DELETE CASCADE
);

