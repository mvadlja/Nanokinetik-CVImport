CREATE TABLE [dbo].[ISOTOPE_STRUCTURE_MN] (
    [isotope_structure_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [isotope_FK]              INT NULL,
    [structure_FK]            INT NULL,
    CONSTRAINT [PK_ISOTOPE_STRUCTURE_MN] PRIMARY KEY CLUSTERED ([isotope_structure_mn_PK] ASC),
    CONSTRAINT [FK_ISOTOPE_STRUCTURE_MN_ISOTOPE] FOREIGN KEY ([isotope_FK]) REFERENCES [dbo].[ISOTOPE] ([isotope_PK]),
    CONSTRAINT [FK_ISOTOPE_STRUCTURE_MN_STRUCTURE] FOREIGN KEY ([structure_FK]) REFERENCES [dbo].[STRUCTURE] ([structure_PK])
);

