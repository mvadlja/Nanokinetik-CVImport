﻿CREATE TABLE [dbo].[APPROVED_SUBST_SUBSTANCESSI_MN] (
    [approved_subst_substancessi_PK] INT IDENTITY (1, 1) NOT NULL,
    [approved_substance_FK]          INT NULL,
    [substancessi_FK]                INT NULL,
    CONSTRAINT [PK_APPROVED_SUBST_SUBSTANCESSI_MN] PRIMARY KEY CLUSTERED ([approved_subst_substancessi_PK] ASC),
    CONSTRAINT [FK_APPROVED_SUBST_SUBSTANCESSI_MN_APPROVED_SUBST_1] FOREIGN KEY ([approved_substance_FK]) REFERENCES [dbo].[APPROVED_SUBSTANCE] ([approved_substance_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_APPROVED_SUBST_SUBSTANCESSI_MN_SUBSTANCESSI_2] FOREIGN KEY ([substancessi_FK]) REFERENCES [dbo].[SUBSTANCESSI] ([substancessis_PK]) ON DELETE CASCADE
);
