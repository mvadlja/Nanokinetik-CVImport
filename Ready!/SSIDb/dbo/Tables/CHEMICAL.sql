CREATE TABLE [dbo].[CHEMICAL] (
    [chemical_PK]     INT            IDENTITY (1, 1) NOT NULL,
    [stoichiometric]  BIT            NULL,
    [comment]         VARCHAR (4000) NULL,
    [non_stoichio_FK] INT            NULL,
    CONSTRAINT [PK_SSI_CHEMICAL] PRIMARY KEY CLUSTERED ([chemical_PK] ASC),
    CONSTRAINT [FK_CHEMICAL_NON_STOICHIOMETRIC] FOREIGN KEY ([non_stoichio_FK]) REFERENCES [dbo].[NON_STOICHIOMETRIC] ([non_stoichiometric_PK])
);

