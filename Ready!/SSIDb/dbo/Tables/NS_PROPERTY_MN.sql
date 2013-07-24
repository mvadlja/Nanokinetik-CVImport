CREATE TABLE [dbo].[NS_PROPERTY_MN] (
    [ns_property_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [ns_FK]             INT NULL,
    [property_FK]       INT NULL,
    CONSTRAINT [PK_NS_PROPERTY_MN] PRIMARY KEY CLUSTERED ([ns_property_mn_PK] ASC),
    CONSTRAINT [FK_NS_PROPERTY_MN_NON_STOICHIOMETRIC] FOREIGN KEY ([ns_FK]) REFERENCES [dbo].[NON_STOICHIOMETRIC] ([non_stoichiometric_PK]),
    CONSTRAINT [FK_NS_PROPERTY_MN_PROPERTY] FOREIGN KEY ([property_FK]) REFERENCES [dbo].[PROPERTY] ([property_PK])
);

