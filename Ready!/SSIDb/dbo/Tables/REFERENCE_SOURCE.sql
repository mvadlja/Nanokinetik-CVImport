CREATE TABLE [dbo].[REFERENCE_SOURCE] (
    [reference_source_PK] INT            IDENTITY (1, 1) NOT NULL,
    [public_domain]       BIT            NOT NULL,
    [rs_type_FK]          INT            NOT NULL,
    [rs_class_FK]         INT            NOT NULL,
    [rs_id]               VARCHAR (12)   NULL,
    [rs_citation]         VARCHAR (2500) NULL,
    CONSTRAINT [PK_SSI_REFERENCE_SOURCE] PRIMARY KEY CLUSTERED ([reference_source_PK] ASC),
    CONSTRAINT [FK_REFERENCE_SOURCE_SSI_CONTROLED_VOCABULARY] FOREIGN KEY ([reference_source_PK]) REFERENCES [dbo].[SSI_CONTROLED_VOCABULARY] ([ssi__cont_voc_PK]),
    CONSTRAINT [FK_REFERENCE_SOURCE_SSI_CONTROLED_VOCABULARY1] FOREIGN KEY ([rs_type_FK]) REFERENCES [dbo].[SSI_CONTROLED_VOCABULARY] ([ssi__cont_voc_PK])
);

