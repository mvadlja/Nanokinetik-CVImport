CREATE TABLE [dbo].[MEDDRA] (
    [meddra_pk]       INT             IDENTITY (1, 1) NOT NULL,
    [version_type_FK] INT             NULL,
    [level_type_FK]   INT             NULL,
    [code]            NVARCHAR (10)   NULL,
    [term]            NVARCHAR (256)  NULL,
    [MeddraFullName]  NVARCHAR (1000) NULL,
    CONSTRAINT [PK_MEDDRA] PRIMARY KEY CLUSTERED ([meddra_pk] ASC)
);



