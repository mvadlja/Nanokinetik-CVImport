CREATE TABLE [dbo].[NS_MOIETY_MN] (
    [ns_moiety_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [moiety_FK]       INT NULL,
    [ns_FK]           INT NULL,
    CONSTRAINT [PK_NS_MOIETY_MN] PRIMARY KEY CLUSTERED ([ns_moiety_mn_PK] ASC),
    CONSTRAINT [FK_NS_MOIETY_MN_MOIETY] FOREIGN KEY ([moiety_FK]) REFERENCES [dbo].[MOIETY] ([moiety_PK]),
    CONSTRAINT [FK_NS_MOIETY_MN_NON_STOICHIOMETRIC] FOREIGN KEY ([ns_FK]) REFERENCES [dbo].[NON_STOICHIOMETRIC] ([non_stoichiometric_PK])
);

