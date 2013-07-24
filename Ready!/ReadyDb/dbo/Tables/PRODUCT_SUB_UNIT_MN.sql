CREATE TABLE [dbo].[PRODUCT_SUB_UNIT_MN] (
    [product_submission_unit_PK] INT IDENTITY (1, 1) NOT NULL,
    [product_FK]                 INT NULL,
    [submission_unit_FK]         INT NULL,
    CONSTRAINT [PK_PRODUCT_SUB_UNIT_MN] PRIMARY KEY CLUSTERED ([product_submission_unit_PK] ASC),
    CONSTRAINT [FK_PRODUCT_SUB_UNIT_MN_PRODUCT_1] FOREIGN KEY ([product_FK]) REFERENCES [dbo].[PRODUCT] ([product_PK]),
    CONSTRAINT [FK_PRODUCT_SUB_UNIT_MN_SUB_UNIT_2] FOREIGN KEY ([submission_unit_FK]) REFERENCES [dbo].[SUBMISSION_UNIT] ([subbmission_unit_PK]) ON DELETE CASCADE
);

