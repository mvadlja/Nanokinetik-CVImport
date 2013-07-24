CREATE TABLE [dbo].[SUBSTANCE_RELATIONSHIP] (
    [substance_relationship_PK] INT            IDENTITY (1, 1) NOT NULL,
    [relationship]              VARCHAR (250)  NOT NULL,
    [interaction_type]          VARCHAR (250)  NULL,
    [substance_id]              VARCHAR (12)   NULL,
    [substance_name]            VARCHAR (4000) NOT NULL,
    [amount_type]               VARCHAR (250)  NULL,
    [amount_FK]                 INT            NULL,
    CONSTRAINT [PK_SSI_SUBSTANCE_RELATIONSHIP] PRIMARY KEY CLUSTERED ([substance_relationship_PK] ASC),
    CONSTRAINT [FK_SUBSTANCE_RELATIONSHIP_AMOUNT] FOREIGN KEY ([amount_FK]) REFERENCES [dbo].[AMOUNT] ([amount_PK]) ON DELETE CASCADE
);

