CREATE TABLE [dbo].[TARGET] (
    [target_PK]            INT            IDENTITY (1, 1) NOT NULL,
    [target_gene_id]       VARCHAR (12)   NULL,
    [target_gene_name]     VARCHAR (4000) NOT NULL,
    [interaction_type]     VARCHAR (250)  NOT NULL,
    [target_organism_type] VARCHAR (250)  NULL,
    [target_type]          VARCHAR (250)  NULL,
    CONSTRAINT [PK_SSI_TARGET] PRIMARY KEY CLUSTERED ([target_PK] ASC)
);

