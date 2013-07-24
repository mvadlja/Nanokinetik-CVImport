CREATE TABLE [dbo].[MOIETY] (
    [moiety_PK]   INT            IDENTITY (1, 1) NOT NULL,
    [moiety_role] VARCHAR (250)  NOT NULL,
    [moiety_name] VARCHAR (4000) NOT NULL,
    [moiety_id]   VARCHAR (12)   NULL,
    [amount_type] VARCHAR (250)  NULL,
    [amount_FK]   INT            NULL,
    CONSTRAINT [PK_SSI_MOIETY] PRIMARY KEY CLUSTERED ([moiety_PK] ASC),
    CONSTRAINT [FK_MOIETY_AMOUNT] FOREIGN KEY ([amount_FK]) REFERENCES [dbo].[AMOUNT] ([amount_PK])
);

