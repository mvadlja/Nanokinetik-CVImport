CREATE TABLE [dbo].[APPROVED_SUBST_SUBST_ATT_MN] (
    [approved_subst_subst_att_PK] INT IDENTITY (1, 1) NOT NULL,
    [approved_substance_FK]       INT NULL,
    [substance_attachment_FK]     INT NULL,
    CONSTRAINT [PK_APPROVED_SUBST_SUBST_ATT_MN] PRIMARY KEY CLUSTERED ([approved_subst_subst_att_PK] ASC),
    CONSTRAINT [FK_APPROVED_SUBST_SUBST_ATT_MN_APPROVED_SUBST_1] FOREIGN KEY ([approved_substance_FK]) REFERENCES [dbo].[APPROVED_SUBSTANCE] ([approved_substance_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_APPROVED_SUBST_SUBST_ATT_MN_SUBST_ATT_2] FOREIGN KEY ([substance_attachment_FK]) REFERENCES [dbo].[SUBSTANCE_ATTACHMENT] ([substance_attachment_PK]) ON DELETE CASCADE
);

