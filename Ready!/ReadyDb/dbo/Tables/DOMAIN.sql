CREATE TABLE [dbo].[DOMAIN] (
    [domain_PK] INT           IDENTITY (1, 1) NOT NULL,
    [name]      NVARCHAR (50) NULL,
    CONSTRAINT [PK_DOMAIN] PRIMARY KEY CLUSTERED ([domain_PK] ASC)
);

