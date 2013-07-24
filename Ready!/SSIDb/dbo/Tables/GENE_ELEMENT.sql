CREATE TABLE [dbo].[GENE_ELEMENT] (
    [gene_element_PK] INT           IDENTITY (1, 1) NOT NULL,
    [ge_type]         VARCHAR (250) NOT NULL,
    [ge_id]           VARCHAR (12)  NULL,
    [ge_name]         VARCHAR (250) NOT NULL,
    CONSTRAINT [PK_SSI_GENE_ELEMENT] PRIMARY KEY CLUSTERED ([gene_element_PK] ASC)
);

