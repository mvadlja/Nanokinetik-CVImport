﻿CREATE TABLE [dbo].[RI_GE_MN] (
    [ri_ge_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [ri_FK]       INT NULL,
    [ge_FK]       INT NULL,
    CONSTRAINT [PK_RI_GE_MN] PRIMARY KEY CLUSTERED ([ri_ge_mn_PK] ASC),
    CONSTRAINT [FK_RI_GE_MN_GENE_ELEMENT] FOREIGN KEY ([ge_FK]) REFERENCES [dbo].[GENE_ELEMENT] ([gene_element_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_RI_GE_MN_REFERENCE_INFORMATION] FOREIGN KEY ([ri_FK]) REFERENCES [dbo].[REFERENCE_INFORMATION] ([reference_info_PK]) ON DELETE CASCADE
);
