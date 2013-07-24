CREATE TABLE [dbo].[INTERNATIONAL_CODE] (
    [international_code_PK]    INT           IDENTITY (1, 1) NOT NULL,
    [sourcecode]               NVARCHAR (60) NULL,
    [resolutionmode_sources]   INT           NULL,
    [referencetext]            NTEXT         NULL,
    [resolutionmode_substance] INT           NULL,
    CONSTRAINT [PK_INTERNATIONAL_CODE] PRIMARY KEY CLUSTERED ([international_code_PK] ASC)
);

