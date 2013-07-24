CREATE TABLE [dbo].[STEREOCHEMISTRY] (
    [stereochemistry_PK] INT           IDENTITY (1, 1) NOT NULL,
    [name]               NVARCHAR (50) NULL,
    CONSTRAINT [PK_STEREOCHEMISTRY] PRIMARY KEY CLUSTERED ([stereochemistry_PK] ASC)
);

