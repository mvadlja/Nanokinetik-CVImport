CREATE TABLE [dbo].[PHARMACEUTICAL_PRODUCT_SAVED_SEARCH] (
    [pharmaceutical_products_PK] INT            IDENTITY (1, 1) NOT NULL,
    [name]                       NVARCHAR (450) NULL,
    [responsible_user_FK]        INT            NULL,
    [description]                NVARCHAR (MAX) NULL,
    [product_FK]                 INT            NULL,
    [Pharmform_FK]               INT            NULL,
    [comments]                   NVARCHAR (MAX) NULL,
    [displayName]                NVARCHAR (100) NULL,
    [user_FK]                    INT            NULL,
    [gridLayout]                 NVARCHAR (MAX) NULL,
    [isPublic]                   BIT            NULL,
    [administrationRoutes]       NVARCHAR (50)  NULL,
    [activeIngridients]          NVARCHAR (50)  NULL,
    [excipients]                 NVARCHAR (50)  NULL,
    [adjuvants]                  NVARCHAR (50)  NULL,
    [medical_devices]            NVARCHAR (50)  NULL,
    [pp_FK]                      INT            NULL,
    CONSTRAINT [PK_PHARMACEUTICAL_PRODUCT_SAVED_SEARCH] PRIMARY KEY CLUSTERED ([pharmaceutical_products_PK] ASC)
);

