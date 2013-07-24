CREATE TABLE [dbo].[NON_STOICHIOMETRIC] (
    [non_stoichiometric_PK] INT IDENTITY (1, 1) NOT NULL,
    [number_moieties]       INT NOT NULL,
    CONSTRAINT [PK_SSI_NON_STOICHIOMETRIC] PRIMARY KEY CLUSTERED ([non_stoichiometric_PK] ASC)
);

