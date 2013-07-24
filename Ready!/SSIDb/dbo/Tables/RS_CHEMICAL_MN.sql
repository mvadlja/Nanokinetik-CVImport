CREATE TABLE [dbo].[RS_CHEMICAL_MN] (
    [rs_chemical_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [rs_FK]             INT NULL,
    [chemical_FK]       INT NULL,
    CONSTRAINT [PK_RS_CHEMICAL_MN] PRIMARY KEY CLUSTERED ([rs_chemical_mn_PK] ASC),
    CONSTRAINT [FK_RS_CHEMICAL_MN_CHEMICAL] FOREIGN KEY ([chemical_FK]) REFERENCES [dbo].[CHEMICAL] ([chemical_PK]),
    CONSTRAINT [FK_RS_CHEMICAL_MN_REFERENCE_SOURCE] FOREIGN KEY ([rs_FK]) REFERENCES [dbo].[REFERENCE_SOURCE] ([reference_source_PK])
);

