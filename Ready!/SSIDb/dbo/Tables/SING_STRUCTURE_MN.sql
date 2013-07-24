CREATE TABLE [dbo].[SING_STRUCTURE_MN] (
    [sing_structure_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [sing_FK]              INT NULL,
    [structure_FK]         INT NULL,
    CONSTRAINT [PK_SING_STRUCTURE_MN] PRIMARY KEY CLUSTERED ([sing_structure_mn_PK] ASC),
    CONSTRAINT [FK_SING_STRUCTURE_MN_SING] FOREIGN KEY ([sing_FK]) REFERENCES [dbo].[SING] ([sing_PK]),
    CONSTRAINT [FK_SING_STRUCTURE_MN_STRUCTURE] FOREIGN KEY ([structure_FK]) REFERENCES [dbo].[STRUCTURE] ([structure_PK])
);

