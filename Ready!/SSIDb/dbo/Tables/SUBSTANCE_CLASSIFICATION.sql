CREATE TABLE [dbo].[SUBSTANCE_CLASSIFICATION] (
    [subst_clf_PK]             INT           IDENTITY (1, 1) NOT NULL,
    [domain]                   VARCHAR (250) NULL,
    [substance_classification] VARCHAR (250) NULL,
    [sclf_type]                VARCHAR (250) NULL,
    [sclf_code]                VARCHAR (250) NULL,
    CONSTRAINT [PK_SSI_SUBSTANCE_CLASSIFICATION] PRIMARY KEY CLUSTERED ([subst_clf_PK] ASC)
);

