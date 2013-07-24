CREATE TABLE [dbo].[COUNTRY] (
    [country_PK]     INT           IDENTITY (1, 1) NOT NULL,
    [name]           NVARCHAR (50) NULL,
    [abbreviation]   NVARCHAR (50) NULL,
    [region]         NVARCHAR (50) NULL,
    [code]           NVARCHAR (50) NULL,
    [full_name]      NVARCHAR (50) NULL,
    [custom_sort_ID] INT           NULL,
    CONSTRAINT [PK_COUNTRY] PRIMARY KEY CLUSTERED ([country_PK] ASC)
);

