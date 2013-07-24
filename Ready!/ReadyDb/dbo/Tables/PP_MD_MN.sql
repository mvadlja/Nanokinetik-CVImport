CREATE TABLE [dbo].[PP_MD_MN] (
    [pp_md_mn_PK]               INT IDENTITY (1, 1) NOT NULL,
    [pp_medical_device_FK]      INT NULL,
    [pharmaceutical_product_FK] INT NULL,
    CONSTRAINT [PK_PP_MD_MN] PRIMARY KEY CLUSTERED ([pp_md_mn_PK] ASC),
    CONSTRAINT [FK_PP_MD_MN_PHARMACEUTICAL_PRODUCT] FOREIGN KEY ([pharmaceutical_product_FK]) REFERENCES [dbo].[PHARMACEUTICAL_PRODUCT] ([pharmaceutical_product_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_PP_MD_MN_PP_MEDICAL_DEVICE] FOREIGN KEY ([pp_medical_device_FK]) REFERENCES [dbo].[PP_MEDICAL_DEVICE] ([medicaldevice_PK]) ON DELETE CASCADE
);

