CREATE TABLE [dbo].[GENE] (
    [gene_PK]              INT            IDENTITY (1, 1) NOT NULL,
    [gene_sequence_origin] VARCHAR (4000) NULL,
    [gene_id]              VARCHAR (12)   NULL,
    [gene_name]            VARCHAR (4000) NOT NULL,
    CONSTRAINT [PK_SSI_GENE] PRIMARY KEY CLUSTERED ([gene_PK] ASC)
);

