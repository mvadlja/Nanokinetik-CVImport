CREATE TABLE [dbo].[AMOUNT] (
    [amount_PK]       INT           IDENTITY (1, 1) NOT NULL,
    [quantity]        VARCHAR (250) NULL,
    [lownumvalue]     VARCHAR (10)  NULL,
    [lownumunit]      VARCHAR (250) NULL,
    [lownumprefix]    VARCHAR (250) NULL,
    [lowdenomvalue]   VARCHAR (10)  NULL,
    [lowdenomunit]    VARCHAR (250) NULL,
    [lowdenomprefix]  VARCHAR (250) NULL,
    [highnumvalue]    VARCHAR (10)  NULL,
    [highnumunit]     VARCHAR (250) NULL,
    [highnumprefix]   VARCHAR (250) NULL,
    [highdenomvalue]  VARCHAR (10)  NULL,
    [highdenomunit]   VARCHAR (250) NULL,
    [highdenomprefix] VARCHAR (250) NULL,
    [average]         VARCHAR (10)  NULL,
    [prefix]          VARCHAR (250) NULL,
    [unit]            VARCHAR (250) NULL,
    [nonnumericvalue] VARCHAR (250) NULL,
    CONSTRAINT [PK_SSI_AMOUNT] PRIMARY KEY CLUSTERED ([amount_PK] ASC)
);

