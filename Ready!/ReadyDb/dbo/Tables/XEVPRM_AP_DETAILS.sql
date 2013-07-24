CREATE TABLE [dbo].[XEVPRM_AP_DETAILS] (
    [xevprm_ap_details_PK]       INT             IDENTITY (1, 1) NOT NULL,
    [ap_FK]                      INT             NULL,
    [ap_name]                    NVARCHAR (2000) NULL,
    [package_description]        NVARCHAR (2000) NULL,
    [authorisation_country_code] NVARCHAR (50)   NULL,
    [related_product_FK]         INT             NULL,
    [related_product_name]       NVARCHAR (2000) NULL,
    [licence_holder]             NVARCHAR (100)  NULL,
    [authorisation_status]       NVARCHAR (50)   NULL,
    [authorisation_number]       NVARCHAR (100)  NULL,
    [operation_type]             INT             NULL,
    [ev_code]                    NVARCHAR (60)   NULL,
    CONSTRAINT [PK_XEVPRM_AP_DETAILS] PRIMARY KEY CLUSTERED ([xevprm_ap_details_PK] ASC),
    CONSTRAINT [FK_XEVPRM_AP_DETAILS_AUTHORISED_PRODUCT] FOREIGN KEY ([ap_FK]) REFERENCES [dbo].[AUTHORISED_PRODUCT] ([ap_PK]) ON DELETE SET NULL
);

