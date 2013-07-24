﻿CREATE TABLE [dbo].[SING] (
    [sing_PK]     INT IDENTITY (1, 1) NOT NULL,
    [chemical_FK] INT NULL,
    CONSTRAINT [PK_SSI_SING] PRIMARY KEY CLUSTERED ([sing_PK] ASC),
    CONSTRAINT [FK_SING_CHEMICAL] FOREIGN KEY ([chemical_FK]) REFERENCES [dbo].[CHEMICAL] ([chemical_PK])
);

