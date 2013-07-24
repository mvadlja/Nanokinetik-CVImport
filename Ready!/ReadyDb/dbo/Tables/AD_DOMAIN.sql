CREATE TABLE [dbo].[AD_DOMAIN] (
    [ad_domain_PK]             INT            IDENTITY (1, 1) NOT NULL,
    [domain_alias]             NVARCHAR (100) NOT NULL,
    [domain_connection_string] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_AD_DOMAIN] PRIMARY KEY CLUSTERED ([ad_domain_PK] ASC)
);

