CREATE TABLE [dbo].[PROPERTY] (
    [property_PK]    INT            IDENTITY (1, 1) NOT NULL,
    [property_type]  VARCHAR (250)  NOT NULL,
    [property_name]  VARCHAR (250)  NOT NULL,
    [substance_id]   VARCHAR (12)   NULL,
    [substance_name] VARCHAR (4000) NULL,
    [amount_type]    VARCHAR (250)  NULL,
    [amount_FK]      INT            NULL,
    CONSTRAINT [PK_SSI_PROPERTY] PRIMARY KEY CLUSTERED ([property_PK] ASC),
    CONSTRAINT [FK_PROPERTY_AMOUNT] FOREIGN KEY ([amount_FK]) REFERENCES [dbo].[AMOUNT] ([amount_PK])
);

